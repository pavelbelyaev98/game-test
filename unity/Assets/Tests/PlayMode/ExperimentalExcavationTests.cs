#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace SomethingDownThere.Tests
{
    public sealed class ExperimentalExcavationTests
    {
        private Scene scene;
        private FpsPlayer player;
        private InputTestFixture devices;
        private Mouse mouse;
        private Keyboard keyboard;

        [UnitySetUp]
        public IEnumerator Open()
        {
            devices = new InputTestFixture(); devices.Setup();
            keyboard = InputSystem.AddDevice<Keyboard>(); mouse = InputSystem.AddDevice<Mouse>();
            Time.timeScale = 1;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/MainGame.unity", new LoadSceneParameters(LoadSceneMode.Additive));
            scene = SceneManager.GetSceneByPath("Assets/Scenes/MainGame.unity");
            player = scene.GetRootGameObjects()[0].GetComponentInChildren<FpsPlayer>();
            player.SetApplicationFocus(true); if (player.IsMenuOpen) player.CloseMenu();
            player.Tuning.Gravity = 0;
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator Close()
        {
            if (scene.IsValid()) yield return SceneManager.UnloadSceneAsync(scene);
            devices.TearDown(); Time.timeScale = 1;
        }

        [UnityTest]
        public IEnumerator AdminOptInDefaultsToShaveAndRestoresTheOriginalCameraAndOwnedProgress()
        {
            var camera = player.ViewCamera; int mask = camera.cullingMask;
            float charge = player.Battery.Charge; int level = player.Shovel.Level;
            Assert.That(player.ExperimentalExcavation, Is.False);
            Assert.That(player.SelectDigMode(ExcavationMode.Shave), Is.False);
            Assert.That(player.GetComponentInChildren<ExcavatorView>(true), Is.Null);
            Assert.That(player.EffectiveDigInterval, Is.EqualTo(player.Tuning.DigInterval * player.EffectiveShovel.CadenceMultiplier));
            player.ShowAdminMenu(); yield return null;
            Assert.That(MenuTestUI.View(player).Root.Query<Button>().ToList().Any(b => b.text == "Experimental excavation: OFF"), Is.True);
            Assert.That(player.SetAdminExperimentalExcavation(true), Is.True);
            Assert.That(player.DigMode, Is.EqualTo(ExcavationMode.Shave));
            Assert.That(player.SetAdminExperimentalExcavation(true), Is.False);
            player.CloseMenu(); yield return null;
            var tool = player.GetComponentInChildren<ExcavatorView>(true);
            Assert.That(tool, Is.Not.Null);
            var overlay = tool.GetComponentInChildren<Camera>(true);
            Assert.That(overlay.enabled, Is.True);
            Assert.That(camera.GetUniversalAdditionalCameraData().cameraStack, Is.EquivalentTo(new[] { overlay }));
            Assert.That(tool.GetComponentsInChildren<AudioSource>(true), Is.Empty);
            Assert.That(tool.GetComponentsInChildren<Collider>(true), Is.Empty);
            foreach (var mesh in tool.GetComponentsInChildren<MeshFilter>(true))
                Assert.That(mesh.GetComponent<Renderer>().sharedMaterial.mainTexture, Is.Not.Null);
            Assert.That(AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/Content/Excavator" }), Is.Empty);
            player.SelectDigMode(ExcavationMode.Fan);
            var snapshot = new WorldSnapshot(); player.Capture(snapshot);
            Assert.That(snapshot.DigMode, Is.EqualTo(ExcavationMode.Scoop));
            player.RestoreAdminOverrides(); yield return null;
            Assert.That(player.ExperimentalExcavation, Is.False); Assert.That(player.DigMode, Is.EqualTo(ExcavationMode.Scoop));
            Assert.That(camera.cullingMask, Is.EqualTo(mask));
            Assert.That(camera.GetUniversalAdditionalCameraData().cameraStack, Is.Empty);
            Assert.That(player.GetComponentInChildren<ExcavatorView>(true), Is.Null);
            Assert.That(player.Battery.Charge, Is.EqualTo(charge)); Assert.That(player.Shovel.Level, Is.EqualTo(level));
        }

        [UnityTest] public IEnumerator HoldCannotResumeAfterExperimentSwitchOrModeChange() => CheckInput(false);
        [UnityTest] public IEnumerator ToggleCannotResumeAfterExperimentSwitchOrModeChange() => CheckInput(true);

        private IEnumerator CheckInput(bool toggle)
        {
            player.ConfigureInputPreferences(new MemoryPreferences()); player.InputSettings.SetToggleDig(toggle);
            player.SetAdminExperimentalExcavation(true);
            var camera = player.ViewCamera;
            player.transform.position = new Vector3(-8, .05f, -8);
            typeof(FpsPlayer).GetField("pitch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(player, 70f);
            camera.transform.localRotation = Quaternion.Euler(70, 0, 0); Physics.SyncTransforms();
            yield return null; yield return null;
            devices.Press(mouse.leftButton, queueEventOnly: true); yield return null; yield return null;
            if (toggle) { devices.Release(mouse.leftButton, queueEventOnly: true); yield return null; }
            Assert.That(player.SuccessfulStrokes, Is.GreaterThan(0));
            devices.Press(keyboard.qKey, queueEventOnly: true); yield return null;
            devices.Release(keyboard.qKey, queueEventOnly: true); yield return null;
            int strokes = player.SuccessfulStrokes;
            Assert.That(player.DigMode, Is.EqualTo(ExcavationMode.Scoop));
            yield return new WaitForSecondsRealtime(.65f);
            Assert.That(player.SuccessfulStrokes, Is.EqualTo(strokes));
            player.SetAdminExperimentalExcavation(false);
            yield return new WaitForSecondsRealtime(.4f);
            Assert.That(player.SuccessfulStrokes, Is.EqualTo(strokes));
            devices.Release(mouse.leftButton, queueEventOnly: true); yield return null;
            devices.Press(mouse.leftButton, queueEventOnly: true); yield return null; yield return null;
            Assert.That(player.SuccessfulStrokes, Is.GreaterThan(strokes));
        }

        [UnityTest]
        public IEnumerator LegacyExperimentalSelectionCannotReenableTheExperimentAndControlsStayOrdinary()
        {
            var state = new WorldSnapshot(); player.Capture(state); state.DigMode = ExcavationMode.Fan;
            player.Restore(state); yield return null;
            Assert.That(player.DigMode, Is.EqualTo(ExcavationMode.Scoop));
            Assert.That(player.ExperimentalExcavation, Is.False);
            player.OpenMenu(PlayerMenu.Pause); player.ShowInputSettings(); yield return null;
            var binding = MenuTestUI.View(player).Root.Q<Button>("bindCycleMode");
            Assert.That(binding, Is.Not.Null); Assert.That(binding.enabledInHierarchy, Is.False);
            Assert.That(binding.parent.ClassListContains("hidden"), Is.True);
            Assert.That(player.SetAdminExperimentalExcavation(true), Is.False, "Ordinary settings cannot enable experiments.");
        }

        private sealed class MemoryPreferences : IDevicePreferencesStore
        {
            public string Read() => null;
            public void Write(string text) { }
        }
    }
}
#endif
