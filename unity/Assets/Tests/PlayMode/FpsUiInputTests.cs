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
        public IEnumerator UnchangedHudAndMenuTextRebuildAtTheCurrentScaleWithoutIdleRedraws()
        {
            player.OpenMenu(PlayerMenu.Pause);
            yield return null;
            yield return null;
            var canvas = root.GetComponentInChildren<Canvas>();
            var scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            var labels = root.GetComponentsInChildren<Text>().Where(t => !string.IsNullOrEmpty(t.text)).ToArray();
            var contents = labels.Select(t => t.text).ToArray();
            foreach (float scale in new[] { 1f, 1.5f, 0.75f, 1.25f })
            {
                scaler.scaleFactor = scale;
                Canvas.ForceUpdateCanvases();
                yield return null;
                Canvas.ForceUpdateCanvases();
                // Compare the game's unchanged labels with a fresh render at the same
                // scale. A cached low-resolution glyph layout must never be stretched.
                var rendered = labels.Select(t => t.cachedTextGenerator.verts.Select(v => v.position).ToArray()).ToArray();
                foreach (var label in labels) label.SetAllDirty();
                Canvas.ForceUpdateCanvases();
                for (int i = 0; i < labels.Length; i++)
                {
                    Assert.That(labels[i].text, Is.EqualTo(contents[i]));
                    Assert.That(rendered[i], Is.EqualTo(labels[i].cachedTextGenerator.verts.Select(v => v.position).ToArray()),
                        labels[i].name + " must already be freshly rendered at scale " + scale);
                }
            }
            var status = labels.Single(t => t.name == "Status");
            scaler.enabled = false;
            Canvas.ForceUpdateCanvases();
            Assert.That(canvas.scaleFactor, Is.EqualTo(1));
            var unscaled = status.cachedTextGenerator.verts.Select(v => v.position).ToArray();
            status.SetAllDirty();
            Canvas.ForceUpdateCanvases();
            Assert.That(unscaled, Is.EqualTo(status.cachedTextGenerator.verts.Select(v => v.position).ToArray()),
                "Disabling the scaler must also refresh the restored scale.");
            scaler.enabled = true;
            Canvas.ForceUpdateCanvases();
            yield return null;
            int redraws = 0;
            UnityEngine.Events.UnityAction onDirty = () => redraws++;
            status.RegisterDirtyVerticesCallback(onDirty);
            for (int i = 0; i < 4; i++) yield return null;
            status.UnregisterDirtyVerticesCallback(onDirty);
            Assert.That(redraws, Is.Zero, "Stable scale/content should not rebuild text every frame.");
        }

        [UnityTest]
        public IEnumerator ReturnWarningsShowReserveBandsAndFreezeAcrossInventoryInspection()
        {
            var batteryLabel = root.GetComponentsInChildren<Text>().Single(t => t.name == "Battery status");
            var warning = root.GetComponentsInChildren<Text>().Single(t => t.name == "Return warning");
            StringAssert.Contains("SAFE", batteryLabel.text);
            player.Battery.TrySpend(65);
            yield return null;
            StringAssert.Contains("RISKY", batteryLabel.text);
            Assert.That(warning.text, Is.EqualTo("RESERVE RUNNING LOW"));
            devices.Press(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(warning.gameObject.activeSelf, Is.False);
            player.Battery.TrySpend(20); // Simulate an external state change while inspection is open.
            yield return null;
            StringAssert.Contains("RISKY", batteryLabel.text, "A paused HUD does not cross warning bands.");
            devices.Release(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            devices.Press(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            yield return null;
            StringAssert.Contains("CRITICAL", batteryLabel.text);
            StringAssert.Contains("15%", batteryLabel.text, "The displayed percentage must agree with the critical threshold.");
            Assert.That(warning.text, Is.EqualTo("CHARGE CRITICAL"));
            Assert.That(warning.gameObject.activeSelf, Is.True);
            player.Battery.TrySpend(15);
            yield return null;
            StringAssert.Contains("EMPTY", batteryLabel.text);
            Assert.That(warning.text, Is.EqualTo("NO POWER FOR DIGGING OR FLIGHT"));
            player.Battery.Recharge();
            yield return null;
            StringAssert.Contains("SAFE", batteryLabel.text);
            Assert.That(warning.text, Is.Empty);
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
        public IEnumerator HeldSpaceCannotRestartThrustAfterFocusPauseUntilReleased()
        {
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.4f);
            Assert.That(player.IsJetpackActive, Is.True);
            player.SetApplicationFocus(false);
            float charge = player.Battery.Charge;
            Assert.That(player.IsJetpackActive, Is.False);
            yield return null;
            player.SetApplicationFocus(true);
            player.CloseMenu();
            yield return new WaitForSecondsRealtime(0.4f);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.None));
            Assert.That(player.IsJetpackActive, Is.False);
            Assert.That(player.Battery.Charge, Is.EqualTo(charge));
            devices.Release(keyboard.spaceKey, queueEventOnly: true);
            yield return null;
            yield return null;
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.4f);
            Assert.That(player.IsJetpackActive, Is.True);
            Assert.That(player.Battery.Charge, Is.LessThan(charge));
        }

        [UnityTest]
        public IEnumerator HeldSpaceCannotRestartThrustAfterInventoryCloseUntilReleased()
        {
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.4f);
            Assert.That(player.IsJetpackActive, Is.True);
            devices.Press(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Inventory));
            Assert.That(player.IsJetpackActive, Is.False);
            float charge = player.Battery.Charge;
            devices.Release(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            devices.Press(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.None));
            yield return new WaitForSecondsRealtime(0.1f);
            Assert.That(player.IsJetpackActive, Is.False);
            Assert.That(player.Battery.Charge, Is.EqualTo(charge));
            devices.Release(keyboard.spaceKey, queueEventOnly: true);
            yield return null;
            yield return null;
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.IsJetpackActive, Is.True, "A fresh press resumes this flight immediately.");
            Assert.That(player.Battery.Charge, Is.LessThan(charge));
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
