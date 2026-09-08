#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace SomethingDownThere.Tests
{
    public sealed class RescueIntegrationTests
    {
        private Scene scene;
        private FpsPlayer player;
        private TerrainVolume terrain;
        private Transform landing;
        private InputTestFixture devices;
        private Keyboard keyboard;
        private Mouse mouse;
        private float oldTimeScale;
        private CursorLockMode oldCursor;
        private bool oldCursorVisible;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            oldTimeScale = Time.timeScale;
            oldCursor = Cursor.lockState;
            oldCursorVisible = Cursor.visible;
            devices = new InputTestFixture();
            devices.Setup();
            keyboard = InputSystem.AddDevice<Keyboard>();
            mouse = InputSystem.AddDevice<Mouse>();
            Time.timeScale = 1;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/MainGame.unity", new LoadSceneParameters(LoadSceneMode.Additive));
            scene = SceneManager.GetSceneByPath("Assets/Scenes/MainGame.unity");
            var root = scene.GetRootGameObjects()[0];
            player = root.GetComponentInChildren<FpsPlayer>();
            terrain = root.GetComponentInChildren<TerrainVolume>();
            landing = root.transform.Find("Surface/ReturnAnchor");
            player.SetApplicationFocus(true);
            if (player.IsMenuOpen) player.CloseMenu();
            player.Tuning.Gravity = 0;
            Place(new Vector3(-8, 3, 0));
            yield return null;
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (scene.IsValid()) yield return SceneManager.UnloadSceneAsync(scene);
            devices.TearDown();
            Time.timeScale = oldTimeScale;
            Cursor.lockState = oldCursor;
            Cursor.visible = oldCursorVisible;
        }

        [UnityTest]
        public IEnumerator CancelAndConfirmUseRealMenuInputAndPreserveTerrainAndCollectedIdentities()
        {
            var find = CollectFirstFind();
            Place(new Vector3(-8, 3, 0));
            player.Wallet.TryCredit(25);
            player.Shovel.TryUpgradeTo(2);
            player.Battery.TrySpend(player.Battery.Charge);
            int revision = terrain.Revision;
            float volume = terrain.RemovedVolume;
            Vector3 before = player.transform.position;
            devices.Press(keyboard.escapeKey, queueEventOnly: true);
            yield return null;
            yield return null;
            devices.Release(keyboard.escapeKey, queueEventOnly: true);
            Button("Call rescue...").onClick.Invoke();
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.ConfirmRescue));
            Assert.That(EventSystem.current.currentSelectedGameObject.name, Is.EqualTo("Cancel"));
            StringAssert.Contains("Rescue fee: 10 credits", Body.text);
            StringAssert.Contains(find.Item.DisplayName, Body.text);
            devices.Press(keyboard.enterKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
            Assert.That(player.transform.position, Is.EqualTo(before));
            Assert.That(player.Inventory.Items.Single(), Is.SameAs(find.Item));
            Assert.That(player.Wallet.Balance, Is.EqualTo(25));
            Assert.That(player.Battery.Charge, Is.Zero);
            devices.Release(keyboard.enterKey, queueEventOnly: true);
            Button("Call rescue...").onClick.Invoke();
            yield return null;
            yield return null;
            devices.Press(keyboard.downArrowKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(EventSystem.current.currentSelectedGameObject.name, Is.EqualTo("Confirm rescue"));
            devices.Press(keyboard.enterKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.None));
            Assert.That(Vector3.Distance(player.transform.position, landing.position), Is.LessThan(0.01f));
            Assert.That(player.Inventory.Count, Is.Zero);
            Assert.That(player.Wallet.Balance, Is.EqualTo(15));
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            Assert.That(player.Shovel.Level, Is.EqualTo(2));
            Assert.That(terrain.Revision, Is.EqualTo(revision));
            Assert.That(terrain.RemovedVolume, Is.EqualTo(volume));
            Assert.That(player.ConfirmRescue(), Is.False);
            Assert.That(player.Wallet.Balance, Is.EqualTo(15));
            Assert.That(find.Collected, Is.True);
            Assert.That(find.gameObject.activeSelf, Is.False);
            terrain.ResetExcavation();
            Assert.That(find.Collected && !find.gameObject.activeSelf, Is.True, "Lost collected loot must never respawn.");
        }

        [UnityTest]
        public IEnumerator EmptyBatteryAndWalletRescueRestoresControlWithoutLeakingHeldActions()
        {
            player.Battery.TrySpend(100);
            devices.Press(mouse.leftButton, queueEventOnly: true);
            devices.Press(keyboard.spaceKey, queueEventOnly: true);
            yield return null;
            player.OpenMenu(PlayerMenu.Pause);
            player.RequestRescue();
            yield return null;
            Assert.That(player.Rescue.Quote.Fee, Is.Zero);
            Assert.That(player.ConfirmRescue(), Is.True);
            yield return new WaitForSecondsRealtime(0.35f);
            Assert.That(player.GameplayActive, Is.True);
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            Assert.That(player.Wallet.Balance, Is.Zero);
            Assert.That(player.SuccessfulStrokes, Is.Zero);
            Assert.That(player.IsJetpackActive, Is.False);
            Assert.That(player.VerticalSpeed, Is.Zero);
            devices.Release(mouse.leftButton, queueEventOnly: true);
            devices.Release(keyboard.spaceKey, queueEventOnly: true);
            yield return null;
            devices.Press(keyboard.wKey, queueEventOnly: true);
            var start = player.transform.position;
            yield return new WaitForSecondsRealtime(0.15f);
            Assert.That(Vector3.Distance(player.transform.position, start), Is.GreaterThan(0.1f));
        }

        [UnityTest]
        public IEnumerator ChangedQuoteNeedsFreshConfirmationAndEscapeAndFocusAreSafe()
        {
            player.Inventory.TryAdd(new InventoryItem("review-a", "Marble", 5));
            player.Wallet.TryCredit(25);
            player.RequestRescue();
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.None), "Rescue can only be requested from Pause.");
            player.OpenMenu(PlayerMenu.Pause);
            player.RequestRescue();
            player.Inventory.TryAdd(new InventoryItem("review-b", "Bead", 11));
            Assert.That(player.ConfirmRescue(), Is.False);
            yield return null;
            StringAssert.Contains("updated cost", Body.text);
            StringAssert.Contains("Carried finds lost: 2", Body.text);
            Assert.That(player.Inventory.Count, Is.EqualTo(2));
            Assert.That(player.Wallet.Balance, Is.EqualTo(25));
            player.SetApplicationFocus(false);
            Assert.That(player.ConfirmRescue(), Is.False);
            player.SetApplicationFocus(true);
            yield return null;
            devices.Press(keyboard.escapeKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(player.Menu, Is.EqualTo(PlayerMenu.Pause));
            Assert.That(player.Rescue.Quote, Is.Null);
            Assert.That(player.ConfirmRescue(), Is.False);
            Assert.That(player.Inventory.Count, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator BlockedLandingNeverDiscardsItemsOrChargesAndDisablingCancelsTheQuote()
        {
            player.Inventory.TryAdd(new InventoryItem("review-a", "Marble", 5));
            player.Wallet.TryCredit(25);
            var original = landing.position;
            landing.position = new Vector3(0, -1, 0); // Actual undug terrain blocks the landing.
            player.OpenMenu(PlayerMenu.Pause);
            player.RequestRescue();
            Assert.That(player.ConfirmRescue(), Is.False);
            yield return null;
            StringAssert.Contains("landing area is blocked", Body.text);
            Assert.That(player.Inventory.Count, Is.EqualTo(1));
            Assert.That(player.Wallet.Balance, Is.EqualTo(25));
            landing.position = original;
            player.enabled = false;
            Assert.That(player.Rescue.Quote, Is.Null);
            Assert.That(player.ConfirmRescue(), Is.False);
            player.enabled = true;
            yield return null;
            player.OpenMenu(PlayerMenu.Pause);
            player.RequestRescue();
            Assert.That(player.ConfirmRescue(), Is.True);
        }

        private Text Body => player.GetComponentsInChildren<Text>().Single(t => t.name == "Body");
        private Button Button(string name) => player.GetComponentsInChildren<Button>().Single(b => b.name == name);

        private void Place(Vector3 position)
        {
            var motor = player.GetComponent<CharacterController>();
            motor.enabled = false;
            player.transform.position = position;
            motor.enabled = true;
            Physics.SyncTransforms();
        }

        private BuriedFind CollectFirstFind()
        {
            var find = player.Discoveries.Finds[0];
            float ring = Mathf.Max(find.WorldBounds.extents.x, find.WorldBounds.extents.z) + 0.12f;
            for (int pass = 0; pass < 12 && !find.Collectible; pass++)
                foreach (var offset in new[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back })
                {
                    if (find.Collectible) break;
                    Vector3 origin = find.transform.position + offset * ring;
                    origin.y = 2;
                    Assert.That(Physics.Raycast(origin, Vector3.down, out var hit, 30, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore), Is.True);
                    Assert.That(terrain.TryDig(hit, 0.65f), Is.True);
                }
            player.ViewCamera.transform.position = find.transform.position + Vector3.up * 1.5f;
            player.ViewCamera.transform.LookAt(find.transform.position);
            Physics.SyncTransforms();
            Assert.That(find.TryCollect(player), Is.True,
                $"Collection setup: menu={player.Menu}, active={player.GameplayActive}, exposure={find.Exposure}, required={find.RequiredExposure}, timeScale={Time.timeScale}");
            player.ViewCamera.transform.localPosition = Vector3.up * 1.6f;
            player.ViewCamera.transform.localRotation = Quaternion.Euler(player.Pitch, 0, 0);
            return find;
        }
    }
}
#endif
