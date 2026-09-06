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
    public class PepperBatchSceneTests
    {
        const string Scene = "Assets/JustAFewPeppers/Scenes/PepperYard.unity";
        YardSession session;
        PepperBatch batch;
        HarvestState state;
        Keyboard keyboard;
        Mouse mouse;
        InputTestFixture fixture;
        float volume;

        [UnitySetUp] public IEnumerator Load()
        {
            volume = AudioListener.volume; AudioListener.volume = 0;
            fixture = new InputTestFixture(); fixture.Setup();
            yield return SceneManager.LoadSceneAsync(Scene); yield return null;
            session = Object.FindAnyObjectByType<YardSession>(); batch = session.handling.peppers; state = session.handling.State;
            keyboard = InputSystem.AddDevice<Keyboard>(); mouse = InputSystem.AddDevice<Mouse>();
            session.Input.Actions.devices = new InputDevice[] { keyboard, mouse };
            session.SendMessage("OnApplicationFocus", true); yield return Key(KeyCodeReturn);
            yield return new WaitForSeconds(.8f);
        }
        const UnityEngine.InputSystem.Key KeyCodeReturn = UnityEngine.InputSystem.Key.Enter;
        [UnityTearDown] public IEnumerator Unload()
        {
            SceneManager.SetActiveScene(SceneManager.CreateScene("Pepper cleanup"));
            yield return SceneManager.UnloadSceneAsync(Scene);
            InputSystem.RemoveDevice(keyboard); InputSystem.RemoveDevice(mouse); fixture.TearDown();
            AudioListener.volume = volume; LogAssert.NoUnexpectedReceived();
        }
        IEnumerator Key(UnityEngine.InputSystem.Key key)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key)); yield return null; yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
        }
        IEnumerator Click(ushort buttons = 2)
        {
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = buttons }); yield return null; yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null; yield return null;
        }
        void Aim(Vector3 position, Vector3 target)
        {
            var pose = new GameObject("Approach").transform; pose.position = position;
            var direction = target - (position + Vector3.up * 1.65f);
            pose.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
            session.player.ResetTo(pose); Object.Destroy(pose.gameObject);
            float pitch = Mathf.Atan2(-direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;
            session.player.Step(Vector2.zero, new Vector2(0, -pitch / session.player.lookSensitivity), false, false, 1f / 60);
            Physics.SyncTransforms(); session.targeting.Refresh();
        }
        IEnumerator AimPepper(PepperBody p)
        {
            Aim(p.transform.position + new Vector3(0, .04f - p.transform.position.y, -1.05f), p.transform.position);
            yield return null; yield return null;
        }
        IEnumerator GrabCrate()
        {
            Aim(new Vector3(-1.25f, .04f, -4.4f), session.handling.crate.transform.position + Vector3.up * .3f);
            yield return null; yield return null; yield return Click(); Assert.That(state.IsHeld, Is.True);
        }
        void Check() { Assert.That(state.AccountedUnits, Is.EqualTo(107)); Assert.That(batch.Bodies.Select(p => p.Record.Id).Distinct().Count(), Is.EqualTo(107)); }

        [UnityTest] public IEnumerator SingleGroundAndExposedContentsPickupNeverGrabsTheContainer()
        {
            var p = batch.Bodies[0]; yield return AimPepper(p);
            Assert.That(batch.Target, Is.SameAs(p)); yield return Click(); Assert.That(batch.Held, Is.SameAs(p));
            Assert.That(state.Remaining, Is.EqualTo(106));
            Aim(new Vector3(-1.25f, .04f, -4.0f), session.handling.crate.transform.position + Vector3.up * .1f);
            yield return null; yield return null; yield return Key(UnityEngine.InputSystem.Key.E); yield return new WaitForSeconds(.8f);
            Assert.That(state.RawUnits, Is.EqualTo(1)); Assert.That(batch.Held, Is.Null);
            var floor = session.handling.crate.transform.Find("Crate base").GetComponent<Renderer>();
            Assert.That(p.skin.bounds.max.y, Is.GreaterThan(floor.bounds.max.y + .04f), "Settled contents must remain above the visible crate floor.");
            Aim(new Vector3(-1.25f, .04f, -3.7f), p.transform.position); yield return null; yield return null;
            Assert.That(batch.Target, Is.SameAs(p), "Exposed contents have precise priority.");
            yield return Click(); Assert.That(batch.Held, Is.SameAs(p)); Assert.That(state.IsHeld, Is.False); Assert.That(state.RawUnits, Is.Zero);
            Aim(new Vector3(0, .04f, -5), new Vector3(0, 1.5f, -2)); yield return null; yield return null;
            float y = p.transform.position.y; yield return Click(); yield return new WaitForSeconds(.25f);
            Assert.That(p.transform.position.y, Is.LessThan(y - .1f)); Check();
        }

        [UnityTest] public IEnumerator PreviewShowsOnlyTheCapacityLimitedVisibleSetAndReleaseStopsGathering()
        {
            yield return GrabCrate();
            state.Gather("gate-mound-8", 10); session.handling.Render();
            yield return AimPepper(batch.Bodies[0]); yield return new WaitForSeconds(.2f);
            var shown = batch.Preview.ToArray(); Assert.That(shown.Length, Is.EqualTo(2));
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.35f);
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null; yield return new WaitForSeconds(.6f);
            Assert.That(state.RawUnits, Is.EqualTo(12));
            foreach (var id in shown) Assert.That(state.Pepper(id).Owner, Is.EqualTo(PepperOwner.Carrier));
            Assert.That(batch.Preview, Is.Empty); Assert.That(session.handling.crate.portable.preview.enabled, Is.False); Check();
        }

        [UnityTest] public IEnumerator OccludedPepperAndBulkSetCannotBeTakenThroughAWall()
        {
            yield return GrabCrate(); var p = batch.Bodies[0]; yield return AimPepper(p);
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube); wall.transform.position = new Vector3(p.transform.position.x, .7f, p.transform.position.z - .5f);
            wall.transform.localScale = new Vector3(1.5f, 1.4f, .12f); Physics.SyncTransforms();
            yield return null; yield return null; Assert.That(batch.Target, Is.Null); Assert.That(batch.Preview, Is.Empty);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.7f);
            Assert.That(state.RawUnits, Is.Zero); Object.Destroy(wall); Check();
        }

        [UnityTest] public IEnumerator FullPhysicalPourContactsIntakeAndProducesOneRecoverableBatch()
        {
            yield return GrabCrate(); state.Gather("gate-mound-1", 12); session.handling.Render();
            Aim(new Vector3(2.65f, .04f, -2.3f), session.handling.station.intake.position); yield return null; yield return null;
            yield return Key(UnityEngine.InputSystem.Key.E); Assert.That(state.UncontainedUnits, Is.GreaterThan(0));
            yield return new WaitForSeconds(1.3f);
            Assert.That(batch.IntakeContacts, Is.EqualTo(12)); Assert.That(state.RawUnits, Is.Zero); Assert.That(state.QueuedUnits, Is.EqualTo(12)); Assert.That(state.ActiveUnits, Is.Zero);
            MachineTestActions.StartPreparedBatch(state);
            yield return new WaitForSeconds(4.1f); Assert.That(state.OutputUnits, Is.EqualTo(12)); Check();
        }

        [UnityTest] public IEnumerator MissedPourPauseAndStrayRecoveryKeepEveryIdentityAndRemainingFood()
        {
            yield return GrabCrate(); state.Gather("gate-mound-1", 5); session.handling.Render();
            Aim(new Vector3(0, .04f, -5), new Vector3(0, 1.5f, -2)); yield return null; yield return null;
            yield return Key(UnityEngine.InputSystem.Key.F); Assert.That(state.UncontainedUnits, Is.EqualTo(5));
            var spilled = batch.Bodies.First(p => p.Record.Owner == PepperOwner.Loose);
            yield return Key(UnityEngine.InputSystem.Key.Escape); var pose = spilled.transform.position;
            yield return new WaitForSecondsRealtime(.2f); Assert.That(spilled.transform.position, Is.EqualTo(pose));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(UnityEngine.InputSystem.Key.F)); yield return null;
            yield return Key(UnityEngine.InputSystem.Key.Escape); yield return new WaitForSeconds(.2f);
            yield return Key(UnityEngine.InputSystem.Key.R);
            Assert.That(state.Remaining, Is.EqualTo(107)); Assert.That(state.UncontainedUnits, Is.Zero); Check();
        }

        [UnityTest] public IEnumerator BothRepresentationsKeepNearbySinglePickupAndConservedMembership()
        {
            foreach (var mode in new[] { PepperSimulation.PhysicalBatch, PepperSimulation.GroupedRest })
            {
                batch.simulation = mode; yield return Key(UnityEngine.InputSystem.Key.R);
                yield return AimPepper(batch.Bodies[0]); yield return new WaitForSeconds(.2f);
                Assert.That(batch.Target, Is.SameAs(batch.Bodies[0])); yield return Click();
                Assert.That(batch.Held, Is.SameAs(batch.Bodies[0])); Check(); yield return Key(UnityEngine.InputSystem.Key.R);
            }
        }
    }
}
