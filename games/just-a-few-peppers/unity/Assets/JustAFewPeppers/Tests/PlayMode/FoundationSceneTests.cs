using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace JustAFewPeppers.Tests
{
    public class FoundationSceneTests
    {
        const string ScenePath = "Assets/JustAFewPeppers/Scenes/PepperYard.unity";
        YardSession session;
        Keyboard keyboard;
        Mouse mouse;
        InputTestFixture inputFixture;

        [UnitySetUp]
        public IEnumerator LoadSavedScene()
        {
            inputFixture = new InputTestFixture();
            inputFixture.Setup();
            yield return SceneManager.LoadSceneAsync(ScenePath);
            yield return null;
            session = Object.FindAnyObjectByType<YardSession>();
            Assert.That(session, Is.Not.Null);
            keyboard = InputSystem.AddDevice<Keyboard>();
            mouse = InputSystem.AddDevice<Mouse>();
            session.Input.Actions.devices = new InputDevice[] { keyboard, mouse };
            // Exercise focus callbacks explicitly: a batch runner has no human-focused Game view.
            session.SendMessage("OnApplicationFocus", true);
            Assert.That(session.IsPaused, Is.True);
        }

        [UnityTearDown]
        public IEnumerator UnloadScene()
        {
            var empty = SceneManager.CreateScene("Test cleanup");
            SceneManager.SetActiveScene(empty);
            yield return SceneManager.UnloadSceneAsync(ScenePath);
            InputSystem.RemoveDevice(keyboard);
            InputSystem.RemoveDevice(mouse);
            inputFixture.TearDown();
            Assert.That(Time.timeScale, Is.EqualTo(1));
            LogAssert.NoUnexpectedReceived();
        }

        IEnumerator KeyPress(Key key)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            yield return null;
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            yield return null;
        }

        [UnityTest]
        public IEnumerator AuthoredBindingsNormalizeDiagonalAndPauseDisablesGameplay()
        {
            yield return KeyPress(Key.Enter);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.D));
            yield return null;
            yield return null;
            Assert.That(session.Input.Move.ReadValue<Vector2>().magnitude, Is.EqualTo(1).Within(.001));
            session.Pause("Binding test");
            Assert.That(session.Input.Move.ReadValue<Vector2>(), Is.EqualTo(Vector2.zero));
            Assert.That(session.Input.Pause.enabled, Is.True);
            Assert.That(session.inputActions.enabled, Is.False, "Runtime maps must not change the authored asset.");
        }

        [UnityTest]
        public IEnumerator MenuKeyboardAndMouseResumeAndEscapePause()
        {
            yield return KeyPress(Key.Enter);
            Assert.That(session.IsPaused, Is.False, "UI Submit must invoke the wired Resume button.");
            Assert.That(session.Input.Actions.FindActionMap("Gameplay").enabled, Is.True);
            Assert.That(session.uiInput.enabled, Is.False);
            yield return KeyPress(Key.Escape);
            Assert.That(session.IsPaused, Is.True);
            Assert.That(Time.timeScale, Is.Zero);
            Assert.That(Cursor.lockState, Is.EqualTo(CursorLockMode.None));
            Assert.That(Cursor.visible, Is.True);
            Canvas.ForceUpdateCanvases();
            Vector2 point = RectTransformUtility.WorldToScreenPoint(null, session.hud.resumeButton.transform.position);
            InputSystem.QueueStateEvent(mouse, new MouseState { position = point });
            yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState { position = point, buttons = 1 });
            yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState { position = point });
            yield return null;
            yield return null;
            Assert.That(session.IsPaused, Is.False, "Input System pointer/click must invoke Resume.");
        }

        [UnityTest]
        public IEnumerator MovementLookFocusLossAndResetUseActualInput()
        {
            yield return KeyPress(Key.Enter);
            Assert.That(session.IsPaused, Is.False, "Start menu must resume before movement.");
            var start = session.player.transform.position;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
            yield return new WaitForSecondsRealtime(.25f);
            Assert.That(session.IsPaused, Is.False, "Movement must remain active.");
            Assert.That(session.Input.Move.ReadValue<Vector2>().y, Is.EqualTo(1), "W must reach the gameplay action.");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            Assert.That(session.player.transform.position.z - start.z, Is.GreaterThan(.3f));
            InputSystem.QueueStateEvent(mouse, new MouseState { delta = new Vector2(90, 25) });
            yield return null;
            yield return null;
            Assert.That(Quaternion.Angle(session.player.transform.rotation, Quaternion.identity), Is.GreaterThan(5));
            Assert.That(Quaternion.Angle(session.player.view.transform.localRotation, Quaternion.identity), Is.GreaterThan(1));
            session.SendMessage("OnApplicationFocus", false);
            Assert.That(session.IsPaused, Is.True);
            var pausedPosition = session.player.transform.position;
            var pausedRotation = session.player.transform.rotation;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
            InputSystem.QueueStateEvent(mouse, new MouseState { delta = new Vector2(200, 200) });
            yield return new WaitForSecondsRealtime(.1f);
            Assert.That(session.player.transform.position, Is.EqualTo(pausedPosition));
            Assert.That(session.player.transform.rotation, Is.EqualTo(pausedRotation));
            session.Resume();
            Assert.That(session.IsPaused, Is.True, "Cannot resume while unfocused.");
            session.SendMessage("OnApplicationFocus", true);
            Assert.That(session.IsPaused, Is.True, "Focus gain must not recapture automatically.");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return KeyPress(Key.Escape);
            Assert.That(session.IsPaused, Is.False);
            Assert.That(session.player.transform.rotation, Is.EqualTo(pausedRotation), "Recapture must discard stale mouse motion.");
            yield return KeyPress(Key.R);
            Assert.That(Vector3.Distance(session.player.transform.position, session.safeSpawn.position), Is.LessThan(.1f));
            Assert.That(session.player.transform.rotation, Is.EqualTo(session.safeSpawn.rotation));
            Assert.That(session.player.view.transform.localRotation, Is.EqualTo(Quaternion.identity));
        }

        [UnityTest]
        public IEnumerator ControllerStopsAtWallAndRecoversOutOfBounds()
        {
            yield return KeyPress(Key.Enter);
            var spawn = new GameObject("Collision test start").transform;
            spawn.position = new Vector3(0, .04f, 8);
            session.player.ResetTo(spawn);
            Object.Destroy(spawn.gameObject);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
            yield return new WaitForSecondsRealtime(.5f);
            Assert.That(session.player.transform.position.z, Is.LessThan(8.65f));
            Assert.That(session.player.transform.position.y, Is.GreaterThan(-.15f));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            session.player.body.enabled = false;
            session.player.transform.position = new Vector3(0, -5, 0);
            session.player.body.enabled = true;
            yield return null;
            yield return null;
            Assert.That(Vector3.Distance(session.player.transform.position, session.safeSpawn.position), Is.LessThan(.1f));
            yield return KeyPress(Key.Escape);
            // Navigate to the real Reset button while paused; it keeps the menu open.
            yield return KeyPress(Key.DownArrow);
            Assert.That(session.hud.events.currentSelectedGameObject, Is.SameAs(session.hud.resetButton.gameObject));
            yield return KeyPress(Key.Enter);
            Assert.That(session.IsPaused, Is.True);
            Assert.That(session.hud.noticeText.text, Is.EqualTo("Back at the gate"));
        }

        [UnityTest]
        public IEnumerator EachAuthoredPlaceholderHasReachableTargetFeedback()
        {
            yield return KeyPress(Key.Enter);
            foreach (var target in Object.FindObjectsByType<YardTarget>())
            {
                var collider = target.GetComponentInChildren<Collider>();
                var center = collider.bounds.center;
                var approach = new GameObject("Target test approach").transform;
                approach.position = new Vector3(center.x, .04f, center.z - 2.2f);
                session.player.ResetTo(approach);
                Object.Destroy(approach.gameObject);
                session.player.view.transform.LookAt(center);
                Physics.SyncTransforms();
                session.targeting.Refresh();
                Assert.That(session.targeting.Current, Is.SameAs(target), target.displayName);
                session.hud.ShowTarget(session.targeting.Current);
                Assert.That(session.hud.targetText.text, Does.Contain(target.displayName));
                var properties = new MaterialPropertyBlock();
                target.marker.GetPropertyBlock(properties);
                Assert.That(properties.GetColor("_Color").r, Is.EqualTo(1));
            }
            session.Pause("Test pause");
            Assert.That(session.targeting.Current, Is.Null);
        }
    }
}
