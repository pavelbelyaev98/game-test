using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace JustAFewPeppers
{
    // Local verification of the exact playtest or diagnostic player; requires batch mode and an explicit flag.
    public sealed class FoundationBuildSmoke : MonoBehaviour
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
            probe.deadline = Time.realtimeSinceStartup + 100;
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
            yield return VerifyHandling(session, keyboard, mouse);
            yield return VerifyProcessing(session, keyboard, mouse);
            Require(AudioListener.volume == 0, "Automated player audio stays muted");
            InputSystem.RemoveDevice(keyboard);
            InputSystem.RemoveDevice(mouse);
            finished = true;
            File.WriteAllText(Path.Combine(output, "result.txt"), "PASS: " + (Debug.isDebugBuild ? "Development" : "Playtest") + " player; packaged scene/menu, walking, sprint speed, jump/landing without held repeat, midair pause freeze, simulated focus callbacks, resume and safe-spawn reset; crate pickup, immediate/local scooping, partial depletion, 12-unit fill, quiet held-full feedback, loaded sprint/jump, rotated ground/worktop/support placement and regrab, gravity/contact/settling, drop focus freeze, safe-pose recovery, prototype reset and conservation; E tipping, visible cascade/tilt, partial jar, output reservation/accumulation, limited acceptance, quiet full input, processing pause/focus and all-food recovery/reset.\nImages: 01-menu.png through 11-full-input.png plus placement-0 through placement-4 captures.\nPhysical focus switching, sound and handling comfort require the tester's playtest.\n");
            Debug.Log("FOUNDATION_BUILD_SMOKE_PASS");
            Application.Quit(0);
        }

        IEnumerator VerifyHandling(YardSession session, Keyboard keyboard, Mouse mouse)
        {
            var handling = session.handling;
            Require(handling != null && handling.State.InitialHarvest == 107, "Saved handling model has the authored finite supply");
            Aim(session, new Vector3(-1.25f, .04f, -4.4f), handling.crate.transform.position + Vector3.up * .3f);
            yield return KeyPress(keyboard, Key.E);
            Require(handling.State.IsHeld, "Packaged E input picks up the unique crate");
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
            yield return VerifyFreePlacement(session, keyboard);
            yield return KeyPress(keyboard, Key.R);
            Require(handling.State.RawUnits == 12 && handling.State.Remaining == 95, "Recovery preserves depletion and load");
            yield return KeyPress(keyboard, Key.F8);
            Require(handling.State.RawUnits == 0 && handling.State.Remaining == 107, "Explicit prototype restart restores authored state");
            Require(session.Input.Actions.FindAction("Gameplay/ScoopMode") == null, "Packaged controls have no scoop mode action");
            Aim(session, new Vector3(-1.25f, .04f, -4.4f), handling.crate.transform.position + Vector3.up * .3f);
            yield return KeyPress(keyboard, Key.E);
            Aim(session, new Vector3(-3, .04f, -2.65f), region.volume.position);
            yield return KeyPress(keyboard, Key.T);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 });
            yield return null; yield return null;
            Require(handling.State.RawUnits == 1, "Hold-only scoop starts on left mouse");
            InputSystem.QueueStateEvent(mouse, new MouseState());
            yield return new WaitForSecondsRealtime(.7f);
            Require(handling.State.RawUnits == 1, "Release stops scooping; T cannot enable automatic gathering");
        }

        IEnumerator VerifyFreePlacement(YardSession session, Keyboard keyboard)
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
                yield return KeyPress(keyboard, Key.E);
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
            if (!finished && Time.realtimeSinceStartup > deadline) Fail("Timed out after 100 seconds");
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
