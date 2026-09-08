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
        public sealed class Workspace
        {
            internal readonly List<Vector3> Vertices = new List<Vector3>(2048);
            internal readonly List<Vector3> Normals = new List<Vector3>(2048);
            internal readonly List<Vector2> UVs = new List<Vector2>(2048);
            internal readonly List<int> Triangles = new List<int>(8192);
            internal readonly float[] Corners = new float[8];
            internal int[] Indices = Array.Empty<int>();
        }

        public static void Rebuild(Mesh mesh, ExcavationGrid grid, Vector3Int start, int chunkSize,
            Workspace workspace = null)
        {
            var w = workspace ?? new Workspace();
            w.Vertices.Clear(); w.Normals.Clear(); w.UVs.Clear(); w.Triangles.Clear();
            Vector3Int end = Vector3Int.Min(grid.Size, start + Vector3Int.one * chunkSize);
            Vector3Int low = start - Vector3Int.one;
            Vector3Int span = end - low + Vector3Int.one;
            int count = span.x * span.y * span.z;
            if (w.Indices.Length < count) w.Indices = new int[count];
            for (int i = 0; i < count; i++) w.Indices[i] = -1;

            for (int z = low.z; z <= end.z; z++)
            for (int y = low.y; y <= end.y; y++)
            for (int x = low.x; x <= end.x; x++)
            {
                int mask = 0;
                for (int c = 0; c < 8; c++)
                {
                    float d = grid.Sample(x + (c & 1), y + ((c >> 1) & 1), z + ((c >> 2) & 1));
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
                w.Normals.Add(grid.SurfaceNormal(vertex));
                w.UVs.Add(new Vector2(vertex.x, vertex.z));
            }

            for (int z = start.z; z <= end.z; z++)
            for (int y = start.y; y <= end.y; y++)
            for (int x = start.x; x <= end.x; x++)
            {
                // Edges at the volume's far border belong only to the last chunk.
                if ((x == end.x && end.x < grid.Size.x) || (y == end.y && end.y < grid.Size.y)
                    || (z == end.z && end.z < grid.Size.z)) continue;
                float a = grid.Sample(x, y, z);
                for (int axis = 0; axis < 3; axis++)
                {
                    var edge = new Vector3Int(x, y, z);
                    if (edge[axis] >= grid.Size[axis]) continue;
                    var next = edge; next[axis]++;
                    if ((a > 0) == (grid.Sample(next.x, next.y, next.z) > 0)) continue;
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
            mesh.Clear();
            mesh.indexFormat = w.Vertices.Count > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16;
            mesh.SetVertices(w.Vertices);
            mesh.SetNormals(w.Normals);
            mesh.SetUVs(0, w.UVs);
            mesh.SetTriangles(w.Triangles, 0);
            mesh.RecalculateBounds();
        }

        private static int Index(int x, int y, int z, Vector3Int low, Vector3Int span)
            => x - low.x + span.x * (y - low.y + span.y * (z - low.z));

        private static void Triangle(Workspace w, int a, int b, int c)
        {
            if (Vector3.Cross(w.Vertices[b] - w.Vertices[a], w.Vertices[c] - w.Vertices[a]).sqrMagnitude < 1e-12f) return;
            w.Triangles.Add(a); w.Triangles.Add(b); w.Triangles.Add(c);
        }
    }
}
