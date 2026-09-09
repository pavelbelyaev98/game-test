#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SomethingDownThere.Tests
{
    public sealed class SurfaceRechargeTests
    {
        private Scene scene;
        private FpsPlayer player;
        private SurfaceRecharge recharge;
        private InputTestFixture devices;
        private float previousTimeScale;
        private CursorLockMode previousCursor;
        private bool previousCursorVisible;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            previousTimeScale = Time.timeScale;
            previousCursor = Cursor.lockState;
            previousCursorVisible = Cursor.visible;
            devices = new InputTestFixture();
            devices.Setup();
            InputSystem.AddDevice<Keyboard>();
            InputSystem.AddDevice<Mouse>();
            Time.timeScale = 1;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/MainGame.unity",
                new LoadSceneParameters(LoadSceneMode.Additive));
            scene = SceneManager.GetSceneByPath("Assets/Scenes/MainGame.unity");
            var root = scene.GetRootGameObjects()[0];
            player = root.GetComponentInChildren<FpsPlayer>();
            recharge = root.GetComponentInChildren<SurfaceRecharge>();
            root.GetComponentInChildren<DiscoveryField>().gameObject.SetActive(false);
            player.SetApplicationFocus(true);
            if (player.IsMenuOpen) player.CloseMenu();
            yield return null;
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
        public void FeetMustBeOnTheSurfaceInsideTheFootprintAndZoneMustBeEnabled()
        {
            Assert.That(UnityEngine.UIElements.UQueryExtensions.Q<UnityEngine.UIElements.Label>(player.GetComponent<FpsHud>().View.Root, "Return warning").text,
                Is.EqualTo("SURFACE RECHARGE"));
            player.Battery.TrySpend(80);
            foreach (var offset in new[] { new Vector3(0, -0.8f, 0), new Vector3(2.01f, 0.1f, 0),
                new Vector3(0, 0.1f, 1.51f), new Vector3(0, 0.36f, 0) })
            {
                Place(recharge.transform.position + offset);
                Assert.That(recharge.TryRecharge(), Is.False, offset.ToString());
                Assert.That(player.Battery.Charge, Is.EqualTo(20));
            }
            Place(recharge.transform.position + Vector3.up * 0.1f);
            recharge.enabled = false;
            Assert.That(recharge.TryRecharge(), Is.False);
            recharge.enabled = true;
            player.enabled = false;
            Assert.That(recharge.TryRecharge(), Is.False);
            player.enabled = true;
            Assert.That(recharge.TryRecharge(), Is.True,
                $"Recharge setup: menu={player.Menu}, active={player.GameplayActive}, inZone={recharge.IsPlayerInZone}, timeScale={Time.timeScale}, charge={player.Battery.Charge}");
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            Assert.That(recharge.TryRecharge(), Is.False, "A full battery does not repeat the refill.");
        }

        [UnityTest]
        public IEnumerator PauseFocusResumeAndReentryRechargeWithoutTriggerEvents()
        {
            Place(recharge.transform.position + Vector3.up * 0.1f);
            player.Battery.TrySpend(99); // Zero now invokes automatic rescue, covered separately.
            player.OpenMenu(PlayerMenu.Inventory);
            yield return null;
            yield return null;
            Assert.That(player.Battery.Charge, Is.EqualTo(1));
            Assert.That(recharge.TryRecharge(), Is.False);
            player.CloseMenu();
            yield return null;
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            Assert.That(recharge.RecentlyRecharged, Is.True);
            yield return null;
            Assert.That(UnityEngine.UIElements.UQueryExtensions.Q<UnityEngine.UIElements.Label>(player.GetComponent<FpsHud>().View.Root, "Return warning").text,
                Is.EqualTo("FULLY RECHARGED"));
            Place(new Vector3(0, 0.1f, -12.5f));
            player.Battery.TrySpend(65);
            yield return null;
            Assert.That(player.Battery.Charge, Is.EqualTo(35));
            Place(recharge.transform.position + Vector3.up * 0.1f);
            player.SetApplicationFocus(false);
            yield return null;
            Assert.That(player.Battery.Charge, Is.EqualTo(35));
            player.SetApplicationFocus(true);
            Assert.That(recharge.TryRecharge(), Is.False, "Regaining focus keeps Pause open.");
            player.CloseMenu();
            yield return null;
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
        }

        [UnityTest]
        public IEnumerator RealDigAndFlightSharePowerAndSurfaceRechargePreservesTheTrip()
        {
            Place(new Vector3(0, 0.1f, -11.5f));
            player.ViewCamera.transform.localRotation = Quaternion.Euler(85, 0, 0);
            Physics.SyncTransforms();
            Assert.That(player.TryDig(), Is.True);
            Assert.That(player.Battery.Charge, Is.EqualTo(98));
            float removed = recharge.Terrain.RemovedVolume;
            Assert.That(removed, Is.GreaterThan(0));
            player.Inventory.TryAdd(new InventoryItem("kept-find", "Coin", 5));
            player.Shovel.TryUpgradeTo(2);
            player.Tick(new FpsInputFrame { JetpackHeld = true }, 0.3f);
            Assert.That(player.IsJetpackActive, Is.True);
            Assert.That(player.Battery.Charge, Is.LessThan(98));
            float charge = player.Battery.Charge;
            player.Tick(new FpsInputFrame { Move = Vector2.right }, 0.02f);
            player.Tick(default, 0.02f);
            Assert.That(player.Battery.Charge, Is.EqualTo(charge), "Walking and waiting are free.");
            player.OpenMenu(PlayerMenu.Inventory);
            Assert.That(player.TryDig(), Is.False);
            Assert.That(player.Battery.Charge, Is.EqualTo(charge));
            player.CloseMenu();
            Place(recharge.transform.position + Vector3.up * 0.1f);
            Assert.That(recharge.TryRecharge(), Is.True);
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            Assert.That(recharge.Terrain.RemovedVolume, Is.EqualTo(removed));
            Assert.That(player.Inventory.Items.Single().InstanceId, Is.EqualTo("kept-find"));
            Assert.That(player.Shovel.Level, Is.EqualTo(2));
            yield return null;
        }

        private void Place(Vector3 position)
        {
            var motor = player.GetComponent<CharacterController>();
            motor.enabled = false;
            player.transform.position = position;
            motor.enabled = true;
            Physics.SyncTransforms();
        }
    }
}
#endif
