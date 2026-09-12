using System.Collections.Generic;
using UnityEngine;

namespace SomethingDownThere
{
    // Walking uses a small contact volume at the feet, not the aim/scoop radius.
    internal sealed class FindWalkCollection
    {
        private readonly FpsPlayer player;
        private readonly CharacterController motor;
        private readonly int worldMask;
        private readonly Collider[] overlaps = new Collider[32];
        private readonly RaycastHit[] blockers = new RaycastHit[32];
        private readonly List<BuriedFind> released = new List<BuriedFind>();
        private bool walking;
        private float Radius => motor.radius + .08f;
        private Vector3 Probe => player.FeetPosition + Vector3.up * .25f;

        public FindWalkCollection(FpsPlayer player, CharacterController motor, int mask)
        { this.player = player; this.motor = motor; worldMask = mask; }

        public void ExcludeUntilDeparture(BuriedFind find)
        { if (find != null && !released.Contains(find)) released.Add(find); }

        public void Clear() { released.Clear(); walking = false; }

        public bool Tick(Vector3 previousFeet, bool movementRequested)
        {
            for (int i = released.Count - 1; i >= 0; i--)
                if (released[i] == null || released[i].Collected || !released[i].isActiveAndEnabled
                    || HasDeparted(released[i])) released.RemoveAt(i);
            Vector3 travel = player.FeetPosition - previousFeet; travel.y = 0;
            walking = movementRequested && travel.sqrMagnitude > .000001f && motor.isGrounded && !player.IsJetpackActive;
            if (!walking || player.Inventory.IsFull || player.HeldFind != null) return false;
            int count = Physics.OverlapCapsuleNonAlloc(player.FeetPosition + Vector3.up * .12f,
                Probe, Radius, overlaps, worldMask, QueryTriggerInteraction.Ignore);
            // A saturated query is retried on the next movement tick.
            if (count == overlaps.Length) return false;
            BuriedFind nearest = null; float distance = float.PositiveInfinity;
            for (int i = 0; i < count; i++)
            {
                var find = overlaps[i].GetComponentInParent<BuriedFind>();
                if (find == null || !CanCollect(find) || !find.FullyUncovered) continue;
                float candidate = find.WorldBounds.SqrDistance(Probe);
                if (candidate >= distance) continue;
                nearest = find; distance = candidate;
            }
            return nearest != null && nearest.TryCollectAtFeet(player);
        }

        public bool CanCollect(BuriedFind find)
        {
            if (!walking || find == null || !find.isActiveAndEnabled || released.Contains(find)
                || player.IsMenuOpen || !player.HasGameplayFocus || player.HeldFind != null
                || (player.Persistence != null && player.Persistence.BlocksPlay)) return false;
            var terrain = player.ExcavationTerrain;
            if (terrain == null || terrain.IsRestoring || terrain.IsSolid(Probe)) return false;
            var bounds = find.WorldBounds;
            if (bounds.min.y > player.FeetPosition.y + .2f || bounds.max.y < player.FeetPosition.y - .08f) return false;
            Vector3 delta = find.HitCollider.ClosestPoint(Probe) - Probe;
            if (delta.sqrMagnitude > Radius * Radius) return false;
            float length = delta.magnitude;
            if (length < .001f) return true; // The player can walk through free finds.
            int count = Physics.RaycastNonAlloc(Probe, delta / length, blockers, length + .005f,
                worldMask, QueryTriggerInteraction.Ignore);
            if (count == blockers.Length) return false;
            for (int i = 0; i < count; i++)
                if (blockers[i].collider != find.HitCollider && blockers[i].collider != motor) return false;
            return true;
        }

        private bool HasDeparted(BuriedFind find)
        {
            var bounds = find.WorldBounds;
            Vector3 horizontal = Probe; horizontal.y = bounds.center.y;
            return bounds.SqrDistance(horizontal) > 1f;
        }
    }
}
