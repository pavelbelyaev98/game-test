using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Chushkopek.Stage0.Editor
{
    public static class Stage0SceneBuilder
    {
        public const string ScenePath = "Assets/Stage0/Scenes/ChushkopekStage0.unity";
        const string Art = "Assets/Stage0/Art/Generated/";

        [MenuItem("Stage 0/Create or rebuild the one-pepper scene")]
        public static void CreateScene()
        {
            Directory.CreateDirectory(Art);
            Directory.CreateDirectory("Assets/Stage0/Scenes");
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var wood = Material("Table", new Color(.29f, .16f, .078f), .15f);
            var dark = Material("Enamel", new Color(.075f, .13f, .12f), .4f);
            var metal = Material("Brushed metal", new Color(.47f, .51f, .48f), .65f, .7f);
            var black = Material("Roaster interior", new Color(.018f, .015f, .013f), .05f);
            var ceramic = Material("Cream ceramic", new Color(.83f, .79f, .65f), .55f);
            var green = Material("Pepper stem", new Color(.12f, .24f, .025f), .35f);
            var floor = Material("Floor", new Color(.19f, .22f, .2f), .05f);
            var wall = Material("Backdrop", new Color(.27f, .32f, .28f), .05f);
            var edge = Material("Loose skin edge", new Color(.3f, .11f, .025f), .25f);
            var lamp = Material("Indicator", new Color(.12f, .12f, .1f), .3f);
            lamp.EnableKeyword("_EMISSION");

            var session = new GameObject("One pepper interaction").AddComponent<Stage0Session>();
            session.sound = session.gameObject.AddComponent<Stage0Audio>();
            Cube("Floor", new Vector3(0, -.08f, 0), new Vector3(5, .16f, 4), floor);
            Cube("Backdrop", new Vector3(0, 1.5f, 1.55f), new Vector3(5, 3, .15f), wall);
            Cube("Left boundary", new Vector3(-2.45f, .6f, 0), new Vector3(.12f, 1.2f, 4), wall);
            Cube("Right boundary", new Vector3(2.45f, .6f, 0), new Vector3(.12f, 1.2f, 4), wall);
            Cube("Rear boundary", new Vector3(0, .6f, -1.95f), new Vector3(5, 1.2f, .12f), wall);
            Cube("Work table", new Vector3(0, .89f, .15f), new Vector3(3.1f, .12f, 1.12f), wood);
            foreach (float x in new[] { -1.35f, 1.35f })
            foreach (float z in new[] { -.25f, .54f }) Cube("Table leg", new Vector3(x, .43f, z), new Vector3(.09f, .86f, .09f), dark);

            var player = new GameObject("First person controller");
            player.transform.position = new Vector3(0, 0, -1.38f);
            var controller = player.AddComponent<CharacterController>();
            controller.height = 1.65f; controller.center = new Vector3(0, .825f, 0); controller.radius = .19f;
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(player.transform, false);
            cameraObject.transform.localPosition = new Vector3(0, 1.7f, 0);
            cameraObject.transform.localRotation = Quaternion.Euler(25, 0, 0);
            var camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 58; camera.nearClipPlane = .05f; camera.farClipPlane = 25;
            camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = new Color(.12f, .18f, .16f);
            cameraObject.AddComponent<AudioListener>();
            session.interaction = player.AddComponent<Stage0Interaction>();
            session.interaction.session = session; session.interaction.view = camera;
            session.carrySocket = Socket("Held pepper", cameraObject.transform, new Vector3(.36f, -.32f, .7f), Quaternion.Euler(0, 0, -20));
            session.startSocket = Socket("Starting pepper position", null, new Vector3(-1.15f, 1.06f, -.18f), Quaternion.Euler(90, 0, 0));
            var start = Cube("Starting board", new Vector3(-1.15f, .968f, .08f), new Vector3(.46f, .035f, .68f), ceramic);
            Target(start, PepperLocation.Start, "return to the starting position");

            var roasterRoot = new GameObject("Chushkopek - ONE socket");
            var roaster = roasterRoot.AddComponent<RoasterStation>();
            session.roaster = roaster; roaster.session = session;
            Target(roasterRoot, PepperLocation.Roaster, "place in the chushkopek");
            Shape(PrimitiveType.Cylinder, "Enamel body", new Vector3(-.62f, 1.13f, .17f), new Vector3(.44f, .18f, .44f), dark, roasterRoot.transform);
            Shape(PrimitiveType.Cylinder, "Metal rim", new Vector3(-.62f, 1.313f, .17f), new Vector3(.45f, .018f, .45f), metal, roasterRoot.transform);
            Shape(PrimitiveType.Cylinder, "Single dark socket", new Vector3(-.62f, 1.334f, .17f), new Vector3(.29f, .008f, .29f), black, roasterRoot.transform);
            roaster.socket = Socket("Roasting position", roasterRoot.transform, new Vector3(-.62f, 1.13f, .17f), Quaternion.identity);
            var roasterVolume = roasterRoot.AddComponent<BoxCollider>();
            roasterVolume.center = new Vector3(-.62f, 1.23f, .17f); roasterVolume.size = new Vector3(.52f, .25f, .52f); roasterVolume.isTrigger = true;
            roaster.indicator = Shape(PrimitiveType.Sphere, "Readiness lamp", new Vector3(-.62f, 1.095f, -.053f), Vector3.one * .06f, lamp, roasterRoot.transform).GetComponent<Renderer>();
            roaster.smoke = Particles("Burn smoke", new Vector3(-.62f, 1.44f, .17f), new Color(.35f, .32f, .29f, .2f), roasterRoot.transform);

            var steamRoot = new GameObject("Steam and peel - ONE position");
            var steam = steamRoot.AddComponent<SteamingStation>();
            session.steaming = steam; steam.session = session;
            Target(steamRoot, PepperLocation.Steam, "place under the steaming cover");
            Shape(PrimitiveType.Cylinder, "Shallow steaming bowl", new Vector3(.2f, .982f, .06f), new Vector3(.59f, .022f, .72f), ceramic, steamRoot.transform);
            steam.socket = Socket("Steam and peel position", steamRoot.transform, new Vector3(.2f, 1.12f, -.22f), Quaternion.Euler(90, 0, 0));
            var lid = Shape(PrimitiveType.Sphere, "Steam cover", new Vector3(.2f, 1.57f, .22f), new Vector3(.59f, .07f, .7f), metal, steamRoot.transform, false);
            steam.cover = lid.transform;
            Shape(PrimitiveType.Sphere, "Cover handle", new Vector3(.2f, 1.615f, .22f), new Vector3(.07f, .045f, .07f), dark, lid.transform, false);
            // Cover closes above the pepper, then parks at the back to expose the peel surface.
            steam.steam = Particles("Steam", new Vector3(.2f, 1.23f, .07f), new Color(.94f, .94f, .88f, .36f), steamRoot.transform);
            var steamVolume = steamRoot.AddComponent<BoxCollider>();
            steamVolume.center = new Vector3(.2f, 1.005f, .06f); steamVolume.size = new Vector3(.66f, .08f, .78f); steamVolume.isTrigger = true;

            var trayRoot = new GameObject("Finished tray - ONE position");
            var tray = trayRoot.AddComponent<FinishedSocket>(); session.tray = tray;
            Target(trayRoot, PepperLocation.Finished, "place on the finished tray");
            Cube("Tray base", new Vector3(1.03f, .984f, .07f), new Vector3(.6f, .04f, .73f), metal, trayRoot.transform);
            foreach (float x in new[] { .74f, 1.32f }) Cube("Tray edge", new Vector3(x, 1.012f, .07f), new Vector3(.024f, .055f, .73f), metal, trayRoot.transform);
            foreach (float z in new[] { -.28f, .42f }) Cube("Tray edge", new Vector3(1.03f, 1.012f, z), new Vector3(.6f, .055f, .024f), metal, trayRoot.transform);
            tray.socket = Socket("Finished pepper position", trayRoot.transform, new Vector3(1.03f, 1.13f, -.22f), Quaternion.Euler(90, 0, 0));

            var pepperRoot = new GameObject("Pepper - only one");
            pepperRoot.transform.SetPositionAndRotation(session.startSocket.position, session.startSocket.rotation);
            var presentation = pepperRoot.AddComponent<PepperPresentation>(); session.pepper = presentation; presentation.session = session;
            Target(pepperRoot, PepperLocation.Start, "pepper").isPepper = true;
            var pick = pepperRoot.AddComponent<BoxCollider>(); pick.center = new Vector3(0, .28f, 0); pick.size = new Vector3(.35f, .66f, .32f); pick.isTrigger = true; presentation.pickVolume = pick;
            var fleshMaterial = new Material(Shader.Find("Stage0/Pepper")); fleshMaterial.SetFloat("_Flesh", 1);
            fleshMaterial = Asset(fleshMaterial, "Flesh.mat");
            var skinMaterial = Asset(new Material(Shader.Find("Stage0/Pepper")), "Skin.mat");
            presentation.flesh = MeshObject("Prepared flesh", pepperRoot.transform, Asset(PepperGeometry.Create("Pepper flesh"), "Pepper-flesh.asset"), fleshMaterial).GetComponent<Renderer>();
            for (int i = 0; i < 2; i++)
            {
                var strip = MeshObject("Broad skin strip " + (i + 1), pepperRoot.transform, Asset(PepperGeometry.Create("Skin " + i, i), "Pepper-skin-" + i + ".asset"), skinMaterial);
                presentation.skin[i] = strip.GetComponent<MeshFilter>(); presentation.skinRenderers[i] = strip.GetComponent<Renderer>();
                var loose = Shape(PrimitiveType.Cube, "Loose edge " + (i + 1), Vector3.zero, new Vector3(.055f, .036f, .008f), edge, null, false);
                loose.transform.SetParent(pepperRoot.transform, false); loose.transform.localPosition = new Vector3(i == 0 ? .07f : -.07f, .51f, -.075f); presentation.looseEdges[i] = loose.transform;
            }
            var stem = Shape(PrimitiveType.Cylinder, "Stem", Vector3.zero, new Vector3(.035f, .055f, .035f), green, null, false);
            stem.transform.SetParent(pepperRoot.transform, false); stem.transform.localPosition = new Vector3(0, .575f, 0); stem.transform.localRotation = Quaternion.Euler(0, 0, -14);

            Label("RAW", new Vector3(-1.15f, .9f, -.425f));
            Label("ROAST", new Vector3(-.62f, .9f, -.425f));
            Label("STEAM / PEEL", new Vector3(.2f, .9f, -.425f));
            Label("FINISHED", new Vector3(1.03f, .9f, -.425f));
            var key = new GameObject("Warm key light").AddComponent<Light>(); key.type = LightType.Directional; key.color = new Color(1f, .91f, .76f); key.intensity = 1.35f; key.shadows = LightShadows.Soft; key.transform.rotation = Quaternion.Euler(48, -30, 0);
            var fill = new GameObject("Table fill").AddComponent<Light>(); fill.type = LightType.Point; fill.transform.position = new Vector3(.3f, 2.4f, -.9f); fill.range = 5; fill.intensity = 1.5f; fill.color = new Color(.77f, .88f, 1f);
            RenderSettings.ambientMode = AmbientMode.Flat; RenderSettings.ambientLight = new Color(.34f, .38f, .35f); RenderSettings.skybox = null;
            QualitySettings.SetQualityLevel(2); QualitySettings.vSyncCount = 1; QualitySettings.antiAliasing = 4;
            PlayerSettings.companyName = "Interaction Spikes"; PlayerSettings.productName = "Chushkopek - One Pepper";
            PlayerSettings.defaultScreenWidth = 1440; PlayerSettings.defaultScreenHeight = 900;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            PlayerSettings.runInBackground = false;
            PlayerSettings.colorSpace = ColorSpace.Linear;
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            Debug.Log("STAGE0_SCENE_CREATED " + ScenePath);
        }

        [MenuItem("Stage 0/Build Windows player")]
        public static void BuildWindows()
        {
            if (!File.Exists(ScenePath)) CreateScene();
            Directory.CreateDirectory("Builds/Stage0");
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
                scenes = new[] { ScenePath }, locationPathName = "Builds/Stage0/ChushkopekStage0.exe",
                target = BuildTarget.StandaloneWindows64, options = BuildOptions.Development
            });
            if (report.summary.result != BuildResult.Succeeded) throw new System.Exception("Stage 0 build failed: " + report.summary.result);
            Debug.Log("STAGE0_BUILD_SUCCEEDED " + report.summary.totalSize + " bytes");
        }

        static Material Material(string name, Color color, float smoothness, float metallic = 0f)
        {
            var m = new Material(Shader.Find("Standard")) { name = name, color = color };
            m.SetFloat("_Glossiness", smoothness); m.SetFloat("_Metallic", metallic);
            return Asset(m, name.Replace(' ', '-') + ".mat");
        }
        static T Asset<T>(T value, string name) where T : Object
        {
            string path = Art + name;
            var existing = AssetDatabase.LoadAssetAtPath<T>(path);
            if (existing)
            {
                // CopySerialized updates a mesh asset's serialized data without reliably refreshing
                // its live native vertex buffer in this editor session. Update that buffer explicitly.
                if (existing is Mesh destination && value is Mesh source)
                {
                    destination.Clear(); destination.vertices = source.vertices; destination.triangles = source.triangles;
                    destination.normals = source.normals; destination.uv = source.uv;
                    destination.RecalculateBounds(); destination.UploadMeshData(false);
                }
                else EditorUtility.CopySerialized(value, existing);
                Object.DestroyImmediate(value); EditorUtility.SetDirty(existing); return existing;
            }
            AssetDatabase.CreateAsset(value, path); return value;
        }
        static GameObject Shape(PrimitiveType type, string name, Vector3 position, Vector3 scale, Material material, Transform parent = null, bool collider = true)
        {
            var go = GameObject.CreatePrimitive(type); go.name = name; go.transform.SetParent(parent, true);
            go.transform.position = position;
            Vector3 parentScale = parent ? parent.lossyScale : Vector3.one;
            go.transform.localScale = new Vector3(scale.x / parentScale.x, scale.y / parentScale.y, scale.z / parentScale.z);
            go.GetComponent<Renderer>().sharedMaterial = material;
            if (!collider) Object.DestroyImmediate(go.GetComponent<Collider>());
            return go;
        }
        static GameObject Cube(string name, Vector3 position, Vector3 scale, Material material, Transform parent = null) => Shape(PrimitiveType.Cube, name, position, scale, material, parent);
        static Transform Socket(string name, Transform parent, Vector3 position, Quaternion rotation)
        {
            var socket = new GameObject(name).transform; socket.SetParent(parent, false); socket.localPosition = position; socket.localRotation = rotation; return socket;
        }
        static Stage0Target Target(GameObject go, PepperLocation location, string label)
        {
            var target = go.AddComponent<Stage0Target>(); target.location = location; target.label = label; return target;
        }
        static GameObject MeshObject(string name, Transform parent, Mesh mesh, Material material)
        {
            var go = new GameObject(name); go.transform.SetParent(parent, false); go.AddComponent<MeshFilter>().sharedMesh = mesh; go.AddComponent<MeshRenderer>().sharedMaterial = material; return go;
        }
        static void Label(string text, Vector3 position)
        {
            var label = new GameObject(text + " label").AddComponent<TextMesh>(); label.transform.position = position;
            label.text = text; label.fontSize = 48; label.characterSize = .026f; label.anchor = TextAnchor.MiddleCenter; label.color = new Color(.93f, .86f, .65f);
        }
        static ParticleSystem Particles(string name, Vector3 position, Color color, Transform parent)
        {
            var go = new GameObject(name); go.transform.SetParent(parent, false); go.transform.position = position; go.transform.rotation = Quaternion.Euler(-90, 0, 0);
            var particles = go.AddComponent<ParticleSystem>(); particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = particles.main; main.duration = 2; main.loop = true; main.startLifetime = new ParticleSystem.MinMaxCurve(.65f, 1.2f); main.startSpeed = new ParticleSystem.MinMaxCurve(.08f, .18f); main.startSize = new ParticleSystem.MinMaxCurve(.055f, .11f); main.startColor = color; main.maxParticles = 70; main.simulationSpace = ParticleSystemSimulationSpace.World;
            var emission = particles.emission; emission.rateOverTime = 0f;
            var shape = particles.shape; shape.shapeType = ParticleSystemShapeType.Cone; shape.radius = .075f; shape.angle = 18f;
            var size = particles.sizeOverLifetime; size.enabled = true; size.size = new ParticleSystem.MinMaxCurve(1, AnimationCurve.Linear(0, .6f, 1, 2.4f));
            var fade = particles.colorOverLifetime; fade.enabled = true;
            var gradient = new Gradient(); gradient.SetKeys(new[] { new GradientColorKey(Color.white, 0), new GradientColorKey(Color.white, 1) }, new[] { new GradientAlphaKey(0, 0), new GradientAlphaKey(1, .12f), new GradientAlphaKey(0, 1) }); fade.color = gradient;
            var texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
            for (int y = 0; y < 32; y++) for (int x = 0; x < 32; x++) { float r = new Vector2((x - 15.5f) / 15.5f, (y - 15.5f) / 15.5f).sqrMagnitude; texture.SetPixel(x, y, new Color(1, 1, 1, Mathf.Pow(Mathf.Clamp01(1 - r), 2))); }
            texture.Apply(); texture = Asset(texture, "Steam-softness.asset");
            var material = new Material(Shader.Find("Particles/Standard Unlit")); material.mainTexture = texture; material.SetFloat("_Mode", 2); material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha); material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha); material.SetInt("_ZWrite", 0); material.EnableKeyword("_ALPHABLEND_ON"); material.renderQueue = 3000;
            go.GetComponent<ParticleSystemRenderer>().sharedMaterial = Asset(material, "Steam-particle.mat");
            particles.Play(); return particles;
        }
    }
}
