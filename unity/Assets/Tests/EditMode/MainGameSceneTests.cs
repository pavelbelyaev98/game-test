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
                var recharge = root.GetComponentInChildren<SurfaceRecharge>();
                Assert.That(recharge, Is.Not.Null);
                Assert.That(recharge.Player, Is.SameAs(root.GetComponentInChildren<FpsPlayer>()));
                Assert.That(recharge.Player.SurfaceRecharge, Is.SameAs(recharge));
                Assert.That(recharge.Terrain, Is.SameAs(root.GetComponentInChildren<TerrainVolume>()));
                Assert.That(recharge.transform, Is.SameAs(root.Find("Surface/RechargeZone")));
                Assert.That(recharge.transform.position.z + recharge.Footprint.y * 0.5f, Is.LessThan(-12f),
                    "Recharge must stay on the permanent rim, outside excavatable soil.");
                Assert.That(root.GetComponentsInChildren<Camera>().Length, Is.EqualTo(1));
                var camera = root.GetComponentInChildren<Camera>();
                Assert.That(camera.clearFlags, Is.EqualTo(CameraClearFlags.Skybox));
                var sky = camera.GetComponent<Skybox>();
                Assert.That(sky, Is.Not.Null);
                Assert.That(sky.material, Is.Not.Null);
                Assert.That(sky.material.shader.name, Is.EqualTo("Something Down There/Sunny Sun Sky"));
                Assert.That(ShaderUtil.ShaderHasError(sky.material.shader), Is.False);
                Assert.That(sky.material.GetTexture("_SunMap"), Is.Not.Null);
                Assert.That(root.Find("Clouds"), Is.Null);
                Assert.That(root.Find("Scenery"), Is.Null, "The requested clear site has no decorative trees, river or rocks.");
                Assert.That(Vector4.Distance(sky.material.GetColor("_SkyColor"), camera.backgroundColor), Is.LessThan(.00001f),
                    "The authored sky must preserve the accepted camera background color.");
                var sunDirection = (Vector3)sky.material.GetVector("_SunDirection");
                Assert.That(Vector3.Dot(sunDirection.normalized, -root.Find("Sun").forward), Is.GreaterThan(.99999f));
                Assert.That(root.GetComponentsInChildren<MonoBehaviour>().Any(c => c.GetType().Name.StartsWith("Validation")), Is.False);
                Assert.That(root.GetComponentsInChildren<SellStation>().Single().transform, Is.SameAs(root.Find("Surface/SellStation")));
                Assert.That(root.GetComponentsInChildren<UpgradeStation>().Single().transform, Is.SameAs(root.Find("Surface/UpgradeStation")));
                foreach (var station in root.GetComponentsInChildren<StationTarget>())
                {
                    Assert.That(station.GetComponent<Collider>(), Is.Not.Null);
                    Assert.That(station.GetComponent<StationMotion>(), Is.Not.Null);
                    foreach (var mesh in station.GetComponentsInChildren<MeshFilter>())
                        StringAssert.StartsWith("Assets/Content/Stations/", AssetDatabase.GetAssetPath(mesh.sharedMesh), "Stations must use their approved Blender models.");
                    foreach (var renderer in station.GetComponentsInChildren<Renderer>())
                        Assert.That(renderer.sharedMaterial.GetTexture("_BaseMap"), Is.Not.Null, "Keep authored station textures.");
                }
                var terrainSettings = new SerializedObject(root.GetComponentInChildren<TerrainVolume>());
                var excavation = root.GetComponentInChildren<TerrainVolume>();
                Assert.That(excavation.Dimensions, Is.EqualTo(new Vector3Int(192, 256, 192)));
                Assert.That(excavation.CellSize, Is.EqualTo(.125f));
                Assert.That(excavation.SurfaceHeight, Is.Zero.Within(.0001f));
                Assert.That(root.Find("Bedrock/Floor").GetComponent<Collider>().bounds.max.y, Is.EqualTo(-32).Within(.0001f));
                var grass = root.GetComponentInChildren<SurfaceGrassRenderer>();
                Assert.That(grass, Is.Not.Null, "The main game must retain the approved moving grass.");
                var grassSettings = new SerializedObject(grass);
                foreach (string property in new[] { "nearMesh", "farMesh", "material" })
                    StringAssert.StartsWith("Assets/Content/GroundGrass/", AssetDatabase.GetAssetPath(
                        grassSettings.FindProperty(property).objectReferenceValue));
                var grassMesh = (Mesh)grassSettings.FindProperty("nearMesh").objectReferenceValue;
                Assert.That(grassMesh.bounds.size.y, Is.InRange(.35f, .5f), "Keep the requested taller authored grass.");
                Assert.That(grassMesh.GetIndexCount(0) / 3, Is.EqualTo(168), "The taller shape must retain the clump's geometry budget.");
                Assert.That(grassMesh.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.TexCoord1), Is.True,
                    "The imported mesh must retain per-blade wind phase and height.");
                var grassMaterial = (Material)grassSettings.FindProperty("material").objectReferenceValue;
                Assert.That(grassMaterial.enableInstancing, Is.True);
                Assert.That(ShaderUtil.ShaderHasError(grassMaterial.shader), Is.False);
                Assert.That(grassMaterial.GetTexture("_BaseMap"), Is.Not.Null);
                var daylight = root.GetComponentInChildren<ExcavationDaylight>();
                Assert.That(daylight, Is.Not.Null, "Excavation must attenuate ambient sky light in enclosed soil.");
                var daylightShader = new SerializedObject(daylight).FindProperty("litShader").objectReferenceValue as Shader;
                Assert.That(daylightShader, Is.Not.Null, "Keep the runtime material shader referenced in Windows builds.");
                Assert.That(ShaderUtil.ShaderHasError(daylightShader), Is.False);
                var ground = (Material)terrainSettings.FindProperty("soilMaterial").objectReferenceValue;
                Assert.That(ground.shader.name, Is.EqualTo("Something Down There/Ground Triplanar"));
                Assert.That(ShaderUtil.ShaderHasError(ground.shader), Is.False);
                foreach (string kind in new[] { "Soil", "Turf" })
                foreach (string channel in new[] { "Albedo", "Normal", "Roughness" })
                    Assert.That(ground.GetTexture("_" + kind + channel), Is.Not.Null, kind + channel);
                foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>(true))
                {
                    bool isGround = renderer.transform.parent == root.Find("Excavation")
                        || renderer.transform.parent == root.Find("Surface") && renderer.name.EndsWith(" rim");
                    if (isGround) Assert.That(renderer.sharedMaterial, Is.SameAs(ground), renderer.name);
                    else Assert.That(renderer.sharedMaterial.shader.name, Is.EqualTo("Universal Render Pipeline/Lit"), renderer.name);
                }
                foreach (string name in new[] { "SellStation", "UpgradeStation", "RechargeZone", "ReturnAnchor" })
                {
                    Transform anchor = root.Find("Surface/" + name);
                    Assert.That(anchor, Is.Not.Null, name);
                    Assert.That(anchor.position.y, Is.InRange(0, 0.2f));
                    Assert.That(Vector3.Distance(anchor.position, new Vector3(0, 0, -12)), Is.LessThan(5), name);
                }
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
