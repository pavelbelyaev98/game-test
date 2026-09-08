using UnityEngine;

namespace SomethingDownThere
{
    // Sample after player movement, before HUD LateUpdate. A feet-volume check also
    // handles standing still/resuming inside the zone without physics trigger events.
    [DisallowMultipleComponent, DefaultExecutionOrder(100)]
    public sealed class SurfaceRecharge : MonoBehaviour
    {
        [SerializeField] private FpsPlayer player;
        [SerializeField] private TerrainVolume terrain;
        [SerializeField] private Vector2 footprint = new Vector2(4f, 3f);
        [SerializeField, Min(0.01f)] private float maximumFeetHeight = 0.35f;
        private bool announcedThisVisit;
        private float lastRechargeTime = float.NegativeInfinity;

        public FpsPlayer Player => player;
        public TerrainVolume Terrain => terrain;
        public Vector2 Footprint => footprint;
        public bool IsPlayerInZone => player != null && ContainsFeet(player.FeetPosition);
        public bool RecentlyRecharged => isActiveAndEnabled && Time.time - lastRechargeTime < 3f;
        public bool IsNearby
        {
            get
            {
                if (!isActiveAndEnabled || player == null || terrain == null) return false;
                Vector3 feet = player.FeetPosition;
                float height = feet.y - terrain.SurfaceHeight;
                Vector3 local = transform.InverseTransformPoint(feet);
                return height >= 0f && height <= 1.8f
                    && new Vector2(local.x, local.z).sqrMagnitude <= 36f;
            }
        }

        public void Configure(FpsPlayer owner, TerrainVolume excavation)
        {
            player = owner;
            terrain = excavation;
        }

        public bool ContainsFeet(Vector3 feet)
        {
            if (!isActiveAndEnabled || terrain == null) return false;
            Vector3 local = transform.InverseTransformPoint(feet);
            float height = feet.y - terrain.SurfaceHeight;
            return height >= 0f && height <= maximumFeetHeight
                && Mathf.Abs(local.x) <= footprint.x * 0.5f
                && Mathf.Abs(local.z) <= footprint.y * 0.5f;
        }

        private void Update() => TryRecharge();

        public bool TryRecharge()
        {
            if (!isActiveAndEnabled || player == null || !player.GameplayActive || Time.timeScale <= 0f) return false;
            if (!IsPlayerInZone) { announcedThisVisit = false; return false; }
            if (player.Battery == null || player.Battery.Charge >= player.Battery.Capacity) return false;
            player.Battery.Recharge();
            if (!announcedThisVisit) lastRechargeTime = Time.time;
            announcedThisVisit = true;
            return true;
        }

        private void OnDisable()
        {
            announcedThisVisit = false;
            lastRechargeTime = float.NegativeInfinity;
        }
    }
}
