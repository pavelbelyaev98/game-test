using System;
using System.Globalization;
using System.IO;
using System.Security;
using UnityEngine;

namespace SomethingDownThere
{
    public interface ICameraPreferencesStore : IDevicePreferencesStore { }

    // A separate device file: never part of an excavation checkpoint or recovery.
    public sealed class CameraPreferencesFile : DevicePreferencesFile, ICameraPreferencesStore
    {
        public CameraPreferencesFile(string path) : base(path) { }
    }

    public sealed class CameraPreferences
    {
        public const int MinimumFov = 55, MaximumFov = 90, DefaultFov = 75;
        private readonly ICameraPreferencesStore store;
        public int VerticalFov { get; private set; } = DefaultFov;
        public bool SteadyCrosshair { get; private set; } = true;
        public bool HasUnsavedChanges { get; private set; }
        public bool WriteFailed { get; private set; }
        public event Action Changed;

        public CameraPreferences(ICameraPreferencesStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            try { Decode(store.Read()); }
            catch (Exception e) when (StorageFailure(e)) { /* Safe session defaults still allow play. */ }
        }

        public void SetVerticalFov(float value)
        {
            int next = float.IsNaN(value) || float.IsInfinity(value) ? DefaultFov
                : Mathf.RoundToInt(Mathf.Clamp(value, MinimumFov, MaximumFov));
            if (VerticalFov == next) return;
            VerticalFov = next;
            HasUnsavedChanges = true;
            Changed?.Invoke();
        }

        public void SetSteadyCrosshair(bool value)
        {
            if (SteadyCrosshair == value) return;
            SteadyCrosshair = value;
            HasUnsavedChanges = true;
            Changed?.Invoke();
        }

        public void Reset()
        {
            VerticalFov = DefaultFov;
            SteadyCrosshair = true;
            HasUnsavedChanges = true; // Also replaces invalid stored settings on an explicit reset.
            Flush();
            Changed?.Invoke();
        }

        public bool Flush()
        {
            if (!HasUnsavedChanges) return true;
            try
            {
                store.Write("version=1\nverticalFov=" + VerticalFov.ToString(CultureInfo.InvariantCulture)
                    + "\nsteadyCrosshair=" + (SteadyCrosshair ? "true" : "false") + "\n");
                HasUnsavedChanges = WriteFailed = false;
            }
            catch (Exception e) when (StorageFailure(e)) { WriteFailed = true; }
            Changed?.Invoke();
            return !WriteFailed;
        }

        private void Decode(string contents)
        {
            if (string.IsNullOrEmpty(contents) || contents.Length > 4096) return;
            string version = null, fov = null, steady = null;
            foreach (string line in contents.Split('\n'))
            {
                int split = line.IndexOf('=');
                if (split < 0) continue;
                string key = line.Substring(0, split).Trim();
                string value = line.Substring(split + 1).Trim();
                if (key == "version") version = value;
                else if (key == "verticalFov") fov = value;
                else if (key == "steadyCrosshair") steady = value;
            }
            // Unknown formats use defaults, preserved on disk until an explicit edit/reset.
            if (version != "1") return;
            if (double.TryParse(fov, NumberStyles.Float, CultureInfo.InvariantCulture, out double valueFov)
                && !double.IsNaN(valueFov) && !double.IsInfinity(valueFov))
                VerticalFov = Mathf.RoundToInt((float)Math.Max(MinimumFov, Math.Min(MaximumFov, valueFov)));
            if (bool.TryParse(steady, out bool valueSteady)) SteadyCrosshair = valueSteady;
        }

        private static bool StorageFailure(Exception e) => e is IOException || e is UnauthorizedAccessException
            || e is SecurityException;
    }
}
