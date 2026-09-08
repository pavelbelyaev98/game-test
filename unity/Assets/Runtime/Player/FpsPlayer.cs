using System;
using UnityEngine;

namespace SomethingDownThere
{
    [Serializable]
    public sealed class FpsTuning
    {
        [Min(0f)] public float WalkSpeed = 4f;
        [Min(0f)] public float LookSensitivity = 0.12f;
        [Range(1f, 89f)] public float PitchLimit = 85f;
        public float Gravity = -20f;
        [Min(0f)] public float JumpHeight = 1.15f;
        [Min(0f)] public float JetpackHoldDelay = 0.22f;
        [Min(0f)] public float JetpackAcceleration = 30f;
        [Min(0f)] public float MaxAscentSpeed = 8f;
        [Min(0.01f)] public float BatteryCapacity = 100f;
        [Min(0f)] public float JetpackEnergyPerSecond = 8f;
        [Min(0f)] public float DigEnergy = 2f;
        [Min(0.01f)] public float DigInterval = 0.35f;
        [Min(0.01f)] public float DigReach = 3f;
        [Min(0.01f)] public float InteractReach = 3f;
        [Min(1)] public int InventorySlots = 10;
    }

    public enum PlayerMenu { None, Pause, Inventory, Station, DeveloperAdmin, ConfirmTerrainReset }

    [DisallowMultipleComponent, RequireComponent(typeof(CharacterController))]
    public sealed class FpsPlayer : MonoBehaviour
    {
        [SerializeField] private FpsTuning tuning = new FpsTuning();
        [SerializeField] private Camera viewCamera;
        [Tooltip("Include all world blockers, not just interactive objects. Player colliders must be excluded.")]
        [SerializeField] private LayerMask worldMask = Physics.DefaultRaycastLayers;
        [SerializeField] private ShovelProfile[] shovelLevels = ShovelProfile.Defaults();
        [UnityEngine.Serialization.FormerlySerializedAs("practiceTerrain")]
        [SerializeField] private TerrainVolume excavationTerrain;
        [SerializeField] private Transform surfaceReturn;
        [SerializeField] private DiscoveryField discoveries;

        private CharacterController motor;
        private FpsInput input;
        private float pitch, verticalSpeed, digCooldown, savedTimeScale, jetpackHoldTime;
        private CursorLockMode savedCursorLock;
        private bool savedCursorVisible, ownsPresentation, focused = true;
        private int transitionFrame = -1;
        private float feedbackUntil;
        private bool primaryConsumedUntilRelease;
        private int adminLevel;
        private bool unlimitedBattery;
        private bool adminXray;
        private bool jetpackReadyInAir;

        public FpsTuning Tuning => tuning;
        public Camera ViewCamera => viewCamera;
        public Battery Battery { get; private set; }
        public SessionInventory Inventory { get; private set; }
        public PlayerMenu Menu { get; private set; }
        public StationTarget Station { get; private set; }
        public bool IsMenuOpen => Menu != PlayerMenu.None;
        public string TargetPrompt { get; private set; } = "";
        public string Feedback { get; private set; } = "";
        public float Pitch => pitch;
        public float VerticalSpeed => verticalSpeed;
        public bool IsJetpackActive { get; private set; }
        public ShovelState Shovel { get; private set; }
        // Unity 6.6 uses managed code variants; DEVELOPMENT_BUILD is deprecated.
        // This engine-owned build flag is true in the Editor/development players.
        public static bool AdminBuild => Debug.isDebugBuild;
        public bool ExcavationAvailable => excavationTerrain != null;
        public bool AdminAvailable => AdminBuild && ExcavationAvailable && surfaceReturn != null;
        public bool HasAdminOverrides => AdminAvailable && (adminLevel > 0 || unlimitedBattery || adminXray);
        public DiscoveryField Discoveries => discoveries;
        public bool AdminXray => AdminAvailable && adminXray && discoveries != null && discoveries.isActiveAndEnabled;
        public bool UnlimitedBattery => AdminAvailable && unlimitedBattery;
        public int EffectiveShovelLevel => AdminAvailable && adminLevel > 0 ? adminLevel : Shovel.Level;
        public ShovelProfile EffectiveShovel => Shovel.GetProfile(EffectiveShovelLevel);
        public const float MaximumDigReach = 4f;
        public float DigReachAtLevel(int level) => Mathf.Min(MaximumDigReach, tuning.DigReach + Shovel.GetProfile(level).ReachBonus);
        public float EffectiveDigReach => DigReachAtLevel(EffectiveShovelLevel);
        public float EffectiveDigInterval => tuning.DigInterval * EffectiveShovel.CadenceMultiplier;
        public float DigPulse { get; private set; }
        public int SuccessfulStrokes { get; private set; }
        public float LastScoopVolume { get; private set; }
        public float ExcavatedVolume => excavationTerrain != null ? excavationTerrain.RemovedVolume : 0;
        public float Depth => excavationTerrain == null ? 0 : Mathf.Max(0, excavationTerrain.SurfaceHeight - transform.position.y);
        public event Action MenuChanged;

        private void Awake()
        {
            motor = GetComponent<CharacterController>();
            if (viewCamera == null) viewCamera = GetComponentInChildren<Camera>(true);
            if (viewCamera == null)
            {
                Debug.LogError("FPS player requires a child camera.", this);
                enabled = false;
                return;
            }
            Battery = new Battery(Mathf.Max(0.01f, tuning.BatteryCapacity));
            Inventory = new SessionInventory(Mathf.Max(1, tuning.InventorySlots));
            Shovel = new ShovelState(shovelLevels);
            pitch = Mathf.DeltaAngle(0f, viewCamera.transform.localEulerAngles.x);
            input = new FpsInput();
        }

        private void OnEnable()
        {
            if (input == null) return;
            input.Enable();
            savedCursorLock = Cursor.lockState;
            savedCursorVisible = Cursor.visible;
            ownsPresentation = true;
            SetGameplayCursor();
        }

        private void Update()
        {
            if (input == null) return;
            Tick(input.Read(), Time.deltaTime);
            if (Time.unscaledTime >= feedbackUntil) Feedback = "";
            DigPulse = Mathf.MoveTowards(DigPulse, 0, Time.deltaTime * 5f);
        }

        // Exposed for deterministic simulation checks; device bindings remain in FpsInput.
        public void Tick(FpsInputFrame frame, float deltaTime)
        {
            if (!focused || input == null || transitionFrame == Time.frameCount) return;
            if (frame.AdminMenuPressed && AdminAvailable
                && (Menu == PlayerMenu.None || Menu == PlayerMenu.Pause || Menu == PlayerMenu.DeveloperAdmin))
            {
                if (Menu == PlayerMenu.DeveloperAdmin) CloseMenu(); else ShowAdminMenu();
                return;
            }
            if (frame.BackPressed)
            {
                if (IsMenuOpen) CloseMenu(); else OpenMenu(PlayerMenu.Pause);
                return;
            }
            if (frame.InventoryPressed)
            {
                if (Menu == PlayerMenu.Inventory) CloseMenu();
                else if (!IsMenuOpen) OpenMenu(PlayerMenu.Inventory);
                return;
            }
            if (IsMenuOpen)
            {
                // Admin actions are available in their panel; other menus remain barriers.
                if (Menu == PlayerMenu.DeveloperAdmin) HandleAdminShortcuts(frame);
                return;
            }
            if (deltaTime <= 0f || float.IsNaN(deltaTime) || float.IsInfinity(deltaTime)) return;
            if (HandleAdminShortcuts(frame)) return;

            ApplyLook(frame.Look);
            Move(frame.Move, frame.JumpPressed, frame.JetpackHeld, deltaTime);
            digCooldown = Mathf.Max(0f, digCooldown - deltaTime);
            RefreshTargetPrompt();
            // Interaction wins a simultaneous press so opening a station cannot also dig.
            if (frame.InteractPressed)
            {
                TryInteract();
                return;
            }
            if (!frame.DigHeld) primaryConsumedUntilRelease = false;
            if (frame.DigPressed)
            {
                TryPrimaryAction();
                return;
            }
            if (frame.DigHeld && !primaryConsumedUntilRelease && digCooldown <= 0f)
            {
                TryDig();
                digCooldown = Mathf.Max(0.01f, EffectiveDigInterval);
            }
        }

        private void ApplyLook(Vector2 delta)
        {
            transform.Rotate(0f, delta.x * tuning.LookSensitivity, 0f, Space.World);
            pitch = Mathf.Clamp(pitch - delta.y * tuning.LookSensitivity, -tuning.PitchLimit, tuning.PitchLimit);
            viewCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private bool HandleAdminShortcuts(FpsInputFrame frame)
        {
            if (!AdminAvailable) return false;
            if (frame.AdminLevel > 0) SelectAdminLevel(frame.AdminLevel);
            if (frame.RefillPressed) RefillAdminBattery();
            if (frame.XrayPressed) ToggleAdminXray();
            if (!frame.ReturnPressed) return false;
            AdminReturnToSurface();
            return true;
        }

        private void Move(Vector2 direction, bool jumpPressed, bool spaceHeld, float deltaTime)
        {
            direction = Vector2.ClampMagnitude(direction, 1f);
            if (motor.isGrounded && verticalSpeed <= 0f) jetpackReadyInAir = false;
            if (motor.isGrounded && verticalSpeed < 0f) verticalSpeed = -2f;
            if (jumpPressed && motor.isGrounded)
                verticalSpeed = Mathf.Sqrt(2f * Mathf.Max(0f, tuning.JumpHeight) * Mathf.Max(0f, -tuning.Gravity));
            verticalSpeed += tuning.Gravity * deltaTime;

            // Charge only for the part of this frame after the hold threshold. A tap
            // is a free jump, including when the battery is exhausted.
            // Once powered flight has begun, another press can immediately arrest
            // a fall. Landing starts a new grounded jump/hold cycle.
            float delay = jetpackReadyInAir ? 0f : Mathf.Max(0f, tuning.JetpackHoldDelay);
            float thrustTime = spaceHeld ? Mathf.Max(0f, deltaTime - Mathf.Max(0f, delay - jetpackHoldTime)) : 0f;
            jetpackHoldTime = spaceHeld ? Mathf.Min(delay, jetpackHoldTime + deltaTime) : 0f;
            float energyRate = Mathf.Max(0f, tuning.JetpackEnergyPerSecond);
            float cost = energyRate * thrustTime;
            if (!UnlimitedBattery && energyRate > 0f && cost > Battery.Charge)
            {
                // Burn the final fraction instead of stranding fuel smaller than
                // this frame's cost, which could restart thrust on a shorter frame.
                cost = Battery.Charge;
                thrustTime = cost / energyRate;
            }
            IsJetpackActive = thrustTime > 0f && SpendEnergy(cost);
            if (IsJetpackActive)
            {
                jetpackReadyInAir = true;
                verticalSpeed = Mathf.Min(tuning.MaxAscentSpeed, Mathf.Max(0f, verticalSpeed)
                    + tuning.JetpackAcceleration * thrustTime);
            }
            var planar = (transform.right * direction.x + transform.forward * direction.y) * tuning.WalkSpeed;
            var collisions = motor.Move((planar + Vector3.up * verticalSpeed) * deltaTime);
            if ((collisions & CollisionFlags.Above) != 0 && verticalSpeed > 0f) verticalSpeed = 0f;
            if ((collisions & CollisionFlags.Below) != 0 && verticalSpeed < 0f) verticalSpeed = -2f;
        }

        public bool TryGetTarget(float reach, out RaycastHit hit)
        {
            return Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward,
                out hit, reach, worldMask, QueryTriggerInteraction.Ignore);
        }

        private static T Contract<T>(Collider collider) where T : class
        {
            foreach (var component in collider.GetComponentsInParent<MonoBehaviour>())
                if (component.isActiveAndEnabled && component is T target) return target;
            return null;
        }

        public void RefreshTargetPrompt()
        {
            TargetPrompt = "";
            if (IsMenuOpen || !TryGetTarget(Mathf.Max(EffectiveDigReach, tuning.InteractReach), out var hit)) return;
            var find = Contract<BuriedFind>(hit.collider);
            var interactable = Contract<IInteractionTarget>(hit.collider);
            if (hit.distance <= tuning.InteractReach && find != null)
                TargetPrompt = find.GetPrompt(this);
            else if (hit.distance <= tuning.InteractReach && interactable != null)
                TargetPrompt = interactable.GetPrompt(this);
            else if (hit.distance <= EffectiveDigReach && Contract<IDigTarget>(hit.collider) is IDigTarget target && !target.CanDig)
                TargetPrompt = target.DigPrompt;
        }

        // One fresh LMB press either collects the visible find or starts a shovel
        // stroke. A pickup consumes the press until release, including full/large finds.
        public bool TryPrimaryAction()
        {
            if (IsMenuOpen || !focused || primaryConsumedUntilRelease) return false;
            if (TryGetTarget(Mathf.Max(EffectiveDigReach, tuning.InteractReach), out var hit)
                && Contract<BuriedFind>(hit.collider) is BuriedFind find)
            {
                primaryConsumedUntilRelease = true;
                bool collected = hit.distance <= tuning.InteractReach && find.TryCollect(this);
                RefreshTargetPrompt();
                return collected;
            }
            if (digCooldown > 0) return false;
            bool dug = TryDig();
            digCooldown = Mathf.Max(0.01f, EffectiveDigInterval);
            return dug;
        }

        public bool TryDig()
        {
            if (IsMenuOpen || !focused || !TryGetTarget(EffectiveDigReach, out var hit)) return false;
            var target = Contract<IDigTarget>(hit.collider);
            if (target == null || !target.CanDig)
            {
                var find = hit.collider.GetComponent<BuriedFind>();
                if (find == null) ShowFeedback(target?.DigPrompt ?? "Cannot dig here");
                return false;
            }
            float cost = Mathf.Max(0f, tuning.DigEnergy);
            if (!UnlimitedBattery && !Battery.CanSpend(cost)) { ShowFeedback("Battery empty - return to recharge"); return false; }
            bool accepted = target is TerrainVolume terrain ? terrain.TryDig(hit, EffectiveShovel.Radius) : target.TryDig(hit);
            if (!accepted) return false;
            SpendEnergy(cost);
            SuccessfulStrokes++;
            LastScoopVolume = target is TerrainVolume volume ? volume.LastRemovedVolume : 0;
            DigPulse = 1;
            RefreshTargetPrompt();
            return true;
        }

        private bool SpendEnergy(float cost) => UnlimitedBattery || Battery.TrySpend(cost);

        public void RestoreAdminOverrides()
        {
            if (!focused || !AdminAvailable) return;
            adminLevel = 0;
            unlimitedBattery = false;
            adminXray = false;
            ShowFeedback("Normal rules restored");
            MenuChanged?.Invoke();
        }

        public void ToggleAdminUnlimitedBattery()
        {
            if (!focused || !AdminAvailable) return;
            unlimitedBattery = !unlimitedBattery;
            ShowFeedback(unlimitedBattery ? "Unlimited battery enabled" : "Normal battery use restored");
            MenuChanged?.Invoke();
        }

        public void ToggleAdminXray()
        {
            if (!focused || !AdminAvailable || discoveries == null
                || (IsMenuOpen && Menu != PlayerMenu.DeveloperAdmin)) return;
            adminXray = !adminXray;
            MenuChanged?.Invoke();
        }

        public bool SelectAdminLevel(int level)
        {
            if (!focused || !AdminAvailable || level < 1 || level > Shovel.LevelCount) return false;
            adminLevel = level;
            ShowFeedback($"Shovel {level}  |  Reach {EffectiveDigReach:F1} m  |  Scoop width {EffectiveShovel.Radius * 2:F2} m");
            MenuChanged?.Invoke();
            return true;
        }

        public void RefillAdminBattery()
        {
            if (!focused || !AdminAvailable) return;
            Battery.Recharge();
            ShowFeedback("Battery refilled");
        }

        public void AdminReturnToSurface()
        {
            if (!focused || !AdminAvailable) return;
            motor.enabled = false;
            transform.SetPositionAndRotation(surfaceReturn.position, surfaceReturn.rotation);
            pitch = 48;
            viewCamera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
            verticalSpeed = 0;
            jetpackReadyInAir = false;
            ResetJetpackHold();
            motor.enabled = true;
            Physics.SyncTransforms();
            input?.SuppressHeldActions();
            ShowFeedback("Returned to the surface. Your excavation is preserved.");
        }

        public void ShowAdminMenu()
        {
            if (!focused || !AdminAvailable || (IsMenuOpen && Menu != PlayerMenu.Pause)) return;
            if (!IsMenuOpen) { OpenMenu(PlayerMenu.DeveloperAdmin); return; }
            Menu = PlayerMenu.DeveloperAdmin;
            MenuChanged?.Invoke();
        }

        public void RequestTerrainReset()
        {
            if (!focused || !AdminAvailable || Menu != PlayerMenu.DeveloperAdmin) return;
            Menu = PlayerMenu.ConfirmTerrainReset;
            MenuChanged?.Invoke();
        }

        public bool ConfirmTerrainReset()
        {
            if (!focused || Menu != PlayerMenu.ConfirmTerrainReset || !AdminAvailable) return false;
            AdminReturnToSurface();
            excavationTerrain.ResetExcavation();
            SuccessfulStrokes = 0;
            LastScoopVolume = DigPulse = 0;
            Battery.Recharge();
            Menu = PlayerMenu.DeveloperAdmin;
            ShowFeedback("Fresh ground ready. Shovel level and inventory preserved.");
            MenuChanged?.Invoke();
            return true;
        }

        public void CancelTerrainReset()
        {
            if (!focused || Menu != PlayerMenu.ConfirmTerrainReset) return;
            Menu = PlayerMenu.DeveloperAdmin;
            MenuChanged?.Invoke();
        }

        public bool TryInteract()
        {
            if (IsMenuOpen || !focused || !TryGetTarget(tuning.InteractReach, out var hit)) return false;
            var target = Contract<IInteractionTarget>(hit.collider);
            if (target == null) return false;
            bool accepted = target.TryInteract(this);
            RefreshTargetPrompt();
            return accepted;
        }

        public void ShowFeedback(string message)
        {
            Feedback = message;
            feedbackUntil = Time.unscaledTime + 2.5f;
        }

        public void OpenStation(StationTarget station)
        {
            if (station == null || IsMenuOpen) return;
            Station = station;
            OpenMenu(PlayerMenu.Station);
        }

        public void OpenMenu(PlayerMenu menu)
        {
            if (menu == PlayerMenu.None || IsMenuOpen) return;
            if ((menu == PlayerMenu.DeveloperAdmin || menu == PlayerMenu.ConfirmTerrainReset) && !AdminAvailable) return;
            savedTimeScale = Time.timeScale;
            Menu = menu;
            Time.timeScale = 0f;
            ResetJetpackHold();
            input?.SuppressHeldActions();
            transitionFrame = Time.frameCount;
            TargetPrompt = "";
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MenuChanged?.Invoke();
        }

        public void CloseMenu()
        {
            if (!IsMenuOpen || !focused) return;
            Menu = PlayerMenu.None;
            Station = null;
            Time.timeScale = savedTimeScale;
            input?.SuppressHeldActions();
            transitionFrame = Time.frameCount;
            SetGameplayCursor();
            MenuChanged?.Invoke();
        }

        public bool ExecuteStationCommand(int index)
        {
            if (!focused || Menu != PlayerMenu.Station || Station == null || !Station.isActiveAndEnabled
                || index < 0 || index >= Station.CommandCount || !Station.CanExecute(index, this)) return false;
            // The world is paused, but the target may have been disabled by another component.
            if (!TryGetTarget(tuning.InteractReach, out var hit)
                || Contract<IInteractionTarget>(hit.collider) != (IInteractionTarget)Station) return false;
            bool result = Station.TryExecute(index, this);
            if (result) MenuChanged?.Invoke();
            return result;
        }

        public void SetApplicationFocus(bool hasFocus)
        {
            focused = hasFocus;
            ResetJetpackHold();
            input?.SuppressHeldActions();
            if (!hasFocus && !IsMenuOpen) OpenMenu(PlayerMenu.Pause);
        }

        private void OnApplicationFocus(bool hasFocus) => SetApplicationFocus(hasFocus);
        private void OnApplicationPause(bool paused) => SetApplicationFocus(!paused);

        private static void SetGameplayCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void ResetJetpackHold()
        {
            jetpackHoldTime = 0f;
            IsJetpackActive = false;
        }

        private void OnDisable()
        {
            jetpackReadyInAir = false;
            ResetJetpackHold();
            input?.Disable();
            if (IsMenuOpen) Time.timeScale = savedTimeScale;
            Menu = PlayerMenu.None;
            Station = null;
            if (ownsPresentation)
            {
                Cursor.lockState = savedCursorLock;
                Cursor.visible = savedCursorVisible;
                ownsPresentation = false;
            }
            MenuChanged?.Invoke();
        }

        private void OnDestroy() => input?.Dispose();
    }
}
