using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace JustAFewPeppers
{
    // Local verification of the exact playtest or diagnostic player; requires batch mode and an explicit flag.
    public sealed partial class FoundationBuildSmoke : MonoBehaviour
    {
        string output;
        float deadline;
        bool finished;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void StartIfRequested()
        {
            var args = Environment.GetCommandLineArgs();
            int flag = Array.IndexOf(args, "-foundationSmoke");
            if (!Application.isBatchMode || flag < 0 || flag + 1 >= args.Length) return;
            // Mute only the explicitly requested probe, before scene audio can start.
            AudioListener.volume = 0;
            var probe = new GameObject("Foundation build verification").AddComponent<FoundationBuildSmoke>();
            probe.output = Path.GetFullPath(args[flag + 1]);
            Directory.CreateDirectory(probe.output);
            probe.deadline = Time.realtimeSinceStartup + 210;
            Application.logMessageReceived += probe.OnLog;
        }

        IEnumerator Start()
        {
            bool expectedDevelopment = Array.IndexOf(Environment.GetCommandLineArgs(), "-expectDevelopment") >= 0;
            Require(Debug.isDebugBuild == expectedDevelopment, "Expected player kind: " + (expectedDevelopment ? "Development" : "Playtest"));
            // A hidden automated player has no physical focus. Only this opt-in probe changes these settings.
            Application.runInBackground = true;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            yield return null;
            yield return null;
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            Require(session != null && session.IsPaused, "Saved yard starts with a usable menu");
            session.SendMessage("OnApplicationFocus", true);
            var keyboard = InputSystem.AddDevice<Keyboard>();
            var mouse = InputSystem.AddDevice<Mouse>();
            session.Input.Actions.devices = new InputDevice[] { keyboard, mouse };
            Capture(session, "01-menu.png");
            yield return VerifyComfort(session, keyboard, mouse);
            yield return KeyPress(keyboard, Key.Enter);
            Require(!session.IsPaused, "Keyboard UI Submit resumes the packaged player");
            var start = session.player.transform.position;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
            yield return new WaitForSecondsRealtime(.35f);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            Require(session.player.transform.position.z > start.z + .5f, "Packaged input moves the character");
            Capture(session, "02-yard.png");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.LeftShift));
            yield return null;
            yield return null;
            var sprintStart = session.player.transform.position;
            float sprintTime = Time.time;
            yield return new WaitForSecondsRealtime(.25f);
            float speed = (session.player.transform.position.z - sprintStart.z) / (Time.time - sprintTime);
            Require(Mathf.Abs(speed - session.player.sprintSpeed) < .25f, "Packaged Shift input sprints at the configured speed");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));
            yield return new WaitForSecondsRealtime(.12f);
            Require(session.player.transform.position.y > .35f, "Packaged Space input jumps");
            Capture(session, "03-jump.png");
            yield return new WaitForSecondsRealtime(.85f);
            Require(session.player.body.isGrounded && session.player.transform.position.y < .1f, "Jump lands without repeating while Space is held");
            yield return KeyPress(keyboard, Key.Space); // Release held Space.
            yield return KeyPress(keyboard, Key.Space);
            yield return new WaitForSecondsRealtime(.08f);
            Require(session.player.transform.position.y > .25f, "Fresh press starts another jump");
            yield return KeyPress(keyboard, Key.Escape);
            Require(session.IsPaused && Time.timeScale == 0, "Escape pauses the packaged player");
            var pausedPosition = session.player.transform.position;
            // Space is a legitimate menu Submit here; don't deliberately activate Quit during this probe.
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.LeftShift));
            yield return new WaitForSecondsRealtime(.1f);
            Require(session.player.transform.position == pausedPosition, "Paused sprint/jump movement stays frozen in midair");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            session.SendMessage("OnApplicationFocus", false);
            session.SendMessage("OnApplicationFocus", true);
            Require(session.IsPaused, "Focus return waits for explicit resume");
            yield return KeyPress(keyboard, Key.Escape);
            yield return KeyPress(keyboard, Key.R);
            Require(Vector3.Distance(session.player.transform.position, session.safeSpawn.position) < .1f, "Reset returns to safe spawn");
            yield return VerifyLooseProps(session, keyboard, mouse);
            yield return VerifyHandling(session, keyboard, mouse);
            yield return VerifyProcessing(session, keyboard, mouse);
            yield return VerifyFinishedFood(session, keyboard, mouse);
            Require(AudioListener.volume == 0, "Automated player audio stays muted");
            InputSystem.RemoveDevice(keyboard);
            InputSystem.RemoveDevice(mouse);
            finished = true;
            File.WriteAllText(Path.Combine(output, "result.txt"), "PASS: " + (Debug.isDebugBuild ? "Development" : "Playtest") + " player; packaged scene/menu, walking, sprint speed, jump/landing without held repeat, midair pause freeze, simulated focus callbacks, resume and safe-spawn reset; crate pickup, immediate/local scooping, partial depletion, 12-unit fill, quiet held-full feedback, loaded sprint/jump, rotated ground/worktop/support placement and regrab, gravity/contact/settling, drop focus freeze, safe-pose recovery, prototype reset and conservation; E tipping, visible cascade/tilt, partial jar, output reservation/accumulation, limited acceptance, quiet full input, processing pause/focus and all-food recovery/reset.\nImages: 01-menu.png through 11-full-input.png plus placement-0 through placement-4 captures.\nPhysical focus switching, sound and handling comfort require the tester's playtest.\n");
            Debug.Log("FOUNDATION_BUILD_SMOKE_PASS");
            File.AppendAllText(Path.Combine(output, "result.txt"), "Finished-food checks: nine input-driven tip/receive/handoff cycles store all 107 units, including the final eleven; prepared loads use public gathering commands. Quiet rotated ground/worktop placement, regrab, loaded sprint/jump, drop/contact/focus freeze/recovery, raw arrangement preservation, exactly-once deposit, empty auto-return and partial stored-food fill pass. Captures: 12-receiving-food.png, 13-finished-load.png, finished-placement-0/1.png, stored-food-12/24/107.png.\n");
            File.AppendAllText(Path.Combine(output, "result.txt"), "Comfort: keyboard and pointer sensitivity changes while paused, actual mouse-look response, reset preserving the setting, quiet gameplay and pause menu; full controls/debug reference only on F1, with safe return to the previous mode. Captures: 01-sensitivity.png, 01-f1-help.png.\n");
            File.AppendAllText(Path.Combine(output, "result.txt"), "Loose props: RMB grab/release and LMB charged throwing, optional E placement/rotation for all four samples; ball fall/moving release/sloped-board roll, support removal and real prop impact, cancelled charge and held grab across pause, hollow-basin stack, focus freeze, lost recovery, explicitly staged raw/finished carriers and exact-once five-unit handoff passed.\n" + propObservation);
            Application.Quit(0);
        }

        IEnumerator MousePress(Mouse mouse, ushort buttons = 2)
        {
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = buttons });
            yield return null; yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState());
            yield return null; yield return null;
        }

        IEnumerator ThrowProp(Mouse mouse)
        {
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 });
            yield return new WaitForSeconds(.85f);
            InputSystem.QueueStateEvent(mouse, new MouseState());
            yield return null; yield return null;
        }

        IEnumerator VerifyComfort(YardSession session, Keyboard keyboard, Mouse mouse)
        {
            var hud = session.hud;
            Require(!hud.controlsText.gameObject.activeInHierarchy && !hud.guidanceText.gameObject.activeInHierarchy && !hud.guidanceBackdrop.activeSelf, "Ordinary pause keeps instructions hidden");
            yield return KeyPress(keyboard, Key.F1);
            Require(hud.controlsText.gameObject.activeInHierarchy && session.IsPaused, "F1 reveals the complete optional controls reference");
            Capture(session, "01-f1-help.png");
            yield return KeyPress(keyboard, Key.F1);
            Require(session.IsPaused && hud.pausePanel.activeSelf, "Closing help returns to the previous paused menu");
            yield return KeyPress(keyboard, Key.DownArrow);
            yield return KeyPress(keyboard, Key.DownArrow);
            yield return KeyPress(keyboard, Key.DownArrow);
            Require(hud.events.currentSelectedGameObject == hud.sensitivitySlider.gameObject, "Keyboard navigation reaches mouse sensitivity");
            yield return KeyPress(keyboard, Key.RightArrow);
            Require(session.player.lookSensitivity > .1f && session.IsPaused, "Keyboard changes sensitivity while gameplay stays paused");
            Canvas.ForceUpdateCanvases();
            var rect = hud.sensitivitySlider.GetComponent<RectTransform>();
            Vector2 point = RectTransformUtility.WorldToScreenPoint(null, rect.TransformPoint(new Vector3(rect.rect.width * .3f, 0, 0)));
            InputSystem.QueueStateEvent(mouse, new MouseState { position = point }); yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState { position = point, buttons = 1 }); yield return null; yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState { position = point }); yield return null; yield return null;
            float sensitivity = session.player.lookSensitivity;
            Require(sensitivity > .19f && sensitivity < .23f, "Pointer changes the wired sensitivity slider");
            Capture(session, "01-sensitivity.png");
            yield return KeyPress(keyboard, Key.Escape);
            Require(!hud.guidanceText.gameObject.activeInHierarchy && !hud.guidanceBackdrop.activeSelf && !hud.controlsText.gameObject.activeInHierarchy, "Gameplay has no continuous controls or instruction footer");
            yield return KeyPress(keyboard, Key.F1);
            Require(session.IsPaused && hud.helpPanel.activeSelf, "F1 from play freezes simulation");
            yield return KeyPress(keyboard, Key.F1);
            Require(!session.IsPaused && !hud.helpPanel.activeSelf, "Closing optional help restores play");
            InputSystem.QueueStateEvent(mouse, new MouseState { delta = new Vector2(100, 0) });
            yield return null; yield return null;
            Require(Mathf.Abs(Mathf.DeltaAngle(0, session.player.transform.eulerAngles.y) - sensitivity * 100) < .2f, "Actual look input uses the adjusted sensitivity");
            yield return KeyPress(keyboard, Key.R);
            Require(session.player.lookSensitivity == sensitivity && session.handling.State.Remaining == 107, "Recovery preserves sensitivity and food");
            session.Pause("Comfort check complete");
            hud.sensitivitySlider.value = 1;
        }

        IEnumerator VerifyHandling(YardSession session, Keyboard keyboard, Mouse mouse)
        {
            var handling = session.handling;
            Require(handling != null && handling.State.InitialHarvest == 107, "Saved handling model has the authored finite supply");
            Aim(session, new Vector3(-1.25f, .04f, -4.4f), handling.crate.transform.position + Vector3.up * .3f);
            yield return MousePress(mouse);
            Require(handling.State.IsHeld, "Packaged RMB input picks up the unique crate");
            var region = handling.regions[1];
            Aim(session, new Vector3(-3, .04f, -2.65f), region.volume.position);
            yield return null;
            float gatherStart = Time.time;
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 });
            yield return null; yield return null;
            Require(handling.State.RawUnits == 1, "First scoop responds immediately");
            Require(region.volume.localScale.y < region.fullScale.y, "Touched region changes on accepted transfer");
            Capture(session, "04-scoop.png");
            float fillDeadline = Time.time + 8;
            while (handling.State.RawUnits < 12 && Time.time < fillDeadline) yield return null;
            Require(handling.State.RawUnits == 12 && handling.State.Remaining == 95, "A complete crate conserves 107 units");
            Debug.Log("HANDLING_GATHER_SECONDS " + (Time.time - gatherStart).ToString("F3") + " for 12 units; first immediate, interval " + handling.scoopInterval);
            Require(handling.presentation.ScoopCues == 12, "Each committed scoop dispatches its action audio once");
            Require(AudioListener.volume == 0 && !handling.presentation.actionAudio.ignoreListenerVolume &&
                !handling.presentation.feedbackAudio.ignoreListenerVolume, "Scoop and feedback audio respect the probe mute");
            int cueCount = handling.presentation.FeedbackCues;
            yield return new WaitForSecondsRealtime(.8f);
            Require(handling.presentation.FeedbackCues == cueCount, "Held-full feedback does not repeat");
            Capture(session, "05-loaded.png");
            InputSystem.QueueStateEvent(mouse, new MouseState());
            Aim(session, new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2));
            yield return new WaitForSecondsRealtime(.1f);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.LeftShift, Key.Space));
            yield return new WaitForSecondsRealtime(.15f);
            Require(session.player.transform.position.y > .4f && handling.State.RawUnits == 12 && handling.State.IsHeld, "Loaded sprint/jump keeps the same crate and units");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return new WaitForSecondsRealtime(.65f);
            yield return VerifyFreePlacement(session, keyboard, mouse);
            yield return KeyPress(keyboard, Key.R);
            Require(handling.State.RawUnits == 12 && handling.State.Remaining == 95, "Recovery preserves depletion and load");
            yield return KeyPress(keyboard, Key.F8);
            Require(handling.State.RawUnits == 0 && handling.State.Remaining == 107, "Explicit prototype restart restores authored state");
            Require(session.Input.Actions.FindAction("Gameplay/ScoopMode") == null, "Packaged controls have no scoop mode action");
            Aim(session, new Vector3(-1.25f, .04f, -4.4f), handling.crate.transform.position + Vector3.up * .3f);
            yield return MousePress(mouse);
            Aim(session, new Vector3(-3, .04f, -2.65f), region.volume.position);
            yield return KeyPress(keyboard, Key.T);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 });
            yield return null; yield return null;
            Require(handling.State.RawUnits == 1, "Hold-only scoop starts on left mouse");
            InputSystem.QueueStateEvent(mouse, new MouseState());
            yield return new WaitForSecondsRealtime(.7f);
            Require(handling.State.RawUnits == 1, "Release stops scooping; T cannot enable automatic gathering");
        }

        IEnumerator VerifyFreePlacement(YardSession session, Keyboard keyboard, Mouse mouse)
        {
            var handling = session.handling;
            var portable = handling.crate.portable;
            var points = new[] { new Vector3(.3f, 0, -3.8f), new Vector3(1.3f, 0, -4.1f),
                new Vector3(-5.7f, .84f, -3.9f), new Vector3(-5.8f, .5f, -5.6f) };
            var approaches = new[] { new Vector3(.3f, .04f, -5.6f), new Vector3(1.3f, .04f, -5.9f),
                new Vector3(-3.8f, .04f, -3.9f), new Vector3(-3.8f, .04f, -5.6f) };
            for (int i = 0; i < points.Length; i++)
            {
                Aim(session, approaches[i], points[i]);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.X));
                yield return new WaitForSecondsRealtime(.22f);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState());
                yield return null; yield return null;
                Require(portable.PlacementValid, "Chosen supported position " + i + ": " + portable.PlacementReason);
                Require(!portable.preview.enabled && session.hud.targetText.text.Length == 0,
                    "Supported placement aim has no outline or continuous validity guidance");
                var chosen = portable.Placement;
                Capture(session, "placement-" + i + "-aim.png");
                yield return KeyPress(keyboard, Key.E);
                Require(!handling.State.IsHeld && handling.State.RawUnits == 12, "E places the full crate at chosen position " + i);
                yield return new WaitForSecondsRealtime(.5f);
                Require(Vector3.Distance(portable.Pose.Position, chosen.Position) < .035f &&
                    Quaternion.Angle(portable.Pose.Rotation, chosen.Rotation) < 2, "Careful placement settles predictably " + i);
                Capture(session, "placement-" + i + "-placed.png");
                if (i == 0) Capture(session, "06-parked.png");
                var position = portable.Pose.Position;
                session.ResetToSpawn();
                Require(Vector3.Distance(portable.Pose.Position, position) < .01f, "Return to gate preserves valid arrangement");
                Aim(session, approaches[i], portable.Pose.Position + Vector3.up * .3f);
                yield return null; yield return null;
                yield return MousePress(mouse);
                Require(handling.State.IsHeld, "Placed crate can be regrabbed " + i);
            }
            Aim(session, new Vector3(2, .04f, -5), new Vector3(2, 1.65f, -2));
            yield return null; yield return null;
            Require(!portable.PlacementValid, "No supported careful pose when aiming into air");
            Require(!portable.preview.enabled && session.hud.targetText.text.Length == 0,
                "Unsupported placement aim has no outline or continuous validity guidance");
            Capture(session, "placement-4-unsupported-aim.png");
            yield return KeyPress(keyboard, Key.E);
            Require(handling.State.IsHeld, "Invalid careful placement retains the crate");
            Require(session.hud.noticeText.text == portable.PlacementReason, "Rejected E placement explains the refusal");
            float height = portable.Pose.Position.y;
            yield return KeyPress(keyboard, Key.G);
            Require(!handling.State.IsHeld && !portable.body.isKinematic, "G drops with real physics despite invalid careful placement");
            session.SendMessage("OnApplicationFocus", false);
            var frozen = portable.Pose;
            yield return new WaitForSecondsRealtime(.2f);
            Require(portable.Pose.Position == frozen.Position, "Focus pause freezes falling crate");
            session.SendMessage("OnApplicationFocus", true); session.Resume();
            yield return new WaitForSecondsRealtime(1.1f);
            Require(portable.Pose.Position.y < height - .15f && portable.Pose.Position.y > -.12f && portable.Settled,
                "Dropped crate collides with ground and settles without a burst");
            Require(handling.State.RawUnits == 12 && portable.ContactCues > 0, "Drop keeps full load and dispatches restrained contact feedback");
            Capture(session, "placement-4-dropped.png");
            var safe = handling.State.SafeRawPose;
            portable.body.position = new Vector3(0, -8, 0);
            portable.transform.position = portable.body.position;
            yield return null; yield return null;
            Require(portable.InBounds && Vector3.Distance(portable.Pose.Position, safe.Position) < .05f,
                "Lost loaded crate automatically returns to its last safe pose");
        }

        IEnumerator VerifyProcessing(YardSession session, Keyboard keyboard, Mouse mouse)
        {
            var handling = session.handling;
            var state = handling.State;
            var station = handling.station;
            Require(state.RawUnits == 1, "Processing probe starts with the actually scooped partial load");
            Aim(session, new Vector3(2.65f, .04f, -1.8f), station.intake.position);
            Require(session.targeting.Current == station.intakeTarget, "Saved broad intake is reachable");
            yield return KeyPress(keyboard, Key.E);
            Require(state.ActiveUnits == 1 && state.RawUnits == 0, "Packaged E commits a partial load once");
            yield return new WaitForSeconds(.24f);
            Require(handling.tipping.IsPlaying && Quaternion.Angle(handling.crate.transform.rotation, handling.crate.carryAnchor.rotation) > 20,
                "Tip visibly tilts the crate");
            Require(handling.crate.transform.TransformPoint(new Vector3(.48f, .42f, 0)).y > station.intake.position.y + .2f,
                "The pouring edge stays above the intake");
            Capture(session, "07-tip.png");
            session.SendMessage("OnApplicationFocus", false);
            double pausedBatch = state.BatchRemaining;
            yield return new WaitForSecondsRealtime(.25f);
            Require(state.BatchRemaining == pausedBatch && !handling.tipping.IsPlaying, "Focus pause freezes processing and cancels only the visual");
            session.SendMessage("OnApplicationFocus", true);
            Require(session.IsPaused, "Processing focus return stays paused");
            yield return KeyPress(keyboard, Key.Escape);
            yield return new WaitForSeconds(4.1f);
            Require(state.OutputUnits == 1 && station.jars[0].activeSelf && Mathf.Abs(station.jarFood[0].localScale.y - .04f) < .001f,
                "Single pepper finishes as a visible partial jar");
            Capture(session, "08-partial-output.png");
            // Repeat real scoop input from the next clump, then fill the remaining output room.
            yield return GatherForProcessing(session, mouse, 2);
            Aim(session, new Vector3(2.65f, .04f, -1.8f), station.intake.position);
            yield return KeyPress(keyboard, Key.E);
            Require(state.ActiveUnits == 11 && state.QueuedUnits == 1 && state.OutputUnits == 1, "Only free output room is reserved");
            yield return new WaitForSeconds(.26f);
            Capture(session, "09-full-cascade.png");
            yield return new WaitForSeconds(3.9f);
            Require(state.OutputUnits == 12 && state.ActiveUnits == 0 && state.QueuedUnits == 1, "Full output safely retains queued food");
            Capture(session, "10-full-output.png");
            yield return GatherForProcessing(session, mouse, 1);
            Aim(session, new Vector3(2.65f, .04f, -1.8f), station.intake.position);
            yield return KeyPress(keyboard, Key.E);
            Require(state.RawUnits == 1 && state.QueuedUnits == 12, "Limited intake accepts eleven and keeps one in the crate");
            yield return new WaitForSeconds(.85f);
            yield return KeyPress(keyboard, Key.E);
            int denied = handling.presentation.FeedbackCues;
            yield return KeyPress(keyboard, Key.E);
            Require(handling.presentation.FeedbackCues == denied && state.RawUnits == 1, "Repeated full-input press stays quiet and preserves load");
            Require(state.AccountedUnits == state.InitialHarvest, "Pile, crate, queue, active and output conserve every unit");
            Require(AudioListener.volume == 0 && !station.audioSource.ignoreListenerVolume && !handling.tipping.audioSource.ignoreListenerVolume,
                "Processing audio respects the probe mute");
            Capture(session, "11-full-input.png");
            yield return KeyPress(keyboard, Key.R);
            Require(state.RawUnits == 1 && state.QueuedUnits == 12 && state.OutputUnits == 12 && state.AccountedUnits == 107,
                "Recovery preserves loaded crate and station food");
            yield return KeyPress(keyboard, Key.F8);
            Require(state.Remaining == 107 && state.RawUnits + state.QueuedUnits + state.ActiveUnits + state.OutputUnits == 0,
                "Explicit restart resets the whole processing test");
        }

        IEnumerator VerifyFinishedFood(YardSession session, Keyboard keyboard, Mouse mouse)
        {
            var handling = session.handling;
            var state = handling.State;
            var food = handling.finished;
            int deposits = 0;
            while (state.Remaining > 0)
            {
                // Existing probe sections verify actual scooping. Prepare the remaining full-job loads through model commands.
                state.PickUp();
                int amount = Mathf.Min(12, state.Remaining);
                int remaining = amount;
                foreach (var region in handling.regions) remaining -= state.Gather(region.regionId, remaining);
                Require(remaining == 0, "Prepared next finite load for complete handoff probe");
                handling.Render();
                Aim(session, new Vector3(2.65f, .04f, -1.8f), handling.station.intake.position);
                yield return null; yield return null; yield return KeyPress(keyboard, Key.E);
                yield return new WaitForSeconds(4.1f);
                Require(state.OutputUnits == amount, "Actual tip and automatic processing finish the next load");
                Aim(session, new Vector3(5.8f, .04f, -5.1f), new Vector3(5.8f, 0, -3.3f));
                yield return null; yield return null; yield return KeyPress(keyboard, Key.E);
                yield return new WaitForSeconds(.6f);
                Require(!state.IsHeld, "Set down raw crate explicitly before receiving finished food");
                Aim(session, new Vector3(4.45f, .04f, -1.3f), food.carrier.dock.position + Vector3.up * .2f);
                yield return null; yield return null;
                Require(session.targeting.Current == food.outputTarget, "Receiving tray has clear broad targeting");
                yield return KeyPress(keyboard, Key.E);
                yield return new WaitForSeconds(.12f);
                Require(food.carrier.IsReceiving && state.FinishedHeld && state.FinishedUnits == amount && state.OutputUnits == 0,
                    "E collects available food once and moves the receiving carrier");
                if (deposits == 0) Capture(session, "12-receiving-food.png");
                yield return new WaitForSeconds(.4f);
                if (deposits == 0)
                {
                    Capture(session, "13-finished-load.png");
                    var rawPose = handling.crate.portable.Pose;
                    Aim(session, new Vector3(0, .04f, -5), new Vector3(0, 1.65f, -2));
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.LeftShift, Key.Space));
                    yield return new WaitForSeconds(.15f);
                    Require(session.player.transform.position.y > .4f && state.FinishedUnits == 12 && state.FinishedHeld,
                        "Finished load survives actual sprint/jump input");
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return new WaitForSeconds(.65f);
                    var points = new[] { new Vector3(.3f, 0, -3.8f), new Vector3(-5.7f, .84f, -3.9f) };
                    var approaches = new[] { new Vector3(.3f, .04f, -5.6f), new Vector3(-3.8f, .04f, -3.9f) };
                    for (int i = 0; i < points.Length; i++)
                    {
                        Aim(session, approaches[i], points[i]);
                        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.X)); yield return new WaitForSeconds(.2f);
                        InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
                        var body = food.carrier.portable;
                        Require(body.PlacementValid && session.hud.targetText.text.Length == 0, "Finished carrier offers quiet supported placement");
                        var chosen = body.Placement;
                        yield return KeyPress(keyboard, Key.E); yield return new WaitForSeconds(.6f);
                        Require(!state.FinishedHeld && state.StoredUnits == 0 && Vector3.Distance(body.Pose.Position, chosen.Position) < .035f &&
                            Quaternion.Angle(body.Pose.Rotation, chosen.Rotation) < 2, "Finished load settles at chosen pose without depositing");
                        Capture(session, "finished-placement-" + i + ".png");
                        Aim(session, approaches[i], body.Pose.Position + Vector3.up * .24f);
                        yield return null; yield return null;
                        Require(session.targeting.Current == food.carrier.target, "Packed jars retain the carrier's grab target");
                        yield return MousePress(mouse);
                    }
                    Aim(session, new Vector3(2, .04f, -5), new Vector3(2, 1.65f, -2)); yield return null; yield return null;
                    yield return KeyPress(keyboard, Key.G);
                    Require(!state.FinishedHeld && state.FinishedUnits == 12, "G drops finished food with contents kept");
                    session.SendMessage("OnApplicationFocus", false);
                    var frozen = food.carrier.portable.Pose;
                    yield return new WaitForSecondsRealtime(.2f);
                    Require(food.carrier.portable.Pose.Position == frozen.Position, "Focus pause freezes falling finished carrier");
                    session.SendMessage("OnApplicationFocus", true); session.Resume();
                    yield return new WaitForSeconds(1.1f);
                    var safe = food.carrier.portable.Pose;
                    Require(food.carrier.portable.Settled && food.carrier.portable.ContactCues > 0, "Finished load contacts and settles after drop");
                    food.carrier.portable.SetPose(new CarrierPose(new Vector3(0, -8, 0), Quaternion.identity), false);
                    yield return null; yield return null;
                    Require(food.carrier.portable.InBounds && state.FinishedUnits == 12 &&
                        Vector3.Distance(handling.crate.portable.Pose.Position, rawPose.Position) < .05f, "Loaded recovery preserves raw arrangement and food");
                    Aim(session, safe.Position + new Vector3(0, .04f, -1.8f), food.carrier.portable.Pose.Position + Vector3.up * .24f);
                    yield return null; yield return null; yield return MousePress(mouse);
                    Require(state.FinishedHeld, "Recovered finished food is regrabbable");
                }
                Aim(session, new Vector3(3, .04f, 3.2f), food.rackTarget.transform.position + Vector3.up);
                yield return null; yield return null;
                Require(session.targeting.Current == food.rackTarget, "The same generous handoff rack remains reachable");
                int before = state.StoredUnits;
                yield return KeyPress(keyboard, Key.E); yield return KeyPress(keyboard, Key.E);
                Require(state.StoredUnits == before + amount && state.FinishedUnits == 0 && state.FinishedDocked,
                    "One handoff credits the load once and returns the empty carrier automatically");
                Require(food.carrier.portable.Pose.Position == food.carrier.dock.position && state.AccountedUnits == 107,
                    "The reusable carrier and all food have one owner");
                deposits++;
                if (deposits <= 2 || state.StoredUnits == 107)
                {
                    Capture(session, "stored-food-" + state.StoredUnits + ".png");
                    Aim(session, new Vector3(4.45f, .04f, -1.3f), food.rackTarget.transform.position + Vector3.up * 1.5f);
                    yield return null; yield return null;
                    Capture(session, "stored-food-from-work-" + state.StoredUnits + ".png");
                }
            }
            Require(deposits == 9 && state.StoredUnits == 107 && state.RawUnits + state.FinishedUnits + state.QueuedUnits + state.ActiveUnits + state.OutputUnits == 0,
                "All 107 units reach storage, including the final eleven-unit load");
            Require(food.storedFood.jars[35].activeSelf && Mathf.Abs(food.storedFood.food[35].localScale.y - .08f) < .001f,
                "Stored-food display includes the exact final partial fill");
            yield return KeyPress(keyboard, Key.R);
            Require(state.StoredUnits == 107 && !session.IsPaused, "Recovery retains stored food and normal yard control");
        }

        IEnumerator GatherForProcessing(YardSession session, Mouse mouse, int regionIndex)
        {
            var handling = session.handling;
            var region = handling.regions[regionIndex];
            Aim(session, new Vector3(region.transform.position.x, .04f, -2.65f), region.volume.position);
            yield return null; yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 });
            float gatherDeadline = Time.time + 8;
            while (handling.State.RawUnits < 12 && Time.time < gatherDeadline) yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState());
            yield return null; yield return null;
            Require(handling.State.RawUnits == 12, "Another actual scoop load is available to process");
        }

        static void Aim(YardSession session, Vector3 position, Vector3 target)
        {
            var pose = new GameObject("Smoke approach").transform;
            pose.position = position;
            var direction = target - (position + Vector3.up * 1.65f);
            pose.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
            session.player.ResetTo(pose);
            UnityEngine.Object.Destroy(pose.gameObject);
            float pitch = Mathf.Atan2(-direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;
            session.player.Step(Vector2.zero, new Vector2(0, -pitch / session.player.lookSensitivity), false, false, 1f / 60);
            Physics.SyncTransforms();
            session.targeting.Refresh();
        }

        static IEnumerator KeyPress(Keyboard keyboard, Key key)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            yield return null;
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            yield return null;
        }

        static void Require(bool condition, string check)
        {
            if (!condition) throw new InvalidOperationException(check);
            Debug.Log("FOUNDATION_CHECK " + check);
        }

        void Capture(YardSession session, string name)
        {
            var camera = session.player.view;
            var canvas = session.hud.GetComponent<Canvas>();
            var render = new RenderTexture(1440, 900, 24);
            var pixels = new Texture2D(1440, 900, TextureFormat.RGB24, false);
            var previous = RenderTexture.active;
            camera.targetTexture = render;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = camera.nearClipPlane + .01f;
            Canvas.ForceUpdateCanvases();
            camera.Render();
            RenderTexture.active = render;
            pixels.ReadPixels(new Rect(0, 0, 1440, 900), 0, 0);
            pixels.Apply();
            File.WriteAllBytes(Path.Combine(output, name), pixels.EncodeToPNG());
            RenderTexture.active = previous;
            camera.targetTexture = null;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            Destroy(pixels);
            render.Release();
            Destroy(render);
        }

        void OnLog(string message, string trace, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert) Fail(message + "\n" + trace);
        }

        void Update()
        {
            if (!finished && Time.realtimeSinceStartup > deadline) Fail("Timed out after 210 seconds");
        }

        void Fail(string message)
        {
            if (finished) return;
            finished = true;
            File.WriteAllText(Path.Combine(output, "result.txt"), "FAIL: " + message);
            Application.Quit(1);
        }

        void OnDestroy() => Application.logMessageReceived -= OnLog;
    }
}
