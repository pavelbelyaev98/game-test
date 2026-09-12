using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SomethingDownThere.Editor
{
    public static class SunPresentationSetup
    {
        public const string Folder = "Assets/Content/Sun/";
        [MenuItem("Tools/Something Down There/Configure Approved Sun")]
        public static void Configure()
        {
            var scene = SceneManager.GetActiveScene();
            if (EditorApplication.isPlaying || scene.name != "MainGame")
                throw new InvalidOperationException("Open MainGame outside Play Mode.");
            var root = scene.GetRootGameObjects().Single(o => o.name == "MainGameRoot");
            var camera = root.GetComponentInChildren<Camera>();
            var sun = root.transform.Find("Sun").GetComponent<Light>();
            var importer = AssetImporter.GetAtPath(Folder + "Sun_Disc.png") as TextureImporter;
            if (importer == null) throw new InvalidOperationException("Import the approved Blender sun disc first.");
            importer.sRGBTexture = true;
            importer.alphaSource = TextureImporterAlphaSource.FromInput;
            importer.alphaIsTransparency = true;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Trilinear;
            importer.mipmapEnabled = true;
            importer.textureCompression = TextureImporterCompression.CompressedHQ;
            importer.SaveAndReimport();
            var shader = Shader.Find("Something Down There/Sunny Sun Sky");
            if (shader == null || ShaderUtil.ShaderHasError(shader))
                throw new InvalidOperationException("The sun sky shader must compile before integration.");
            var material = AssetDatabase.LoadAssetAtPath<Material>(Folder + "SunnySun.mat");
            if (material == null) {
                material = new Material(shader) { name = "SunnySun" };
                AssetDatabase.CreateAsset(material, Folder + "SunnySun.mat");
            }
            Undo.RecordObject(material, "Configure approved sun");
            material.shader = shader;
            material.SetTexture("_SunMap", AssetDatabase.LoadAssetAtPath<Texture2D>(Folder + "Sun_Disc.png"));
            // Preserve the accepted clear cyan sky independently of cloud art.
            camera.backgroundColor = new Color(.12f, .77f, .85f, 1);
            material.SetColor("_SkyColor", camera.backgroundColor);
            material.SetColor("_HorizonColor", new Color(.42f, .88f, .9f, 1));
            material.SetVector("_SunDirection", -sun.transform.forward);
            // The authored disc occupies ~60% of the 10-degree texture width.
            material.SetFloat("_SunTangentRadius", Mathf.Tan(5 * Mathf.Deg2Rad));
            EditorUtility.SetDirty(material);
            var sky = camera.GetComponent<Skybox>();
            if (sky == null) sky = Undo.AddComponent<Skybox>(camera.gameObject);
            Undo.RecordObjects(new UnityEngine.Object[] { camera, sky }, "Show approved sun in the existing sky");
            sky.material = material;
            camera.clearFlags = CameraClearFlags.Skybox;
            EditorUtility.SetDirty(camera);
            EditorUtility.SetDirty(sky);
            EditorSceneManager.MarkSceneDirty(scene);
            AssetDatabase.SaveAssets();
        }
    }
}
