using System;
using System.Collections.Generic;
using UnityEngine;

namespace SomethingDownThere
{
    // Positive density is soil; zero is the surface. Samples are shared by all chunks.
    public sealed class ExcavationGrid
    {
        private readonly float[] density;
        private readonly int strideY, strideZ;
        private readonly float band;
        private readonly List<int> severedSamples = new List<int>(4096);
        private readonly List<int> supportVisited = new List<int>(4096);
        private readonly List<int> supportPending = new List<int>(4096);
        private byte[] supportState; // 0 unknown, 1 current search, 2 anchored in this stroke.
        private int lowestCarvedY;
        public Vector3Int Size { get; }
        public float CellSize { get; }
        public Vector3 Extent => (Vector3)Size * CellSize;
        public int Revision { get; private set; }
        public float RemovedVolume { get; private set; }
        public float LastRemovedVolume { get; private set; }
        public float LastDetachedVolume { get; private set; }
        public int LastDetachedSamples { get; private set; }
        public int LastSupportVisitedSamples { get; private set; }

        public ExcavationGrid(Vector3Int size, float cellSize)
        {
            if (size.x < 1 || size.y < 1 || size.z < 1 || size.x > 256 || size.y > 256 || size.z > 256)
                throw new ArgumentOutOfRangeException(nameof(size), "Use 1-256 cells per axis.");
            if (!Finite(cellSize) || cellSize <= 0f) throw new ArgumentOutOfRangeException(nameof(cellSize));
            Size = size;
            CellSize = cellSize;
            band = cellSize * 2f;
            strideY = size.x + 1;
            strideZ = strideY * (size.y + 1);
            density = new float[strideZ * (size.z + 1)];
            Reset();
        }

        public void Reset()
        {
            for (int z = 0; z <= Size.z; z++)
            for (int y = 0; y <= Size.y; y++)
            for (int x = 0; x <= Size.x; x++)
                density[x + y * strideY + z * strideZ] = Mathf.Min(band, (Size.y - y) * CellSize);
            Revision = 0;
            RemovedVolume = LastRemovedVolume = LastDetachedVolume = 0;
            LastDetachedSamples = LastSupportVisitedSamples = 0;
            lowestCarvedY = Size.y;
            severedSamples.Clear();
            ClearSupportSearch();
        }

        // Horizontal ghost samples extend the border; mesh vertices are clipped at the
        // permanent walls. There is no removable outer shell hiding those walls.
        public float Sample(int x, int y, int z)
        {
            if (y > Size.y) return -(y - Size.y) * CellSize;
            return density[Mathf.Clamp(x, 0, Size.x) + Mathf.Clamp(y, 0, Size.y) * strideY
                + Mathf.Clamp(z, 0, Size.z) * strideZ];
        }

        public float Sample(Vector3 point)
        {
            Vector3 p = point / CellSize;
            var a = Vector3Int.FloorToInt(p);
            Vector3 t = p - (Vector3)a;
            float bottom = Mathf.Lerp(
                Mathf.Lerp(Sample(a.x, a.y, a.z), Sample(a.x + 1, a.y, a.z), t.x),
                Mathf.Lerp(Sample(a.x, a.y + 1, a.z), Sample(a.x + 1, a.y + 1, a.z), t.x), t.y);
            float top = Mathf.Lerp(
                Mathf.Lerp(Sample(a.x, a.y, a.z + 1), Sample(a.x + 1, a.y, a.z + 1), t.x),
                Mathf.Lerp(Sample(a.x, a.y + 1, a.z + 1), Sample(a.x + 1, a.y + 1, a.z + 1), t.x), t.y);
            return Mathf.Lerp(bottom, top, t.z);
        }

        public Vector3 SurfaceNormal(Vector3 point)
        {
            float h = CellSize * 0.5f;
            var gradient = new Vector3(
                Sample(point + Vector3.right * h) - Sample(point - Vector3.right * h),
                Sample(point + Vector3.up * h) - Sample(point - Vector3.up * h),
                Sample(point + Vector3.forward * h) - Sample(point - Vector3.forward * h));
            return gradient.sqrMagnitude > 1e-12f ? -gradient.normalized : Vector3.up;
        }

        public bool IsSolid(Vector3 point) => Finite(point.x) && Finite(point.y) && Finite(point.z)
            && point.x >= 0 && point.y >= 0 && point.z >= 0
            && point.x < Extent.x && point.y < Extent.y && point.z < Extent.z && Sample(point) > 0;

        public bool RemoveSphere(Vector3 center, float radius, out BoundsInt changed)
            => RemoveBrush(center, radius, Vector3.up, 0, 0, false, out changed);

        public bool RemoveScoop(Vector3 center, float radius, int seed, float variation, out BoundsInt changed)
            => RemoveScoop(center, radius, Vector3.up, seed, variation, out changed);

        public bool RemoveScoop(Vector3 center, float radius, Vector3 normal, int seed, float variation, out BoundsInt changed)
            => RemoveBrush(center, radius, normal, seed, variation, true, out changed);

        private bool RemoveBrush(Vector3 center, float radius, Vector3 normal, int seed, float variation,
            bool shovel, out BoundsInt changed)
        {
            changed = default;
            LastRemovedVolume = LastDetachedVolume = 0;
            LastDetachedSamples = LastSupportVisitedSamples = 0;
            severedSamples.Clear();
            if (!Finite(center.x) || !Finite(center.y) || !Finite(center.z)
                || !Finite(radius) || radius <= 0f || radius > 1000f
                || !Finite(variation) || variation < 0 || variation > 0.15f
                || !Finite(normal.x) || !Finite(normal.y) || !Finite(normal.z)
                || !Finite(normal.sqrMagnitude) || normal.sqrMagnitude < 0.0001f) return false;
            // Covers the bevelled, tapered bite in every orientation, including its
            // outward cap. The density halo must fit too for matching chunk normals.
            float maximumRadius = radius * (shovel ? 1.8f : 1f);
            float influence = maximumRadius + band;
            Vector3 extent = Extent;
            if (center.x + maximumRadius < 0 || center.y + maximumRadius < 0 || center.z + maximumRadius < 0
                || center.x - maximumRadius > extent.x || center.y - maximumRadius > extent.y || center.z - maximumRadius > extent.z)
                return false;
            // A local hash never consumes UnityEngine.Random (discovery owns its own
            // randomness). Low spatial frequencies keep silhouettes and normals smooth.
            uint random = unchecked((uint)seed) ^ 0x9e3779b9u;
            Quaternion rotation = Quaternion.Euler(Next01(ref random) * 360, Next01(ref random) * 360, Next01(ref random) * 360);
            Vector3 axisA = rotation * Vector3.right * (3.8f / radius);
            Vector3 axisB = rotation * Vector3.up * (4.6f / radius);
            Vector3 axisC = rotation * Vector3.forward * (3.1f / radius);
            Vector3 phase = new Vector3(Next01(ref random), Next01(ref random), Next01(ref random)) * (2 * Mathf.PI);
            normal.Normalize();
            Vector3 tangent = Vector3.Cross(normal, Mathf.Abs(normal.y) < 0.95f ? Vector3.up : Vector3.forward).normalized;
            tangent = Quaternion.AngleAxis(Next01(ref random) * 360, normal) * tangent;
            Vector3 bitangent = Vector3.Cross(normal, tangent);
            float width = radius * Mathf.Lerp(1.02f, 1.14f, Next01(ref random));
            float length = radius * Mathf.Lerp(0.84f, 0.96f, Next01(ref random));
            float depth = radius * Mathf.Lerp(0.68f, 0.82f, Next01(ref random));
            float tiltX = Mathf.Lerp(-0.16f, 0.16f, Next01(ref random));
            float tiltZ = Mathf.Lerp(-0.12f, 0.12f, Next01(ref random));
            float amplitude = radius * variation, bevel = radius * 0.24f;
            Vector3Int first = Vector3Int.Max(Vector3Int.zero,
                Vector3Int.FloorToInt((center - Vector3.one * influence) / CellSize));
            Vector3Int last = Vector3Int.Min(Size,
                Vector3Int.CeilToInt((center + Vector3.one * influence) / CellSize));
            Vector3Int changedMin = Size + Vector3Int.one, changedMax = -Vector3Int.one;
            float unitVolume = CellSize * CellSize * CellSize;
            for (int z = first.z; z <= last.z; z++)
            for (int y = first.y; y <= last.y; y++)
            for (int x = first.x; x <= last.x; x++)
            {
                int index = x + y * strideY + z * strideZ;
                float before = density[index];
                if (before <= -band) continue;
                Vector3 delta = new Vector3(x, y, z) * CellSize - center;
                float cut;
                if (shovel)
                {
                    float u = Vector3.Dot(delta, tangent), v = Vector3.Dot(delta, bitangent);
                    float height = Vector3.Dot(delta, normal);
                    float floor = -height - depth + u * tiltX + v * tiltZ;
                    float cap = height - radius * 0.8f;
                    if (Mathf.Max(floor, cap) - amplitude >= before) continue;
                    // A broad, slanted fracture face instead of a spherical bottom.
                    // Taper and a rounded superellipse soften the lip without making
                    // a hemisphere; oblique clipped shoulders break the stamped rim.
                    float taper = 1 - 0.16f * Mathf.Clamp01(-height / radius);
                    float a = Mathf.Abs(u / (width * taper)), b = Mathf.Abs(v / (length * taper));
                    // The superellipse is at least max(a,b). Reject unchanged samples
                    // with that cheap bound before powers/noise, especially in deep pits.
                    if ((Mathf.Max(a, b) - 1) * length - amplitude >= before) continue;
                    float side = (Mathf.Pow(Mathf.Pow(a, 2.8f) + Mathf.Pow(b, 2.8f), 1f / 2.8f) - 1) * length;
                    side = Mathf.Max(side, (u * 0.72f + v * 0.69f - radius * 0.98f) * 0.9f);
                    side = Mathf.Max(side, (-u * 0.86f - v * 0.51f - radius * 0.94f) * 0.9f);
                    float join = Mathf.Max(bevel - Mathf.Abs(side - floor), 0) / bevel;
                    cut = Mathf.Max(side, floor) + join * join * bevel * 0.25f;
                    cut = Mathf.Max(cut, cap);
                    if (cut - amplitude >= before) continue;
                    float ripple = variation == 0 ? 0 : 0.5f * Mathf.Sin(Vector3.Dot(delta, axisA) + phase.x)
                        + 0.3f * Mathf.Sin(Vector3.Dot(delta, axisB) + phase.y)
                        + 0.2f * Mathf.Sin(Vector3.Dot(delta, axisC) + phase.z);
                    cut -= amplitude * ripple;
                }
                else cut = delta.magnitude - radius;
                float after = Mathf.Max(-band, Mathf.Min(before, cut));
                if (before - after < 0.00001f) continue;
                density[index] = after;
                if (before > 0 && after <= 0)
                {
                    severedSamples.Add(index);
                    lowestCarvedY = Mathf.Min(lowestCarvedY, y);
                }
                // Sample quadrature in m3, with half weights at finite-domain boundaries.
                float weight = (x == 0 || x == Size.x ? 0.5f : 1f)
                    * (y == 0 || y == Size.y ? 0.5f : 1f) * (z == 0 || z == Size.z ? 0.5f : 1f);
                LastRemovedVolume += (Mathf.Clamp01(0.5f + before / CellSize)
                    - Mathf.Clamp01(0.5f + after / CellSize)) * unitVolume * weight;
                var sample = new Vector3Int(x, y, z);
                changedMin = Vector3Int.Min(changedMin, sample);
                changedMax = Vector3Int.Max(changedMax, sample);
            }
            if (changedMax.x < 0) return false;
            RemoveDetachedSoil(ref changedMin, ref changedMax);
            changed = new BoundsInt(changedMin, changedMax - changedMin + Vector3Int.one);
            RemovedVolume += LastRemovedVolume;
            Revision++;
            return true;
        }

        private void ClearSupportSearch()
        {
            foreach (int index in supportVisited) supportState[index] = 0;
            supportVisited.Clear();
            supportPending.Clear();
        }

        private void RemoveDetachedSoil(ref Vector3Int changedMin, ref Vector3Int changedMax)
        {
            if (severedSamples.Count == 0) return;
            if (supportState == null) supportState = new byte[density.Length];
            ClearSupportSearch();
            // The initial field is connected. Deleting samples can only detach a
            // component next to a newly cut solid edge; no whole-site scan is needed.
            foreach (int index in severedSamples)
            {
                var p = SampleCoordinates(index);
                if (p.x > 0) CheckSupport(index - 1, ref changedMin, ref changedMax);
                if (p.x < Size.x) CheckSupport(index + 1, ref changedMin, ref changedMax);
                if (p.y > 0) CheckSupport(index - strideY, ref changedMin, ref changedMax);
                if (p.y < Size.y) CheckSupport(index + strideY, ref changedMin, ref changedMax);
                if (p.z > 0) CheckSupport(index - strideZ, ref changedMin, ref changedMax);
                if (p.z < Size.z) CheckSupport(index + strideZ, ref changedMin, ref changedMax);
            }
            LastSupportVisitedSamples = supportVisited.Count;
        }

        private Vector3Int SampleCoordinates(int index)
            => new Vector3Int(index % strideY, index / strideY % (Size.y + 1), index / strideZ);

        private bool VisitSupport(int index)
        {
            if (density[index] <= 0) return false;
            if (supportState[index] == 2) return true;
            if (supportState[index] == 0)
            {
                supportState[index] = 1;
                supportVisited.Add(index);
                supportPending.Add(index);
            }
            return false;
        }

        private void CheckSupport(int seed, ref Vector3Int changedMin, ref Vector3Int changedMax)
        {
            if (density[seed] <= 0 || supportState[seed] != 0) return;
            int first = supportVisited.Count;
            VisitSupport(seed);
            bool anchored = false;
            while (supportPending.Count > 0 && !anchored)
            {
                int last = supportPending.Count - 1;
                int index = supportPending[last];
                supportPending.RemoveAt(last);
                var p = SampleCoordinates(index);
                // Untouched layers below the deepest cut still join the bedrock.
                // The top face is deliberately not an anchor: surface islands vanish.
                anchored = p.y < lowestCarvedY || p.y == 0 || p.x == 0 || p.x == Size.x
                    || p.z == 0 || p.z == Size.z;
                if (anchored) break;
                // Solid sample edges define support. Iterative depth-first traversal
                // prefers downward paths and stops as soon as anchorage is proven.
                anchored = (p.y < Size.y && VisitSupport(index + strideY))
                    || (p.z > 0 && VisitSupport(index - strideZ))
                    || (p.z < Size.z && VisitSupport(index + strideZ))
                    || (p.x > 0 && VisitSupport(index - 1))
                    || (p.x < Size.x && VisitSupport(index + 1))
                    || (p.y > 0 && VisitSupport(index - strideY));
            }
            supportPending.Clear();
            float unitVolume = CellSize * CellSize * CellSize;
            for (int i = first; i < supportVisited.Count; i++)
            {
                int index = supportVisited[i];
                if (anchored) { supportState[index] = 2; continue; }
                var p = SampleCoordinates(index);
                float weight = (p.x == 0 || p.x == Size.x ? 0.5f : 1f)
                    * (p.y == 0 || p.y == Size.y ? 0.5f : 1f) * (p.z == 0 || p.z == Size.z ? 0.5f : 1f);
                float removed = Mathf.Clamp01(0.5f + density[index] / CellSize) * unitVolume * weight;
                density[index] = -band;
                LastRemovedVolume += removed;
                LastDetachedVolume += removed;
                LastDetachedSamples++;
                changedMin = Vector3Int.Min(changedMin, p);
                changedMax = Vector3Int.Max(changedMax, p);
            }
        }

        private static float Next01(ref uint state)
        {
            unchecked { state = state * 1664525u + 1013904223u; }
            return (state >> 8) * (1f / 16777216f);
        }

        internal static bool Finite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    }
}
