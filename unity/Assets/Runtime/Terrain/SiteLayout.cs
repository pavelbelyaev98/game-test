using UnityEngine;

namespace SomethingDownThere
{
    // The shipped MainGame site in one place. The scene owns the serialized copy;
    // editor tooling, bounds checks and tests read these numbers from here so the
    // reservoir depth is a single authored value. Deepening appends soil below the
    // surface, which is why the origin moves down with the depth.
    public static class SiteLayout
    {
        public const float CellSize = 0.125f;
        public const int DepthCells = 800;
        public const int WidthCells = 192;
        public const int ChunkSize = 16;
        public static readonly Vector3Int Size = new Vector3Int(WidthCells, DepthCells, WidthCells);
        public static readonly Vector3 Origin = new Vector3(-12f, -DepthCells * CellSize, -12f);
        public static readonly Vector3 Extent = new Vector3(WidthCells * CellSize, DepthCells * CellSize, WidthCells * CellSize);
    }
}
