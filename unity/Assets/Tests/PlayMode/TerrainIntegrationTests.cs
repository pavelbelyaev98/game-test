#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
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

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            previousTimeScale = Time.timeScale;
            previousCursor = Cursor.lockState;
            previousCursorVisible = Cursor.visible;
            Time.timeScale = 1;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/MainGame.unity",
                new LoadSceneParameters(LoadSceneMode.Additive));
            scene = SceneManager.GetSceneByPath("Assets/Scenes/MainGame.unity");
            GameObject root = scene.GetRootGameObjects()[0];
            terrain = root.GetComponentInChildren<TerrainVolume>();
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
            Time.timeScale = previousTimeScale;
            Cursor.lockState = previousCursor;
            Cursor.visible = previousCursorVisible;
        }

        [Test]
        public void FreshSceneHasUntouchedSoilAndNoSurfaceSlabBlockingExcavation()
        {
            Assert.That(terrain.RemainingCells, Is.EqualTo(48 * 24 * 48));
            Assert.That(terrain.ChunkCount, Is.EqualTo(108));
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
            Assert.That(floor.point.y, Is.LessThan(-0.5f));
            Assert.That(terrain.IsSolid(new Vector3(0.25f, -0.25f, 0.25f)), Is.False);
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
            TestContext.WriteLine($"Terrain task 06: {timings.Count} accepted cuts, mean {timings.Average():F3} ms, max {timings.Max():F3} ms; "
                + $"rebuilt {dirtyCounts.Min()}-{dirtyCounts.Max()} of {terrain.ChunkCount} chunks per cut (includes collision cooking).");
            Assert.That(dirtyCounts.Max(), Is.LessThan(terrain.ChunkCount));
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
