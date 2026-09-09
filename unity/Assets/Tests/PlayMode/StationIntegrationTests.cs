#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SomethingDownThere.Tests
{
    public sealed class StationIntegrationTests
    {
        private Scene scene;
        private FpsPlayer player;
        private SellStation sell;
        private UpgradeStation upgrade;
        private InputTestFixture devices;
        private Keyboard keyboard;
        private Mouse mouse;
        private float oldTimeScale;
        private CursorLockMode oldCursor;
        private bool oldCursorVisible;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            oldTimeScale = Time.timeScale; oldCursor = Cursor.lockState; oldCursorVisible = Cursor.visible;
            devices = new InputTestFixture(); devices.Setup();
            keyboard = InputSystem.AddDevice<Keyboard>(); mouse = InputSystem.AddDevice<Mouse>();
            Time.timeScale = 1;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/MainGame.unity", new LoadSceneParameters(LoadSceneMode.Additive));
            scene = SceneManager.GetSceneByPath("Assets/Scenes/MainGame.unity");
            var root = scene.GetRootGameObjects().Single();
            player = root.GetComponentInChildren<FpsPlayer>();
            sell = root.GetComponentInChildren<SellStation>();
            upgrade = root.GetComponentInChildren<UpgradeStation>();
            player.Tuning.Gravity = 0;
            yield return null;
            yield return null;
            player.SetApplicationFocus(true);
            if (player.IsMenuOpen) player.CloseMenu();
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (scene.IsValid()) yield return SceneManager.UnloadSceneAsync(scene);
            devices.TearDown();
            Time.timeScale = oldTimeScale; Cursor.lockState = oldCursor; Cursor.visible = oldCursorVisible;
        }

        [UnityTest]
        public IEnumerator RealStationInputSellsSelectedIdentitiesAndRejectsOldButtons()
        {
            var first = new InventoryItem("first", "Coin", 5);
            var second = new InventoryItem("second", "Coin", 17);
            player.Inventory.TryAdd(first); player.Inventory.TryAdd(second);
            player.OpenMenu(PlayerMenu.Inventory);
            Assert.That(player.ExecuteStationCommand(0), Is.False);
            Assert.That(player.Inventory.Count, Is.EqualTo(2));
            player.CloseMenu();
            yield return null;
            Face(sell);
            devices.Press(keyboard.eKey, queueEventOnly: true);
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Station, Is.SameAs(sell));
            Assert.That(player.Wallet.Balance, Is.Zero);
            Assert.That(player.Inventory.Count, Is.EqualTo(2));
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("Close station"));
            var oldClick = Button("Sell second");
            MenuTestUI.Click(oldClick);
            MenuTestUI.Click(oldClick);
            yield return null;
            Assert.That(player.Inventory.Items, Is.EqualTo(new[] { first }));
            Assert.That(player.Wallet.Balance, Is.EqualTo(17));
            MenuTestUI.Click(oldClick); // The displayed row has since been replaced.
            Assert.That(player.Inventory.Items, Is.EqualTo(new[] { first }));
            StringAssert.Contains("Sold Coin", Text("Trade result"));
            MenuTestUI.Click(Button("Sell all"));
            yield return null;
            Assert.That(player.Inventory.Count, Is.Zero);
            Assert.That(player.Wallet.Balance, Is.EqualTo(22));
            Assert.That(Button("Sell all").enabledSelf, Is.False);
            devices.Release(keyboard.eKey, queueEventOnly: true);
            MenuTestUI.Click(Button("Close station"));
            yield return null;
            Assert.That(player.GameplayActive, Is.True);
            Assert.That(player.SuccessfulStrokes, Is.Zero, "Held LMB must not leak out of the station.");
        }

        [UnityTest]
        public IEnumerator UpgradeDisplaysOwnedStatsChargesOnceAndSurvivesSurfaceTrips()
        {
            player.Wallet.TryCredit(10);
            Face(upgrade);
            Assert.That(player.TryInteract(), Is.True);
            yield return null;
            yield return null;
            StringAssert.Contains("0.82 m", Text("Upgrade comparison"));
            StringAssert.Contains("1.04 m", Text("Upgrade comparison"));
            Assert.That(player.Shovel.Level, Is.EqualTo(1));
            var click = Button("Buy upgrade");
            MenuTestUI.Click(click); MenuTestUI.Click(click);
            yield return null;
            Assert.That(player.Shovel.Level, Is.EqualTo(2));
            Assert.That(player.Wallet.Balance, Is.Zero);
            Assert.That(Button("Buy upgrade").enabledSelf, Is.False);
            StringAssert.Contains("Need 25 more", Text("Upgrade cost"));
            MenuTestUI.Click(click);
            Assert.That(player.Wallet.Balance, Is.Zero);
            MenuTestUI.Click(Button("Close station"));
            yield return null;
            Place(new Vector3(0, 0.1f, -11.5f));
            player.transform.rotation = Quaternion.identity;
            player.Tick(new FpsInputFrame { Look = new Vector2(0, (player.Pitch - 85) / player.Tuning.LookSensitivity) }, 0.016f);
            Assert.That(player.TryDig(), Is.True);
            float removed = player.ExcavatedVolume;
            Assert.That(removed, Is.GreaterThan(0));
            player.Battery.TrySpend(player.Battery.Charge);
            Place(player.SurfaceRecharge.transform.position + Vector3.up * 0.1f);
            Assert.That(player.SurfaceRecharge.TryRecharge(), Is.True);
            Assert.That(player.Shovel.Level, Is.EqualTo(2));
            Assert.That(player.Wallet.Balance, Is.Zero);
            Assert.That(player.ExcavatedVolume, Is.EqualTo(removed));
        }

        [UnityTest]
        public IEnumerator PausedStationRevalidatesFocusRangeDisabledStateAndDisplayedContents()
        {
            player.Inventory.TryAdd(new InventoryItem("a", "Marble", 5));
            Face(sell);
            Assert.That(player.TryInteract(), Is.True);
            yield return null;
            long displayed = player.StationRevision;
            player.Inventory.TryAdd(new InventoryItem("b", "Token", 9));
            Assert.That(player.ExecuteStationCommand(0, displayed), Is.False);
            Assert.That(player.Inventory.Count, Is.EqualTo(2));
            Assert.That(player.Wallet.Balance, Is.Zero);
            yield return null;
            StringAssert.Contains("Offer changed", Text("Trade result"));
            player.SetApplicationFocus(false);
            Assert.That(player.ExecuteStationCommand(0), Is.False);
            player.SetApplicationFocus(true);
            sell.enabled = false;
            Assert.That(player.ExecuteStationCommand(0), Is.False);
            sell.enabled = true;
            Place(player.transform.position + Vector3.forward * 8);
            Assert.That(player.ExecuteStationCommand(0), Is.False);
            Assert.That(sell.TryExecute(0, player), Is.False);
            Assert.That(player.Inventory.Count, Is.EqualTo(2));
            Assert.That(player.Wallet.Balance, Is.Zero);
            player.CloseMenu();
            player.OpenStation(sell);
            Assert.That(player.IsMenuOpen, Is.False, "A remote station cannot be opened directly.");
        }

        [UnityTest]
        public IEnumerator FullBagRowsScrollWithKeyboardAndResizeWithoutHidingTheFooter()
        {
            for (int i = 0; i < player.Inventory.Capacity; i++) player.Inventory.TryAdd(new InventoryItem("row-" + i, "Coin " + i, i + 1));
            Face(sell);
            Assert.That(player.TryInteract(), Is.True);
            yield return null;
            yield return null;
            var scroll = MenuTestUI.View(player).Root.Q<ScrollView>("menuScroll");
            Assert.That(scroll.contentContainer.layout.height, Is.GreaterThan(scroll.contentViewport.layout.height));
            Vector3 wheelDelta = Vector3.zero;
            scroll.RegisterCallback<WheelEvent>(e => wheelDelta = e.delta, TrickleDown.TrickleDown);
            devices.Set(mouse.position, MenuTestUI.ScreenPoint(player, scroll.contentViewport), queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.1f);
            devices.Set(mouse.scroll, new Vector2(0, -120), queueEventOnly: true);
            yield return new WaitForSecondsRealtime(0.15f);
            Assert.That(scroll.scrollOffset.y, Is.GreaterThan(0), "Mouse wheel must reach the active Toolkit list: " + wheelDelta);
            scroll.scrollOffset = Vector2.zero;
            // Close -> Sell All -> last item. Selection must bring that row into view.
            for (int i = 0; i < 2; i++)
            {
                devices.Press(keyboard.upArrowKey, queueEventOnly: true);
                yield return null;
                yield return null;
                devices.Release(keyboard.upArrowKey, queueEventOnly: true);
                yield return null;
            }
            Assert.That(MenuTestUI.Focused(player), Is.EqualTo("Sell row-9"));
            Assert.That(scroll.scrollOffset.y, Is.GreaterThan(0));
            var row = Button("Sell row-9").worldBound;
            Assert.That(row.yMax, Is.LessThanOrEqualTo(scroll.contentViewport.worldBound.yMax + 1));
            Assert.That(row.yMin, Is.GreaterThanOrEqualTo(scroll.contentViewport.worldBound.yMin - 1));
            devices.Press(keyboard.enterKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Wallet.Balance, Is.EqualTo(10));
            Assert.That(player.Inventory.Items.Any(i => i.InstanceId == "row-9"), Is.False);
            var close = Button("Close station");
            Assert.That(close.enabledInHierarchy, Is.True);
            Assert.That(close.worldBound.yMin, Is.GreaterThanOrEqualTo(scroll.worldBound.yMax));
            Assert.That(close.worldBound.yMax, Is.LessThan(MenuTestUI.View(player).Root.worldBound.yMax));

        }

        private void Face(StationTarget station)
        {
            Place(station.transform.position + new Vector3(0, 0.1f, 2.4f));
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
            player.Tick(new FpsInputFrame { Look = new Vector2(0, (player.Pitch - 12) / player.Tuning.LookSensitivity) }, 0.016f);
            Physics.SyncTransforms();
        }

        private void Place(Vector3 position)
        {
            var motor = player.GetComponent<CharacterController>();
            motor.enabled = false; player.transform.position = position; motor.enabled = true;
            Physics.SyncTransforms();
        }

        private Button Button(string name) => MenuTestUI.Button(player, name);
        private string Text(string name) => MenuTestUI.Text(player, name);
    }
}
#endif
