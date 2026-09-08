using System.Linq;
using NUnit.Framework;
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
                Assert.That(p.x, Is.InRange(0.3f, 23.7f));
                Assert.That(p.y, Is.InRange(0.3f, 11.7f));
                Assert.That(p.z, Is.InRange(0.3f, 23.7f));
                for (int j = 0; j < i; j++) Assert.That(Vector3.Distance(p, first[j].Position), Is.GreaterThanOrEqualTo(0.8999f));
            }
            for (int i = 0; i < 3; i++) Assert.That(first.Count(p => p.PrefabIndex == i), Is.EqualTo(32));
            Assert.That(DiscoveryField.Generate(extent, 96, seed + 1)[0].Position, Is.Not.EqualTo(first[0].Position));
        }
    }
}
