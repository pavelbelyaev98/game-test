using UnityEngine;

namespace SomethingDownThere
{
    public enum ExcavationMode { Scoop, Bore, Fan, Shave }

    public static class ExcavationModes
    {
        public const int Count = 4;
        public static bool Valid(ExcavationMode mode) => (int)mode >= 0 && (int)mode < Count;
        public static string Name(ExcavationMode mode) => mode.ToString();
        public static string Purpose(ExcavationMode mode) => mode switch
        {
            ExcavationMode.Bore => "Narrow and deep",
            ExcavationMode.Fan => "Wide and shallow",
            ExcavationMode.Shave => "Fast, fine layers",
            _ => "All-purpose digging"
        };
        // Width/height on the surface, then depth into the soil, in shovel radii.
        public static Vector3 Shape(ExcavationMode mode) => mode switch
        {
            ExcavationMode.Bore => new Vector3(.67f, .67f, 2.25f),
            ExcavationMode.Fan => new Vector3(1.85f, .65f, .40f),
            ExcavationMode.Shave => new Vector3(1.10f, .82f, .22f),
            _ => Vector3.one
        };
        public static float Cadence(ExcavationMode mode) => mode == ExcavationMode.Shave ? .32f : mode == ExcavationMode.Bore ? 1.1f : 1f;
        public static float Energy(ExcavationMode mode) => mode == ExcavationMode.Shave ? .3f : mode == ExcavationMode.Bore ? 1.2f : 1f;
    }
}
