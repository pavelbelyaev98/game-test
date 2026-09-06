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
    public class LoosePropSceneTests
    {
        const string ScenePath = "Assets/JustAFewPeppers/Scenes/PepperYard.unity";
        YardSession session;
        YardHandling handling;
        LoosePropHandling props;
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
            handling = session.handling; props = handling.looseProps;
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
                SceneManager.SetActiveScene(SceneManager.CreateScene("Loose prop cleanup"));
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
            var pose = new GameObject("Prop approach").transform; pose.position = position;
            var direction = target - (position + Vector3.up * 1.65f);
            pose.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
            session.player.ResetTo(pose); Object.Destroy(pose.gameObject);
            float pitch = Mathf.Atan2(-direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;
            session.player.Step(Vector2.zero, new Vector2(0, -pitch / session.player.lookSensitivity), false, false, 1f / 60);
            Physics.SyncTransforms(); session.targeting.Refresh();
        }

        IEnumerator Grab(LooseProp prop)
        {
            Aim(new Vector3(prop.transform.position.x, .04f, prop.transform.position.z - .9f), prop.portable.CollisionShape.bounds.center + (prop.propId == "loose-stool" ? Vector3.right * .3f : Vector3.zero));
            yield return null; yield return null;
            Assert.That(session.targeting.Current, Is.SameAs(prop.target), prop.name);
            yield return RightClick();
            Assert.That(props.Held, Is.SameAs(prop), prop.name);
            Assert.That(handling.State.IsHeld || handling.State.FinishedHeld, Is.False);
        }

        IEnumerator Place(Vector3 surface, Vector3 approach)
        {
            var prop = props.Held;
            Aim(approach, surface); yield return null; yield return null;
            Assert.That(prop.portable.PlacementValid, Is.True, prop.name + ": " + prop.portable.PlacementReason);
            var pose = prop.portable.Placement;
            yield return Press(Key.E);
            Assert.That(props.Held, Is.Null, "Top surface means place, not grab the support.");
            yield return new WaitForSeconds(.6f);
            Assert.That(Vector3.Distance(prop.portable.Pose.Position, pose.Position), Is.LessThan(.04f), prop.name);
            if (prop.portable.roundShape == null) Assert.That(prop.portable.Settled, Is.True, prop.name);
            Assert.That(prop.State.Pose.Position, Is.EqualTo(prop.portable.Pose.Position));
        }

        [UnityTest]
        public IEnumerator EverySampleRotatesPlacesOnWorktopAndRecoversWithoutChangingArrangements()
        {
            foreach (var prop in props.props)
            {
                yield return Grab(prop);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.X));
                yield return new WaitForSeconds(.25f);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
                Assert.That(prop.RotationOffset, Is.GreaterThan(15));
                yield return Place(new Vector3(-5.7f, .84f, -3.9f), new Vector3(-3.8f, .04f, -3.9f));
                var arranged = prop.State.Pose;
                yield return Press(Key.R);
                Assert.That(Vector3.Distance(prop.portable.Pose.Position, arranged.Position), Is.LessThan(.02f));
                Aim(new Vector3(-3.8f, .04f, -3.9f), prop.portable.CollisionShape.bounds.center);
                yield return null; yield return null; yield return RightClick();
                Assert.That(props.Held, Is.SameAs(prop));
                // Clear the shared test surface through ordinary careful placement.
                var home = prop.portable.Fallback.Position;
                yield return Place(new Vector3(home.x, 0, home.z), new Vector3(home.x, .04f, home.z - 1.1f));
            }
            Assert.That(handling.State.Remaining, Is.EqualTo(107));
            Assert.That(props.props.Select(p => p.State.Id).Distinct().Count(), Is.EqualTo(4));
            Assert.That(Object.FindObjectsByType<Rigidbody>().Length, Is.EqualTo(6));
        }

        [UnityTest]
        public IEnumerator BasinStacksOnStoolAndBallFitsInsideBasinWithRealCollisions()
        {
            var stool = props.props.Single(p => p.propId == "loose-stool");
            var basin = props.props.Single(p => p.propId == "loose-basin");
            var ball = props.props.Single(p => p.propId == "loose-ball");
            yield return Grab(stool);
            yield return Place(new Vector3(.4f, 0, -4), new Vector3(.4f, .04f, -5.8f));
            yield return Grab(basin);
            yield return Place(stool.transform.position + Vector3.up * .57f, new Vector3(.4f, .04f, -5.8f));
            yield return Grab(ball);
            yield return Place(basin.transform.position + Vector3.up * .05f, new Vector3(.4f, .04f, -4.8f));
            Assert.That(ball.portable.CollisionShape.bounds.min.y, Is.EqualTo(basin.transform.position.y + .062f).Within(.03f), "Ball rests on the interior base, not the rim.");
            Assert.That(basin.portable.Clear(basin.portable.Pose), Is.True, "Interior contents do not make a hollow basin invalid.");
            var poses = new[] { stool.State.Pose, basin.State.Pose, ball.State.Pose };
            yield return Press(Key.R);
            yield return new WaitForSeconds(.5f);
            Assert.That(Vector3.Distance(stool.transform.position, poses[0].Position), Is.LessThan(.03f));
            Assert.That(Vector3.Distance(basin.transform.position, poses[1].Position), Is.LessThan(.03f));
            Assert.That(Vector3.Distance(ball.transform.position, poses[2].Position), Is.LessThan(.03f));
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
        }

        [UnityTest]
        public IEnumerator TossDropPauseFocusAndLostRecoveryKeepBodiesAndFood()
        {
            var ball = props.props.Single(p => p.propId == "loose-ball");
            yield return Grab(ball);
            Aim(new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2)); yield return null; yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 });
            yield return new WaitForSeconds(.85f);
            Assert.That(props.Held, Is.SameAs(ball), "Charging does not release early.");
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null; yield return null;
            Assert.That(props.Held, Is.Null);
            Assert.That(ball.portable.body.linearVelocity.z, Is.GreaterThan(4));
            session.SendMessage("OnApplicationFocus", false);
            var frozen = ball.portable.Pose;
            var velocity = ball.portable.body.linearVelocity;
            yield return new WaitForSecondsRealtime(.2f);
            Assert.That(ball.portable.Pose.Position, Is.EqualTo(frozen.Position));
            Assert.That(ball.portable.body.linearVelocity, Is.EqualTo(velocity));
            session.SendMessage("OnApplicationFocus", true);
            yield return Press(Key.Escape);
            yield return new WaitForSeconds(.9f);
            Assert.That(ball.portable.ContactCues, Is.GreaterThan(0));
            Assert.That(ball.portable.InBounds, Is.True);
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null;
            yield return Press(Key.R);
            yield return Grab(ball);
            // Holding toss over pause/resume cannot replay a throw.
            yield return Press(Key.F1);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return null;
            yield return Press(Key.F1); yield return new WaitForSeconds(.1f);
            Assert.That(props.Held, Is.SameAs(ball));
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null; yield return null;
            Aim(new Vector3(1, .04f, -5), new Vector3(1, 1.65f, -2)); yield return null; yield return null;
            yield return Press(Key.G);
            Assert.That(props.Held, Is.Null);
            Assert.That(Mathf.Abs(ball.portable.body.linearVelocity.z), Is.LessThan(.1f), "G adds no toss velocity.");
            yield return Press(Key.Escape);
            frozen = ball.portable.Pose;
            yield return new WaitForSecondsRealtime(.2f);
            Assert.That(ball.portable.Pose.Position, Is.EqualTo(frozen.Position));
            session.Resume();
            ball.portable.SetPose(new CarrierPose(new Vector3(0, -8, 0), Quaternion.identity), false);
            yield return null; yield return null;
            Assert.That(ball.portable.InBounds, Is.True, "Automatic lost recovery uses the same body.");
            Assert.That(ball.State.Id, Is.EqualTo("loose-ball"));
            Assert.That(Object.FindObjectsByType<Rigidbody>().Length, Is.EqualTo(6));
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
        }

        [UnityTest]
        public IEnumerator CompoundHeldWallContactAndBlockedRecoveryPreserveOtherArrangements()
        {
            var stool = props.props.Single(p => p.propId == "loose-stool");
            var basin = props.props.Single(p => p.propId == "loose-basin");
            var arranged = basin.portable.Pose;
            yield return Grab(stool);
            Aim(new Vector3(0, .04f, -8.5f), new Vector3(0, 1.65f, -10));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.LeftShift));
            yield return new WaitForSeconds(.3f);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
            Assert.That(session.player.transform.position.z, Is.GreaterThan(-8.65f));
            Assert.That(session.player.transform.position.y, Is.LessThan(.1f), "Held compound body cannot launch the holder.");
            Assert.That(stool.portable.Clear(stool.portable.Pose, false), Is.True, "All compound parts stay clear of the wall.");
            yield return Place(new Vector3(.4f, 0, -4), new Vector3(.4f, .04f, -5.8f));
            var safe = stool.State.SafePose;
            session.Pause("Stuck object check");
            var obstruction = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obstruction.transform.position = safe.Position + Vector3.up * .3f;
            obstruction.transform.localScale = new Vector3(1.1f, .8f, 1.1f);
            Physics.SyncTransforms();
            Assert.That(stool.portable.Clear(stool.portable.Pose), Is.False);
            session.ResetToSpawn();
            Assert.That(stool.Stable, Is.True);
            Assert.That(Vector3.Distance(stool.transform.position, safe.Position), Is.GreaterThan(1));
            Assert.That(basin.portable.Pose.Position, Is.EqualTo(arranged.Position));
            Assert.That(obstruction.transform.position, Is.EqualTo(safe.Position + Vector3.up * .3f));
            Assert.That(stool.State.Id, Is.EqualTo("loose-stool"));
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
            Assert.That(Object.FindObjectsByType<Rigidbody>().Length, Is.EqualTo(6));
            Assert.That(session.IsPaused, Is.True);
        }

        [UnityTest]
        public IEnumerator ExplicitReleasesKeepLoadedCarriersAndPropPlayCannotCreditFood()
        {
            handling.State.PickUp(); handling.State.Gather("gate-mound-1", 5); handling.Render();
            Aim(new Vector3(.4f, .04f, -5.8f), new Vector3(.4f, 0, -4));
            yield return null; yield return null; yield return Press(Key.E);
            yield return new WaitForSeconds(.6f);
            var rawPose = handling.State.RawPose;
            yield return Grab(props.props.Single(p => p.propId == "loose-stool"));
            Assert.That(handling.State.RawPose.Position, Is.EqualTo(rawPose.Position));
            yield return Press(Key.R); // Recover the held stool; parked food stays put.
            Aim(new Vector3(.4f, .04f, -5.8f), rawPose.Position + Vector3.up * .3f);
            yield return null; yield return null; yield return RightClick();
            Assert.That(handling.State.IsHeld, Is.True);
            Aim(new Vector3(2.65f, .04f, -2.3f), handling.station.intake.position);
            yield return null; yield return null; yield return Press(Key.E);
            yield return new WaitForSeconds(.8f);
            yield return Press(Key.R); // Empty raw crate is released before grabbing a prop.
            yield return Grab(props.props.Single(p => p.propId == "loose-ball"));
            yield return new WaitForSeconds(4);
            Assert.That(handling.State.OutputUnits, Is.EqualTo(5));
            yield return Press(Key.R);
            Aim(new Vector3(4.45f, .04f, -1.3f), handling.finished.carrier.dock.position + Vector3.up * .2f);
            yield return null; yield return null; yield return Press(Key.E); yield return new WaitForSeconds(.5f);
            Assert.That(handling.State.FinishedHeld, Is.True);
            Aim(new Vector3(.4f, .04f, -5.8f), new Vector3(.4f, 1.65f, -2));
            yield return null; yield return null; yield return RightClick();
            yield return new WaitForSeconds(1);
            var finishedPose = handling.State.FinishedPose;
            yield return Grab(props.props.Single(p => p.propId == "loose-empty-crate"));
            Assert.That(handling.State.FinishedUnits, Is.EqualTo(5));
            yield return Press(Key.R);
            Assert.That(Vector3.Distance(handling.finished.carrier.transform.position, finishedPose.Position), Is.LessThan(.04f));
            Aim(finishedPose.Position + new Vector3(0, .04f, -1.8f), finishedPose.Position + Vector3.up * .25f);
            yield return null; yield return null; yield return RightClick();
            Assert.That(handling.State.FinishedHeld, Is.True);
            Aim(new Vector3(3, .04f, 3.2f), handling.finished.rackTarget.transform.position + Vector3.up);
            yield return null; yield return null; yield return Press(Key.E); yield return Press(Key.E);
            Assert.That(handling.State.StoredUnits, Is.EqualTo(5));
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
        }

        [UnityTest]
        public IEnumerator OrdinaryMouseReleaseFallsCarriesMotionAndRollsDownTheAuthoredBoard()
        {
            var ball = props.props.Single(p => p.propId == "loose-ball");
            yield return Grab(ball);
            Aim(new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2));
            yield return null; yield return null;
            Assert.That(ball.portable.PlacementValid, Is.False);
            float height = ball.transform.position.y;
            yield return RightClick();
            Assert.That(props.Held, Is.Null);
            Assert.That(ball.portable.body.isKinematic, Is.False);
            yield return new WaitForSeconds(.15f);
            Assert.That(ball.transform.position.y, Is.LessThan(height - .06f));
            yield return new WaitForSeconds(1);
            yield return Grab(ball);
            Aim(new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2));
            yield return null; yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
            yield return new WaitForSeconds(.2f);
            yield return RightClick();
            Assert.That(ball.portable.body.linearVelocity.z, Is.InRange(2f, 4.6f));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return Press(Key.R);
            yield return Grab(ball);
            var board = GameObject.Find("Sloped play board").transform;
            Aim(new Vector3(board.position.x, .04f, board.position.z - 1),
                new Vector3(board.position.x, 1.65f, board.position.z + 2));
            yield return null; yield return null;
            var start = ball.transform.position;
            yield return RightClick();
            yield return new WaitForSeconds(.9f);
            Assert.That(ball.transform.position.z, Is.GreaterThan(start.z + .12f), "Released ball rolls downhill through real contact.");
            Assert.That(ball.portable.ContactCues, Is.GreaterThan(0));
            Assert.That(ball.portable.InBounds, Is.True);
        }

        [UnityTest]
        public IEnumerator SleepingBallFallsWhenItsStoolIsGrabbedAndWakesOnAnotherPropImpact()
        {
            var stool = props.props.Single(p => p.propId == "loose-stool");
            var ball = props.props.Single(p => p.propId == "loose-ball");
            yield return Grab(stool);
            yield return Place(new Vector3(.4f, 0, -4), new Vector3(.4f, .04f, -5.8f));
            yield return Grab(ball);
            yield return Place(stool.transform.position + Vector3.up * .57f, new Vector3(.4f, .04f, -5.4f));
            yield return new WaitForSeconds(1);
            float height = ball.transform.position.y;
            Aim(new Vector3(.4f, .04f, -5.4f), stool.transform.position + new Vector3(.3f, .3f, 0));
            yield return null; yield return null; yield return RightClick();
            Assert.That(props.Held, Is.SameAs(stool));
            yield return new WaitForSeconds(.65f);
            Assert.That(ball.transform.position.y, Is.LessThan(height - .2f), "Removing support wakes a resting ball.");
            yield return Press(Key.R);
            // Recovery may rebuild the former stack. Put the ball on clear ground before the separate impact case.
            yield return Grab(ball);
            Aim(new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2));
            yield return null; yield return null; yield return RightClick();
            yield return new WaitForSeconds(2);
            Assert.That(ball.portable.Settled, Is.True);
            // A real released stool strikes a resting ball; no direct ball velocity/pose perturbation.
            var before = ball.transform.position;
            yield return Grab(stool);
            Aim(new Vector3(before.x + .3f, .04f, before.z - 1), new Vector3(before.x + .3f, 1.65f, before.z + 2));
            yield return null; yield return null; yield return RightClick();
            yield return new WaitForSeconds(1);
            Assert.That(Vector3.Distance(ball.transform.position, before), Is.GreaterThan(.08f));
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
        }

        [UnityTest]
        public IEnumerator GatheringChargeAndGrabInputsCannotTurnIntoAnAccidentalThrow()
        {
            handling.State.PickUp(); handling.Render();
            Aim(new Vector3(-3, .04f, -2.65f), handling.regions[1].volume.position);
            yield return null; yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 });
            yield return new WaitForSeconds(.2f);
            int food = handling.State.RawUnits;
            Assert.That(food, Is.GreaterThan(0));
            Aim(new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2));
            yield return new WaitForSeconds(.6f);
            Assert.That(handling.State.RawUnits, Is.EqualTo(food));
            // Keep LMB held while releasing the crate and pressing grab again.
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 3 }); yield return null; yield return null;
            Assert.That(handling.HasHeldObject, Is.False);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return null;
            var ball = props.props.Single(p => p.propId == "loose-ball");
            Aim(new Vector3(ball.transform.position.x, .04f, ball.transform.position.z - .9f), ball.portable.CollisionShape.bounds.center);
            yield return null; yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 3 }); yield return null; yield return null;
            Assert.That(props.Held, Is.Null, "Old gather press keeps handling disarmed.");
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null; yield return null;
            yield return RightClick();
            Assert.That(props.Held, Is.SameAs(ball));
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.4f);
            Assert.That(handling.ThrowCharging, Is.True);
            yield return Press(Key.F1);
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null;
            yield return Press(Key.F1); yield return new WaitForSeconds(.2f);
            Assert.That(props.Held, Is.SameAs(ball), "Pausing cancels charge instead of throwing on release/resume.");
            Assert.That(handling.ThrowCharging, Is.False);
            Aim(new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2));
            yield return null; yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.4f);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2 }); yield return null; yield return null;
            Assert.That(props.Held, Is.Null);
            Assert.That(Mathf.Abs(ball.portable.body.linearVelocity.z), Is.LessThan(.1f), "RMB release wins over charged LMB release.");
            InputSystem.QueueStateEvent(mouse, new MouseState());
            Assert.That(handling.State.AccountedUnits, Is.EqualTo(107));
        }
    }
}
