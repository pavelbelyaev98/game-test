using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static JustAFewPeppers.Editor.FoundationSceneBuilder;

namespace JustAFewPeppers.Editor
{
    public static class LoosePropsAuthoring
    {
        public static void Apply()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            if (session.handling.looseProps != null) throw new InvalidOperationException("Loose props already authored; edit the saved scene.");
            var handling = session.handling.gameObject.AddComponent<LoosePropHandling>();
            session.handling.looseProps = handling;
            handling.carryAnchor = session.handling.crate.carryAnchor;
            var root = new GameObject("Loose yard props").transform;
            var metal = Material("Basin blue", new Color(.24f, .53f, .61f));
            var wood = Material("Loose prop wood", new Color(.59f, .38f, .19f));
            var rubber = Material("Ball ochre", new Color(.94f, .64f, .19f));
            var contact = session.handling.crate.portable.shape.sharedMaterial;
            var bounce = new PhysicsMaterial("Ball contact") { dynamicFriction = .55f, staticFriction = .6f,
                bounciness = .48f, frictionCombine = PhysicsMaterialCombine.Average, bounceCombine = PhysicsMaterialCombine.Maximum };
            AssetDatabase.CreateAsset(bounce, "Assets/JustAFewPeppers/Content/Ball contact.physicMaterial");
            var basin = Prop("loose-basin", "Basin", new Vector3(-7, .012f, -1.7f), new Vector3(.72f, .22f, .62f), root, session, 1.5f, 4.5f);
            Tray(basin, metal, .72f, .22f, .62f);
            var stool = Prop("loose-stool", "Stool", new Vector3(-5.6f, .012f, -1.7f), new Vector3(.8f, .57f, .68f), root, session, 3, 3.6f);
            var shapes = new List<Collider>();
            stool.portable.shape = Part(stool, "Seat", new Vector3(0, .52f, 0), new Vector3(.8f, .1f, .68f), wood);
            foreach (float x in new[] { -.31f, .31f })
            foreach (float z in new[] { -.25f, .25f })
                shapes.Add(Part(stool, "Leg", new Vector3(x, .235f, z), new Vector3(.12f, .47f, .12f), wood));
            stool.portable.additionalShapes = shapes.ToArray();
            var crate = Prop("loose-empty-crate", "Empty crate", new Vector3(-7, .012f, -.2f), new Vector3(.65f, .4f, .48f), root, session, 2, 4);
            Tray(crate, wood, .65f, .4f, .48f);
            var ball = Prop("loose-ball", "Ball", new Vector3(-5.6f, .012f, -.2f), Vector3.zero, root, session, .45f, 6);
            var visual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            visual.name = "Rubber ball";
            visual.transform.SetParent(ball.transform, false);
            visual.transform.localPosition = Vector3.up * .18f;
            visual.transform.localScale = Vector3.one * .36f;
            visual.GetComponent<Renderer>().sharedMaterial = rubber;
            UnityEngine.Object.DestroyImmediate(visual.GetComponent<Collider>());
            ball.portable.roundShape = ball.gameObject.AddComponent<SphereCollider>();
            ball.portable.roundShape.center = Vector3.up * .18f;
            ball.portable.roundShape.radius = .18f;
            ball.portable.body.angularDamping = .2f;
            ball.portable.body.centerOfMass = Vector3.up * .18f;
            handling.props = new[] { basin, stool, crate, ball };
            foreach (var prop in handling.props)
            {
                prop.portable.CollisionShape.sharedMaterial = prop == ball ? bounce : contact;
                foreach (var collider in prop.portable.additionalShapes) collider.sharedMaterial = contact;
            }
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Loose props save failed.");
            AssetDatabase.SaveAssets();
            Debug.Log("LOOSE_PROPS_AUTHORED four reusable bodies; six portable bodies including food carriers. " + ScenePath);
        }

        public static void TuneBall()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            session.hud.resetButton.GetComponentInChildren<Text>().text = "Return to gate / recover objects";
            foreach (var prop in session.handling.looseProps.props)
            {
                if (prop.propId == "loose-basin")
                {
                    prop.portable.handlingSize = new Vector3(.72f, .22f, .62f);
                    Resize(prop, "Base", prop.portable.shape, new Vector3(0, .025f, 0), new Vector3(.72f, .05f, .62f));
                    Resize(prop, "Left rim", (BoxCollider)prop.portable.additionalShapes[0], new Vector3(-.335f, .11f, 0), new Vector3(.05f, .22f, .62f));
                    Resize(prop, "Right rim", (BoxCollider)prop.portable.additionalShapes[1], new Vector3(.335f, .11f, 0), new Vector3(.05f, .22f, .62f));
                    Resize(prop, "Front rim", (BoxCollider)prop.portable.additionalShapes[2], new Vector3(0, .11f, -.285f), new Vector3(.72f, .22f, .05f));
                    Resize(prop, "Back rim", (BoxCollider)prop.portable.additionalShapes[3], new Vector3(0, .11f, .285f), new Vector3(.72f, .22f, .05f));
                }
                if (prop.propId != "loose-ball") continue;
                prop.transform.Find("Rubber ball").localPosition = Vector3.up * .18f;
                prop.transform.Find("Rubber ball").localScale = Vector3.one * .36f;
                prop.portable.roundShape.center = Vector3.up * .18f;
                prop.portable.roundShape.radius = .18f;
                prop.portable.body.centerOfMass = Vector3.up * .18f;
            }
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Ball tuning save failed.");
        }

        static void Resize(LooseProp prop, string name, BoxCollider collider, Vector3 center, Vector3 size)
        {
            prop.transform.Find(name).localPosition = center;
            prop.transform.Find(name).localScale = size;
            collider.center = center; collider.size = size;
        }

        static LooseProp Prop(string id, string name, Vector3 position, Vector3 size, Transform root, YardSession session, float mass, float toss)
        {
            var go = new GameObject(name);
            go.transform.SetParent(root, false);
            go.transform.position = position;
            var prop = go.AddComponent<LooseProp>();
            prop.propId = id;
            prop.tossSpeed = toss;
            prop.target = go.AddComponent<YardTarget>();
            prop.target.displayName = name;
            prop.target.description = "E  Pick up";
            var portable = go.AddComponent<PortableBody>();
            prop.portable = portable;
            portable.handlingSize = size;
            portable.handlingCenter = Vector3.up * size.y * .5f;
            portable.recoveryPoint = new GameObject(name + " recovery fallback").transform;
            portable.recoveryPoint.SetParent(root, false);
            portable.recoveryPoint.position = position;
            portable.body = go.AddComponent<Rigidbody>();
            portable.body.mass = mass;
            portable.body.linearDamping = .15f;
            portable.body.angularDamping = 1.5f;
            portable.body.solverIterations = 10;
            portable.body.solverVelocityIterations = 4;
            portable.body.maxDepenetrationVelocity = 2;
            portable.body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            portable.body.interpolation = RigidbodyInterpolation.Interpolate;
            portable.contactAudio = go.AddComponent<AudioSource>();
            portable.contactAudio.playOnAwake = false;
            portable.contactAudio.spatialBlend = 1;
            portable.contactAudio.maxDistance = 10;
            portable.contactClip = session.handling.presentation.crateClip;
            return prop;
        }

        static void Tray(LooseProp prop, Material material, float width, float height, float depth)
        {
            prop.portable.shape = Part(prop, "Base", new Vector3(0, .025f, 0), new Vector3(width, .05f, depth), material);
            prop.portable.additionalShapes = new Collider[] {
                Part(prop, "Left rim", new Vector3(-width * .5f + .025f, height * .5f, 0), new Vector3(.05f, height, depth), material),
                Part(prop, "Right rim", new Vector3(width * .5f - .025f, height * .5f, 0), new Vector3(.05f, height, depth), material),
                Part(prop, "Front rim", new Vector3(0, height * .5f, -depth * .5f + .025f), new Vector3(width, height, .05f), material),
                Part(prop, "Back rim", new Vector3(0, height * .5f, depth * .5f - .025f), new Vector3(width, height, .05f), material) };
            prop.portable.body.centerOfMass = Vector3.up * height * .35f;
        }

        static BoxCollider Part(LooseProp prop, string name, Vector3 center, Vector3 size, Material material)
        {
            var go = Box(name, center, size, material, prop.transform);
            UnityEngine.Object.DestroyImmediate(go.GetComponent<Collider>());
            // All compound colliders share the root's unscaled local coordinates.
            var collider = prop.gameObject.AddComponent<BoxCollider>();
            collider.center = center;
            collider.size = size;
            return collider;
        }

        static Material Material(string name, Color color)
        {
            var material = new Material(Shader.Find("Standard")) { name = name, color = color };
            material.SetFloat("_Glossiness", .22f);
            AssetDatabase.CreateAsset(material, "Assets/JustAFewPeppers/Content/" + name + ".mat");
            return material;
        }
    }
}
