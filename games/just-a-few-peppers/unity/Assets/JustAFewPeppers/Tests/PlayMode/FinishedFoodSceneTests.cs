using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace JustAFewPeppers.Tests
{
    public class FinishedFoodSceneTests
    {
        const string ScenePath = "Assets/JustAFewPeppers/Scenes/PepperYard.unity";
        YardSession session;
        YardHandling handling;
        FinishedFoodHandling finished;
        HarvestState State => handling.State;
        Keyboard keyboard;
        Mouse mouse;
        InputTestFixture fixture;
        float previousVolume;

        [UnitySetUp]
        public IEnumerator Load()
        {
            previousVolume = AudioListener.volume; AudioListener.volume = 0;
            fixture = new InputTestFixture(); fixture.Setup();
            yield return SceneManager.LoadSceneAsync(ScenePath); yield return null;
            session = Object.FindAnyObjectByType<YardSession>();
            handling = session.handling; finished = handling.finished;
            keyboard = InputSystem.AddDevice<Keyboard>(); mouse = InputSystem.AddDevice<Mouse>();
            session.Input.Actions.devices = new InputDevice[] { keyboard, mouse };
            session.SendMessage("OnApplicationFocus", true);
            yield return Press(Key.Enter);
        }

        [UnityTearDown]
        public IEnumerator Unload()
        {
            try
            {
                SceneManager.SetActiveScene(SceneManager.CreateScene("Finished food cleanup"));
                yield return SceneManager.UnloadSceneAsync(ScenePath);
                InputSystem.RemoveDevice(keyboard); InputSystem.RemoveDevice(mouse);
                fixture.TearDown(); LogAssert.NoUnexpectedReceived();
            }
            finally { AudioListener.volume = previousVolume; }
        }

        IEnumerator Press(Key key)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            yield return null; yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
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
            var pose = new GameObject("Finished approach").transform; pose.position = position;
            var direction = target - (position + Vector3.up * 1.65f);
            pose.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
            session.player.ResetTo(pose); Object.Destroy(pose.gameObject);
            float pitch = Mathf.Atan2(-direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;
            session.player.Step(Vector2.zero, new Vector2(0, -pitch / session.player.lookSensitivity), false, false, 1f / 60);
            Physics.SyncTransforms(); session.targeting.Refresh();
        }

        void AimOutput() => Aim(new Vector3(4.45f, .04f, -1.3f), finished.carrier.dock.position + Vector3.up * .2f);
        void AimRack() => Aim(new Vector3(3, .04f, 3.2f), finished.rackTarget.transform.position + Vector3.up);
        void Fill(int count)
        {
            State.PickUp();
            foreach (var region in handling.regions) count -= State.Gather(region.regionId, count);
            Assert.That(count, Is.Zero); handling.Render();
        }
        void PrepareOutput(int amount)
        {
            Fill(amount); State.Tip(); MachineTestActions.StartPreparedBatch(State); State.AdvanceProcessing(4); handling.Render();
        }
        IEnumerator SetDownRaw()
        {
            if (!State.IsHeld) yield break;
            Aim(new Vector3(5.8f, .04f, -5.1f), new Vector3(5.8f, 0, -3.3f));
            yield return null; yield return null; yield return Press(Key.E);
            yield return new WaitForSeconds(.6f);
            Assert.That(State.IsHeld, Is.False);
        }
        IEnumerator Collect()
        {
            yield return SetDownRaw();
            AimOutput(); yield return null; yield return null;
            Assert.That(session.targeting.Current, Is.SameAs(finished.outputTarget));
            yield return MachineTestActions.KeyboardStroke(session, keyboard, true, false); yield return new WaitForSeconds(.5f);
            Assert.That(State.FinishedHeld, Is.True);
        }
        void Conserved()
        {
            Assert.That(State.AccountedUnits, Is.EqualTo(107));
            Assert.That(State.IsHeld && State.FinishedHeld, Is.False);
            Assert.That(Object.FindObjectsByType<FinishedCarrierView>().Length, Is.EqualTo(1));
            Assert.That(Object.FindObjectsByType<RawCarrierView>().Length, Is.EqualTo(1));
            Assert.That(finished.carrier.food.jars.Count(j => j.activeSelf), Is.EqualTo((State.FinishedUnits + 2) / 3));
            Assert.That(finished.storedFood.jars.Count(j => j.activeSelf), Is.EqualTo((State.StoredUnits + 2) / 3));
        }

        [UnityTest]
        public IEnumerator OccupiedHandsRequireSetDownThenReceivingInterruptionCannotReplayPickup()
        {
            PrepareOutput(12); AimOutput();
            yield return null; yield return null;
            Assert.That(session.targeting.Current, Is.SameAs(finished.outputTarget));
            yield return Press(Key.E);
            Assert.That(State.IsHeld, Is.True);
            Assert.That(State.FinishedDocked, Is.True);
            Assert.That(State.OutputUnits, Is.EqualTo(12));
            Assert.That(session.hud.noticeText.text, Does.Contain("Release the held object"));
            yield return SetDownRaw();
            AimOutput(); yield return null; yield return null;
            yield return MachineTestActions.KeyboardStroke(session, keyboard, true, false);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E));
            yield return new WaitForSeconds(.12f);
            Assert.That(finished.carrier.IsReceiving, Is.True);
            Assert.That(State.FinishedUnits, Is.EqualTo(12));
            var rawPose = State.RawPose;
            Assert.That(handling.crate.portable.Supported(rawPose), Is.True);
            session.SendMessage("OnApplicationFocus", false);
            Assert.That(finished.carrier.IsReceiving, Is.False);
            session.SendMessage("OnApplicationFocus", true); session.Resume();
            yield return new WaitForSeconds(.25f);
            Assert.That(State.FinishedUnits, Is.EqualTo(12));
            Assert.That(State.OutputUnits, Is.Zero);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null; yield return null;
            AimRack(); yield return null; yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E));
            yield return null; yield return null;
            session.Pause("Interrupted handoff"); session.Resume();
            yield return new WaitForSeconds(.2f);
            Assert.That(State.StoredUnits, Is.EqualTo(12));
            Assert.That(finished.DepositCues, Is.EqualTo(1));
            Assert.That(State.FinishedDocked, Is.True);
            Assert.That(Vector3.Distance(handling.crate.portable.Pose.Position, rawPose.Position), Is.LessThan(.04f));
            Conserved();
        }

        [UnityTest]
        public IEnumerator FinishedLoadPlacesRotatesDropsPausesAndRecoversWithoutMovingRawArrangement()
        {
            PrepareOutput(7); yield return Collect();
            var carrier = finished.carrier; var body = carrier.portable;
            var raw = handling.crate.portable.Pose;
            var points = new[] { new Vector3(.3f, 0, -3.8f), new Vector3(-5.7f, .84f, -3.9f) };
            var approaches = new[] { new Vector3(.3f, .04f, -5.6f), new Vector3(-3.8f, .04f, -3.9f) };
            for (int i = 0; i < points.Length; i++)
            {
                Aim(approaches[i], points[i]);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.X));
                yield return new WaitForSeconds(.2f);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
                Assert.That(body.PlacementValid, Is.True, body.PlacementReason);
                Assert.That(session.hud.targetText.text, Is.Empty);
                var chosen = body.Placement;
                yield return Press(Key.E); yield return new WaitForSeconds(.6f);
                Assert.That(State.FinishedHeld, Is.False);
                Assert.That(Vector3.Distance(body.Pose.Position, chosen.Position), Is.LessThan(.035f));
                Assert.That(Quaternion.Angle(body.Pose.Rotation, chosen.Rotation), Is.LessThan(2));
                Assert.That(body.Settled, Is.True);
                session.ResetToSpawn();
                Assert.That(Vector3.Distance(body.Pose.Position, chosen.Position), Is.LessThan(.035f));
                Aim(approaches[i], body.Pose.Position + Vector3.up * .24f); yield return null; yield return null;
                Assert.That(session.targeting.Current, Is.SameAs(carrier.target), "Jar details must not steal the carrier target.");
                yield return RightClick();
            }
            Aim(new Vector3(2, .04f, -5), new Vector3(2, 1.65f, -2)); yield return null; yield return null;
            Assert.That(body.PlacementValid, Is.False);
            yield return Press(Key.G);
            Assert.That(State.FinishedHeld, Is.False);
            session.SendMessage("OnApplicationFocus", false); var frozen = body.Pose;
            yield return new WaitForSecondsRealtime(.2f);
            Assert.That(body.Pose.Position, Is.EqualTo(frozen.Position));
            session.SendMessage("OnApplicationFocus", true); session.Resume();
            yield return new WaitForSeconds(1);
            Assert.That(body.Settled, Is.True);
            Assert.That(body.ContactCues, Is.GreaterThan(0));
            var safe = State.SafeFinishedPose;
            var blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = safe.Position + Vector3.up * .5f;
            blocker.transform.localScale = new Vector3(1.4f, 1, 1.4f);
            body.SetPose(new CarrierPose(new Vector3(0, -8, 0), Quaternion.Euler(0, 0, 90)), false);
            Physics.SyncTransforms(); yield return null; yield return null;
            Assert.That(body.InBounds, Is.True);
            Assert.That(Vector3.Distance(body.Pose.Position, safe.Position), Is.GreaterThan(1));
            Assert.That(State.FinishedUnits, Is.EqualTo(7));
            Assert.That(Vector3.Distance(handling.crate.portable.Pose.Position, raw.Position), Is.LessThan(.04f));
            Aim(body.Pose.Position + new Vector3(0, .04f, -2), body.Pose.Position + Vector3.up * .24f);
            yield return null; yield return null; yield return RightClick();
            Assert.That(State.FinishedHeld, Is.True);
            AimRack(); yield return null; yield return null; yield return Press(Key.E);
            Assert.That(State.StoredUnits, Is.EqualTo(7)); Conserved();
        }

        [UnityTest]
        public IEnumerator ParkedFinishedLoadAllowsMoreOutputAndExplicitReleasesNeverSwapOrDeposit()
        {
            PrepareOutput(5); yield return Collect();
            Aim(new Vector3(6.6f, .04f, 4), new Vector3(4.8f, 0, 4)); yield return null; yield return null;
            Assert.That(finished.carrier.portable.PlacementValid, Is.True);
            yield return Press(Key.E); yield return new WaitForSeconds(.5f);
            Assert.That(State.StoredUnits, Is.Zero, "Placing beside the rack cannot hand off food.");
            var raw = handling.crate.portable.Pose.Position;
            Aim(raw + new Vector3(0, .04f, -1.8f), raw + Vector3.up * .3f);
            yield return null; yield return null; yield return RightClick();
            Assert.That(State.IsHeld, Is.True);
            Fill(12); State.Tip(); MachineTestActions.StartPreparedBatch(State); State.AdvanceProcessing(4); handling.Render();
            Assert.That(State.OutputUnits, Is.EqualTo(12));
            Assert.That(State.FinishedUnits, Is.EqualTo(5));
            AimOutput(); yield return null; yield return null; yield return Press(Key.E);
            Assert.That(State.IsHeld, Is.True);
            Assert.That(State.OutputUnits, Is.EqualTo(12));
            var parked = finished.carrier.portable.Pose.Position;
            Aim(new Vector3(6.6f, .04f, 4), parked + Vector3.up * .24f);
            yield return null; yield return null; yield return RightClick();
            Assert.That(State.IsHeld || State.FinishedHeld, Is.False, "RMB releases the current crate; it does not swap hands.");
            Assert.That(State.FinishedPose.Position, Is.EqualTo(parked));
            yield return new WaitForSeconds(1);
            yield return RightClick();
            Assert.That(State.FinishedHeld, Is.True);
            raw = handling.crate.portable.Pose.Position;
            Aim(raw + new Vector3(0, .04f, -1.8f), raw + Vector3.up * .3f);
            yield return null; yield return null; yield return RightClick();
            Assert.That(State.IsHeld || State.FinishedHeld, Is.False, "Reverse RMB also releases without automatic parking or pickup.");
            Assert.That(State.FinishedUnits, Is.EqualTo(5));
            Assert.That(State.StoredUnits, Is.Zero); Conserved();
        }

        [UnityTest]
        public IEnumerator GuidanceFollowsPartialWorkParkedFoodAndRecoveryWithoutMandatoryFullLoads()
        {
            Assert.That(session.hud.guidanceText.text, Does.Contain("orange crate"));
            Fill(2); yield return null; yield return null;
            Assert.That(session.hud.guidanceText.text, Does.Contain("hold left mouse"));
            Aim(new Vector3(2.65f, .04f, -1.4f), handling.station.intake.position);
            yield return null; yield return null;
            Assert.That(session.hud.guidanceText.text, Does.Contain("partial crate"));
            yield return Press(Key.E);
            yield return new WaitForSeconds(1.3f);
            MachineTestActions.StartPreparedBatch(State); State.AdvanceProcessing(4); handling.Render();
            yield return new WaitForSeconds(.8f);
            Assert.That(session.hud.guidanceText.text, Does.Contain("tray on the processor's right"));
            yield return Collect();
            Assert.That(session.hud.guidanceText.text, Does.Contain("handoff rack"));
            Aim(new Vector3(6, .04f, -4), new Vector3(6, 0, -2));
            yield return null; yield return null;
            Assert.That(session.hud.targetText.text, Is.Empty, "Placement validity stays quiet.");
            Assert.That(finished.carrier.portable.preview == null || !finished.carrier.portable.preview.enabled, Is.True);
            yield return Press(Key.G); yield return new WaitForSeconds(1);
            Assert.That(session.hud.guidanceText.text, Does.Contain("carrier you set down"));
            session.ResetToSpawn(); yield return null;
            Assert.That(State.FinishedUnits, Is.EqualTo(2));
            Assert.That(session.hud.guidanceText.text, Does.Contain("carrier you set down"));
            Conserved();
        }

        [UnityTest]
        public IEnumerator All107UnitsReachRackAndPartialFoodDisplaySurvivesRecoveryUntilExplicitRestart()
        {
            // Public gathering/time commands prepare batches; all receiving and rack actions use real input.
            int deposits = 0;
            while (State.Remaining > 0)
            {
                int amount = Mathf.Min(12, State.Remaining);
                PrepareOutput(amount); yield return Collect();
                AimRack(); yield return null; yield return null;
                Assert.That(session.targeting.Current, Is.SameAs(finished.rackTarget));
                int before = State.StoredUnits;
                yield return Press(Key.E); yield return Press(Key.E);
                Assert.That(State.StoredUnits, Is.EqualTo(before + amount));
                Assert.That(State.FinishedDocked, Is.True);
                Assert.That(finished.carrier.portable.Pose.Position, Is.EqualTo(finished.carrier.dock.position));
                Assert.That(finished.storedFood.jars.Count(j => j.activeSelf), Is.EqualTo((State.StoredUnits + 2) / 3));
                deposits++; Conserved();
            }
            Assert.That(State.StoredUnits, Is.EqualTo(107));
            Assert.That(session.hud.guidanceText.text, Does.Contain("All peppers stored"));
            Assert.That(deposits, Is.EqualTo(9));
            Assert.That(finished.DepositCues, Is.EqualTo(9));
            Assert.That(finished.storedFood.food[35].localScale.y, Is.EqualTo(.08f).Within(.001f));
            Assert.That(State.RawUnits + State.OutputUnits + State.QueuedUnits + State.ActiveUnits + State.FinishedUnits, Is.Zero);
            Assert.That(session.IsPaused, Is.False);
            session.ResetToSpawn(); Assert.That(State.StoredUnits, Is.EqualTo(107));
            Assert.That(session.hud.guidanceText.text, Does.Contain("All peppers stored"));
            Assert.That(AudioListener.volume, Is.Zero);
            session.Pause("Restart check"); session.hud.restartPrototypeButton.onClick.Invoke();
            Assert.That(session.IsPaused, Is.True);
            Assert.That(State.StoredUnits, Is.Zero);
            Assert.That(finished.storedFood.jars.All(j => !j.activeSelf), Is.True);
            Assert.That(State.Remaining, Is.EqualTo(107));
            Assert.That(session.hud.guidanceText.text, Does.Contain("orange crate"));
        }
    }
}
