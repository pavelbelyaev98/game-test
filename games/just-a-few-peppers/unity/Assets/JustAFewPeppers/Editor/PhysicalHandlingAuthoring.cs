using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using static JustAFewPeppers.Editor.FoundationSceneBuilder;

namespace JustAFewPeppers.Editor
{
    public static class PhysicalHandlingAuthoring
    {
        public static void Apply()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            var input = InputActionAsset.FromJson(File.ReadAllText(InputPath));
            if (input.FindAction("Gameplay/Grab") != null) throw new InvalidOperationException("Physical handling already authored.");
            input.FindAction("Gameplay/Toss", true).Rename("Grab");
            input.FindAction("Gameplay/Scoop", true).Rename("Use");
            input.FindAction("Gameplay/Use", true).wantsInitialStateCheck = true;
            File.WriteAllText(InputPath, input.ToJson());
            UnityEngine.Object.DestroyImmediate(input);
            AssetDatabase.ImportAsset(InputPath);
            session.hud.controlsText.text = "WASD / Arrows  Move     Shift  Sprint     Space  Jump     Mouse  Look\n" +
                "Right click  Grab / release     G  Release (same action)\n" +
                "Hold left mouse  Gather with crate / charge a held prop's throw\n" +
                "Release left mouse  Stop gathering / throw the held prop\n" +
                "E  Tip / collect / hand off; otherwise carefully set down\n" +
                "Z / X  Optional rotation     Esc  Pause     F1  This reference\n\n" +
                "R  Return / recover objects (keeps food)\n" +
                "F8  Restart food test (clears food, keeps prop arrangements)";
            session.hud.controlsText.fontSize = 20;
            // A small reusable graybox surface makes rolling easy to inspect in ordinary play.
            var board = GameObject.CreatePrimitive(PrimitiveType.Cube);
            board.name = "Sloped play board";
            board.transform.SetParent(session.handling.looseProps.transform.parent, false);
            board.transform.SetPositionAndRotation(new Vector3(-6, .22f, 1.6f), Quaternion.Euler(15, 0, 0));
            board.transform.localScale = new Vector3(1.2f, .10f, 1.8f);
            board.GetComponent<Renderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Loose prop wood.mat");
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Physical handling scene save failed.");
            AssetDatabase.SaveAssets();
            Debug.Log("PHYSICAL_HANDLING_AUTHORED " + ScenePath);
        }

        public static void MoveBoard()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            GameObject.Find("Sloped play board").transform.position = new Vector3(-6, .22f, 1.6f);
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Board move save failed.");
        }
    }
}
