using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SomethingDownThere
{
    // Only exposed faces are emitted, including faces revealed across chunk seams.
    public static class TerrainChunkMesh
    {
        private static readonly Vector3Int[] Neighbors =
        {
            Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down,
            new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1)
        };
        private static readonly Vector3[,] Corners =
        {
            { new Vector3(1,0,0), new Vector3(1,1,0), new Vector3(1,1,1), new Vector3(1,0,1) },
            { new Vector3(0,0,1), new Vector3(0,1,1), new Vector3(0,1,0), new Vector3(0,0,0) },
            { new Vector3(0,1,0), new Vector3(0,1,1), new Vector3(1,1,1), new Vector3(1,1,0) },
            { new Vector3(0,0,1), new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(1,0,1) },
            { new Vector3(0,0,1), new Vector3(1,0,1), new Vector3(1,1,1), new Vector3(0,1,1) },
            { new Vector3(1,0,0), new Vector3(0,0,0), new Vector3(0,1,0), new Vector3(1,1,0) }
        };
        private static readonly Vector2[] UVs = { Vector2.zero, Vector2.up, Vector2.one, Vector2.right };

        public static void Rebuild(Mesh mesh, ExcavationGrid grid, Vector3Int start, int chunkSize)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            Vector3Int end = Vector3Int.Min(grid.Size, start + Vector3Int.one * chunkSize);
            for (int z = start.z; z < end.z; z++)
            for (int y = start.y; y < end.y; y++)
            for (int x = start.x; x < end.x; x++)
            {
                if (!grid.IsSolid(x, y, z)) continue;
                var cell = new Vector3Int(x, y, z);
                for (int face = 0; face < 6; face++)
                {
                    Vector3Int neighbor = cell + Neighbors[face];
                    if (grid.IsSolid(neighbor.x, neighbor.y, neighbor.z)) continue;
                    int first = vertices.Count;
                    for (int corner = 0; corner < 4; corner++)
                    {
                        vertices.Add(((Vector3)cell + Corners[face, corner]) * grid.CellSize);
                        normals.Add(Neighbors[face]);
                        uvs.Add(UVs[corner]);
                    }
                    triangles.Add(first); triangles.Add(first + 1); triangles.Add(first + 2);
                    triangles.Add(first); triangles.Add(first + 2); triangles.Add(first + 3);
                }
            }
            mesh.Clear();
            mesh.indexFormat = IndexFormat.UInt32;
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
        }
    }
}
