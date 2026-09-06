using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace JustAFewPeppers
{
    // Composition and pause owner; handling owns the sole runtime harvest model.
    public sealed class YardSession : MonoBehaviour
    {
        public InputActionAsset inputActions;
        public InputSystemUIInputModule uiInput;
        public YardPlayer player;
        public YardTargeting targeting;
        public YardHud hud;
        public Transform safeSpawn;
        public YardHandling handling;
        public YardInput Input { get; private set; }
        public bool IsPaused { get; private set; } = true;
        bool focused;
        int ignoreLookThroughFrame;
        bool jumpArmed;
        bool helpOpen;
        bool returnToPlay;

        void Awake()
        {
            focused = Application.isFocused || Application.isBatchMode;
            Input = new YardInput(inputActions, uiInput);
            if (handling != null) handling.Initialize(this);
            hud.Bind(this);
        }

        void Start()
        {
            player.ResetTo(safeSpawn);
            Pause("Just a few peppers");
        }

        void Update()
        {
            if (focused && Input.PepperComparison != null && Input.PepperComparison.WasPressedThisFrame() && handling.peppers != null)
            {
                var batch = handling.peppers;
                batch.simulation = batch.simulation == PepperSimulation.PhysicalBatch ? PepperSimulation.GroupedRest : PepperSimulation.PhysicalBatch;
                batch.Render();
                hud.Notice("Pepper comparison: " + (batch.simulation == PepperSimulation.PhysicalBatch ? "physical batch" : "grouped resting supply"));
                hud.ShowGuidance();
            }
            if (focused && Input.Help.WasPressedThisFrame())
            {
                if (helpOpen) CloseHelp();
                else
                {
                    bool wasPlaying = !IsPaused;
                    Pause("Paused");
                    returnToPlay = wasPlaying;
                    helpOpen = true;
                    hud.ShowHelp();
                }
                return;
            }
            if (Input.Pause.WasPressedThisFrame())
            {
                if (helpOpen) CloseHelp();
                else if (IsPaused) Resume(); else Pause("Paused");
                return;
            }
            if (IsPaused) return;
            if (!Application.isBatchMode && Cursor.lockState != CursorLockMode.Locked)
            {
                Pause("Cursor released");
                return;
            }
            if (Input.Reset.WasPressedThisFrame())
            {
                ResetToSpawn();
                return;
            }
            if (Input.RestartPrototype.WasPressedThisFrame())
            {
                RestartPrototype();
                return;
            }
            var look = Time.frameCount <= ignoreLookThroughFrame ? Vector2.zero : Input.Look.ReadValue<Vector2>();
            // Space also submits menus. Require release after resume/reset before accepting a fresh jump.
            bool jump = jumpArmed && Input.Jump.WasPressedThisFrame();
            if (jump) jumpArmed = false;
            if (Time.frameCount > ignoreLookThroughFrame && !Input.Jump.IsPressed()) jumpArmed = true;
            bool operating = handling != null && handling.machine != null && handling.machine.ControlsPlayer;
            player.Step(operating ? Vector2.zero : Input.Move.ReadValue<Vector2>(), operating ? Vector2.zero : look, Input.Sprint.IsPressed(), !operating && jump, Time.deltaTime);
            var position = player.transform.position;
            if (position.y < -3 || Mathf.Abs(position.x) > 12 || Mathf.Abs(position.z) > 12) ResetToSpawn();
            targeting.Refresh();
            hud.ShowTarget(targeting.Current);
            if (handling != null) handling.Step(Time.deltaTime);
            hud.ShowGuidance();
        }

        public void Pause(string reason)
        {
            helpOpen = false;
            returnToPlay = false;
            IsPaused = true;
            jumpArmed = false;
            player.ClearJumpRequest();
            if (handling != null) handling.Interrupt();
            Time.timeScale = 0;
            Input.SetPaused(true, focused);
            targeting.Clear();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            hud.ShowPause(true, reason);
        }

        public void Resume()
        {
            if (!focused) return;
            helpOpen = false;
            returnToPlay = false;
            Input.SetPaused(false, focused);
            jumpArmed = false;
            IsPaused = false;
            Time.timeScale = 1;
            // Discard recapture/warp delta, including the next input update.
            ignoreLookThroughFrame = Time.frameCount + 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            hud.ShowPause(false, "");
            hud.ShowGuidance();
        }

        void CloseHelp()
        {
            if (returnToPlay) Resume(); else Pause("Paused");
        }

        public void ResetToSpawn()
        {
            player.ResetTo(safeSpawn);
            if (handling != null) handling.Recover();
            jumpArmed = false;
            targeting.Clear();
            hud.ShowTarget(null);
            ignoreLookThroughFrame = Time.frameCount + 1;
            hud.Notice("Back at the gate");
            hud.ShowGuidance();
        }

        public void RestartPrototype()
        {
            ResetToSpawn();
            if (handling != null) handling.ResetPrototype();
            hud.ShowGuidance();
            hud.Notice("Food test restarted - pepper pile restored, both carriers, station and stored food cleared");
        }

        void OnApplicationFocus(bool hasFocus)
        {
            focused = hasFocus;
            if (!hasFocus && Input != null) Pause("Paused while you were away");
            else if (Input != null) Input.SetPaused(IsPaused, focused);
        }

        void OnApplicationPause(bool paused)
        {
            if (paused && Input != null) Pause("Paused while you were away");
        }

        public void Quit()
        {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        void OnDestroy()
        {
            Input?.Dispose();
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
