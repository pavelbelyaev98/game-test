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
    public class MachineOperationSceneTests
    {
        const string Scene = "Assets/JustAFewPeppers/Scenes/PepperYard.unity";
        YardSession session;
        MachineOperation machine;
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
            session = Object.FindAnyObjectByType<YardSession>(); machine = session.handling.machine; state = session.handling.State;
            keyboard = InputSystem.AddDevice<Keyboard>(); mouse = InputSystem.AddDevice<Mouse>();
            session.Input.Actions.devices = new InputDevice[] { keyboard, mouse };
            session.SendMessage("OnApplicationFocus", true); yield return KeyPress(Key.Enter); yield return new WaitForSeconds(.7f);
        }
        [UnityTearDown] public IEnumerator Unload()
        {
            SceneManager.SetActiveScene(SceneManager.CreateScene("Operation cleanup"));
            yield return SceneManager.UnloadSceneAsync(Scene);
            InputSystem.RemoveDevice(keyboard); InputSystem.RemoveDevice(mouse); fixture.TearDown();
            AudioListener.volume = volume; LogAssert.NoUnexpectedReceived();
        }
        IEnumerator KeyPress(Key key)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key)); yield return null; yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
        }
        IEnumerator MouseInput(ushort buttons, Vector2 delta = default)
        {
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = buttons, delta = delta }); yield return null; yield return null;
        }
        void PrepareInput(int count)
        {
            state.PickUp(); foreach (var region in session.handling.regions) count -= state.Gather(region.regionId, count);
            Assert.That(count, Is.Zero); state.Tip(); state.Release(state.RawPose, true); session.handling.Render();
        }
        void PrepareOutput(int count)
        {
            PrepareInput(count); MachineTestActions.StartPreparedBatch(state); state.AdvanceProcessing(4); session.handling.Render();
        }
        void Check()
        {
            Assert.That(state.AccountedUnits, Is.EqualTo(107)); Assert.That(state.OutputUnits + state.ActiveUnits, Is.LessThanOrEqualTo(12));
            Assert.That(machine.preparedFood.Count(p => p.gameObject.activeSelf), Is.EqualTo(state.OutputUnits));
        }

        [UnityTest] public IEnumerator MouseDisplacementDrivesRackReversalAndReleaseStopsWithoutATimer()
        {
            PrepareInput(12); MachineTestActions.AimMechanism(session, false); yield return null; yield return null;
            Assert.That(session.targeting.Current, Is.SameAs(machine.operationTarget));
            var position = session.player.transform.position; var view = session.player.view.transform.rotation; var rest = machine.rack.localPosition;
            yield return MouseInput(1); yield return new WaitForSeconds(.25f);
            Assert.That(machine.Engaged, Is.True); Assert.That(state.OperationStroke, Is.Zero); Assert.That(state.ActiveUnits, Is.Zero);
            yield return MouseInput(1, new Vector2(0, 80)); Assert.That(state.OperationStroke, Is.EqualTo(1f / 3).Within(.015f));
            Assert.That(Vector3.Distance(rest, machine.rack.localPosition), Is.GreaterThan(.2f));
            Assert.That(session.player.transform.position, Is.EqualTo(position)); Assert.That(Quaternion.Angle(view, session.player.view.transform.rotation), Is.LessThan(.01f));
            yield return MouseInput(0); var stopped = state.OperationStroke; yield return new WaitForSeconds(.25f);
            Assert.That(state.OperationStroke, Is.EqualTo(stopped));
            yield return MouseInput(1); yield return MouseInput(1, new Vector2(0, -40)); Assert.That(state.OperationStroke, Is.LessThan(stopped));
            yield return MouseInput(0); yield return MouseInput(2); yield return MouseInput(0);
            Assert.That(state.OperationStroke, Is.Zero); Assert.That(state.QueuedUnits, Is.EqualTo(12));
            yield return MachineTestActions.KeyboardStroke(session, keyboard, false); Assert.That(state.ActiveUnits, Is.EqualTo(12)); Check();
        }

        [UnityTest] public IEnumerator GroupingMovesPreparedMaterialAndKeepsItsSubsetWhenMoreFoodFinishes()
        {
            PrepareOutput(3); PrepareInput(6); MachineTestActions.StartPreparedBatch(state);
            MachineTestActions.AimMechanism(session, true); yield return null; yield return null;
            var rest = machine.preparedFood[0].localPosition;
            yield return MouseInput(1); yield return MouseInput(1, new Vector2(80, 0));
            Assert.That(state.GroupingSelection, Is.EqualTo(3)); Assert.That(machine.preparedFood[0].localPosition, Is.Not.EqualTo(rest));
            state.AdvanceProcessing(4); session.handling.Render(); Assert.That(state.OutputUnits, Is.EqualTo(9)); Assert.That(state.GroupingSelection, Is.EqualTo(3));
            yield return MouseInput(0); Assert.That(state.FinishedUnits, Is.Zero);
            yield return MachineTestActions.KeyboardStroke(session, keyboard, true); Assert.That(state.FinishedUnits, Is.EqualTo(3));
            Assert.That(state.OutputUnits, Is.EqualTo(6)); Assert.That(state.GroupsCompleted, Is.EqualTo(1));
            session.SendMessage("OnApplicationFocus", false); session.SendMessage("OnApplicationFocus", true); session.Resume();
            yield return new WaitForSeconds(.2f); Assert.That(state.FinishedUnits, Is.EqualTo(3)); Assert.That(state.GroupsCompleted, Is.EqualTo(1)); Check();
        }

        [UnityTest] public IEnumerator PauseAndFocusCancelUncommittedStrokesAndRequireFreshInput()
        {
            foreach (bool grouping in new[] { false, true })
            {
                session.RestartPrototype(); yield return null; yield return null;
                if (grouping) PrepareOutput(5); else PrepareInput(5);
                MachineTestActions.AimMechanism(session, grouping); yield return null; yield return null;
                yield return MouseInput(1); yield return MouseInput(1, grouping ? new Vector2(80, 0) : new Vector2(0, 80));
                Assert.That(state.HasStroke, Is.True);
                session.SendMessage("OnApplicationFocus", false); Assert.That(state.HasStroke, Is.False);
                var food = state.AccountedUnits; var time = state.BatchRemaining;
                yield return new WaitForSecondsRealtime(.2f); Assert.That(state.BatchRemaining, Is.EqualTo(time));
                session.SendMessage("OnApplicationFocus", true); session.Resume();
                yield return MouseInput(1, new Vector2(250, 250)); yield return new WaitForSeconds(.2f);
                Assert.That(state.HasStroke, Is.False); Assert.That(state.FinishedUnits + state.ActiveUnits, Is.Zero);
                yield return MouseInput(0); yield return MachineTestActions.KeyboardStroke(session, keyboard, grouping);
                Assert.That(state.AccountedUnits, Is.EqualTo(food)); Check();
            }
        }

        [UnityTest] public IEnumerator BlockedAndOccupiedTargetsCannotOperateOrRepeatDenialAudio()
        {
            PrepareInput(5); var point = machine.operationTarget.transform.position;
            MachineTestActions.Aim(session, new Vector3(point.x, .04f, point.z - 4), point);
            yield return KeyPress(Key.E); Assert.That(state.HasStroke, Is.False);
            MachineTestActions.AimMechanism(session, false);
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube); wall.transform.position = point + Vector3.back * .55f;
            wall.transform.localScale = new Vector3(1, 3, .08f); Physics.SyncTransforms(); yield return null; yield return KeyPress(Key.E);
            Assert.That(state.HasStroke, Is.False); wall.SetActive(false); Object.Destroy(wall); yield return null; yield return null;
            state.PickUp(); session.handling.Render(); yield return KeyPress(Key.E);
            Assert.That(state.IsHeld, Is.True); Assert.That(state.HasStroke, Is.False);
            state.Release(state.RawPose, true); MachineTestActions.AimMechanism(session, true); yield return null; yield return null;
            yield return KeyPress(Key.E); int cues = machine.ContactCues; yield return KeyPress(Key.E); yield return KeyPress(Key.E);
            Assert.That(machine.ContactCues, Is.EqualTo(cues)); Assert.That(AudioListener.volume, Is.Zero); Check();
        }

        [UnityTest] public IEnumerator RealPourWaitsForOperationThenGroupingAndHandoffAcceptFullAndPartialLoads()
        {
            foreach (int count in new[] { 12, 1 })
            {
                state.PickUp(); int left = count; foreach (var region in session.handling.regions) left -= state.Gather(region.regionId, left);
                session.handling.Render();
                MachineTestActions.Aim(session, new Vector3(2.65f, .04f, -2.3f), session.handling.station.intake.position);
                yield return null; yield return null; yield return KeyPress(Key.E); yield return new WaitForSeconds(1.3f);
                Assert.That(state.QueuedUnits, Is.EqualTo(count)); Assert.That(state.ActiveUnits, Is.Zero);
                yield return KeyPress(Key.G); yield return MachineTestActions.KeyboardStroke(session, keyboard, false);
                yield return new WaitForSeconds(4.1f); Assert.That(state.OutputUnits, Is.EqualTo(count));
                yield return MachineTestActions.KeyboardStroke(session, keyboard, true); yield return new WaitForSeconds(.5f);
                MachineTestActions.Aim(session, new Vector3(3, .04f, 3.2f), session.handling.finished.rackTarget.transform.position + Vector3.up);
                yield return null; yield return null; yield return KeyPress(Key.E); yield return KeyPress(Key.E); Check();
            }
            Assert.That(state.StoredUnits, Is.EqualTo(13)); Assert.That(state.OperationsCompleted, Is.EqualTo(2)); Assert.That(state.GroupsCompleted, Is.EqualTo(2));
        }
    }
}
