using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static JustAFewPeppers.Editor.FoundationSceneBuilder;

namespace JustAFewPeppers.Editor
{
    public static class MachineOperationAuthoring
    {
        public static void Apply()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            var handling = session.handling;
            if (handling.machine != null) throw new InvalidOperationException("Direct machine operation already authored; edit the saved work.");
            var station = handling.station;
            var metal = AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Metal.mat");
            var wood = AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Loose prop wood.mat");
            var food = AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Prepared peppers.mat");
            var marker = AssetDatabase.LoadAssetAtPath<Material>("Assets/JustAFewPeppers/Content/Target marker.mat");
            var machine = station.gameObject.AddComponent<MachineOperation>(); handling.machine = machine;
            machine.audioSource = station.gameObject.AddComponent<AudioSource>(); machine.audioSource.playOnAwake = false;
            machine.contactClip = handling.presentation.crateClip;

            // Protected sliding frame and substantial side grip; the pouring corridor stays clear.
            var rack = new GameObject("Manually sliding batch rack").transform;
            rack.SetParent(station.transform, false); rack.localPosition = new Vector3(-.35f, 1.24f, -1.2f); machine.rack = rack;
            Box("Batch pushing edge", Vector3.zero, new Vector3(1.35f, .11f, .075f), metal, rack, false);
            Box("Handle arm", new Vector3(-.8f, 0, 0), new Vector3(.45f, .08f, .08f), metal, rack, false);
            Box("Wooden operating grip", new Vector3(-1f, 0, 0), new Vector3(.14f, .14f, .48f), wood, rack, false);
            foreach (float x in new[] { -1.0f, .63f })
                Box("Protected rack rail", new Vector3(x, -.08f, .35f), new Vector3(.055f, .07f, 1.0f), metal, rack.parent, false)
                    .transform.localPosition += rack.localPosition;
            var control = new GameObject("Batch rack operation target").transform;
            control.SetParent(station.transform, false); control.localPosition = rack.localPosition + Vector3.left;
            machine.operationTarget = control.gameObject.AddComponent<YardTarget>();
            machine.operationTarget.displayName = "Batch rack"; machine.operationTarget.description = "Hold left mouse and drag up";
            var hit = control.gameObject.AddComponent<BoxCollider>(); hit.size = new Vector3(.55f, .22f, .55f);
            machine.operationTarget.marker = Box("Rack label plate", new Vector3(0, -.13f, -.25f), new Vector3(.68f, .12f, .035f), marker, control, false).GetComponent<Renderer>();
            Label("SLIDE BATCH RACK", control, new Vector3(0, -.13f, -.275f), .004f);

            var output = handling.finished.outputTarget.transform;
            var guide = new GameObject("Prepared food grouping guide").transform;
            guide.SetParent(output, false); guide.localPosition = new Vector3(-.42f, .2f, 0); machine.guide = guide;
            machine.guideDistance = .8f;
            Box("Wide grouping blade", Vector3.zero, new Vector3(.04f, .17f, .68f), metal, guide, false);
            Box("Grouping grip", new Vector3(0, .1f, -.32f), new Vector3(.18f, .09f, .16f), wood, guide, false);
            machine.preparedFood = new Transform[12];
            for (int i = 0; i < machine.preparedFood.Length; i++)
            {
                var piece = Shape(PrimitiveType.Sphere, "Loose prepared pepper " + i, output,
                    new Vector3((i % 4 - 1.5f) * .18f, .13f + i % 2 * .015f, (i / 4 - 1) * .2f), new Vector3(.11f, .055f, .18f), food, false);
                piece.transform.localRotation = Quaternion.Euler(0, i * 47, 0); piece.SetActive(false); machine.preparedFood[i] = piece.transform;
            }
            handling.finished.outputTarget.description = "Hold left mouse and drag right to group prepared food";
            output.Find("E  COLLECT FINISHED FOOD").GetComponent<TextMesh>().text = "GROUP PREPARED FOOD";
            session.hud.controlsText.text = session.hud.controlsText.text
                .Replace("Hold / release left mouse with one object", "Hold / release left mouse with one pepper / prop")
                .Replace(" / collect / hand off", " / hand off") +
                "\nRack: hold left mouse + drag UP  (or hold E + W / S)\n" +
                "Output: hold left mouse + drag RIGHT  (or hold E + D / A)\nRelease stops a stroke; right click cancels it";
            session.hud.controlsText.fontSize = 18;
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Could not save direct operation scene.");
            AssetDatabase.SaveAssets();
        }
    }
}
