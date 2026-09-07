using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SomethingDownThere.Tests
{
    public sealed class MainGameSceneTests
    {
        [Test]
        public void AuthoredShellHasValidOwnersUrpMaterialsAndReachableSurfaceAnchors()
        {
            Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/MainGame.unity", OpenSceneMode.Additive);
            try
            {
                GameObject[] roots = scene.GetRootGameObjects();
                Assert.That(roots.Length, Is.EqualTo(1));
                Transform root = roots[0].transform;
                foreach (Transform item in root.GetComponentsInChildren<Transform>(true))
                    Assert.That(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(item.gameObject), Is.Zero, item.name);
                Assert.That(root.GetComponentsInChildren<TerrainVolume>().Length, Is.EqualTo(1));
                Assert.That(root.GetComponentsInChildren<FpsPlayer>().Length, Is.EqualTo(1));
                Assert.That(root.GetComponentsInChildren<FpsHud>().Length, Is.EqualTo(1));
                Assert.That(root.GetComponentsInChildren<Camera>().Length, Is.EqualTo(1));
                Assert.That(root.GetComponentsInChildren<MonoBehaviour>().Any(c => c.GetType().Name.StartsWith("Validation")), Is.False);
                foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>())
                    Assert.That(renderer.sharedMaterial.shader.name, Is.EqualTo("Universal Render Pipeline/Lit"), renderer.name);
                foreach (string name in new[] { "SellStation", "UpgradeStation", "RechargeZone", "ReturnAnchor" })
                {
                    Transform anchor = root.Find("Surface/" + name);
                    Assert.That(anchor, Is.Not.Null, name);
                    Assert.That(anchor.position.y, Is.InRange(0, 0.2f));
                    Assert.That(Vector3.Distance(anchor.position, new Vector3(0, 0, -12)), Is.LessThan(5), name);
                }
                Assert.That(root.Find("Scenery/Water").GetComponent<Collider>(), Is.Null);
                Assert.That(root.Find("Scenery/Water").GetComponent<Renderer>().bounds.min.z, Is.GreaterThanOrEqualTo(17));
                Assert.That(root.Find("Bedrock").GetComponentsInChildren<PermanentTerrainBoundary>().Length, Is.EqualTo(5));
                Assert.That(root.Find("Perimeter").GetComponentsInChildren<PermanentTerrainBoundary>().Length, Is.EqualTo(8));
                foreach (string side in new[] { "West", "East", "North", "South" })
                {
                    var wall = root.Find("Bedrock/" + side).GetComponent<Renderer>().bounds;
                    var rim = root.Find("Surface/" + side + " rim").GetComponent<Renderer>().bounds;
                    Assert.That(wall.max.y, Is.EqualTo(rim.min.y).Within(0.0001f),
                        side + ": coincident vertical wall/rim faces must meet without overlapping or leaving a gap.");
                }
                Assert.That(root.Find("Player").gameObject.layer, Is.EqualTo(LayerMask.NameToLayer("Ignore Raycast")));
            }
            finally { EditorSceneManager.CloseScene(scene, true); }
        }
    }
}
