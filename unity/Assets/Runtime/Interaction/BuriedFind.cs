using UnityEngine;

namespace SomethingDownThere
{
    public enum FindSize { Small, Large }

    // The approved starter forms are ellipsoids. Sample their actual surface, not
    // the empty corners of an axis-aligned bounding box. Future art can author samples.
    [DisallowMultipleComponent, RequireComponent(typeof(MeshRenderer), typeof(MeshCollider))]
    public sealed class BuriedFind : MonoBehaviour
    {
        [SerializeField] private string saveContentId;
        public string SaveContentId => saveContentId;
        [SerializeField] private string displayName = "Blue marble";
        [SerializeField, Min(0)] private int saleValue = 5;
        [SerializeField] private FindSize size = FindSize.Small;
        [SerializeField, Range(0.1f, 1f)] private float collectionThreshold = 0.4f;
        [SerializeField] private Vector3[] exposureSamples;
        private TerrainVolume terrain;
        private MeshCollider hitCollider;
        private MeshRenderer visual;
        public InventoryItem Item { get; private set; }
        public float Exposure { get; private set; }
        public bool Collected { get; private set; }
        public FindSize Size => size;
        // Visibility/range are checked against the actual collider when collecting.
        public float RequiredExposure => Mathf.Clamp(collectionThreshold, 0.1f, 1f);
        public bool Collectible => Item != null && !Collected && Exposure >= RequiredExposure;
        public Bounds WorldBounds => visual.bounds;

        public void Initialize(TerrainVolume owner, string identity)
        {
            terrain = owner;
            hitCollider = GetComponent<MeshCollider>();
            visual = GetComponent<MeshRenderer>();
            Item = new InventoryItem(identity, displayName, saleValue);
            if (exposureSamples == null || exposureSamples.Length == 0)
            {
                exposureSamples = new Vector3[96];
                for (int i = 0; i < exposureSamples.Length; i++)
                {
                    float y = 1f - 2f * (i + 0.5f) / exposureSamples.Length;
                    float radius = Mathf.Sqrt(1f - y * y);
                    float angle = i * 2.39996323f;
                    exposureSamples[i] = new Vector3(Mathf.Cos(angle) * radius, y, Mathf.Sin(angle) * radius) * 0.5f;
                }
            }
            RefreshExposure();
        }

        public FindSnapshot Capture() => new FindSnapshot { ContentId = saveContentId, Item = ItemSnapshot.Capture(Item),
            Position = terrain.transform.InverseTransformPoint(transform.position), Rotation = Quaternion.Inverse(terrain.transform.rotation) * transform.rotation,
            Scale = transform.localScale, Collected = Collected };

        public void Restore(FindSnapshot state)
        {
            Item = state.Item.Restore();
            transform.SetPositionAndRotation(terrain.transform.TransformPoint(state.Position), terrain.transform.rotation * state.Rotation);
            transform.localScale = state.Scale;
            Collected = state.Collected;
            visual.enabled = hitCollider.enabled = !Collected;
            gameObject.SetActive(!Collected);
            RefreshExposure();
        }

        public void RefreshExposure()
        {
            if (Collected || terrain == null) return;
            int clear = 0;
            foreach (Vector3 sample in exposureSamples)
                if (!terrain.IsSolid(transform.TransformPoint(sample))) clear++;
            Exposure = clear / (float)exposureSamples.Length;
            // Keep the real surface targetable even when a newly visible sliver falls
            // between exposure samples. The nearest terrain collider still occludes it.
            hitCollider.enabled = true;
        }

        public string GetPrompt(FpsPlayer player)
        {
            if (Collected || !isActiveAndEnabled) return "";
            if (!Collectible) return $"Uncover more  |  {Mathf.RoundToInt(Exposure * 100)}% / {Mathf.RoundToInt(RequiredExposure * 100)}% exposed";
            return player.Inventory.IsFull ? "Inventory full" : Item.DisplayName;
        }

        public bool TryCollect(FpsPlayer player)
        {
            if (player == null || player.IsMenuOpen || !isActiveAndEnabled || !Collectible
                || terrain.IsSolid(player.ViewCamera.transform.position)
                || !player.TryGetTarget(player.Tuning.InteractReach, out var hit) || hit.collider != hitCollider) return false;
            if (!player.Inventory.TryAdd(Item))
            {
                player.ShowFeedback(player.Inventory.IsFull ? "Inventory full - find left in place" : "Find already carried");
                return false;
            }
            Collected = true;
            hitCollider.enabled = false;
            visual.enabled = false;
            player.ShowFeedback($"Collected {Item.DisplayName}  |  Finds {player.Inventory.Count}/{player.Inventory.Capacity}");
            gameObject.SetActive(false);
            return true;
        }
    }
}
