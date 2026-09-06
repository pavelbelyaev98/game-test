using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace JustAFewPeppers
{
    public sealed partial class FoundationBuildSmoke
    {
        string propObservation;

        IEnumerator GrabProp(YardSession session, Mouse mouse, LooseProp prop)
        {
            Aim(session, new Vector3(prop.transform.position.x, .04f, prop.transform.position.z - .9f), prop.portable.CollisionShape.bounds.center + (prop.propId == "loose-stool" ? Vector3.right * .3f : Vector3.zero));
            yield return null; yield return null;
            Require(session.targeting.Current == prop.target, "Reachable prop target: " + prop.propId);
            yield return MousePress(mouse);
            Require(session.handling.looseProps.Held == prop, "RMB picks up " + prop.propId);
        }

        IEnumerator PlaceProp(YardSession session, Keyboard keyboard, Vector3 surface, Vector3 approach)
        {
            var handling = session.handling.looseProps;
            var prop = handling.Held;
            Aim(session, approach, surface); yield return null; yield return null;
            Require(prop.portable.PlacementValid, prop.propId + " placement: " + prop.portable.PlacementReason);
            var pose = prop.portable.Placement;
            yield return KeyPress(keyboard, Key.E);
            yield return new WaitForSeconds(.65f);
            Require(handling.Held == null && Vector3.Distance(prop.transform.position, pose.Position) < .04f && prop.portable.Settled,
                prop.propId + " carefully places and settles without launch");
        }

        IEnumerator VerifyPhysicalRelease(YardSession session, Keyboard keyboard, Mouse mouse, LooseProp ball, LooseProp stool)
        {
            var props = session.handling.looseProps;
            yield return GrabProp(session, mouse, ball);
            Aim(session, new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2)); yield return null; yield return null;
            Require(!ball.portable.PlacementValid, "Ordinary release needs no supported placement target");
            float height = ball.transform.position.y;
            yield return MousePress(mouse);
            yield return new WaitForSeconds(.15f);
            Require(props.Held == null && ball.transform.position.y < height - .06f, "RMB releases into a real fall");
            Capture(session, "physics-01-release.png");
            yield return KeyPress(keyboard, Key.R);
            yield return GrabProp(session, mouse, ball);
            Aim(session, new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2)); yield return null; yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W)); yield return new WaitForSeconds(.2f);
            yield return MousePress(mouse);
            Require(ball.portable.body.linearVelocity.z > 2 && ball.portable.body.linearVelocity.magnitude <= 4.7f, "Moving release retains bounded carry motion");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return KeyPress(keyboard, Key.R);
            yield return GrabProp(session, mouse, ball);
            var board = GameObject.Find("Sloped play board").transform;
            Aim(session, new Vector3(board.position.x, .04f, board.position.z - 1), new Vector3(board.position.x, 1.65f, board.position.z + 2));
            yield return null; yield return null;
            var start = ball.transform.position;
            yield return MousePress(mouse); yield return new WaitForSeconds(.9f);
            Require(ball.transform.position.z > start.z + .12f, "Released ball rolls down the authored sloped board");
            Capture(session, "physics-02-board.png");
            yield return KeyPress(keyboard, Key.R);
            yield return GrabProp(session, mouse, stool);
            yield return PlaceProp(session, keyboard, new Vector3(.4f, 0, -4), new Vector3(.4f, .04f, -5.8f));
            yield return GrabProp(session, mouse, ball);
            yield return PlaceProp(session, keyboard, stool.transform.position + Vector3.up * .57f, new Vector3(.4f, .04f, -5.4f));
            yield return new WaitForSeconds(1);
            height = ball.transform.position.y;
            Aim(session, new Vector3(.4f, .04f, -5.4f), stool.transform.position + new Vector3(.3f, .3f, 0));
            yield return null; yield return null; yield return MousePress(mouse); yield return new WaitForSeconds(.65f);
            Require(props.Held == stool && ball.transform.position.y < height - .2f, "Grabbing support wakes the resting ball");
            yield return KeyPress(keyboard, Key.R);
            yield return GrabProp(session, mouse, ball);
            Aim(session, new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2));
            yield return null; yield return null; yield return MousePress(mouse); yield return new WaitForSeconds(2);
            Require(ball.portable.Settled, "Impact case starts with a ball resting on clear ground");
            var before = ball.transform.position;
            yield return GrabProp(session, mouse, stool);
            Aim(session, new Vector3(before.x + .3f, .04f, before.z - 1), new Vector3(before.x + .3f, 1.65f, before.z + 2));
            yield return null; yield return null; yield return MousePress(mouse); yield return new WaitForSeconds(1);
            Require(Vector3.Distance(ball.transform.position, before) > .08f, "A released prop moves the resting ball on contact");
            yield return KeyPress(keyboard, Key.R);
            // Begin the independent charge/pause case with clear fixtures, not the previous impact's clutter.
            foreach (var prop in props.props) { prop.State.Release(prop.portable.Fallback, true); prop.portable.SetPose(prop.portable.Fallback, true); }
            yield return GrabProp(session, mouse, ball);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.4f);
            Require(session.handling.ThrowCharging, "LMB begins deliberate charge");
            yield return KeyPress(keyboard, Key.F1);
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null;
            yield return KeyPress(keyboard, Key.F1); yield return new WaitForSeconds(.2f);
            Require(props.Held == ball && !session.handling.ThrowCharging, "Help cancels charge without throwing on return");
            // Holding RMB across resume must not release; a fresh press does.
            yield return KeyPress(keyboard, Key.Escape);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2 }); yield return null;
            yield return KeyPress(keyboard, Key.Escape); yield return new WaitForSeconds(.1f);
            Require(props.Held == ball, "Held grab input cannot release on resume");
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null; yield return null;
            yield return KeyPress(keyboard, Key.R);
            foreach (var prop in props.props) { prop.State.Release(prop.portable.Fallback, true); prop.portable.SetPose(prop.portable.Fallback, true); }
        }

        IEnumerator VerifyLooseProps(YardSession session, Keyboard keyboard, Mouse mouse)
        {
            var handling = session.handling;
            var props = handling.looseProps;
            var bodies = UnityEngine.Object.FindObjectsByType<Rigidbody>().Where(b => b.GetComponent<PepperBody>() == null).ToArray();
            Require(bodies.Length == 6 && props.props.Select(p => p.State.Id).Distinct().Count() == 4, "Six reusable portable bodies, four distinct prop IDs");
            Aim(session, new Vector3(-4.1f, .04f, -3), new Vector3(-6.4f, .4f, -.8f));
            Capture(session, "props-01-sample.png");
            foreach (var prop in props.props)
            {
                yield return GrabProp(session, mouse, prop);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.X)); yield return new WaitForSeconds(.25f);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
                Require(prop.RotationOffset > 15, "Optional rotation: " + prop.propId);
                yield return PlaceProp(session, keyboard, new Vector3(-5.7f, .84f, -3.9f), new Vector3(-3.8f, .04f, -3.9f));
                Capture(session, "props-worktop-" + prop.propId + ".png");
                // Aim at the visible body, not the hollow crate's thin base beside the worktop.
                var grabPoint = prop.portable.handlingSize != Vector3.zero
                    ? prop.transform.TransformPoint(prop.portable.handlingCenter) : prop.portable.CollisionShape.bounds.center;
                Aim(session, new Vector3(-3.8f, .04f, -3.9f), grabPoint);
                yield return null; yield return null;
                Require(session.targeting.Current == prop.target, "Worktop regrab target: " + prop.propId);
                yield return MousePress(mouse);
                Require(props.Held == prop, "Regrab " + prop.propId);
                Aim(session, new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2)); yield return null; yield return null;
                yield return ThrowProp(mouse);
                Require(props.Held == null && prop.portable.body.linearVelocity.z > 2.5f, "Deliberate physical toss: " + prop.propId);
                InputSystem.QueueStateEvent(mouse, new MouseState());
                yield return new WaitForSeconds(1.4f);
                Require(prop.portable.InBounds && prop.portable.ContactCues > 0, "Toss collides within the yard: " + prop.propId);
                // Restore the initial sample between deterministic cases, without reconstructing any body.
                prop.State.Release(prop.portable.Fallback, true); prop.portable.SetPose(prop.portable.Fallback, true);
                prop.RotationOffset = 0;
            }
            var stool = props.props.Single(p => p.propId == "loose-stool");
            var basin = props.props.Single(p => p.propId == "loose-basin");
            var ball = props.props.Single(p => p.propId == "loose-ball");
            yield return GrabProp(session, mouse, stool);
            yield return PlaceProp(session, keyboard, new Vector3(.4f, 0, -4), new Vector3(.4f, .04f, -5.8f));
            yield return GrabProp(session, mouse, basin);
            yield return PlaceProp(session, keyboard, stool.transform.position + Vector3.up * .57f, new Vector3(.4f, .04f, -5.8f));
            yield return GrabProp(session, mouse, ball);
            yield return PlaceProp(session, keyboard, basin.transform.position + Vector3.up * .05f, new Vector3(.4f, .04f, -4.8f));
            Require(Mathf.Abs(ball.portable.CollisionShape.bounds.min.y - basin.transform.position.y - .062f) < .03f, "Ball rests inside the hollow basin");
            var arranged = props.props.Select(p => p.portable.Pose).ToArray();
            yield return KeyPress(keyboard, Key.R);
            for (int i = 0; i < arranged.Length; i++) Require(Vector3.Distance(props.props[i].transform.position, arranged[i].Position) < .04f, "Recovery preserves arrangement " + i);
            Aim(session, new Vector3(.4f, .04f, -5.8f), basin.transform.position + Vector3.up * .15f);
            Capture(session, "props-02-stack.png");
            // Clear the route through the same grab/drop controls.
            Aim(session, new Vector3(.4f, .04f, -4.8f), ball.portable.CollisionShape.bounds.center);
            yield return null; yield return null; yield return MousePress(mouse);
            Require(props.Held == ball, "Take the intended ball from the stack");
            Aim(session, new Vector3(1.8f, .04f, -5), new Vector3(1.8f, 1.65f, -2)); yield return null; yield return null;
            yield return MousePress(mouse);
            Require(props.Held == null && Mathf.Abs(ball.portable.body.linearVelocity.z) < .1f, "Ordinary RMB release drops without throw velocity");
            session.SendMessage("OnApplicationFocus", false);
            var frozen = ball.portable.Pose; var velocity = ball.portable.body.linearVelocity;
            yield return new WaitForSecondsRealtime(.2f);
            Require(ball.portable.Pose.Position == frozen.Position && ball.portable.body.linearVelocity == velocity, "Physical fall freezes on lost focus");
            session.SendMessage("OnApplicationFocus", true);
            yield return KeyPress(keyboard, Key.Escape);
            yield return new WaitForSeconds(1.2f);
            ball.portable.SetPose(new CarrierPose(new Vector3(0, -8, 0), Quaternion.identity), false);
            yield return null; yield return null;
            Require(ball.portable.InBounds && ball.State.Id == "loose-ball", "Automatic recovery retains the same lost ball");
            // Remove the deliberately arranged obstruction before the retained carrier placement checks.
            foreach (var prop in props.props) { prop.State.Release(prop.portable.Fallback, true); prop.portable.SetPose(prop.portable.Fallback, true); }

            yield return VerifyPhysicalRelease(session, keyboard, mouse, ball, stool);
            handling.State.PickUp(); handling.State.Gather("gate-mound-1", 5); handling.Render();
            Aim(session, new Vector3(.4f, .04f, -5.8f), new Vector3(.4f, 0, -4));
            yield return null; yield return null; yield return KeyPress(keyboard, Key.E); yield return new WaitForSeconds(.6f);
            var rawPose = handling.State.RawPose;
            yield return GrabProp(session, mouse, stool);
            Require(handling.State.RawUnits == 5 && !handling.State.IsHeld && handling.State.RawPose.Position == rawPose.Position, "Prop pickup preserves the explicitly parked raw load");
            yield return KeyPress(keyboard, Key.R);
            Aim(session, new Vector3(.4f, .04f, -5.8f), rawPose.Position + Vector3.up * .3f);
            yield return null; yield return null; yield return MousePress(mouse);
            Require(handling.State.IsHeld && props.Held == null, "Regrab the parked raw load");
            Aim(session, new Vector3(2.65f, .04f, -2.3f), handling.station.intake.position);
            yield return null; yield return null; yield return KeyPress(keyboard, Key.E); yield return new WaitForSeconds(1.3f);
            yield return KeyPress(keyboard, Key.R);
            yield return GrabProp(session, mouse, ball);
            Aim(session, new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2)); yield return null; yield return null;
            yield return ThrowProp(mouse);
            // Observe real player frames during a bounded physics sample; exclude capture/file-writing frames.
            var frames = new List<float>(); int maxAwake = 0;
            float until = Time.realtimeSinceStartup + 5;
            while (Time.realtimeSinceStartup < until)
            {
                yield return null;
                frames.Add(Time.unscaledDeltaTime * 1000);
                maxAwake = Mathf.Max(maxAwake, bodies.Count(b => !b.isKinematic && !b.IsSleeping()));
            }
            frames.Sort();
            propObservation = "Physical sample: " + bodies.Length + " total reusable bodies; maximum " + maxAwake + " awake; " +
                bodies.Count(b => !b.isKinematic && b.IsSleeping()) + " sleeping released bodies at sample end. Five-second hidden batch-player ball/processing update observation: " + frames.Count +
                " updates; median " + frames[frames.Count / 2].ToString("F2") + " ms; p95 " + frames[(int)((frames.Count - 1) * .95f)].ToString("F2") +
                " ms; max " + frames.Last().ToString("F2") + " ms. " + SystemInfo.processorType + "; " + SystemInfo.graphicsDeviceName + "; RAM " + SystemInfo.systemMemorySize + " MB" +
                "; " + Screen.width + "x" + Screen.height + "; vSync " + QualitySettings.vSyncCount + "; targetFrameRate " + Application.targetFrameRate +
                ". Batch mode can skip rendering; these are simulation/update intervals, not rendered FPS or a minimum-hardware performance pass.\n";
            Require(handling.State.OutputUnits == 5, "Processing continues through optional prop play");
            Aim(session, new Vector3(4.45f, .04f, -1.3f), handling.finished.carrier.dock.position + Vector3.up * .2f);
            yield return null; yield return null; yield return KeyPress(keyboard, Key.E); yield return new WaitForSeconds(.5f);
            Aim(session, new Vector3(.4f, .04f, -5.8f), new Vector3(.4f, 1.65f, -2));
            yield return null; yield return null; yield return MousePress(mouse);
            yield return new WaitForSeconds(1);
            var finishedPose = handling.State.FinishedPose;
            yield return GrabProp(session, mouse, props.props.Single(p => p.propId == "loose-empty-crate"));
            Require(handling.State.FinishedUnits == 5 && !handling.State.FinishedHeld, "Explicit finished release preserves food during prop handling");
            yield return KeyPress(keyboard, Key.R);
            Aim(session, finishedPose.Position + new Vector3(0, .04f, -1.8f), finishedPose.Position + Vector3.up * .25f);
            yield return null; yield return null; yield return MousePress(mouse);
            Require(handling.State.FinishedHeld, "Recovered/regrabbed finished load is usable");
            Aim(session, new Vector3(3, .04f, 3.2f), handling.finished.rackTarget.transform.position + Vector3.up);
            yield return null; yield return null; yield return KeyPress(keyboard, Key.E); yield return KeyPress(keyboard, Key.E);
            Require(handling.State.StoredUnits == 5 && handling.State.AccountedUnits == 107, "Prop play has no food credit; final handoff commits once");
            File.WriteAllText(Path.Combine(output, "loose-props-observation.txt"), propObservation);
            foreach (var prop in props.props) { prop.State.Release(prop.portable.Fallback, true); prop.portable.SetPose(prop.portable.Fallback, true); }
            session.RestartPrototype();
            yield return null; yield return null; // Recovery deliberately requires a fresh input update.
        }
    }
}
