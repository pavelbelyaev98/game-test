using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using static JustAFewPeppers.Editor.FoundationSceneBuilder;

namespace JustAFewPeppers.Editor
{
    public static class PepperBatchAuthoring
    {
        public const string PrefabPath = "Assets/JustAFewPeppers/Content/Physical pepper.prefab";
        static void AlignCrate(RawCarrierView crate)
        {
            var floor = crate.transform.Find("Crate base");
            floor.localPosition = crate.portable.shape.center; floor.localScale = crate.portable.shape.size;
            int side = 0, end = 2;
            foreach (Transform child in crate.transform)
            {
                int index = child.name == "Crate side" ? side++ : child.name == "Crate end" ? end++ : -1;
                if (index < 0) continue;
                var wall = (BoxCollider)crate.portable.additionalShapes[index];
                child.localPosition = wall.center;
                child.localScale = wall.size;
            }
            crate.target.marker.transform.localPosition = new Vector3(0, .29f, -.38f);
        }

        public static void AlignCrateGeometry()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            AlignCrate(UnityEngine.Object.FindAnyObjectByType<YardSession>().handling.crate);
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Crate geometry save failed.");
        }
        static void ScatterSeeds(PepperSeed[] seeds)
        {
            var random = new System.Random(1707);
            for (int i = 0; i < seeds.Length; i++)
            {
                seeds[i].position = new Vector3(-4.4f + i % 11 * .34f + (float)(random.NextDouble() - .5) * .18f,
                    .14f + (float)random.NextDouble() * .06f, -1 + i / 11 * .35f + (float)(random.NextDouble() - .5) * .18f);
                seeds[i].yaw = (float)random.NextDouble() * 360;
            }
        }

        public static void ScatterSupply()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var batch = UnityEngine.Object.FindAnyObjectByType<PepperBatch>();
            ScatterSeeds(batch.seeds);
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Scattered supply save failed.");
        }
        public static void ConfigureComparison()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            var input = InputActionAsset.FromJson(File.ReadAllText(InputPath));
            var map = input.FindActionMap("System", true);
            if (map.FindAction("PepperComparison") == null) map.AddAction("PepperComparison", InputActionType.Button, "<Keyboard>/f9");
            File.WriteAllText(InputPath, input.ToJson()); UnityEngine.Object.DestroyImmediate(input); AssetDatabase.ImportAsset(InputPath);
            if (!session.hud.controlsText.text.Contains("F9")) session.hud.controlsText.text += "\nF9  Compare pepper simulation (test tool; keeps food)";
            GameObject.Find("Pepper mound").GetComponent<YardTarget>().enabled = false;
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Comparison setup save failed.");
        }
        public static void Apply()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            if (session.handling.peppers != null) throw new InvalidOperationException("Pepper batch already authored; edit the saved scene.");
            var input = InputActionAsset.FromJson(File.ReadAllText(InputPath));
            var pour = input.FindActionMap("Gameplay", true).AddAction("Pour", InputActionType.Button, "<Keyboard>/f");
            pour.wantsInitialStateCheck = true;
            File.WriteAllText(InputPath, input.ToJson()); UnityEngine.Object.DestroyImmediate(input); AssetDatabase.ImportAsset(InputPath);
            var red = AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Ripe pepper.mat");
            var green = AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Pepper stem.mat");
            var contact = AssetDatabase.LoadAssetAtPath<PhysicsMaterial>("Assets/JustAFewPeppers/Content/Crate contact.physicMaterial");
            var prototype = new GameObject("Physical pepper").AddComponent<PepperBody>();
            prototype.body = prototype.gameObject.AddComponent<Rigidbody>();
            prototype.body.mass = .12f; prototype.body.linearDamping = .08f; prototype.body.angularDamping = .35f;
            prototype.body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            prototype.body.interpolation = RigidbodyInterpolation.Interpolate;
            prototype.body.maxDepenetrationVelocity = 2; prototype.body.solverIterations = 10;
            prototype.shape = prototype.gameObject.AddComponent<CapsuleCollider>();
            prototype.shape.direction = 2; prototype.shape.radius = .065f; prototype.shape.height = .27f;
            prototype.shape.sharedMaterial = contact;
            var skin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            skin.name = "Pepper flesh"; skin.transform.SetParent(prototype.transform, false);
            skin.transform.localScale = new Vector3(.14f, .13f, .29f);
            UnityEngine.Object.DestroyImmediate(skin.GetComponent<Collider>());
            prototype.skin = skin.GetComponent<Renderer>(); prototype.skin.sharedMaterial = red;
            var stem = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stem.name = "Pepper stem"; stem.transform.SetParent(prototype.transform, false);
            stem.transform.localPosition = new Vector3(0, .025f, .16f); stem.transform.localScale = new Vector3(.025f, .03f, .075f);
            UnityEngine.Object.DestroyImmediate(stem.GetComponent<Collider>()); stem.GetComponent<Renderer>().sharedMaterial = green;
            var prefab = PrefabUtility.SaveAsPrefabAsset(prototype.gameObject, PrefabPath).GetComponent<PepperBody>();
            UnityEngine.Object.DestroyImmediate(prototype.gameObject);
            var batch = new GameObject("Physical pepper batch").AddComponent<PepperBatch>();
            session.handling.peppers = batch; batch.pepperPrefab = prefab;
            batch.handAnchor = new GameObject("Single pepper hand pose").transform;
            batch.handAnchor.SetParent(session.player.view.transform, false); batch.handAnchor.localPosition = new Vector3(.25f, -.32f, .85f);
            var seeds = new List<PepperSeed>();
            int index = 0;
            foreach (var region in session.handling.regions)
                for (int n = 0; n < region.initialUnits; n++)
                {
                    seeds.Add(new PepperSeed { source = region.regionId,
                        position = new Vector3(-4.4f + index % 11 * .34f, .14f, -1.0f + index / 11 * .35f), yaw = (index % 3 - 1) * 12 });
                    index++;
                }
            batch.seeds = seeds.ToArray();
            ScatterSeeds(batch.seeds);
            batch.groupedViews = new GameObject[session.handling.regions.Length];
            for (int i = 0; i < batch.groupedViews.Length; i++)
            {
                Vector3 center = Vector3.zero; int count = 0;
                foreach (var seed in batch.seeds) if (seed.source == session.handling.regions[i].regionId) { center += seed.position; count++; }
                var group = GameObject.CreatePrimitive(PrimitiveType.Sphere); group.name = "Grouped distant supply " + i;
                group.transform.SetParent(batch.transform); group.transform.position = center / count + Vector3.down * .05f;
                group.transform.localScale = new Vector3(1, .12f, .32f); group.GetComponent<Renderer>().sharedMaterial = red;
                UnityEngine.Object.DestroyImmediate(group.GetComponent<Collider>()); group.SetActive(false); batch.groupedViews[i] = group;
            }
            // Preserve the original collider ID as the base; add four actual walls around exposed contents.
            var portable = session.handling.crate.portable;
            portable.handlingSize = portable.shape.size; portable.handlingCenter = portable.shape.center;
            portable.shape.center = new Vector3(0, .065f, 0); portable.shape.size = new Vector3(.97f, .05f, .72f);
            var sides = new BoxCollider[4];
            for (int i = 0; i < 4; i++)
            {
                sides[i] = portable.gameObject.AddComponent<BoxCollider>(); sides[i].sharedMaterial = contact;
                sides[i].center = i < 2 ? new Vector3(i == 0 ? -.46f : .46f, .29f, 0) : new Vector3(0, .29f, i == 2 ? -.335f : .335f);
                sides[i].size = i < 2 ? new Vector3(.05f, .5f, .72f) : new Vector3(.92f, .5f, .05f);
            }
            portable.additionalShapes = sides; session.handling.crate.physicalContents = true;
            AlignCrate(session.handling.crate);
            session.hud.controlsText.text = "WASD / Arrows  Move     Shift  Sprint     Space  Jump     Mouse  Look\n" +
                "Right click  Grab one pepper / object; click again to release\n" +
                "Hold left mouse with crate  Gather highlighted set into crate\n" +
                "Hold / release left mouse with one object  Charge / throw\n" +
                "E  Set down / put pepper in crate / tip at intake / collect / hand off\n" +
                "F  Pour the crate toward your aim (misses stay recoverable)\n" +
                "G  Release     Z / X  Optional rotation     Esc  Pause\n\n" +
                "F1  This reference     R  Recover objects and stray peppers (keeps food)\n" +
                "F8  Restart food test (clears stored food too)";
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Pepper batch save failed.");
            AssetDatabase.SaveAssets();
        }
    }
}
