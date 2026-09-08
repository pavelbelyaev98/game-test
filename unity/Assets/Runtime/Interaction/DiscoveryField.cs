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
        public DiscoveryPlacement(Vector3 position, Quaternion rotation, int prefabIndex)
        { Position = position; Rotation = rotation; PrefabIndex = prefabIndex; }
    }

    [DisallowMultipleComponent]
    public sealed class DiscoveryField : MonoBehaviour
    {
        [SerializeField] private TerrainVolume terrain;
        [SerializeField] private BuriedFind[] prefabs;
        [SerializeField] private int seed = 90127;
        [SerializeField, Range(1, 256)] private int count = 96;
        [Tooltip("Keep the user-requested simple review shapes out of release gameplay until final-art acceptance.")]
        [SerializeField] private bool developmentContent = true;
        private readonly List<BuriedFind> finds = new List<BuriedFind>();
        private bool initialized;
        public IReadOnlyList<BuriedFind> Finds => finds;
        public int Seed => seed;

        private void OnEnable()
        {
            if (terrain != null) terrain.Changed += HandleExcavationChanged;
            if (initialized) foreach (var find in finds) find.RefreshExposure();
        }

        private void OnDisable()
        {
            if (terrain != null) terrain.Changed -= HandleExcavationChanged;
        }

        private void Start()
        {
            if (developmentContent && !FpsPlayer.AdminBuild) { gameObject.SetActive(false); return; }
            if (terrain == null || prefabs == null || prefabs.Length != 3 || Array.Exists(prefabs, p => p == null))
            {
                Debug.LogError("Discoveries require terrain and three find prefabs.", this);
                enabled = false;
                return;
            }
            var placements = Generate((Vector3)terrain.Dimensions * terrain.CellSize, count, seed);
            for (int i = 0; i < placements.Length; i++)
            {
                var placement = placements[i];
                var find = Instantiate(prefabs[placement.PrefabIndex], terrain.transform.TransformPoint(placement.Position),
                    terrain.transform.rotation * placement.Rotation, transform);
                find.Initialize(terrain, $"find-{seed}-{i:D3}");
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

        // Separate deterministic stream from excavation. Centers have at least 0.9 m
        // clearance; the starter prefabs fit inside a 0.3 m radius in any rotation.
        public static DiscoveryPlacement[] Generate(Vector3 extent, int total, int placementSeed)
        {
            if (!ExcavationGrid.Finite(extent.x) || !ExcavationGrid.Finite(extent.y) || !ExcavationGrid.Finite(extent.z)
                || extent.x < 8 || extent.y < 4 || extent.z < 8 || total < 1 || total > 256)
                throw new ArgumentOutOfRangeException(nameof(total), "Use a site at least 8 x 4 x 8 m and 1-256 finds.");
            var random = new System.Random(placementSeed);
            var result = new DiscoveryPlacement[total];
            float Range(float min, float max) => Mathf.Lerp(min, max, (float)random.NextDouble());
            for (int i = 0; i < total; i++)
            {
                bool placed = false;
                for (int attempt = 0; attempt < 2000 && !placed; attempt++)
                {
                    float x = i < 6 ? Range(extent.x * 0.5f - 2.5f, extent.x * 0.5f + 2.5f) : Range(0.8f, extent.x - 0.8f);
                    float z = i < 6 ? Range(1, 3.5f) : i < 24 ? Range(0.8f, 6) : Range(0.8f, extent.z - 0.8f);
                    float depth = i < 24 ? Range(0.55f, 1.25f) : i < 60 ? Range(1.2f, Mathf.Min(3.5f, extent.y - 0.8f))
                        : Range(2.5f, extent.y - 0.8f);
                    var position = new Vector3(x, extent.y - depth, z);
                    placed = true;
                    for (int j = 0; j < i; j++)
                        if ((result[j].Position - position).sqrMagnitude < 0.81f) { placed = false; break; }
                    if (placed) result[i] = new DiscoveryPlacement(position, Quaternion.Euler(Range(0, 360), Range(0, 360), Range(0, 360)), i % 3);
                }
                if (!placed) throw new InvalidOperationException("The discovery density is too high for this site.");
            }
            return result;
        }
    }
}
