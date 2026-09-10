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
            var extent = new Vector3(24,12,24); var layout = catalog.Generate(extent,seed);
            CollectionAssert.AreEqual(layout,catalog.Generate(extent,seed));
            Assert.That(layout.Length,Is.EqualTo(96));
            CollectionAssert.AreEqual(new[] {29,24,19,24}, catalog.Entries.Select(e=>e.Count));
            for(int index=0;index<catalog.Entries.Length;index++)
            {
                var entry=catalog.Entries[index];
                Assert.That(layout.Count(p=>p.PrefabIndex==index),Is.EqualTo(entry.Count));
                Assert.That(layout.Take(24).Count(p=>p.PrefabIndex==index),Is.EqualTo(entry.ShallowCount));
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
                foreach(var placement in layout.Where(p=>p.PrefabIndex==index))
                    foreach(var vertex in entry.Appearance(placement.AppearanceIndex).GetComponent<MeshFilter>().sharedMesh.vertices)
                    {
                        var world=placement.Position+placement.Rotation*vertex;
                        Assert.That(world.x,Is.InRange(0,extent.x)); Assert.That(world.y,Is.InRange(0,extent.y-.01f)); Assert.That(world.z,Is.InRange(0,extent.z));
                    }
            }
            for(int i=0;i<layout.Length;i++) for(int j=0;j<i;j++)
                Assert.That(Vector3.Distance(layout[i].Position,layout[j].Position),Is.GreaterThanOrEqualTo(1.15f-.0001f));
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
                foreach (var placement in catalog.Generate(new Vector3(24, 12, 24), seed))
                {
                    var entry = catalog.Entries[placement.PrefabIndex];
                    if (entry != rock) continue;
                    var prefab = entry.Appearance(placement.AppearanceIndex);
                    seen.Add(prefab.SaveContentId);
                    Assert.That(prefab.DisplayName, Is.EqualTo("Rock"));
                    Assert.That(prefab.SaleValue, Is.EqualTo(1));
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
        public void CurrentItemIdentityAndSavedValueSurviveChangingItsPrefabArt()
        {
            var original=Catalog;var catalog=UnityEngine.Object.Instantiate(original);
            var replacement=UnityEngine.Object.Instantiate(original.Entries[0].Prefab.gameObject);
            try
            {
                replacement.GetComponent<MeshFilter>().sharedMesh=original.Entries[1].Prefab.GetComponent<MeshFilter>().sharedMesh;
                replacement.GetComponent<MeshCollider>().sharedMesh=original.Entries[1].Prefab.GetComponent<MeshCollider>().sharedMesh;
                catalog.Entries[0]=new DiscoveryCatalog.Entry {Prefab=replacement.GetComponent<BuriedFind>(),Count=29,ShallowCount=10,LayOnSide=true};
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
