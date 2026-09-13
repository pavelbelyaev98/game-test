using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace SomethingDownThere.Tests
{
    public sealed class ExcavationModeTests
    {
        private static ExcavationGrid Cut(ExcavationMode mode, float radius)
        {
            var grid = new ExcavationGrid(new Vector3Int(64, 64, 64), .125f);
            Assert.That(grid.RemoveCut(new Vector3(4, 7.96f, 4), radius, Vector3.up, Vector3.right, mode, 19, .12f, out _), Is.True);
            return grid;
        }

        private static Bounds EmptyBounds(ExcavationGrid grid)
        {
            bool first = true; var result = new Bounds();
            for (int x = 0; x <= grid.Size.x; x++)
            for (int y = 0; y < grid.Size.y; y++)
            for (int z = 0; z <= grid.Size.z; z++)
            {
                if (grid.Sample(x, y, z) > 0) continue;
                var point = new Vector3(x, y, z) * grid.CellSize;
                if (first) { result = new Bounds(point, Vector3.zero); first = false; }
                else result.Encapsulate(point);
            }
            Assert.That(first, Is.False); return result;
        }

        [TestCase(.345807f)] [TestCase(.809696f)]
        public void ModesOfferDeepWideAndFineCutsAtBothEndsOfPurchasedProgress(float radius)
        {
            var scoop = Cut(ExcavationMode.Scoop, radius); var bore = Cut(ExcavationMode.Bore, radius);
            var fan = Cut(ExcavationMode.Fan, radius); var shave = Cut(ExcavationMode.Shave, radius);
            var ordinary = EmptyBounds(scoop); var narrow = EmptyBounds(bore); var broad = EmptyBounds(fan);
            Assert.That(narrow.min.y, Is.LessThan(ordinary.min.y - .125f));
            Assert.That(narrow.size.x, Is.LessThan(ordinary.size.x));
            Assert.That(broad.size.x, Is.GreaterThan(ordinary.size.x));
            Assert.That(broad.min.y, Is.GreaterThanOrEqualTo(ordinary.min.y));
            Assert.That(shave.RemovedVolume, Is.LessThan(scoop.RemovedVolume));
            Assert.That(shave.RemovedVolume / ExcavationModes.Energy(ExcavationMode.Shave), Is.GreaterThan(scoop.RemovedVolume * .5f));
            TestContext.WriteLine($"radius={radius}: scoop={scoop.RemovedVolume:F3}, bore={bore.RemovedVolume:F3}, fan={fan.RemovedVolume:F3}, shave={shave.RemovedVolume:F3} m3");
        }

        [TestCase(ExcavationMode.Scoop)] [TestCase(ExcavationMode.Bore)]
        [TestCase(ExcavationMode.Fan)] [TestCase(ExcavationMode.Shave)]
        public void EveryModePreservesSnapshotsAndDeterministicallyReplaysAcrossABoundary(ExcavationMode mode)
        {
            var grid = Cut(mode, .81f); var snapshot = grid.Capture(); var before = snapshot.Density.ToArray();
            var restored = new ExcavationGrid(grid.Size, grid.CellSize); restored.Restore(snapshot);
            for (int i = 0; i < 5; i++)
            {
                var point = new Vector3(.1f, 7.8f - i * .2f, 3.95f);
                Assert.That(grid.RemoveCut(point, .81f, new Vector3(0, 1, 1), Vector3.right, mode, 42 + i, .12f, out var bounds), Is.True);
                Assert.That(bounds.min.x, Is.GreaterThanOrEqualTo(0)); Assert.That(bounds.max.x, Is.LessThanOrEqualTo(65));
                restored.RemoveCut(point, .81f, new Vector3(0, 1, 1), Vector3.right, mode, 42 + i, .12f, out _);
            }
            Assert.That(snapshot.Density.ToArray(), Is.EqualTo(before));
            Assert.That(restored.Capture().Density.ToArray(), Is.EqualTo(grid.Capture().Density.ToArray()));
            Assert.That(grid.Capture().Density.ToArray().All(float.IsFinite), Is.True);
        }

        [Test]
        public void InvalidModeCannotMutateTerrain()
        {
            var grid = Cut(ExcavationMode.Scoop, .6f); int revision = grid.Revision;
            Assert.That(grid.RemoveCut(new Vector3(4, 7, 4), .8f, Vector3.up, Vector3.right, (ExcavationMode)99, 0, 0, out _), Is.False);
            Assert.That(grid.Revision, Is.EqualTo(revision));
        }
    }
}
