using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SomethingDownThere.Editor
{
    public static class MainGameSceneBuilder
    {
        public const string ScenePath = "Assets/Scenes/MainGame.unity";
        private const string MaterialsPath = "Assets/Settings/MainGame";

        [MenuItem("Tools/Something Down There/Create Main Game Scene")]
        public static void CreateFromMenu()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            CreateIfMissing();
        }

        // Run in a closed or isolated project. Existing scene content is never overwritten.
        public static void CreateIfMissing()
        {
            if (File.Exists(ScenePath))
            {
                EditorSceneManager.OpenScene(ScenePath);
                return;
            }
            Directory.CreateDirectory(MaterialsPath);
            AssetDatabase.Refresh();
            Material soil = MaterialAsset("Soil", new Color(0.43f, 0.28f, 0.14f));
            Material grass = MaterialAsset("Surface", new Color(0.32f, 0.42f, 0.22f));
            Material rock = MaterialAsset("Bedrock", new Color(0.24f, 0.29f, 0.34f));
            Material water = MaterialAsset("Water", new Color(0.08f, 0.42f, 0.57f), 0.8f);
            Material bark = MaterialAsset("Bark", new Color(0.25f, 0.17f, 0.10f));
            Material foliage = MaterialAsset("Foliage", new Color(0.14f, 0.29f, 0.19f));
            Material sell = MaterialAsset("SellAnchor", new Color(0.76f, 0.49f, 0.16f));
            Material upgrade = MaterialAsset("UpgradeAnchor", new Color(0.16f, 0.41f, 0.52f));

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "MainGame";
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.58f, 0.64f, 0.70f);
            var root = new GameObject("MainGameRoot").transform;
            var sun = new GameObject("Sun", typeof(Light));
            sun.transform.SetParent(root, false);
            sun.transform.rotation = Quaternion.Euler(48, -28, 0);
            sun.GetComponent<Light>().type = LightType.Directional;
            sun.GetComponent<Light>().intensity = 1.4f;
            sun.GetComponent<Light>().shadows = LightShadows.None;

            Transform surface = Group("Surface", root);
            Block("South rim", surface, new Vector3(0, -0.5f, -14), new Vector3(32, 1, 4), grass);
            Block("North rim", surface, new Vector3(0, -0.5f, 14), new Vector3(32, 1, 4), grass);
            Block("West rim", surface, new Vector3(-14, -0.5f, 0), new Vector3(4, 1, 24), grass);
            Block("East rim", surface, new Vector3(14, -0.5f, 0), new Vector3(4, 1, 24), grass);

            Transform bedrock = Group("Bedrock", root);
            Boundary("Floor", bedrock, new Vector3(0, -12.5f, 0), new Vector3(26, 1, 26), rock);
            // Meet the rim underside at y=-1. Overlapping vertical faces flicker after digging.
            Boundary("West", bedrock, new Vector3(-12.5f, -6.5f, 0), new Vector3(1, 11, 24), rock);
            Boundary("East", bedrock, new Vector3(12.5f, -6.5f, 0), new Vector3(1, 11, 24), rock);
            Boundary("North", bedrock, new Vector3(0, -6.5f, 12.5f), new Vector3(26, 11, 1), rock);
            Boundary("South", bedrock, new Vector3(0, -6.5f, -12.5f), new Vector3(26, 11, 1), rock);

            Transform perimeter = Group("Perimeter", root);
            Perimeter("West", perimeter, new Vector3(-16.5f, 0.6f, 0), new Vector3(1, 1.2f, 34), rock);
            Perimeter("East", perimeter, new Vector3(16.5f, 0.6f, 0), new Vector3(1, 1.2f, 34), rock);
            Perimeter("North shoreline", perimeter, new Vector3(0, 0.6f, 16.5f), new Vector3(34, 1.2f, 1), rock);
            Perimeter("South", perimeter, new Vector3(0, 0.6f, -16.5f), new Vector3(34, 1.2f, 1), rock);

            var terrainRoot = new GameObject("Excavation");
            terrainRoot.SetActive(false);
            terrainRoot.transform.SetParent(root, false);
            terrainRoot.transform.position = new Vector3(-12, -12, -12);
            var preview = Block("Untouched preview (edit mode only)", terrainRoot.transform,
                new Vector3(0, -6, 0), new Vector3(24, 12, 24), soil);
            terrainRoot.AddComponent<TerrainVolume>().Configure(new Vector3Int(192, 96, 192), 0.125f, 16, ShovelProfile.Defaults()[0].Radius, soil, preview);
            terrainRoot.SetActive(true);

            // Colored pedestals reserve nearby station positions; no fake transactions/recharge.
            Transform sellAnchor = Anchor("SellStation", surface, new Vector3(-3, 0, -14));
            Block("Selling pedestal", sellAnchor, new Vector3(-3, 0.6f, -14), new Vector3(1.2f, 1.2f, 1), sell);
            Transform upgradeAnchor = Anchor("UpgradeStation", surface, new Vector3(3, 0, -14));
            Block("Upgrade pedestal", upgradeAnchor, new Vector3(3, 0.6f, -14), new Vector3(1.2f, 1.2f, 1), upgrade);
            Anchor("RechargeZone", surface, new Vector3(0, 0, -14.5f));
            Anchor("ReturnAnchor", surface, new Vector3(0, 0.1f, -13.5f));

            Transform scenery = Group("Scenery", root);
            var waterObject = Block("Water", scenery, new Vector3(0, -0.35f, 24), new Vector3(44, 0.15f, 14), water);
            UnityEngine.Object.DestroyImmediate(waterObject.GetComponent<Collider>());
            for (int i = 0; i < 6; i++)
            {
                float side = i % 2 == 0 ? -1 : 1;
                var boulder = Primitive(PrimitiveType.Sphere, "Rock " + i, scenery,
                    new Vector3(side * (18 + i % 3), 0.3f, -8 + i * 4), new Vector3(3, 2.2f, 2.5f), rock);
                boulder.transform.rotation = Quaternion.Euler(0, i * 31, i * 9);
            }
            for (int i = 0; i < 4; i++)
            {
                Vector3 position = new Vector3(i < 2 ? -19 : 19, 0, i % 2 == 0 ? -12 : 11);
                Primitive(PrimitiveType.Cylinder, "Tree trunk " + i, scenery, position + Vector3.up * 2,
                    new Vector3(0.55f, 2, 0.55f), bark);
                Primitive(PrimitiveType.Sphere, "Tree crown " + i, scenery, position + Vector3.up * 4.8f,
                    new Vector3(4, 5, 4), foliage);
            }
            CreatePlayer(root);
            SurfaceStationSetup.Configure();
            var playerSettings = new SerializedObject(root.GetComponentInChildren<FpsPlayer>());
            playerSettings.FindProperty("excavationTerrain").objectReferenceValue = terrainRoot.GetComponent<TerrainVolume>();
            playerSettings.FindProperty("surfaceReturn").objectReferenceValue = surface.Find("ReturnAnchor");
            playerSettings.ApplyModifiedPropertiesWithoutUndo();
            ConfigureSurfaceRecharge();
            ConfigureDiscoveryContent();
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            Debug.Log("Main game scene created with untouched terrain and permanent boundaries.");
        }

        [MenuItem("Tools/Something Down There/Configure Surface Recharge")]
        public static void ConfigureSurfaceRecharge()
        {
            var scene = SceneManager.GetActiveScene();
            if (EditorApplication.isPlaying || scene.path != ScenePath && scene.name != "MainGame")
                throw new InvalidOperationException("Open MainGame outside Play Mode to configure recharge.");
            var root = scene.GetRootGameObjects()[0].transform;
            var anchor = root.Find("Surface/RechargeZone");
            var player = root.GetComponentInChildren<FpsPlayer>();
            var terrain = root.GetComponentInChildren<TerrainVolume>();
            if (anchor == null || player == null || terrain == null)
                throw new InvalidOperationException("MainGame needs its existing recharge anchor, player and terrain.");
            var recharge = anchor.GetComponent<SurfaceRecharge>();
            if (recharge == null) recharge = Undo.AddComponent<SurfaceRecharge>(anchor.gameObject);
            Undo.RecordObject(recharge, "Configure surface recharge");
            recharge.Configure(player, terrain);
            EditorUtility.SetDirty(recharge);
            var settings = new SerializedObject(player);
            settings.FindProperty("surfaceRecharge").objectReferenceValue = recharge;
            settings.ApplyModifiedProperties();
            EditorSceneManager.MarkSceneDirty(scene);
        }

        [MenuItem("Tools/Something Down There/Configure Discovery Content")]
        public static void ConfigureDiscoveryContent()
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.path != ScenePath && scene.name != "MainGame")
                throw new InvalidOperationException("Open MainGame before configuring its discoveries.");
            if (EditorApplication.isPlaying) throw new InvalidOperationException("Configure discovery assets outside Play Mode.");
            var root = scene.GetRootGameObjects()[0].transform;
            var terrain = root.GetComponentInChildren<TerrainVolume>();
            var player = root.GetComponentInChildren<FpsPlayer>();
            if (terrain == null || player == null) throw new InvalidOperationException("MainGame needs its terrain and player.");
            const string folder = "Assets/Content/Finds";
            if (!AssetDatabase.IsValidFolder("Assets/Content")) AssetDatabase.CreateFolder("Assets", "Content");
            if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder("Assets/Content", "Finds");
            var names = new[] { "Blue marble", "Copper token", "Amber bead" };
            var colors = new[] { new Color(0.05f, 0.7f, 0.95f), new Color(0.93f, 0.36f, 0.13f), new Color(1f, 0.72f, 0.08f) };
            var sizes = new[] { Vector3.one * 0.8f, new Vector3(1f, 0.18f, 1f), new Vector3(0.64f, 0.9f, 0.64f) };
            var prefabs = new BuriedFind[3];
            for (int i = 0; i < names.Length; i++)
            {
                string prefabPath = folder + "/" + names[i] + ".prefab";
                var existing = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (existing != null) { prefabs[i] = existing.GetComponent<BuriedFind>(); continue; }
                string materialPath = folder + "/" + names[i] + ".mat";
                var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                if (material == null)
                {
                    material = new Material(Shader.Find("Universal Render Pipeline/Lit")) { name = names[i], color = colors[i] };
                    material.SetFloat("_Smoothness", 0.55f);
                    AssetDatabase.CreateAsset(material, materialPath);
                }
                // These simple forms are explicitly requested for development review.
                // The reusable collection system accepts replacement prefab art later.
                var shape = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                try
                {
                    shape.name = names[i];
                    shape.transform.localScale = sizes[i];
                    shape.GetComponent<MeshRenderer>().sharedMaterial = material;
                    UnityEngine.Object.DestroyImmediate(shape.GetComponent<SphereCollider>());
                    shape.AddComponent<MeshCollider>().sharedMesh = shape.GetComponent<MeshFilter>().sharedMesh;
                    var find = shape.AddComponent<BuriedFind>();
                    var settings = new SerializedObject(find);
                    settings.FindProperty("displayName").stringValue = names[i];
                    settings.FindProperty("saleValue").intValue = 5 + i * 3;
                    settings.ApplyModifiedPropertiesWithoutUndo();
                    prefabs[i] = PrefabUtility.SaveAsPrefabAsset(shape, prefabPath).GetComponent<BuriedFind>();
                }
                finally { UnityEngine.Object.DestroyImmediate(shape); }
            }
            Transform fieldRoot = root.Find("Discoveries");
            if (fieldRoot == null) fieldRoot = Group("Discoveries", root);
            var field = fieldRoot.GetComponent<DiscoveryField>();
            if (field == null) field = fieldRoot.gameObject.AddComponent<DiscoveryField>();
            var fieldSettings = new SerializedObject(field);
            fieldSettings.FindProperty("terrain").objectReferenceValue = terrain;
            var sources = fieldSettings.FindProperty("prefabs");
            sources.arraySize = prefabs.Length;
            for (int i = 0; i < prefabs.Length; i++) sources.GetArrayElementAtIndex(i).objectReferenceValue = prefabs[i];
            fieldSettings.ApplyModifiedPropertiesWithoutUndo();
            var playerSettings = new SerializedObject(player);
            playerSettings.FindProperty("discoveries").objectReferenceValue = field;
            playerSettings.FindProperty("tuning.DigReach").floatValue = 3f;
            var levels = playerSettings.FindProperty("shovelLevels");
            for (int i = 0; i < levels.arraySize; i++) levels.GetArrayElementAtIndex(i).FindPropertyRelative("ReachBonus").floatValue = i * 0.2f;
            playerSettings.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.MarkSceneDirty(scene);
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Tools/Something Down There/Configure Find Collection")]
        public static void ConfigureFindCollection()
        {
            if (EditorApplication.isPlaying) throw new InvalidOperationException("Configure finds outside Play Mode.");
            var names = new[] { "Blue marble", "Copper token", "Amber bead" };
            var sizes = new[] { Vector3.one * 0.8f, new Vector3(1f, 0.18f, 1f), new Vector3(0.64f, 0.9f, 0.64f) };
            for (int i = 0; i < names.Length; i++)
            {
                string path = "Assets/Content/Finds/" + names[i] + ".prefab";
                if (AssetDatabase.LoadAssetAtPath<GameObject>(path) == null)
                    throw new InvalidOperationException("Missing approved find prefab: " + path);
                var root = PrefabUtility.LoadPrefabContents(path);
                try
                {
                    root.transform.localScale = sizes[i];
                    var settings = new SerializedObject(root.GetComponent<BuriedFind>());
                    settings.FindProperty("collectionThreshold").floatValue = 0.5f;
                    settings.ApplyModifiedPropertiesWithoutUndo();
                    PrefabUtility.SaveAsPrefabAsset(root, path);
                }
                finally { PrefabUtility.UnloadPrefabContents(root); }
            }
        }

        private static void CreatePlayer(Transform parent)
        {
            var player = new GameObject("Player");
            player.SetActive(false);
            player.transform.SetParent(parent, false);
            player.transform.position = new Vector3(0, 0.1f, -13);
            player.layer = LayerMask.NameToLayer("Ignore Raycast");
            var motor = player.AddComponent<CharacterController>();
            motor.height = 1.8f;
            motor.radius = 0.3f;
            motor.center = new Vector3(0, 0.9f, 0);
            motor.stepOffset = 0.3f;
            motor.minMoveDistance = 0;
            var cameraRoot = new GameObject("Camera", typeof(Camera), typeof(AudioListener));
            cameraRoot.transform.SetParent(player.transform, false);
            cameraRoot.transform.localPosition = new Vector3(0, 1.6f, 0);
            cameraRoot.tag = "MainCamera";
            var camera = cameraRoot.GetComponent<Camera>();
            camera.fieldOfView = 75;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 180f;
            camera.allowMSAA = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.48f, 0.67f, 0.79f);
            player.AddComponent<FpsPlayer>();
            player.AddComponent<FpsHud>();
            player.SetActive(true);
        }

        private static Transform Group(string name, Transform parent)
        {
            var root = new GameObject(name).transform;
            root.SetParent(parent, false);
            return root;
        }

        private static Transform Anchor(string name, Transform parent, Vector3 position)
        {
            Transform root = Group(name, parent);
            root.position = position;
            return root;
        }

        private static GameObject Block(string name, Transform parent, Vector3 position, Vector3 size, Material material)
            => Primitive(PrimitiveType.Cube, name, parent, position, size, material);

        private static GameObject Primitive(PrimitiveType type, string name, Transform parent,
            Vector3 position, Vector3 size, Material material)
        {
            GameObject item = GameObject.CreatePrimitive(type);
            item.name = name;
            item.transform.SetParent(parent, false);
            item.transform.position = position;
            item.transform.localScale = size;
            item.GetComponent<Renderer>().sharedMaterial = material;
            return item;
        }

        private static void Boundary(string name, Transform parent, Vector3 position, Vector3 size, Material material)
            => Block(name, parent, position, size, material).AddComponent<PermanentTerrainBoundary>();

        private static void Perimeter(string name, Transform parent, Vector3 position, Vector3 size, Material material)
        {
            Boundary(name, parent, position, size, material);
            // A visible wall marks the finite site; its airspace also blocks powered escape.
            Transform airspace = Anchor(name + " airspace", parent, new Vector3(position.x, 65.2f, position.z));
            airspace.gameObject.AddComponent<BoxCollider>().size = new Vector3(size.x, 128, size.z);
            airspace.gameObject.AddComponent<PermanentTerrainBoundary>();
        }

        private static Material MaterialAsset(string name, Color color, float smoothness = 0.15f)
        {
            string path = MaterialsPath + "/" + name + ".mat";
            Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return existing;
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) throw new InvalidOperationException("The project requires its URP Lit shader.");
            var material = new Material(shader) { name = name, color = color };
            material.SetFloat("_Smoothness", smoothness);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }
    }
}
