using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SomethingDownThere.Editor
{
    public static class ExcavatorSetup
    {
        public const string Folder = "Assets/Content/Excavator";
        public const int ToolLayer = ExcavatorView.ToolLayer;
        public const string PrefabPath = Folder + "/Resources/ExperimentalExcavator.prefab";

        [MenuItem("Tools/Something Down There/Sync Experimental Excavator")]
        public static void Sync()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) throw new System.InvalidOperationException("Stop Play before syncing the tool.");
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (scene.path != "Assets/Scenes/MainGame.unity") throw new System.InvalidOperationException("Open MainGame before syncing the tool.");
            var player = scene.GetRootGameObjects().SelectMany(r => r.GetComponentsInChildren<FpsPlayer>(true)).Single();
            var camera = player.GetComponentInChildren<Camera>();
            string source = Path.GetFullPath(Path.Combine(Application.dataPath, "../../art/earthshaper"));
            Directory.CreateDirectory(Folder + "/Resources"); Directory.CreateDirectory(Folder + "/Meshes");
            foreach (string name in new[] { "Excavator.fbx", "BaseColor.png", "Occlusion.png", "LICENSE.txt" })
                File.Copy(Path.Combine(source, name), Folder + "/" + name, true);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            // The user withdrew the audio addition. Never reimport it on sync.
            if (AssetDatabase.IsValidFolder(Folder + "/Audio")) AssetDatabase.DeleteAsset(Folder + "/Audio");
            if (AssetDatabase.LoadAssetAtPath<GameObject>(Folder + "/Excavator.prefab") != null)
            {
                string error = AssetDatabase.MoveAsset(Folder + "/Excavator.prefab", PrefabPath);
                if (!string.IsNullOrEmpty(error)) throw new System.IO.IOException(error);
            }
            var importer = (ModelImporter)AssetImporter.GetAtPath(Folder + "/Excavator.fbx");
            importer.materialImportMode = ModelImporterMaterialImportMode.None;
            importer.isReadable = true; importer.importAnimation = false; importer.addCollider = false;
            importer.SaveAndReimport();
            foreach (string name in new[] { "BaseColor", "Occlusion" })
            {
                var tex = (TextureImporter)AssetImporter.GetAtPath(Folder + "/" + name + ".png");
                tex.sRGBTexture = name == "BaseColor"; tex.mipmapEnabled = true; tex.maxTextureSize = 1024;
                tex.textureCompression = TextureImporterCompression.CompressedHQ; tex.SaveAndReimport();
            }
            string materialPath = Folder + "/Excavator.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material == null) { material = new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(material, materialPath); }
            material.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(Folder + "/BaseColor.png"));
            material.SetTexture("_OcclusionMap", AssetDatabase.LoadAssetAtPath<Texture2D>(Folder + "/Occlusion.png"));
            material.EnableKeyword("_OCCLUSIONMAP"); material.SetFloat("_OcclusionStrength", .65f);
            material.SetFloat("_Metallic", .18f); material.SetFloat("_Smoothness", .32f); EditorUtility.SetDirty(material);
            var root = new GameObject("Experimental excavator");
            var model = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Folder + "/Excavator.fbx"), root.transform);
            PrefabUtility.UnpackPrefabInstance(model, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            model.name = "Geometry";
            foreach (var filter in model.GetComponentsInChildren<MeshFilter>())
            {
                var mesh = Object.Instantiate(filter.sharedMesh);
                var triangles = mesh.triangles; mesh.subMeshCount = 1; mesh.SetTriangles(triangles, 0);
                string path = Folder + "/Meshes/" + filter.name + ".asset";
                var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
                if (existing != null) { EditorUtility.CopySerialized(mesh, existing); Object.DestroyImmediate(mesh); mesh = existing; }
                else AssetDatabase.CreateAsset(mesh, path);
                filter.sharedMesh = mesh;
                var renderer = filter.GetComponent<MeshRenderer>(); renderer.sharedMaterials = new[] { material };
                renderer.shadowCastingMode = ShadowCastingMode.Off; renderer.receiveShadows = false;
                filter.gameObject.layer = ToolLayer;
            }
            Transform Part(string name) => model.GetComponentsInChildren<Transform>(true).Single(t => t.name == name);
            var toolCamera = new GameObject("Tool camera", typeof(Camera)).GetComponent<Camera>();
            toolCamera.transform.SetParent(root.transform, false);
            toolCamera.cullingMask = 1 << ToolLayer; toolCamera.nearClipPlane = .01f; toolCamera.farClipPlane = 5;
            toolCamera.clearFlags = CameraClearFlags.Depth; toolCamera.fieldOfView = camera.fieldOfView;
            var data = toolCamera.GetUniversalAdditionalCameraData(); data.renderType = CameraRenderType.Overlay;
            data.renderPostProcessing = false; data.renderShadows = false;
            root.AddComponent<ExcavatorView>().Configure(model.transform, Part("Rotor"), Part("JawLeft"), Part("JawRight"),
                Part("Coils"), Part("PowerPack"), Part("Brace"), toolCamera);
            model.transform.localPosition = new Vector3(.30f, -.29f, .48f);
            model.transform.localScale = Vector3.one * .62f;
            model.SetActive(false); toolCamera.enabled = false;
            PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            Object.DestroyImmediate(root);
            var old = camera.transform.Find("Excavator");
            var cameraData = camera.GetUniversalAdditionalCameraData();
            if (old != null)
            {
                cameraData.cameraStack.Remove(old.GetComponentInChildren<Camera>(true));
                Object.DestroyImmediate(old.gameObject);
            }
            camera.cullingMask |= 1 << ToolLayer;
            EditorUtility.SetDirty(camera); EditorUtility.SetDirty(cameraData);
            // The baseline scene had no authored URP camera component. An admin
            // experiment adds the default component at runtime only when needed.
            if (cameraData.cameraStack.Count == 0) Object.DestroyImmediate(cameraData);
            AssetDatabase.SaveAssets(); EditorSceneManager.MarkSceneDirty(scene); EditorSceneManager.SaveScene(scene);
            Debug.Log("Silent experimental excavator synced. MainGame uses the normal shovel; admin opt-in loads the trial rig.");
        }
    }
}
