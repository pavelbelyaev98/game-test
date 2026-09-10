using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace SomethingDownThere.Tests
{
    public sealed class FpsUiInputTests
    {
        private Keyboard keyboard;
        private Mouse mouse;
        private GameObject root, target, floor;
        private FpsPlayer player;
        private InputTestFixture devices;
        private PreferencesStore preferences;
        private PreferencesStore inputPreferences;

        private sealed class PreferencesStore : ICameraPreferencesStore
        {
            public string Contents;
            public int Writes;
            public bool Fail;
            public string Read() => Contents;
            public void Write(string contents)
            {
                Writes++;
                if (Fail) throw new System.IO.IOException("Test settings write failure");
                Contents = contents;
            }
        }

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
            preferences = new PreferencesStore();
            player.ConfigureCameraPreferences(preferences);
            inputPreferences = new PreferencesStore();
            player.ConfigureInputPreferences(inputPreferences);
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
            // Establish the fixture's focus after Editor activation callbacks; individual
            // tests drive focus changes explicitly through the production input barrier.
            player.SetApplicationFocus(true);
            if (player.IsMenuOpen) player.CloseMenu();
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
        public IEnumerator CameraKeyboardNavigationPreservesPauseFocusAndHeldActionBarriers()
        {
            var dig = target.AddComponent<ValidationDigTarget>();
            player.OpenMenu(PlayerMenu.Pause);
            yield return null; yield return null;
            yield return Key(keyboard.downArrowKey);
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("Camera comfort"));
            yield return Key(keyboard.enterKey);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.CameraComfort));
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("fovSlider"));
            // A native keyboard supplies raw Toolkit keys as well as Input System
            // navigation. Only the latter may change the one-degree camera value.
            using (var raw = KeyDownEvent.GetPooled(new Event { type = EventType.KeyDown, keyCode = KeyCode.RightArrow }))
                MenuTestUI.View(player).Root.SendEvent(raw);
            Assert.That(player.ViewCamera.fieldOfView, Is.EqualTo(75));
            yield return Key(keyboard.rightArrowKey);
            Assert.That(player.ViewCamera.fieldOfView, Is.EqualTo(76));
            yield return Key(keyboard.downArrowKey);
            yield return Key(keyboard.leftArrowKey);
            Assert.That(player.CameraSettings.SteadyCrosshair, Is.False);
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("steadyCrosshair"));
            yield return Key(keyboard.tabKey);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.CameraComfort));
            float charge = player.Battery.Charge;
            Vector3 position = player.transform.position;
            Quaternion rotation = player.ViewCamera.transform.rotation;
            devices.Press(keyboard.wKey, queueEventOnly: true);
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            devices.Press(keyboard.eKey, queueEventOnly: true);
            devices.Press(mouse.leftButton, queueEventOnly: true);
            devices.Set(mouse.delta, new Vector2(100, 100), queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.35f);
            Assert.That(player.transform.position, Is.EqualTo(position));
            Assert.That(player.ViewCamera.transform.rotation, Is.EqualTo(rotation));
            Assert.That(player.Battery.Charge, Is.EqualTo(charge));
            Assert.That(dig.HitsRemaining, Is.EqualTo(3));
            player.SetApplicationFocus(false);
            yield return null;
            player.SetApplicationFocus(true);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.CameraComfort));
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("steadyCrosshair"));
            Assert.That(preferences.Writes, Is.EqualTo(1));
            yield return Key(keyboard.escapeKey);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("Camera comfort"));
            devices.Release(keyboard.wKey, queueEventOnly: true);
            yield return Key(keyboard.upArrowKey);
            yield return Key(keyboard.enterKey);
            yield return new WaitForSecondsRealtime(0.35f);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.None));
            Assert.That(player.IsJetpackActive, Is.False);
            Assert.That(dig.HitsRemaining, Is.EqualTo(3));
            Assert.That(player.Battery.Charge, Is.EqualTo(charge));
            devices.Release(mouse.leftButton, queueEventOnly: true);
            yield return null; yield return null;
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return null; yield return null;
            Assert.That(dig.HitsRemaining, Is.EqualTo(2), "A new held press resumes digging.");
        }

        [UnityTest]
        public IEnumerator CameraResetAndRetryKeepControlsSelectionAndWorldState()
        {
            player.Inventory.TryAdd(new InventoryItem("kept", "Find", 7));
            player.Wallet.TryCredit(12);
            player.OpenMenu(PlayerMenu.Pause);
            yield return null;
            player.ShowCameraComfort();
            yield return null; yield return null;
            var page = MenuTestUI.View(player).CurrentScreen;
            var controls = page.Query().ToList().ToArray();
            var slider = page.Q<SliderInt>("fovSlider");
            slider.value = 55;
            player.CameraSettings.SetSteadyCrosshair(false);
            Assert.That(preferences.Writes, Is.Zero);
            var reset = page.Q<UnityEngine.UIElements.Button>("cameraReset");
            reset.Focus();
            preferences.Fail = true;
            yield return Key(keyboard.enterKey);
            Assert.That(player.ViewCamera.fieldOfView, Is.EqualTo(75));
            Assert.That(player.CameraSettings.SteadyCrosshair, Is.True);
            Assert.That(player.CameraSettings.WriteFailed, Is.True);
            Assert.That(MenuTestUI.View(player).Focused, Is.SameAs(reset));
            Assert.That(page.Query().ToList(), Is.EquivalentTo(controls));
            Assert.That(page.Q("settingsError").ClassListContains("hidden"), Is.False);
            Assert.That(player.Inventory.Count, Is.EqualTo(1));
            Assert.That(player.Wallet.Balance, Is.EqualTo(12));
            preferences.Fail = false;
            var retry = page.Q<UnityEngine.UIElements.Button>("settingsRetry");
            retry.Focus();
            yield return Key(keyboard.enterKey);
            Assert.That(player.CameraSettings.WriteFailed, Is.False);
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("cameraBack"));
            int writes = preferences.Writes, redraws = 0;
            var label = page.Q<Label>("fovValue");
            yield return null; yield return null;
            EventCallback<GeometryChangedEvent> dirty = e => redraws++;
            label.RegisterCallback(dirty);
            for (int i = 0; i < 10; i++) yield return null;
            label.UnregisterCallback(dirty);
            Assert.That(preferences.Writes, Is.EqualTo(writes));
            Assert.That(redraws, Is.Zero);
        }

        [UnityTest]
        public IEnumerator CameraMouseSliderAndSteadyCrosshairPreserveActualCenterActions()
        {
            target.AddComponent<ValidationDigTarget>();
            yield return null; yield return null;
            var reticle = root.GetComponent<FpsHud>().View.Root.Q<Label>("Reticle");
            Vector2 center = reticle.worldBound.center;
            foreach (int fov in new[] { 55, 75, 90 })
            {
                player.CameraSettings.SetVerticalFov(fov);
                Assert.That(player.TryDig(), Is.True);
                yield return null;
                Assert.That(reticle.worldBound.center, Is.EqualTo(center));
                Assert.That(reticle.resolvedStyle.scale.value, Is.EqualTo(Vector3.one));
                Assert.That(reticle.resolvedStyle.color, Is.EqualTo(Color.white));
            }
            player.CameraSettings.SetSteadyCrosshair(false);
            yield return null;
            Assert.That(reticle.resolvedStyle.scale.value.x, Is.GreaterThan(1));
            Assert.That(reticle.resolvedStyle.color, Is.Not.EqualTo(Color.white));
            player.CameraSettings.SetSteadyCrosshair(true);
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.4f);
            Assert.That(player.IsJetpackActive, Is.True);
            Assert.That(reticle.resolvedStyle.scale.value, Is.EqualTo(Vector3.one));
            player.OpenMenu(PlayerMenu.Pause);
            yield return null;
            player.ShowCameraComfort();
            yield return null; yield return null;
            var slider = MenuTestUI.View(player).Root.Q<SliderInt>("fovSlider");
            devices.Set(mouse.position, MenuTestUI.ScreenPoint(player, slider, 0.25f), queueEventOnly: true);
            yield return null;
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return null; yield return null;
            Assert.That(player.ViewCamera.fieldOfView, Is.InRange(62, 65));
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.CameraComfort));
        }

        private IEnumerator Key(UnityEngine.InputSystem.Controls.ButtonControl key)
        {
            devices.Press(key, queueEventOnly: true);
            yield return null; yield return null;
            devices.Release(key, queueEventOnly: true);
            yield return null; yield return null;
        }

        [UnityTest]
        public IEnumerator HudRetainsControlsAndCenteredLayoutAcrossScaleAndMenuChanges()
        {
            yield return null; yield return null;
            var hud = root.GetComponent<FpsHud>().View.Root;
            var status = hud.Q<Label>("Status");
            var reticle = hud.Q<Label>("Reticle");
            var panel = root.GetComponentInChildren<UIDocument>().panelSettings;
            var content = status.text;
            try
            {
                foreach (var size in new[] { new Vector2Int(960, 540), new Vector2Int(1920, 1080), new Vector2Int(1280, 800) })
                {
                    panel.referenceResolution = size;
                    yield return null; yield return null;
                    Assert.That(hud.worldBound.Contains(status.worldBound.min), Is.True);
                    Assert.That(hud.worldBound.Contains(status.worldBound.max), Is.True);
                    Assert.That(Vector2.Distance(reticle.worldBound.center, hud.worldBound.center), Is.LessThan(0.1f));
                    Assert.That(status.text, Is.EqualTo(content));
                    Assert.That(hud.Q<Label>("Status"), Is.SameAs(status));
                    Assert.That(hud.Query<VisualElement>().ToList().All(e => e.pickingMode == PickingMode.Ignore), Is.True);
                }
                player.OpenMenu(PlayerMenu.Pause);
                yield return null; yield return null;
                Assert.That(hud.resolvedStyle.display, Is.EqualTo(DisplayStyle.None));
                player.CloseMenu();
                yield return null; yield return null;
                Assert.That(hud.resolvedStyle.display, Is.EqualTo(DisplayStyle.Flex));
                int layouts = 0;
                EventCallback<GeometryChangedEvent> changed = e => layouts++;
                status.RegisterCallback(changed);
                for (int i = 0; i < 5; i++) yield return null;
                status.UnregisterCallback(changed);
                Assert.That(layouts, Is.Zero, "Unchanged HUD content should retain its layout.");
            }
            finally { panel.referenceResolution = new Vector2Int(1280, 720); }
        }

        [UnityTest]
        public IEnumerator ReturnWarningsShowReserveBandsAndFreezeAcrossInventoryInspection()
        {
            var batteryLabel = root.GetComponent<FpsHud>().View.Root.Q<Label>("Battery status");
            var warning = root.GetComponent<FpsHud>().View.Root.Q<Label>("Return warning");
            StringAssert.Contains("SAFE", batteryLabel.text);
            player.Battery.TrySpend(65);
            yield return null;
            StringAssert.Contains("RISKY", batteryLabel.text);
            Assert.That(warning.text, Is.EqualTo("RESERVE RUNNING LOW"));
            devices.Press(keyboard.tabKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(root.GetComponent<FpsHud>().View.Root.ClassListContains("hidden"), Is.True);
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
            Assert.That(root.GetComponent<FpsHud>().View.Root.ClassListContains("hidden"), Is.False);
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
            var view = MenuTestUI.View(player);
            StringAssert.Contains("Carried finds: 10 / 10", MenuTestUI.Text(player, "menuSubtitle"));
            var rows = view.CurrentScreen.Query(className: "item-row").ToList();
            Assert.That(rows.Count, Is.EqualTo(10));
            for (int i = 0; i < carried.Length; i++)
            {
                Assert.That(rows[i].Q<Label>("Find name").text, Is.EqualTo("Coin"));
                Assert.That(rows[i].Q<Label>("Sale value").text, Is.EqualTo("Sale value: " + carried[i].SaleValue));
            }
            Assert.That(view.CurrentScreen.Query<UnityEngine.UIElements.Button>().ToList().Select(b => b.name), Is.EqualTo(new[] { "Close" }));
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
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("Sell All"));
            devices.Press(keyboard.downArrowKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("Sell first item"));
            devices.Press(keyboard.enterKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Inventory.Items, Is.EqualTo(new[] { second }));
            var rows = MenuTestUI.View(player).CurrentScreen.Query(className: "item-row").ToList();
            Assert.That(rows.Count, Is.EqualTo(1));
            Assert.That(rows[0].Q<Label>("Find name").text, Is.EqualTo("Coin"));
            Assert.That(rows[0].Q<Label>("Sale value").text, Is.EqualTo("Sale value: 17"));
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
        public IEnumerator HeldCrouchUsesLoweredTargetingAndSurvivesMenuAndDeviceRecovery()
        {
            target.transform.position = new Vector3(0, 1, 2);
            var dig = target.AddComponent<ValidationDigTarget>();
            devices.Press(keyboard.leftCtrlKey, queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.3f);
            Assert.That(player.CrouchAmount, Is.EqualTo(1));
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.1f);
            Assert.That(dig.HitsRemaining, Is.EqualTo(2));
            player.OpenMenu(PlayerMenu.Pause);
            yield return null;
            Assert.That(MenuTestUI.View(player).Root.Query<Label>().ToList()
                .Any(label => label.text == "Hold to crouch / move carefully"), Is.True);
            Assert.That(MenuTestUI.View(player).Root.Query<Label>().ToList()
                .Any(label => label.text == player.InputSettings.Display(PlayerBinding.Crouch)), Is.True);
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            yield return null;
            float charge = player.Battery.Charge;
            player.SetApplicationFocus(false);
            InputSystem.RemoveDevice(keyboard);
            keyboard = InputSystem.AddDevice<Keyboard>();
            devices.Press(keyboard.leftCtrlKey, queueEventOnly: true);
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            yield return null;
            player.SetApplicationFocus(true);
            player.CloseMenu();
            yield return new WaitForSecondsRealtime(0.3f);
            Assert.That(player.CrouchAmount, Is.EqualTo(1));
            Assert.That(player.IsJetpackActive, Is.False);
            Assert.That(dig.HitsRemaining, Is.EqualTo(2));
            Assert.That(player.Battery.Charge, Is.EqualTo(charge));
            player.OpenMenu(PlayerMenu.Pause);
            devices.Release(keyboard.leftCtrlKey, queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.25f);
            Assert.That(player.CrouchAmount, Is.EqualTo(1), "Paused stance does not animate on key release.");
            player.CloseMenu();
            yield return new WaitForSecondsRealtime(0.3f);
            Assert.That(player.CrouchAmount, Is.Zero);
        }

        [UnityTest]
        public IEnumerator ControlsCaptureBlocksSubmitBackMovementAndDigThenUpdatesPauseLabels()
        {
            var dig = target.AddComponent<ValidationDigTarget>();
            player.OpenMenu(PlayerMenu.Pause); yield return null;
            MenuTestUI.Click(MenuTestUI.Button(player, "Controls")); yield return null; yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.InputSettings));
            var page = MenuTestUI.View(player).CurrentScreen;
            var bind = page.Q<UnityEngine.UIElements.Button>("bindDig");
            bind.Focus(); yield return Key(keyboard.enterKey);
            Assert.That(player.BindingCapture.State, Is.EqualTo(BindingCaptureState.Listening));
            // Enter is now a binding candidate, never a second click on the focused row.
            yield return Key(keyboard.enterKey);
            Assert.That(player.InputSettings.Path(PlayerBinding.Dig), Is.EqualTo("<Keyboard>/enter"));
            Assert.That(player.BindingCapture.State, Is.EqualTo(BindingCaptureState.Idle));
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.InputSettings));
            float charge = player.Battery.Charge; var position = player.transform.position;
            MenuTestUI.Click(page.Q<UnityEngine.UIElements.Button>("digMode"));
            Assert.That(player.InputSettings.ToggleDig, Is.True);
            player.InputSettings.Bind(PlayerBinding.Dig, "<Mouse>/rightButton", true);
            devices.Press(keyboard.wKey, queueEventOnly: true); devices.Press(mouse.rightButton, queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.4f);
            Assert.That(player.Battery.Charge, Is.EqualTo(charge)); Assert.That(player.transform.position, Is.EqualTo(position));
            Assert.That(dig.HitsRemaining, Is.EqualTo(3));
            devices.Release(keyboard.wKey, queueEventOnly: true); devices.Release(mouse.rightButton, queueEventOnly: true);
            yield return null; yield return null;
            player.BackFromInputSettings(); yield return null; yield return null;
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("Controls"));
            Assert.That(MenuTestUI.Text(player, "controlDig"), Is.EqualTo(player.InputSettings.Display(PlayerBinding.Dig)));
            Assert.That(MenuTestUI.Text(player, "controlDigDescription"), Is.EqualTo("Toggle dig / collect; click to throw held find"));
            Assert.That(MenuTestUI.Text(player, "controlGrab"), Is.EqualTo(player.InputSettings.Display(PlayerBinding.Grab)));
            Assert.That(new InputPreferences(inputPreferences).ToggleDig, Is.True);
            player.CloseMenu(); yield return null; yield return null;
            yield return new WaitForSecondsRealtime(0.4f); Assert.That(dig.HitsRemaining, Is.EqualTo(3));
            yield return Key(mouse.rightButton);
            yield return new WaitForSecondsRealtime(0.4f); Assert.That(dig.HitsRemaining, Is.LessThan(3));
        }

        [UnityTest]
        public IEnumerator ControlsConflictCancelReplaceResetAndWriteRetryPreserveWorldAndCamera()
        {
            player.Inventory.TryAdd(new InventoryItem("kept", "Find", 7)); player.Wallet.TryCredit(12);
            player.CameraSettings.SetVerticalFov(80);
            player.OpenMenu(PlayerMenu.Pause); yield return null; player.ShowInputSettings(); yield return null; yield return null;
            var page = MenuTestUI.View(player).CurrentScreen;
            MenuTestUI.Click(page.Q<UnityEngine.UIElements.Button>("bindDig")); yield return null;
            yield return Key(keyboard.spaceKey);
            Assert.That(player.BindingCapture.State, Is.EqualTo(BindingCaptureState.Conflict));
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("bindingCancel"));
            yield return Key(keyboard.enterKey); Assert.That(player.InputSettings.Path(PlayerBinding.Dig), Is.EqualTo("<Mouse>/leftButton"));
            MenuTestUI.Click(page.Q<UnityEngine.UIElements.Button>("bindDig")); yield return null;
            yield return Key(keyboard.spaceKey);
            MenuTestUI.Click(page.Q<UnityEngine.UIElements.Button>("bindingReplace")); yield return null; yield return null;
            Assert.That(player.InputSettings.Path(PlayerBinding.Dig), Is.EqualTo("<Keyboard>/space"));
            Assert.That(player.InputSettings.Path(PlayerBinding.Jump), Is.EqualTo("<Mouse>/leftButton"));
            inputPreferences.Fail = true;
            MenuTestUI.Click(page.Q<UnityEngine.UIElements.Button>("inputReset")); yield return null;
            Assert.That(player.InputSettings.WriteFailed, Is.True);
            Assert.That(page.Q("inputSettingsError").ClassListContains("hidden"), Is.False);
            Assert.That(player.InputSettings.Path(PlayerBinding.Dig), Is.EqualTo("<Mouse>/leftButton"));
            inputPreferences.Fail = false;
            MenuTestUI.Click(page.Q<UnityEngine.UIElements.Button>("inputRetry")); yield return null;
            Assert.That(player.InputSettings.WriteFailed, Is.False);
            Assert.That(player.CameraSettings.VerticalFov, Is.EqualTo(80)); Assert.That(player.Inventory.Count, Is.EqualTo(1)); Assert.That(player.Wallet.Balance, Is.EqualTo(12));
        }

        [UnityTest]
        public IEnumerator ToggleClearsOnFocusAndModeChangesWithoutAutonomousResume()
        {
            var dig = target.AddComponent<ValidationDigTarget>();
            player.InputSettings.Bind(PlayerBinding.Dig, "<Keyboard>/q"); player.InputSettings.SetToggleDig(true);
            yield return null; yield return null;
            yield return Key(keyboard.qKey); Assert.That(dig.HitsRemaining, Is.EqualTo(2));
            player.SetApplicationFocus(false); yield return null; player.SetApplicationFocus(true); player.CloseMenu();
            yield return new WaitForSecondsRealtime(0.5f);
            Assert.That(dig.HitsRemaining, Is.EqualTo(2));
            yield return Key(keyboard.qKey); Assert.That(dig.HitsRemaining, Is.EqualTo(1));
            player.InputSettings.SetToggleDig(false);
            yield return new WaitForSecondsRealtime(0.5f); Assert.That(dig.HitsRemaining, Is.EqualTo(1));
            player.InputSettings.SetToggleDig(true);
            yield return new WaitForSecondsRealtime(0.5f); Assert.That(dig.HitsRemaining, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator WorldRestoreRetainsPreferencesAndClearsActiveToggle()
        {
            var dig = target.AddComponent<ValidationDigTarget>();
            player.InputSettings.SetToggleDig(true); player.InputSettings.Bind(PlayerBinding.Dig, "<Keyboard>/q");
            yield return null; yield return null;
            yield return Key(keyboard.qKey); Assert.That(dig.HitsRemaining, Is.EqualTo(2));
            var snapshot = new WorldSnapshot(); player.Capture(snapshot); player.Restore(snapshot);
            yield return new WaitForSecondsRealtime(0.5f);
            Assert.That(dig.HitsRemaining, Is.EqualTo(2)); Assert.That(player.InputSettings.ToggleDig, Is.True);
            yield return Key(keyboard.qKey); Assert.That(dig.HitsRemaining, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator CaptureEscapeAndFocusLossCancelWithoutClosingControls()
        {
            player.OpenMenu(PlayerMenu.Pause); yield return null; player.ShowInputSettings(); yield return null; yield return null;
            var page = MenuTestUI.View(player).CurrentScreen;
            MenuTestUI.Click(page.Q<UnityEngine.UIElements.Button>("bindPause")); yield return null;
            yield return Key(keyboard.escapeKey);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.InputSettings));
            Assert.That(player.InputSettings.Path(PlayerBinding.Pause), Is.EqualTo("<Keyboard>/escape"));
            MenuTestUI.Click(page.Q<UnityEngine.UIElements.Button>("bindPause")); yield return null;
            player.SetApplicationFocus(false); yield return null; player.SetApplicationFocus(true); yield return null;
            Assert.That(player.BindingCapture.State, Is.EqualTo(BindingCaptureState.Idle));
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.InputSettings));
            yield return Key(keyboard.escapeKey); Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
        }

        [UnityTest]
        public IEnumerator PauseReboundToMouseOrEnterStillAllowsMenuButtonsAndCannotImmediatelyResume()
        {
            player.OpenMenu(PlayerMenu.Pause); yield return null; yield return null;
            var point = MenuTestUI.ScreenPoint(player, MenuTestUI.Button(player, "Resume"));
            player.CloseMenu();
            player.InputSettings.Bind(PlayerBinding.Pause, "<Mouse>/leftButton", true);
            yield return null; yield return null;
            devices.Set(mouse.position, point, queueEventOnly: true); yield return null;
            yield return Key(mouse.leftButton);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause), "The press opening Pause cannot click Resume.");
            yield return Key(keyboard.enterKey);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.None));
            player.InputSettings.Bind(PlayerBinding.Pause, "<Keyboard>/enter"); yield return null; yield return null;
            yield return Key(keyboard.enterKey);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
            yield return Key(keyboard.downArrowKey); yield return Key(keyboard.enterKey);
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.CameraComfort), "Enter belongs to menu submit while a menu is open.");
        }

        [UnityTest]
        public IEnumerator PointerCanResumePauseWithoutDiggingUnderneath()
        {
            var dig = target.AddComponent<ValidationDigTarget>();
            devices.Press(keyboard.escapeKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
            var resume = MenuTestUI.Button(player, "Resume");
            var screen = MenuTestUI.ScreenPoint(player, resume);
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
