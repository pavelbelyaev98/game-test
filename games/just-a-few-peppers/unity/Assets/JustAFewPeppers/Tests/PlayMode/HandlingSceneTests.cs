using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace JustAFewPeppers.Tests
{
    public class HandlingSceneTests
    {
        const string ScenePath = "Assets/JustAFewPeppers/Scenes/PepperYard.unity";
        YardSession session;
        YardHandling handling;
        Keyboard keyboard;
        Mouse mouse;
        InputTestFixture fixture;
        float previousAudioVolume;

        [UnitySetUp]
        public IEnumerator Load()
        {
            previousAudioVolume = AudioListener.volume;
            AudioListener.volume = 0;
            fixture = new InputTestFixture(); fixture.Setup();
            yield return SceneManager.LoadSceneAsync(ScenePath);
            yield return null;
            session = Object.FindAnyObjectByType<YardSession>();
            handling = session.handling;
            keyboard = InputSystem.AddDevice<Keyboard>(); mouse = InputSystem.AddDevice<Mouse>();
            session.Input.Actions.devices = new InputDevice[] { keyboard, mouse };
            session.SendMessage("OnApplicationFocus", true);
            yield return KeyPress(Key.Enter);
        }

        [UnityTearDown]
        public IEnumerator Unload()
        {
            try
            {
                SceneManager.SetActiveScene(SceneManager.CreateScene("Handling cleanup"));
                yield return SceneManager.UnloadSceneAsync(ScenePath);
                InputSystem.RemoveDevice(keyboard); InputSystem.RemoveDevice(mouse);
                fixture.TearDown();
                LogAssert.NoUnexpectedReceived();
            }
            finally
            {
                // Restore even if a test or its cleanup fails.
                AudioListener.volume = previousAudioVolume;
            }
        }

        IEnumerator KeyPress(Key key)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            yield return null; yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null; yield return null;
        }

        IEnumerator MouseDown(bool down)
        {
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = (ushort)(down ? 1 : 0) });
            yield return null; yield return null;
        }

        IEnumerator RightClick()
        {
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2 });
            yield return null; yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState());
            yield return null; yield return null;
        }

        void Aim(Vector3 position, Vector3 target)
        {
            var pose = new GameObject("Test approach").transform;
            pose.position = position;
            var direction = target - (position + Vector3.up * 1.65f);
            pose.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
            session.player.ResetTo(pose);
            Object.Destroy(pose.gameObject);
            float pitch = Mathf.Atan2(-direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;
            session.player.Step(Vector2.zero, new Vector2(0, -pitch / session.player.lookSensitivity), false, false, 1f / 60);
            Physics.SyncTransforms();
            session.targeting.Refresh();
        }

        void AimRegion(int index)
        {
            var region = handling.regions[index];
            Aim(new Vector3(region.transform.position.x, .04f, -2.65f), region.volume.position);
            Assert.That(session.targeting.CurrentRegion, Is.SameAs(region));
        }

        IEnumerator PickUp()
        {
            Aim(new Vector3(-1.25f, .04f, -4.4f), handling.crate.transform.position + Vector3.up * .3f);
            Assert.That(session.targeting.Current, Is.SameAs(handling.crate.target));
            yield return RightClick();
            Assert.That(handling.State.IsHeld, Is.True);
        }

        void Conserved()
        {
            Assert.That(handling.State.RawUnits + handling.State.Remaining, Is.EqualTo(107));
            Assert.That(handling.crate.contents.Count(item => item.activeSelf), Is.EqualTo(handling.State.RawUnits));
        }

        [UnityTest]
        public IEnumerator LocalDepletionInterruptionFullLoadAndDenialCuesUseActualMouseInput()
        {
            yield return PickUp();
            AimRegion(0);
            var untouchedScale = handling.regions[1].volume.localScale;
            yield return MouseDown(true);
            Assert.That(handling.State.RawUnits, Is.EqualTo(1), "First valid scoop responds immediately.");
            var crateTop = session.player.view.WorldToViewportPoint(handling.crate.transform.TransformPoint(new Vector3(0, .55f, .325f)));
            Assert.That(crateTop.y, Is.LessThan(.44f), "Looking down must leave the scoop target above the crate.");
            Assert.That(handling.regions[0].volume.localScale.y, Is.LessThan(.25f));
            Assert.That(handling.regions[1].volume.localScale, Is.EqualTo(untouchedScale));
            yield return MouseDown(false);
            yield return new WaitForSeconds(.7f);
            Assert.That(handling.State.RawUnits, Is.EqualTo(1), "Release cannot queue another transfer.");
            yield return MouseDown(true);
            yield return new WaitForSeconds(1.1f);
            Assert.That(handling.State.UnitsIn("gate-mound-0"), Is.Zero);
            Assert.That(handling.regions[0].volume.gameObject.activeSelf, Is.False);
            Assert.That(handling.State.RawUnits, Is.EqualTo(3));
            int denied = handling.presentation.FeedbackCues;
            yield return new WaitForSeconds(.8f);
            Assert.That(handling.presentation.FeedbackCues, Is.EqualTo(denied), "Empty region cannot spam.");
            yield return MouseDown(false);
            AimRegion(1);
            float start = Time.time;
            yield return MouseDown(true);
            yield return new WaitForSeconds(4.8f);
            Assert.That(handling.State.RawUnits, Is.EqualTo(12));
            Assert.That(handling.presentation.ScoopCues, Is.EqualTo(12));
            Assert.That(AudioListener.volume, Is.Zero, "Action cues must stay muted throughout automated play.");
            Assert.That(handling.presentation.actionAudio.ignoreListenerVolume, Is.False);
            Assert.That(handling.presentation.feedbackAudio.ignoreListenerVolume, Is.False);
            Assert.That(handling.regions[1].surfaceClumps[0].GetComponent<Renderer>().bounds.size.y, Is.GreaterThan(.08f), "The final unit stays visibly substantial.");
            Assert.That(Time.time - start, Is.GreaterThan(4));
            denied = handling.presentation.FeedbackCues;
            Assert.That(session.hud.targetText.text, Does.Contain("Crate full"));
            yield return new WaitForSeconds(1.1f);
            yield return MouseDown(false); yield return MouseDown(true);
            yield return new WaitForSeconds(.6f);
            Assert.That(handling.presentation.FeedbackCues, Is.EqualTo(denied), "Held/repressed full trigger stays quiet.");
            Conserved();
        }

        [UnityTest]
        public IEnumerator InvalidOccludedTargetsAndRapidClicksCannotGatherOrSpam()
        {
            yield return PickUp();
            Aim(new Vector3(2, .04f, -5), new Vector3(2, 1.65f, -8.8f));
            yield return MouseDown(true);
            int denied = handling.presentation.FeedbackCues;
            Assert.That(denied, Is.EqualTo(1));
            yield return new WaitForSeconds(.7f);
            Assert.That(handling.State.RawUnits, Is.Zero);
            Assert.That(handling.presentation.FeedbackCues, Is.EqualTo(denied));
            yield return MouseDown(false);
            AimRegion(1);
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.position = new Vector3(-3, 1, -1.9f);
            wall.transform.localScale = new Vector3(2, 2, .1f);
            Physics.SyncTransforms();
            yield return MouseDown(true);
            yield return new WaitForSeconds(.7f);
            Assert.That(handling.State.RawUnits, Is.Zero, "No gathering through solid scenery.");
            wall.SetActive(false); Physics.SyncTransforms();
            yield return MouseDown(false);
            yield return MouseDown(true);
            Assert.That(handling.State.RawUnits, Is.EqualTo(1));
            yield return MouseDown(false); yield return MouseDown(true);
            Assert.That(handling.State.RawUnits, Is.EqualTo(1), "Rapid clicks do not bypass cadence.");
            Conserved();
        }

        [UnityTest]
        public IEnumerator HoldReleaseAndFocusResumeRequireFreshScoopInput()
        {
            yield return PickUp(); AimRegion(1);
            yield return KeyPress(Key.T);
            yield return MouseDown(true); yield return MouseDown(false);
            int stopped = handling.State.RawUnits;
            Assert.That(stopped, Is.EqualTo(1));
            yield return new WaitForSeconds(.7f);
            Assert.That(handling.State.RawUnits, Is.EqualTo(stopped), "Release stops gathering even after pressing the former mode key.");
            yield return MouseDown(true);
            yield return new WaitForSeconds(.6f);
            Assert.That(handling.State.RawUnits, Is.GreaterThan(stopped));
            session.SendMessage("OnApplicationFocus", false);
            stopped = handling.State.RawUnits;
            yield return new WaitForSecondsRealtime(.7f);
            Assert.That(handling.State.RawUnits, Is.EqualTo(stopped));
            Assert.That(session.Input.Use.enabled, Is.False);
            session.SendMessage("OnApplicationFocus", true);
            session.Resume();
            yield return new WaitForSecondsRealtime(.7f);
            Assert.That(handling.State.RawUnits, Is.EqualTo(stopped), "Held menu click must not restart scooping.");
            yield return MouseDown(false); yield return MouseDown(true);
            yield return new WaitForSeconds(.6f);
            Assert.That(handling.State.RawUnits, Is.GreaterThan(stopped));
            yield return MouseDown(false);
            stopped = handling.State.RawUnits;
            yield return new WaitForSeconds(.7f);
            Assert.That(handling.State.RawUnits, Is.EqualTo(stopped));
            Conserved();
        }

        [UnityTest]
        public IEnumerator LoadedCratePlacesAtChosenRotationsOnGroundWorktopAndSupportThenTips()
        {
            yield return PickUp();
            handling.State.Gather("gate-mound-1", 7); handling.Render();
            var portable = handling.crate.portable;
            var positions = new[] { new Vector3(.3f, 0, -3.8f), new Vector3(1.3f, 0, -4.1f),
                new Vector3(-5.7f, .84f, -3.9f), new Vector3(-5.8f, .5f, -5.6f) };
            var approaches = new[] { new Vector3(.3f, .04f, -5.6f), new Vector3(1.3f, .04f, -5.9f),
                new Vector3(-3.8f, .04f, -3.9f), new Vector3(-3.8f, .04f, -5.6f) };
            for (int i = 0; i < positions.Length; i++)
            {
                Aim(approaches[i], positions[i]);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.X));
                yield return new WaitForSeconds(.25f);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState());
                yield return null; yield return null;
                Assert.That(portable.PlacementValid, Is.True, portable.PlacementReason + " at " + i);
                Assert.That(portable.preview.enabled, Is.False);
                Assert.That(session.hud.targetText.text, Is.Empty, "Aiming placement stays visually quiet.");
                var chosen = portable.Placement;
                yield return KeyPress(Key.E);
                Assert.That(handling.State.IsHeld, Is.False);
                yield return new WaitForSeconds(.6f);
                Assert.That(Vector3.Distance(portable.Pose.Position, chosen.Position), Is.LessThan(.035f));
                Assert.That(Quaternion.Angle(portable.Pose.Rotation, chosen.Rotation), Is.LessThan(2));
                Assert.That(portable.Settled, Is.True);
                Assert.That(handling.State.RawUnits, Is.EqualTo(7));
                // Returning the player must preserve this valid arrangement.
                session.ResetToSpawn();
                Assert.That(Vector3.Distance(portable.Pose.Position, chosen.Position), Is.LessThan(.035f));
                Aim(approaches[i], portable.Pose.Position + Vector3.up * .3f);
                yield return null; yield return null;
                Assert.That(session.targeting.Current, Is.SameAs(handling.crate.target));
                yield return RightClick();
                Assert.That(handling.State.IsHeld, Is.True);
            }
            Aim(new Vector3(2.65f, .04f, -2.3f), handling.station.intake.position);
            yield return null; yield return null;
            yield return KeyPress(Key.E);
            Assert.That(handling.State.RawUnits, Is.Zero);
            Assert.That(handling.State.ActiveUnits, Is.EqualTo(7));
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
        }

        [UnityTest]
        public IEnumerator NarrowSupportRejectsCarefulPlacementButDropFallsPausesSettlesAndRecoversSameFood()
        {
            yield return PickUp();
            handling.State.Gather("gate-mound-1", 9); handling.State.Tip();
            handling.State.Gather("gate-mound-2", 5); handling.Render();
            var portable = handling.crate.portable;
            var rail = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rail.transform.position = new Vector3(2, .45f, -3);
            rail.transform.localScale = new Vector3(.2f, .9f, 1);
            Physics.SyncTransforms();
            Aim(new Vector3(2, .04f, -5), new Vector3(2, .9f, -3));
            yield return null; yield return null;
            Assert.That(portable.PlacementValid, Is.False);
            Assert.That(portable.PlacementReason, Does.Contain("Support"));
            Assert.That(portable.preview.enabled, Is.False);
            Assert.That(session.hud.targetText.text, Is.Empty);
            yield return KeyPress(Key.E);
            Assert.That(handling.State.IsHeld, Is.True);
            Assert.That(session.hud.noticeText.text, Is.EqualTo(portable.PlacementReason), "Explain refusal only after a placement attempt.");
            float height = portable.Pose.Position.y;
            yield return KeyPress(Key.G);
            Assert.That(handling.State.IsHeld, Is.False);
            Assert.That(portable.body.isKinematic, Is.False);
            session.SendMessage("OnApplicationFocus", false);
            var frozen = portable.Pose;
            var velocity = portable.body.linearVelocity;
            double work = handling.State.BatchRemaining;
            yield return new WaitForSecondsRealtime(.25f);
            Assert.That(portable.Pose.Position, Is.EqualTo(frozen.Position));
            Assert.That(portable.body.linearVelocity, Is.EqualTo(velocity));
            Assert.That(handling.State.BatchRemaining, Is.EqualTo(work));
            session.SendMessage("OnApplicationFocus", true); session.Resume();
            yield return new WaitForSeconds(.9f);
            Assert.That(portable.Pose.Position.y, Is.LessThan(height - .15f));
            Assert.That(portable.Pose.Position.y, Is.GreaterThan(-.12f));
            Assert.That(portable.Settled, Is.True);
            Assert.That(portable.ContactCues, Is.GreaterThan(0));
            Assert.That(handling.State.RawUnits, Is.EqualTo(5));
            var safe = handling.State.SafeRawPose;
            session.Pause("Recovery check");
            work = handling.State.BatchRemaining;
            portable.body.position = new Vector3(0, -8, 0);
            portable.transform.position = portable.body.position;
            handling.Recover();
            Assert.That(Vector3.Distance(portable.Pose.Position, safe.Position), Is.LessThan(.05f));
            Assert.That(handling.State.RawUnits, Is.EqualTo(5));
            Assert.That(handling.State.ActiveUnits, Is.EqualTo(9));
            Assert.That(handling.State.BatchRemaining, Is.EqualTo(work));
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
            Assert.That(Object.FindObjectsByType<RawCarrierView>().Length, Is.EqualTo(1));
            Assert.That(portable.body.linearVelocity, Is.EqualTo(Vector3.zero));
            session.hud.restartPrototypeButton.onClick.Invoke();
            Assert.That(session.IsPaused, Is.True);
            Assert.That(handling.State.Remaining, Is.EqualTo(107));
            Assert.That(handling.State.RawUnits + handling.State.ActiveUnits, Is.Zero);
        }

        [UnityTest]
        public IEnumerator DropWinsOverTipAndToppledLoadUsesClearFallbackWhenSafePoseIsBlocked()
        {
            yield return PickUp();
            handling.State.Gather("gate-mound-1", 6); handling.Render();
            Aim(new Vector3(2.65f, .04f, -2.3f), handling.station.intake.position);
            yield return null; yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E, Key.G));
            yield return null; yield return null;
            Assert.That(handling.State.IsHeld, Is.False);
            Assert.That(handling.State.RawUnits, Is.EqualTo(6));
            Assert.That(handling.State.ActiveUnits, Is.Zero, "Dropping beside the intake must not transfer food.");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            var portable = handling.crate.portable;
            // Perturb the released body to exercise a loaded side landing, not just an upright drop.
            portable.SetPose(new CarrierPose(new Vector3(2, 1.2f, -4), Quaternion.Euler(0, 25, 90)), false);
            yield return new WaitForSeconds(1.3f);
            Assert.That(Vector3.Dot(portable.transform.up, Vector3.up), Is.LessThan(.5f));
            Assert.That(handling.State.RawUnits, Is.EqualTo(6));
            Aim(new Vector3(2, .04f, -6), portable.Pose.Position + portable.Pose.Rotation * portable.shape.center);
            yield return null; yield return null;
            yield return RightClick();
            Assert.That(handling.State.IsHeld, Is.True, "A toppled crate remains grabbable.");
            Aim(new Vector3(2, .04f, -6), new Vector3(2, 0, -4.1f));
            yield return null; yield return null;
            yield return KeyPress(Key.E);
            Assert.That(handling.State.IsHeld, Is.False);
            var safe = handling.State.SafeRawPose;
            var blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = safe.Position + Vector3.up * .5f;
            blocker.transform.localScale = new Vector3(1.4f, 1, 1.4f);
            var blockerPosition = blocker.transform.position;
            Physics.SyncTransforms();
            session.Pause("Blocked recovery check");
            portable.body.position = new Vector3(0, -8, 0);
            portable.transform.position = portable.body.position;
            handling.Recover();
            Assert.That(portable.Clear(portable.Pose) && portable.Supported(portable.Pose), Is.True);
            Assert.That(Vector3.Distance(portable.Pose.Position, safe.Position), Is.GreaterThan(1));
            Assert.That(blocker.transform.position, Is.EqualTo(blockerPosition));
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
            Assert.That(handling.State.RawUnits, Is.EqualTo(6));
        }

        [UnityTest]
        public IEnumerator HeldWallContactSprintJumpAndFreshReleaseCannotPushPlayerOrReplayInput()
        {
            yield return PickUp();
            handling.State.Gather("gate-mound-1", 6); handling.Render();
            Aim(new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2));
            yield return new WaitForSeconds(.1f);
            var start = session.player.transform.position;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.LeftShift, Key.Space));
            yield return new WaitForSeconds(.15f);
            Assert.That(session.player.transform.position.y, Is.GreaterThan(.4f));
            Assert.That(session.player.transform.position.z - start.z, Is.GreaterThan(.6f));
            Assert.That(handling.State.IsHeld, Is.True);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            Aim(new Vector3(0, .04f, -8.5f), new Vector3(0, 1.65f, -10));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.LeftShift));
            yield return new WaitForSeconds(.3f);
            Assert.That(session.player.transform.position.z, Is.GreaterThan(-8.65f));
            Assert.That(handling.crate.portable.Clear(handling.crate.portable.Pose, false), Is.True);
            Assert.That(handling.crate.portable.body.isKinematic, Is.True);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.G));
            session.Pause("Held release");
            yield return null; yield return null;
            session.Resume();
            yield return new WaitForSeconds(.2f);
            Assert.That(handling.State.IsHeld, Is.True, "G held across a menu cannot release.");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null; yield return null;
            yield return KeyPress(Key.G);
            Assert.That(handling.State.IsHeld, Is.False);
            Assert.That(handling.State.RawUnits, Is.EqualTo(6));
            handling.crate.portable.body.position = new Vector3(0, -8, 0);
            handling.crate.transform.position = new Vector3(0, -8, 0);
            yield return null; yield return null;
            Assert.That(handling.crate.portable.InBounds, Is.True);
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
        }
    }
}
