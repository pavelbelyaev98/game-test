#if UNITY_EDITOR
using System;
using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SomethingDownThere.Tests
{
    public sealed class SaveIntegrationTests
    {
        private Scene scene;
        private FpsPlayer player;
        private TerrainVolume terrain;
        private DiscoveryField discoveries;
        private WorldSaveController save;
        private string directory;
        private InputTestFixture devices;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            directory = Path.Combine(Path.GetTempPath(), "SDT-save-integration", Guid.NewGuid().ToString("N"));
            devices = new InputTestFixture(); devices.Setup();
            InputSystem.AddDevice<Keyboard>(); InputSystem.AddDevice<Mouse>();
            Time.timeScale = 1;
            yield return Open();
        }

        private IEnumerator Open()
        {
            // The fixture deliberately disables its player; unloading it cannot run
            // OnDisable again to release a Pause opened during save inspection.
            Time.timeScale = 1f;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/MainGame.unity", new LoadSceneParameters(LoadSceneMode.Additive));
            scene = SceneManager.GetSceneByPath("Assets/Scenes/MainGame.unity");
            player = scene.GetRootGameObjects()[0].GetComponentInChildren<FpsPlayer>();
            player.enabled = false;
            player.SetApplicationFocus(true);
            terrain = player.ExcavationTerrain;
            discoveries = player.Discoveries;
            save = player.GetComponent<WorldSaveController>();
            Assert.That(save, Is.Not.Null, "MainGame owns its save integration.");
            save.BeginSession(directory);
            Assert.That(save.BlocksPlay, Is.True);
            Assert.That(player.GameplayActive, Is.False);
            yield return Until(() => save.State != WorldSaveState.Loading);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (save != null && save.State == WorldSaveState.Saving) yield return Until(() => save.State != WorldSaveState.Saving);
            if (scene.IsValid()) yield return SceneManager.UnloadSceneAsync(scene);
            devices.TearDown();
            Time.timeScale = 1;
            // OnDestroy may release the profile after its immutable writer finishes.
            double deadline = Time.realtimeSinceStartupAsDouble + 5;
            while (Directory.Exists(directory))
            {
                bool removed = false;
                try { Directory.Delete(directory, true); removed = true; }
                catch (IOException) { if (Time.realtimeSinceStartupAsDouble >= deadline) throw; }
                if (removed) break;
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator RealExcavationSaleUpgradeAndRescueSurviveRepeatedWholeWorldLoads()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            player.CloseMenu();
            yield return null;
            var find = discoveries.Finds[0];
            Expose(find);
            player.ViewCamera.transform.position = find.transform.position + Vector3.up * 1.5f;
            player.ViewCamera.transform.LookAt(find.transform.position);
            Physics.SyncTransforms();
            Assert.That(find.TryCollect(player), Is.True);
            string collectedId = find.Item.InstanceId;
            int value = find.Item.SaleValue;
            Assert.That(player.Trade.TrySell(player.Trade.OfferSale()), Is.True);
            Assert.That(player.Wallet.Balance, Is.EqualTo(value));
            // Use actual low-value trial finds until this real sale funds the upgrade.
            int soldCount = 1;
            while (player.Wallet.Balance < 10)
            {
                var next = discoveries.Finds.First(f => !f.Collected);
                Expose(next);
                player.ViewCamera.transform.position = next.transform.position + Vector3.up * 1.5f;
                player.ViewCamera.transform.LookAt(next.transform.position);
                Physics.SyncTransforms();
                Assert.That(next.TryCollect(player), Is.True);
                Assert.That(player.Trade.TrySell(player.Trade.OfferSale()), Is.True);
                soldCount++;
            }
            var offer = player.Trade.OfferUpgrade();
            Assert.That(player.Trade.TryUpgrade(offer), Is.True);
            Assert.That(player.Trade.TryUpgrade(offer), Is.False);
            // Cut into a side face, not only the upward-facing entrance.
            var point = find.transform.position;
            // Low-value finds require more nearby excavation to fund the purchase.
            // Author a real shaft below that widened area before testing its wall.
            for (int i = 0; i < 16; i++)
            {
                Assert.That(Physics.Raycast(point + Vector3.up * 4, Vector3.down, out var floor, 12), Is.True);
                if (floor.point.y < point.y - 1.2f) break;
                Assert.That(terrain.TryDig(floor, .65f), Is.True);
            }
            bool sideways = false;
            foreach (var direction in new[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back })
                if (Physics.Raycast(point + Vector3.down * .6f, direction, out var side, 3)
                    && side.collider.GetComponentInParent<TerrainVolume>() == terrain)
                    sideways |= terrain.TryDig(side, 0.65f);
            Assert.That(sideways, Is.True);
            player.AdminReturnToSurface();
            player.Battery.TrySpend(37.5f);
            player.SelectAdminLevel(6);
            player.ToggleAdminUnlimitedBattery();
            player.OpenMenu(PlayerMenu.Pause);
            long previous = save.CompletedSequence;
            save.RequestCheckpoint();
            yield return Until(() => save.CompletedSequence > previous && save.State == WorldSaveState.Ready);
            var expected = WorldSaveStore.Read(Path.Combine(directory, "world.sav"));
            Assert.That(expected.ShovelLevel, Is.EqualTo(2), "Admin level 6 is not owned progression.");
            Assert.That(expected.BatteryCharge, Is.EqualTo(62.5f));
            Vector3 rayOrigin = find.transform.position + Vector3.up * 4;
            Assert.That(Physics.Raycast(rayOrigin, Vector3.down, out var ground, 12), Is.True);
            float groundY = ground.point.y;
            for (int repeat = 0; repeat < 2; repeat++)
            {
                yield return SceneManager.UnloadSceneAsync(scene);
                yield return Open();
                Assert.That(save.State, Is.EqualTo(WorldSaveState.Ready));
                Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
                Assert.That(player.HasAdminOverrides, Is.False);
                Assert.That(player.Shovel.Level, Is.EqualTo(expected.ShovelLevel));
                Assert.That(player.Wallet.Balance, Is.EqualTo(expected.Credits));
                Assert.That(player.Inventory.Count, Is.Zero);
                Assert.That(player.Battery.Charge, Is.EqualTo(expected.BatteryCharge));
                Assert.That(player.transform.position, Is.EqualTo(expected.PlayerPosition));
                Assert.That(terrain.Capture().Density.ToArray(), Is.EqualTo(expected.Terrain.Density.ToArray()));
                Assert.That(discoveries.Finds.Single(f => f.Item.InstanceId == collectedId).Collected, Is.True);
                Assert.That(discoveries.Finds.Count, Is.EqualTo(96));
                Assert.That(discoveries.Finds.Count(f => f.Collected), Is.EqualTo(soldCount));
                Assert.That(Physics.Raycast(rayOrigin, Vector3.down, out ground, 12), Is.True);
                Assert.That(ground.point.y, Is.EqualTo(groundY).Within(0.001f), "Collision must be restored before Resume is available.");
            }
            // Rescue uses the same boundary and must not respawn its lost discovery.
            player.CloseMenu();
            var carried = discoveries.Finds.First(f => !f.Collected);
            Expose(carried);
            player.ViewCamera.transform.position = carried.transform.position + Vector3.up * 1.5f;
            player.ViewCamera.transform.LookAt(carried.transform.position);
            Physics.SyncTransforms();
            Assert.That(carried.TryCollect(player), Is.True);
            player.enabled = true;
            player.SetApplicationFocus(true);
            player.Battery.TrySpend(player.Battery.Charge);
            previous = save.CompletedSequence;
            yield return null;
            yield return null;
            Assert.That(player.Battery.Charge, Is.EqualTo(player.Battery.Capacity));
            player.OpenMenu(PlayerMenu.Pause);
            yield return Until(() => save.CompletedSequence > previous && save.State == WorldSaveState.Ready);
            player.enabled = false;
            var rescued = WorldSaveStore.Read(Path.Combine(directory, "world.sav"));
            Assert.That(rescued.Inventory, Is.Empty);
            Assert.That(rescued.Finds.Count(f => f.Collected), Is.EqualTo(soldCount + 1));
            Assert.That(rescued.Credits, Is.EqualTo(Math.Max(0, expected.Credits - 10)));
            Assert.That(rescued.BatteryCharge, Is.EqualTo(100));
            Assert.That(rescued.Terrain.RemovedVolume, Is.GreaterThanOrEqualTo(expected.Terrain.RemovedVolume));
        }

        [UnityTest]
        public IEnumerator LegacyFindsSurviveMigrationCheckpointRecoveryAndRepeatedReload()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            var old = WorldSaveStore.Read(Path.Combine(directory, "world.sav"));
            var catalog = discoveries.Catalog;
            var layout = DiscoveryField.Generate((Vector3)terrain.Dimensions * terrain.CellSize, 96, old.DiscoverySeed);
            old.Finds = layout.Select((p, i) => new FindSnapshot {
                ContentId = catalog.LegacyAliases[i % 3].OldId,
                Item = new ItemSnapshot { Id = "legacy-" + i, Name = "Historical find " + (i % 3), Value = 5 + i % 3 * 3 },
                Position = p.Position, Rotation = p.Rotation, Scale = Vector3.one * .8f, Collected = i < 2
            }).ToArray();
            old.Inventory = new[] { old.Finds[0].Item }; old.Sequence++;
            yield return SceneManager.UnloadSceneAsync(scene);
            using (var store = new WorldSaveStore(directory)) { store.Load(); store.Commit(old); }
            yield return Open();
            Assert.That(discoveries.Finds.Count, Is.EqualTo(96));
            Assert.That(player.Inventory.Items.Single().DisplayName, Is.EqualTo(old.Inventory[0].Name));
            for (int i = 0; i < 96; i++)
            {
                var actual = discoveries.Finds[i].Capture();
                Assert.That(Vector3.Distance(actual.Position, old.Finds[i].Position), Is.LessThan(.000003f), "World/local transforms retain the saved centre within float precision.");
                Assert.That(actual.Item.Id, Is.EqualTo(old.Finds[i].Item.Id));
                Assert.That(actual.Item.Value, Is.EqualTo(old.Finds[i].Item.Value));
                Assert.That(actual.Collected, Is.EqualTo(i < 2)); Assert.That(actual.Scale, Is.EqualTo(Vector3.one));
            }
            long previous = save.CompletedSequence; save.RequestCheckpoint();
            yield return Until(() => save.CompletedSequence > previous && save.State == WorldSaveState.Ready);
            yield return SceneManager.UnloadSceneAsync(scene);
            // Recovery falls back to the retained legacy checkpoint, then migrates again.
            string path = Path.Combine(directory, "world.sav"); var bytes = File.ReadAllBytes(path);
            bytes[bytes.Length - 1] ^= 0x3f; File.WriteAllBytes(path, bytes);
            yield return Open(); Assert.That(save.State, Is.EqualTo(WorldSaveState.Recovery));
            save.AcceptRecovery(); yield return Until(() => save.State == WorldSaveState.Ready);
            Assert.That(discoveries.Finds.Count, Is.EqualTo(96));
            Assert.That(discoveries.Finds.Count(f => f.Collected), Is.EqualTo(2));
            Assert.That(player.Inventory.Items.Single().SaleValue, Is.EqualTo(5));
            previous = save.CompletedSequence; save.RequestCheckpoint();
            yield return Until(() => save.CompletedSequence > previous && save.State == WorldSaveState.Ready);
            yield return SceneManager.UnloadSceneAsync(scene); yield return Open();
            Assert.That(discoveries.Finds.Count, Is.EqualTo(96));
            Assert.That(discoveries.Finds.Select(f => f.Item.InstanceId), Is.EqualTo(old.Finds.Select(f => f.Item.Id)));
            Assert.That(discoveries.Finds.All(f => f.transform.localScale == Vector3.one), Is.True);
        }

        [UnityTest]
        public IEnumerator DirtyAutosaveAndOverlappingTransactionRequestsKeepLatestStateWithoutIdleGridCopies()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            long sequence = save.CompletedSequence;
            long copies = save.CapturedTerrainCopies;
            player.Battery.TrySpend(1);
            double started = Time.realtimeSinceStartupAsDouble;
            yield return Until(() => save.CompletedSequence > sequence);
            double elapsed = Time.realtimeSinceStartupAsDouble - started;
            Assert.That(elapsed, Is.InRange(WorldSaveController.AutosaveSeconds, WorldSaveController.AutosaveSeconds + 1));
            Assert.That(save.CapturedTerrainCopies, Is.EqualTo(copies), "Battery-only checkpoints reuse immutable density.");
            sequence = save.CompletedSequence;
            player.Wallet.TryCredit(10);
            save.RequestCheckpoint();
            yield return null;
            yield return null;
            Assert.That(player.Trade.TryUpgrade(player.Trade.OfferUpgrade()), Is.True);
            player.Battery.TrySpend(3);
            save.RequestCheckpoint();
            yield return Until(() => save.CompletedSequence > sequence && save.State == WorldSaveState.Ready);
            var latest = WorldSaveStore.Read(Path.Combine(directory, "world.sav"));
            Assert.That(latest.Credits, Is.Zero);
            Assert.That(latest.ShovelLevel, Is.EqualTo(2));
            Assert.That(latest.BatteryCharge, Is.EqualTo(96));
            Assert.That(save.CapturedTerrainCopies, Is.EqualTo(copies));
            Assert.That(save.LastCheckpointLatencyMilliseconds, Is.LessThanOrEqualTo(1000));
            UnityEngine.Debug.Log($"Save timing: dirty={elapsed:F3}s capture={save.LastCaptureMilliseconds:F3}ms write={save.LastWriteMilliseconds:F3}ms checkpoint={save.LastCheckpointLatencyMilliseconds:F3}ms");
        }

        [UnityTest]
        public IEnumerator DamagedPrimaryUsesReadableRecoveryAndUnknownVersionCannotResume()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            player.Wallet.TryCredit(7);
            long previous = save.CompletedSequence;
            yield return Until(() => save.CompletedSequence > previous && save.State == WorldSaveState.Ready);
            yield return SceneManager.UnloadSceneAsync(scene);
            string path = Path.Combine(directory, "world.sav");
            var bytes = File.ReadAllBytes(path);
            bytes[bytes.Length - 1] ^= 1;
            File.WriteAllBytes(path, bytes);
            yield return Open();
            Assert.That(save.State, Is.EqualTo(WorldSaveState.Recovery));
            Assert.That(player.GameplayActive, Is.False);
            player.CloseMenu();
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Persistence));
            save.AcceptRecovery();
            yield return Until(() => save.State == WorldSaveState.Ready && save.CompletedSequence > 1);
            Assert.That(Directory.GetFiles(directory, "world.damaged-*.sav").Length, Is.EqualTo(1));
            yield return SceneManager.UnloadSceneAsync(scene);
            bytes = File.ReadAllBytes(path); bytes[8] = 99; File.WriteAllBytes(path, bytes);
            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("World save: SomethingDownThere.UnsupportedSaveException"));
            yield return Open();
            Assert.That(save.State, Is.EqualTo(WorldSaveState.LoadFailed));
            Assert.That(save.BlocksPlay, Is.True);
            Assert.That(File.ReadAllBytes(path), Is.EqualTo(bytes));
        }

        [UnityTest]
        public IEnumerator UnwritableCheckpointPausesWithRetryAndKeepsLatestWorldInMemory()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            long previous = save.CompletedSequence;
            var accepted = File.ReadAllBytes(Path.Combine(directory, "world.sav"));
            string blocked = Path.Combine(directory, "world.pending");
            Directory.CreateDirectory(blocked);
            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("World save: System.(UnauthorizedAccessException|IO.IOException)"));
            player.Wallet.TryCredit(23);
            yield return Until(() => save.State == WorldSaveState.WriteFailed);
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Persistence));
            Assert.That(save.BlocksPlay, Is.True);
            Assert.That(MenuTestUI.Text(player, "menuTitle"), Is.EqualTo("Progress could not be saved"));
            Assert.That(File.ReadAllBytes(Path.Combine(directory, "world.sav")), Is.EqualTo(accepted));
            player.CloseMenu();
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Persistence));
            save.RequestExit();
            Assert.That(save.State, Is.EqualTo(WorldSaveState.ConfirmQuit));
            save.CancelUnsavedExit();
            Directory.Delete(blocked);
            save.Retry();
            yield return Until(() => save.CompletedSequence > previous && save.State == WorldSaveState.Ready);
            Assert.That(WorldSaveStore.Read(Path.Combine(directory, "world.sav")).Credits, Is.EqualTo(23));
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
        }

        [UnityTest]
        public IEnumerator CrouchOnlyChangesAutosaveAndLowRoofStanceSurvivesRelaunch()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            player.CloseMenu();
            yield return null;
            long previous = save.CompletedSequence;
            // No position, camera angle or battery change: stance alone is dirty.
            player.Tuning.Gravity = 0;
            player.Tick(new FpsInputFrame { CrouchHeld = true }, 0.1f);
            float partial = player.CrouchAmount;
            Assert.That(partial, Is.InRange(0.1f, 0.9f));
            yield return Until(() => save.CompletedSequence > previous && save.State == WorldSaveState.Ready);
            Assert.That(WorldSaveStore.Read(Path.Combine(directory, "world.sav")).CrouchAmount, Is.EqualTo(partial));
            yield return SceneManager.UnloadSceneAsync(scene);
            yield return Open();
            Assert.That(player.CrouchAmount, Is.EqualTo(partial));
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
            player.CloseMenu();
            yield return null;
            yield return CrouchTerrainFixture.Prepare(terrain);
            // Keep discovery state real, but place the fixture away from its finds.
            CrouchTerrainFixture.Place(player, new Vector3(6, -2.9f, 0), 1);
            player.Tick(default, 1f / 60);
            Assert.That(player.StandBlocked, Is.True);
            previous = save.CompletedSequence;
            save.RequestCheckpoint();
            yield return Until(() => save.CompletedSequence > previous && save.State == WorldSaveState.Ready);
            var expected = save.Capture(previous + 1);
            yield return SceneManager.UnloadSceneAsync(scene);
            yield return Open();
            Assert.That(save.State, Is.EqualTo(WorldSaveState.Ready));
            Assert.That(player.CrouchAmount, Is.EqualTo(1));
            Assert.That(player.transform.position, Is.EqualTo(expected.PlayerPosition));
            Assert.That(player.ViewCamera.transform.localPosition.y, Is.EqualTo(0.95f).Within(0.001f));
            Assert.That(terrain.Capture().Density.ToArray(), Is.EqualTo(expected.Terrain.Density.ToArray()));
            player.CloseMenu();
            yield return null;
            player.Tick(default, 1f / 60);
            Assert.That(player.StandBlocked, Is.True);
            Assert.That(player.CrouchAmount, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator ImpossibleSavedStanceStaysBehindRecoveryWithoutEditingTheSave()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            var snapshot = save.Capture(save.CompletedSequence + 1);
            snapshot.PlayerPosition = new Vector3(0, -6, 0); // Entire capsule inside solid soil.
            snapshot.CrouchAmount = 1;
            yield return SceneManager.UnloadSceneAsync(scene);
            string path = Path.Combine(directory, "world.sav");
            using (var stream = File.Create(path)) WorldSaveCodec.Write(stream, snapshot);
            var bytes = File.ReadAllBytes(path);
            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("World save: System.IO.InvalidDataException: The saved player stance"));
            yield return Open();
            Assert.That(save.State, Is.EqualTo(WorldSaveState.LoadFailed));
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Persistence));
            Assert.That(save.BlocksPlay, Is.True);
            Assert.That(File.ReadAllBytes(path), Is.EqualTo(bytes));
        }

        [UnityTest]
        public IEnumerator BottleMotionAloneAutosavesAndReleasedPoseSurvivesReload()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            player.CloseMenu();
            yield return null;
            long previous = save.CompletedSequence, terrainRevision = terrain.StateRevision;
            Vector3 playerPosition = player.transform.position;
            var find = discoveries.Finds[0]; var physical = find.GetComponent<FindPhysics>();
            find.transform.SetPositionAndRotation(terrain.transform.TransformPoint(new Vector3(12, terrain.Dimensions.y * terrain.CellSize + .8f, 12)), Quaternion.Euler(0, 0, 90));
            physical.Restore(false); Physics.SyncTransforms(); find.RefreshExposure();
            yield return new WaitForSeconds(1.8f);
            Assert.That(physical.Released, Is.True);
            Assert.That(terrain.StateRevision, Is.EqualTo(terrainRevision));
            Assert.That(player.transform.position, Is.EqualTo(playerPosition));
            yield return Until(() => save.CompletedSequence > previous && save.State == WorldSaveState.Ready);
            var expected = WorldSaveStore.Read(Path.Combine(directory, "world.sav")).Finds.First(f => f.Item.Id == find.Item.InstanceId);
            Assert.That(expected.PhysicsReleased, Is.True);
            Assert.That(Vector3.Distance(expected.Position, find.Capture().Position), Is.LessThan(.005f));
            yield return SceneManager.UnloadSceneAsync(scene); yield return Open();
            var restored = discoveries.Finds.First(f => f.Item.InstanceId == expected.Item.Id);
            Assert.That(Vector3.Distance(restored.Capture().Position, expected.Position), Is.LessThan(.00001f));
            Assert.That(restored.GetComponent<FindPhysics>().Released, Is.True);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
            player.CloseMenu(); yield return new WaitForSeconds(.3f);
            Assert.That(restored.Collectible, Is.True);
        }

        [UnityTest]
        public IEnumerator RockAppearancePoseHistoricalValueAndCollectedAbsenceSurviveFileReload()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            var snapshot = save.Capture(save.CompletedSequence + 1);
            var rocks = snapshot.Finds.Where(f => f.ContentId.StartsWith("common_rock_")).GroupBy(f => f.ContentId).Select(g => g.First()).ToArray();
            Assert.That(rocks.Length, Is.EqualTo(3));
            for (int i = 0; i < rocks.Length; i++)
            {
                rocks[i].Position = new Vector3(12 + i * 1.5f, terrain.Dimensions.y * terrain.CellSize + .5f, 12);
                rocks[i].Rotation = Quaternion.Euler(19 + i * 27, 33 + i * 53, 71 + i * 31);
                rocks[i].PhysicsReleased = true;
                rocks[i].Item.Value = 7 + i;
                rocks[i].Collected = i == 0;
            }
            snapshot.Inventory = new[] { rocks[0].Item };
            yield return SceneManager.UnloadSceneAsync(scene);
            using (var stream = File.Create(Path.Combine(directory, "world.sav"))) WorldSaveCodec.Write(stream, snapshot);
            yield return Open();
            foreach (var expected in rocks)
            {
                var restored = discoveries.Finds.Single(f => f.Item.InstanceId == expected.Item.Id);
                var actual = restored.Capture();
                Assert.That(actual.ContentId, Is.EqualTo(expected.ContentId));
                Assert.That(actual.Position, Is.EqualTo(expected.Position));
                Assert.That(Quaternion.Angle(actual.Rotation, expected.Rotation), Is.LessThan(.01f));
                Assert.That(actual.PhysicsReleased, Is.True);
                Assert.That(actual.Item.Value, Is.EqualTo(expected.Item.Value));
                Assert.That(actual.Collected, Is.EqualTo(expected.Collected));
                Assert.That(restored.gameObject.activeSelf, Is.EqualTo(!expected.Collected));
                Assert.That(restored.GetComponent<MeshFilter>().sharedMesh,
                    Is.SameAs(discoveries.Catalog.Resolve(expected.ContentId, out _).GetComponent<MeshFilter>().sharedMesh));
            }
            Assert.That(player.Inventory.Items.Single().InstanceId, Is.EqualTo(rocks[0].Item.Id));
        }

        [UnityTest]
        public IEnumerator CheckpointWhileLiftingRestoresOneReleasedWorldFindWithoutInventoryDuplication()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            player.CloseMenu(); yield return null;
            var find = discoveries.Finds.First(f => f.Size == FindSize.Large);
            find.transform.position = terrain.transform.TransformPoint(new Vector3(12, terrain.Dimensions.y * terrain.CellSize + .7f, 12));
            find.GetComponent<FindPhysics>().Restore(false); Physics.SyncTransforms(); find.RefreshExposure();
            player.ViewCamera.transform.position = find.transform.position + new Vector3(0, 1, -1);
            player.ViewCamera.transform.LookAt(find.transform.position);
            Assert.That(player.TryGrabOrDrop(), Is.True);
            var snapshot = save.Capture(save.CompletedSequence + 1);
            var expected = snapshot.Finds.Single(f => f.Item.Id == find.Item.InstanceId);
            Assert.That(expected.PhysicsReleased, Is.True); Assert.That(expected.Collected, Is.False);
            Assert.That(snapshot.Inventory.Any(i => i.Id == expected.Item.Id), Is.False);
            yield return SceneManager.UnloadSceneAsync(scene);
            using (var stream = File.Create(Path.Combine(directory, "world.sav"))) WorldSaveCodec.Write(stream, snapshot);
            yield return Open();
            var restored = discoveries.Finds.Single(f => f.Item.InstanceId == expected.Item.Id);
            Assert.That(player.HeldFind, Is.Null); Assert.That(restored.GetComponent<FindPhysics>().Released, Is.True);
            Assert.That(restored.Capture().Position, Is.EqualTo(expected.Position));
            Assert.That(restored.SaveContentId, Is.EqualTo(expected.ContentId)); Assert.That(restored.Collected, Is.False);
            Assert.That(player.Inventory.Items.Any(i => i.InstanceId == expected.Item.Id), Is.False);
        }

        [UnityTest]
        public IEnumerator RetiredCanAndBrickRecordsBecomeBottlesWithoutLosingPopulationOrValue()
        {
            yield return Until(() => save.CompletedSequence > 0 && save.State == WorldSaveState.Ready);
            var old = save.Capture(save.CompletedSequence + 1);
            old.Finds = old.Finds.Where(f => f.ContentId.StartsWith("common_bottle_")).ToArray();
            string[] retired = { "common_can_intact", "common_can_crushed", "common_brick_whole", "common_brick_chipped" };
            for (int i = 0; i < 4; i++) { old.Finds[i].ContentId = retired[i]; old.Finds[i].Item.Name = i < 2 ? "Food/Drink Can" : "Brick"; old.Finds[i].Item.Value = i < 2 ? 1 : 3; }
            yield return SceneManager.UnloadSceneAsync(scene);
            using (var stream = File.Create(Path.Combine(directory, "world.sav"))) WorldSaveCodec.Write(stream, old);
            yield return Open();
            Assert.That(discoveries.Finds.Count, Is.EqualTo(72));
            Assert.That(discoveries.Finds.All(f => f.SaveContentId.StartsWith("common_bottle_")), Is.True);
            for (int i = 0; i < 4; i++)
            {
                var actual = discoveries.Finds[i].Capture();
                Assert.That(actual.Item.Name, Is.EqualTo("Glass Bottle")); Assert.That(actual.Item.Id, Is.EqualTo(old.Finds[i].Item.Id));
                Assert.That(actual.Item.Value, Is.EqualTo(old.Finds[i].Item.Value));
                Assert.That(Vector3.Distance(actual.Position, old.Finds[i].Position), Is.LessThan(.00001f));
            }
        }

        private void Expose(BuriedFind find)
        {
            float ring = Mathf.Max(find.WorldBounds.extents.x, find.WorldBounds.extents.z) + 0.12f;
            for (int pass = 0; pass < 12 && !find.Collectible; pass++)
                foreach (var offset in new[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back })
                {
                    if (find.Collectible) break;
                    Vector3 origin = find.transform.position + offset * ring;
                    origin.y = terrain.SurfaceHeight + 2;
                    Assert.That(Physics.Raycast(origin, Vector3.down, out var hit, 30), Is.True);
                    Assert.That(terrain.TryDig(hit, 0.65f), Is.True);
                }
            Assert.That(find.Collectible, Is.True);
            // Exposure can be on one side while soil still covers the vertical camera ray.
            // The save/transaction fixture needs a real clear view, in addition to eligibility.
            for (int i = 0; i < 12; i++)
            {
                Assert.That(Physics.Raycast(find.transform.position + Vector3.up * 3, Vector3.down, out var hit, 6), Is.True);
                if (hit.collider == find.GetComponent<MeshCollider>()) return;
                Assert.That(terrain.TryDig(hit, .41f), Is.True);
            }
            Assert.Fail("Could not open an actual overhead view of the eligible bottle.");
        }

        private static IEnumerator Until(Func<bool> condition)
        {
            double deadline = Time.realtimeSinceStartupAsDouble + 30;
            while (!condition())
            {
                Assert.That(Time.realtimeSinceStartupAsDouble, Is.LessThan(deadline), "Save operation timed out.");
                yield return null;
            }
        }
    }
}
#endif
