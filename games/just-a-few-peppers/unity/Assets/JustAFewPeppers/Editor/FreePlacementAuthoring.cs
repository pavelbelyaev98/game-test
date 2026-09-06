using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static JustAFewPeppers.Editor.FoundationSceneBuilder;

namespace JustAFewPeppers.Editor
{
    public static class FreePlacementAuthoring
    {
        [MenuItem("Just a few peppers/Apply 1_02 free placement revision")]
        public static void Apply()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            var crate = session.handling.crate;
            if (crate.portable != null) throw new InvalidOperationException("Free placement is already authored. Edit the saved scene; do not recreate it.");
            var input = InputActionAsset.FromJson(File.ReadAllText(InputPath));
            var map = input.FindActionMap("Gameplay", true);
            var drop = map.AddAction("Drop", InputActionType.Button, "<Keyboard>/g");
            drop.wantsInitialStateCheck = true;
            var rotate = map.AddAction("Rotate", InputActionType.Value);
            rotate.expectedControlType = "Axis";
            rotate.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/z").With("Positive", "<Keyboard>/x");
            File.WriteAllText(InputPath, input.ToJson());
            UnityEngine.Object.DestroyImmediate(input);
            AssetDatabase.ImportAsset(InputPath);

            var portable = crate.gameObject.AddComponent<PortableBody>();
            crate.portable = portable;
            portable.recoveryPoint = crate.restingPoints[0];
            portable.recoveryPoint.name = "Crate recovery fallback";
            foreach (Transform child in portable.recoveryPoint) UnityEngine.Object.DestroyImmediate(child.gameObject);
            UnityEngine.Object.DestroyImmediate(crate.restingPoints[1].gameObject);
            foreach (var old in crate.parkedColliders) UnityEngine.Object.DestroyImmediate(old);
            crate.restingPoints = Array.Empty<Transform>();
            crate.parkedColliders = Array.Empty<Collider>();
            portable.shape = crate.gameObject.AddComponent<BoxCollider>();
            portable.shape.center = new Vector3(0, .295f, 0);
            portable.shape.size = new Vector3(.97f, .51f, .72f);
            var contact = new PhysicsMaterial("Crate contact") { dynamicFriction = .7f, staticFriction = .8f,
                bounciness = 0, frictionCombine = PhysicsMaterialCombine.Maximum, bounceCombine = PhysicsMaterialCombine.Minimum };
            AssetDatabase.CreateAsset(contact, "Assets/JustAFewPeppers/Content/Crate contact.physicMaterial");
            portable.shape.sharedMaterial = contact;
            portable.body = crate.gameObject.AddComponent<Rigidbody>();
            portable.body.mass = 6;
            portable.body.linearDamping = .15f;
            portable.body.angularDamping = 1.5f;
            portable.body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            portable.body.interpolation = RigidbodyInterpolation.Interpolate;
            portable.body.solverIterations = 10;
            portable.body.solverVelocityIterations = 4;
            portable.body.maxDepenetrationVelocity = 2;
            portable.body.centerOfMass = new Vector3(0, .21f, 0);
            portable.contactAudio = crate.gameObject.AddComponent<AudioSource>();
            portable.contactAudio.playOnAwake = false;
            portable.contactAudio.spatialBlend = 1;
            portable.contactAudio.maxDistance = 10;
            portable.contactClip = session.handling.presentation.crateClip;
            var preview = new GameObject("Crate placement preview").AddComponent<LineRenderer>();
            portable.preview = preview;
            var lineMaterial = new Material(Shader.Find("Unlit/Color"));
            AssetDatabase.CreateAsset(lineMaterial, "Assets/JustAFewPeppers/Content/Placement outline.mat");
            preview.sharedMaterial = lineMaterial;
            preview.useWorldSpace = true;
            preview.widthMultiplier = .014f;
            preview.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            preview.receiveShadows = false;
            preview.enabled = false;
            crate.target.description = "E  Pick up  |  Place it where it fits";
            var pile = GameObject.Find("Pepper mound").GetComponent<YardTarget>();
            pile.displayName = "Pepper pile";
            pile.description = "Hold left mouse with the crate to scoop";

            var wood = AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Wood.mat");
            if (wood == null) wood = crate.target.marker.sharedMaterial;
            var table = new GameObject("Placement worktop").transform;
            table.position = new Vector3(-5.8f, 0, -3.9f);
            Box("Clear worktop", new Vector3(0, .78f, 0), new Vector3(2.4f, .12f, 1.5f), wood, table);
            foreach (float x in new[] { -1f, 1f })
            foreach (float z in new[] { -.55f, .55f })
                Box("Worktop foot", new Vector3(x, .36f, z), new Vector3(.14f, .72f, .14f), wood, table);
            Label("WORKTOP", table, new Vector3(0, .57f, -.77f), .013f);
            var support = Box("Stable low support", new Vector3(-5.8f, .25f, -5.6f), new Vector3(1.25f, .5f, 1.1f), wood);
            Label("LOW SUPPORT", support.transform, new Vector3(0, 0, -.52f), .01f);

            var hud = session.hud;
            hud.transform.Find("Scope").GetComponent<Text>().text = "Free crate handling / Automatic processing";
            hud.transform.Find("Controls").GetComponent<Text>().text =
                "WASD / Arrows  Move     Shift  Sprint     Space  Jump     Mouse  Look\n" +
                "E  Grab / place / tip at intake     Z / X  Rotate     G  Drop     Hold left mouse  Scoop\n" +
                "Esc  Pause     R  Return + recover lost crate     F8  Restart processing test";
            hud.pausePanel.transform.Find("Pause help").GetComponent<Text>().text =
                "Hold left mouse at the pepper pile. E tips at the intake.\nAim at a surface: E places, Z/X rotates, G drops. R keeps all food.";
            hud.resetButton.GetComponentInChildren<Text>().text = "Return to gate / recover lost crate";
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("FREE_PLACEMENT_AUTHORED " + ScenePath);
        }

        [MenuItem("Just a few peppers/Tune placement preview visibility")]
        public static void TunePreview()
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Placement outline.mat");
            material.shader = Shader.Find("Unlit/Color");
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            Debug.Log("PLACEMENT_PREVIEW_MATERIAL_UPDATED");
        }
    }
}
