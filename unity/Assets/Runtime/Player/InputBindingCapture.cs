using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace SomethingDownThere
{
    public enum BindingCaptureState { Idle, ReleaseButtons, Listening, Conflict }

    // Sampling only physical button edges avoids pointer motion, synthetic modifier
    // aliases and the UI's submit key becoming a second action on the capture frame.
    public sealed class InputBindingCapture
    {
        private readonly InputPreferences settings;
        private bool quarantine;
        private uint completedUpdate;
        public BindingCaptureState State { get; private set; }
        public PlayerBinding Binding { get; private set; }
        public string Candidate { get; private set; }
        public int Conflict { get; private set; } = -1;
        public string Notice { get; private set; } = "Select an action, then press a key or mouse button.";
        public bool BlocksInput => quarantine || State == BindingCaptureState.ReleaseButtons || State == BindingCaptureState.Listening;
        public event Action Changed;
        public InputBindingCapture(InputPreferences settings) => this.settings = settings;

        public void Begin(PlayerBinding binding)
        {
            if (BlocksInput || State != BindingCaptureState.Idle) return;
            Binding = binding;
            State = BindingCaptureState.ReleaseButtons;
            Notice = "Release the buttons, then choose a control for " + InputPreferences.Label(binding) + ". Escape cancels.";
            Changed?.Invoke();
        }

        public void Tick()
        {
            if (quarantine && completedUpdate != InputState.updateCount && !AnyHeld()) quarantine = false;
            if (State == BindingCaptureState.ReleaseButtons)
            {
                if (AnyHeld()) return;
                State = BindingCaptureState.Listening;
                Notice = "Press a key or mouse button for " + InputPreferences.Label(Binding) + ". Escape cancels.";
                Changed?.Invoke();
                return;
            }
            if (State != BindingCaptureState.Listening) return;
            var keyboard = Keyboard.current;
            if (keyboard != null)
                foreach (var key in keyboard.allKeys)
                    if (key.wasPressedThisFrame)
                    {
                        if (key.keyCode == Key.Escape) Cancel();
                        else Choose("<Keyboard>/" + key.name);
                        return;
                    }
            var mouse = Mouse.current;
            if (mouse == null) return;
            if (mouse.leftButton.wasPressedThisFrame) Choose("<Mouse>/leftButton");
            else if (mouse.rightButton.wasPressedThisFrame) Choose("<Mouse>/rightButton");
            else if (mouse.middleButton.wasPressedThisFrame) Choose("<Mouse>/middleButton");
            else if (mouse.backButton.wasPressedThisFrame) Choose("<Mouse>/backButton");
            else if (mouse.forwardButton.wasPressedThisFrame) Choose("<Mouse>/forwardButton");
        }

        private void Choose(string path)
        {
            Quarantine();
            if (!settings.CanBind(Binding, path, out int conflict))
            {
                State = BindingCaptureState.Idle;
                Notice = "That control is unavailable. Choose a keyboard key or mouse button.";
            }
            else if (conflict >= 0)
            {
                Candidate = path; Conflict = conflict; State = BindingCaptureState.Conflict;
                Notice = InputPreferences.DisplayPath(path) + " is assigned to " + InputPreferences.Label((PlayerBinding)conflict)
                    + ". Replace assigns that action to " + settings.Display(Binding) + ".";
            }
            else
            {
                settings.Bind(Binding, path);
                State = BindingCaptureState.Idle;
                Notice = InputPreferences.Label(Binding) + " is now " + settings.Display(Binding) + ".";
            }
            Changed?.Invoke();
        }

        public void Replace()
        {
            if (BlocksInput || State != BindingCaptureState.Conflict) return;
            bool applied = settings.Bind(Binding, Candidate, true);
            Finish(applied ? "Bindings updated." : "Bindings changed. Choose the action again.");
        }

        public void Cancel()
        {
            if (State == BindingCaptureState.Idle) return;
            Finish("Binding kept.");
        }

        private void Finish(string notice)
        {
            State = BindingCaptureState.Idle; Candidate = null; Conflict = -1;
            Notice = notice; Quarantine(); Changed?.Invoke();
        }
        private void Quarantine() { quarantine = true; completedUpdate = InputState.updateCount; }

        private static bool AnyHeld()
        {
            if (Keyboard.current != null)
                foreach (var key in Keyboard.current.allKeys) if (key.isPressed) return true;
            var mouse = Mouse.current;
            return mouse != null && (mouse.leftButton.isPressed || mouse.rightButton.isPressed || mouse.middleButton.isPressed
                || mouse.backButton.isPressed || mouse.forwardButton.isPressed);
        }
    }
}
