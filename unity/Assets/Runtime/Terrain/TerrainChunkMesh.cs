using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SomethingDownThere
{
    // Surface nets: one vertex at the average interpolated edge crossing per cell.
    // Every primal edge owns one quad. A halo gives neighboring chunks identical
    // positions and density-gradient normals, independent of rebuild order.
    public static class TerrainChunkMesh
    {
        public sealed class DensityCache
        {
            private float[] samples;
            internal bool Matches(float[] current, int count)
            {
                if (samples == null || samples.Length != count) return false;
                for (int i = 0; i < count; i++) if (samples[i] != current[i]) return false;
                return true;
            }
            internal void Store(float[] current, int count)
            {
                if (samples == null || samples.Length != count) samples = new float[count];
                Array.Copy(current, samples, count);
            }
        }

        public sealed class Workspace
        {
            internal readonly List<Vector3> Vertices = new List<Vector3>(2048);
            internal readonly List<Vector3> Normals = new List<Vector3>(2048);
            internal readonly List<Vector2> UVs = new List<Vector2>(2048);
            internal readonly List<int> Triangles = new List<int>(8192);
            internal readonly float[] Corners = new float[8];
            internal int[] Indices = Array.Empty<int>();
            internal float[] Samples = Array.Empty<float>();
            internal Vector3Int SampleOrigin;
            internal int SampleStrideY, SampleStrideZ;
        }

        public static bool Rebuild(Mesh mesh, ExcavationGrid grid, Vector3Int start, int chunkSize,
            Workspace workspace = null, DensityCache cache = null, Action beforeWrite = null)
        {
            var w = workspace ?? new Workspace();
            w.Vertices.Clear(); w.Normals.Clear(); w.UVs.Clear(); w.Triangles.Clear();
            Vector3Int end = Vector3Int.Min(grid.Size, start + Vector3Int.one * chunkSize);
            Vector3Int low = start - Vector3Int.one;
            Vector3Int span = end - low + Vector3Int.one;
            int count = span.x * span.y * span.z;
            if (w.Indices.Length < count) w.Indices = new int[count];
            for (int i = 0; i < count; i++) w.Indices[i] = -1;
            // Each shared density point is read once. Include the normal's half-cell
            // interpolation halo so gradients use exactly the same samples as before.
            w.SampleOrigin = low - Vector3Int.one;
            var sampleSpan = span + Vector3Int.one * 3;
            w.SampleStrideY = sampleSpan.x;
            w.SampleStrideZ = sampleSpan.x * sampleSpan.y;
            int samples = w.SampleStrideZ * sampleSpan.z;
            if (w.Samples.Length < samples) w.Samples = new float[samples];
            int sampleIndex = 0;
            for (int z = w.SampleOrigin.z; z <= end.z + 2; z++)
            for (int y = w.SampleOrigin.y; y <= end.y + 2; y++)
            for (int x = w.SampleOrigin.x; x <= end.x + 2; x++)
                w.Samples[sampleIndex++] = grid.Sample(x, y, z);
            // A brush/cleanup bounding box can cross chunks whose entire interpolation
            // halo stayed unchanged. Preserve their mesh and cooked collision data.
            if (cache != null && cache.Matches(w.Samples, samples)) return false;

            for (int z = low.z; z <= end.z; z++)
            for (int y = low.y; y <= end.y; y++)
            for (int x = low.x; x <= end.x; x++)
            {
                int mask = 0;
                int sample = SampleIndex(w, x, y, z);
                for (int c = 0; c < 8; c++)
                {
                    float d = w.Samples[sample + (c & 1) + ((c >> 1) & 1) * w.SampleStrideY + ((c >> 2) & 1) * w.SampleStrideZ];
                    w.Corners[c] = d;
                    if (d > 0) mask |= 1 << c;
                }
                if (mask == 0 || mask == 255) continue;
                Vector3 sum = Vector3.zero;
                int crossings = 0;
                for (int c = 0; c < 8; c++)
                for (int axis = 0; axis < 3; axis++)
                {
                    int bit = 1 << axis;
                    if ((c & bit) != 0) continue;
                    float a = w.Corners[c], b = w.Corners[c | bit];
                    if ((a > 0) == (b > 0)) continue;
                    Vector3 p = new Vector3(c & 1, (c >> 1) & 1, (c >> 2) & 1);
                    p[axis] += a / (a - b);
                    sum += p;
                    crossings++;
                }
                Vector3 vertex = (new Vector3(x, y, z) + sum / crossings) * grid.CellSize;
                vertex = Vector3.Max(Vector3.zero, Vector3.Min(grid.Extent, vertex));
                int index = Index(x, y, z, low, span);
                w.Indices[index] = w.Vertices.Count;
                w.Vertices.Add(vertex);
                w.Normals.Add(SurfaceNormal(w, vertex, grid.CellSize));
                w.UVs.Add(new Vector2(vertex.x, vertex.z));
            }

            for (int z = start.z; z <= end.z; z++)
            for (int y = start.y; y <= end.y; y++)
            for (int x = start.x; x <= end.x; x++)
            {
                // Edges at the volume's far border belong only to the last chunk.
                if ((x == end.x && end.x < grid.Size.x) || (y == end.y && end.y < grid.Size.y)
                    || (z == end.z && end.z < grid.Size.z)) continue;
                int sample = SampleIndex(w, x, y, z);
                float a = w.Samples[sample];
                for (int axis = 0; axis < 3; axis++)
                {
                    var edge = new Vector3Int(x, y, z);
                    if (edge[axis] >= grid.Size[axis]) continue;
                    int step = axis == 0 ? 1 : axis == 1 ? w.SampleStrideY : w.SampleStrideZ;
                    if ((a > 0) == (w.Samples[sample + step] > 0)) continue;
                    int u = (axis + 1) % 3, v = (axis + 2) % 3;
                    var q0 = edge; q0[u]--; q0[v]--;
                    var q1 = edge; q1[v]--;
                    var q2 = edge;
                    var q3 = edge; q3[u]--;
                    int i0 = w.Indices[Index(q0.x, q0.y, q0.z, low, span)];
                    int i1 = w.Indices[Index(q1.x, q1.y, q1.z, low, span)];
                    int i2 = w.Indices[Index(q2.x, q2.y, q2.z, low, span)];
                    int i3 = w.Indices[Index(q3.x, q3.y, q3.z, low, span)];
                    if (i0 < 0 || i1 < 0 || i2 < 0 || i3 < 0) continue;
                    if (a <= 0) { int swap = i1; i1 = i3; i3 = swap; }
                    // Shortest diagonal reduces thin triangles on overlapping scoops.
                    if ((w.Vertices[i0] - w.Vertices[i2]).sqrMagnitude
                        <= (w.Vertices[i1] - w.Vertices[i3]).sqrMagnitude)
                    {
                        Triangle(w, i0, i1, i2); Triangle(w, i0, i2, i3);
                    }
                    else { Triangle(w, i0, i1, i3); Triangle(w, i1, i2, i3); }
                }
            }
            beforeWrite?.Invoke();
            mesh.Clear();
            mesh.indexFormat = w.Vertices.Count > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16;
            mesh.SetVertices(w.Vertices);
            mesh.SetNormals(w.Normals);
            mesh.SetUVs(0, w.UVs);
            mesh.SetTriangles(w.Triangles, 0);
            mesh.RecalculateBounds();
            cache?.Store(w.Samples, samples);
            return true;
        }

        private static int Index(int x, int y, int z, Vector3Int low, Vector3Int span)
            => x - low.x + span.x * (y - low.y + span.y * (z - low.z));

        private static int SampleIndex(Workspace w, int x, int y, int z)
            => x - w.SampleOrigin.x + (y - w.SampleOrigin.y) * w.SampleStrideY + (z - w.SampleOrigin.z) * w.SampleStrideZ;

        private static float Sample(Workspace w, Vector3 point, float cellSize)
        {
            Vector3 p = point / cellSize;
            var a = Vector3Int.FloorToInt(p);
            Vector3 t = p - (Vector3)a;
            int i = SampleIndex(w, a.x, a.y, a.z), y = w.SampleStrideY, z = w.SampleStrideZ;
            var s = w.Samples;
            float bottom = Mathf.Lerp(Mathf.Lerp(s[i], s[i + 1], t.x), Mathf.Lerp(s[i + y], s[i + y + 1], t.x), t.y);
            float top = Mathf.Lerp(Mathf.Lerp(s[i + z], s[i + z + 1], t.x), Mathf.Lerp(s[i + y + z], s[i + y + z + 1], t.x), t.y);
            return Mathf.Lerp(bottom, top, t.z);
        }

        private static Vector3 SurfaceNormal(Workspace w, Vector3 point, float cellSize)
        {
            float h = cellSize * 0.5f;
            var gradient = new Vector3(
                Sample(w, point + Vector3.right * h, cellSize) - Sample(w, point - Vector3.right * h, cellSize),
                Sample(w, point + Vector3.up * h, cellSize) - Sample(w, point - Vector3.up * h, cellSize),
                Sample(w, point + Vector3.forward * h, cellSize) - Sample(w, point - Vector3.forward * h, cellSize));
            return gradient.sqrMagnitude > 1e-12f ? -gradient.normalized : Vector3.up;
        }

        private static void Triangle(Workspace w, int a, int b, int c)
        {
            if (Vector3.Cross(w.Vertices[b] - w.Vertices[a], w.Vertices[c] - w.Vertices[a]).sqrMagnitude < 1e-12f) return;
            w.Triangles.Add(a); w.Triangles.Add(b); w.Triangles.Add(c);
        }
    }
}
