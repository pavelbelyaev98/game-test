using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SomethingDownThere
{
    internal sealed class ToolkitInputSettings : IDisposable
    {
        private readonly FpsPlayer player;
        private readonly InputPreferences settings;
        private readonly InputBindingCapture capture;
        private readonly VisualElement root, conflict, error;
        private readonly ScrollView scroll;
        private readonly Label notice, modeHint;
        private readonly Button mode, reset, back, retry, replace, cancel;
        private readonly Button[] bindings = new Button[InputPreferences.BindingCount];
        private BindingCaptureState displayedState;
        public VisualElement First => mode;

        public ToolkitInputSettings(VisualElement root, FpsPlayer player)
        {
            this.root = root; this.player = player;
            settings = player.InputSettings; capture = player.BindingCapture;
            mode = root.Q<Button>("digMode");
            modeHint = root.Q<Label>("digModeHint");
            notice = root.Q<Label>("bindingNotice");
            scroll = root.Q<ScrollView>("bindingScroll");
            scroll.mouseWheelScrollSize = 42;
            conflict = root.Q("bindingConflict");
            error = root.Q("inputSettingsError");
            reset = root.Q<Button>("inputReset"); back = root.Q<Button>("inputBack");
            retry = root.Q<Button>("inputRetry"); replace = root.Q<Button>("bindingReplace"); cancel = root.Q<Button>("bindingCancel");
            mode.clicked += () => settings.SetToggleDig(!settings.ToggleDig);
            reset.clicked += settings.Reset;
            back.clicked += player.BackFromInputSettings;
            retry.clicked += () => settings.Flush();
            replace.clicked += capture.Replace; cancel.clicked += capture.Cancel;
            for (int i = 0; i < bindings.Length; i++)
            {
                var binding = (PlayerBinding)i;
                var row = new VisualElement(); row.AddToClassList("binding-row");
                var label = new Label(InputPreferences.Label(binding)) { pickingMode = PickingMode.Ignore };
                label.AddToClassList("binding-label"); row.Add(label);
                var button = new Button(() => capture.Begin(binding)) { name = "bind" + binding };
                button.AddToClassList("menu-button"); button.AddToClassList("binding-button");
                button.RegisterCallback<FocusInEvent>(_ => scroll.ScrollTo(row));
                row.Add(button); scroll.Add(row); bindings[i] = button;
            }
            settings.Changed += Refresh;
            capture.Changed += Refresh;
            Refresh();
        }

        public void AddNavigation(List<VisualElement> controls)
        {
            if (capture.BlocksInput) return;
            if (capture.State == BindingCaptureState.Conflict) { controls.Add(cancel); controls.Add(replace); return; }
            controls.Add(mode);
            controls.AddRange(bindings);
            controls.Add(reset); controls.Add(back);
            if (settings.WriteFailed) controls.Add(retry);
        }

        public bool Adjust(VisualElement focused, NavigationMoveEvent.Direction direction)
        {
            if (focused != mode || (direction != NavigationMoveEvent.Direction.Left && direction != NavigationMoveEvent.Direction.Right)) return false;
            settings.SetToggleDig(direction == NavigationMoveEvent.Direction.Right);
            return true;
        }

        private void Refresh()
        {
            mode.text = settings.ToggleDig ? "Toggle" : "Hold";
            modeHint.text = settings.ToggleDig ? "Press to start or stop digging. Keep aim on an uncovered find briefly to collect. While lifting, press Dig to throw. Menus stop digging."
                : "Hold to dig. Keep aim on an uncovered find briefly to collect. While lifting, press Dig to throw. Release to stop digging.";
            notice.text = capture.Notice;
            bool idle = capture.State == BindingCaptureState.Idle;
            mode.SetEnabled(idle); reset.SetEnabled(idle);
            for (int i = 0; i < bindings.Length; i++)
            {
                bindings[i].text = settings.Display((PlayerBinding)i);
                bindings[i].SetEnabled(idle);
                bindings[i].EnableInClassList("binding-selected", !idle && i == (int)capture.Binding);
            }
            GameMenuView.Show(conflict, capture.State == BindingCaptureState.Conflict);
            GameMenuView.Show(error, settings.WriteFailed);
            root.EnableInClassList("has-settings-error", settings.WriteFailed);
            if (player.Menu == PlayerMenu.InputSettings && displayedState != capture.State)
            {
                if (capture.State == BindingCaptureState.Conflict) cancel.Focus();
                else if (idle) bindings[(int)capture.Binding].Focus();
            }
            if (!settings.WriteFailed && root.focusController?.focusedElement == retry) back.Focus();
            displayedState = capture.State;
        }

        public void Dispose() { settings.Changed -= Refresh; capture.Changed -= Refresh; }
    }
}
