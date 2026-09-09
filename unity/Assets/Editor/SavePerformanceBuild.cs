using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SomethingDownThere.Editor
{
    public static class SavePerformanceBuild
    {
        [MenuItem("Tools/Something Down There/Validation/Build Save Performance Player")]
        public static void Build()
        {
            const string fixture = "Assets/Scenes/SavePerformanceValidation.unity";
            if (File.Exists(fixture)) throw new InvalidOperationException("The generated validation scene path is already occupied.");
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty)
                throw new InvalidOperationException("Save scene work before building validation.");
            var setup = EditorSceneManager.GetSceneManagerSetup();
            try
            {
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                new GameObject("Save performance validation", typeof(SavePerformanceFixture));
                EditorSceneManager.SaveScene(scene, fixture);
                string path = Path.GetFullPath(Path.Combine(Application.dataPath, "../../builds/validation/saving/SavePerformance.exe"));
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
                    scenes = new[] { fixture, "Assets/Scenes/MainGame.unity" }, locationPathName = path,
                    target = BuildTarget.StandaloneWindows64, options = BuildOptions.None
                });
                if (report.summary.result != BuildResult.Succeeded) throw new InvalidOperationException("Save validation build failed.");
            }
            finally
            {
                EditorSceneManager.RestoreSceneManagerSetup(setup);
                AssetDatabase.DeleteAsset(fixture);
            }
        }
    }
}
