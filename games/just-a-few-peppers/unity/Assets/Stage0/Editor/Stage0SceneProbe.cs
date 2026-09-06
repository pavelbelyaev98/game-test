using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Chushkopek.Stage0.Editor
{
    // Editor-only integration smoke check. It deliberately does not judge mouse feel or audio quality.
    [InitializeOnLoad]
    public static class Stage0SceneProbe
    {
        const string Running = "Chushkopek.Stage0.ProbeRunning";
        static int step;
        static double next;
        static double deadline;
        static bool exiting;
        static int editorSearchExceptions;

        static Stage0SceneProbe()
        {
            EditorApplication.playModeStateChanged += state => {
                if (!SessionState.GetBool(Running, false)) return;
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    step = 0; next = EditorApplication.timeSinceStartup + .7; deadline = next + 60;
                    EditorApplication.update += Tick;
                    Application.logMessageReceived += OnLog;
                }
            };
        }

        public static void Run()
        {
            Stage0SceneBuilder.CreateScene();
            EditorSceneManager.OpenScene(Stage0SceneBuilder.ScenePath);
            SessionState.SetBool(Running, true);
            EditorApplication.EnterPlaymode();
        }

        static void OnLog(string message, string trace, LogType type)
        {
            // Observed Unity 6000.6 batch-startup issue, outside the scene. Keep it in the report.
            if (type == LogType.Exception && message.StartsWith("ArgumentOutOfRangeException") &&
                trace.Contains("UnityEditor.Search.SearchInit.IndexationOnStartup"))
            {
                editorSearchExceptions++;
                Debug.LogWarning("Stage 0 probe recorded an editor search-index startup exception; gameplay errors still fail the run.");
                return;
            }
            if (type == LogType.Exception || type == LogType.Error) Finish(false, message + "\n" + trace);
        }

        static void Tick()
        {
            if (exiting || EditorApplication.timeSinceStartup < next) return;
            try
            {
                Require(EditorApplication.timeSinceStartup < deadline, "Probe timed out");
                var s = UnityEngine.Object.FindAnyObjectByType<Stage0Session>();
                Require(s && s.State != null, "Session did not initialize");
                if (s.Paused) s.SetPaused(false);
                switch (step)
                {
                    case 0:
                        Require(UnityEngine.Object.FindObjectsByType<PepperPresentation>().Length == 1, "Expected one pepper");
                        Require(Camera.allCamerasCount == 1, "Expected one camera");
                        Vector3 fleshPole = s.pepper.flesh.GetComponent<MeshFilter>().sharedMesh.vertices[0];
                        Require(Vector3.Distance(fleshPole, new Vector3(0, PepperGeometry.Length, 0)) < .0001f, "Stale or open-ended flesh mesh");
                        RequirePepperSelectable(s);
                        Capture(s, "01-raw", s.pepper.transform.position + Vector3.up * .1f);
                        Require(s.Pick(), "Pick raw");
                        Require(!s.Place(PepperLocation.Finished), "Raw tray placement must reject");
                        Require(s.Place(PepperLocation.Roaster), "Place roaster");
                        step++; break;
                    case 1:
                        if (s.State.Stage != PepperStage.Perfect) return;
                        Capture(s, "02-perfect", s.roaster.socket.position + Vector3.up * .35f);
                        Require(s.Pick(), "Lift perfect"); float heat = s.State.RoastElapsed; s.State.Tick(20);
                        Require(s.State.RoastElapsed == heat && !s.State.WasBurnt, "Heat continued after removal");
                        Require(s.Place(PepperLocation.Steam), "Place steam"); step++; break;
                    case 2:
                        if (s.State.Stage != PepperStage.Peelable) return;
                        next = EditorApplication.timeSinceStartup + .4; step++; break;
                    case 3:
                        RequirePepperSelectable(s);
                        Capture(s, "03-peelable", s.pepper.transform.position + s.pepper.transform.up * .28f);
                        s.Peel(0, .45f); next = EditorApplication.timeSinceStartup + .4; step++; break;
                    case 4:
                        Require(Math.Abs(s.State.StripProgress(0) - .45f) < .0001f, "Idle peeling progressed");
                        Capture(s, "04-partial-peel", s.pepper.transform.position + s.pepper.transform.up * .28f);
                        s.Peel(0, .55f); s.Peel(1, 1f); next = EditorApplication.timeSinceStartup + .7; step++; break;
                    case 5:
                        Require(s.State.Stage == PepperStage.Peeled, "Peel did not finish");
                        Capture(s, "05-peeled", s.pepper.transform.position + s.pepper.transform.up * .28f);
                        Require(s.Pick() && s.Place(PepperLocation.Finished), "Finish placement");
                        Require(!s.Place(PepperLocation.Finished) && s.State.CompletionCount == 1, "Duplicate completion");
                        next = EditorApplication.timeSinceStartup + .4; step++; break;
                    case 6:
                        Capture(s, "06-finished", s.tray.socket.position + s.tray.socket.up * .28f);
                        s.ResetCycle(); next = EditorApplication.timeSinceStartup + .2; step++; break;
                    case 7:
                        Require(s.State.Stage == PepperStage.Raw && s.State.Location == PepperLocation.Start, "Reset failed");
                        RequirePepperSelectable(s);
                        Require(s.Pick() && s.Place(PepperLocation.Roaster), "Second cycle start");
                        s.State.Tick(20); next = EditorApplication.timeSinceStartup + .4; step++; break;
                    case 8:
                        Capture(s, "07-burnt", s.roaster.socket.position + Vector3.up * .35f);
                        Require(s.State.WasBurnt && s.Pick() && s.Place(PepperLocation.Steam), "Burnt pepper not recoverable");
                        step++; break;
                    case 9:
                        if (s.State.Stage != PepperStage.Peelable) return;
                        s.Peel(0, 1); s.Peel(1, 1);
                        Require(s.Pick() && s.Place(PepperLocation.Finished), "Burnt completion");
                        next = EditorApplication.timeSinceStartup + .7; step++; break;
                    case 10:
                        Capture(s, "08-burnt-finished", s.tray.socket.position + s.tray.socket.up * .28f);
                        Require(s.State.CompletionCount == 1 && s.State.WasBurnt, "Burnt quality / completion changed");
                        Finish(true, "Two scene cycles, invalid placement recovery, stopping heat, partial peel, single completion, reset, selection rays; 8 captures.");
                        break;
                }
            }
            catch (Exception error) { Finish(false, error.ToString()); }
        }

        static void RequirePepperSelectable(Stage0Session s)
        {
            Physics.SyncTransforms();
            Vector3 center = s.pepper.transform.TransformPoint(0, .28f, 0);
            Vector3 origin = s.interaction.view.transform.position;
            Require(Physics.Raycast(origin, (center - origin).normalized, out RaycastHit hit, s.interaction.reach, ~0, QueryTriggerInteraction.Collide), "No pepper selection hit");
            var target = hit.collider.GetComponentInParent<Stage0Target>();
            Require(target && target.isPepper, "Pepper is occluded by " + hit.collider.name);
        }

        static void Capture(Stage0Session s, string name, Vector3 focus)
        {
            Directory.CreateDirectory("Logs/Stage0Captures");
            Camera camera = s.interaction.view;
            Quaternion rotation = camera.transform.rotation;
            float fov = camera.fieldOfView;
            camera.transform.LookAt(focus); camera.fieldOfView = 46;
            var render = new RenderTexture(1280, 800, 24);
            camera.targetTexture = render; camera.Render();
            RenderTexture old = RenderTexture.active; RenderTexture.active = render;
            var texture = new Texture2D(1280, 800, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, 1280, 800), 0, 0); texture.Apply();
            File.WriteAllBytes("Logs/Stage0Captures/" + name + ".png", texture.EncodeToPNG());
            camera.targetTexture = null; RenderTexture.active = old;
            UnityEngine.Object.DestroyImmediate(texture); render.Release(); UnityEngine.Object.DestroyImmediate(render);
            camera.transform.rotation = rotation; camera.fieldOfView = fov;
        }

        static void Require(bool condition, string message) { if (!condition) throw new Exception(message); }
        static void Finish(bool success, string message)
        {
            if (exiting) return;
            exiting = true; SessionState.SetBool(Running, false); EditorApplication.update -= Tick; Application.logMessageReceived -= OnLog;
            message += " Editor search-index startup exceptions recorded separately: " + editorSearchExceptions + ".";
            Directory.CreateDirectory("Logs"); File.WriteAllText("Logs/stage0-scene-probe-result.txt", (success ? "PASS " : "FAIL ") + message);
            Debug.Log((success ? "STAGE0_SCENE_PROBE_PASS " : "STAGE0_SCENE_PROBE_FAIL ") + message);
            EditorApplication.Exit(success ? 0 : 1);
        }
    }
}
