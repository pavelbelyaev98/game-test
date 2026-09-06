#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace JustAFewPeppers
{
    // Opt-in automation of the actual development player. Absent from release builds.
    public sealed class FoundationBuildSmoke : MonoBehaviour
    {
        string output;
        float deadline;
        bool finished;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void StartIfRequested()
        {
            var args = Environment.GetCommandLineArgs();
            int flag = Array.IndexOf(args, "-foundationSmoke");
            if (flag < 0 || flag + 1 >= args.Length) return;
            var probe = new GameObject("Foundation build verification").AddComponent<FoundationBuildSmoke>();
            probe.output = Path.GetFullPath(args[flag + 1]);
            Directory.CreateDirectory(probe.output);
            probe.deadline = Time.realtimeSinceStartup + 30;
            Application.logMessageReceived += probe.OnLog;
        }

        IEnumerator Start()
        {
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
            yield return KeyPress(keyboard, Key.Escape);
            Require(session.IsPaused && Time.timeScale == 0, "Escape pauses the packaged player");
            var pausedPosition = session.player.transform.position;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
            yield return new WaitForSecondsRealtime(.1f);
            Require(session.player.transform.position == pausedPosition, "Paused movement stays frozen");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            session.SendMessage("OnApplicationFocus", false);
            session.SendMessage("OnApplicationFocus", true);
            Require(session.IsPaused, "Focus return waits for explicit resume");
            yield return KeyPress(keyboard, Key.Escape);
            yield return KeyPress(keyboard, Key.R);
            Require(Vector3.Distance(session.player.transform.position, session.safeSpawn.position) < .1f, "Reset returns to safe spawn");
            InputSystem.RemoveDevice(keyboard);
            InputSystem.RemoveDevice(mouse);
            finished = true;
            File.WriteAllText(Path.Combine(output, "result.txt"), "PASS: packaged scene/menu, input movement, pause freeze, simulated focus callbacks, resume and safe-spawn reset.\nImages: 01-menu.png, 02-yard.png.\nPhysical focus switching and camera comfort require Pavel's playtest.\n");
            Debug.Log("FOUNDATION_BUILD_SMOKE_PASS");
            Application.Quit(0);
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
            canvas.planeDistance = 1;
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
            if (!finished && Time.realtimeSinceStartup > deadline) Fail("Timed out after 30 seconds");
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
#endif
