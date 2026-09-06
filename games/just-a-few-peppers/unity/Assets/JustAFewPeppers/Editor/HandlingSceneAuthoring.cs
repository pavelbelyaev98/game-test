using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static JustAFewPeppers.Editor.FoundationSceneBuilder;

namespace JustAFewPeppers.Editor
{
    // A focused one-time migration of the saved foundation, never a scene generator.
    public static class HandlingSceneAuthoring
    {
        const string AudioPath = "Assets/JustAFewPeppers/Content/Audio/";
        const string HandlingHelp = "WASD / Arrows  Move     Shift  Sprint     Space  Jump     Mouse  Look\nE  Pick up / park at a mat     Hold left mouse  Scoop\nEsc  Pause     R  Recover player + crate (keeps load)     F8  Restart scoop test";

        internal static void AddHandlingActions(InputActionMap gameplay)
        {
            foreach (var binding in new[] { ("Scoop", "<Mouse>/leftButton"), ("Interact", "<Keyboard>/e"),
                ("RestartPrototype", "<Keyboard>/f8") })
            {
                var action = gameplay.FindAction(binding.Item1) ?? gameplay.AddAction(binding.Item1, InputActionType.Button, binding.Item2);
                action.wantsInitialStateCheck = true;
            }
        }

        [MenuItem("Just a few peppers/Apply 1_02 handling to foundation")]
        public static void Apply()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            if (session.handling != null) throw new InvalidOperationException("Handling is already authored. Edit the saved scene; do not recreate it.");
            var input = InputActionAsset.FromJson(File.ReadAllText(InputPath));
            AddHandlingActions(input.FindActionMap("Gameplay", true));
            File.WriteAllText(InputPath, input.ToJson());
            UnityEngine.Object.DestroyImmediate(input);
            AssetDatabase.ImportAsset(InputPath);

            var handling = session.gameObject.AddComponent<YardHandling>();
            session.handling = handling;
            var red = AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Mound.mat");
            var orange = Material("Ripe pepper", new Color(.83f, .22f, .055f));
            var green = Material("Pepper stem", new Color(.20f, .31f, .09f));
            var mat = Material("Crate mat", new Color(.74f, .65f, .35f));
            var highlight = Material("Scoop focus", new Color(1, .83f, .35f));
            var mound = GameObject.Find("Pepper mound");
            UnityEngine.Object.DestroyImmediate(mound.transform.Find("Mound placeholder").gameObject);
            // The old front marker must not intercept a low scoop at the pile edge.
            UnityEngine.Object.DestroyImmediate(mound.transform.Find("Mound marker").GetComponent<Collider>());
            handling.regions = new PileRegion[9];
            for (int z = 0; z < 3; z++)
            for (int x = 0; x < 3; x++)
            {
                int index = z * 3 + x;
                var region = new GameObject("Pile region " + index).AddComponent<PileRegion>();
                region.transform.SetParent(mound.transform, false);
                region.transform.localPosition = new Vector3((x - 1) * 1.25f, 0, (z - 1) * .95f);
                region.regionId = "gate-mound-" + index;
                region.initialUnits = index == 0 ? 3 : 13;
                float height = index == 0 ? .25f : index == 4 ? 1.08f : .65f;
                region.fullScale = new Vector3(1.4f, height, 1.15f);
                Box("Cleared region target", new Vector3(0, .012f, 0), new Vector3(1.24f, .024f, .94f),
                    AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Earth.mat"), region.transform).GetComponent<Renderer>().enabled = false;
                region.volume = Shape(PrimitiveType.Sphere, "Local pile volume", region.transform,
                    new Vector3(0, height * .5f + .025f, 0), region.fullScale, red).transform;
                region.surfaceClumps = new GameObject[9];
                for (int p = 0; p < region.surfaceClumps.Length; p++)
                {
                    float px = (p % 3 - 1) * .21f;
                    float pz = (p / 3 - 1) * .22f;
                    float py = Mathf.Sqrt(Mathf.Max(0, .25f - px * px - pz * pz));
                    var pepper = Shape(PrimitiveType.Sphere, "Surface pepper", region.volume, new Vector3(px, py, pz),
                        new Vector3(.14f, .11f, .3f), p % 3 == 0 ? orange : red, false);
                    pepper.transform.localRotation = Quaternion.Euler(0, p * 57, p * 11);
                    region.surfaceClumps[p] = pepper;
                }
                region.focusMarker = Box("Scoop region edge", new Vector3(0, .045f, -.47f), new Vector3(.85f, .045f, .035f), highlight, region.transform, false).GetComponent<Renderer>();
                region.focusMarker.enabled = false;
                handling.regions[index] = region;
            }
            mound.GetComponent<YardTarget>().description = "Scoop across the local clumps";

            var crate = GameObject.Find("Crate").AddComponent<RawCarrierView>();
            handling.crate = crate;
            crate.target = crate.GetComponent<YardTarget>();
            crate.label = crate.transform.Find("02  CRATE").gameObject;
            crate.parkedColliders = crate.GetComponentsInChildren<Collider>();
            crate.carryAnchor = new GameObject("Crate carry pose").transform;
            crate.carryAnchor.SetParent(session.player.view.transform, false);
            crate.carryAnchor.localPosition = new Vector3(0, -.88f, 1f);
            crate.restingPoints = new Transform[2];
            for (int i = 0; i < 2; i++)
            {
                var point = new GameObject(i == 0 ? "Gate crate mat" : "Mound crate mat").transform;
                point.position = i == 0 ? crate.transform.position : new Vector3(-5.8f, 0, -.5f);
                crate.restingPoints[i] = point;
                Box("Crate parking mat", new Vector3(0, .006f, 0), new Vector3(1.3f, .012f, 1.05f), mat, point, false);
                Label("CRATE", point, new Vector3(0, .025f, -.55f), .01f);
            }
            crate.contents = new GameObject[12];
            for (int i = 0; i < 12; i++)
            {
                var group = new GameObject("Carried unit " + (i + 1));
                group.transform.SetParent(crate.transform, false);
                group.transform.localPosition = new Vector3((i % 3 - 1) * .24f, .24f + i / 6 * .16f, (i / 3 % 2 == 0 ? -.14f : .14f));
                Shape(PrimitiveType.Sphere, "Pepper", group.transform, Vector3.zero, new Vector3(.18f, .14f, .27f), i % 4 == 0 ? orange : red, false);
                Shape(PrimitiveType.Cube, "Stem", group.transform, new Vector3(0, .03f, .14f), new Vector3(.035f, .04f, .08f), green, false);
                group.SetActive(false);
                crate.contents[i] = group;
            }
            crate.contentDestination = new GameObject("Scoop destination").transform;
            crate.contentDestination.SetParent(crate.transform, false);
            crate.contentDestination.localPosition = new Vector3(0, .42f, 0);
            var presentation = new GameObject("Scoop feedback").AddComponent<ScoopPresentation>();
            handling.presentation = presentation;
            presentation.flyingClumps = new Transform[3];
            for (int i = 0; i < 3; i++)
            {
                var clump = Shape(PrimitiveType.Sphere, "Scoop pepper proxy", presentation.transform, Vector3.zero,
                    new Vector3(.14f, .12f, .23f), i == 0 ? orange : red, false);
                presentation.flyingClumps[i] = clump.transform;
                clump.SetActive(false);
            }
            presentation.actionAudio = presentation.gameObject.AddComponent<AudioSource>();
            presentation.feedbackAudio = presentation.gameObject.AddComponent<AudioSource>();
            presentation.actionAudio.playOnAwake = presentation.feedbackAudio.playOnAwake = false;
            presentation.scoopClip = Clip("impactSoft_medium_000.ogg");
            presentation.crateClip = Clip("impactPlank_medium_000.ogg");
            presentation.softCue = Clip("impactSoft_heavy_000.ogg");

            var hud = session.hud;
            hud.transform.Find("Scope").GetComponent<Text>().text = "Scoop & carry  /  Processing comes next";
            handling.statusText = Text("Handling status", hud.transform, "", new Vector2(0, 330), new Vector2(1300, 40), 22);
            var help = hud.transform.Find("Controls").GetComponent<Text>();
            help.text = HandlingHelp;
            help.fontSize = 19;
            help.rectTransform.anchoredPosition = new Vector2(0, -390);
            help.rectTransform.sizeDelta = new Vector2(1380, 98);
            AddControlsBacking(help);
            hud.noticeText.rectTransform.anchoredPosition = new Vector2(0, -300);
            hud.noticeText.rectTransform.sizeDelta = new Vector2(1320, 45);
            hud.pausePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(760, 510);
            hud.pauseTitle.rectTransform.anchoredPosition = new Vector2(0, 200);
            var pauseHelp = hud.pausePanel.transform.Find("Pause help").GetComponent<Text>();
            pauseHelp.text = "Pick up the crate, then hold left mouse at the mound.\nR keeps your load. Restart restores the test mound.";
            pauseHelp.rectTransform.anchoredPosition = new Vector2(0, 137);
            pauseHelp.rectTransform.sizeDelta = new Vector2(730, 65);
            hud.resumeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 58);
            hud.resetButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10);
            hud.resetButton.GetComponentInChildren<Text>().text = "Return to gate + recover crate";
            hud.quitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -146);
            hud.restartPrototypeButton = Button("Restart scoop test (clears load)", hud.pausePanel.transform, -78);
            var buttons = new[] { hud.resumeButton, hud.resetButton, hud.restartPrototypeButton, hud.quitButton };
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].navigation = new Navigation { mode = Navigation.Mode.Explicit,
                    selectOnUp = buttons[(i + 3) % 4], selectOnDown = buttons[(i + 1) % 4] };
            // Leave the other targets as explicit placeholders for their own tasks.
            GameObject.Find("Automatic processor").GetComponent<YardTarget>().description = "Processing comes next - F8 restarts this scoop test";
            var rack = GameObject.Find("Storage rack").GetComponent<YardTarget>();
            rack.displayName = "Finished Food Handoff Rack";
            rack.description = "Finished food handling comes later";
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("HANDLING_SCENE_AUTHORED " + ScenePath);
        }

        static AudioClip Clip(string file)
        {
            var importer = (AudioImporter)AssetImporter.GetAtPath(AudioPath + file);
            importer.forceToMono = true;
            var settings = importer.defaultSampleSettings;
            settings.preloadAudioData = true;
            settings.loadType = AudioClipLoadType.DecompressOnLoad;
            importer.defaultSampleSettings = settings;
            importer.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<AudioClip>(AudioPath + file);
        }

        [MenuItem("Just a few peppers/Apply handling visibility tuning")]
        public static void ApplyVisibilityTuning()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            var anchor = session.handling.crate.carryAnchor;
            anchor.SetParent(session.player.view.transform, false);
            anchor.localPosition = new Vector3(0, -.88f, 1f);
            anchor.localRotation = Quaternion.identity;
            foreach (var region in session.handling.regions) region.Render(region.initialUnits);
            AddControlsBacking(session.hud.transform.Find("Controls").GetComponent<Text>());
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("HANDLING_VISIBILITY_UPDATED");
        }

        [MenuItem("Just a few peppers/Apply hold-only scooping")]
        public static void ApplyHoldOnlyScooping()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var input = InputActionAsset.FromJson(File.ReadAllText(InputPath));
            input.FindAction("Gameplay/ScoopMode")?.RemoveAction();
            File.WriteAllText(InputPath, input.ToJson());
            UnityEngine.Object.DestroyImmediate(input);
            AssetDatabase.ImportAsset(InputPath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            session.hud.transform.Find("Controls").GetComponent<Text>().text = HandlingHelp;
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("HOLD_ONLY_SCOOPING_AUTHORED");
        }

        static void AddControlsBacking(Text controls)
        {
            if (controls.transform.parent.Find("Controls backing") != null) return;
            var backing = new GameObject("Controls backing", typeof(RectTransform), typeof(Image));
            backing.transform.SetParent(controls.transform.parent, false);
            backing.transform.SetSiblingIndex(controls.transform.GetSiblingIndex());
            var rect = backing.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(.5f, .5f);
            rect.anchoredPosition = controls.rectTransform.anchoredPosition;
            rect.sizeDelta = new Vector2(1420, 110);
            backing.GetComponent<Image>().color = new Color(.055f, .1f, .085f, .90f);
            backing.GetComponent<Image>().raycastTarget = false;
        }
    }
}
