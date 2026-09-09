using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SomethingDownThere.Editor
{
    public static class SaveGameSetup
    {
        [MenuItem("Tools/Something Down There/Configure Save Integration")]
        public static void Configure()
        {
            var scene = SceneManager.GetActiveScene();
            if (EditorApplication.isPlaying || scene.path != MainGameSceneBuilder.ScenePath)
                throw new InvalidOperationException("Open MainGame outside Play Mode to configure saving.");
            var player = UnityEngine.Object.FindAnyObjectByType<FpsPlayer>();
            if (player == null || player.Discoveries == null) throw new InvalidOperationException("MainGame requires its player and discoveries.");
            var field = new SerializedObject(player.Discoveries);
            var prefabs = field.FindProperty("prefabs");
            for (int i = 0; i < prefabs.arraySize; i++)
            {
                string path = AssetDatabase.GetAssetPath(prefabs.GetArrayElementAtIndex(i).objectReferenceValue);
                var root = PrefabUtility.LoadPrefabContents(path);
                try
                {
                    var settings = new SerializedObject(root.GetComponent<BuriedFind>());
                    var id = settings.FindProperty("saveContentId");
                    if (!string.IsNullOrEmpty(id.stringValue)) continue;
                    id.stringValue = AssetDatabase.AssetPathToGUID(path);
                    settings.ApplyModifiedPropertiesWithoutUndo();
                    PrefabUtility.SaveAsPrefabAsset(root, path);
                }
                finally { PrefabUtility.UnloadPrefabContents(root); }
            }
            if (player.GetComponent<WorldSaveController>() == null) Undo.AddComponent<WorldSaveController>(player.gameObject);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }
    }
}
