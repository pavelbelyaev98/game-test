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
    public class ProcessingSceneTests
    {
        const string ScenePath = "Assets/JustAFewPeppers/Scenes/PepperYard.unity";
        YardSession session;
        YardHandling handling;
        Keyboard keyboard;
        Mouse mouse;
        InputTestFixture fixture;
        float previousVolume;

        [UnitySetUp]
        public IEnumerator Load()
        {
            previousVolume = AudioListener.volume;
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
                SceneManager.SetActiveScene(SceneManager.CreateScene("Processing cleanup"));
                yield return SceneManager.UnloadSceneAsync(ScenePath);
                InputSystem.RemoveDevice(keyboard); InputSystem.RemoveDevice(mouse);
                fixture.TearDown();
                LogAssert.NoUnexpectedReceived();
            }
            finally { AudioListener.volume = previousVolume; }
        }

        IEnumerator KeyPress(Key key)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            yield return null; yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null; yield return null;
        }

        void Aim(Vector3 position, Vector3 target)
        {
            var pose = new GameObject("Processing approach").transform;
            pose.position = position;
            var direction = target - (position + Vector3.up * 1.65f);
            pose.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
            session.player.ResetTo(pose);
            Object.Destroy(pose.gameObject);
            float pitch = Mathf.Atan2(-direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;
            session.player.Step(Vector2.zero, new Vector2(0, -pitch / session.player.lookSensitivity), false, false, 1f / 60);
            Physics.SyncTransforms(); session.targeting.Refresh();
        }

        void AimIntake()
        {
            Aim(new Vector3(2.65f, .04f, -1.8f), handling.station.intake.position);
            Assert.That(session.targeting.Current, Is.SameAs(handling.station.intakeTarget));
        }

        // Tipping tests prepare loads via public transactions; actual gathering input has its own integration tests.
        void Fill(int count)
        {
            handling.State.PickUp();
            foreach (var region in handling.regions)
                count -= handling.State.Gather(region.regionId, count);
            Assert.That(count, Is.Zero);
            handling.Render();
        }

        void Conserved()
        {
            var state = handling.State;
            Assert.That(state.AccountedUnits, Is.EqualTo(107));
            Assert.That(state.OutputUnits + state.ActiveUnits, Is.LessThanOrEqualTo(12));
            Assert.That(state.Peppers.Count(p => p.Owner == PepperOwner.Carrier), Is.EqualTo(state.RawUnits));
            Assert.That(handling.station.queuedPeppers.Count(c => c.activeSelf), Is.EqualTo(state.QueuedUnits));
            Assert.That(handling.machine.preparedFood.Count(c => c.gameObject.activeSelf), Is.EqualTo(state.OutputUnits));
        }

        [UnityTest]
        public IEnumerator PhysicalPourInterruptionFreezesBodiesAndCannotRepeatOnFocusReturn()
        {
            Fill(12); AimIntake();
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E)); yield return new WaitForSeconds(.2f);
            Assert.That(handling.State.RawUnits, Is.Zero);
            Assert.That(handling.peppers.Pouring, Is.True);
            Assert.That(handling.peppers.Bodies.Any(p => p.Record.Owner == PepperOwner.Transit), Is.True);
            session.SendMessage("OnApplicationFocus", false);
            var positions = handling.peppers.Bodies.Select(p => p.transform.position).ToArray();
            double remaining = handling.State.BatchRemaining;
            yield return new WaitForSecondsRealtime(.3f);
            Assert.That(handling.State.BatchRemaining, Is.EqualTo(remaining));
            Assert.That(handling.peppers.Bodies.Select(p => p.transform.position).ToArray(), Is.EqualTo(positions));
            Assert.That(handling.State.Peppers.All(p => p.Owner != PepperOwner.Transit), Is.True);
            session.SendMessage("OnApplicationFocus", true); session.Resume(); Fill(2);
            yield return new WaitForSeconds(.3f); Assert.That(handling.State.RawUnits, Is.EqualTo(2));
            Assert.That(handling.crate.TipBlend, Is.Zero); Conserved();
            yield return KeyPress(Key.F8); Fill(12); AimIntake(); yield return null; yield return null;
            yield return KeyPress(Key.E); yield return new WaitForSeconds(1.3f);
            MachineTestActions.StartPreparedBatch(handling.State); yield return new WaitForSeconds(4.1f);
            Assert.That(handling.State.OutputUnits, Is.EqualTo(12));
            Assert.That(handling.station.CompletionCues, Is.EqualTo(1)); Conserved();
        }

        [UnityTest]
        public IEnumerator PreparedOutputAccumulatesReservesRoomAndLimitedInputRetainsTheRemainderQuietly()
        {
            Fill(1); AimIntake(); yield return null; yield return null; yield return KeyPress(Key.E);
            yield return new WaitForSeconds(1.3f);
            MachineTestActions.StartPreparedBatch(handling.State); yield return new WaitForSeconds(4.1f);
            Assert.That(handling.State.OutputUnits, Is.EqualTo(1));
            Assert.That(handling.machine.preparedFood.Count(p => p.gameObject.activeSelf), Is.EqualTo(1));
            Fill(12); yield return null; yield return null; yield return KeyPress(Key.E); yield return new WaitForSeconds(1.3f);
            MachineTestActions.StartPreparedBatch(handling.State);
            Assert.That(handling.State.ActiveUnits, Is.EqualTo(11)); Assert.That(handling.State.QueuedUnits, Is.EqualTo(1));
            yield return new WaitForSeconds(4.1f);
            Fill(12); yield return null; yield return null; yield return KeyPress(Key.E); yield return new WaitForSeconds(1.3f);
            Assert.That(handling.State.QueuedUnits, Is.EqualTo(12)); Assert.That(handling.State.RawUnits, Is.EqualTo(1));
            yield return KeyPress(Key.E); int denied = handling.presentation.FeedbackCues;
            yield return KeyPress(Key.E); yield return new WaitForSeconds(.5f);
            Assert.That(handling.presentation.FeedbackCues, Is.EqualTo(denied));
            Assert.That(handling.State.RawUnits, Is.EqualTo(1)); Conserved();
        }

        [UnityTest]
        public IEnumerator OcclusionAndRecoveryKeepUnacceptedFoodAndPrototypeResetClearsAllOwners()
        {
            Fill(5); Aim(new Vector3(2.65f, .04f, -5), handling.station.intake.position);
            yield return KeyPress(Key.E); Assert.That(handling.State.RawUnits, Is.EqualTo(5));
            AimIntake(); var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.position = new Vector3(2.65f, 1, -.9f); wall.transform.localScale = new Vector3(2, 2, .1f); Physics.SyncTransforms();
            yield return KeyPress(Key.E); Assert.That(handling.State.RawUnits, Is.EqualTo(5));
            wall.SetActive(false); Physics.SyncTransforms(); yield return null; yield return null;
            yield return KeyPress(Key.E); yield return KeyPress(Key.R);
            Assert.That(handling.State.IsHeld, Is.False);
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
            Assert.That(handling.State.UncontainedUnits, Is.Zero);
            yield return KeyPress(Key.Escape); session.hud.restartPrototypeButton.onClick.Invoke();
            Assert.That(session.IsPaused, Is.True); Assert.That(handling.State.Remaining, Is.EqualTo(107));
            Assert.That(handling.State.ActiveUnits + handling.State.QueuedUnits + handling.State.OutputUnits, Is.Zero);
            Assert.That(handling.State.BatchRemaining, Is.Zero); Conserved();
        }
    }
}
