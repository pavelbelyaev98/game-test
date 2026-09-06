using System.Linq;
using JustAFewPeppers.Editor;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JustAFewPeppers.Tests
{
    public class FoundationAssetTests
    {
        [Test]
        public void SavedSceneHasCompleteReferencesAndOnlyV4BuildEntry()
        {
            var scene = EditorSceneManager.OpenScene(FoundationSceneBuilder.ScenePath);
            var objects = scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<Transform>(true)).ToArray();
            foreach (var item in objects)
                Assert.That(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(item.gameObject), Is.Zero, item.name);
            foreach (var renderer in objects.SelectMany(item => item.GetComponents<Renderer>()))
            foreach (var material in renderer.sharedMaterials)
            {
                Assert.That(material, Is.Not.Null, renderer.name);
                Assert.That(material.shader, Is.Not.Null, renderer.name);
            }
            var session = Object.FindAnyObjectByType<YardSession>();
            Assert.That(session, Is.Not.Null);
            Assert.That(session.inputActions, Is.Not.Null);
            Assert.That(session.uiInput, Is.Not.Null);
            Assert.That(session.player.body, Is.Not.Null);
            Assert.That(session.player.view, Is.Not.Null);
            Assert.That(session.targeting.view, Is.SameAs(session.player.view));
            Assert.That(session.hud.events, Is.Not.Null);
            Assert.That(session.hud.resumeButton, Is.Not.Null);
            Assert.That(session.hud.resetButton, Is.Not.Null);
            Assert.That(session.hud.quitButton, Is.Not.Null);
            var controls = session.hud.transform.Find("Controls").GetComponent<Text>();
            Assert.That(controls.text, Does.Contain("Shift  Sprint").And.Contain("Space  Jump"));
            foreach (string action in new[] { "Gameplay/Sprint", "Gameplay/Jump" })
                Assert.That(session.inputActions.FindAction(action, true).bindings.Count, Is.GreaterThan(0));
            foreach (string name in new[] { "Mound placeholder", "Appliance placeholder", "Feed rim" })
            {
                var prop = GameObject.Find(name);
                Assert.That(prop.GetComponent<MeshCollider>().sharedMesh, Is.SameAs(prop.GetComponent<MeshFilter>().sharedMesh), name);
            }
            Assert.That(Object.FindObjectsByType<YardTarget>().Length, Is.EqualTo(4));
            Assert.That(Object.FindObjectsByType<AudioListener>().Length, Is.EqualTo(1));
            Assert.That(EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path), Is.EqualTo(new[] { FoundationSceneBuilder.ScenePath }));
            var settings = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
            Assert.That(settings.FindProperty("activeInputHandler").intValue, Is.EqualTo(1));
        }

        [Test]
        public void BroadTargetingAllowsNearMissButRespectsWallsAndRange()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var camera = new GameObject("Camera").AddComponent<Camera>();
            var targeting = camera.gameObject.AddComponent<YardTargeting>();
            targeting.view = camera;
            var targetObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            targetObject.transform.position = new Vector3(.55f, 0, 2);
            targetObject.transform.localScale = Vector3.one * .8f;
            var target = targetObject.AddComponent<YardTarget>();
            Physics.SyncTransforms();
            Assert.That(Physics.Raycast(Vector3.zero, Vector3.forward, 3), Is.False, "The center ray deliberately misses.");
            targeting.Refresh();
            Assert.That(targeting.Current, Is.SameAs(target));
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.position = new Vector3(0, 0, 1);
            wall.transform.localScale = new Vector3(3, 3, .1f);
            Physics.SyncTransforms();
            targeting.Refresh();
            Assert.That(targeting.Current, Is.Null, "Solid scenery blocks the target.");
            Object.DestroyImmediate(wall);
            targetObject.transform.position = new Vector3(0, 0, 5);
            Physics.SyncTransforms();
            targeting.Refresh();
            Assert.That(targeting.Current, Is.Null, "Distant targets cannot be selected.");
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }
    }
}
