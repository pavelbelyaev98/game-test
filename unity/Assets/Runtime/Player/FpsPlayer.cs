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
        [Min(0.01f)] public float PickupInterval = 0.2f;
        [Min(1)] public int InventorySlots = 10;
    }

    public enum PlayerMenu { None, Pause, Inventory, Station, DeveloperAdmin, ConfirmTerrainReset, ConfirmRescue, Persistence, CameraComfort }

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
        [SerializeField] private SurfaceRecharge surfaceRecharge;
        [SerializeField] private ReturnWarning returnWarning = new ReturnWarning();
        [SerializeField, Min(0)] private int maximumRescueFee = 10;
        [SerializeField] private int[] shovelUpgradeCosts = StationTrade.DefaultPrices();

        private CharacterController motor;
        private FpsInput input;
        private float pitch, verticalSpeed, digCooldown, savedTimeScale, jetpackHoldTime;
        private CursorLockMode savedCursorLock;
        private bool savedCursorVisible, ownsPresentation, focused = true;
        private int transitionFrame = -1;
        private float feedbackUntil;
        private float pickupRecovery;
        private BuriedFind blockedPickup;
        private int adminLevel;
        private bool unlimitedBattery;
        private bool adminXray;
        private bool jetpackReadyInAir;

        public FpsTuning Tuning => tuning;
        public Camera ViewCamera => viewCamera;
        public CameraPreferences CameraSettings { get; private set; }
        public Battery Battery { get; private set; }
        public SessionInventory Inventory { get; private set; }
        public SessionWallet Wallet { get; private set; }
        public StationTrade Trade { get; private set; }
        public long StationRevision { get; private set; }
        public string StationNotice { get; private set; } = "";
        public RescueController Rescue { get; private set; }
        public bool RescueAvailable => Rescue != null && surfaceReturn != null && excavationTerrain != null;
        public string RescueNotice { get; private set; } = "";
        public PlayerMenu Menu { get; private set; }
        public StationTarget Station { get; private set; }
        public bool IsMenuOpen => Menu != PlayerMenu.None;
        public bool GameplayActive => isActiveAndEnabled && focused && !IsMenuOpen && (Persistence == null || !Persistence.BlocksPlay);
        public WorldSaveController Persistence { get; internal set; }
        public TerrainVolume ExcavationTerrain => excavationTerrain;
        public SurfaceRecharge SurfaceRecharge => surfaceRecharge;
        public ReturnWarning ReturnWarning => returnWarning;
        public Vector3 FeetPosition => motor == null ? transform.position
            : transform.TransformPoint(motor.center - Vector3.up * (motor.height * 0.5f));
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
            Wallet = new SessionWallet();
            Rescue = new RescueController(Inventory, Wallet, Mathf.Max(0, maximumRescueFee));
            Shovel = new ShovelState(shovelLevels);
            Trade = new StationTrade(Inventory, Wallet, Shovel, shovelUpgradeCosts);
            pitch = Mathf.DeltaAngle(0f, viewCamera.transform.localEulerAngles.x);
            input = new FpsInput();
            if (CameraSettings == null)
                ConfigureCameraPreferences(new CameraPreferencesFile(System.IO.Path.Combine(Application.persistentDataPath,
                    Application.isEditor ? "EditorPreferences" : "Preferences", "camera-v1.ini")));
            else ApplyCameraPreferences();
        }

        // Injectable storage keeps integration fixtures independent of the user's device preferences.
        public void ConfigureCameraPreferences(ICameraPreferencesStore store)
        {
            if (CameraSettings != null) CameraSettings.Changed -= ApplyCameraPreferences;
            CameraSettings = new CameraPreferences(store);
            CameraSettings.Changed += ApplyCameraPreferences;
            ApplyCameraPreferences();
        }

        private void ApplyCameraPreferences()
        {
            if (viewCamera != null && viewCamera.fieldOfView != CameraSettings.VerticalFov)
                viewCamera.fieldOfView = CameraSettings.VerticalFov;
        }

        public void Capture(WorldSnapshot snapshot)
        {
            snapshot.InventoryCapacity = Inventory.Capacity;
            snapshot.Inventory = new ItemSnapshot[Inventory.Count];
            for (int i = 0; i < Inventory.Count; i++) snapshot.Inventory[i] = ItemSnapshot.Capture(Inventory.Items[i]);
            snapshot.Credits = Wallet.Balance;
            snapshot.ShovelLevel = Shovel.Level;
            snapshot.BatteryCapacity = Battery.Capacity;
            snapshot.BatteryCharge = Battery.Charge;
            snapshot.PlayerPosition = transform.position;
            snapshot.PlayerRotation = transform.rotation;
            snapshot.Pitch = pitch;
            snapshot.VerticalSpeed = verticalSpeed;
            snapshot.SuccessfulStrokes = SuccessfulStrokes;
        }

        public void Restore(WorldSnapshot snapshot)
        {
            var inventory = new SessionInventory(snapshot.InventoryCapacity);
            foreach (var item in snapshot.Inventory)
                if (!inventory.TryAdd(item.Restore())) throw new System.IO.InvalidDataException("The carried finds could not be restored.");
            var shovel = new ShovelState(shovelLevels);
            for (int level = 2; level <= snapshot.ShovelLevel; level++)
                if (!shovel.TryUpgradeTo(level)) throw new System.IO.InvalidDataException("The owned shovel could not be restored.");
            var battery = new Battery(snapshot.BatteryCapacity);
            battery.RestoreCharge(snapshot.BatteryCharge);
            Inventory = inventory;
            Wallet = new SessionWallet(snapshot.Credits);
            Shovel = shovel;
            Battery = battery;
            Trade = new StationTrade(Inventory, Wallet, Shovel, shovelUpgradeCosts);
            Rescue = new RescueController(Inventory, Wallet, maximumRescueFee);
            adminLevel = 0;
            unlimitedBattery = adminXray = jetpackReadyInAir = false;
            motor.enabled = false;
            transform.SetPositionAndRotation(snapshot.PlayerPosition, snapshot.PlayerRotation);
            pitch = snapshot.Pitch;
            viewCamera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
            verticalSpeed = snapshot.VerticalSpeed;
            SuccessfulStrokes = snapshot.SuccessfulStrokes;
            ResetJetpackHold();
            digCooldown = pickupRecovery = DigPulse = LastScoopVolume = 0;
            blockedPickup = null;
            motor.enabled = true;
            Physics.SyncTransforms();
            input?.SuppressHeldActions();
        }

        public void ShowPersistenceMenu()
        {
            if (!IsMenuOpen) OpenMenu(PlayerMenu.Persistence);
            else { Menu = PlayerMenu.Persistence; MenuChanged?.Invoke(); }
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
            if (Persistence != null && Persistence.BlocksPlay) return;
            if (!focused || input == null || transitionFrame == Time.frameCount) return;
            if (frame.AdminMenuPressed && AdminAvailable
                && (Menu == PlayerMenu.None || Menu == PlayerMenu.Pause || Menu == PlayerMenu.DeveloperAdmin))
            {
                if (Menu == PlayerMenu.DeveloperAdmin) CloseMenu(); else ShowAdminMenu();
                return;
            }
            if (frame.BackPressed)
            {
                if (Menu == PlayerMenu.CameraComfort) BackFromCameraComfort();
                else if (Menu == PlayerMenu.ConfirmRescue) CancelRescue();
                else if (IsMenuOpen) CloseMenu(); else OpenMenu(PlayerMenu.Pause);
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
            pickupRecovery = Mathf.Max(0f, pickupRecovery - deltaTime);
            RefreshTargetPrompt();
            // Interaction wins a simultaneous press so opening a station cannot also dig.
            if (frame.InteractPressed)
            {
                TryInteract();
                return;
            }
            if (!frame.DigHeld) blockedPickup = null;
            if (frame.DigPressed || frame.DigHeld) TryPrimaryAction();
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

        // Held primary input checks pickup even while a shovel stroke cools down.
        // A pickup occupies this action and gives feedback time before continuing.
        public bool TryPrimaryAction()
        {
            if (IsMenuOpen || !focused || pickupRecovery > 0f) return false;
            if (TryGetTarget(Mathf.Max(EffectiveDigReach, tuning.InteractReach), out var hit)
                && Contract<BuriedFind>(hit.collider) is BuriedFind find)
            {
                if (hit.distance > tuning.InteractReach || !find.Collectible) return false;
                if (Inventory.IsFull && blockedPickup == find) return false;
                bool collected = find.TryCollect(this);
                blockedPickup = !collected && Inventory.IsFull ? find : null;
                if (collected)
                {
                    pickupRecovery = Mathf.Max(0.01f, tuning.PickupInterval);
                    digCooldown = Mathf.Max(digCooldown, Mathf.Max(pickupRecovery, EffectiveDigInterval));
                }
                RefreshTargetPrompt();
                return collected;
            }
            blockedPickup = null;
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
            if (!UnlimitedBattery && !Battery.CanSpend(cost)) { ShowFeedback("Not enough charge to dig - return to recharge"); return false; }
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
            ReturnToSurface();
            ShowFeedback("Returned to the surface. Your excavation is preserved.");
        }

        private void ReturnToSurface()
        {
            motor.enabled = false;
            transform.SetPositionAndRotation(surfaceReturn.position, surfaceReturn.rotation);
            pitch = 48;
            viewCamera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
            verticalSpeed = 0;
            jetpackReadyInAir = false;
            ResetJetpackHold();
            digCooldown = pickupRecovery = DigPulse = 0;
            blockedPickup = null;
            motor.enabled = true;
            Physics.SyncTransforms();
            input?.SuppressHeldActions();
        }

        public void RequestRescue()
        {
            if (!focused || !isActiveAndEnabled || !RescueAvailable || Menu != PlayerMenu.Pause) return;
            Rescue.Prepare();
            RescueNotice = "";
            Menu = PlayerMenu.ConfirmRescue;
            input?.SuppressHeldActions();
            MenuChanged?.Invoke();
        }

        public void CancelRescue()
        {
            if (!focused || Menu != PlayerMenu.ConfirmRescue) return;
            Rescue.Cancel();
            RescueNotice = "";
            Menu = PlayerMenu.Pause;
            input?.SuppressHeldActions();
            MenuChanged?.Invoke();
        }

        public bool ConfirmRescue()
        {
            if (!focused || !isActiveAndEnabled || !RescueAvailable || Menu != PlayerMenu.ConfirmRescue) return false;
            // Never charge or discard loot if the authored landing area is unavailable.
            Physics.SyncTransforms();
            Vector3 center = surfaceReturn.position + surfaceReturn.rotation * motor.center;
            float half = Mathf.Max(0, motor.height * 0.5f - motor.radius);
            if (Physics.CheckCapsule(center + Vector3.up * half, center - Vector3.up * half,
                    motor.radius, worldMask, QueryTriggerInteraction.Ignore)
                || !Physics.Raycast(surfaceReturn.position + Vector3.up * 0.2f, Vector3.down,
                    out var ground, 0.6f, worldMask, QueryTriggerInteraction.Ignore) || ground.normal.y < 0.7f)
            {
                RescueNotice = "The surface landing area is blocked. Nothing has been lost or charged.";
                MenuChanged?.Invoke();
                return false;
            }
            if (!Rescue.TryConfirm(out var receipt))
            {
                Rescue.Prepare();
                RescueNotice = "Your carried finds or credits changed. Review the updated cost.";
                MenuChanged?.Invoke();
                return false;
            }
            ReturnToSurface();
            Battery.Recharge();
            CloseMenu();
            int count = receipt.LostItems.Count;
            ShowFeedback($"Rescued  |  {count} {(count == 1 ? "find" : "finds")} lost  |  {receipt.Fee} credits");
            Persistence?.RequestCheckpoint();
            return true;
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
            if (!GameplayActive || station == null || !station.isActiveAndEnabled
                || !TryGetTarget(tuning.InteractReach, out var hit)
                || Contract<IInteractionTarget>(hit.collider) != (IInteractionTarget)station) return;
            Station = station;
            StationNotice = "";
            RefreshStationOffers();
            OpenMenu(PlayerMenu.Station);
        }

        private void RefreshStationOffers()
        {
            StationRevision++;
            if (Station != null) Station.RefreshOffers(this);
        }

        public bool CanUseStation(StationTarget station) => isActiveAndEnabled && focused
            && Menu == PlayerMenu.Station && station != null && Station == station && station.isActiveAndEnabled
            && TryGetTarget(tuning.InteractReach, out var hit)
            && Contract<IInteractionTarget>(hit.collider) == (IInteractionTarget)station;

        public void ShowStationFeedback(string message)
        {
            StationNotice = message;
            ShowFeedback(message);
        }

        public void OpenMenu(PlayerMenu menu)
        {
            if (menu == PlayerMenu.None || menu == PlayerMenu.ConfirmRescue || IsMenuOpen) return;
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
            if (Menu == PlayerMenu.CameraComfort) { BackFromCameraComfort(); return; }
            if (Persistence != null && Persistence.BlocksPlay) return;
            if (!IsMenuOpen || !focused) return;
            Rescue?.Cancel();
            RescueNotice = "";
            Menu = PlayerMenu.None;
            Station = null;
            StationRevision++;
            StationNotice = "";
            Time.timeScale = savedTimeScale;
            input?.SuppressHeldActions();
            transitionFrame = Time.frameCount;
            SetGameplayCursor();
            MenuChanged?.Invoke();
        }

        public void ShowCameraComfort()
        {
            if (Menu != PlayerMenu.Pause || !focused) return;
            Menu = PlayerMenu.CameraComfort;
            input?.SuppressHeldActions();
            transitionFrame = Time.frameCount;
            MenuChanged?.Invoke();
        }

        public void BackFromCameraComfort()
        {
            if (Menu != PlayerMenu.CameraComfort || !focused) return;
            CameraSettings.Flush();
            Menu = PlayerMenu.Pause;
            input?.SuppressHeldActions();
            transitionFrame = Time.frameCount;
            MenuChanged?.Invoke();
        }

        public bool ExecuteStationCommand(int index) => ExecuteStationCommand(index, StationRevision);

        public bool ExecuteStationCommand(int index, long displayedRevision)
        {
            if (displayedRevision != StationRevision || index < 0 || Station == null || index >= Station.CommandCount) return false;
            if (!CanUseStation(Station))
            {
                StationNotice = "Station unavailable. Close and approach it again.";
                MenuChanged?.Invoke();
                return false;
            }
            bool result = Station.CanExecute(index, this) && Station.TryExecute(index, this);
            if (result) Persistence?.RequestCheckpoint();
            if (!result) StationNotice = "Offer changed. Review the current items and price.";
            RefreshStationOffers();
            MenuChanged?.Invoke();
            return result;
        }

        public void SetApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) CameraSettings?.Flush();
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
            Rescue?.Cancel();
            RescueNotice = "";
            jetpackReadyInAir = false;
            ResetJetpackHold();
            input?.Disable();
            if (IsMenuOpen) Time.timeScale = savedTimeScale;
            Menu = PlayerMenu.None;
            Station = null;
            StationRevision++;
            if (ownsPresentation)
            {
                Cursor.lockState = savedCursorLock;
                Cursor.visible = savedCursorVisible;
                ownsPresentation = false;
            }
            MenuChanged?.Invoke();
        }

        private void OnApplicationQuit() => CameraSettings?.Flush();

        private void OnDestroy()
        {
            input?.Dispose();
            if (CameraSettings != null) CameraSettings.Changed -= ApplyCameraPreferences;
        }
    }
}
