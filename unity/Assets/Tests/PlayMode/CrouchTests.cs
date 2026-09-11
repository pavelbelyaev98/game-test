using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace SomethingDownThere.Tests
{
    public sealed class CrouchTests
    {
        private readonly List<GameObject> objects = new List<GameObject>();
        private FpsPlayer player;
        private CharacterController motor;
        private float previousTimeScale;
        private CursorLockMode previousCursor;
        private bool previousCursorVisible;
        private sealed class Preferences : ICameraPreferencesStore
        {
            public string Read() => null;
            public void Write(string contents) { }
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            previousTimeScale = Time.timeScale;
            previousCursor = Cursor.lockState;
            previousCursorVisible = Cursor.visible;
            Time.timeScale = 1;
            var root = new GameObject("Crouch controller fixture");
            objects.Add(root);
            root.SetActive(false);
            root.layer = 2;
            motor = root.AddComponent<CharacterController>();
            motor.height = 1.8f; motor.radius = 0.3f; motor.center = Vector3.up * 0.9f;
            motor.skinWidth = 0.08f; motor.stepOffset = 0.3f; motor.slopeLimit = 45; motor.minMoveDistance = 0;
            var camera = new GameObject("Camera", typeof(Camera));
            camera.transform.SetParent(root.transform, false);
            camera.transform.localPosition = Vector3.up * 1.6f;
            camera.GetComponent<Camera>().nearClipPlane = 0.05f;
            player = root.AddComponent<FpsPlayer>();
            player.ConfigureCameraPreferences(new Preferences());
            Box("Floor", Vector3.down * 0.5f, new Vector3(100, 1, 100));
            root.SetActive(true);
            player.enabled = false;
            yield return null;
            player.SetApplicationFocus(true);
            if (player.IsMenuOpen) player.CloseMenu();
            yield return null;
            Physics.SyncTransforms();
            Advance(0.3f, default);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var item in objects) if (item != null) Object.DestroyImmediate(item);
            objects.Clear();
            Time.timeScale = previousTimeScale;
            Cursor.lockState = previousCursor;
            Cursor.visible = previousCursorVisible;
        }

        [TestCase(30)]
        [TestCase(60)]
        [TestCase(144)]
        public void CrouchGivesNormalizedSmallCorrectionsWithoutMovingFeetOrChangingViewSettings(int fps)
        {
            Vector3 start = player.FeetPosition;
            float fov = player.ViewCamera.fieldOfView;
            Advance(1f, new FpsInputFrame { CrouchHeld = true, Move = Vector2.one }, fps);
            Vector3 delta = player.FeetPosition - start;
            Assert.That(new Vector2(delta.x, delta.z).magnitude, Is.EqualTo(1.4f).Within(0.01f));
            Assert.That(delta.y, Is.EqualTo(0).Within(0.01f));
            Assert.That(motor.height, Is.EqualTo(1.1f).Within(0.001f));
            Assert.That(motor.center.y - motor.height * 0.5f, Is.EqualTo(0).Within(0.00001f));
            Assert.That(player.ViewCamera.transform.localPosition.y, Is.EqualTo(0.95f).Within(0.001f));
            Assert.That(player.ViewCamera.fieldOfView, Is.EqualTo(fov));
            Assert.That(player.CameraSettings.SteadyCrosshair, Is.True);
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            Advance(0.25f, default, fps);
            start = player.FeetPosition;
            Advance(0.2f, new FpsInputFrame { Move = Vector2.right }, fps);
            Assert.That(player.FeetPosition.x - start.x, Is.EqualTo(0.8f).Within(0.01f));
            Assert.That(motor.height, Is.EqualTo(1.8f));
        }

        [Test]
        public void LowRoofKeepsReleasedCrouchSlowAndClearingItStandsAutomatically()
        {
            Advance(0.25f, new FpsInputFrame { CrouchHeld = true });
            var roof = Box("Low roof", new Vector3(0, 1.55f, 0), new Vector3(8, 0.5f, 8));
            Physics.SyncTransforms();
            float x = player.transform.position.x;
            Advance(0.5f, new FpsInputFrame { Move = Vector2.right, SprintHeld = true });
            Assert.That(player.CrouchAmount, Is.EqualTo(1));
            Assert.That(player.StandBlocked, Is.True);
            Assert.That(player.Feedback, Is.EqualTo("Low ceiling"));
            Assert.That(player.transform.position.x - x, Is.EqualTo(0.7f).Within(0.01f));
            player.ViewCamera.fieldOfView = 90;
            player.Tick(new FpsInputFrame { Look = new Vector2(0, 500) }, 0.02f);
            Assert.That(player.ViewCamera.transform.position.y, Is.LessThan(1.3f));
            roof.SetActive(false);
            Advance(0.25f, default);
            Assert.That(player.CrouchAmount, Is.Zero);
            Assert.That(player.StandBlocked, Is.False);
        }

        [TestCase(30)]
        [TestCase(60)]
        [TestCase(144)]
        public void ModestSprintNormalizesDiagonalsReleasesImmediatelyAndYieldsToCrouch(int fps)
        {
            float charge = player.Battery.Charge, fov = player.ViewCamera.fieldOfView;
            var start = player.transform.position;
            Advance(1f, new FpsInputFrame { Move = Vector2.one, SprintHeld = true }, fps);
            var delta = player.transform.position - start; delta.y = 0;
            Assert.That(delta.magnitude, Is.EqualTo(5.4f).Within(0.01f));
            start = player.transform.position;
            Advance(0.5f, new FpsInputFrame { Move = Vector2.right }, fps);
            Assert.That(player.transform.position.x - start.x, Is.EqualTo(2f).Within(0.01f));
            start = player.transform.position;
            Advance(0.5f, new FpsInputFrame { Move = Vector2.right, SprintHeld = true, CrouchHeld = true }, fps);
            Assert.That(player.transform.position.x - start.x, Is.EqualTo(0.7f).Within(0.01f));
            start = player.transform.position;
            Advance(0.5f, new FpsInputFrame { SprintHeld = true }, fps);
            Assert.That(player.transform.position.x, Is.EqualTo(start.x).Within(0.0001f));
            Assert.That(player.Battery.Charge, Is.EqualTo(charge));
            Assert.That(player.ViewCamera.fieldOfView, Is.EqualTo(fov));
        }

        [Test]
        public void NearPlaneStaysInsideCollisionEnvelopeAcrossStancesFovAndAspect()
        {
            var camera = player.ViewCamera;
            var corners = new Vector3[4];
            foreach (float amount in new[] { 0f, 0.5f, 1f })
            foreach (float fov in new[] { 55f, 75f, 90f })
            foreach (float aspect in new[] { 16f / 9f, 16f / 10f, 32f / 9f })
            foreach (float pitch in new[] { -85f, 0f, 85f })
            {
                var snapshot = new WorldSnapshot();
                player.Capture(snapshot);
                snapshot.CrouchAmount = amount;
                snapshot.Pitch = pitch;
                camera.fieldOfView = fov;
                camera.aspect = aspect;
                player.Restore(snapshot);
                camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), camera.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, corners);
                foreach (var corner in corners)
                {
                    Vector3 point = player.transform.InverseTransformPoint(camera.transform.TransformPoint(corner));
                    Vector3 axis = new Vector3(0, Mathf.Clamp(point.y, motor.radius, motor.height - motor.radius), 0);
                    Assert.That(Vector3.Distance(point, axis), Is.LessThan(motor.radius - motor.skinWidth - 0.005f),
                        $"Near plane escaped stance {amount}, FOV {fov}, aspect {aspect}, pitch {pitch}.");
                }
                Assert.That(camera.fieldOfView, Is.EqualTo(fov));
            }
        }

        [Test]
        public void ReversalAndNewObstructionNeverExpandPastSafeHeight()
        {
            player.Tick(new FpsInputFrame { CrouchHeld = true }, 0.1f);
            float halfEye = player.ViewCamera.transform.localPosition.y;
            Assert.That(halfEye, Is.InRange(0.95f, 1.6f));
            player.Tick(default, 0.02f);
            Assert.That(player.ViewCamera.transform.localPosition.y, Is.GreaterThan(halfEye));
            Advance(0.25f, new FpsInputFrame { CrouchHeld = true });
            player.Tick(default, 0.04f);
            float partialHeight = motor.height;
            var roof = Box("New roof over partial stand", new Vector3(0, 1.65f, 0), new Vector3(4, 0.2f, 4));
            Physics.SyncTransforms();
            player.Tick(default, 0.02f);
            Assert.That(motor.height, Is.LessThan(partialHeight));
            Advance(0.25f, default);
            Assert.That(player.CrouchAmount, Is.EqualTo(1));
            Assert.That(motor.isGrounded, Is.True);
            roof.SetActive(false);
            Advance(0.25f, default);
            Assert.That(player.CrouchAmount, Is.Zero);
        }

        [Test]
        public void TriggerAndOwnCollidersDoNotBlockStanding()
        {
            Advance(0.25f, new FpsInputFrame { CrouchHeld = true });
            Box("Nonblocking trigger", new Vector3(0, 1.5f, 0), Vector3.one).GetComponent<BoxCollider>().isTrigger = true;
            var own = Box("Own collision", new Vector3(0, 1.5f, 0), Vector3.one);
            own.transform.SetParent(player.transform, true);
            Physics.SyncTransforms();
            Advance(0.25f, default);
            Assert.That(player.CrouchAmount, Is.Zero);
        }

        [TestCase(30)]
        [TestCase(60)]
        [TestCase(144)]
        public void AirborneCrouchKeepsVerticalRulesAndPrecisionThroughRestartAndDepletion(int fps)
        {
            player.Tick(new FpsInputFrame { JumpPressed = true, CrouchHeld = true }, 1f / fps);
            Assert.That(player.VerticalSpeed, Is.GreaterThan(5));
            Assert.That(player.Battery.Charge, Is.EqualTo(100));
            Advance(0.3f, new FpsInputFrame { JetpackHeld = true, CrouchHeld = true }, fps);
            Assert.That(player.IsJetpackActive, Is.True);
            Advance(0.1f, new FpsInputFrame { CrouchHeld = true }, fps);
            float x = player.transform.position.x;
            float energy = player.Battery.Charge;
            player.Tick(new FpsInputFrame { JetpackHeld = true, CrouchHeld = true, Move = Vector2.right }, 1f / fps);
            Assert.That(player.IsJetpackActive, Is.True);
            Assert.That(player.transform.position.x - x, Is.EqualTo(1.4f / fps).Within(0.001f));
            Assert.That(player.Battery.Charge, Is.EqualTo(energy - 8f / fps).Within(0.001f));
            player.Battery.TrySpend(player.Battery.Charge - 0.01f);
            player.Tick(new FpsInputFrame { JetpackHeld = true, CrouchHeld = true }, 1f / fps);
            Assert.That(player.Battery.Charge, Is.Zero);
            Advance(3f, new FpsInputFrame { JetpackHeld = true, CrouchHeld = true }, fps);
            Assert.That(motor.isGrounded, Is.True);
            Assert.That(player.IsJetpackActive, Is.False);
            player.Tick(new FpsInputFrame { JumpPressed = true, CrouchHeld = true }, 1f / fps);
            Assert.That(player.VerticalSpeed, Is.GreaterThan(5), "Empty-battery crouched jump remains free.");
        }

        [Test]
        public void PartialAndLowRoofStancesRestoreBeforePlayAndImpossibleRestoreIsRejected()
        {
            player.Tick(new FpsInputFrame { CrouchHeld = true }, 0.1f);
            var snapshot = new WorldSnapshot();
            player.Capture(snapshot);
            float eye = player.ViewCamera.transform.localPosition.y;
            Advance(0.25f, default);
            player.Restore(snapshot);
            Assert.That(player.CrouchAmount, Is.EqualTo(snapshot.CrouchAmount));
            Assert.That(player.ViewCamera.transform.localPosition.y, Is.EqualTo(eye));
            Advance(0.25f, new FpsInputFrame { CrouchHeld = true });
            player.Capture(snapshot);
            Box("Roof at saved position", new Vector3(0, 1.55f, 0), new Vector3(5, 0.5f, 5));
            Physics.SyncTransforms();
            player.Restore(snapshot);
            Advance(0.25f, default);
            Assert.That(player.StandBlocked, Is.True);
            snapshot.CrouchAmount = 0;
            Assert.Throws<InvalidDataException>(() => player.Restore(snapshot));
            Assert.That(player.CrouchAmount, Is.EqualTo(1), "Failed restore must retain the safe body.");
        }

        [UnityTest]
        public IEnumerator MenusFocusAndReenablePreservePhysicalStanceUntilExplicitResume()
        {
            player.Tick(new FpsInputFrame { CrouchHeld = true }, 0.1f);
            foreach (PlayerMenu menu in Enum.GetValues(typeof(PlayerMenu)))
            {
                if (menu == PlayerMenu.None || menu == PlayerMenu.CameraComfort
                    || menu == PlayerMenu.DeveloperAdmin || menu == PlayerMenu.ConfirmTerrainReset) continue;
                player.OpenMenu(menu);
                Assert.That(player.Menu, Is.EqualTo(menu));
                yield return null;
                float amount = player.CrouchAmount;
                Vector3 eye = player.ViewCamera.transform.position;
                player.Tick(new FpsInputFrame { CrouchHeld = true, Move = Vector2.one, SprintHeld = true, JetpackHeld = true, DigHeld = true }, 1);
                Assert.That(player.CrouchAmount, Is.EqualTo(amount));
                Assert.That(player.ViewCamera.transform.position, Is.EqualTo(eye));
                player.CloseMenu();
                if (player.Menu == PlayerMenu.Pause) player.CloseMenu();
                yield return null;
            }
            player.OpenMenu(PlayerMenu.Pause);
            player.ShowCameraComfort();
            player.SetApplicationFocus(false);
            yield return null;
            float paused = player.CrouchAmount;
            player.Tick(default, 1);
            Assert.That(player.CrouchAmount, Is.EqualTo(paused));
            player.SetApplicationFocus(true);
            player.BackFromCameraComfort();
            player.CloseMenu();
            yield return null;
            player.Tick(new FpsInputFrame { CrouchHeld = true }, 0.2f);
            Box("Roof over disabled player", new Vector3(0, 1.55f, 0), new Vector3(4, 0.5f, 4));
            Physics.SyncTransforms();
            player.enabled = true;
            player.enabled = false;
            Advance(0.3f, default);
            Assert.That(player.CrouchAmount, Is.EqualTo(1));
        }

        private void Advance(float seconds, FpsInputFrame frame, int fps = 60)
        {
            int frames = Mathf.CeilToInt(seconds * fps);
            for (int i = 0; i < frames; i++) player.Tick(frame, seconds / frames);
        }

        private GameObject Box(string name, Vector3 position, Vector3 size)
        {
            var root = new GameObject(name, typeof(BoxCollider));
            root.transform.position = position;
            root.GetComponent<BoxCollider>().size = size;
            objects.Add(root);
            return root;
        }
    }
}
