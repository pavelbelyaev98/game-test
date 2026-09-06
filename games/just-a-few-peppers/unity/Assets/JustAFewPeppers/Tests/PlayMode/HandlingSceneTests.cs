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
            yield return KeyPress(Key.E);
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
            Assert.That(session.Input.Scoop.enabled, Is.False);
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
        public IEnumerator LoadedCrateSurvivesSprintJumpParkingFallRecoveryAndPrototypeReset()
        {
            yield return PickUp(); AimRegion(1);
            yield return MouseDown(true); yield return new WaitForSeconds(1.1f); yield return MouseDown(false);
            int load = handling.State.RawUnits;
            Assert.That(load, Is.GreaterThanOrEqualTo(3));
            Aim(new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2));
            yield return new WaitForSeconds(.1f);
            var start = session.player.transform.position;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.LeftShift, Key.Space));
            yield return new WaitForSeconds(.15f);
            Assert.That(session.player.transform.position.y, Is.GreaterThan(.4f));
            Assert.That(session.player.transform.position.z - start.z, Is.GreaterThan(.6f));
            Assert.That(handling.State.IsHeld, Is.True);
            Assert.That(Vector3.Distance(handling.crate.transform.position, handling.crate.carryAnchor.position), Is.LessThan(.001f));
            yield return KeyPress(Key.E);
            Assert.That(handling.State.IsHeld, Is.True, "No parking in midair.");
            yield return new WaitForSeconds(.7f);
            Aim(new Vector3(-1.25f, .04f, -4.1f), handling.crate.restingPoints[0].position);
            yield return new WaitForSeconds(.1f);
            var obstruction = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obstruction.transform.position = handling.crate.restingPoints[0].position + Vector3.up * .4f;
            obstruction.transform.localScale = Vector3.one * .6f;
            Physics.SyncTransforms();
            yield return KeyPress(Key.E);
            Assert.That(handling.State.IsHeld, Is.True, "An occupied mat cannot accept a crate.");
            obstruction.SetActive(false); Physics.SyncTransforms();
            yield return KeyPress(Key.E);
            Assert.That(handling.State.IsHeld, Is.False);
            Assert.That(handling.State.RawUnits, Is.EqualTo(load));
            Assert.That(handling.crate.parkedColliders.All(c => c.enabled), Is.True);
            // Corrupting a view cannot move its authoritative rest point or harvest.
            handling.crate.transform.position = new Vector3(0, -50, 0);
            yield return null; yield return null;
            Assert.That(handling.crate.transform.position, Is.EqualTo(handling.crate.restingPoints[0].position));
            yield return PickUp();
            Aim(new Vector3(0, 2.4f, 8), new Vector3(0, 3, 10));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.LeftShift));
            yield return new WaitForSeconds(.3f);
            Assert.That(session.player.transform.position.z, Is.LessThan(8.65f));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            session.player.body.enabled = false;
            session.player.transform.position = new Vector3(0, -5, 0);
            session.player.body.enabled = true;
            yield return null; yield return null;
            Assert.That(handling.State.IsHeld, Is.False);
            Assert.That(handling.State.RawUnits, Is.EqualTo(load));
            Assert.That(Vector3.Distance(session.player.transform.position, session.safeSpawn.position), Is.LessThan(.1f));
            Conserved();
            yield return KeyPress(Key.Escape);
            session.hud.restartPrototypeButton.onClick.Invoke();
            Assert.That(session.IsPaused, Is.True);
            Assert.That(handling.State.RawUnits, Is.Zero);
            Assert.That(handling.State.Remaining, Is.EqualTo(107));
            Assert.That(handling.regions[0].volume.gameObject.activeSelf, Is.True);
            Conserved();
        }
    }
}
