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

    public enum PlayerMenu { None, Pause, Inventory, Station }

    [DisallowMultipleComponent, RequireComponent(typeof(CharacterController))]
    public sealed class FpsPlayer : MonoBehaviour
    {
        [SerializeField] private FpsTuning tuning = new FpsTuning();
        [SerializeField] private Camera viewCamera;
        [Tooltip("Include all world blockers, not just interactive objects. Player colliders must be excluded.")]
        [SerializeField] private LayerMask worldMask = Physics.DefaultRaycastLayers;

        private CharacterController motor;
        private FpsInput input;
        private float pitch, verticalSpeed, digCooldown, savedTimeScale;
        private CursorLockMode savedCursorLock;
        private bool savedCursorVisible, ownsPresentation, focused = true;
        private int transitionFrame = -1;
        private float feedbackUntil;
        private bool hasMoved, hasLooked, hasDug, hasFlown, hasInspected;

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
        public event Action MenuChanged;

        public string Hint
        {
            get
            {
                if (!hasMoved || !hasLooked) return "WASD Move   |   Mouse Look";
                if (!hasDug) return "LMB Dig   |   E Interact";
                if (Inventory.Count > 0 && !hasInspected) return "Tab Inventory";
                if (!hasFlown) return "Hold Space: Jetpack - shares digging battery";
                return "Esc Pause / controls";
            }
        }

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
        }

        // Exposed for deterministic simulation checks; device bindings remain in FpsInput.
        public void Tick(FpsInputFrame frame, float deltaTime)
        {
            if (!focused || input == null || transitionFrame == Time.frameCount) return;
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
            if (IsMenuOpen) return;
            if (deltaTime <= 0f || float.IsNaN(deltaTime) || float.IsInfinity(deltaTime)) return;

            ApplyLook(frame.Look);
            Move(frame.Move, frame.JetpackHeld, deltaTime);
            digCooldown = Mathf.Max(0f, digCooldown - deltaTime);
            RefreshTargetPrompt();
            // Interaction wins a simultaneous press so opening a station cannot also dig.
            if (frame.InteractPressed)
            {
                TryInteract();
                return;
            }
            if (frame.DigHeld && digCooldown <= 0f)
            {
                TryDig();
                digCooldown = Mathf.Max(0.01f, tuning.DigInterval);
            }
        }

        private void ApplyLook(Vector2 delta)
        {
            hasLooked |= delta.sqrMagnitude > 0f;
            transform.Rotate(0f, delta.x * tuning.LookSensitivity, 0f, Space.World);
            pitch = Mathf.Clamp(pitch - delta.y * tuning.LookSensitivity, -tuning.PitchLimit, tuning.PitchLimit);
            viewCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void Move(Vector2 direction, bool thrust, float deltaTime)
        {
            direction = Vector2.ClampMagnitude(direction, 1f);
            hasMoved |= direction.sqrMagnitude > 0f;
            if (motor.isGrounded && verticalSpeed < 0f) verticalSpeed = -2f;
            verticalSpeed += tuning.Gravity * deltaTime;
            if (thrust && Battery.TrySpend(Mathf.Max(0f, tuning.JetpackEnergyPerSecond) * deltaTime))
            {
                hasFlown = true;
                verticalSpeed = Mathf.Min(tuning.MaxAscentSpeed, Mathf.Max(0f, verticalSpeed)
                    + tuning.JetpackAcceleration * deltaTime);
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
            if (IsMenuOpen || !TryGetTarget(Mathf.Max(tuning.DigReach, tuning.InteractReach), out var hit)) return;
            var interactable = Contract<IInteractionTarget>(hit.collider);
            if (hit.distance <= tuning.InteractReach && interactable != null)
                TargetPrompt = interactable.GetPrompt(this);
            else if (hit.distance <= tuning.DigReach && Contract<IDigTarget>(hit.collider) is IDigTarget target)
                TargetPrompt = target.DigPrompt;
        }

        public bool TryDig()
        {
            if (IsMenuOpen || !focused || !TryGetTarget(tuning.DigReach, out var hit)) return false;
            var target = Contract<IDigTarget>(hit.collider);
            if (target == null || !target.CanDig)
            {
                ShowFeedback(target?.DigPrompt ?? "Cannot dig here");
                return false;
            }
            float cost = Mathf.Max(0f, tuning.DigEnergy);
            if (!Battery.CanSpend(cost)) { ShowFeedback("Battery empty - return to recharge"); return false; }
            if (!target.TryDig(hit)) return false;
            Battery.TrySpend(cost);
            hasDug = true;
            RefreshTargetPrompt();
            return true;
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
            savedTimeScale = Time.timeScale;
            Menu = menu;
            hasInspected |= menu == PlayerMenu.Inventory;
            Time.timeScale = 0f;
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

        private void OnDisable()
        {
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
