using System;
using UnityEngine;

namespace SomethingDownThere
{
    // Finite session state. Coordinates are meters from the volume's minimum corner.
    public sealed class ExcavationGrid
    {
        private readonly bool[] solid;
        public Vector3Int Size { get; }
        public float CellSize { get; }
        public int RemainingCells { get; private set; }
        public int Revision { get; private set; }

        public ExcavationGrid(Vector3Int size, float cellSize)
        {
            if (size.x < 1 || size.y < 1 || size.z < 1 || size.x > 128 || size.y > 128 || size.z > 128)
                throw new ArgumentOutOfRangeException(nameof(size), "Use 1-128 cells per axis.");
            if (!Finite(cellSize) || cellSize <= 0f) throw new ArgumentOutOfRangeException(nameof(cellSize));
            Size = size;
            CellSize = cellSize;
            solid = new bool[size.x * size.y * size.z];
            for (int i = 0; i < solid.Length; i++) solid[i] = true;
            RemainingCells = solid.Length;
        }

        public bool IsSolid(int x, int y, int z) => x >= 0 && y >= 0 && z >= 0
            && x < Size.x && y < Size.y && z < Size.z && solid[x + Size.x * (y + Size.y * z)];

        public bool IsSolid(Vector3 point) => Finite(point.x) && Finite(point.y) && Finite(point.z)
            && IsSolid(Mathf.FloorToInt(point.x / CellSize), Mathf.FloorToInt(point.y / CellSize),
                Mathf.FloorToInt(point.z / CellSize));

        public bool RemoveSphere(Vector3 center, float radius, out BoundsInt changed)
        {
            changed = default;
            if (!Finite(center.x) || !Finite(center.y) || !Finite(center.z) || !Finite(radius) || radius <= 0f)
                return false;
            // Reject remote brushes before converting coordinates to cell indices.
            Vector3 extent = (Vector3)Size * CellSize;
            if (center.x + radius < 0 || center.y + radius < 0 || center.z + radius < 0
                || center.x - radius > extent.x || center.y - radius > extent.y || center.z - radius > extent.z)
                return false;
            Vector3 low = Vector3.Max(Vector3.zero, center - Vector3.one * radius) / CellSize;
            Vector3 high = Vector3.Min(extent, center + Vector3.one * radius) / CellSize;
            Vector3Int start = Vector3Int.Max(Vector3Int.zero, Vector3Int.FloorToInt(low));
            Vector3Int end = Vector3Int.Min(Size - Vector3Int.one, Vector3Int.FloorToInt(high));
            Vector3Int removedMin = Size, removedMax = -Vector3Int.one;
            float squaredRadius = radius * radius;
            for (int z = start.z; z <= end.z; z++)
            for (int y = start.y; y <= end.y; y++)
            for (int x = start.x; x <= end.x; x++)
            {
                if (!IsSolid(x, y, z)) continue;
                Vector3 cellCenter = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) * CellSize;
                if ((cellCenter - center).sqrMagnitude > squaredRadius) continue;
                solid[x + Size.x * (y + Size.y * z)] = false;
                RemainingCells--;
                var cell = new Vector3Int(x, y, z);
                removedMin = Vector3Int.Min(removedMin, cell);
                removedMax = Vector3Int.Max(removedMax, cell);
            }
            if (removedMax.x < 0) return false;
            changed = new BoundsInt(removedMin, removedMax - removedMin + Vector3Int.one);
            Revision++;
            return true;
        }

        internal static bool Finite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    }
}
