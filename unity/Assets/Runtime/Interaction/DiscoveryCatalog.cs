using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SomethingDownThere
{
    // Generated from the editable source catalog. IDs describe items, never mesh GUIDs.
    [CreateAssetMenu(menuName = "Something Down There/Discovery catalog")]
    public sealed class DiscoveryCatalog : ScriptableObject
    {
        [Serializable] public sealed class Entry
        {
            public string ItemId;
            public BuriedFind Prefab;
            // One catalog item, several saved appearances with identical gameplay specifications.
            public BuriedFind[] AppearanceVariants = Array.Empty<BuriedFind>();
            public int Count, ShallowCount;
            public bool LayOnSide, RandomOrientation;
            public int AppearanceCount => 1 + (AppearanceVariants?.Length ?? 0);
            public BuriedFind Appearance(int index) => index == 0 ? Prefab : AppearanceVariants[index - 1];
        }
        [Serializable] public sealed class LegacyAlias
        {
            public string OldId, CurrentId;
        }
        public Entry[] Entries = Array.Empty<Entry>();
        public LegacyAlias[] LegacyAliases = Array.Empty<LegacyAlias>();
        public int TotalCount { get { int total = 0; foreach (var e in Entries) total += e.Count; return total; } }

        public void Validate()
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            int shallow = 0;
            if (Entries == null || Entries.Length == 0) throw new InvalidDataException("Missing discovery catalog.");
            foreach (var e in Entries)
            {
                if (e == null || e.Prefab == null || string.IsNullOrWhiteSpace(e.Prefab.SaveContentId)
                    || !ids.Add(e.Prefab.SaveContentId) || e.Count < 1 || e.ShallowCount < 0 || e.ShallowCount > e.Count)
                    throw new InvalidDataException("Invalid discovery catalog entry.");
                shallow += e.ShallowCount;
                for (int i = 1; i < e.AppearanceCount; i++)
                {
                    var appearance = e.Appearance(i);
                    if (appearance == null || string.IsNullOrWhiteSpace(appearance.SaveContentId)
                        || !ids.Add(appearance.SaveContentId) || appearance.DisplayName != e.Prefab.DisplayName
                        || appearance.SaleValue != e.Prefab.SaleValue || appearance.Size != e.Prefab.Size
                        || appearance.DetectorEligible != e.Prefab.DetectorEligible
                        || !Mathf.Approximately(appearance.RequiredExposure, e.Prefab.RequiredExposure))
                        throw new InvalidDataException("Item appearances must have unique save keys and matching gameplay specifications.");
                }
            }
            if (TotalCount > 256 || shallow != 24) throw new InvalidDataException("Starter allocation requires 24 shallow finds and at most 256 total.");
            var aliases = new HashSet<string>(StringComparer.Ordinal);
            foreach (var a in LegacyAliases)
                if (a == null || string.IsNullOrWhiteSpace(a.OldId) || ids.Contains(a.OldId)
                    || !aliases.Add(a.OldId) || !ids.Contains(a.CurrentId))
                    throw new InvalidDataException("Invalid legacy discovery mapping.");
        }

        public BuriedFind Resolve(string id, out bool legacy)
        {
            legacy = false;
            foreach (var alias in LegacyAliases)
                if (alias.OldId == id) { id = alias.CurrentId; legacy = true; break; }
            foreach (var entry in Entries)
                for (int i = 0; i < entry.AppearanceCount; i++)
                    if (entry.Appearance(i).SaveContentId == id) return entry.Appearance(i);
            throw new InvalidDataException("This save needs discovery content missing from this game version.");
        }

        // Content migration 1: legacy primitive GUIDs -> stable catalog keys.
        // The binary v1/v2 readers are unchanged. Never mutate the decoded recovery record.
        public FindSnapshot PrepareRestore(FindSnapshot saved)
        {
            var prefab = Resolve(saved.ContentId, out bool legacy);
            return new FindSnapshot {
                ContentId = prefab.SaveContentId, Position = saved.Position, Rotation = saved.Rotation,
                Scale = legacy ? Vector3.one : saved.Scale, Collected = saved.Collected, PhysicsReleased = saved.PhysicsReleased,
                Item = new ItemSnapshot { Id = saved.Item.Id, Value = saved.Item.Value,
                    Name = legacy && !saved.Collected ? prefab.DisplayName : saved.Item.Name }
            };
        }

        public DiscoveryPlacement[] Generate(Vector3 extent, int seed)
        {
            Validate();
            var layout = DiscoveryField.Generate(extent, TotalCount, seed);
            var shallow = new List<int>(); var remaining = new List<int>();
            for (int i = 0; i < Entries.Length; i++)
                for (int n = 0; n < Entries[i].Count; n++)
                    (n < Entries[i].ShallowCount ? shallow : remaining).Add(i);
            var random = new System.Random(unchecked(seed ^ 0x45A7123));
            var appearances = new System.Random(unchecked(seed ^ 0x72BD139));
            Shuffle(shallow, random); Shuffle(remaining, random); shallow.AddRange(remaining);
            for (int i = 0; i < layout.Length; i++)
            {
                int index = shallow[i];
                float yaw = (float)random.NextDouble() * 360;
                float tilt = ((float)random.NextDouble() - .5f) * 24;
                bool side = Entries[index].LayOnSide && random.NextDouble() < .85;
                var rotation = Quaternion.Euler((side ? 90 : 0) + tilt, yaw, ((float)random.NextDouble() - .5f) * 18);
                if (Entries[index].RandomOrientation)
                {
                    // Uniform quaternion sampling: every 3D orientation is equally likely.
                    double u = appearances.NextDouble(), v = appearances.NextDouble() * Math.PI * 2,
                        w = appearances.NextDouble() * Math.PI * 2;
                    rotation = new Quaternion((float)(Math.Sqrt(1-u)*Math.Sin(v)), (float)(Math.Sqrt(1-u)*Math.Cos(v)),
                        (float)(Math.Sqrt(u)*Math.Sin(w)), (float)(Math.Sqrt(u)*Math.Cos(w)));
                }
                layout[i] = new DiscoveryPlacement(layout[i].Position, rotation, index, appearances.Next(Entries[index].AppearanceCount));
            }
            return layout;
        }

        private static void Shuffle(List<int> values, System.Random random)
        {
            for (int i = values.Count - 1; i > 0; i--)
            { int j = random.Next(i + 1); int value = values[i]; values[i] = values[j]; values[j] = value; }
        }
    }
}
