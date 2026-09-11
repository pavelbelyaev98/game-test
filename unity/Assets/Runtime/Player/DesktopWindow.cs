namespace SomethingDownThere
{
    // GamePreferences is the sole runtime owner of display changes, including startup.
    public static class DesktopWindow
    {
        public static DisplaySelection RecommendedDisplay(int width, int height) => width >= 960 && height >= 540
            ? new DisplaySelection(width, height, 0) : new DisplaySelection(1280, 720, 0);
    }
}
