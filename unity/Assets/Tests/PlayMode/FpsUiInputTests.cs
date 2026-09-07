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
        public IEnumerator StationUsesEnterAndArrowNavigationWithoutSpaceSubmission()
        {
            target.AddComponent<ValidationStation>();
            player.Inventory.TryAdd("Coin");
            // E opens the station while Space is held. Space must not submit Sell All.
            devices.Press(keyboard.eKey, queueEventOnly: true);
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Station));
            Assert.That(player.Inventory.Count, Is.EqualTo(1));
            Assert.That(EventSystem.current.currentSelectedGameObject.name, Is.EqualTo("Sell All"));
            devices.Press(keyboard.downArrowKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(EventSystem.current.currentSelectedGameObject.name, Is.EqualTo("Sell first item"));
            devices.Press(keyboard.enterKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Inventory.Count, Is.Zero);
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
