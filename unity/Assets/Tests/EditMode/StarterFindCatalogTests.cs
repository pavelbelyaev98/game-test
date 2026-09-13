using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace SomethingDownThere.Tests
{
    public sealed class StarterFindCatalogTests
    {
        private static DiscoveryCatalog Catalog => AssetDatabase.LoadAssetAtPath<DiscoveryCatalog>("Assets/Content/StarterFinds/StarterFindCatalog.asset");

        [TestCase(90127)] [TestCase(12)] [TestCase(991)]
        public void TrialPopulationIsReproducibleWithExactQuotasAndBuriedClearance(int seed)
        {
            var catalog = Catalog; catalog.Validate();
            var extent = new Vector3(24,32,24); var layout = catalog.Generate(extent,seed);
            CollectionAssert.AreEqual(layout,catalog.Generate(extent,seed));
            Assert.That(layout.Length,Is.EqualTo(1024));
            CollectionAssert.AreEqual(new[] {0,0,0,96,240,160,128,112,96,80,64,48}, catalog.Entries.Select(e=>e.Count));
            for(int index=0;index<catalog.Entries.Length;index++)
            {
                var entry=catalog.Entries[index];
                Assert.That(layout.Count(p=>p.PrefabIndex==index),Is.EqualTo(entry.Count));
                Assert.That(layout.Take(catalog.ShallowCount).Count(p=>p.PrefabIndex==index),Is.EqualTo(entry.ShallowCount));
                Assert.That(entry.Prefab.DetectorEligible,Is.False);
                Assert.That(entry.Prefab.SurfaceSampleCount,Is.EqualTo(256));
                Assert.That(entry.Prefab.RequiredExposure,Is.EqualTo(.6f));
                Assert.That(entry.Prefab.GetComponent<FindPhysics>(),Is.Not.Null);
                Assert.That(entry.Prefab.GetComponent<MeshCollider>().convex,Is.True);
                var mesh=entry.Prefab.GetComponent<MeshFilter>().sharedMesh;
                var hull = entry.Prefab.GetComponent<MeshCollider>().sharedMesh;
                Assert.That(hull,Is.Not.SameAs(mesh));
                Assert.That(hull.triangles.Length/3,Is.LessThanOrEqualTo(220));
                Assert.That((hull.bounds.size-mesh.bounds.size).magnitude,Is.LessThan(.0001f));
                Assert.That(mesh.bounds.center.magnitude,Is.LessThan(.0001f));
                var renderer=entry.Prefab.GetComponent<MeshRenderer>();
                Assert.That(renderer.sharedMaterial.GetTexture("_BaseMap"),Is.Not.Null);
                Assert.That(renderer.sharedMaterial.GetTexture("_BumpMap"),Is.Not.Null);
                bool buried = true;
                foreach(var placement in layout.Where(p=>p.PrefabIndex==index))
                    foreach(var vertex in entry.Appearance(placement.AppearanceIndex).GetComponent<MeshFilter>().sharedMesh.vertices)
                    {
                        var world=placement.Position+placement.Rotation*vertex;
                        buried &= world.x >= 0 && world.x <= extent.x && world.y >= 0 && world.y <= extent.y-.01f && world.z >= 0 && world.z <= extent.z;
                    }
                Assert.That(buried, Is.True, entry.ItemId + ": every rotated mesh vertex must start inside soil.");
            }
            AssertSeparated(layout, catalog.Entries.Select(e => e.PlacementRadius).ToArray(), seed);
        }

        [Test]
        public void ShallowEncountersCoverTheTopAndStartingRimAcrossSeeds()
        {
            var catalog = Catalog;
            var radii = catalog.Entries.Select(e => e.PlacementRadius).ToArray();
            Assert.That(catalog.ShallowCount, Is.EqualTo(312));
            CollectionAssert.AreEqual(new[] { 0, 0, 0, 96, 216, 0, 0, 0, 0, 0, 0, 0 }, catalog.Entries.Select(e => e.ShallowCount));
            for (int seed = 0; seed < 100; seed++)
            {
                var layout = catalog.Generate(new Vector3(24, 32, 24), seed);
                var top = layout.Take(catalog.ShallowCount).ToArray();
                Assert.That(top.All(p => 32 - p.Position.y >= .65f - .0001f && 32 - p.Position.y <= 1.1f), Is.True);
                Assert.That(top.Count(p => p.Position.z <= 6), Is.GreaterThanOrEqualTo(50));
                Assert.That(layout.Skip(catalog.ShallowCount).Count(), Is.EqualTo(712));
                Assert.That(layout.Count(p => p.Position.y < 8.5f), Is.GreaterThanOrEqualTo(100));
                // Sample walkable excavation locations, including lateral/back areas. This is a
                // spatial bound on empty topsoil, not a claim about every player's encounter time.
                for (float x = .8f; x <= 23.2f; x += .5f)
                    for (float z = .8f; z <= 23.2f; z += .5f)
                    {
                        float distance = top.Min(p => Vector2.Distance(new Vector2(x, z), new Vector2(p.Position.x, p.Position.z)));
                        Assert.That(distance, Is.LessThanOrEqualTo(1.5f), $"Seed {seed}, topsoil at {x}, {z}");
                    }
                AssertSeparated(layout, radii, seed);
            }
        }

        private static void AssertSeparated(DiscoveryPlacement[] layout, float[] radii, int seed)
        {
            // Check every pair, without millions of NUnit constraints or repeated
            // Unity mesh/component reads for the same immutable prefab dimensions.
            for (int i = 0; i < layout.Length; i++) for (int j = 0; j < i; j++)
            {
                float required = radii[layout[i].PrefabIndex] + radii[layout[j].PrefabIndex] + DiscoveryField.SoilClearance - .0001f;
                if ((layout[i].Position - layout[j].Position).sqrMagnitude < required * required)
                    Assert.Fail($"Seed {seed}: placements {i} and {j} overlap their soil envelopes.");
            }
        }

        [Test]
        public void MineralBandsHaveIncreasingValuesAndLateralCoverageAcrossSeeds()
        {
            var catalog = Catalog;
            var minerals = catalog.Entries.Where(e => e.ItemId.StartsWith("mineral_")).ToArray();
            CollectionAssert.AreEqual(new[] { "Coal", "Copper", "Iron", "Silver", "Gold", "Emerald", "Ruby", "Diamond" }, minerals.Select(e => e.Prefab.DisplayName));
            CollectionAssert.AreEqual(new[] { 2, 4, 6, 9, 13, 20, 30, 45 }, minerals.Select(e => e.Prefab.SaleValue));
            for (int seed = 0; seed < 20; seed++)
            {
                var layout = catalog.Generate(new Vector3(24, 32, 24), seed);
                foreach (var entry in minerals)
                {
                    int index = Array.IndexOf(catalog.Entries, entry);
                    var placements = layout.Where(p => p.PrefabIndex == index).ToArray();
                    Assert.That(placements.All(p => 32 - p.Position.y >= entry.MinDepth - .0001f && 32 - p.Position.y <= entry.MaxDepth + .0001f), Is.True, entry.ItemId);
                    for (int quadrant = 0; quadrant < 4; quadrant++)
                        Assert.That(placements.Count(p => (p.Position.x < 12 ? 0 : 1) + (p.Position.z < 12 ? 0 : 2) == quadrant),
                            Is.GreaterThanOrEqualTo(entry.Count / 10), $"Seed {seed}, {entry.ItemId}, quadrant {quadrant}");
                }
            }
        }

        [Test]
        public void ImpossibleShallowDensityFailsWithinABoundedSearch()
        {
            Assert.Throws<InvalidOperationException>(() => DiscoveryField.Generate(new Vector3(8, 4, 8), 256, 12, 256));
            Assert.Throws<ArgumentOutOfRangeException>(() => DiscoveryField.Generate(new Vector3(24, 12, 24), 192, 12, 193));
        }

        [Test]
        public void RockIsOneItemWithThreeSavedAppearancesAndVariedSeededOrientations()
        {
            var catalog = Catalog;
            var rock = catalog.Entries.Single(e => e.ItemId == "common_rock");
            Assert.That(rock.AppearanceCount, Is.EqualTo(3));
            var seen = new System.Collections.Generic.HashSet<string>();
            bool tipped = false, inverted = false;
            foreach (int seed in new[] { 90127, 12, 991 })
                foreach (var placement in catalog.Generate(new Vector3(24, 32, 24), seed))
                {
                    var entry = catalog.Entries[placement.PrefabIndex];
                    if (entry != rock) continue;
                    var prefab = entry.Appearance(placement.AppearanceIndex);
                    seen.Add(prefab.SaveContentId);
                    Assert.That(prefab.DisplayName, Is.EqualTo("Rock"));
                    Assert.That(prefab.SaleValue, Is.EqualTo(2));
                    Assert.That(prefab.Size, Is.EqualTo(FindSize.Large));
                    Assert.That(prefab.RequiredExposure, Is.EqualTo(.6f));
                    Assert.That(prefab.DetectorEligible, Is.False);
                    Assert.That(catalog.Resolve(prefab.SaveContentId, out bool legacy), Is.SameAs(prefab));
                    Assert.That(legacy, Is.False);
                    var state = new FindSnapshot { ContentId = prefab.SaveContentId, Item = new ItemSnapshot { Id = "saved-rock", Name = "Rock", Value = 7 },
                        Position = placement.Position, Rotation = placement.Rotation, Scale = Vector3.one, PhysicsReleased = true };
                    var restored = catalog.PrepareRestore(state);
                    Assert.That(restored.ContentId, Is.EqualTo(state.ContentId));
                    Assert.That(restored.Rotation, Is.EqualTo(state.Rotation));
                    Assert.That(restored.Item.Value, Is.EqualTo(7), "Historical value survives tuning.");
                    Assert.That(restored.PhysicsReleased, Is.True);
                    float up = Vector3.Dot(placement.Rotation * Vector3.up, Vector3.up);
                    tipped |= Mathf.Abs(up) < .4f; inverted |= up < -.5f;
                }
            Assert.That(seen.Count, Is.EqualTo(3));
            Assert.That(tipped && inverted, Is.True, "Rotations must include full tilt, not just yaw.");
        }

        [Test]
        public void LegacyPopulationMigratesWithoutRerollOrChangesToCarriedRecordsAndMigrationIsIdempotent()
        {
            var catalog=Catalog;
            var layout=DiscoveryField.Generate(new Vector3(24,12,24),96,90127);
            for(int i=0;i<96;i++)
            {
                var alias=catalog.LegacyAliases[i%catalog.LegacyAliases.Length]; bool collected=i%5==0;
                var saved=new FindSnapshot { ContentId=alias.OldId,Item=new ItemSnapshot {Id="old-"+i,Name="Historical item",Value=5+(i%3)*3},
                    Position=layout[i].Position,Rotation=layout[i].Rotation,Scale=Vector3.one*.8f,Collected=collected };
                var migrated=catalog.PrepareRestore(saved); var repeated=catalog.PrepareRestore(migrated);
                Assert.That(migrated.ContentId,Is.EqualTo(alias.CurrentId)); Assert.That(migrated.Position,Is.EqualTo(saved.Position));
                Assert.That(migrated.Rotation,Is.EqualTo(saved.Rotation)); Assert.That(migrated.Scale,Is.EqualTo(Vector3.one));
                Assert.That(migrated.Item.Id,Is.EqualTo(saved.Item.Id)); Assert.That(migrated.Item.Value,Is.EqualTo(saved.Item.Value));
                Assert.That(migrated.Collected,Is.EqualTo(collected)); Assert.That(saved.ContentId,Is.EqualTo(alias.OldId));
                Assert.That(saved.Scale,Is.EqualTo(Vector3.one*.8f)); Assert.That(saved.Item.Name,Is.EqualTo("Historical item"));
                Assert.That(migrated.Item.Name,collected?Is.EqualTo(saved.Item.Name):Is.Not.EqualTo(saved.Item.Name));
                Assert.That(repeated.ContentId,Is.EqualTo(migrated.ContentId)); Assert.That(repeated.Item.Name,Is.EqualTo(migrated.Item.Name));
                Assert.That(repeated.Item.Value,Is.EqualTo(migrated.Item.Value)); Assert.That(repeated.Scale,Is.EqualTo(migrated.Scale));
            }
            Assert.Throws<InvalidDataException>(()=>catalog.Resolve("missing-content",out _));
        }

        [Test]
        public void ShallowRockAndCoalPopulationFundsTheExistingShovelTrackWithoutClearingTheWholeMap()
        {
            var catalog = Catalog;
            var rock = catalog.Entries.Single(e => e.ItemId == "common_rock");
            int totalCost = StationTrade.DefaultPrices().Sum();
            Assert.That(catalog.Entries.Sum(e => e.ShallowCount * e.Prefab.SaleValue), Is.GreaterThan(totalCost),
                "Shallow play should fund every existing shovel upgrade without mandatory deep-map clearance.");
            Assert.That(5 * rock.Prefab.SaleValue, Is.EqualTo(StationTrade.DefaultPrices()[0]));
        }

        [Test]
        public void DisabledBottlesRemainRestorableWithHistoricalIdentityPoseAndValue()
        {
            var catalog = Catalog;
            foreach (var entry in catalog.Entries.Where(e => e.ItemId.StartsWith("common_bottle_")))
            {
                Assert.That(entry.Count, Is.Zero); Assert.That(entry.ShallowCount, Is.Zero);
                foreach (bool collected in new[] { false, true })
                {
                    var saved = new FindSnapshot { ContentId = entry.Prefab.SaveContentId,
                        Item = new ItemSnapshot { Id = "old-bottle", Name = "Glass Bottle", Value = 17 },
                        Position = new Vector3(2, -1, 3), Rotation = Quaternion.Euler(45, 90, 12),
                        Scale = Vector3.one, PhysicsReleased = true, Collected = collected };
                    var restored = catalog.PrepareRestore(saved);
                    Assert.That(catalog.Resolve(saved.ContentId, out bool legacy), Is.SameAs(entry.Prefab));
                    Assert.That(legacy, Is.False);
                    Assert.That(JsonUtility.ToJson(restored), Is.EqualTo(JsonUtility.ToJson(saved)));
                }
            }
            var invalid = UnityEngine.Object.Instantiate(catalog);
            try
            {
                invalid.Entries[0].Count = -1;
                Assert.Throws<InvalidDataException>(() => invalid.Validate());
            }
            finally { UnityEngine.Object.DestroyImmediate(invalid); }
        }

        [Test]
        public void CurrentItemIdentityAndSavedValueSurviveChangingItsPrefabArt()
        {
            var original=Catalog;var catalog=UnityEngine.Object.Instantiate(original);
            var replacement=UnityEngine.Object.Instantiate(original.Entries[0].Prefab.gameObject);
            try
            {
                replacement.GetComponent<MeshFilter>().sharedMesh=original.Entries[1].Prefab.GetComponent<MeshFilter>().sharedMesh;
                replacement.GetComponent<MeshCollider>().sharedMesh=original.Entries[1].Prefab.GetComponent<MeshCollider>().sharedMesh;
                catalog.Entries[0]=new DiscoveryCatalog.Entry {Prefab=replacement.GetComponent<BuriedFind>(),Count=0,ShallowCount=0,LayOnSide=true};
                catalog.Validate();
                var saved=new FindSnapshot {ContentId=original.Entries[0].Prefab.SaveContentId,Item=new ItemSnapshot {Id="same-item",Name="Glass Bottle",Value=17},Scale=Vector3.one,Rotation=Quaternion.identity};
                var restored=catalog.PrepareRestore(saved);
                Assert.That(catalog.Resolve(saved.ContentId,out bool legacy),Is.SameAs(replacement.GetComponent<BuriedFind>()));
                Assert.That(legacy,Is.False);Assert.That(restored.Item.Id,Is.EqualTo("same-item"));Assert.That(restored.Item.Value,Is.EqualTo(17));
            }
            finally {UnityEngine.Object.DestroyImmediate(replacement);UnityEngine.Object.DestroyImmediate(catalog);}
        }
    }
}
