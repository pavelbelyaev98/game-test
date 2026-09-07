using UnityEngine;

namespace SomethingDownThere
{
    public static class DesktopWindow
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Configure()
        {
#if UNITY_STANDALONE && !UNITY_EDITOR
            var display = Screen.mainWindowDisplayInfo;
            var size = ChooseInitialSize(display.width, display.height, display.workArea.width, display.workArea.height);
            // Apply once per launch: resizing during a session remains under the player's control.
            // Explicit Windowed also replaces full-screen preferences from older builds.
            Screen.SetResolution(size.x, size.y, FullScreenMode.Windowed);
#endif
        }

        public static Vector2Int ChooseInitialSize(int displayWidth, int displayHeight, int workWidth, int workHeight)
        {
            if (displayWidth <= 0 || displayHeight <= 0) return new Vector2Int(1280, 720);
            if (workWidth <= 0) workWidth = displayWidth;
            if (workHeight <= 0) workHeight = displayHeight;
            // Leave desktop space and room for the title bar, borders and taskbar.
            float width = Mathf.Min(1920f, displayWidth * 0.75f, Mathf.Max(16, workWidth - 64));
            float height = Mathf.Min(1080f, displayHeight * 0.75f, Mathf.Max(9, workHeight - 64));
            int units = Mathf.Max(1, Mathf.FloorToInt(Mathf.Min(width / 16f, height / 9f)));
            return new Vector2Int(units * 16, units * 9);
        }
    }
}
