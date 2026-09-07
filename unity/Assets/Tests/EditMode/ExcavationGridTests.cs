using NUnit.Framework;
using UnityEngine;

namespace SomethingDownThere.Tests
{
    public sealed class ExcavationGridTests
    {
        [Test]
        public void UntouchedSoilSupportsDownwardDiagonalAndLateralRemovalWithAnOverhang()
        {
            var grid = new ExcavationGrid(new Vector3Int(16, 12, 16), 0.5f);
            Assert.That(grid.RemainingCells, Is.EqualTo(3072));
            Vector3[] cuts =
            {
                new Vector3(2.25f, 5.75f, 2.25f), new Vector3(2.25f, 4.75f, 2.25f),
                new Vector3(3.25f, 3.75f, 2.25f), new Vector3(4.25f, 3.75f, 2.25f)
            };
            foreach (Vector3 cut in cuts)
            {
                Assert.That(grid.RemoveSphere(cut, 1.1f, out BoundsInt changed), Is.True);
                Assert.That(changed.Contains(Vector3Int.FloorToInt(cut / grid.CellSize)), Is.True);
                Assert.That(grid.IsSolid(cut), Is.False);
            }
            Assert.That(grid.IsSolid(new Vector3(4.25f, 5.75f, 2.25f)), Is.True, "Roof over lateral cut stays solid.");
            Assert.That(grid.IsSolid(new Vector3(7.75f, 5.75f, 7.75f)), Is.True, "Unrelated soil is untouched.");
            Assert.That(grid.Revision, Is.EqualTo(4));
        }

        [Test]
        public void InvalidRepeatedAndOutsideBrushesDoNotMutateAndRemovalClipsToFiniteBounds()
        {
            var grid = new ExcavationGrid(new Vector3Int(4, 4, 4), 0.5f);
            Vector3 center = new Vector3(0.25f, 0.25f, 0.25f);
            Assert.That(grid.RemoveSphere(center, 0.4f, out _), Is.True);
            Assert.That(grid.RemoveSphere(center, 0.4f, out _), Is.False);
            foreach (float radius in new[] { -1f, 0f, float.NaN, float.PositiveInfinity })
                Assert.That(grid.RemoveSphere(center, radius, out _), Is.False);
            Assert.That(grid.RemoveSphere(Vector3.one * 1000, 1, out _), Is.False);
            Assert.That(grid.RemoveSphere(new Vector3(float.NaN, 0, 0), 1, out _), Is.False);
            Assert.That(grid.RemainingCells, Is.EqualTo(63));
            Assert.That(grid.Revision, Is.EqualTo(1));
            Assert.That(grid.RemoveSphere(Vector3.zero, 10, out BoundsInt changed), Is.True);
            Assert.That(changed.min, Is.EqualTo(Vector3Int.zero));
            Assert.That(changed.max, Is.EqualTo(grid.Size));
            Assert.That(grid.RemainingCells, Is.Zero);
            Assert.That(grid.RemoveSphere(Vector3.one, 10, out _), Is.False);
            Assert.That(grid.IsSolid(-1, 0, 0), Is.False);
            Assert.That(grid.IsSolid(4, 0, 0), Is.False);
        }

        [Test]
        public void ExcavationRevealsNeighborChunkFacesWithOutwardTriangleWinding()
        {
            var grid = new ExcavationGrid(new Vector3Int(4, 2, 2), 1);
            var mesh = new Mesh();
            try
            {
                TerrainChunkMesh.Rebuild(mesh, grid, Vector3Int.zero, 2);
                Assert.That(HasFace(mesh, new Vector3(2, 0, 0), Vector3.right), Is.False);
                grid.RemoveSphere(new Vector3(2.5f, 0.5f, 0.5f), 0.6f, out _);
                TerrainChunkMesh.Rebuild(mesh, grid, Vector3Int.zero, 2);
                Assert.That(HasFace(mesh, new Vector3(2, 0, 0), Vector3.right), Is.True);
                Vector3[] vertices = mesh.vertices, normals = mesh.normals;
                int[] triangles = mesh.triangles;
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int a = triangles[i], b = triangles[i + 1], c = triangles[i + 2];
                    Assert.That(Vector3.Dot(Vector3.Cross(vertices[b] - vertices[a], vertices[c] - vertices[a]), normals[a]),
                        Is.GreaterThan(0), "Generated faces must render/collide from the air side.");
                }
            }
            finally { Object.DestroyImmediate(mesh); }
        }

        private static bool HasFace(Mesh mesh, Vector3 vertex, Vector3 normal)
        {
            Vector3[] vertices = mesh.vertices, normals = mesh.normals;
            for (int i = 0; i < vertices.Length; i++)
                if (vertices[i] == vertex && normals[i] == normal) return true;
            return false;
        }
    }
}
