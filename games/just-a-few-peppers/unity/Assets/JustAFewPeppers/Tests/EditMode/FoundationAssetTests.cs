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
            var controls = session.hud.controlsText;
            Assert.That(controls.transform.IsChildOf(session.hud.helpPanel.transform), Is.True);
            Assert.That(session.hud.guidanceText, Is.Not.Null);
            Assert.That(session.hud.sensitivitySlider, Is.Not.Null);
            Assert.That(session.hud.sensitivitySlider.minValue, Is.EqualTo(.25f));
            Assert.That(session.hud.sensitivitySlider.maxValue, Is.EqualTo(2.5f));
            Assert.That(session.hud.sensitivitySlider.navigation.selectOnDown, Is.SameAs(session.hud.quitButton));
            Assert.That(controls.text, Does.Contain("Shift  Sprint").And.Contain("Space  Jump"));
            foreach (string action in new[] { "Gameplay/Sprint", "Gameplay/Jump" })
                Assert.That(session.inputActions.FindAction(action, true).bindings.Count, Is.GreaterThan(0));
            foreach (string name in new[] { "Appliance placeholder", "Feed rim" })
            {
                var prop = GameObject.Find(name);
                Assert.That(prop.GetComponent<MeshCollider>().sharedMesh, Is.SameAs(prop.GetComponent<MeshFilter>().sharedMesh), name);
            }
            Assert.That(Object.FindObjectsByType<YardTarget>().Length, Is.EqualTo(10));
            Assert.That(Object.FindObjectsByType<AudioListener>().Length, Is.EqualTo(1));
            Assert.That(session.hud.helpPanel.activeSelf, Is.False);
            Assert.That(session.hud.guidanceText.transform.IsChildOf(session.hud.helpPanel.transform), Is.True);
            Assert.That(session.inputActions.FindAction("System/Help", true).bindings[0].path, Is.EqualTo("<Keyboard>/f1"));
            Assert.That(session.inputActions.FindAction("Gameplay/Grab", true).bindings[0].path, Is.EqualTo("<Mouse>/rightButton"));
            Assert.That(session.inputActions.FindAction("Gameplay/Use", true).bindings[0].path, Is.EqualTo("<Mouse>/leftButton"));
            var board = GameObject.Find("Sloped play board").GetComponent<Collider>();
            Assert.That(board.attachedRigidbody, Is.Null);
            foreach (var region in session.handling.regions)
                Assert.That(board.bounds.Intersects(region.volume.GetComponent<Collider>().bounds), Is.False, "Play board must not obstruct " + region.regionId);
            var handling = session.handling;
            Assert.That(handling.looseProps.props.Select(p => p.propId).Distinct().Count(), Is.EqualTo(4));
            Assert.That(Object.FindObjectsByType<Rigidbody>().Length, Is.EqualTo(6));
            foreach (var prop in handling.looseProps.props)
            {
                Assert.That(prop.portable.CollisionShape.attachedRigidbody, Is.SameAs(prop.portable.body));
                Assert.That(prop.target.gameObject, Is.SameAs(prop.gameObject));
                Assert.That(prop.portable.recoveryPoint, Is.Not.Null);
                Assert.That(prop.portable.preview, Is.Null);
            }
            Assert.That(handling, Is.Not.Null);
            Assert.That(handling.regions.Length, Is.EqualTo(9));
            Assert.That(handling.regions.Select(r => r.regionId).Distinct().Count(), Is.EqualTo(9));
            Assert.That(handling.regions.Sum(r => r.initialUnits), Is.EqualTo(107));
            foreach (var region in handling.regions)
                Assert.That(region.volume.GetComponent<MeshCollider>().sharedMesh, Is.SameAs(region.volume.GetComponent<MeshFilter>().sharedMesh));
            Assert.That(handling.crate.contents.Length, Is.EqualTo(12));
            Assert.That(handling.crate.carryAnchor.parent, Is.SameAs(session.player.view.transform));
            Assert.That(handling.presentation.flyingClumps.Length, Is.EqualTo(3));
            Assert.That(handling.presentation.scoopClip.length, Is.GreaterThan(0));
            Assert.That(handling.presentation.crateClip.length, Is.GreaterThan(0));
            Assert.That(handling.presentation.softCue.length, Is.GreaterThan(0));
            var station = handling.station;
            Assert.That(station, Is.Not.Null);
            Assert.That(station.inputCapacity, Is.EqualTo(12));
            Assert.That(station.outputCapacity, Is.EqualTo(12));
            Assert.That(station.intakeTarget.GetComponentsInChildren<Collider>().Length, Is.GreaterThan(0));
            Assert.That(station.intakeTarget.transform, Is.SameAs(station.intake));
            Assert.That(station.queuedPeppers.Length, Is.EqualTo(12));
            Assert.That(station.jars.Length, Is.EqualTo(4));
            Assert.That(station.jarFood.All(t => t != null), Is.True);
            Assert.That(station.stageMarkers.Length, Is.EqualTo(4));
            Assert.That(station.stageLabel, Is.Not.Null);
            Assert.That(station.statusText, Is.Not.Null);
            Assert.That(station.completionClip.length, Is.GreaterThan(0));
            Assert.That(handling.tipping.crate, Is.SameAs(handling.crate));
            Assert.That(handling.tipping.destination, Is.SameAs(station.intake));
            Assert.That(handling.tipping.flyingPeppers.Length, Is.EqualTo(9));
            Assert.That(handling.tipping.GetComponentsInChildren<Rigidbody>(true).Length, Is.Zero);
            var finished = handling.finished;
            Assert.That(finished.carrier.capacity, Is.EqualTo(12));
            Assert.That(Object.FindObjectsByType<FinishedCarrierView>().Length, Is.EqualTo(1));
            Assert.That(finished.outputTarget.GetComponent<Collider>(), Is.Not.Null);
            Assert.That(finished.rackTarget.displayName, Is.EqualTo("Finished Food Handoff Rack"));
            Assert.That(finished.rackTarget.GetComponent<Collider>().bounds.size.x, Is.GreaterThan(2));
            Assert.That(finished.carrier.food.jars.Length, Is.EqualTo(4));
            Assert.That(finished.storedFood.jars.Length * finished.storedFood.unitsPerJar, Is.GreaterThanOrEqualTo(107));
            Assert.That(finished.storedFood.GetComponentsInChildren<Collider>(true), Is.Empty);
            Assert.That(finished.carrier.food.GetComponentsInChildren<Collider>(true), Is.Empty);
            Assert.That(finished.carrier.portable.body, Is.SameAs(finished.carrier.GetComponent<Rigidbody>()));
            Assert.That(finished.carrier.portable.shape.enabled, Is.False, "The fixed receiving fixture owns dock collision.");
            Assert.That(finished.carrier.portable.shape.sharedMaterial.bounciness, Is.Zero);
            Assert.That(finished.carrier.portable.recoveryPoint, Is.Not.Null);
            Assert.That(finished.carrier.dock, Is.Not.Null);
            Assert.That(finished.transferClip, Is.Not.Null);
            Assert.That(finished.statusText, Is.Not.Null);
            Assert.That(controls.text, Does.Contain("tip at intake / collect / hand off"));
            Assert.That(handling.peppers.seeds.Length, Is.EqualTo(107));
            Assert.That(handling.peppers.pepperPrefab.shape.direction, Is.EqualTo(2));
            Assert.That(handling.crate.physicalContents, Is.True);
            Assert.That(handling.crate.portable.additionalShapes.Length, Is.EqualTo(4));
            Assert.That(handling.GetComponentsInChildren<Rigidbody>().Length, Is.Zero);
            Assert.That(handling.crate.GetComponentsInChildren<Rigidbody>().Length, Is.EqualTo(1));
            var portable = handling.crate.portable;
            Assert.That(portable.shape.attachedRigidbody, Is.SameAs(portable.body));
            Assert.That(portable.shape.sharedMaterial.bounciness, Is.Zero);
            Assert.That(portable.recoveryPoint, Is.Not.Null);
            Assert.That(portable.preview, Is.Not.Null);
            Assert.That(portable.contactClip, Is.Not.Null);
            Assert.That(GameObject.Find("Placement worktop"), Is.Not.Null);
            Assert.That(GameObject.Find("Stable low support"), Is.Not.Null);
            Assert.That(GameObject.Find("Crate parking mat"), Is.Null);
            Assert.That(session.hud.restartPrototypeButton, Is.Not.Null);
            foreach (string action in new[] { "Use", "Interact", "RestartPrototype", "Drop", "Rotate" })
                Assert.That(session.inputActions.FindAction("Gameplay/" + action, true).bindings.Count, Is.GreaterThan(0));
            Assert.That(session.inputActions.FindAction("Gameplay/ScoopMode"), Is.Null);
            Assert.That(session.inputActions.FindActionMap("Gameplay").bindings.Any(b => b.path == "<Keyboard>/t"), Is.False);
            Assert.That(controls.text, Does.Contain("Hold left mouse").And.Not.Contain("toggle"));
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
