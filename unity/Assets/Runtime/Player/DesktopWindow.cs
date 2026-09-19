namespace SomethingDownThere
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    // GamePreferences is the sole runtime owner of display changes, including startup.
    public static class DesktopWindow
    {
        public static DisplaySelection RecommendedDisplay(int width, int height) => width >= 960 && height >= 540
            ? new DisplaySelection(width, height, 0) : new DisplaySelection(1280, 720, 0);

        public static Vector2Int[] ResolutionOptions(IEnumerable<Vector2Int> reported, Vector2Int desktop, Vector2Int window)
        {
            // The current desktop mode is not the monitor's maximum supported mode.
            // Restrict only our fallback window sizes; retain every reported output mode.
            var fallback = new[] { new Vector2Int(960, 540), new Vector2Int(1280, 720),
                new Vector2Int(1600, 900), desktop, window };
            var options = reported.Concat(fallback.Where(r => r.x <= desktop.x && r.y <= desktop.y))
                .Where(r => r.x >= 960 && r.y >= 540 && r.x <= 16384 && r.y <= 8640)
                .Distinct().OrderBy(r => r.x).ThenBy(r => r.y).ToArray();
            return options.Length > 0 ? options : new[] { new Vector2Int(1280, 720) };
        }
    }
}
