using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace SomethingDownThere.Tests
{
    public sealed class DiscoveryPlacementTests
    {
        [TestCase(90127)]
        [TestCase(12)]
        [TestCase(991)]
        public void SeedReplaysSeparatedBuriedFindsWithShallowEntryConcentration(int seed)
        {
            var extent = new Vector3(24, 12, 24);
            var first = DiscoveryField.Generate(extent, 96, seed);
            var replay = DiscoveryField.Generate(extent, 96, seed);
            CollectionAssert.AreEqual(first, replay);
            Assert.That(first.Count(p => p.Position.y > 10.7f && p.Position.z < 6.1f), Is.GreaterThanOrEqualTo(24));
            Assert.That(first.Take(6).All(p => Mathf.Abs(p.Position.x - 12) < 2.6f && p.Position.z < 3.6f), Is.True);
            Assert.That(first.Count(p => p.Position.y < 8.5f), Is.GreaterThan(15));
            for (int i = 0; i < first.Length; i++)
            {
                Vector3 p = first[i].Position;
                Assert.That(p.x, Is.InRange(0.8f, 23.2f));
                Assert.That(p.y, Is.InRange(0.8f, 11.35f));
                Assert.That(p.z, Is.InRange(0.8f, 23.2f));
                for (int j = 0; j < i; j++) Assert.That(Vector3.Distance(p, first[j].Position),
                    Is.GreaterThanOrEqualTo(DiscoveryField.MinimumSpacing - 0.0001f));
            }
            for (int i = 0; i < 3; i++) Assert.That(first.Count(p => p.PrefabIndex == i), Is.EqualTo(32));
            Assert.That(DiscoveryField.Generate(extent, 96, seed + 1)[0].Position, Is.Not.EqualTo(first[0].Position));
        }

        [TestCase(7)] [TestCase(90127)] [TestCase(51)]
        public void ExplicitMineralBandsStayBuriedSeparatedAndReproducible(int seed)
        {
            const int total = 400;
            var radii = Enumerable.Repeat(.38f, total).ToArray();
            var bands = Enumerable.Range(0, total).Select(i => new Vector2(.65f + i / 50 * 3.8f, 4.4f + i / 50 * 3.8f)).ToArray();
            var extent = new Vector3(24, 32, 24);
            var layout = DiscoveryField.Generate(extent, total, seed, 24, radii, bands);
            CollectionAssert.AreEqual(layout, DiscoveryField.Generate(extent, total, seed, 24, radii, bands));
            for (int i = 24; i < total; i++)
                Assert.That(32 - layout[i].Position.y, Is.InRange(bands[i].x - .0001f, Mathf.Min(bands[i].y, 31.2f) + .0001f));
            for (int i = 0; i < total; i++) for (int j = 0; j < i; j++)
                Assert.That(Vector3.Distance(layout[i].Position, layout[j].Position), Is.GreaterThanOrEqualTo(.86f - .0001f));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => DiscoveryField.Generate(new Vector3(24, 12, 24), total, seed, 24, radii, bands));
        }

        [Test]
        public void EnlargedApprovedPrefabsFitThePlacementClearanceAndRequireFiftyPercent()
        {
            foreach (string name in new[] { "Blue marble", "Copper token", "Amber bead" })
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Content/Finds/" + name + ".prefab");
                var mesh = prefab.GetComponent<MeshCollider>().sharedMesh;
                float radius = mesh.vertices.Max(v => Vector3.Scale(v, prefab.transform.localScale).magnitude);
                Assert.That(radius, Is.InRange(0.39f, DiscoveryField.MaximumFindRadius + 0.001f), name);
                Assert.That(radius * 2, Is.LessThan(DiscoveryField.MinimumSpacing));
                Assert.That(prefab.GetComponent<BuriedFind>().RequiredExposure, Is.EqualTo(0.4f));
            }
        }
    }
}
