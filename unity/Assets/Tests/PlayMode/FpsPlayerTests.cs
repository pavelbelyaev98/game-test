using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SomethingDownThere.Tests
{
    public sealed class FpsPlayerTests
    {
        private readonly List<GameObject> objects = new List<GameObject>();
        private FpsPlayer player;
        private float originalTimeScale;
        private CursorLockMode originalCursor;
        private bool originalCursorVisible;

        [SetUp]
        public void SetUp()
        {
            originalTimeScale = Time.timeScale;
            originalCursor = Cursor.lockState;
            originalCursorVisible = Cursor.visible;
            Time.timeScale = 1f;
            var root = Track(new GameObject("Test Player"));
            root.SetActive(false);
            root.layer = 2;
            var motor = root.AddComponent<CharacterController>();
            motor.height = 1.8f;
            motor.center = new Vector3(0, 0.9f, 0);
            motor.radius = 0.3f;
            motor.minMoveDistance = 0;
            var camera = new GameObject("Camera", typeof(Camera));
            camera.transform.SetParent(root.transform, false);
            camera.transform.localPosition = new Vector3(0, 1.6f, 0);
            player = root.AddComponent<FpsPlayer>();
            root.SetActive(true);
            Box("Floor", new Vector3(0, -0.5f, 0), new Vector3(100, 1, 100));
            Physics.SyncTransforms();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var item in objects) if (item != null) Object.DestroyImmediate(item);
            objects.Clear();
            Time.timeScale = originalTimeScale;
            Cursor.lockState = originalCursor;
            Cursor.visible = originalCursorVisible;
        }

        [Test]
        public void LookingDownKeepsWalkingHorizontalAndClampsPitch()
        {
            float energy = player.Battery.Charge;
            player.Tick(new FpsInputFrame { Look = new Vector2(0, -10000), Move = new Vector2(1, 1) }, 0.1f);
            var position = player.transform.position;
            Assert.That(new Vector2(position.x, position.z).magnitude, Is.EqualTo(0.4f).Within(0.02f));
            // CharacterController depenetration may lift the feet by its skin width.
            Assert.That(position.y, Is.LessThanOrEqualTo(player.GetComponent<CharacterController>().skinWidth + 0.01f));
            Assert.That(player.Pitch, Is.EqualTo(85f));
            Assert.That(player.Battery.Charge, Is.EqualTo(energy));
        }

        [Test]
        public void WalkingCannotPassThroughSolidWall()
        {
            Box("Wall", new Vector3(0, 1.5f, 1.5f), new Vector3(5, 3, 0.5f));
            Physics.SyncTransforms();
            for (int i = 0; i < 60; i++) player.Tick(new FpsInputFrame { Move = Vector2.up }, 1f / 60f);
            Assert.That(player.transform.position.z, Is.LessThan(1.1f));
            Assert.That(player.transform.position.z, Is.GreaterThan(0.5f));
        }

        [Test]
        public void OccluderAndReachPreventDiggingAndCollection()
        {
            var target = Box("Target", new Vector3(0, 1.6f, 2), Vector3.one * 0.4f);
            var dig = target.AddComponent<ValidationDigTarget>();
            target.AddComponent<ValidationFind>();
            var wall = Box("Blocker", new Vector3(0, 1.6f, 1), Vector3.one * 0.5f);
            Physics.SyncTransforms();
            Assert.That(player.TryDig(), Is.False);
            Assert.That(player.TryInteract(), Is.False);
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            wall.SetActive(false);
            target.transform.position = new Vector3(0, 1.6f, 5);
            Physics.SyncTransforms();
            Assert.That(player.TryDig(), Is.False);
            Assert.That(player.TryInteract(), Is.False);
            Assert.That(dig.HitsRemaining, Is.EqualTo(3));
        }

        [Test]
        public void DigCadenceChargesOnlyAcceptedHitsAndDoesNotCollect()
        {
            var target = Box("Dig", new Vector3(0, 1.6f, 2), Vector3.one);
            var dig = target.AddComponent<ValidationDigTarget>();
            var find = Box("Find", new Vector3(0, 1.6f, 2.8f), Vector3.one * 0.2f);
            find.AddComponent<ValidationFind>();
            Physics.SyncTransforms();
            for (int i = 0; i < 10; i++) player.Tick(new FpsInputFrame { DigHeld = true }, 0.01f);
            Assert.That(dig.HitsRemaining, Is.EqualTo(2));
            Assert.That(player.Battery.Charge, Is.EqualTo(98));
            player.Tick(new FpsInputFrame { DigHeld = true }, 0.35f);
            Assert.That(dig.HitsRemaining, Is.EqualTo(1));
            Assert.That(player.Inventory.Count, Is.Zero);
        }

        [Test]
        public void EmptyBatteryBlocksDigAndJetpackButAllowsWalking()
        {
            var target = Box("Dig", new Vector3(0, 1.6f, 2), Vector3.one).AddComponent<ValidationDigTarget>();
            Physics.SyncTransforms();
            player.Battery.TrySpend(100);
            Assert.That(player.TryDig(), Is.False);
            player.Tick(new FpsInputFrame { JetpackHeld = true, Move = Vector2.right }, 0.1f);
            Assert.That(player.transform.position.x, Is.GreaterThan(0.3f));
            Assert.That(player.VerticalSpeed, Is.LessThanOrEqualTo(0));
            Assert.That(target.HitsRemaining, Is.EqualTo(3));
        }

        [Test]
        public void JetpackUsesSharedBatteryAndCannotPassThroughCeiling()
        {
            Box("Ceiling", new Vector3(0, 3, 0), new Vector3(10, 0.5f, 10));
            Physics.SyncTransforms();
            for (int i = 0; i < 120; i++) player.Tick(new FpsInputFrame { JetpackHeld = true }, 1f / 60f);
            Assert.That(player.transform.position.y, Is.InRange(0.5f, 1.1f));
            Assert.That(player.Battery.Charge, Is.EqualTo(84f).Within(0.01f));
        }

        [Test]
        public void FullOrUnexposedFindStaysInWorldAndCannotBeCollectedTwice()
        {
            var find = Box("Find", new Vector3(0, 1.6f, 2), Vector3.one * 0.4f).AddComponent<ValidationFind>();
            Physics.SyncTransforms();
            find.Exposed = false;
            Assert.That(player.TryInteract(), Is.False);
            find.Exposed = true;
            for (int i = 0; i < player.Inventory.Capacity; i++) player.Inventory.TryAdd("Tin");
            Assert.That(player.TryInteract(), Is.False);
            Assert.That(find.gameObject.activeSelf, Is.True);
            player.Inventory.TryRemoveAt(0);
            Assert.That(player.TryInteract(), Is.True);
            Assert.That(player.TryInteract(), Is.False);
            Assert.That(player.Inventory.Count, Is.EqualTo(player.Inventory.Capacity));
            Assert.That(find.gameObject.activeSelf, Is.False);
        }

        [UnityTest]
        public IEnumerator MenuFreezesGameplayAndRestoresPreviousTimeScaleOnCloseAndDisable()
        {
            Time.timeScale = 0.5f;
            player.OpenMenu(PlayerMenu.Inventory);
            yield return null;
            var position = player.transform.position;
            float energy = player.Battery.Charge;
            player.Tick(new FpsInputFrame { Move = Vector2.up, Look = Vector2.one, DigHeld = true, JetpackHeld = true }, 0.1f);
            Assert.That(Time.timeScale, Is.Zero);
            Assert.That(player.transform.position, Is.EqualTo(position));
            Assert.That(player.Battery.Charge, Is.EqualTo(energy));
            Assert.That(Cursor.visible, Is.True);
            player.CloseMenu();
            Assert.That(Time.timeScale, Is.EqualTo(0.5f));
            yield return null;
            player.OpenMenu(PlayerMenu.Pause);
            player.enabled = false;
            Assert.That(Time.timeScale, Is.EqualTo(0.5f));
        }

        [UnityTest]
        public IEnumerator FocusReturnDoesNotResumeUntilExplicitClose()
        {
            player.SetApplicationFocus(false);
            yield return null;
            player.CloseMenu();
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
            player.SetApplicationFocus(true);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
            Assert.That(Time.timeScale, Is.Zero);
            player.CloseMenu();
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.None));
        }

        [UnityTest]
        public IEnumerator StationOpeningDoesNotSellAndInventoryCannotRunStationCommands()
        {
            var station = Box("Sell", new Vector3(0, 1.6f, 2), Vector3.one).AddComponent<ValidationStation>();
            Physics.SyncTransforms();
            player.Inventory.TryAdd("Coin");
            player.OpenMenu(PlayerMenu.Inventory);
            Assert.That(player.ExecuteStationCommand(0), Is.False);
            player.CloseMenu();
            yield return null;
            Assert.That(player.TryInteract(), Is.True);
            Assert.That(player.Inventory.Count, Is.EqualTo(1));
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Station));
            Assert.That(player.ExecuteStationCommand(0), Is.True);
            Assert.That(player.Inventory.Count, Is.Zero);
            Assert.That(player.ExecuteStationCommand(0), Is.False);
            Assert.That(station, Is.EqualTo(player.Station));
        }

        [Test]
        public void DisabledStationCannotExecuteAStaleCommand()
        {
            var station = Box("Sell", new Vector3(0, 1.6f, 2), Vector3.one).AddComponent<ValidationStation>();
            Physics.SyncTransforms();
            player.Inventory.TryAdd("Coin");
            player.TryInteract();
            station.enabled = false;
            Assert.That(player.ExecuteStationCommand(0), Is.False);
            Assert.That(player.Inventory.Count, Is.EqualTo(1));
        }

        private GameObject Box(string name, Vector3 position, Vector3 scale)
        {
            var box = Track(GameObject.CreatePrimitive(PrimitiveType.Cube));
            box.name = name;
            box.transform.position = position;
            box.transform.localScale = scale;
            return box;
        }

        private GameObject Track(GameObject item) { objects.Add(item); return item; }
    }
}
