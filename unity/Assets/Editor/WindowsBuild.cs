using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SomethingDownThere.Editor
{
    public static class WindowsBuild
    {
        private const string ScenePath = "Assets/Scenes/MainGame.unity";

        [MenuItem("Tools/Something Down There/Build Windows Player")]
        public static void Build() => BuildPlayer(BuildOptions.Development);

        [MenuItem("Tools/Something Down There/Build Windows Release Player")]
        public static void BuildRelease() => BuildPlayer(BuildOptions.None);

        private static void BuildPlayer(BuildOptions options)
        {
            string output = Path.GetFullPath(Path.Combine(
                Application.dataPath, "../../builds/windows/SomethingDownThere.exe"));
            Directory.CreateDirectory(Path.GetDirectoryName(output) ?? throw new InvalidOperationException());

            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = output,
                target = BuildTarget.StandaloneWindows64,
                // The single local executable includes developer admin controls. Release
                // builds report Debug.isDebugBuild=false, disabling the runtime gate.
                options = options
            });

            if (report.summary.result != BuildResult.Succeeded)
                throw new InvalidOperationException($"Windows build failed: {report.summary.result}");

            Debug.Log($"Windows build: {output}");
        }
    }
}
