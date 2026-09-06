using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static JustAFewPeppers.Editor.FoundationSceneBuilder;

namespace JustAFewPeppers.Editor
{
    public static class FinishedFoodAuthoring
    {
        const string Content = "Assets/JustAFewPeppers/Content/";

        [MenuItem("Just a few peppers/Apply 1_04 finished food handoff")]
        public static void Apply()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            var handling = session.handling;
            if (handling.finished != null) throw new InvalidOperationException("Finished handling is already authored; edit the saved scene instead.");
            var finished = new GameObject("Finished food handling").AddComponent<FinishedFoodHandling>();
            handling.finished = finished;
            var metal = AssetDatabase.LoadAssetAtPath<Material>(Content + "Metal.mat");
            var wood = AssetDatabase.LoadAssetAtPath<Material>(Content + "Rack.mat");
            var marker = AssetDatabase.LoadAssetAtPath<Material>(Content + "Target marker.mat");
            var prepared = Material("Prepared peppers", new Color(.48f, .105f, .045f));
            var bright = AssetDatabase.LoadAssetAtPath<Material>(Content + "Ripe pepper.mat");
            var station = handling.station;

            // Preserve the existing output geometry and jar references, expanding the receiving fixture.
            var outputSpace = station.transform.Find("Output space");
            outputSpace.localPosition = new Vector3(1.45f, 1.04f, 0);
            outputSpace.localScale = new Vector3(1.05f, .12f, 1.1f);
            var output = new GameObject("Finished food receiving tray").transform;
            output.SetParent(station.transform, false);
            output.localPosition = new Vector3(1.45f, 1.112f, 0);
            finished.outputTarget = output.gameObject.AddComponent<YardTarget>();
            finished.outputTarget.displayName = "Finished food receiving tray";
            finished.outputTarget.description = "E  Collect ready food in the reusable carrier";
            // A permanent receiving fixture reserves room for automatic empty-carrier return.
            var receivingVolume = output.gameObject.AddComponent<BoxCollider>();
            receivingVolume.center = new Vector3(0, .2f, 0);
            receivingVolume.size = new Vector3(.96f, .4f, .86f);
            finished.outputTarget.marker = Box("Receiving marker", new Vector3(0, -.01f, -.55f),
                new Vector3(.85f, .1f, .03f), marker, output, false).GetComponent<Renderer>();
            Label("E  COLLECT FINISHED FOOD", output, new Vector3(0, -.08f, -.57f), .0065f);
            foreach (float x in new[] { -.48f, .48f })
                Box("Receiving guide", new Vector3(x, .2f, 0), new Vector3(.035f, .4f, .86f), metal, output, false);

            for (int i = 0; i < station.jars.Length; i++)
            {
                station.jars[i].transform.SetParent(output, false);
                station.jars[i].transform.localPosition = JarPosition(i);
                station.jarFood[i].GetComponent<Renderer>().sharedMaterial = prepared;
                for (int strip = 0; strip < 3; strip++)
                    Shape(PrimitiveType.Sphere, "Prepared pepper strip", station.jarFood[i],
                        new Vector3((strip - 1) * .35f, .05f, -.35f), new Vector3(.25f, 1.35f, .4f), bright, false);
            }

            var carrier = new GameObject("Reusable finished carrier").AddComponent<FinishedCarrierView>();
            finished.carrier = carrier;
            carrier.dock = new GameObject("Finished carrier dock pose").transform;
            carrier.dock.SetParent(output, false);
            carrier.transform.SetPositionAndRotation(carrier.dock.position, carrier.dock.rotation);
            carrier.carryAnchor = handling.crate.carryAnchor;
            carrier.target = carrier.gameObject.AddComponent<YardTarget>();
            carrier.target.displayName = "Finished-food carrier";
            carrier.target.description = "E  Grab  |  Place or drop freely; hand off at the rack";
            Box("Carrier base", new Vector3(0, .04f, 0), new Vector3(.94f, .08f, .84f), wood, carrier.transform, false);
            foreach (float x in new[] { -.45f, .45f })
            {
                Box("Carrier side", new Vector3(x, .15f, 0), new Vector3(.04f, .22f, .84f), wood, carrier.transform, false);
                Box("Carrier handle", new Vector3(x, .34f, 0), new Vector3(.045f, .06f, .46f), metal, carrier.transform, false);
            }
            carrier.target.marker = Box("Carrier front", new Vector3(0, .14f, -.4f), new Vector3(.86f, .16f, .035f),
                wood, carrier.transform, false).GetComponent<Renderer>();
            carrier.food = MakeGroup("Packed food", carrier.transform, station.jars[0], 4, false);
            var body = carrier.gameObject.AddComponent<PortableBody>();
            carrier.portable = body;
            body.shape = carrier.gameObject.AddComponent<BoxCollider>();
            body.shape.center = new Vector3(0, .24f, 0);
            body.shape.size = new Vector3(.96f, .48f, .86f);
            body.shape.enabled = false;
            body.shape.sharedMaterial = handling.crate.portable.shape.sharedMaterial;
            body.body = carrier.gameObject.AddComponent<Rigidbody>();
            body.body.mass = 5;
            body.body.isKinematic = true;
            body.body.linearDamping = .15f;
            body.body.angularDamping = 1.5f;
            body.body.solverIterations = 10;
            body.body.solverVelocityIterations = 4;
            body.body.maxDepenetrationVelocity = 2;
            body.body.centerOfMass = new Vector3(0, .16f, 0);
            body.recoveryPoint = new GameObject("Finished carrier recovery fallback").transform;
            body.recoveryPoint.position = new Vector3(5.8f, .012f, 2.8f);
            body.contactAudio = carrier.gameObject.AddComponent<AudioSource>();
            body.contactAudio.playOnAwake = false;
            body.contactAudio.spatialBlend = 1;
            body.contactClip = handling.presentation.crateClip;

            finished.rackTarget = GameObject.Find("Storage rack").GetComponent<YardTarget>();
            finished.rackTarget.displayName = "Finished Food Handoff Rack";
            finished.rackTarget.description = "E  Hand off finished food  |  Empty carrier returns automatically";
            // Keep the foundation label and target component identities; update their authored guidance.
            finished.rackLabel = finished.rackTarget.transform.Find("04  STORE").GetComponent<TextMesh>();
            MountRackLabel(finished);
            var rackArea = finished.rackTarget.gameObject.AddComponent<BoxCollider>();
            rackArea.center = new Vector3(0, 1, 0);
            rackArea.size = new Vector3(2.2f, 1.85f, .98f);
            finished.storedFood = MakeGroup("Winter food stored display", finished.rackTarget.transform, station.jars[0], 36, true);
            finished.audioSource = finished.gameObject.AddComponent<AudioSource>();
            finished.audioSource.playOnAwake = false;
            finished.transferClip = handling.presentation.crateClip;
            finished.storedFood.Render(0);
            carrier.food.Render(0);
            finished.rackLabel.text = "FINISHED FOOD HANDOFF RACK\nOne handoff - jars go to the household";

            var hud = session.hud;
            hud.transform.Find("Scope").GetComponent<Text>().text = "Scoop / Process / Receive / Hand off winter food";
            finished.statusText = Text("Winter food status", hud.transform, "Winter food stored  0 / 107    Finished carrier  0 / 12",
                new Vector2(0, 258), new Vector2(1380, 36), 21);
            hud.transform.Find("Controls").GetComponent<Text>().text =
                "WASD / Arrows  Move     Shift  Sprint     Space  Jump     Mouse  Look\n" +
                "E  Grab / place / tip at intake / collect / hand off     Z / X  Rotate     G  Drop\n" +
                "Hold left mouse  Scoop     Esc  Pause     R  Recover     F8  Restart food test";
            hud.pausePanel.transform.Find("Pause help").GetComponent<Text>().text =
                "Scoop, E at intake, collect finished food, E at the handoff rack.\nE places, Z/X rotates, G drops. R keeps all food; F8 clears the test.";
            hud.resetButton.GetComponentInChildren<Text>().text = "Return to gate / recover carriers";
            hud.restartPrototypeButton.GetComponentInChildren<Text>().text = "Restart food test (clears stored food too)";
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Could not save finished food scene.");
            AssetDatabase.SaveAssets();
            Debug.Log("FINISHED_FOOD_AUTHORED " + ScenePath);
        }

        static Vector3 JarPosition(int i) => new Vector3((i % 2 == 0 ? -1 : 1) * .19f, .08f, (i / 2 == 0 ? -1 : 1) * .21f);

        static void MountRackLabel(FinishedFoodHandling finished)
        {
            finished.rackLabel.characterSize = .0065f;
            finished.rackLabel.transform.localPosition = new Vector3(0, 1.46f, -.54f);
            finished.rackTarget.marker.transform.localPosition = new Vector3(0, 1.46f, -.51f);
            finished.rackTarget.marker.transform.localScale = new Vector3(1.95f, .26f, .03f);
        }

        [MenuItem("Just a few peppers/Tune 1_04 food display visibility")]
        public static void TunePresentation()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var finished = UnityEngine.Object.FindAnyObjectByType<FinishedFoodHandling>();
            MountRackLabel(finished);
            finished.rackLabel.text = "FINISHED FOOD HANDOFF RACK\nOne handoff - jars go to the household";
            for (int i = 0; i < finished.storedFood.jars.Length; i++)
            {
                var position = finished.storedFood.jars[i].transform.localPosition;
                position.y = 1.7f - i / 12 * .7f;
                finished.storedFood.jars[i].transform.localPosition = position;
            }
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Could not save food display tuning.");
            Debug.Log("FINISHED_FOOD_PRESENTATION_TUNED");
        }

        static FoodGroupView MakeGroup(string name, Transform parent, GameObject template, int count, bool shelf)
        {
            var group = new GameObject(name).AddComponent<FoodGroupView>();
            group.transform.SetParent(parent, false);
            group.jars = new GameObject[count];
            group.food = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                var jar = UnityEngine.Object.Instantiate(template, group.transform);
                jar.name = "Packed jar " + (i + 1);
                jar.transform.localPosition = shelf ? new Vector3((i % 6 - 2.5f) * .32f,
                    1.7f - i / 12 * .7f, ((i / 6) % 2 == 0 ? -1 : 1) * .21f) : JarPosition(i);
                group.jars[i] = jar;
                group.food[i] = jar.transform.Find("Roasted pepper fill");
                jar.SetActive(false);
            }
            return group;
        }
    }
}
