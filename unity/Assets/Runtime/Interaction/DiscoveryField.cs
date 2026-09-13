using System;
using System.Collections.Generic;
using UnityEngine;

namespace SomethingDownThere
{
    public readonly struct DiscoveryPlacement
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly int PrefabIndex;
        public readonly int AppearanceIndex;
        public DiscoveryPlacement(Vector3 position, Quaternion rotation, int prefabIndex, int appearanceIndex = 0)
        { Position = position; Rotation = rotation; PrefabIndex = prefabIndex; AppearanceIndex = appearanceIndex; }
    }

    [DisallowMultipleComponent]
    public sealed class DiscoveryField : MonoBehaviour
    {
        [SerializeField] private TerrainVolume terrain;
        [SerializeField] private BuriedFind[] prefabs;
        [SerializeField] private DiscoveryCatalog catalog;
        public DiscoveryCatalog Catalog => catalog;
        [SerializeField] private int seed = 90127;
        [SerializeField, Range(1, MaximumPopulation)] private int count = 96;
        [Tooltip("Keep the user-requested simple review shapes out of release gameplay until final-art acceptance.")]
        [SerializeField] private bool developmentContent = true;
        private readonly List<BuriedFind> finds = new List<BuriedFind>();
        private bool initialized;
        private bool generationDeferred;
        public IReadOnlyList<BuriedFind> Finds => finds;
        public int Seed => seed;
        public long MotionRevision { get; private set; }
        public Collider PlayerCollider { get; private set; }
        internal void NotifyMotion() => MotionRevision++;
        public bool Initialized => initialized || (developmentContent && !FpsPlayer.AdminBuild);
        public void DeferGeneration() => generationDeferred = true;

        public FindSnapshot[] Capture()
        {
            var states = new FindSnapshot[finds.Count];
            for (int i = 0; i < states.Length; i++) states[i] = finds[i].Capture();
            return states;
        }

        public void ValidateRestore(FindSnapshot[] states)
        {
            if (catalog != null)
            {
                catalog.Validate();
                foreach (var state in states) catalog.Resolve(state.ContentId, out _);
                return;
            }
            if (developmentContent && !FpsPlayer.AdminBuild && states.Length > 0)
                throw new System.IO.InvalidDataException("This save uses development discoveries unavailable in this build.");
            foreach (var state in states)
                if (Array.Find(prefabs, p => p != null && p.SaveContentId == state.ContentId) == null)
                    throw new System.IO.InvalidDataException("This save needs discovery content missing from this game version.");
        }

        public void Restore(FindSnapshot[] states, int savedSeed)
        {
            ValidateRestore(states);
            foreach (var find in finds) { find.gameObject.SetActive(false); Destroy(find.gameObject); }
            finds.Clear();
            seed = savedSeed;
            foreach (var saved in states)
            {
                var state = catalog != null ? catalog.PrepareRestore(saved) : saved;
                var prefab = catalog != null ? catalog.Resolve(state.ContentId, out _) : Array.Find(prefabs, p => p.SaveContentId == state.ContentId);
                var find = Instantiate(prefab, transform);
                find.Initialize(terrain, state.Item.Id, this);
                find.Restore(state);
                find.name = state.Item.Name + " " + state.Item.Id;
                finds.Add(find);
            }
            initialized = true;
        }

        private void OnEnable()
        {
            PlayerCollider = transform.root.GetComponentInChildren<CharacterController>();
            if (terrain != null) terrain.Changed += HandleExcavationChanged;
            if (initialized) foreach (var find in finds) find.RefreshExposure();
        }

        private void OnDisable()
        {
            if (terrain != null) terrain.Changed -= HandleExcavationChanged;
        }

        private void Start()
        {
            if (!generationDeferred) InitializePopulation();
        }

        public void InitializePopulation()
        {
            if (initialized) return;
            if (developmentContent && !FpsPlayer.AdminBuild) { gameObject.SetActive(false); return; }
            if (terrain == null || (catalog == null && (prefabs == null || prefabs.Length != 3 || Array.Exists(prefabs, p => p == null))))
            {
                Debug.LogError("Discoveries require terrain and three find prefabs.", this);
                enabled = false;
                return;
            }
            var extent = (Vector3)terrain.Dimensions * terrain.CellSize;
            var placements = catalog != null ? catalog.Generate(extent, seed) : Generate(extent, count, seed);
            for (int i = 0; i < placements.Length; i++)
            {
                var placement = placements[i];
                var source = catalog != null ? catalog.Entries[placement.PrefabIndex].Appearance(placement.AppearanceIndex) : prefabs[placement.PrefabIndex];
                var find = Instantiate(source, terrain.transform.TransformPoint(placement.Position),
                    terrain.transform.rotation * placement.Rotation, transform);
                find.Initialize(terrain, $"find-{seed}-{i:D3}", this);
                find.name = find.Item.DisplayName + " " + i;
                finds.Add(find);
            }
            initialized = true;
        }

        private void HandleExcavationChanged(Bounds changed)
        {
            foreach (var find in finds)
                if (!find.Collected && changed.Intersects(find.WorldBounds)) find.RefreshExposure();
        }

        // Separate deterministic stream from excavation. The enlarged starter forms
        // fit within 0.5 m of their centers in every rotation, with soil between them.
        public const float MinimumSpacing = 1.15f;
        public const float MaximumFindRadius = 0.5f;
        public const float SoilClearance = 0.10f;
        public const int MaximumPopulation = 1024;
        public static DiscoveryPlacement[] Generate(Vector3 extent, int total, int placementSeed)
            => Generate(extent, total, placementSeed, Math.Min(total, 24));

        public static DiscoveryPlacement[] Generate(Vector3 extent, int total, int placementSeed, int shallowCount)
            => Generate(extent, total, placementSeed, shallowCount, null);

        // Catalog placements use the enclosing radius of each approved mesh. A small
        // bottle does not need the same empty soil envelope as a large rock.
        public static DiscoveryPlacement[] Generate(Vector3 extent, int total, int placementSeed, int shallowCount, float[] radii)
        {
            if (!ExcavationGrid.Finite(extent.x) || !ExcavationGrid.Finite(extent.y) || !ExcavationGrid.Finite(extent.z)
                || extent.x < 8 || extent.y < 4 || extent.z < 8 || total < 1 || total > MaximumPopulation
                || shallowCount < 0 || shallowCount > total)
                throw new ArgumentOutOfRangeException(nameof(total), "Use a site at least 8 x 4 x 8 m and a supported discovery population.");
            if (radii != null && (radii.Length != total || Array.Exists(radii, r => !ExcavationGrid.Finite(r) || r <= 0 || r > MaximumFindRadius)))
                throw new ArgumentOutOfRangeException(nameof(radii));
            var random = new System.Random(placementSeed);
            var result = new DiscoveryPlacement[total];
            float Range(float min, float max) => Mathf.Lerp(min, max, (float)random.NextDouble());
            for (int i = 0; i < total; i++)
            {
                bool placed = false;
                bool shallow = i < shallowCount;
                float bestDistance = -1;
                Vector3 best = default;
                // Best of 64 candidates spreads common encounters without rows or a fixed route.
                // Preserve the established spread first; denser allocations can fill
                // remaining gaps with the smaller, still nonoverlapping soil envelope.
                // Both passes are bounded rather than dropping an identity.
                int maxAttempts = radii == null ? 2000 : 4000;
                for (int attempt = 0; attempt < maxAttempts && (!placed || (shallow && attempt < 64)); attempt++)
                {
                    float x = i < 6 ? Range(extent.x * 0.5f - 2.5f, extent.x * 0.5f + 2.5f) : Range(0.8f, extent.x - 0.8f);
                    // The catalog covers the whole layer; the legacy three-prefab
                    // validation field keeps its small entrance allocation.
                    float z = i < 6 ? Range(1, 3.5f) : radii == null && i < Math.Min(shallowCount, 48)
                        ? Range(0.8f, 6) : Range(0.8f, extent.z - 0.8f);
                    float depth = shallow ? Range(0.65f, 1.1f) : i < shallowCount + 36 ? Range(1.2f, Mathf.Min(3.5f, extent.y - 0.8f))
                        : Range(2.5f, extent.y - 0.8f);
                    var position = new Vector3(x, extent.y - depth, z);
                    bool clear = true;
                    float nearest = float.MaxValue;
                    for (int j = 0; j < i; j++)
                    {
                        var delta = result[j].Position - position;
                        float clearance = attempt < 2000 ? 0.15f : SoilClearance;
                        float spacing = radii == null ? MinimumSpacing : radii[i] + radii[j] + clearance;
                        if (delta.sqrMagnitude < spacing * spacing) { clear = false; break; }
                        nearest = Mathf.Min(nearest, delta.x * delta.x + delta.z * delta.z);
                    }
                    if (!clear || nearest <= bestDistance) continue;
                    placed = true;
                    bestDistance = nearest;
                    best = position;
                }
                if (!placed) throw new InvalidOperationException("The discovery density is too high for this site.");
                result[i] = new DiscoveryPlacement(best, Quaternion.Euler(Range(0, 360), Range(0, 360), Range(0, 360)), i % 3);
            }
            return result;
        }
    }
}
