using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace SomethingDownThere.Tests
{
    public sealed class WorldSaveTests
    {
        private string directory;
        [SetUp] public void SetUp() => directory = Path.Combine(Path.GetTempPath(), "SDT-save-tests", Guid.NewGuid().ToString("N"));
        [TearDown] public void TearDown() { if (Directory.Exists(directory)) Directory.Delete(directory, true); }

        [Test]
        public void WholeWorldRoundTripPreservesExactDensityItemsAndDeterministicNextCut()
        {
            var grid = new ExcavationGrid(new Vector3Int(32, 24, 32), 0.125f);
            grid.RemoveScoop(new Vector3(2, 2.95f, 2), 0.8f, Vector3.up, 72, 0.12f, out _);
            grid.RemoveScoop(new Vector3(2.3f, 2.7f, 2), 0.65f, Vector3.left, 73, 0.12f, out _);
            var state = Snapshot(1);
            state.Terrain = grid.Capture();
            using var memory = new MemoryStream();
            WorldSaveCodec.Write(memory, state);
            memory.Position = 0;
            var restored = WorldSaveCodec.Read(memory);
            AssertSame(state, restored);
            var loadedGrid = new ExcavationGrid(grid.Size, grid.CellSize);
            loadedGrid.Restore(restored.Terrain);
            var center = new Vector3(2.4f, 2.5f, 2);
            grid.RemoveScoop(center, 0.7f, Vector3.left, 74, 0.12f, out _);
            loadedGrid.RemoveScoop(center, 0.7f, Vector3.left, 74, 0.12f, out _);
            Assert.That(loadedGrid.Capture().Density, Is.EqualTo(grid.Capture().Density));
            Assert.That(loadedGrid.RemovedVolume, Is.EqualTo(grid.RemovedVolume));
        }

        [TestCase(SaveWriteStage.BeforeWrite, 1)]
        [TestCase(SaveWriteStage.TemporaryFlushed, 1)]
        [TestCase(SaveWriteStage.BeforeReplace, 1)]
        [TestCase(SaveWriteStage.Replaced, 2)]
        public void InterruptionAtCommitBoundaryLoadsOneCompleteWorld(SaveWriteStage interruption, int expected)
        {
            bool interrupt = false;
            var first = Snapshot(1);
            var second = Snapshot(2);
            second.Credits = 123;
            second.Inventory = Array.Empty<ItemSnapshot>(); // Sold: identity remains collected.
            second.Terrain.Density[42] = -0.25f;
            second.Terrain.LowestCarvedY = 0;
            second.ShovelLevel = 3;
            using (var store = new WorldSaveStore(directory, stage => { if (interrupt && stage == interruption) throw new IOException("Simulated process interruption"); }))
            {
                store.Load(); store.Commit(first); interrupt = true;
                Assert.Throws<IOException>(() => store.Commit(second));
            }
            using var reopened = new WorldSaveStore(directory);
            var result = reopened.Load();
            AssertSame(expected == 1 ? first : second, result.Snapshot);
            Assert.That(result.Recovered, Is.False);
        }

        [Test]
        public void CorruptPrimaryRecoversBackupAndKeepsDamagedFileWhenContinuing()
        {
            byte[] damaged;
            using (var store = new WorldSaveStore(directory))
            {
                store.Load(); store.Commit(Snapshot(1)); store.Commit(Snapshot(2));
                damaged = File.ReadAllBytes(store.PrimaryPath);
                damaged[damaged.Length - 7] ^= 32;
                File.WriteAllBytes(store.PrimaryPath, damaged);
            }
            using var recovery = new WorldSaveStore(directory);
            var loaded = recovery.Load();
            Assert.That(loaded.Recovered, Is.True);
            Assert.That(loaded.Snapshot.Sequence, Is.EqualTo(1));
            recovery.Commit(Snapshot(3));
            Assert.That(File.ReadAllBytes(Directory.GetFiles(directory, "world.damaged-*.sav").Single()), Is.EqualTo(damaged));
            Assert.That(WorldSaveStore.Read(recovery.BackupPath).Sequence, Is.EqualTo(1));
            Assert.That(WorldSaveStore.Read(recovery.PrimaryPath).Sequence, Is.EqualTo(3));
        }

        [Test]
        public void IncompatiblePrimaryCannotSilentlyFallBackOrBeOverwritten()
        {
            using (var store = new WorldSaveStore(directory))
            {
                store.Load(); store.Commit(Snapshot(1)); store.Commit(Snapshot(2));
                using var stream = new FileStream(store.PrimaryPath, FileMode.Open, FileAccess.Write);
                stream.Position = 8;
                stream.Write(BitConverter.GetBytes(999), 0, 4);
            }
            var before = Directory.GetFiles(directory, "*.sav").Select(File.ReadAllBytes).ToArray();
            using var reader = new WorldSaveStore(directory);
            Assert.Throws<UnsupportedSaveException>(() => reader.Load());
            Assert.Throws<InvalidOperationException>(() => reader.Commit(Snapshot(3)));
            Assert.That(Directory.GetFiles(directory, "*.sav").Select(File.ReadAllBytes).ToArray(), Is.EqualTo(before));
        }

        [Test]
        public void DamagedOnlyFileBlocksFreshWorldAndIncompleteCaptureKeepsPriorCheckpoint()
        {
            using (var store = new WorldSaveStore(directory))
            {
                store.Load(); store.Commit(Snapshot(1));
                var invalid = Snapshot(2);
                invalid.Inventory[0].Value++;
                Assert.Throws<InvalidDataException>(() => store.Commit(invalid));
                Assert.That(WorldSaveStore.Read(store.PrimaryPath).Sequence, Is.EqualTo(1));
                File.WriteAllBytes(store.PrimaryPath, new byte[] { 1, 2, 3 });
            }
            using var reader = new WorldSaveStore(directory);
            Assert.Throws<InvalidDataException>(() => reader.Load());
            Assert.Throws<InvalidOperationException>(() => reader.Commit(Snapshot(2)));
        }

        [Test]
        public void ProfileLockPreventsTwoGameInstancesFromOverwritingEachOther()
        {
            using (var first = new WorldSaveStore(directory))
            using (var second = new WorldSaveStore(directory))
            {
                first.Load(); first.Commit(Snapshot(1));
                Assert.Throws<IOException>(() => second.Load());
            }
            using var reopened = new WorldSaveStore(directory);
            Assert.That(reopened.Load().Snapshot.Sequence, Is.EqualTo(1));
        }

        [Test]
        public void CapturedDensityIsIndependentOfFurtherDiggingAndRejectsInvalidSamples()
        {
            var grid = new ExcavationGrid(new Vector3Int(16, 16, 16), 0.125f);
            var snapshot = grid.Capture();
            var before = (float[])snapshot.Density.Clone();
            grid.RemoveSphere(new Vector3(1, 1.9f, 1), 0.5f, out _);
            Assert.That(snapshot.Density, Is.EqualTo(before));
            Assert.That(grid.Capture().Density, Is.Not.EqualTo(before));
            snapshot.Density[0] = float.NaN;
            Assert.Throws<InvalidDataException>(snapshot.Validate);
        }

        private static WorldSnapshot Snapshot(long sequence)
        {
            var item = new ItemSnapshot { Id = "stable-find-42", Name = "Blue marble", Value = 5 };
            return new WorldSnapshot { Sequence = sequence, UtcTicks = DateTime.UtcNow.Ticks,
                Terrain = new ExcavationGrid(new Vector3Int(8, 8, 8), 0.125f).Capture(), TerrainRotation = Quaternion.identity,
                ExcavationSeed = 2718, DiscoverySeed = 90127,
                Finds = new[] { new FindSnapshot { ContentId = "blue-marble", Item = item, Position = Vector3.one * 0.5f,
                    Rotation = Quaternion.identity, Scale = Vector3.one, Collected = true } },
                Inventory = new[] { new ItemSnapshot { Id = item.Id, Name = item.Name, Value = item.Value } },
                InventoryCapacity = 10, Credits = 17, ShovelLevel = 2, BatteryCapacity = 100, BatteryCharge = 37.25f,
                PlayerPosition = new Vector3(0.1f, 1, 0.4f), PlayerRotation = Quaternion.Euler(0, 76, 0), Pitch = 42, VerticalSpeed = -2 };
        }

        private static void AssertSame(WorldSnapshot expected, WorldSnapshot actual)
        {
            Assert.That(actual.Sequence, Is.EqualTo(expected.Sequence));
            Assert.That(actual.Terrain.Density, Is.EqualTo(expected.Terrain.Density));
            Assert.That(actual.Terrain.Revision, Is.EqualTo(expected.Terrain.Revision));
            Assert.That(actual.Terrain.LowestCarvedY, Is.EqualTo(expected.Terrain.LowestCarvedY));
            Assert.That(actual.Terrain.RemovedVolume, Is.EqualTo(expected.Terrain.RemovedVolume));
            Assert.That(actual.Credits, Is.EqualTo(expected.Credits));
            Assert.That(actual.ShovelLevel, Is.EqualTo(expected.ShovelLevel));
            Assert.That(actual.BatteryCharge, Is.EqualTo(expected.BatteryCharge));
            Assert.That(actual.PlayerPosition, Is.EqualTo(expected.PlayerPosition));
            Assert.That(actual.PlayerRotation, Is.EqualTo(expected.PlayerRotation));
            Assert.That(actual.Inventory.Select(i => (i.Id, i.Name, i.Value)), Is.EqualTo(expected.Inventory.Select(i => (i.Id, i.Name, i.Value))));
            Assert.That(actual.Finds.Select(f => (f.ContentId, f.Item.Id, f.Collected, f.Position, f.Rotation, f.Scale)),
                Is.EqualTo(expected.Finds.Select(f => (f.ContentId, f.Item.Id, f.Collected, f.Position, f.Rotation, f.Scale))));
        }
    }
}
