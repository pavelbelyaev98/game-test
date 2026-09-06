using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace JustAFewPeppers.Editor
{
    public static class FoundationSceneBuilder
    {
        public const string ScenePath = "Assets/JustAFewPeppers/Scenes/PepperYard.unity";
        public const string InputPath = "Assets/JustAFewPeppers/Content/YardControls.inputactions";
        public const string BuildPath = "Builds/JustAFewPeppers/JustAFewPeppers.exe";
        public const string DevelopmentBuildPath = "Builds/JustAFewPeppers-Development/JustAFewPeppers.exe";
        const string Content = "Assets/JustAFewPeppers/Content/";
        const string MovementHelp = "WASD / Arrows  Move     Shift  Sprint     Space  Jump\nMouse  Look     Esc  Pause     R  Return to gate";

        [MenuItem("Just a few peppers/Create foundation if missing")]
        public static void CreateScene()
        {
            // Subsequent tasks edit the saved scene; this command must never regenerate their work.
            if (File.Exists(ScenePath))
                throw new InvalidOperationException("PepperYard already exists. Open and edit the saved scene; generation will not overwrite it.");
            Directory.CreateDirectory(Content);
            Directory.CreateDirectory("Assets/JustAFewPeppers/Scenes");
            AssetDatabase.Refresh();
            CreateInput();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var ground = Material("Earth", new Color(.36f, .39f, .25f));
            var wall = Material("Plaster", new Color(.74f, .68f, .51f));
            var stone = Material("Path", new Color(.61f, .59f, .48f));
            var wood = Material("Wood", new Color(.35f, .22f, .12f));
            var paleWood = Material("Rack", new Color(.66f, .49f, .27f));
            var red = Material("Mound", new Color(.65f, .13f, .065f));
            var orange = Material("Crate", new Color(.88f, .45f, .13f));
            var enamel = Material("Station", new Color(.13f, .35f, .31f));
            var metal = Material("Metal", new Color(.35f, .41f, .4f));
            var marker = Material("Target marker", new Color(.38f, .45f, .4f));

            Box("Yard ground", new Vector3(0, -.15f, 0), new Vector3(18, .3f, 18), ground);
            Box("West wall", new Vector3(-9, 1.1f, 0), new Vector3(.3f, 2.2f, 18), wall);
            Box("East wall", new Vector3(9, 1.1f, 0), new Vector3(.3f, 2.2f, 18), wall);
            Box("North wall", new Vector3(0, 1.1f, 9), new Vector3(18, 2.2f, .3f), wall);
            Box("Gate boundary", new Vector3(0, 1.1f, -9), new Vector3(18, 2.2f, .3f), wall);
            ExtendYardBoundaries();
            Box("Wooden gate", new Vector3(0, 1.05f, -8.8f), new Vector3(2.3f, 2.1f, .15f), wood);
            for (int z = -7; z <= 6; z++)
                Box("Walking route", new Vector3(0, .015f, z), new Vector3(1.8f, .03f, .92f), stone, null, false);
            for (int x = 1; x <= 4; x++)
                Box("Station path", new Vector3(x, .015f, -1.3f), new Vector3(.92f, .03f, 1.6f), stone, null, false);
            for (int z = 0; z <= 6; z++)
                Box("Rack path", new Vector3(5.3f, .015f, z), new Vector3(1.6f, .03f, .92f), stone, null, false);

            var mound = Target("Pepper mound", "A broad gathering area", new Vector3(-3, 0, 0));
            Shape(PrimitiveType.Sphere, "Mound placeholder", mound.transform, new Vector3(0, .25f, 0), new Vector3(4, 1.6f, 3), red);
            mound.marker = Box("Mound marker", new Vector3(0, .12f, -1.6f), new Vector3(2.1f, .24f, .12f), marker, mound.transform).GetComponent<Renderer>();
            Label("01  PEPPERS", mound.transform, new Vector3(0, 1.5f, .4f));

            var crate = Target("Crate", "Your first carrier", new Vector3(-1.25f, 0, -2.7f));
            Box("Crate base", new Vector3(0, .13f, 0), new Vector3(.9f, .18f, .65f), orange, crate.transform);
            foreach (float x in new[] { -.42f, .42f })
                Box("Crate side", new Vector3(x, .35f, 0), new Vector3(.07f, .4f, .65f), orange, crate.transform);
            foreach (float z in new[] { -.29f, .29f })
                Box("Crate end", new Vector3(0, .35f, z), new Vector3(.9f, .4f, .07f), orange, crate.transform);
            crate.marker = Box("Crate marker", new Vector3(0, .36f, -.34f), new Vector3(.45f, .16f, .025f), marker, crate.transform, false).GetComponent<Renderer>();
            Label("02  CRATE", crate.transform, new Vector3(0, .9f, 0), .015f);

            var station = Target("Automatic processor", "The future tipping point", new Vector3(3, 0, 1));
            Box("Worktop", new Vector3(0, .9f, 0), new Vector3(2.5f, .16f, 1.4f), wood, station.transform);
            foreach (float x in new[] { -1.05f, 1.05f })
            foreach (float z in new[] { -.5f, .5f })
                Box("Worktop leg", new Vector3(x, .42f, z), new Vector3(.13f, .84f, .13f), wood, station.transform);
            Shape(PrimitiveType.Cylinder, "Appliance placeholder", station.transform, new Vector3(-.35f, 1.25f, 0), new Vector3(.78f, .3f, .78f), enamel);
            Shape(PrimitiveType.Cylinder, "Feed rim", station.transform, new Vector3(-.35f, 1.57f, 0), new Vector3(.94f, .035f, .94f), metal);
            Box("Output space", new Vector3(.65f, 1.04f, 0), new Vector3(.65f, .12f, .9f), metal, station.transform);
            station.marker = Box("Station marker", new Vector3(0, .85f, -.73f), new Vector3(1.5f, .2f, .035f), marker, station.transform, false).GetComponent<Renderer>();
            Label("03  PROCESS", station.transform, new Vector3(0, 2.05f, .25f));

            var rack = Target("Storage rack", "Finished food will come here", new Vector3(3, 0, 5.4f));
            foreach (float x in new[] { -1.05f, 1.05f })
            foreach (float z in new[] { -.42f, .42f })
                Box("Rack post", new Vector3(x, 1, z), new Vector3(.12f, 2, .12f), paleWood, rack.transform);
            foreach (float y in new[] { .25f, .95f, 1.65f })
                Box("Shelf", new Vector3(0, y, 0), new Vector3(2.2f, .1f, .96f), paleWood, rack.transform);
            Box("Rack back", new Vector3(0, 1, .46f), new Vector3(2.2f, 1.8f, .08f), wood, rack.transform);
            rack.marker = Box("Rack marker", new Vector3(0, 1.65f, -.51f), new Vector3(1.6f, .18f, .03f), marker, rack.transform, false).GetComponent<Renderer>();
            Label("04  STORE", rack.transform, new Vector3(0, 2.3f, 0));

            var session = new GameObject("Yard session").AddComponent<YardSession>();
            session.inputActions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputPath);
            session.safeSpawn = new GameObject("Safe spawn at gate").transform;
            session.safeSpawn.position = new Vector3(0, .04f, -6);
            var playerObject = new GameObject("Player");
            playerObject.layer = 2; // Ignore Raycast: never target the controller from its own camera.
            session.player = playerObject.AddComponent<YardPlayer>();
            session.player.body = playerObject.AddComponent<CharacterController>();
            session.player.body.height = 1.8f;
            session.player.body.center = new Vector3(0, .9f, 0);
            session.player.body.radius = .3f;
            session.player.body.stepOffset = .25f;
            session.player.body.skinWidth = .03f;
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(playerObject.transform, false);
            cameraObject.transform.localPosition = new Vector3(0, 1.65f, 0);
            var camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            camera.fieldOfView = 72;
            camera.nearClipPlane = .05f;
            camera.farClipPlane = 80;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(.53f, .72f, .79f);
            session.player.view = camera;
            session.player.ResetTo(session.safeSpawn);
            session.targeting = playerObject.AddComponent<YardTargeting>();
            session.targeting.view = camera;
            CreateHud(session);

            var sun = new GameObject("Afternoon sun").AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.transform.rotation = Quaternion.Euler(48, -35, 0);
            sun.color = new Color(1, .91f, .74f);
            sun.intensity = 1.25f;
            sun.shadows = LightShadows.Soft;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(.57f, .63f, .66f);
            RenderSettings.skybox = null;
            QualitySettings.vSyncCount = 1;
            QualitySettings.antiAliasing = 4;
            EditorSceneManager.SaveScene(scene, ScenePath);
            ConfigureBuild();
            AssetDatabase.SaveAssets();
            Debug.Log("FOUNDATION_SCENE_CREATED " + ScenePath);
        }

        static void CreateInput()
        {
            if (File.Exists(InputPath)) return;
            var asset = ScriptableObject.CreateInstance<InputActionAsset>();
            asset.name = "YardControls";
            var gameplay = asset.AddActionMap("Gameplay");
            var move = gameplay.AddAction("Move", InputActionType.Value, expectedControlLayout: "Vector2");
            Directions(move, "w", "s", "a", "d");
            Directions(move, "upArrow", "downArrow", "leftArrow", "rightArrow");
            gameplay.AddAction("Look", InputActionType.Value, "<Mouse>/delta", expectedControlLayout: "Vector2");
            gameplay.AddAction("Reset", InputActionType.Button, "<Keyboard>/r");
            AddMovementActions(gameplay);
            HandlingSceneAuthoring.AddHandlingActions(gameplay);
            asset.AddActionMap("System").AddAction("Pause", InputActionType.Button, "<Keyboard>/escape");
            var ui = asset.AddActionMap("UI");
            var navigate = ui.AddAction("Navigate", InputActionType.PassThrough, expectedControlLayout: "Vector2");
            Directions(navigate, "w", "s", "a", "d");
            Directions(navigate, "upArrow", "downArrow", "leftArrow", "rightArrow");
            ui.AddAction("Submit", InputActionType.Button, "<Keyboard>/enter").AddBinding("<Keyboard>/space");
            ui.AddAction("Cancel", InputActionType.Button, "<Keyboard>/escape");
            ui.AddAction("Point", InputActionType.PassThrough, "<Mouse>/position", expectedControlLayout: "Vector2");
            ui.AddAction("Click", InputActionType.PassThrough, "<Mouse>/leftButton", expectedControlLayout: "Button");
            File.WriteAllText(InputPath, asset.ToJson());
            UnityEngine.Object.DestroyImmediate(asset);
            AssetDatabase.ImportAsset(InputPath);
        }

        static void Directions(InputAction action, string up, string down, string left, string right)
        {
            action.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/" + up).With("Down", "<Keyboard>/" + down)
                .With("Left", "<Keyboard>/" + left).With("Right", "<Keyboard>/" + right);
        }

        static void AddMovementActions(InputActionMap gameplay)
        {
            var sprint = gameplay.FindAction("Sprint") ?? gameplay.AddAction("Sprint", InputActionType.Button);
            if (sprint.bindings.Count == 0)
            {
                sprint.AddBinding("<Keyboard>/leftShift");
                sprint.AddBinding("<Keyboard>/rightShift");
            }
            sprint.wantsInitialStateCheck = true;
            var jump = gameplay.FindAction("Jump") ?? gameplay.AddAction("Jump", InputActionType.Button, "<Keyboard>/space");
            // Session explicitly waits for release after menu activation, including already held controls.
            jump.wantsInitialStateCheck = true;
        }

        [MenuItem("Just a few peppers/Apply foundation movement update")]
        public static void ApplyMovementUpdate()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var scene = EditorSceneManager.OpenScene(ScenePath);
            // Clone/serialize through Unity's API to retain existing action IDs and the asset's .meta GUID.
            var asset = InputActionAsset.FromJson(File.ReadAllText(InputPath));
            AddMovementActions(asset.FindActionMap("Gameplay", true));
            File.WriteAllText(InputPath, asset.ToJson());
            UnityEngine.Object.DestroyImmediate(asset);
            AssetDatabase.ImportAsset(InputPath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            var help = session.hud.transform.Find("Controls").GetComponent<Text>();
            help.text = MovementHelp;
            help.rectTransform.sizeDelta = new Vector2(1250, 64);
            // Non-uniform primitive Sphere/Capsule colliders do not follow the visible flattened mesh.
            foreach (var mesh in UnityEngine.Object.FindObjectsByType<MeshFilter>())
            {
                var collider = mesh.GetComponent<Collider>();
                if (collider is SphereCollider || collider is CapsuleCollider) UseMeshCollision(mesh.gameObject);
            }
            ExtendYardBoundaries();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("FOUNDATION_MOVEMENT_UPDATED " + ScenePath);
        }

        static void ExtendYardBoundaries()
        {
            // Keep jumping from props inside the compact yard. Visual walls retain their authored height.
            foreach (var name in new[] { "West wall", "East wall", "North wall", "Gate boundary" })
            {
                var wall = GameObject.Find(name);
                var collider = wall.GetComponent<BoxCollider>();
                collider.center = new Vector3(0, (2 - wall.transform.position.y) / wall.transform.localScale.y, 0);
                collider.size = new Vector3(1, 4 / wall.transform.localScale.y, 1);
            }
        }

        static void UseMeshCollision(GameObject go)
        {
            UnityEngine.Object.DestroyImmediate(go.GetComponent<Collider>());
            go.AddComponent<MeshCollider>().sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;
        }

        static void CreateHud(YardSession session)
        {
            var eventObject = new GameObject("EventSystem", typeof(EventSystem));
            session.uiInput = eventObject.AddComponent<InputSystemUIInputModule>();
            session.uiInput.enabled = false;
            var canvasObject = new GameObject("Yard HUD", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1440, 900);
            scaler.matchWidthOrHeight = .5f;
            var hud = canvasObject.AddComponent<YardHud>();
            session.hud = hud;
            hud.events = eventObject.GetComponent<EventSystem>();
            var root = canvasObject.transform;
            Text("Title", root, "JUST A FEW PEPPERS", new Vector2(0, 405), new Vector2(650, 45), 28);
            Text("Scope", root, "Walk around the yard  /  Handling comes next", new Vector2(0, 369), new Vector2(800, 32), 19);
            Text("Controls", root, MovementHelp, new Vector2(0, -404), new Vector2(1250, 64), 21);
            hud.reticle = Text("Reticle", root, "+", Vector2.zero, new Vector2(40, 40), 24).gameObject;
            hud.targetText = Text("Target", root, "", new Vector2(0, -100), new Vector2(850, 85), 23);
            hud.noticeText = Text("Notice", root, "", new Vector2(0, -330), new Vector2(850, 40), 22);
            var panel = new GameObject("Pause menu", typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(root, false);
            Rect(panel, Vector2.zero, new Vector2(670, 430));
            panel.GetComponent<Image>().color = new Color(.055f, .10f, .085f, .98f);
            hud.pausePanel = panel;
            hud.pauseTitle = Text("Pause heading", panel.transform, "", new Vector2(0, 158), new Vector2(620, 55), 28);
            Text("Pause help", panel.transform, "Enter or click to walk.  Esc pauses.\nReturning to this window keeps the yard paused.", new Vector2(0, 90), new Vector2(620, 60), 20);
            hud.resumeButton = Button("Walk / Resume", panel.transform, 13);
            hud.resetButton = Button("Return to gate", panel.transform, -55);
            hud.quitButton = Button("Quit", panel.transform, -123);
            var buttons = new[] { hud.resumeButton, hud.resetButton, hud.quitButton };
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].navigation = new Navigation { mode = Navigation.Mode.Explicit,
                    selectOnUp = buttons[(i + 2) % 3], selectOnDown = buttons[(i + 1) % 3] };
        }

        internal static Button Button(string label, Transform parent, float y)
        {
            var go = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            Rect(go, new Vector2(0, y), new Vector2(450, 54));
            go.GetComponent<Image>().color = new Color(.28f, .38f, .3f);
            var button = go.GetComponent<Button>();
            var colors = button.colors;
            colors.highlightedColor = new Color(1f, .9f, .58f);
            colors.selectedColor = new Color(1f, .9f, .58f);
            button.colors = colors;
            Text(label + " text", go.transform, label, Vector2.zero, new Vector2(440, 52), 23);
            return button;
        }

        internal static Text Text(string name, Transform parent, string value, Vector2 position, Vector2 size, int fontSize)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            Rect(go, position, size);
            var text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = value;
            text.fontSize = fontSize;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(1f, .97f, .85f);
            text.raycastTarget = false;
            go.AddComponent<Shadow>().effectDistance = new Vector2(1, -1);
            return text;
        }

        static void Rect(GameObject go, Vector2 position, Vector2 size)
        {
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(.5f, .5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        static YardTarget Target(string name, string description, Vector3 position)
        {
            var target = new GameObject(name).AddComponent<YardTarget>();
            target.transform.position = position;
            target.displayName = name;
            target.description = description;
            return target;
        }

        internal static Material Material(string name, Color color)
        {
            var path = Content + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null) return material;
            material = new Material(Shader.Find("Standard")) { color = color };
            material.SetFloat("_Glossiness", .15f);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        internal static GameObject Box(string name, Vector3 position, Vector3 scale, Material material, Transform parent = null, bool collision = true)
            => Shape(PrimitiveType.Cube, name, parent, position, scale, material, collision);

        internal static GameObject Shape(PrimitiveType type, string name, Transform parent, Vector3 position, Vector3 scale, Material material, bool collision = true)
        {
            var go = GameObject.CreatePrimitive(type);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = position;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = material;
            if (!collision) UnityEngine.Object.DestroyImmediate(go.GetComponent<Collider>());
            else if (type == PrimitiveType.Sphere || type == PrimitiveType.Cylinder) UseMeshCollision(go);
            return go;
        }

        internal static void Label(string value, Transform parent, Vector3 position, float size = .024f)
        {
            var go = new GameObject(value);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = position;
            var text = go.AddComponent<TextMesh>();
            text.text = value;
            text.fontSize = 64;
            text.characterSize = size;
            text.anchor = TextAnchor.MiddleCenter;
            text.color = new Color(.12f, .16f, .11f);
        }

        [MenuItem("Just a few peppers/Configure foundation build")]
        public static void ConfigureBuild()
        {
            if (!File.Exists(ScenePath)) throw new FileNotFoundException(ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            PlayerSettings.companyName = "Interaction Spikes";
            PlayerSettings.productName = "Just a few peppers";
            PlayerSettings.defaultScreenWidth = 1440;
            PlayerSettings.defaultScreenHeight = 900;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            PlayerSettings.runInBackground = false;
            PlayerSettings.colorSpace = ColorSpace.Linear;
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
        }

        [MenuItem("Just a few peppers/Build Windows playtest")]
        public static void BuildWindows()
            => BuildWindowsPlayer(BuildPath, BuildOptions.None);

        [MenuItem("Just a few peppers/Build Windows development diagnostics")]
        public static void BuildWindowsDevelopment()
            => BuildWindowsPlayer(DevelopmentBuildPath, BuildOptions.Development);

        static void BuildWindowsPlayer(string path, BuildOptions options)
        {
            ConfigureBuild();
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
                scenes = new[] { ScenePath }, locationPathName = path,
                target = BuildTarget.StandaloneWindows64, options = options });
            if (report.summary.result != BuildResult.Succeeded)
                throw new InvalidOperationException("Foundation build failed: " + report.summary.result);
            var kind = (options & BuildOptions.Development) != 0 ? "Development" : "Playtest";
            Debug.Log("FOUNDATION_BUILD_SUCCEEDED " + kind + " " + path + " " + report.summary.totalSize + " bytes");
        }
    }
}
