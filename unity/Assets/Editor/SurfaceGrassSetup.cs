using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SomethingDownThere.Editor
{
    public static class SurfaceGrassSetup
    {
        public const string Folder = "Assets/Content/GroundGrass/";
        [MenuItem("Tools/Something Down There/Configure Approved Surface Grass")]
        public static void Configure()
        {
            var scene = SceneManager.GetActiveScene();
            if (EditorApplication.isPlaying || scene.name != "MainGame")
                throw new InvalidOperationException("Open MainGame outside Play Mode.");
            var terrain = scene.GetRootGameObjects().SelectMany(o => o.GetComponentsInChildren<TerrainVolume>()).Single();
            var importer = AssetImporter.GetAtPath(Folder + "GrassClumps.fbx") as ModelImporter;
            if (importer == null) throw new InvalidOperationException("Import the approved Blender grass batch first.");
            importer.materialImportMode = ModelImporterMaterialImportMode.None;
            importer.importCameras = importer.importLights = importer.importAnimation = false;
            importer.isReadable = false;
            importer.SaveAndReimport();
            var meshes = AssetDatabase.LoadAllAssetsAtPath(Folder + "GrassClumps.fbx").OfType<Mesh>().ToArray();
            var near = meshes.Single(m => m.name == "GrassClump");
            var far = meshes.Single(m => m.name == "GrassClump_LOD1");
            if (near.bounds.size.y < .35f || near.bounds.size.y > .5f)
                throw new InvalidOperationException("Grass FBX must have upright metre-scale blades.");
            if (!near.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.TexCoord1))
                throw new InvalidOperationException("Grass requires the authored per-blade phase and height UV channel.");
            var atlasImporter = (TextureImporter)AssetImporter.GetAtPath(Folder + "Grass_Albedo.png");
            atlasImporter.sRGBTexture = true;
            atlasImporter.wrapMode = TextureWrapMode.Clamp;
            atlasImporter.mipmapEnabled = true;
            atlasImporter.filterMode = FilterMode.Trilinear;
            atlasImporter.anisoLevel = 4;
            atlasImporter.textureCompression = TextureImporterCompression.CompressedHQ;
            atlasImporter.SaveAndReimport();
            var shader = Shader.Find("Something Down There/Sunny Grass");
            if (shader == null || ShaderUtil.ShaderHasError(shader)) throw new InvalidOperationException("Grass shader must compile.");
            var material = AssetDatabase.LoadAssetAtPath<Material>(Folder + "SunnyGrass.mat");
            if (material == null) {
                material = new Material(shader) { name = "SunnyGrass" };
                AssetDatabase.CreateAsset(material, Folder + "SunnyGrass.mat");
            }
            material.shader = shader;
            material.enableInstancing = true;
            material.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(Folder + "Grass_Albedo.png"));
            material.SetFloat("_WindAmplitude", .07f);
            material.SetFloat("_WindSpeed", 1.1f);
            EditorUtility.SetDirty(material);
            var grass = terrain.GetComponent<SurfaceGrassRenderer>();
            if (grass == null) grass = Undo.AddComponent<SurfaceGrassRenderer>(terrain.gameObject);
            var settings = new SerializedObject(grass);
            settings.FindProperty("nearMesh").objectReferenceValue = near;
            settings.FindProperty("farMesh").objectReferenceValue = far;
            settings.FindProperty("cellsPerPatch").intValue = 8;
            settings.FindProperty("material").objectReferenceValue = material;
            settings.ApplyModifiedProperties();
            EditorSceneManager.MarkSceneDirty(scene);
            AssetDatabase.SaveAssets();
        }
    }
}
