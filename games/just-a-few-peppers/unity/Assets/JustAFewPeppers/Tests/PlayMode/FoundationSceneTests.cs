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
        public IEnumerator SprintBindingsIncreaseSpeedWithoutDiagonalBonusAndReleaseToWalk()
        {
            yield return KeyPress(Key.Enter);
            foreach (var keys in new[] {
                new[] { Key.W }, new[] { Key.W, Key.LeftShift },
                new[] { Key.W, Key.D, Key.RightShift }, new[] { Key.W } })
            {
                session.ResetToSpawn();
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(keys));
                yield return null;
                yield return null;
                var start = session.player.transform.position;
                float started = Time.time;
                while (Time.time - started < .2f) yield return null;
                var movement = session.player.transform.position - start;
                movement.y = 0;
                float expectedSpeed = keys.Length > 1 ? session.player.sprintSpeed : session.player.walkSpeed;
                Assert.That(movement.magnitude / (Time.time - started), Is.EqualTo(expectedSpeed).Within(.15f));
                Assert.That(session.player.view.fieldOfView, Is.EqualTo(72), "Sprinting does not change FOV.");
            }
            session.Pause("Sprint test");
            Assert.That(session.Input.Sprint.enabled, Is.False);
            Assert.That(session.Input.Jump.enabled, Is.False);
        }

        [UnityTest]
        public IEnumerator JumpInputLandsOnceWhileHeldAndRejectsMidairSecondJump()
        {
            yield return KeyPress(Key.Enter);
            yield return new WaitForSecondsRealtime(.1f);
            float floor = session.player.transform.position.y;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));
            float started = Time.time;
            float peak = floor;
            while (Time.time - started < 1.2f)
            {
                peak = Mathf.Max(peak, session.player.transform.position.y);
                if (Time.time - started > .8f)
                    Assert.That(session.player.transform.position.y, Is.EqualTo(floor).Within(.06f), "Holding Space must not repeat on landing.");
                yield return null;
            }
            Assert.That(peak - floor, Is.InRange(.70f, .86f));
            yield return KeyPress(Key.Space); // Release the previously held key.
            yield return KeyPress(Key.Space);
            yield return new WaitForSecondsRealtime(.1f);
            yield return KeyPress(Key.Space); // Too early for a landing buffer; must not jump again in air.
            started = Time.time;
            while (Time.time - started < .9f)
            {
                Assert.That(session.player.transform.position.y - floor, Is.LessThan(.87f));
                if (Time.time - started > .65f)
                    Assert.That(session.player.transform.position.y, Is.EqualTo(floor).Within(.06f));
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator MenuSpaceFocusPauseAndResetDoNotLeakJumpInput()
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));
            yield return new WaitForSecondsRealtime(.25f);
            Assert.That(session.IsPaused, Is.False, "Space submits the actual Resume button.");
            Assert.That(session.player.transform.position.y, Is.LessThan(.1f), "Menu submit must not jump.");
            yield return KeyPress(Key.Space); // Release.
            yield return KeyPress(Key.Space); // A fresh gameplay jump.
            yield return new WaitForSecondsRealtime(.08f);
            Assert.That(session.player.transform.position.y, Is.GreaterThan(.25f));
            session.SendMessage("OnApplicationFocus", false);
            var frozen = session.player.transform.position;
            Assert.That(session.uiInput.enabled, Is.False, "An unfocused window must not navigate/submit its pause menu.");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space, Key.LeftShift, Key.W));
            yield return new WaitForSecondsRealtime(.15f);
            Assert.That(session.player.transform.position, Is.EqualTo(frozen));
            session.SendMessage("OnApplicationFocus", true);
            Assert.That(session.IsPaused, Is.True);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));
            session.Resume();
            yield return new WaitForSecondsRealtime(.9f);
            Assert.That(session.player.transform.position.y, Is.LessThan(.1f), "Resume continues the arc without repeating a held jump.");
            yield return KeyPress(Key.Space);
            yield return KeyPress(Key.Space);
            yield return new WaitForSecondsRealtime(.08f);
            Assert.That(session.player.transform.position.y, Is.GreaterThan(.25f));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.R, Key.Space));
            yield return new WaitForSecondsRealtime(.2f);
            Assert.That(Vector3.Distance(session.player.transform.position, session.safeSpawn.position), Is.LessThan(.1f));
            Assert.That(session.player.view.transform.localRotation, Is.EqualTo(Quaternion.identity));
        }

        void PlacePlayer(Vector3 position)
        {
            var spawn = new GameObject("Movement test spawn").transform;
            spawn.position = position;
            session.player.ResetTo(spawn);
            Object.Destroy(spawn.gameObject);
            Physics.SyncTransforms();
        }

        void StepFor(float seconds, float frameTime = 1f / 60, bool jump = false, Vector2 move = default, bool sprint = false)
        {
            while (seconds > .000001f)
            {
                float dt = Mathf.Min(seconds, frameTime);
                session.player.Step(move, Vector2.zero, sprint, jump, dt);
                jump = false;
                seconds -= dt;
            }
        }

        [UnityTest]
        public IEnumerator JumpArcIsConsistentAtThirtySixtyAndOneHundredFortyFourFps()
        {
            yield return KeyPress(Key.Enter);
            session.enabled = false; // Drive the real controller with deterministic render-frame durations.
            foreach (float fps in new[] { 30f, 60f, 144f })
            {
                PlacePlayer(session.safeSpawn.position);
                StepFor(.15f);
                float floor = session.player.transform.position.y;
                StepFor(.28f, 1 / fps, true, Vector2.up, true);
                Assert.That(session.player.transform.position.y - floor, Is.InRange(.74f, .86f), "Jump apex at " + fps + " FPS");
                Assert.That(session.player.transform.position.z - session.safeSpawn.position.z, Is.EqualTo(5.4f * .28f).Within(.04f));
                StepFor(.6f, 1 / fps);
                Assert.That(session.player.body.isGrounded, Is.True);
                Assert.That(session.player.transform.position.y, Is.EqualTo(floor).Within(.04f));
            }
        }

        [UnityTest]
        public IEnumerator JumpGraceAndLandingBufferExpireAndPauseClearsPendingRequests()
        {
            yield return KeyPress(Key.Enter);
            session.enabled = false;
            var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
            platform.name = "Temporary movement ledge";
            platform.transform.position = new Vector3(6, .6f, -5);
            platform.transform.localScale = new Vector3(2, 1.2f, 2);
            Physics.SyncTransforms();
            foreach (float delay in new[] { .04f, .16f })
            {
                platform.SetActive(true);
                PlacePlayer(new Vector3(6, 1.24f, -5));
                StepFor(.15f);
                Assert.That(session.player.body.isGrounded, Is.True);
                // Remove support exactly; avoids an edge's rounded capsule affecting the test timing.
                platform.SetActive(false);
                Physics.SyncTransforms();
                StepFor(delay);
                float before = session.player.transform.position.y;
                StepFor(.05f, jump: true);
                if (delay < .1f) Assert.That(session.player.transform.position.y, Is.GreaterThan(before + .1f));
                else Assert.That(session.player.transform.position.y, Is.LessThan(before));
            }
            foreach (bool clearOnPause in new[] { false, true })
            {
                PlacePlayer(new Vector3(6, .4f, -5));
                StepFor(.13f); // Falling, roughly 0.08 s before landing.
                StepFor(.01f, jump: true);
                if (clearOnPause)
                {
                    session.Pause("Pending jump test");
                    session.Resume();
                }
                StepFor(.28f);
                if (clearOnPause) Assert.That(session.player.transform.position.y, Is.LessThan(.1f));
                else Assert.That(session.player.transform.position.y, Is.GreaterThan(.5f), "A recent press should launch after landing.");
            }
            PlacePlayer(new Vector3(6, 2, -5));
            StepFor(.01f, jump: true);
            StepFor(.85f);
            Assert.That(session.player.transform.position.y, Is.LessThan(.1f), "An old airborne request must expire before landing.");
        }

        [UnityTest]
        public IEnumerator JumpRespectsCeilingsPropsAndElevatedYardBoundaries()
        {
            yield return KeyPress(Key.Enter);
            session.enabled = false;
            var ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ceiling.transform.position = new Vector3(0, 2.15f, -6);
            ceiling.transform.localScale = new Vector3(3, .2f, 3);
            PlacePlayer(session.safeSpawn.position);
            StepFor(.1f);
            StepFor(.12f, jump: true);
            Assert.That(session.player.transform.position.y, Is.InRange(.1f, .31f), "Head must stop at the ceiling.");
            StepFor(.2f);
            Assert.That(session.player.body.isGrounded, Is.True, "Ceiling contact must cancel ascent immediately.");
            ceiling.SetActive(false);
            PlacePlayer(new Vector3(-3, 1.5f, 0));
            StepFor(.6f);
            Assert.That(session.player.transform.position.y, Is.InRange(.95f, 1.13f), "Land on the visible flattened mound, not an oversized sphere.");
            PlacePlayer(new Vector3(0, 2.4f, 8));
            StepFor(.3f, move: Vector2.up, sprint: true);
            Assert.That(session.player.transform.position.z, Is.LessThan(8.65f), "Prop-height movement must remain inside the yard.");
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
