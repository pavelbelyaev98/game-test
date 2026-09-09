using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SomethingDownThere
{
    internal sealed class ToolkitCameraSettings : IDisposable
    {
        private readonly CameraPreferences settings;
        private readonly VisualElement root, error;
        private readonly Label value;
        private readonly Button steady, reset, back, retry;
        public SliderInt Slider { get; }

        public ToolkitCameraSettings(VisualElement root, FpsPlayer player)
        {
            this.root = root;
            settings = player.CameraSettings;
            Slider = root.Q<SliderInt>("fovSlider");
            value = root.Q<Label>("fovValue");
            steady = root.Q<Button>("steadyCrosshair");
            reset = root.Q<Button>("cameraReset");
            back = root.Q<Button>("cameraBack");
            retry = root.Q<Button>("settingsRetry");
            error = root.Q("settingsError");
            Slider.RegisterValueChangedCallback(e => settings.SetVerticalFov(e.newValue));
            steady.clicked += () => settings.SetSteadyCrosshair(!settings.SteadyCrosshair);
            reset.clicked += settings.Reset;
            back.clicked += player.BackFromCameraComfort;
            retry.clicked += () => settings.Flush();
            settings.Changed += Refresh;
            Refresh();
        }

        public void AddNavigation(List<VisualElement> controls)
        {
            controls.Add(Slider);
            controls.Add(steady);
            controls.Add(reset);
            controls.Add(back);
            if (settings.WriteFailed) controls.Add(retry);
        }

        public bool Adjust(VisualElement focused, NavigationMoveEvent.Direction direction)
        {
            if (direction != NavigationMoveEvent.Direction.Left && direction != NavigationMoveEvent.Direction.Right) return false;
            int delta = direction == NavigationMoveEvent.Direction.Left ? -1 : 1;
            if (focused == Slider || Slider.Contains(focused)) { settings.SetVerticalFov(settings.VerticalFov + delta); return true; }
            if (focused == steady) { settings.SetSteadyCrosshair(delta > 0); return true; }
            return false;
        }

        private void Refresh()
        {
            Slider.SetValueWithoutNotify(settings.VerticalFov);
            value.text = settings.VerticalFov + "°";
            steady.text = settings.SteadyCrosshair ? "On" : "Off";
            if (!settings.WriteFailed && root.focusController?.focusedElement == retry) back.Focus();
            GameMenuView.Show(error, settings.WriteFailed);
            root.EnableInClassList("has-settings-error", settings.WriteFailed);
        }

        public void Dispose() => settings.Changed -= Refresh;
    }
}
