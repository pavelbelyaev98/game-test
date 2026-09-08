using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SomethingDownThere.Editor
{
    public static class SurfaceStationSetup
    {
        private const string Folder = "Assets/Content/Stations/";

        [MenuItem("Tools/Something Down There/Configure Surface Stations")]
        public static void Configure()
        {
            var scene = SceneManager.GetActiveScene();
            if (EditorApplication.isPlaying || scene.path != MainGameSceneBuilder.ScenePath && scene.name != "MainGame")
                throw new InvalidOperationException("Open MainGame outside Play Mode to configure its stations.");
            var root = scene.GetRootGameObjects().Single(o => o.name == "MainGameRoot").transform;
            ConfigureStation<SellStation>(root.Find("Surface/SellStation"), "SalvageBuyer", "Selling pedestal",
                "IntakeFlap_Export", new Vector3(0, 1.06f, 0.09f), new Vector3(1.5f, 2.12f, 1.26f), new Vector3(-55, 0, 0), Vector3.zero);
            ConfigureStation<UpgradeStation>(root.Find("Surface/UpgradeStation"), "UpgradeWorkbench", "Upgrade pedestal",
                "ToolDrawer_Export", new Vector3(0.04f, 0.98f, 0), new Vector3(2.05f, 1.96f, 1.06f), Vector3.zero, new Vector3(0, 0, 0.12f));
            EditorSceneManager.MarkSceneDirty(scene);
        }

        private static void ConfigureStation<T>(Transform anchor, string modelName, string pedestal, string movingPart,
            Vector3 center, Vector3 size, Vector3 turn, Vector3 slide) where T : StationTarget
        {
            if (anchor == null) throw new InvalidOperationException("Missing surface station anchor.");
            var importer = AssetImporter.GetAtPath(Folder + modelName + ".fbx") as ModelImporter;
            if (importer == null) throw new InvalidOperationException("Missing approved station model: " + modelName);
            importer.materialImportMode = ModelImporterMaterialImportMode.None;
            importer.importCameras = importer.importLights = importer.importAnimation = false;
            importer.isReadable = false;
            importer.SaveAndReimport();
            ConfigureTexture(modelName + "_Albedo.png", false);
            ConfigureTexture(modelName + "_Normal.png", true);
            string materialPath = Folder + modelName + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material == null)
            {
                material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(material, materialPath);
            }
            // Import the Blender-authored baked surface; no substitute flat art.
            material.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(Folder + modelName + "_Albedo.png"));
            material.SetColor("_BaseColor", Color.white);
            material.SetTexture("_BumpMap", AssetDatabase.LoadAssetAtPath<Texture2D>(Folder + modelName + "_Normal.png"));
            material.SetFloat("_BumpScale", 1f);
            material.SetFloat("_Metallic", 0.35f);
            material.SetFloat("_Smoothness", 0.54f);
            material.EnableKeyword("_NORMALMAP");
            EditorUtility.SetDirty(material);
            var prefabRoot = new GameObject(modelName);
            try
            {
                var model = AssetDatabase.LoadAssetAtPath<GameObject>(Folder + modelName + ".fbx");
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(model, prefabRoot.transform);
                instance.name = "Model";
                foreach (var renderer in instance.GetComponentsInChildren<Renderer>())
                    renderer.sharedMaterials = Enumerable.Repeat(material, renderer.sharedMaterials.Length).ToArray();
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, Folder + modelName + ".prefab");
            }
            finally { UnityEngine.Object.DestroyImmediate(prefabRoot); }
            // Touch only this task's station visuals; preserve anchors and terrain.
            foreach (string name in new[] { pedestal, "Station visual" })
            {
                var previous = anchor.Find(name);
                if (previous != null) Undo.DestroyObjectImmediate(previous.gameObject);
            }
            var visual = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Folder + modelName + ".prefab"), anchor);
            visual.name = "Station visual";
            Undo.RegisterCreatedObjectUndo(visual, "Install approved station");
            if (anchor.GetComponent<T>() == null) Undo.AddComponent<T>(anchor.gameObject);
            var collider = anchor.GetComponent<BoxCollider>();
            if (collider == null) collider = Undo.AddComponent<BoxCollider>(anchor.gameObject);
            Undo.RecordObject(collider, "Configure station interaction bounds");
            collider.center = center;
            collider.size = size;
            var motion = anchor.GetComponent<StationMotion>();
            if (motion == null) motion = Undo.AddComponent<StationMotion>(anchor.gameObject);
            Undo.RecordObject(motion, "Wire station model motion");
            var part = visual.GetComponentsInChildren<Transform>().Single(t => t.name == movingPart);
            motion.Configure(part, turn, slide);
            EditorUtility.SetDirty(motion);
            AssetDatabase.SaveAssets();
        }

        private static void ConfigureTexture(string name, bool normal)
        {
            var texture = AssetImporter.GetAtPath(Folder + name) as TextureImporter;
            if (texture == null) throw new InvalidOperationException("Missing Blender texture: " + name);
            texture.textureType = normal ? TextureImporterType.NormalMap : TextureImporterType.Default;
            texture.sRGBTexture = !normal;
            texture.maxTextureSize = 2048;
            texture.mipmapEnabled = true;
            texture.anisoLevel = 2;
            texture.textureCompression = TextureImporterCompression.CompressedHQ;
            texture.SaveAndReimport();
        }
    }
}
