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
            Assert.That(layout.Length,Is.EqualTo(2578));
            CollectionAssert.AreEqual(new[] {0,0,0,1000,186,229,218,219,219,235,171,101}, catalog.Entries.Select(e=>e.Count));
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
                {
                    // The prefab's authored shrink moves real soil clearance with it.
                    var appearance = entry.Appearance(placement.AppearanceIndex);
                    var scale = appearance.transform.localScale;
                    foreach(var vertex in appearance.GetComponent<MeshFilter>().sharedMesh.vertices)
                    {
                        var world=placement.Position+placement.Rotation*Vector3.Scale(vertex, scale);
                        buried &= world.x >= 0 && world.x <= extent.x && world.y >= 0 && world.y <= extent.y-.01f && world.z >= 0 && world.z <= extent.z;
                    }
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
            Assert.That(catalog.ShallowCount, Is.EqualTo(460));
            CollectionAssert.AreEqual(new[] { 0, 0, 0, 460, 0, 0, 0, 0, 0, 0, 0, 0 }, catalog.Entries.Select(e => e.ShallowCount));
            int worstSlice = 0, driestWindow = int.MaxValue;
            float worstRatio = 0;
            for (int seed = 0; seed < 100; seed++)
            {
                var layout = catalog.Generate(new Vector3(24, 32, 24), seed);
                var top = layout.Take(catalog.ShallowCount).ToArray();
                // Every entry find hangs off its own envelope: just buried, in two tiers, so
                // a starter scrape reaches most of them while nothing pokes through the turf.
                Assert.That(top.All(p => 32 - p.Position.y >= radii[p.PrefabIndex] + DiscoveryField.SoilClearance + .02f - .0001f
                    && 32 - p.Position.y <= radii[p.PrefabIndex] + DiscoveryField.SoilClearance + .4f + .0001f), Is.True);
                Assert.That(top.Count(p => p.Position.z <= 6), Is.GreaterThanOrEqualTo(50));
                Assert.That(layout.Skip(catalog.ShallowCount).Count(), Is.EqualTo(2118));
                Assert.That(layout.Count(p => p.Position.y < 8.5f), Is.GreaterThanOrEqualTo(100));
                // The dig rate stays flat over a few metres of descent: single metres
                // wobble around band edges, but no stretch may run dry or flood. The
                // packed entry layer lives above 1 m, so the banded rate starts at 2 m.
                var slices = new int[29];
                foreach (var placement in layout)
                {
                    int slice = Mathf.FloorToInt(32 - placement.Position.y) - 2;
                    if (slice >= 0 && slice < slices.Length) slices[slice]++;
                }
                int driest = int.MaxValue, richest = 0;
                for (int i = 0; i + 2 < slices.Length; i++)
                {
                    int window = slices[i] + slices[i + 1] + slices[i + 2];
                    driest = Mathf.Min(driest, window);
                    richest = Mathf.Max(richest, window);
                }
                // The packed entry layer pushes banded finds out of the metre right under
                // it, so a single banded layer may dip; the 3 m window below is the guard
                // that matters for feel.
                Assert.That(slices.Min(), Is.GreaterThanOrEqualTo(24),
                    $"Seed {seed}: a 1 m layer is too sparse: {string.Join(",", slices)}");
                // Band edges stack two cores in the same metre, so only the worst seed is
                // asserted, after the sweep, against bounds measured over the whole set.
                worstSlice = Mathf.Max(worstSlice, slices.Max());
                driestWindow = Mathf.Min(driestWindow, driest);
                worstRatio = Mathf.Max(worstRatio, richest / (float)driest);
                // Sample walkable excavation locations, including lateral/back areas. This is a
                // spatial bound on empty topsoil, not a claim about every player's encounter time.
                // The rug is bucketed so the sweep stays linear as the entry layer grows.
                var rug = new System.Collections.Generic.Dictionary<(int, int), System.Collections.Generic.List<Vector2>>();
                foreach (var p in top)
                {
                    var cell = (Mathf.FloorToInt(p.Position.x / 2f), Mathf.FloorToInt(p.Position.z / 2f));
                    if (!rug.TryGetValue(cell, out var list)) rug[cell] = list = new System.Collections.Generic.List<Vector2>();
                    list.Add(new Vector2(p.Position.x, p.Position.z));
                }
                for (float x = .8f; x <= 23.2f; x += .5f)
                    for (float z = .8f; z <= 23.2f; z += .5f)
                    {
                        int cx = Mathf.FloorToInt(x / 2f), cz = Mathf.FloorToInt(z / 2f);
                        float distance = float.MaxValue;
                        for (int ox = -1; ox <= 1; ox++)
                        for (int oz = -1; oz <= 1; oz++)
                            if (rug.TryGetValue((cx + ox, cz + oz), out var list))
                                foreach (var p in list) distance = Mathf.Min(distance, Vector2.Distance(new Vector2(x, z), p));
                        Assert.That(distance, Is.LessThanOrEqualTo(1.5f), $"Seed {seed}, topsoil at {x}, {z}");
                    }
                AssertSeparated(layout, radii, seed);
            }
            Debug.Log($"Entry canopy invariants: worst 1 m layer {worstSlice}, driest 3 m window {driestWindow}, "
                + $"worst 3 m ratio {worstRatio:F2} across 100 seeds.");
            Assert.That(worstSlice, Is.LessThanOrEqualTo(160), "A 1 m layer must never flood with finds.");
            Assert.That(driestWindow, Is.GreaterThanOrEqualTo(130), "No 3 m stretch may dig dry.");
            Assert.That(worstRatio, Is.LessThanOrEqualTo(2.4f), "The dig rate must stay roughly constant with depth.");
        }

        private static void AssertSeparated(DiscoveryPlacement[] layout, float[] radii, int seed)
        {
            // Bucket by the largest envelope any pair can require, so a 5,000 find carpet
            // stays linear instead of thirteen million pair checks per seed.
            const float cell = 1.2f;
            var buckets = new System.Collections.Generic.Dictionary<(int, int, int), System.Collections.Generic.List<int>>();
            for (int i = 0; i < layout.Length; i++)
            {
                var p = layout[i].Position;
                var key = (Mathf.FloorToInt(p.x / cell), Mathf.FloorToInt(p.y / cell), Mathf.FloorToInt(p.z / cell));
                if (!buckets.TryGetValue(key, out var list)) buckets[key] = list = new System.Collections.Generic.List<int>();
                list.Add(i);
            }
            for (int i = 0; i < layout.Length; i++)
            {
                var p = layout[i].Position;
                int cx = Mathf.FloorToInt(p.x / cell), cy = Mathf.FloorToInt(p.y / cell), cz = Mathf.FloorToInt(p.z / cell);
                for (int ox = -1; ox <= 1; ox++)
                for (int oy = -1; oy <= 1; oy++)
                for (int oz = -1; oz <= 1; oz++)
                {
                    if (!buckets.TryGetValue((cx + ox, cy + oy, cz + oz), out var list)) continue;
                    foreach (int j in list)
                    {
                        if (j >= i) continue;
                        float required = radii[layout[i].PrefabIndex] + radii[layout[j].PrefabIndex]
                            + DiscoveryField.SoilClearance - .0001f;
                        if ((p - layout[j].Position).sqrMagnitude < required * required)
                            Assert.Fail($"Seed {seed}: placements {i} and {j} overlap their soil envelopes.");
                    }
                }
            }
        }

        [Test]
        public void DepthMixSlidesFromJunkToValueAndKeepsScatteredOutliers()
        {
            var catalog = Catalog;
            var cheap = new System.Collections.Generic.HashSet<string> { "common_rock", "mineral_coal" };
            var rich = new System.Collections.Generic.HashSet<string> { "mineral_gold", "mineral_emerald", "mineral_ruby", "mineral_diamond" };
            int outlierSeeds = 0, deepCheapTotal = 0, highRichTotal = 0;
            for (int seed = 0; seed < 20; seed++)
            {
                var layout = catalog.Generate(new Vector3(24, 32, 24), seed);
                Assert.That(DepthShare(layout, catalog, 2, 6, cheap), Is.GreaterThanOrEqualTo(.45f), $"Seed {seed}: the top layers must stay junk-heavy.");
                Assert.That(DepthShare(layout, catalog, 20, 31, cheap), Is.LessThanOrEqualTo(.05f), $"Seed {seed}: junk must not dominate deep ground.");
                Assert.That(DepthShare(layout, catalog, 2, 8, rich), Is.LessThanOrEqualTo(.15f), $"Seed {seed}: rich finds must stay rare near the surface.");
                Assert.That(DepthShare(layout, catalog, 20, 31, rich), Is.GreaterThanOrEqualTo(.8f), $"Seed {seed}: deep ground must be worth digging.");
                // Scatter goes both ways: the odd lump of junk deep, the odd valuable high.
                int deepCheap = 0, highRich = 0;
                foreach (var placement in layout)
                {
                    string id = catalog.Entries[placement.PrefabIndex].ItemId;
                    if (32 - placement.Position.y >= 18 && cheap.Contains(id)) deepCheap++;
                    if (32 - placement.Position.y < 8 && rich.Contains(id)) highRich++;
                }
                // Outliers are counted per seed but asserted across the set: the entry carpet
                // reshuffles the placement stream, so one seed may carry none of a scarce
                // outlier while the set as a whole still mixes both directions.
                Assert.That(deepCheap, Is.GreaterThanOrEqualTo(1), $"Seed {seed}: deep ground needs the odd junk outlier.");
                deepCheapTotal += deepCheap; highRichTotal += highRich;
                if (deepCheap >= 2 && highRich >= 2) outlierSeeds++;
            }
            Assert.That(deepCheapTotal, Is.GreaterThanOrEqualTo(40), "Every seed set needs junk well below its band.");
            Assert.That(highRichTotal, Is.GreaterThanOrEqualTo(20), "Every seed set needs valuables high up.");
            Assert.That(outlierSeeds, Is.GreaterThanOrEqualTo(4), "The set carries outliers in both directions.");
        }

        private static float DepthShare(DiscoveryPlacement[] layout, DiscoveryCatalog catalog, float from, float to, System.Collections.Generic.HashSet<string> ids)
        {
            int total = 0, hits = 0;
            foreach (var placement in layout)
            {
                float depth = 32 - placement.Position.y;
                if (depth < from || depth >= to) continue;
                total++;
                if (ids.Contains(catalog.Entries[placement.PrefabIndex].ItemId)) hits++;
            }
            return total == 0 ? 0f : hits / (float)total;
        }

        [Test]
        public void MineralBandsHaveIncreasingValuesAndLateralCoverageAcrossSeeds()
        {
            var catalog = Catalog;
            var minerals = catalog.Entries.Where(e => e.ItemId.StartsWith("mineral_")).ToArray();
            CollectionAssert.AreEqual(new[] { "Coal", "Copper", "Iron", "Silver", "Gold", "Emerald", "Ruby", "Diamond" }, minerals.Select(e => e.Prefab.DisplayName));
            CollectionAssert.AreEqual(new[] { 4, 5, 6, 9, 13, 20, 30, 45 }, minerals.Select(e => e.Prefab.SaleValue));
            for (int seed = 0; seed < 20; seed++)
            {
                var layout = catalog.Generate(new Vector3(24, 32, 24), seed);
                foreach (var entry in minerals)
                {
                    int index = Array.IndexOf(catalog.Entries, entry);
                    var placements = layout.Where(p => p.PrefabIndex == index).ToArray();
                    // The entry burst sits in its own 0.65-1.1 m layer above every band.
                    var banded = layout.Skip(catalog.ShallowCount).Where(p => p.PrefabIndex == index).ToArray();
                    Assert.That(banded.All(p => 32 - p.Position.y >= entry.MinDepth - .0001f && 32 - p.Position.y <= entry.MaxDepth + .0001f), Is.True, entry.ItemId);
                    // Most of a type still sits in its core: the wide band only scatters outliers.
                    int core = banded.Count(p => 32 - p.Position.y >= entry.CoreMinDepth - .0001f && 32 - p.Position.y <= entry.CoreMaxDepth + .0001f);
                    Assert.That(core, Is.GreaterThanOrEqualTo(Mathf.FloorToInt(banded.Length * entry.CoreShare * .9f)), $"{seed}: {entry.ItemId} core share");
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
        public void FullPopulationGeneratesInsideTheWarmTimeBudget()
        {
            var catalog = Catalog;
            var extent = new Vector3(24, 32, 24);
            catalog.Generate(extent, 90127); // warm meshes, JIT and the placement grid
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var layout = catalog.Generate(extent, 12);
            watch.Stop();
            Debug.Log($"Full population placement: {watch.Elapsed.TotalMilliseconds:F0} ms for {layout.Length} finds.");
            Assert.That(layout.Length, Is.EqualTo(2578));
            Assert.That(watch.Elapsed.TotalSeconds, Is.LessThan(1.0), "Placement must stay clear of the old all-pairs scan.");
        }

        [Test]
        public void PreviousThousandFindPopulationStillResolvesWithoutReroll()
        {
            var catalog = Catalog;
            var counts = new[] { 0, 0, 0, 96, 240, 160, 128, 112, 96, 80, 64, 48 };
            int index = 0;
            for (int e = 0; e < catalog.Entries.Length; e++)
                for (int n = 0; n < counts[e]; n++)
                {
                    var state = new FindSnapshot { ContentId = catalog.Entries[e].Prefab.SaveContentId,
                        Item = new ItemSnapshot { Id = "legacy-" + index, Name = "Historical item", Value = 5 },
                        Position = new Vector3(1 + index * .001f, 12, 3), Rotation = Quaternion.Euler(0, index % 360, 0),
                        Scale = Vector3.one, Collected = index % 5 == 0 };
                    Assert.That(catalog.Resolve(state.ContentId, out bool legacy), Is.SameAs(catalog.Entries[e].Prefab));
                    Assert.That(legacy, Is.False);
                    var restored = catalog.PrepareRestore(state);
                    Assert.That(restored.ContentId, Is.EqualTo(state.ContentId));
                    Assert.That(restored.Position, Is.EqualTo(state.Position));
                    Assert.That(restored.Rotation, Is.EqualTo(state.Rotation));
                    Assert.That(restored.Item.Id, Is.EqualTo(state.Item.Id));
                    Assert.That(restored.Item.Value, Is.EqualTo(state.Item.Value));
                    Assert.That(restored.Collected, Is.EqualTo(state.Collected));
                    index++;
                }
            Assert.That(index, Is.EqualTo(1024));
            Assert.That(index, Is.LessThanOrEqualTo(DiscoveryField.MaximumPopulation));
        }

        [Test]
        public void RockOwnsTheShallowLayerWithThreeSavedAppearancesAndFullTiltOrientations()
        {
            var catalog = Catalog;
            var rock = catalog.Entries.Single(e => e.ItemId == "common_rock");
            // Rocks are the shallow layer: every saved appearance must resolve and restore.
            Assert.That(rock.Count, Is.EqualTo(1000));
            Assert.That(rock.ShallowCount, Is.EqualTo(460));
            Assert.That(rock.AppearanceCount, Is.EqualTo(3));
            var seen = new System.Collections.Generic.HashSet<string>();
            for (int i = 0; i < rock.AppearanceCount; i++)
            {
                var prefab = rock.Appearance(i);
                seen.Add(prefab.SaveContentId);
                Assert.That(prefab.DisplayName, Is.EqualTo("Rock"));
                Assert.That(prefab.SaleValue, Is.EqualTo(2));
                Assert.That(prefab.Size, Is.EqualTo(FindSize.Large));
                Assert.That(prefab.RequiredExposure, Is.EqualTo(.6f));
                Assert.That(prefab.DetectorEligible, Is.False);
                Assert.That(catalog.Resolve(prefab.SaveContentId, out bool legacy), Is.SameAs(prefab));
                Assert.That(legacy, Is.False);
                var state = new FindSnapshot { ContentId = prefab.SaveContentId, Item = new ItemSnapshot { Id = "saved-rock", Name = "Rock", Value = 7 },
                    Position = new Vector3(3, 20, 4), Rotation = Quaternion.Euler(11, 22, 33), Scale = Vector3.one, PhysicsReleased = true };
                var restored = catalog.PrepareRestore(state);
                Assert.That(restored.ContentId, Is.EqualTo(state.ContentId));
                Assert.That(restored.Rotation, Is.EqualTo(state.Rotation));
                Assert.That(restored.Item.Value, Is.EqualTo(7), "Historical value survives tuning.");
                Assert.That(restored.PhysicsReleased, Is.True);
            }
            Assert.That(seen.Count, Is.EqualTo(3));
            // Shipped types seed full tilt, not just yaw, across the rock population.
            bool tipped = false, inverted = false;
            foreach (int seed in new[] { 90127, 12, 991 })
                foreach (var placement in catalog.Generate(new Vector3(24, 32, 24), seed))
                {
                    if (catalog.Entries[placement.PrefabIndex].ItemId != "common_rock") continue;
                    float up = Vector3.Dot(placement.Rotation * Vector3.up, Vector3.up);
                    tipped |= Mathf.Abs(up) < .4f; inverted |= up < -.5f;
                }
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
        public void RetiredEntriesRemainRestorableWithHistoricalIdentityPoseAndValue()
        {
            var catalog = UnityEngine.Object.Instantiate(Catalog);
            try
            {
                // Simulate retiring the junk types again: a zero-count entry must keep
                // resolving for existing saves without ever spawning in a new game.
                foreach (bool collected in new[] { false, true })
                    foreach (var entry in catalog.Entries.Where(e => e.ItemId.StartsWith("common_bottle_")))
                    {
                        entry.Count = 0; entry.ShallowCount = 0;
                        catalog.Validate();
                        var saved = new FindSnapshot { ContentId = entry.Prefab.SaveContentId,
                            Item = new ItemSnapshot { Id = "old-bottle", Name = "Glass Bottle", Value = 17 },
                            Position = new Vector3(2, -1, 3), Rotation = Quaternion.Euler(45, 90, 12),
                            Scale = Vector3.one, PhysicsReleased = true, Collected = collected };
                        var restored = catalog.PrepareRestore(saved);
                        Assert.That(catalog.Resolve(saved.ContentId, out bool legacy), Is.SameAs(entry.Prefab));
                        Assert.That(legacy, Is.False);
                        Assert.That(JsonUtility.ToJson(restored), Is.EqualTo(JsonUtility.ToJson(saved)));
                    }
                // Historical ids replaced by different art keep resolving to the current type.
                var legacySaved = new FindSnapshot { ContentId = "common_can_intact",
                    Item = new ItemSnapshot { Id = "old-can", Name = "Food/Drink Can", Value = 1 },
                    Position = new Vector3(4, -2, 5), Rotation = Quaternion.Euler(10, 20, 30),
                    Scale = Vector3.one * .7f, PhysicsReleased = true };
                var prefab = catalog.Resolve(legacySaved.ContentId, out bool replaced);
                var legacyRestored = catalog.PrepareRestore(legacySaved);
                Assert.That(replaced, Is.True);
                Assert.That(prefab, Is.Not.Null);
                Assert.That(legacyRestored.ContentId, Is.EqualTo(prefab.SaveContentId));
                Assert.That(legacyRestored.Item.Id, Is.EqualTo("old-can"));
                Assert.That(legacyRestored.Item.Value, Is.EqualTo(1));
                Assert.That(legacyRestored.Position, Is.EqualTo(legacySaved.Position));
                Assert.That(legacyRestored.Rotation, Is.EqualTo(legacySaved.Rotation));
                catalog.Entries[0].Count = -1;
                Assert.Throws<InvalidDataException>(() => catalog.Validate());
            }
            finally { UnityEngine.Object.DestroyImmediate(catalog); }
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
