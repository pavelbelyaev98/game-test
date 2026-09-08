#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SomethingDownThere.Tests
{
    public sealed class DiscoveryIntegrationTests
    {
        private Scene scene;
        private TerrainVolume terrain;
        private FpsPlayer player;
        private DiscoveryField field;
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
            Time.timeScale = 1;
            devices = new InputTestFixture();
            devices.Setup();
            keyboard = InputSystem.AddDevice<Keyboard>();
            mouse = InputSystem.AddDevice<Mouse>();
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/MainGame.unity", new LoadSceneParameters(LoadSceneMode.Additive));
            scene = SceneManager.GetSceneByPath("Assets/Scenes/MainGame.unity");
            var root = scene.GetRootGameObjects()[0];
            terrain = root.GetComponentInChildren<TerrainVolume>();
            field = root.GetComponentInChildren<DiscoveryField>();
            player = root.GetComponentInChildren<FpsPlayer>();
            player.enabled = false;
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
            Time.timeScale = oldTimeScale;
            Cursor.lockState = oldCursor;
            Cursor.visible = oldCursorVisible;
        }

        [TestCase(FindSize.Small)]
        [TestCase(FindSize.Large)]
        public void CollectionUsesSizeRulesVisibilityAndOneIdentityWithFeedback(FindSize size)
        {
            Assert.That(field.Finds.Count, Is.EqualTo(96));
            Assert.That(field.Finds.Select(f => f.Item.InstanceId).Distinct().Count(), Is.EqualTo(96));
            Assert.That(field.Finds.All(f => f.Exposure == 0 && f.Size == FindSize.Small), Is.True);
            var find = field.Finds[0];
            var settings = new SerializedObject(find);
            settings.FindProperty("size").enumValueIndex = (int)size;
            settings.ApplyModifiedPropertiesWithoutUndo();
            Aim(find.transform.position + Vector3.up * 2, find.transform.position);
            player.ToggleAdminXray();
            Assert.That(player.AdminXray, Is.True);
            Assert.That(player.TryInteract(), Is.False, "X-ray cannot collect through soil.");
            Assert.That(find.TryCollect(player), Is.False);
            for (int i = 0; i < 12 && find.Exposure == 0; i++) DigAbove(find, Vector3.zero, 0.22f);
            Assert.That(find.Exposure, Is.InRange(0.001f, 0.79f));
            if (size == FindSize.Large)
            {
                StringAssert.Contains("Uncover more", find.GetPrompt(player));
                Assert.That(find.TryCollect(player), Is.False);
                Expose(find);
            }
            else Assert.That(find.GetPrompt(player), Is.EqualTo(find.Item.DisplayName));
            Aim(find.transform.position + Vector3.up * 4, find.transform.position);
            Assert.That(find.TryCollect(player), Is.False, "Collection keeps its separate 3 m reach.");
            Aim(find.transform.position + Vector3.up * 1.5f, find.transform.position);
            player.RefreshTargetPrompt();
            Assert.That(player.TargetPrompt, Is.EqualTo(find.Item.DisplayName));
            Assert.That(player.TryInteract(), Is.False, "E is for stations, not discovery collection.");
            float energy = player.Battery.Charge;
            Assert.That(player.TryPrimaryAction(), Is.True);
            Assert.That(player.Inventory.Items.Single(), Is.SameAs(find.Item));
            StringAssert.Contains("Collected " + find.Item.DisplayName, player.Feedback);
            Assert.That(find.gameObject.activeSelf, Is.False);
            Assert.That(find.GetComponent<Collider>().enabled, Is.False);
            Assert.That(find.TryCollect(player), Is.False);
            Assert.That(player.Battery.Charge, Is.EqualTo(energy));
            player.AdminReturnToSurface();
            terrain.ResetExcavation();
            Assert.That(find.Collected, Is.True);
            Assert.That(find.gameObject.activeSelf, Is.False);
            Assert.That(field.Finds.Skip(1).All(f => f.Exposure == 0), Is.True);
            Assert.That(player.Inventory.Count, Is.EqualTo(1));
        }

        [Test]
        public void CapacityAndCoverKeepTheFindInTheWorldAndResetReburiesIt()
        {
            var find = field.Finds[1];
            Expose(find);
            // From below, the ray still meets untouched soil before the eligible object.
            Aim(find.transform.position + Vector3.down * 2, find.transform.position);
            Assert.That(find.TryCollect(player), Is.False);
            Aim(find.transform.position + Vector3.up * 1.5f, find.transform.position);
            for (int i = 0; i < player.Inventory.Capacity; i++)
                player.Inventory.TryAdd(new InventoryItem("full-" + i, "Carried find", 1));
            Assert.That(player.TryPrimaryAction(), Is.False);
            StringAssert.Contains("Inventory full", player.Feedback);
            Assert.That(find.Collected, Is.False);
            Assert.That(find.gameObject.activeSelf, Is.True);
            terrain.ResetExcavation();
            Assert.That(find.Exposure, Is.Zero);
            Assert.That(find.TryCollect(player), Is.False, "Reburied items cannot be collected through soil.");
            Assert.That(player.Inventory.Count, Is.EqualTo(player.Inventory.Capacity));
        }

        [Test]
        public void VisibleSliverOfSmallFindCanCollectEvenBetweenExposureSamples()
        {
            var find = field.Finds[0];
            // Deliberately sparse authored samples exercise visibility independently
            // from the large-object percentage estimate; no exposure flag is set.
            var settings = new SerializedObject(find);
            var samples = settings.FindProperty("exposureSamples");
            samples.arraySize = 1;
            samples.GetArrayElementAtIndex(0).vector3Value = Vector3.down * 0.5f;
            settings.ApplyModifiedPropertiesWithoutUndo();
            find.RefreshExposure();
            Aim(find.transform.position + Vector3.up * 2, find.transform.position);
            bool visible = false;
            for (int i = 0; i < 12; i++)
            {
                Assert.That(player.TryGetTarget(3, out var hit), Is.True);
                if (hit.collider == find.GetComponent<Collider>()) { visible = true; break; }
                Assert.That(terrain.TryDig(hit, 0.22f), Is.True);
            }
            Assert.That(visible, Is.True);
            Assert.That(find.Exposure, Is.Zero, "The sampled underside should still be buried.");
            Assert.That(player.TryPrimaryAction(), Is.True, "Actual visible geometry is enough for a small find.");
        }

        [UnityTest]
        public IEnumerator RealLeftClickCollectsVisibleSmallFindAndCannotDigThroughItWhileHeld()
        {
            var find = field.Finds[0];
            var motor = player.GetComponent<CharacterController>();
            motor.enabled = false;
            player.transform.SetPositionAndRotation(new Vector3(find.transform.position.x, 0.01f, find.transform.position.z - 1.05f), Quaternion.identity);
            player.ViewCamera.transform.localPosition = Vector3.up * 1.6f;
            motor.enabled = true;
            player.Tuning.Gravity = 0;
            Vector3 direction = find.transform.position - player.ViewCamera.transform.position;
            float pitch = -Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
            player.Tick(new FpsInputFrame { Look = new Vector2(0, (player.Pitch - pitch) / player.Tuning.LookSensitivity) }, 0.001f);
            Physics.SyncTransforms();
            for (int i = 0; i < 16; i++)
            {
                Assert.That(player.TryGetTarget(3, out var hit), Is.True);
                if (hit.collider == find.GetComponent<Collider>()) break;
                Assert.That(terrain.TryDig(hit, 0.22f), Is.True);
            }
            Assert.That(find.Exposure, Is.InRange(0.001f, 0.79f));
            Assert.That(player.TryGetTarget(3, out var visibleHit), Is.True);
            Assert.That(visibleHit.collider, Is.EqualTo(find.GetComponent<Collider>()), "Prepare a real visible surface before clicking.");
            player.enabled = true;
            player.SetApplicationFocus(true);
            yield return null;
            devices.Press(keyboard.eKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(find.Collected, Is.False);
            devices.Release(keyboard.eKey, queueEventOnly: true);
            yield return null;
            int revision = terrain.Revision;
            float energy = player.Battery.Charge;
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(find.Collected, Is.True);
            yield return new WaitForSeconds(player.EffectiveDigInterval * 2 + 0.1f);
            Assert.That(player.Inventory.Count, Is.EqualTo(1));
            Assert.That(terrain.Revision, Is.EqualTo(revision), "A pickup press must not excavate soil behind the removed object.");
            Assert.That(player.Battery.Charge, Is.EqualTo(energy));
            devices.Release(mouse.leftButton, queueEventOnly: true);
            yield return null;
            yield return null;
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(terrain.Revision, Is.GreaterThan(revision), "A new press can resume digging.");
        }

        [UnityTest]
        public IEnumerator XrayUsesRealChordAndMenuButtonAndRestoreHidesMarkers()
        {
            Assert.That(player.AdminXray, Is.False);
            player.enabled = true;
            player.SetApplicationFocus(true);
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.X));
            yield return null;
            yield return null;
            Assert.That(player.AdminXray, Is.False);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.LeftCtrl, Key.LeftShift, Key.X));
            yield return null;
            yield return null;
            Assert.That(player.AdminXray, Is.True);
            Assert.That(player.HasAdminOverrides, Is.True);
            yield return null;
            Assert.That(player.AdminXray, Is.True, "A held chord cannot toggle repeatedly.");
            player.enabled = false;
            player.OpenMenu(PlayerMenu.Pause);
            player.ToggleAdminXray();
            Assert.That(player.AdminXray, Is.True, "Pause is a barrier to the direct shortcut action too.");
            player.ShowAdminMenu();
            yield return null;
            var button = player.GetComponentsInChildren<UnityEngine.UI.Button>().Single(b => b.name == "X-ray: ON");
            button.onClick.Invoke();
            Assert.That(player.AdminXray, Is.False);
            player.SetApplicationFocus(false);
            player.ToggleAdminXray();
            Assert.That(player.AdminXray, Is.False);
            player.SetApplicationFocus(true);
            player.ToggleAdminXray();
            Assert.That(player.AdminXray, Is.True);
            player.RestoreAdminOverrides();
            Assert.That(player.AdminXray || player.HasAdminOverrides, Is.False);
            player.CloseMenu();
            yield return null;
            Assert.That(player.GetComponentsInChildren<RectTransform>(true).Single(t => t.name == "Admin X-ray").gameObject.activeSelf, Is.False);
        }

        private void Aim(Vector3 origin, Vector3 point)
        {
            player.ViewCamera.transform.position = origin;
            player.ViewCamera.transform.LookAt(point);
            Physics.SyncTransforms();
        }

        private void Expose(BuriedFind find)
        {
            for (int pass = 0; pass < 8 && find.Exposure < 0.8f; pass++)
                foreach (var offset in new[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back })
                {
                    if (find.Exposure >= 0.8f) break;
                    DigAbove(find, offset * 0.35f, 0.65f);
                }
            Assert.That(find.Exposure, Is.GreaterThanOrEqualTo(0.8f), "Digging around the find should uncover it.");
        }

        private void DigAbove(BuriedFind find, Vector3 offset, float radius)
        {
            var origin = find.transform.position + offset;
            origin.y = 2;
            Assert.That(Physics.Raycast(origin, Vector3.down, out var hit, 30, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore), Is.True);
            Assert.That(terrain.TryDig(hit, radius), Is.True, "Only actual terrain hits should excavate.");
        }
    }
}
#endif
