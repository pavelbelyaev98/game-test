using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SomethingDownThere.Editor
{
    // One source item contract owns all three replaceable appearances and their gameplay tuning.
    public static class PhotoRockSetup
    {
        public const string Folder = "Assets/Content/PhotoRocks";
        private static string Source => Path.GetFullPath(Path.Combine(Application.dataPath, "../../art/photo-rock"));
        [Serializable] private sealed class RockCatalog
        {
            public int schema_version, instances, shallow_instances, sale_value, slots;
            public string item_id, display_name;
            public float required_exposure, mass_kg, throw_speed;
            public StarterFindSetup.SourceEntry[] appearances;
        }

        [MenuItem("Tools/Something Down There/Sync Rock Models")]
        public static void Sync() => StarterFindSetup.Sync();

        internal static void AppendToCatalog(List<DiscoveryCatalog.Entry> entries)
        {
            var source = JsonUtility.FromJson<RockCatalog>(File.ReadAllText(Path.Combine(Source, "catalog.json")));
            if (source == null || source.schema_version != 1 || source.item_id != "common_rock"
                || source.appearances == null || source.appearances.Length != 3 || source.slots != 1
                || source.instances < 1 || source.shallow_instances < 0 || source.shallow_instances > source.instances || source.sale_value < 0
                || string.IsNullOrWhiteSpace(source.display_name) || source.required_exposure != .6f
                || !float.IsFinite(source.mass_kg) || source.mass_kg <= 0)
                throw new InvalidDataException("Invalid three-appearance common rock source contract.");
            var prefabs = new List<BuriedFind>();
            var keys = new HashSet<string>(StringComparer.Ordinal);
            foreach (var appearance in source.appearances)
            {
                if (!keys.Add(appearance.content_id) || !System.Text.RegularExpressions.Regex.IsMatch(appearance.content_id ?? "", "^common_rock_[a-z0-9_]+$")
                    || !System.Text.RegularExpressions.Regex.IsMatch(appearance.atlas_group ?? "", "^[A-Za-z][A-Za-z0-9_]+$")
                    || appearance.dimensions_m == null || appearance.dimensions_m.Length != 3
                    || appearance.dimensions_m.Any(v => !float.IsFinite(v) || v <= 0))
                    throw new InvalidDataException("Invalid rock appearance key, dimensions or atlas group.");
                appearance.display_name = source.display_name;
                appearance.sale_value = source.sale_value; appearance.slots = source.slots;
                appearance.required_exposure = source.required_exposure;
                appearance.throw_speed = source.throw_speed;
                appearance.tier = "common"; appearance.detector_eligible = false;
                prefabs.Add(StarterFindSetup.ImportAppearance(appearance, Source, Folder, true, source.mass_kg));
            }
            entries.Add(new DiscoveryCatalog.Entry { ItemId = source.item_id, Prefab = prefabs[0],
                AppearanceVariants = prefabs.Skip(1).ToArray(), Count = source.instances,
                ShallowCount = source.shallow_instances, RandomOrientation = true });
        }
    }
}
