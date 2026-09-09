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
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

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
            SceneManager.sceneLoaded += TestInputPreferences.Configure;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/MainGame.unity", new LoadSceneParameters(LoadSceneMode.Additive));
            SceneManager.sceneLoaded -= TestInputPreferences.Configure;
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
        public void AllFindSizesRequireAuthoredExposureVisibilityAndOneIdentityWithFeedback(FindSize size)
        {
            Assert.That(field.Finds.Count, Is.EqualTo(96));
            Assert.That(field.Finds.Select(f => f.Item.InstanceId).Distinct().Count(), Is.EqualTo(96));
            Assert.That(field.Finds.All(f => f.Exposure == 0 && f.Size == FindSize.Small), Is.True);
            var find = field.Finds[0];
            var settings = new SerializedObject(find);
            settings.FindProperty("size").enumValueIndex = (int)size;
            settings.FindProperty("collectionThreshold").floatValue = size == FindSize.Large ? 0.7f : 0.4f;
            settings.ApplyModifiedPropertiesWithoutUndo();
            Aim(find.transform.position + Vector3.up * 2, find.transform.position);
            player.ToggleAdminXray();
            Assert.That(player.AdminXray, Is.True);
            Assert.That(player.TryInteract(), Is.False, "X-ray cannot collect through soil.");
            Assert.That(find.TryCollect(player), Is.False);
            for (int i = 0; i < 12 && find.Exposure == 0; i++) DigAbove(find, Vector3.zero, 0.22f);
            Assert.That(find.Exposure, Is.InRange(0.001f, find.RequiredExposure - 0.001f));
            StringAssert.Contains("Uncover more", find.GetPrompt(player));
            StringAssert.Contains(Mathf.RoundToInt(find.RequiredExposure * 100) + "% exposed", find.GetPrompt(player));
            Assert.That(find.TryCollect(player), Is.False);
            Expose(find);
            Aim(find.transform.position + Vector3.up * 4, find.transform.position);
            Assert.That(find.TryCollect(player), Is.False, "Collection keeps its separate 3 m reach.");
            Aim(find.transform.position + Vector3.up * 1.5f, find.transform.position);
            player.RefreshTargetPrompt();
            Assert.That(player.TargetPrompt, Is.EqualTo(find.Item.DisplayName));
            Assert.That(player.TryInteract(), Is.False, "E is for stations, not discovery collection.");
            player.Battery.TrySpend(player.Battery.Charge);
            float energy = player.Battery.Charge;
            Assert.That(player.TryPrimaryAction(), Is.True);
            Assert.That(player.Inventory.Items.Single(), Is.SameAs(find.Item));
            StringAssert.Contains("Collected " + find.Item.DisplayName, player.Feedback);
            Assert.That(find.gameObject.activeSelf, Is.False);
            Assert.That(find.GetComponent<Collider>().enabled, Is.False);
            Assert.That(find.TryCollect(player), Is.False);
            Assert.That(player.Battery.Charge, Is.EqualTo(energy));
            Assert.That(player.TryPrimaryAction(), Is.False, "Pickup recovery prevents a second action in the same frame.");
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
            player.ShowFeedback("Other feedback");
            Assert.That(player.TryPrimaryAction(), Is.False);
            Assert.That(player.Feedback, Is.EqualTo("Other feedback"), "Holding on a full bag must not restart its error every frame.");
            Assert.That(find.Collected, Is.False);
            Assert.That(find.gameObject.activeSelf, Is.True);
            terrain.ResetExcavation();
            Assert.That(find.Exposure, Is.Zero);
            Assert.That(find.TryCollect(player), Is.False, "Reburied items cannot be collected through soil.");
            Assert.That(player.Inventory.Count, Is.EqualTo(player.Inventory.Capacity));
        }

        [Test]
        public void VisibleSliverCannotBypassExposureEvenBetweenSamples()
        {
            var find = field.Finds[0];
            // Deliberately sparse authored samples exercise visibility independently
            // from the percentage estimate; no exposure flag is set.
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
            player.TryPrimaryAction(); // A visible sliver may dig, but cannot collect before exposure.
            Assert.That(find.Collected, Is.False);
            Assert.That(player.Inventory.Count, Is.Zero);
            Assert.That(find.gameObject.activeSelf, Is.True);
        }

        [UnityTest]
        public IEnumerator HeldMouseDigsUncoversCollectsAndResumesWithoutASecondPress()
        {
            yield return ExerciseDigAndCollection(false);
        }

        [UnityTest]
        public IEnumerator RemappedToggleDigsUncoversCollectsAndContinuesAfterButtonRelease()
        {
            yield return ExerciseDigAndCollection(true);
        }

        private IEnumerator ExerciseDigAndCollection(bool toggle)
        {
            var find = field.Finds[0];
            player.InputSettings.SetToggleDig(toggle);
            if (toggle) player.InputSettings.Bind(PlayerBinding.Dig, "<Mouse>/rightButton");
            PrepareDeviceView(find);
            player.enabled = true;
            player.SetApplicationFocus(true);
            yield return null;
            devices.Press(keyboard.eKey, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(find.Collected, Is.False);
            devices.Release(keyboard.eKey, queueEventOnly: true);
            yield return null;
            var primary = toggle ? mouse.rightButton : mouse.leftButton;
            devices.Press(primary, queueEventOnly: true);
            if (toggle) { yield return null; yield return null; devices.Release(primary, queueEventOnly: true); }
            yield return new WaitForSeconds(player.EffectiveDigInterval + 0.02f);
            Assert.That(player.SuccessfulStrokes, Is.GreaterThan(0), "Start this hold by digging actual covering terrain.");
            Assert.That(find.Collected, Is.False, "The initial glimpse is not enough to collect.");
            int strokes = player.SuccessfulStrokes;
            int beforePickupRevision = terrain.Revision;
            float beforePickupEnergy = player.Battery.Charge;
            // Dig cadence uses elapsed game time; uncapped rendering can consume
            // a fixed frame budget before enough strokes have been allowed.
            float collectionDeadline = Time.time + 10f;
            while (!find.Collected && Time.time < collectionDeadline)
            {
                beforePickupRevision = terrain.Revision;
                beforePickupEnergy = player.Battery.Charge;
                yield return null;
            }
            Assert.That(find.Collected, Is.True, "One unchanged aim/hold must uncover and collect the small find.");
            Assert.That(player.SuccessfulStrokes, Is.GreaterThan(strokes));
            Assert.That(player.Inventory.Count, Is.EqualTo(1));
            Assert.That(terrain.Revision, Is.EqualTo(beforePickupRevision), "Pickup cannot also dig.");
            Assert.That(player.Battery.Charge, Is.EqualTo(beforePickupEnergy), "Pickup costs no energy.");
            int revision = terrain.Revision;
            float energy = player.Battery.Charge;
            LookAt(new Vector3(find.transform.position.x, 0, find.transform.position.z - 1.5f));
            yield return new WaitForSeconds(player.EffectiveDigInterval + 0.05f);
            Assert.That(terrain.Revision, Is.GreaterThan(revision), "The same hold resumes digging after pickup recovery.");
            Assert.That(player.Battery.Charge, Is.LessThan(energy));
            Assert.That(player.Inventory.Count, Is.EqualTo(1), "Continued holding cannot duplicate the find.");
        }

        [TestCase(FindSize.Small)]
        [TestCase(FindSize.Large)]
        public void AimedUncoveringOnlyAssistsSmallFindsAndUsesOrdinaryFuel(FindSize size)
        {
            var find = field.Finds[0];
            var settings = new SerializedObject(find);
            settings.FindProperty("size").enumValueIndex = (int)size;
            settings.ApplyModifiedPropertiesWithoutUndo();
            Aim(find.transform.position + Vector3.up * 2, find.transform.position);
            for (int i = 0; i < 12; i++)
            {
                Assert.That(player.TryGetTarget(3, out var hit), Is.True);
                if (hit.collider == find.GetComponent<Collider>()) break;
                Assert.That(terrain.TryDig(hit, 0.22f), Is.True);
            }
            Assert.That(player.TryGetTarget(3, out var aimed), Is.True);
            Assert.That(aimed.collider, Is.EqualTo(find.GetComponent<Collider>()));
            Assert.That(find.Collectible, Is.False);
            int revision = terrain.Revision;
            float charge = player.Battery.Charge;
            Assert.That(player.TryPrimaryAction(), Is.EqualTo(size == FindSize.Small));
            Assert.That(find.Collected, Is.False);
            Assert.That(player.Inventory.Count, Is.Zero);
            Assert.That(terrain.Revision > revision, Is.EqualTo(size == FindSize.Small));
            Assert.That(player.Battery.Charge, Is.EqualTo(charge - (size == FindSize.Small ? player.Tuning.DigEnergy : 0)));
            if (size == FindSize.Small)
            {
                revision = terrain.Revision;
                player.TryPrimaryAction();
                Assert.That(terrain.Revision, Is.EqualTo(revision), "Held assistance respects the shovel cooldown.");
            }
        }

        [Test]
        public void AssistedStrokeCannotReachDistantSoilOrIgnoreAnUnrelatedBlocker()
        {
            var find = field.Finds[0];
            Aim(find.transform.position + Vector3.up * 2, find.transform.position);
            for (int i = 0; i < 12; i++)
            {
                Assert.That(player.TryGetTarget(3, out var hit), Is.True);
                if (hit.collider == find.GetComponent<Collider>()) break;
                Assert.That(terrain.TryDig(hit, 0.22f), Is.True);
            }
            int revision = terrain.Revision;
            float charge = player.Battery.Charge;
            // The visible top of the find is reachable, its remaining soil is not.
            player.Tuning.DigReach = Vector3.Distance(player.ViewCamera.transform.position, find.WorldBounds.max) - 0.1f;
            Assert.That(player.TryDig(), Is.False);
            player.Tuning.DigReach = 3;
            var blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            try
            {
                blocker.transform.position = find.transform.position + Vector3.up * 1;
                blocker.transform.localScale = new Vector3(3, 0.1f, 3);
                Physics.SyncTransforms();
                Assert.That(player.TryDig(), Is.False);
                Assert.That(terrain.Revision, Is.EqualTo(revision));
                Assert.That(player.Battery.Charge, Is.EqualTo(charge));
            }
            finally { Object.DestroyImmediate(blocker); }
        }

        [UnityTest]
        public IEnumerator HeldCollectionNeedsReleaseAfterInventoryAndFocusResume()
        {
            var find = field.Finds[0];
            Expose(find);
            PrepareDeviceView(find);
            player.enabled = true;
            player.SetApplicationFocus(true);
            yield return null;
            player.OpenMenu(PlayerMenu.Inventory);
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return null;
            player.CloseMenu();
            yield return new WaitForSeconds(player.EffectiveDigInterval + 0.05f);
            Assert.That(find.Collected, Is.False, "An inspection click cannot become a held pickup after resume.");
            player.SetApplicationFocus(false);
            yield return null;
            player.SetApplicationFocus(true);
            player.CloseMenu();
            yield return new WaitForSeconds(player.EffectiveDigInterval + 0.05f);
            Assert.That(find.Collected, Is.False, "Focus recovery also requires release.");
            devices.Release(mouse.leftButton, queueEventOnly: true);
            yield return null;
            devices.Press(mouse.leftButton, queueEventOnly: true);
            yield return null;
            yield return null;
            Assert.That(find.Collected, Is.True);
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
            var button = MenuTestUI.Button(player, "X-ray: ON");
            MenuTestUI.Click(button);
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
            Assert.That(UnityEngine.UIElements.UQueryExtensions.Q(player.GetComponent<FpsHud>().View.Root, "Admin X-ray").ClassListContains("hidden"), Is.True);
        }

        [UnityTest]
        public IEnumerator ToolkitMarkersFollowCameraProjectionAndHideOutsideTheView()
        {
            var find = field.Finds[0];
            Aim(find.transform.position + new Vector3(0, 0, -4), find.transform.position + Vector3.up * 0.8f);
            player.ToggleAdminXray();
            yield return null; yield return null;
            var hud = player.GetComponent<FpsHud>().View.Root;
            var marker = hud.Q("Admin X-ray").Query<Label>().ToList()[0];
            foreach (int fov in new[] { 55, 90 })
            {
                player.ViewCamera.fieldOfView = fov;
                yield return null; yield return null;
                var viewport = player.ViewCamera.WorldToViewportPoint(find.transform.position);
                var expected = new Vector2(hud.worldBound.xMin + viewport.x * hud.worldBound.width,
                    hud.worldBound.yMin + (1 - viewport.y) * hud.worldBound.height);
                Assert.That(marker.ClassListContains("hidden"), Is.False);
                Assert.That(Vector2.Distance(marker.worldBound.center, expected), Is.LessThan(0.2f));
            }
            player.ViewCamera.transform.Rotate(0, 180, 0);
            yield return null; yield return null;
            Assert.That(marker.ClassListContains("hidden"), Is.True);
            player.OpenMenu(PlayerMenu.Pause);
            yield return null;
            Assert.That(hud.ClassListContains("hidden"), Is.True);
        }

        private void Aim(Vector3 origin, Vector3 point)
        {
            player.ViewCamera.transform.position = origin;
            player.ViewCamera.transform.LookAt(point);
            Physics.SyncTransforms();
        }

        [UnityTest]
        public IEnumerator ToggleOnFullBagKeepsFindAndChargeAndCollectsOnceWhenSpaceBecomesAvailable()
        {
            var find = field.Finds[0]; Expose(find); PrepareDeviceView(find);
            for (int i = 0; i < player.Inventory.Capacity; i++) player.Inventory.TryAdd(new InventoryItem("full-" + i, "Carried", 1));
            player.InputSettings.SetToggleDig(true); player.InputSettings.Bind(PlayerBinding.Dig, "<Mouse>/rightButton");
            player.enabled = true; player.SetApplicationFocus(true); yield return null; yield return null;
            devices.Press(mouse.rightButton, queueEventOnly: true); yield return null; yield return null;
            devices.Release(mouse.rightButton, queueEventOnly: true); yield return null;
            float charge = player.Battery.Charge; int revision = terrain.Revision;
            yield return new WaitForSecondsRealtime(0.5f);
            Assert.That(find.Collected, Is.False); Assert.That(player.Battery.Charge, Is.EqualTo(charge));
            Assert.That(terrain.Revision, Is.EqualTo(revision));
            player.Inventory.TryRemove("full-0", out var removed);
            for (int i = 0; i < 120 && !find.Collected; i++) yield return null;
            Assert.That(find.Collected, Is.True); Assert.That(player.Inventory.Count, Is.EqualTo(player.Inventory.Capacity));
            player.OpenMenu(PlayerMenu.Pause); yield return null;
            Assert.That(player.Inventory.Items.Count(item => item.InstanceId == find.Item.InstanceId), Is.EqualTo(1));
        }

        private void Expose(BuriedFind find)
        {
            float ring = Mathf.Max(find.WorldBounds.extents.x, find.WorldBounds.extents.z) + 0.12f;
            for (int pass = 0; pass < 12 && !find.Collectible; pass++)
                foreach (var offset in new[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back })
                {
                    if (find.Collectible) break;
                    DigAbove(find, offset * ring, 0.65f);
                }
            Assert.That(find.Exposure, Is.GreaterThanOrEqualTo(find.RequiredExposure), "Digging around the find should uncover it.");
        }

        private void PrepareDeviceView(BuriedFind find)
        {
            var motor = player.GetComponent<CharacterController>();
            motor.enabled = false;
            player.transform.SetPositionAndRotation(new Vector3(find.transform.position.x, 0.01f, find.transform.position.z - 1.05f), Quaternion.identity);
            player.ViewCamera.transform.localPosition = Vector3.up * 1.6f;
            motor.enabled = true;
            player.Tuning.Gravity = 0;
            LookAt(find.transform.position);
            Physics.SyncTransforms();
        }

        private void LookAt(Vector3 point)
        {
            Vector3 direction = point - player.ViewCamera.transform.position;
            float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float pitch = -Mathf.Atan2(direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;
            player.Tick(new FpsInputFrame { Look = new Vector2(Mathf.DeltaAngle(player.transform.eulerAngles.y, yaw), player.Pitch - pitch)
                / player.Tuning.LookSensitivity }, 0.001f);
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
