using System.Collections.Generic;
using UnityEngine;

namespace SomethingDownThere
{
    // Legacy Text caches glyphs at the canvas scale used by its last mesh build.
    // Fixed-size labels need an explicit refresh when only that scale changes.
    [DisallowMultipleComponent]
    public sealed class HudCanvasScaler : UnityEngine.UI.CanvasScaler
    {
        private Canvas rootCanvas;
        private float renderedScale = -1f;
        private readonly List<UnityEngine.UI.Text> labels = new List<UnityEngine.UI.Text>();

        protected override void Handle()
        {
            base.Handle();
            // CanvasScaler runs in preWillRenderCanvases. Refresh after its scale
            // calculation, before the UI rebuild, including while timeScale is zero.
            RefreshTextForScale();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            RefreshTextForScale(); // The base scaler restores scale 1 when disabled.
        }

        private void RefreshTextForScale()
        {
            if (rootCanvas == null) rootCanvas = GetComponent<Canvas>();
            if (rootCanvas == null || renderedScale == rootCanvas.scaleFactor) return;
            renderedScale = rootCanvas.scaleFactor;
            GetComponentsInChildren(true, labels);
            foreach (var label in labels) label.SetAllDirty();
            labels.Clear();
        }
    }
}
