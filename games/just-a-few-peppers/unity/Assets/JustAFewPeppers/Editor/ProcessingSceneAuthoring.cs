using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static JustAFewPeppers.Editor.FoundationSceneBuilder;

namespace JustAFewPeppers.Editor
{
    // Add the selected task to the existing authored scene without rebuilding the yard or controls.
    public static class ProcessingSceneAuthoring
    {
        const string Content = "Assets/JustAFewPeppers/Content/";

        [MenuItem("Just a few peppers/Apply 1_03 processing to handling")]
        public static void Apply()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            var handling = session.handling;
            if (handling == null || handling.station != null)
                throw new InvalidOperationException("Requires authored handling without processing. Do not recreate existing processing.");
            var root = GameObject.Find("Automatic processor");
            var station = root.AddComponent<StationView>();
            handling.station = station;
            var metal = AssetDatabase.LoadAssetAtPath<Material>(Content + "Metal.mat");
            var red = AssetDatabase.LoadAssetAtPath<Material>(Content + "Mound.mat");
            var orange = AssetDatabase.LoadAssetAtPath<Material>(Content + "Ripe pepper.mat");
            var marker = AssetDatabase.LoadAssetAtPath<Material>(Content + "Target marker.mat");
            var jar = Material("Jar body", new Color(.63f, .75f, .62f));
            // Only the broad tray and its rim are targets. The machine body/output are scenery.
            UnityEngine.Object.DestroyImmediate(root.GetComponent<YardTarget>());
            var tray = new GameObject("Broad intake");
            tray.transform.SetParent(root.transform, false);
            tray.transform.localPosition = new Vector3(-.35f, 1.13f, -.85f);
            var target = tray.AddComponent<YardTarget>();
            target.displayName = "Automatic processor intake";
            target.description = "E  Tip the carried load";
            station.intakeTarget = target;
            station.intake = tray.transform;
            Box("Intake tray", Vector3.zero, new Vector3(1.55f, .12f, .85f), metal, tray.transform);
            foreach (float x in new[] { -.75f, .75f })
                Box("Intake side", new Vector3(x, .12f, 0), new Vector3(.07f, .22f, .85f), metal, tray.transform);
            target.marker = Box("Tip marker", new Vector3(0, -.02f, -.44f), new Vector3(1.2f, .12f, .035f), marker, tray.transform, false).GetComponent<Renderer>();
            Label("E  TIP LOAD", tray.transform, new Vector3(0, -.02f, -.465f), .008f);
            station.queuedPeppers = new GameObject[12];
            for (int i = 0; i < 12; i++)
            {
                station.queuedPeppers[i] = Shape(PrimitiveType.Sphere, "Queued pepper", tray.transform,
                    new Vector3((i % 4 - 1.5f) * .26f, .12f, (i / 4 - 1) * .22f), new Vector3(.16f, .12f, .25f), red, false);
                station.queuedPeppers[i].SetActive(false);
            }
            station.jars = new GameObject[4];
            station.jarFood = new Transform[4];
            for (int i = 0; i < 4; i++)
            {
                var group = new GameObject("Finished jar " + (i + 1));
                group.transform.SetParent(root.transform, false);
                group.transform.localPosition = new Vector3(.48f + i % 2 * .32f, 1.1f, -.22f + i / 2 * .42f);
                Shape(PrimitiveType.Cylinder, "Jar base", group.transform, new Vector3(0, .025f, 0), new Vector3(.26f, .025f, .26f), jar, false);
                Shape(PrimitiveType.Cylinder, "Jar lid", group.transform, new Vector3(0, .31f, 0), new Vector3(.26f, .025f, .26f), metal, false);
                // Open graybox silhouette leaves exact partial food fill visible.
                foreach (float x in new[] { -.11f, .11f })
                    Box("Jar edge", new Vector3(x, .17f, 0), new Vector3(.025f, .28f, .025f), jar, group.transform, false);
                station.jarFood[i] = Shape(PrimitiveType.Cylinder, "Roasted pepper fill", group.transform,
                    new Vector3(0, .14f, 0), new Vector3(.21f, .12f, .21f), orange, false).transform;
                station.jars[i] = group;
                group.SetActive(false);
            }
            station.stageMarkers = new Renderer[4];
            for (int i = 0; i < 4; i++)
                station.stageMarkers[i] = Box("Automatic stage " + i, new Vector3(-.8f + i * .33f, 1.85f, .15f),
                    new Vector3(.22f, .1f, .08f), marker, root.transform, false).GetComponent<Renderer>();
            station.feeder = Box("Feeder motion", new Vector3(-.35f, 1.7f, 0), new Vector3(.6f, .08f, .15f), metal, root.transform, false).transform;
            station.stageLabel = root.transform.Find("03  PROCESS").GetComponent<TextMesh>();
            station.stageLabel.characterSize = .0075f;
            station.stageLabel.transform.localPosition = new Vector3(.65f, .83f, -.77f);
            station.audioSource = root.AddComponent<AudioSource>();
            station.audioSource.playOnAwake = false;
            station.completionClip = handling.presentation.crateClip;
            var tipping = new GameObject("Tip cascade").AddComponent<TipPresentation>();
            handling.tipping = tipping;
            tipping.crate = handling.crate;
            tipping.destination = station.intake;
            tipping.audioSource = tipping.gameObject.AddComponent<AudioSource>();
            tipping.audioSource.playOnAwake = false;
            tipping.impactClip = handling.presentation.softCue;
            tipping.finishClip = handling.presentation.crateClip;
            tipping.flyingPeppers = new Transform[9];
            for (int i = 0; i < 9; i++)
            {
                var pepper = Shape(PrimitiveType.Sphere, "Cascade pepper proxy", tipping.transform, Vector3.zero,
                    new Vector3(.16f, .12f, .24f), i % 3 == 0 ? orange : red, false);
                tipping.flyingPeppers[i] = pepper.transform;
                pepper.SetActive(false);
            }
            var hud = session.hud;
            hud.transform.Find("Scope").GetComponent<Text>().text = "Scoop, tip & process  /  Output collection comes next";
            station.statusText = Text("Station status", hud.transform, "", new Vector2(0, 295), new Vector2(1380, 38), 21);
            hud.transform.Find("Controls").GetComponent<Text>().text =
                "WASD / Arrows  Move     Shift  Sprint     Space  Jump     Mouse  Look\nE  Pick up / tip at intake / park at a mat     Hold left mouse  Scoop\nEsc  Pause     R  Recover (keeps all food)     F8  Restart processing test";
            hud.pausePanel.transform.Find("Pause help").GetComponent<Text>().text =
                "Scoop peppers, bring the crate to the intake, press E.\nR keeps all food. Restart restores the entire test.";
            hud.restartPrototypeButton.GetComponentInChildren<Text>().text = "Restart processing test (clears food)";
            station.Render(new HarvestState(new[] { "preview" }, new[] { 107 }, 12, 2));
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Could not save processing scene.");
            AssetDatabase.SaveAssets();
            Debug.Log("PROCESSING_SCENE_AUTHORED " + ScenePath);
        }

        [MenuItem("Just a few peppers/Tune 1_03 station labels")]
        public static void TuneLabels()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var station = UnityEngine.Object.FindAnyObjectByType<YardSession>().handling.station;
            station.stageLabel.characterSize = .0075f;
            station.stageLabel.transform.localPosition = new Vector3(.65f, .83f, -.77f);
            var tipLabel = station.intake.Find("E  TIP LOAD").GetComponent<TextMesh>();
            tipLabel.characterSize = .008f;
            tipLabel.transform.localPosition = new Vector3(0, -.02f, -.465f);
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Could not save station label tuning.");
        }
    }
}
