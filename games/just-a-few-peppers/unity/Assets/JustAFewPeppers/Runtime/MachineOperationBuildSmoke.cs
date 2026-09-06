using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace JustAFewPeppers
{
    public sealed partial class FoundationBuildSmoke
    {
        float walkedMetres, walkedSeconds;

        IEnumerator MachineStroke(YardSession session, Keyboard keyboard, bool grouping, bool position = true)
        {
            var machine = session.handling.machine; var state = session.handling.State;
            var target = grouping ? session.handling.finished.outputTarget : machine.operationTarget;
            var point = target.transform.position + (grouping ? Vector3.up * .2f : Vector3.zero);
            Aim(session, position ? new Vector3(point.x, .04f, point.z - 2.3f) : session.player.transform.position, point);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
            Require(session.targeting.Current == target, "Broad mechanism target is reachable");
            int before = grouping ? state.GroupsCompleted : state.OperationsCompleted;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E)); yield return null; yield return null;
            Require(machine.Engaged, "Fresh keyboard gesture engages mechanism");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E, grouping ? Key.D : Key.W));
            float until = Time.time + 2;
            while ((grouping ? state.GroupsCompleted : state.OperationsCompleted) == before && Time.time < until) yield return null;
            Require((grouping ? state.GroupsCompleted : state.OperationsCompleted) == before + 1, "Direct stroke commits exactly once");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
        }

        IEnumerator VerifyMachineInterruptions(YardSession session, Keyboard keyboard, Mouse mouse)
        {
            var state = session.handling.State; var machine = session.handling.machine;
            foreach (bool grouping in new[] { false, true })
            {
                yield return KeyPress(keyboard, Key.F8);
                state.PickUp(); PrepareRaw(session, 5); state.Tip(); state.Release(state.RawPose, true);
                if (grouping) { state.BeginOperation(); state.MoveOperation(.94f); state.AdvanceProcessing(4); }
                session.handling.Render();
                var target = grouping ? session.handling.finished.outputTarget : machine.operationTarget;
                var point = target.transform.position + (grouping ? Vector3.up * .2f : Vector3.zero);
                Aim(session, new Vector3(point.x, .04f, point.z - 2.3f), point); yield return null; yield return null;
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.2f);
                Require((grouping ? state.GroupingStroke : state.OperationStroke) == 0, "Holding alone does not animate the mechanism");
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1, delta = grouping ? new Vector2(80, 0) : new Vector2(0, 80) });
                yield return null; yield return null;
                float stroke = grouping ? state.GroupingStroke : state.OperationStroke;
                Require(stroke > .3f && stroke < .4f, "Mouse displacement directly moves mechanism");
                Capture(session, grouping ? "machine-grouping-partial.png" : "machine-rack-partial.png");
                InputSystem.QueueStateEvent(mouse, new MouseState()); yield return new WaitForSeconds(.2f);
                Require((grouping ? state.GroupingStroke : state.OperationStroke) == stroke, "Release stops at the partial stroke");
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return null; yield return null;
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1, delta = grouping ? new Vector2(-40, 0) : new Vector2(0, -40) });
                yield return null; yield return null;
                Require((grouping ? state.GroupingStroke : state.OperationStroke) < stroke, "Reverse input backs up useful motion");
                session.SendMessage("OnApplicationFocus", false);
                Require(!state.HasStroke && state.AccountedUnits == 107, "Focus cancels before commit and retains food");
                session.SendMessage("OnApplicationFocus", true); session.Resume();
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1, delta = new Vector2(250, 250) });
                yield return new WaitForSeconds(.2f); Require(!state.HasStroke, "Held gesture cannot replay on resume");
                InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null; yield return null;
                yield return MachineStroke(session, keyboard, grouping);
                session.SendMessage("OnApplicationFocus", false); session.SendMessage("OnApplicationFocus", true); session.Resume();
                yield return KeyPress(keyboard, Key.R);
                Require((grouping ? state.FinishedUnits : state.ActiveUnits) == 5 && state.AccountedUnits == 107, "Recovery after commit keeps the same active or finished food");
            }
        }

        // Only orientation is arranged here; W moves the actual CharacterController along clear yard routes.
        IEnumerator WalkTo(YardSession session, Keyboard keyboard, Vector3 destination)
        {
            var start = session.player.transform.position; destination.y = start.y;
            var look = destination + Vector3.up * 1.65f;
            Aim(session, start, look); yield return null; yield return null;
            float until = Time.time + 10, began = Time.time;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
            Vector3 previous = session.player.transform.position;
            while (Vector2.Distance(new Vector2(previous.x, previous.z), new Vector2(destination.x, destination.z)) > .16f && Time.time < until)
            {
                yield return null;
                var now = session.player.transform.position;
                walkedMetres += Vector2.Distance(new Vector2(previous.x, previous.z), new Vector2(now.x, now.z)); previous = now;
            }
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
            walkedSeconds += Time.time - began;
            Require(Time.time < until, "Measured walking route reaches its waypoint");
        }

        IEnumerator MeasureMachineJob(YardSession session, Keyboard keyboard, Mouse mouse)
        {
            yield return KeyPress(keyboard, Key.F8);
            var state = session.handling.State; var machine = session.handling.machine;
            machine.ResetMeasurements();
            var csv = new StringBuilder("batch,units,operation_strokes,operation_held_s,grouping_strokes,grouping_held_s,pour_s,travel_m,travel_s,internal_processing_s,idle_wait_at_output_s,output_work_s,cycle_s\n");
            int count = 0;
            while (state.Remaining > 0)
            {
                walkedMetres = walkedSeconds = 0; float cycle = Time.time;
                // Return around the east side after each handoff; no position resets during measured cycles.
                if (count > 0)
                { yield return WalkTo(session, keyboard, new Vector3(5.6f, 0, 3.2f)); yield return WalkTo(session, keyboard, new Vector3(5.6f, 0, -2.7f)); }
                var raw = session.handling.crate.transform.position;
                yield return WalkTo(session, keyboard, new Vector3(raw.x, 0, raw.z - 1.8f));
                Aim(session, session.player.transform.position, raw + Vector3.up * .3f); yield return null; yield return null;
                yield return MousePress(mouse); Require(state.IsHeld, "Walking approach grabs the raw crate");
                int amount = Mathf.Min(12, state.Remaining); PrepareRaw(session, amount);
                yield return WalkTo(session, keyboard, new Vector3(2.65f, 0, -2.3f));
                Aim(session, session.player.transform.position, session.handling.station.intake.position); yield return null; yield return null;
                float poured = Time.time;
                yield return KeyPress(keyboard, Key.E);
                float pourDeadline = Time.time + 2;
                while ((session.handling.peppers.Pouring || state.QueuedUnits < amount) && Time.time < pourDeadline) yield return null;
                poured = Time.time - poured;
                Require(state.QueuedUnits == amount && state.ActiveUnits == 0, "Full or partial poured food waits for direct operation");
                yield return WalkTo(session, keyboard, new Vector3(.4f, 0, -2.7f));
                Aim(session, session.player.transform.position, new Vector3(.4f, 0, -1.2f)); yield return null; yield return null;
                yield return KeyPress(keyboard, Key.E); Require(!state.IsHeld, "Raw crate stays at the player's chosen staging point");
                yield return WalkTo(session, keyboard, new Vector3(1.65f, 0, -2.5f));
                float opHeld = machine.OperationHeldSeconds; int ops = state.OperationsCompleted;
                yield return MachineStroke(session, keyboard, false, false);
                float internalTime = (float)state.BatchRemaining;
                yield return WalkTo(session, keyboard, new Vector3(4.45f, 0, -2.5f));
                yield return WalkTo(session, keyboard, new Vector3(4.45f, 0, -1.3f));
                float waited = Time.time;
                while (state.ActiveUnits > 0) yield return null;
                waited = Time.time - waited;
                Require(state.OutputUnits == amount, "Unaccelerated processing prepares the same load");
                float outputWork = Time.time, groupHeld = machine.GroupingHeldSeconds; int groups = state.GroupsCompleted;
                yield return MachineStroke(session, keyboard, true, false); yield return new WaitForSeconds(.5f);
                outputWork = Time.time - outputWork;
                Require(state.FinishedHeld && state.FinishedUnits == amount, "Direct grouping collects the same selected load");
                yield return WalkTo(session, keyboard, new Vector3(5.6f, 0, -1.3f));
                yield return WalkTo(session, keyboard, new Vector3(5.6f, 0, 3.2f));
                yield return WalkTo(session, keyboard, new Vector3(3, 0, 3.2f));
                Aim(session, session.player.transform.position, session.handling.finished.rackTarget.transform.position + Vector3.up);
                yield return null; yield return null; yield return KeyPress(keyboard, Key.E); yield return KeyPress(keyboard, Key.E);
                count++;
                Require(state.StoredUnits == Mathf.Min(count * 12, 107) && state.AccountedUnits == 107, "Walked full/partial handoff credits once");
                csv.AppendLine(string.Join(",", count, amount, state.OperationsCompleted - ops, Number(machine.OperationHeldSeconds - opHeld),
                    state.GroupsCompleted - groups, Number(machine.GroupingHeldSeconds - groupHeld), Number(poured), Number(walkedMetres), Number(walkedSeconds),
                    Number(internalTime), Number(waited), Number(outputWork), Number(Time.time - cycle)));
                File.WriteAllText(Path.Combine(output, "machine-cycle-baseline.csv"), csv.ToString());
                if (count == 1 || count == 9) Capture(session, "machine-stored-" + state.StoredUnits + ".png");
            }
            Require(count == 9 && state.OperationsCompleted == 9 && state.GroupsCompleted == 9 && state.StoredUnits == 107 &&
                session.handling.peppers.Bodies.All(p => p.Record.Owner == PepperOwner.Processed), "Nine direct operation/grouping cycles finish all 107 peppers");
            File.WriteAllText(Path.Combine(output, "machine-cycle-context.txt"),
                "Ordinary hidden muted Windows player. Eight 12-unit loads and final 11, normal game time, no purchases. W walks the CharacterController between fixed clear waypoints; orientation is arranged without teleportation during each measured cycle. The first cycle starts at the normal spawn; later cycles include return from rack to the staged raw crate. Public Gather fills the held crate (gathering time excluded); actual E pours/places/hands off, E+W operates and E+D groups. Internal processing column is remaining timer just after commit (~4 s), overlapping travel; idle wait is only stationary waiting at output; output work includes grouping and 0.5 s receiving, excludes walking/handoff. Captures/file IO occur outside measured cycles. This deterministic route/keyboard baseline is not a human speed, control preference or FPS result.\n");
        }

        static string Number(float value) => value.ToString("F3", CultureInfo.InvariantCulture);
    }
}
