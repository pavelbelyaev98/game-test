using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace SomethingDownThere.Tests
{
    public sealed class FpsUiInputTests
    {
        private Keyboard keyboard;
        private Mouse mouse;
        private GameObject root, target, floor;
        private FpsPlayer player;
        private InputTestFixture devices;

        [UnitySetUp]
        public IEnumerator CreatePlayer()
        {
            // Own the isolated input lifetime inside the coroutine setup: a later NUnit
            // SetUp reset would otherwise invalidate the player's already enabled actions.
            devices = new InputTestFixture();
            devices.Setup();
            keyboard = InputSystem.AddDevice<Keyboard>();
            mouse = InputSystem.AddDevice<Mouse>();
            root = new GameObject("UI test player");
            root.SetActive(false);
            root.layer = 2;
            var motor = root.AddComponent<CharacterController>();
            motor.center = new Vector3(0, 0.9f, 0);
            motor.height = 1.8f;
            motor.radius = 0.3f;
            var camera = new GameObject("Camera", typeof(Camera));
            camera.transform.SetParent(root.transform, false);
            camera.transform.localPosition = new Vector3(0, 1.6f, 0);
            player = root.AddComponent<FpsPlayer>();
            root.AddComponent<FpsHud>();
            floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.transform.position = new Vector3(0, -0.5f, 0);
            floor.transform.localScale = new Vector3(20, 1, 20);
            target = GameObject.CreatePrimitive(PrimitiveType.Cube);
            target.transform.position = new Vector3(0, 1.6f, 2);
            root.SetActive(true);
            Physics.SyncTransforms();
            yield return null;
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator RemovePlayer()
        {
            Object.Destroy(root);
            Object.Destroy(target);
            Object.Destroy(floor);
            yield return null;
            Time.timeScale = 1f;
            devices.TearDown();
        }

        [UnityTest]
        public IEnumerator HeldDigDoesNotLeakThroughInventoryClose()
        {
            var dig = target.AddComponent<ValidationDigTarget>();
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(dig.HitsRemaining, Is.EqualTo(2));
            devices.Press(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Inventory));
            devices.Release(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            devices.Press(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.None));
            yield return new WaitForSecondsRealtime(0.5f);
            Assert.That(dig.HitsRemaining, Is.EqualTo(2));
            devices.Release(mouse.leftButton, queueEventOnly: true);
            yield return null;
            yield return null;
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(dig.HitsRemaining, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator TabInspectsNamesAndValuesWithoutOfferingOrExecutingTransactions()
        {
            target.AddComponent<ValidationStation>();
            var carried = Enumerable.Range(0, 10).Select(i => new InventoryItem("coin-" + i, "Coin", i + 1)).ToArray();
            foreach (var item in carried) player.Inventory.TryAdd(item);
            devices.Press(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Inventory));
            var body = root.GetComponentsInChildren<Text>().Single(text => text.name == "Body");
            StringAssert.Contains("Carried finds: 10 / 10", body.text);
            Assert.That(body.text.Split('\n').Count(line => line.StartsWith("Coin  |")), Is.EqualTo(10));
            foreach (var item in carried) StringAssert.Contains("Coin  |  Sale value: " + item.SaleValue + "\n", body.text);
            Assert.That(root.GetComponentsInChildren<Button>().Select(button => button.name), Is.EqualTo(new[] { "Close" }));
            devices.Press(keyboard.eKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Inventory));
            Assert.That(player.ExecuteStationCommand(0), Is.False);
            devices.Release(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            devices.Press(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.None));
            Assert.That(player.Inventory.Items, Is.EqualTo(carried));
        }

        [UnityTest]
        public IEnumerator StationUsesEnterAndArrowNavigationWithoutSpaceSubmission()
        {
            target.AddComponent<ValidationStation>();
            var first = new InventoryItem("coin-01", "Coin", 5);
            var second = new InventoryItem("coin-02", "Coin", 17);
            player.Inventory.TryAdd(first);
            player.Inventory.TryAdd(second);
            // E opens the station while Space is held. Space must not submit Sell All.
            devices.Press(keyboard.eKey, queueEventOnly: true);
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Station));
            Assert.That(player.Inventory.Items, Is.EqualTo(new[] { first, second }));
            Assert.That(EventSystem.current.currentSelectedGameObject.name, Is.EqualTo("Sell All"));
            devices.Press(keyboard.downArrowKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(EventSystem.current.currentSelectedGameObject.name, Is.EqualTo("Sell first item"));
            devices.Press(keyboard.enterKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Inventory.Items, Is.EqualTo(new[] { second }));
            var body = root.GetComponentsInChildren<Text>().Single(text => text.name == "Body");
            StringAssert.Contains("Coin  |  Sale value: 17", body.text);
            StringAssert.DoesNotContain("Sale value: 5", body.text);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Station));
        }

        [UnityTest]
        public IEnumerator PointerCanResumePauseWithoutDiggingUnderneath()
        {
            var dig = target.AddComponent<ValidationDigTarget>();
            devices.Press(keyboard.escapeKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
            var resume = root.GetComponentsInChildren<Button>().Single(button => button.name == "Resume");
            var screen = RectTransformUtility.WorldToScreenPoint(null, resume.transform.position);
            devices.Set(mouse.position, screen, queueEventOnly: true);
            yield return null;
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return null;
            devices.Release(mouse.leftButton, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.None));
            Assert.That(dig.HitsRemaining, Is.EqualTo(3));
        }
    }
}
