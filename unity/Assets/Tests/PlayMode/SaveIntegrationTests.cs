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
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
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
            // A second actual find supplies the rest of the first shovel price.
            var second = discoveries.Finds[1];
            Expose(second);
            player.ViewCamera.transform.position = second.transform.position + Vector3.up * 1.5f;
            player.ViewCamera.transform.LookAt(second.transform.position);
            Physics.SyncTransforms();
            Assert.That(second.TryCollect(player), Is.True);
            Assert.That(player.Trade.TrySell(player.Trade.OfferSale()), Is.True);
            var offer = player.Trade.OfferUpgrade();
            Assert.That(player.Trade.TryUpgrade(offer), Is.True);
            Assert.That(player.Trade.TryUpgrade(offer), Is.False);
            // Cut into a side face, not only the upward-facing entrance.
            var point = find.transform.position;
            bool sideways = false;
            for (int i = 0; i < 5; i++)
                if (Physics.Raycast(point + Vector3.up * 0.15f, Vector3.right, out var side, 3)
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
                Assert.That(terrain.Capture().Density, Is.EqualTo(expected.Terrain.Density));
                Assert.That(discoveries.Finds.Single(f => f.Item.InstanceId == collectedId).Collected, Is.True);
                Assert.That(discoveries.Finds.Count, Is.EqualTo(96));
                Assert.That(discoveries.Finds.Count(f => f.Collected), Is.EqualTo(2));
                Assert.That(Physics.Raycast(rayOrigin, Vector3.down, out ground, 12), Is.True);
                Assert.That(ground.point.y, Is.EqualTo(groundY).Within(0.001f), "Collision must be restored before Resume is available.");
            }
            // Rescue uses the same boundary and must not respawn its lost discovery.
            player.CloseMenu();
            var carried = discoveries.Finds[2];
            Expose(carried);
            player.ViewCamera.transform.position = carried.transform.position + Vector3.up * 1.5f;
            player.ViewCamera.transform.LookAt(carried.transform.position);
            Physics.SyncTransforms();
            Assert.That(carried.TryCollect(player), Is.True);
            player.enabled = true;
            player.SetApplicationFocus(true);
            player.OpenMenu(PlayerMenu.Pause);
            player.RequestRescue();
            Assert.That(player.ConfirmRescue(), Is.True);
            player.OpenMenu(PlayerMenu.Pause);
            previous = save.CompletedSequence;
            yield return Until(() => save.CompletedSequence > previous && save.State == WorldSaveState.Ready);
            player.enabled = false;
            var rescued = WorldSaveStore.Read(Path.Combine(directory, "world.sav"));
            Assert.That(rescued.Inventory, Is.Empty);
            Assert.That(rescued.Finds.Count(f => f.Collected), Is.EqualTo(3));
            Assert.That(rescued.Credits, Is.EqualTo(Math.Max(0, expected.Credits - 10)));
            Assert.That(rescued.BatteryCharge, Is.EqualTo(100));
            Assert.That(rescued.Terrain.RemovedVolume, Is.GreaterThanOrEqualTo(expected.Terrain.RemovedVolume));
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
            Assert.That(elapsed, Is.InRange(WorldSaveController.AutosaveSeconds, WorldSaveController.AutosaveSeconds + 3));
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
