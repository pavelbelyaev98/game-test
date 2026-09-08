#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SomethingDownThere.Tests
{
    public sealed class TerrainIntegrationTests
    {
        private Scene scene;
        private TerrainVolume terrain;
        private FpsPlayer player;
        private float previousTimeScale;
        private CursorLockMode previousCursor;
        private bool previousCursorVisible;
        private InputTestFixture devices;
        private Keyboard keyboard;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            previousTimeScale = Time.timeScale;
            previousCursor = Cursor.lockState;
            previousCursorVisible = Cursor.visible;
            devices = new InputTestFixture();
            devices.Setup();
            keyboard = InputSystem.AddDevice<Keyboard>();
            InputSystem.AddDevice<Mouse>();
            Time.timeScale = 1;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/MainGame.unity",
                new LoadSceneParameters(LoadSceneMode.Additive));
            scene = SceneManager.GetSceneByPath("Assets/Scenes/MainGame.unity");
            GameObject root = scene.GetRootGameObjects()[0];
            terrain = root.GetComponentInChildren<TerrainVolume>();
            // These checks own terrain geometry. Discovery interaction has its own
            // MainGame integration fixture, with the generated finds enabled.
            root.GetComponentInChildren<DiscoveryField>()?.gameObject.SetActive(false);
            player = root.GetComponentInChildren<FpsPlayer>();
            player.enabled = false; // Tick explicitly; real device state must not influence checks.
            player.SetApplicationFocus(true);
            if (player.IsMenuOpen) player.CloseMenu();
            yield return null;
            Physics.SyncTransforms();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (scene.IsValid()) yield return SceneManager.UnloadSceneAsync(scene);
            devices.TearDown();
            Time.timeScale = previousTimeScale;
            Cursor.lockState = previousCursor;
            Cursor.visible = previousCursorVisible;
        }

        [Test]
        public void DetachedColumnDisappearsAcrossChunksInTheSamePaidStroke()
        {
            // A moat leaves a tall, narrow pillar supported from below. Its crown
            // crosses four chunk seams and lies well outside the final shovel brush.
            for (int i = 0; i < 24; i++)
            {
                float angle = i * Mathf.PI * 2 / 24;
                var origin = new Vector3(Mathf.Cos(angle) * 1.2f, 2, Mathf.Sin(angle) * 1.2f);
                for (int cut = 0; cut < 14; cut++)
                {
                    var floor = Hit(origin, Vector3.down);
                    if (floor.point.y < -5.3f) break;
                    Assert.That(terrain.TryDig(floor, 0.8f), Is.True);
                }
            }
            var crown = new Vector3(0, -0.2f, 0);
            Assert.That(terrain.IsSolid(crown), Is.True, "The pillar must survive while its base is attached.");
            var oldCrownHit = Hit(new Vector3(0, 2, 0), Vector3.down);
            player.SelectAdminLevel(6);
            player.ViewCamera.transform.position = new Vector3(1.2f, -4.1f, 0);
            player.ViewCamera.transform.LookAt(new Vector3(0, -4.1f, 0));
            Physics.SyncTransforms();
            int revision = terrain.Revision;
            float energy = player.Battery.Charge, volume = terrain.RemovedVolume;
            Assert.That(player.TryDig(), Is.True);
            Assert.That(terrain.LastDetachedSamples, Is.GreaterThan(0));
            Assert.That(terrain.LastDetachedVolume, Is.GreaterThan(0));
            Assert.That(terrain.IsSolid(crown), Is.False);
            Assert.That(Hit(new Vector3(0, 2, 0), Vector3.down).point.y, Is.LessThan(-3),
                "The crown's collider must disappear before the accepted dig returns.");
            Assert.That(player.Battery.Charge, Is.EqualTo(energy - 2));
            Assert.That(terrain.Revision, Is.EqualTo(revision + 1));
            Assert.That(terrain.RemovedVolume - volume, Is.EqualTo(player.LastScoopVolume).Within(0.001f));
            Assert.That(terrain.GetComponentsInChildren<Rigidbody>(), Is.Empty);
            foreach (var collider in terrain.GetComponentsInChildren<MeshCollider>().Where(c => c.enabled))
                Assert.That(collider.sharedMesh, Is.SameAs(collider.GetComponent<MeshFilter>().sharedMesh));
            float removed = terrain.RemovedVolume;
            Assert.That(terrain.TryDig(oldCrownHit), Is.False, "A cached ray cannot dig the disappeared crown again.");
            Assert.That(terrain.RemovedVolume, Is.EqualTo(removed));
            player.OpenMenu(PlayerMenu.Pause);
            player.ShowAdminMenu();
            player.RequestTerrainReset();
            Assert.That(player.ConfirmTerrainReset(), Is.True);
            Assert.That(terrain.IsSolid(crown), Is.True);
            Assert.That(Hit(new Vector3(0, 2, 0), Vector3.down).point.y, Is.EqualTo(0).Within(0.001f));
        }

        [Test]
        public void FreshSceneHasUntouchedSoilAndNoSurfaceSlabBlockingExcavation()
        {
            Assert.That(terrain.RemovedVolume, Is.Zero);
            Assert.That(terrain.Dimensions, Is.EqualTo(new Vector3Int(192, 96, 192)));
            Assert.That(terrain.ChunkCount, Is.EqualTo(864));
            Assert.That(terrain.Revision, Is.Zero);
            foreach (Vector3 origin in new[] { new Vector3(-10, 2, -10), new Vector3(0, 2, 0), new Vector3(10, 2, 10) })
            {
                RaycastHit hit = Hit(origin, Vector3.down);
                Assert.That(hit.collider.GetComponentInParent<TerrainVolume>(), Is.EqualTo(terrain));
                Assert.That(hit.point.y, Is.EqualTo(0).Within(0.001f));
            }
            var anchor = scene.GetRootGameObjects()[0].transform.Find("Surface/ReturnAnchor");
            Assert.That(Physics.CheckCapsule(anchor.position + Vector3.up * 0.35f,
                anchor.position + Vector3.up * 1.5f, 0.3f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore), Is.False);
        }

        [Test]
        public void RealDigChargesOnceRejectsStaleHitsAndRespectsPauseDepletionAndBedrock()
        {
            PlacePlayer(new Vector3(0, 0.1f, 0));
            player.ViewCamera.transform.LookAt(new Vector3(0, -1, 0));
            RaycastHit stale = Hit(player.ViewCamera.transform.position, Vector3.down);
            Assert.That(player.TryDig(), Is.True);
            Assert.That(player.Battery.Charge, Is.EqualTo(98));
            int remaining = terrain.RemainingCells;
            Assert.That(terrain.TryDig(stale), Is.False);
            Assert.That(terrain.RemainingCells, Is.EqualTo(remaining));
            player.OpenMenu(PlayerMenu.Pause);
            Assert.That(player.TryDig(), Is.False);
            player.CloseMenu();
            player.Battery.TrySpend(98);
            Assert.That(player.TryDig(), Is.False);
            Assert.That(terrain.RemainingCells, Is.EqualTo(remaining));
            player.Battery.Recharge();
            PlacePlayer(new Vector3(0, 0.1f, -15));
            player.ViewCamera.transform.LookAt(new Vector3(0, 0.8f, -17));
            Assert.That(player.TryDig(), Is.False);
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
        }

        [Test]
        public void SeamCutsUpdateRenderAndCollisionLocallyAndSurviveLeavingAndReenabling()
        {
            var filters = terrain.GetComponentsInChildren<MeshFilter>();
            MeshFilter distant = filters.First(f => f.name == "Chunk 0,0,0");
            Vector3[] previousVertices = distant.sharedMesh.vertices;
            RaycastHit top = Hit(new Vector3(0, 2, 0), Vector3.down); // Four chunks meet here.
            Assert.That(terrain.TryDig(top), Is.True);
            Assert.That(terrain.LastRebuiltChunkCount, Is.InRange(4, 8));
            Assert.That(terrain.LastRebuiltChunkCount, Is.LessThan(terrain.ChunkCount));
            CollectionAssert.AreEqual(previousVertices, distant.sharedMesh.vertices);
            foreach (var collider in terrain.GetComponentsInChildren<MeshCollider>().Where(c => c.enabled))
                Assert.That(collider.sharedMesh, Is.SameAs(collider.GetComponent<MeshFilter>().sharedMesh));
            RaycastHit floor = Hit(new Vector3(0, 2, 0), Vector3.down);
            Assert.That(floor.point.y, Is.InRange(-0.5f, -0.25f), "A shallow shovel bite still opens usable space.");
            Assert.That(terrain.IsSolid(new Vector3(0.1f, -0.15f, 0.1f)), Is.False);
            int count = terrain.RemainingCells;
            // A physical walk along the surface must not initialize a new excavation.
            for (int i = 0; i < 60; i++) player.Tick(new FpsInputFrame { Move = Vector2.right }, 1f / 60f);
            Assert.That(player.transform.position.x, Is.GreaterThan(3));
            terrain.gameObject.SetActive(false);
            terrain.gameObject.SetActive(true);
            terrain.InitializeSession();
            Physics.SyncTransforms();
            Assert.That(terrain.RemainingCells, Is.EqualTo(count));
            Assert.That(Hit(new Vector3(0, 2, 0), Vector3.down).point.y, Is.EqualTo(floor.point.y).Within(0.001f));
        }

        [Test]
        public void PlayerCanDescendWalkIntoLateralCutAndFlyBackThroughOwnShaft()
        {
            terrain.DigRadius = 1.1f; // A wider tool makes a body-sized lateral passage in one pass.
            for (int i = 0; i < 4; i++) Assert.That(terrain.TryDig(Hit(new Vector3(0, 2, 0), Vector3.down)), Is.True);
            float floorY = Hit(new Vector3(0, 2, 0), Vector3.down).point.y;
            Vector3 tunnelOrigin = new Vector3(0, floorY + 1.25f, 0);
            for (int i = 0; i < 4; i++) Assert.That(terrain.TryDig(Hit(tunnelOrigin, Vector3.forward)), Is.True);
            PlacePlayer(new Vector3(0, 0.1f, 0));
            for (int i = 0; i < 180; i++) player.Tick(default, 1f / 60f);
            Assert.That(player.transform.position.y, Is.InRange(floorY - 0.5f, floorY + 0.2f));
            for (int i = 0; i < 30; i++) player.Tick(new FpsInputFrame { Move = Vector2.up }, 1f / 60f);
            Assert.That(player.transform.position.z, Is.GreaterThan(1.2f), "Lateral cut must fit the CharacterController.");
            for (int i = 0; i < 30; i++) player.Tick(new FpsInputFrame { Move = Vector2.down }, 1f / 60f);
            int count = terrain.RemainingCells;
            for (int i = 0; i < 100; i++) player.Tick(new FpsInputFrame { JetpackHeld = true }, 1f / 60f);
            Assert.That(player.transform.position.y, Is.GreaterThan(0.5f));
            Assert.That(terrain.RemainingCells, Is.EqualTo(count));
            Assert.That(terrain.IsSolid(new Vector3(0.25f, -0.25f, 0.25f)), Is.False);
        }

        [Test]
        public void LargeRepeatedCutsExposeButNeverRemoveFloorOrSideBoundaries()
        {
            terrain.DigRadius = 4;
            DigUntilBoundary(new Vector3(0, 2, 0), Vector3.down, -12);
            foreach (Vector3 direction in new[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back })
                DigUntilBoundary(new Vector3(0, -5, 0), direction, 12);
        }

        [Test]
        public void RepresentativeAcceptedCutsReportMeshAndColliderUpdateCosts()
        {
            var timings = new List<double>();
            var dirtyCounts = new List<int>();
            foreach (float coordinate in new[] { -6f, -2f, 2f, 6f })
            for (int i = 0; i < 5; i++)
            {
                Assert.That(terrain.TryDig(Hit(new Vector3(coordinate, 2, coordinate), Vector3.down)), Is.True);
                timings.Add(terrain.LastDigMilliseconds);
                dirtyCounts.Add(terrain.LastRebuiltChunkCount);
            }
            TestContext.WriteLine($"Smooth terrain: {timings.Count} accepted cuts, mean {timings.Average():F3} ms, max {timings.Max():F3} ms; "
                + $"rebuilt {dirtyCounts.Min()}-{dirtyCounts.Max()} of {terrain.ChunkCount} chunks per cut (includes collision cooking).");
            Assert.That(dirtyCounts.Max(), Is.LessThan(terrain.ChunkCount));
        }

        [Test]
        public void AdminShortcutsRemainGatedByPauseAndFocus()
        {
            Assert.That(player.AdminAvailable, Is.True);
            Assert.That(player.HasAdminOverrides, Is.False);
            player.Battery.TrySpend(20);
            player.Tick(new FpsInputFrame { RefillPressed = true }, 0.01f);
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            player.OpenMenu(PlayerMenu.Pause);
            player.Tick(new FpsInputFrame { AdminLevel = 6, DigHeld = true }, 0.01f);
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(1));
            Assert.That(terrain.Revision, Is.Zero);
            player.SetApplicationFocus(false);
            Assert.That(player.SelectAdminLevel(6), Is.False);
            player.ToggleAdminUnlimitedBattery();
            Assert.That(player.UnlimitedBattery, Is.False);
            Assert.That(player.ConfirmTerrainReset(), Is.False);
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator RealAdminChordsWorkOnNormalLaunchInFlightAndInsideAdminMenu()
        {
            Assert.That(player.AdminAvailable, Is.True);
            PlacePlayer(new Vector3(0, 5, 0));
            player.enabled = true;
            player.SetApplicationFocus(true);
            player.Battery.TrySpend(50);
            yield return null;
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.LeftCtrl, Key.LeftShift, Key.R));
            yield return null;
            yield return null;
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.RightCtrl, Key.RightShift, Key.Numpad6));
            yield return null;
            yield return null;
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(6));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));
            yield return new WaitForSeconds(0.35f);
            Assert.That(player.IsJetpackActive, Is.True);
            float altitude = player.transform.position.y;
            player.Battery.TrySpend(25);
            // Submit each chord as one keyboard state. Several queued single-key
            // writes would copy stale state and inadvertently resurrect released keys.
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.LeftCtrl, Key.LeftShift, Key.Space, Key.R, Key.Digit2));
            yield return null;
            yield return null;
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(2));
            Assert.That(player.Battery.Charge, Is.GreaterThan(98));
            Assert.That(player.IsJetpackActive, Is.True, "Refill and strength must not disarm held Space.");
            Assert.That(player.transform.position.y, Is.GreaterThan(altitude));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return new WaitForSeconds(0.45f);
            Assert.That(player.VerticalSpeed, Is.LessThan(0));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));
            yield return null;
            yield return null;
            Assert.That(player.IsJetpackActive, Is.True);
            Assert.That(player.VerticalSpeed, Is.GreaterThan(0));

            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.LeftCtrl, Key.LeftShift, Key.F10));
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.DeveloperAdmin));
            player.Battery.TrySpend(10);
            Vector3 pausedPosition = player.transform.position;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.LeftCtrl, Key.LeftShift, Key.Numpad4, Key.R));
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.DeveloperAdmin));
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(4));
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            Assert.That(player.transform.position, Is.EqualTo(pausedPosition));
            Assert.That(terrain.Revision, Is.Zero);
        }

        [UnityTest]
        public IEnumerator AdminMenuButtonsChooseStrengthAndKeepResetConfirmationSeparate()
        {
            player.OpenMenu(PlayerMenu.Pause);
            yield return null;
            player.GetComponentsInChildren<UnityEngine.UI.Button>().Single(b => b.name == "Developer admin  /  Ctrl+Shift+F10").onClick.Invoke();
            yield return null;
            player.GetComponentsInChildren<UnityEngine.UI.Button>().Single(b => b.name.StartsWith("Shovel 6")).onClick.Invoke();
            yield return null;
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(6));
            player.GetComponentsInChildren<UnityEngine.UI.Button>().Single(b => b.name == "Reset ground...").onClick.Invoke();
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.ConfirmTerrainReset));
            player.GetComponentsInChildren<UnityEngine.UI.Button>().Single(b => b.name == "Keep excavation").onClick.Invoke();
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.DeveloperAdmin));
            Assert.That(player.Shovel.Level, Is.EqualTo(1));
        }

        [Test]
        public void AdminLevelsChargeEqualEnergyAndOverrideNeverChangesOwnedProgression()
        {
            Assert.That(player.AdminAvailable, Is.True);
            float previous = 0;
            for (int level = 1; level <= 6; level++)
            {
                Assert.That(player.SelectAdminLevel(level), Is.True);
                Assert.That(player.EffectiveShovel.Radius, Is.EqualTo(ShovelProfile.Defaults()[level - 1].Radius),
                    "MainGame must use the current shovel tuning, including serialized scene profiles.");
                float x = (level - 1) * 3.6f - 9;
                PlacePlayer(new Vector3(x, 0.1f, 0));
                player.ViewCamera.transform.LookAt(new Vector3(x, -1, 0));
                float energy = player.Battery.Charge;
                Assert.That(player.TryDig(), Is.True);
                Assert.That(player.Battery.Charge, Is.EqualTo(energy - 2));
                if (previous > 0) Assert.That(player.LastScoopVolume / previous, Is.InRange(1.2f, 2.5f));
                Assert.That(player.LastScoopVolume, Is.InRange(0.1f, 3.2f));
                previous = player.LastScoopVolume;
                Assert.That(player.Shovel.Level, Is.EqualTo(1));
            }
            Assert.That(player.SelectAdminLevel(0), Is.False);
            Assert.That(player.SelectAdminLevel(7), Is.False);
            player.AdminReturnToSurface();
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(6));
            Assert.That(player.ExcavatedVolume, Is.GreaterThan(0));
            player.RestoreAdminOverrides();
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(1));
        }

        [Test]
        public void OwnedUpgradesExtendRealDigReachAtEveryLevelWithoutAdminOverrides()
        {
            float previous = 0;
            for (int level = 1; level <= 6; level++)
            {
                if (level > 1) Assert.That(player.Shovel.TryUpgradeTo(level), Is.True);
                Assert.That(player.HasAdminOverrides, Is.False);
                float reach = player.EffectiveDigReach;
                Assert.That(reach, Is.GreaterThan(previous));
                float x = (level - 1) * 3.6f - 9;
                PlacePlayer(new Vector3(x, reach + 0.15f - 1.6f, 0));
                player.ViewCamera.transform.LookAt(new Vector3(x, -1, 0));
                float charge = player.Battery.Charge;
                Assert.That(player.TryDig(), Is.False, $"Level {level} must respect its maximum reach.");
                Assert.That(player.Battery.Charge, Is.EqualTo(charge));
                player.RefreshTargetPrompt();
                Assert.That(player.TargetPrompt, Is.Empty, "Out-of-range digging remains silent.");
                PlacePlayer(new Vector3(x, reach - 0.1f - 1.6f, 0));
                Assert.That(player.TryDig(), Is.True, $"Level {level} must dig at its advertised range.");
                Assert.That(player.Battery.Charge, Is.EqualTo(charge - 2));
                previous = reach;
            }
            Assert.That(player.EffectiveDigReach, Is.EqualTo(4));
        }

        [Test]
        public void UnlimitedBatteryCoversDigAndFlightAndRestoresNormalRules()
        {
            PlacePlayer(new Vector3(0, 0.1f, 0));
            player.ViewCamera.transform.LookAt(Vector3.down);
            player.Battery.TrySpend(100);
            player.SelectAdminLevel(3);
            player.ToggleAdminUnlimitedBattery();
            Assert.That(player.TryDig(), Is.True);
            PlacePlayer(new Vector3(0, 3, 0));
            player.Tick(new FpsInputFrame { JetpackHeld = true }, 0.3f);
            Assert.That(player.IsJetpackActive, Is.True);
            Assert.That(player.Battery.Charge, Is.Zero);
            player.RestoreAdminOverrides();
            Assert.That(player.UnlimitedBattery || player.HasAdminOverrides, Is.False);
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(player.Shovel.Level));
            player.Tick(new FpsInputFrame { JetpackHeld = true }, 0.1f);
            Assert.That(player.IsJetpackActive, Is.False);
            Assert.That(player.TryDig(), Is.False);
            player.RefillAdminBattery();
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
        }

        [Test]
        public void FreshScoopDepthVariesAndResetReplaysTheSameShapes()
        {
            var depths = new List<float>();
            for (int pass = 0; pass < 2; pass++)
            {
                for (int i = 0; i < 6; i++)
                {
                    var origin = new Vector3(-8 + i * 3, 2, 0);
                    Assert.That(terrain.TryDig(Hit(origin, Vector3.down), 0.85f), Is.True);
                    float depth = -Hit(origin, Vector3.down).point.y;
                    Assert.That(depth, Is.InRange(0.5f, 1.0f));
                    if (pass == 0) depths.Add(depth);
                    else Assert.That(depth, Is.EqualTo(depths[i]).Within(0.0001f));
                }
                if (pass == 0) terrain.ResetExcavation();
            }
            Assert.That(depths.Max() - depths.Min(), Is.GreaterThan(0.02f));
        }

        [Test]
        public void AdminResetRequiresConfirmationAndReturnsPlayerBeforeFillingTerrain()
        {
            Assert.That(player.ConfirmTerrainReset(), Is.False);
            Assert.That(player.AdminAvailable, Is.True);
            player.SelectAdminLevel(3);
            Assert.That(terrain.TryDig(Hit(new Vector3(0, 2, 0), Vector3.down)), Is.True);
            player.Battery.TrySpend(20);
            PlacePlayer(new Vector3(0, -0.2f, 0));
            player.OpenMenu(PlayerMenu.Pause);
            player.ShowAdminMenu();
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(3), "Reopening admin preserves selection.");
            Assert.That(player.ConfirmTerrainReset(), Is.False);
            player.RequestTerrainReset();
            player.CancelTerrainReset();
            Assert.That(terrain.Revision, Is.EqualTo(1));
            player.RequestTerrainReset();
            Assert.That(player.ConfirmTerrainReset(), Is.True);
            Assert.That(terrain.Revision, Is.Zero);
            Assert.That(terrain.RemovedVolume, Is.Zero);
            Assert.That(Hit(new Vector3(0, 2, 0), Vector3.down).point.y, Is.EqualTo(0).Within(0.001f));
            Assert.That(player.transform.position.z, Is.LessThan(-12));
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            Assert.That(player.EffectiveShovelLevel, Is.EqualTo(3));
            Assert.That(player.Shovel.Level, Is.EqualTo(1));
        }

        private void DigUntilBoundary(Vector3 origin, Vector3 direction, float expectedCoordinate)
        {
            RaycastHit hit = default;
            for (int i = 0; i < 20; i++)
            {
                hit = Hit(origin, direction);
                if (hit.collider.GetComponent<PermanentTerrainBoundary>() != null) break;
                Assert.That(terrain.TryDig(hit), Is.True);
            }
            var boundary = hit.collider.GetComponent<PermanentTerrainBoundary>();
            Assert.That(boundary, Is.Not.Null);
            float coordinate = direction == Vector3.down ? hit.point.y : Mathf.Abs(Vector3.Dot(hit.point, direction));
            Assert.That(coordinate, Is.EqualTo(expectedCoordinate).Within(0.01f));
            int count = terrain.RemainingCells;
            for (int i = 0; i < 5; i++)
            {
                Assert.That(boundary.TryDig(hit), Is.False);
                Assert.That(terrain.TryDig(hit), Is.False);
            }
            Assert.That(terrain.RemainingCells, Is.EqualTo(count));
            Assert.That(boundary.GetComponent<Collider>().enabled, Is.True);
        }

        private void PlacePlayer(Vector3 position)
        {
            var motor = player.GetComponent<CharacterController>();
            motor.enabled = false;
            player.transform.position = position;
            motor.enabled = true;
            Physics.SyncTransforms();
        }

        private static RaycastHit Hit(Vector3 origin, Vector3 direction)
        {
            Physics.SyncTransforms();
            Assert.That(Physics.Raycast(origin, direction, out RaycastHit hit, 40, Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore), Is.True, $"Missing collision at {origin} toward {direction}.");
            return hit;
        }
    }
}
#endif
