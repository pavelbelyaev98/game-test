using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace JustAFewPeppers
{
    public sealed partial class FoundationBuildSmoke
    {
        IEnumerator AimPepper(YardSession session, PepperBody pepper)
        {
            Aim(session, new Vector3(pepper.transform.position.x, .04f, pepper.transform.position.z - 1.05f), pepper.transform.position);
            yield return null; yield return null;
        }

        IEnumerator GrabRaw(YardSession session, Mouse mouse)
        {
            var crate = session.handling.crate;
            Aim(session, new Vector3(crate.transform.position.x, .04f, crate.transform.position.z - 1.8f), crate.transform.position + Vector3.up * .3f);
            yield return null; yield return null;
            Require(session.targeting.Current == crate.target, "Reachable raw crate side target");
            yield return MousePress(mouse);
            Require(session.handling.State.IsHeld, "RMB grabs the raw container");
        }

        void PrepareRaw(YardSession session, int amount)
        {
            var state = session.handling.State;
            foreach (var region in session.handling.regions) amount -= state.Gather(region.regionId, amount);
            Require(amount == 0, "Prepared the same finite raw amount");
            session.handling.Render(); session.handling.peppers.Render();
        }

        IEnumerator VerifyPepperBatch(YardSession session, Keyboard keyboard, Mouse mouse)
        {
            var batch = session.handling.peppers; var state = session.handling.State;
            var identities = batch.Bodies.Select(p => p.Record.Id).ToArray();
            foreach (var mode in new[] { PepperSimulation.PhysicalBatch, PepperSimulation.GroupedRest })
            {
                yield return KeyPress(keyboard, Key.F8); batch.simulation = mode; batch.Render();
                yield return new WaitForSeconds(.7f);
                var p = batch.Bodies[0]; yield return AimPepper(session, p); yield return new WaitForSeconds(.2f);
                Require(batch.Target == p, mode + " single target is visible"); Capture(session, mode + "-single.png");
                yield return MousePress(mouse); Require(batch.Held == p && state.UncontainedUnits == 1, mode + " takes exactly one pepper");
                Aim(session, new Vector3(-1.25f, .04f, -4), session.handling.crate.transform.position + Vector3.up * .1f);
                yield return null; yield return null; yield return KeyPress(keyboard, Key.E); yield return new WaitForSeconds(.8f);
                Require(state.RawUnits == 1 && batch.Held == null && p.Contacts > 0, mode + " pepper enters and contacts the open crate");
                Require(p.skin.bounds.max.y > session.handling.crate.transform.Find("Crate base").GetComponent<Renderer>().bounds.max.y + .04f,
                    mode + " settled pepper remains above the visible crate floor");
                Aim(session, new Vector3(-1.25f, .04f, -3.7f), p.transform.position); yield return null; yield return null;
                Require(batch.Target == p, mode + " exposed contents have priority over the carrier");
                Capture(session, mode + "-contents.png"); yield return MousePress(mouse);
                Require(batch.Held == p && !state.IsHeld && state.RawUnits == 0, "Contents pickup debits only the intended pepper");
                yield return KeyPress(keyboard, Key.R);
                yield return GrabRaw(session, mouse); PrepareRaw(session, 10);
                yield return AimPepper(session, batch.Bodies.First(v => v.Record.Owner == PepperOwner.Source));
                yield return new WaitForSeconds(.2f);
                var preview = batch.Preview.ToArray(); Require(preview.Length == 2, mode + " previews exactly the two remaining spaces");
                Capture(session, mode + "-bulk-preview.png");
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.25f);
                InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null; yield return new WaitForSeconds(.6f);
                Require(state.RawUnits == 12 && preview.All(id => state.Pepper(id).Owner == PepperOwner.Carrier), "Only the previewed set commits; release stops");
                Aim(session, new Vector3(2.65f, .04f, -2.3f), session.handling.station.intake.position); yield return null; yield return null;
                int before = batch.IntakeContacts;
                yield return KeyPress(keyboard, Key.E); yield return new WaitForSeconds(.18f);
                Capture(session, mode + "-physical-pour.png"); yield return new WaitForSeconds(1.2f);
                Require(batch.IntakeContacts - before == 12 && state.ActiveUnits == 12, mode + " all twelve physically contact the intake as one batch");
                yield return new WaitForSeconds(4.1f); Require(state.OutputUnits == 12, "Physical batch finishes normally");
                yield return KeyPress(keyboard, Key.R);
                Require(batch.Bodies.Select(v => v.Record.Id).SequenceEqual(identities) && state.AccountedUnits == 107, "Representation and recovery retain all identities and food");
            }
            yield return KeyPress(keyboard, Key.F8); batch.simulation = PepperSimulation.PhysicalBatch; batch.Render();
            yield return GrabRaw(session, mouse); PrepareRaw(session, 5);
            Aim(session, new Vector3(0, .04f, -5), new Vector3(0, 1.5f, -2)); yield return null; yield return null;
            yield return KeyPress(keyboard, Key.F); Require(state.UncontainedUnits == 5, "Off-target pour creates five registered loose units");
            var stray = batch.Bodies.First(p => p.Record.Owner == PepperOwner.Loose);
            yield return KeyPress(keyboard, Key.Escape); var frozen = stray.transform.position;
            yield return new WaitForSecondsRealtime(.2f); Require(stray.transform.position == frozen, "Spill motion freezes while paused");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.F)); yield return null;
            yield return KeyPress(keyboard, Key.Escape); yield return new WaitForSeconds(.3f);
            Require(!batch.Pouring, "Pause cancels the pour without resumed input replay");
            yield return new WaitForSeconds(.8f);
            Aim(session, new Vector3(0, .04f, -5), stray.transform.position); Capture(session, "peppers-spill.png");
            stray.Pose(new CarrierPose(new Vector3(0, -8, 0), Quaternion.identity), false); yield return null; yield return null;
            Require(stray.transform.position.y > -1, "Out-of-bounds pepper recovers automatically");
            yield return KeyPress(keyboard, Key.R);
            Require(state.Remaining == 107 && state.UncontainedUnits == 0 && batch.Bodies.Select(p => p.Record.Id).SequenceEqual(identities), "One recovery regroups the same strays");

            yield return ComparePepperSimulation(session, keyboard);

            // The entire food job uses physical input pours, receiving and handoff, including the final partial load.
            yield return KeyPress(keyboard, Key.F8); batch.simulation = PepperSimulation.PhysicalBatch; batch.Render();
            int handoffs = 0;
            while (state.Remaining > 0)
            {
                yield return GrabRaw(session, mouse); int amount = Mathf.Min(12, state.Remaining); PrepareRaw(session, amount);
                Aim(session, new Vector3(2.65f, .04f, -2.3f), session.handling.station.intake.position); yield return null; yield return null;
                int before = batch.IntakeContacts;
                yield return KeyPress(keyboard, Key.E); yield return new WaitForSeconds(5.4f);
                Require(batch.IntakeContacts - before == amount && state.OutputUnits == amount, "Physical full/partial load reaches output");
                Aim(session, new Vector3(5.8f, .04f, -5.1f), new Vector3(5.8f, 0, -3.3f)); yield return null; yield return null;
                yield return KeyPress(keyboard, Key.E); yield return new WaitForSeconds(.6f);
                Require(!state.IsHeld, "Release the raw crate explicitly before collecting");
                Aim(session, new Vector3(4.45f, .04f, -1.3f), session.handling.finished.carrier.dock.position + Vector3.up * .2f);
                yield return null; yield return null; yield return KeyPress(keyboard, Key.E); yield return new WaitForSeconds(.5f);
                Require(state.FinishedHeld && state.FinishedUnits == amount, "Collect the same full/partial output");
                Aim(session, new Vector3(3, .04f, 3.2f), session.handling.finished.rackTarget.transform.position + Vector3.up);
                yield return null; yield return null; yield return KeyPress(keyboard, Key.E); yield return KeyPress(keyboard, Key.E);
                handoffs++;
                Require(state.StoredUnits == Mathf.Min(handoffs * 12, 107) && state.AccountedUnits == 107, "Exact-once food handoff preserves the complete job");
            }
            Require(handoffs == 9 && state.StoredUnits == 107 && batch.Bodies.All(p => p.Record.Owner == PepperOwner.Processed), "All 107 units finish with no live duplicate pepper");
            Capture(session, "peppers-stored-107.png");
        }

        IEnumerator ComparePepperSimulation(YardSession session, Keyboard keyboard)
        {
            var report = new StringBuilder("mode,phase,trial,physics_steps,active_pepper_objects,awake_start,sleeping_end,physics_median_ms,physics_p95_ms,handling_median_ms,handling_p95_ms,managed_bytes_per_step\n");
            var batch = session.handling.peppers;
            for (int trial = 0; trial < 3; trial++)
            foreach (var mode in (trial % 2 == 0 ? new[] { PepperSimulation.PhysicalBatch, PepperSimulation.GroupedRest } : new[] { PepperSimulation.GroupedRest, PepperSimulation.PhysicalBatch }))
            foreach (string phase in new[] { "scattered", "filling", "pouring" })
            {
                yield return KeyPress(keyboard, Key.F8); batch.simulation = mode;
                Aim(session, new Vector3(-3.3f, .04f, -2.5f), new Vector3(-3.3f, .1f, 0));
                batch.Render(); yield return null; yield return null;
                if (phase != "scattered")
                {
                    session.handling.State.PickUp(); PrepareRaw(session, 12);
                    if (phase == "filling")
                    {
                        var crate = session.handling.crate;
                        session.handling.State.Release(crate.portable.Fallback, true);
                        crate.portable.SetPose(crate.portable.Fallback, true); batch.Render();
                    }
                    else
                    {
                        Aim(session, new Vector3(2.65f, .04f, -2.3f), session.handling.station.intake.position);
                        yield return null; yield return null; yield return KeyPress(keyboard, Key.E);
                    }
                }
                MeasurePepperSteps(session, mode, phase, trial, report);
                Require(session.handling.State.AccountedUnits == 107, "Matched comparison conserves all food");
            }
            File.WriteAllText(Path.Combine(output, "pepper-comparison.csv"), report.ToString());
            File.WriteAllText(Path.Combine(output, "pepper-comparison-context.txt"),
                "Three trials, alternating candidate order; identical 107-unit authored supply, 12-unit fill/pour, 200 fixed 0.02-second physics steps per phase. Setup uses public food commands and actual E pour input. Physics.Simulate and PepperBatch.Tick measured separately with Stopwatch; GC.GetAllocatedBytesForCurrentThread brackets only those loops. Other scene bodies participate in physics. Game time/other gameplay Update functions are not advanced by these synchronous fixed-step loops. No camera capture/file write inside a measurement.\n" +
                SystemInfo.processorType + "; " + SystemInfo.graphicsDeviceName + "; RAM " + SystemInfo.systemMemorySize + " MB; window " + Screen.width + "x" + Screen.height + "; vSync " + QualitySettings.vSyncCount + "; targetFrameRate " + Application.targetFrameRate + "; Development " + Debug.isDebugBuild +
                ". Batch mode can skip rendering: this is a controlled physics/handling CPU and managed-allocation comparison, not rendered FPS, full frame allocations or a minimum-hardware pass.\n");
        }

        void MeasurePepperSteps(YardSession session, PepperSimulation mode, string phase, int trial, StringBuilder report)
        {
            const int steps = 200;
            var batch = session.handling.peppers;
            var physics = new double[steps]; var handling = new double[steps];
            int active = batch.Bodies.Count(p => p.gameObject.activeSelf);
            int awake = batch.Bodies.Count(p => p.gameObject.activeSelf && !p.body.isKinematic && !p.body.IsSleeping());
            var prior = Physics.simulationMode;
            Physics.simulationMode = SimulationMode.Script;
            long allocated = GC.GetAllocatedBytesForCurrentThread();
            try
            {
                for (int i = 0; i < steps; i++)
                {
                    long start = Stopwatch.GetTimestamp(); Physics.Simulate(.02f);
                    long between = Stopwatch.GetTimestamp(); batch.Tick(.02f);
                    physics[i] = (between - start) * 1000.0 / Stopwatch.Frequency;
                    handling[i] = (Stopwatch.GetTimestamp() - between) * 1000.0 / Stopwatch.Frequency;
                }
            }
            finally { Physics.simulationMode = prior; }
            allocated = GC.GetAllocatedBytesForCurrentThread() - allocated;
            int sleeping = batch.Bodies.Count(p => p.gameObject.activeSelf && !p.body.isKinematic && p.body.IsSleeping());
            Array.Sort(physics); Array.Sort(handling);
            report.AppendLine(string.Join(",", mode, phase, trial, steps, active, awake, sleeping,
                physics[steps / 2].ToString("F4", CultureInfo.InvariantCulture), physics[(int)(steps * .95)].ToString("F4", CultureInfo.InvariantCulture),
                handling[steps / 2].ToString("F4", CultureInfo.InvariantCulture), handling[(int)(steps * .95)].ToString("F4", CultureInfo.InvariantCulture),
                ((double)allocated / steps).ToString("F1", CultureInfo.InvariantCulture)));
        }
    }
}
