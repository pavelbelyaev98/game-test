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
            probe.deadline = Time.realtimeSinceStartup + 60;
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
            Require(AudioListener.volume == 0, "Automated player audio stays muted");
            InputSystem.RemoveDevice(keyboard);
            InputSystem.RemoveDevice(mouse);
            finished = true;
            File.WriteAllText(Path.Combine(output, "result.txt"), "PASS: " + (Debug.isDebugBuild ? "Development" : "Playtest") + " player; packaged scene/menu, walking, sprint speed, jump/landing without held repeat, midair pause freeze, simulated focus callbacks, resume and safe-spawn reset; crate pickup, immediate/local scooping, partial depletion, 12-unit fill, quiet held-full feedback, loaded sprint/jump, parking, recovery, prototype reset and conservation.\nImages: 01-menu.png, 02-yard.png, 03-jump.png, 04-scoop.png, 05-loaded.png, 06-parked.png.\nPhysical focus switching, sound and handling comfort require the tester's playtest.\n");
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
            Aim(session, new Vector3(-1.25f, .04f, -4.1f), handling.crate.restingPoints[0].position + Vector3.up * .3f);
            yield return new WaitForSecondsRealtime(.1f);
            yield return KeyPress(keyboard, Key.E);
            Require(!handling.State.IsHeld && handling.State.RawUnits == 12, "E parks the full crate on a clear mat");
            Require(session.hud.targetText.text.Contains("Pick up"), "A parked full crate prompts pickup");
            Capture(session, "06-parked.png");
            yield return KeyPress(keyboard, Key.R);
            Require(handling.State.RawUnits == 12 && handling.State.Remaining == 95, "Gate recovery preserves earned depletion and load");
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
            if (!finished && Time.realtimeSinceStartup > deadline) Fail("Timed out after 60 seconds");
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


