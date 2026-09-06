using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class RawCarrierView : MonoBehaviour
    {
        public YardTarget target;
        public Transform carryAnchor;
        public Transform[] restingPoints;
        public Collider[] parkedColliders;
        public GameObject label;
        public GameObject[] contents;
        public Transform contentDestination;
        public float TipBlend { get; set; }
        public Vector3 TipPosition { get; set; }

        public void Render(HarvestState state)
        {
            var pose = state.IsHeld ? carryAnchor : restingPoints[state.RestingPoint];
            foreach (var collider in parkedColliders) collider.enabled = !state.IsHeld;
            var position = pose.position;
            if (state.IsHeld)
            {
                // Tuck the visual toward the torso near a wall; it cannot push the controller or gate.
                var origin = carryAnchor.parent.position + carryAnchor.up * carryAnchor.localPosition.y;
                var delta = position - origin;
                if (Physics.BoxCast(origin + carryAnchor.up * .3f, new Vector3(.45f, .26f, .33f), delta.normalized,
                    out var hit, pose.rotation, delta.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                    position = origin + delta.normalized * Mathf.Max(0, hit.distance - .03f);
            }
            var rotation = pose.rotation;
            if (state.IsHeld && TipBlend > 0)
            {
                var forward = Vector3.ProjectOnPlane(carryAnchor.forward, Vector3.up).normalized;
                position = Vector3.Lerp(position, TipPosition, TipBlend);
                rotation = Quaternion.Slerp(rotation, Quaternion.LookRotation(forward) * Quaternion.Euler(0, 0, -58), TipBlend);
            }
            transform.SetPositionAndRotation(position, rotation);
            label.SetActive(!state.IsHeld);
            for (int i = 0; i < contents.Length; i++) contents[i].SetActive(i < state.RawUnits);
        }

        public int FindParkingPoint(YardPlayer player)
        {
            // Authored mats cannot stack, float, or become movable steps through a future access gate.
            if (!player.body.isGrounded || player.transform.position.y > .15f) return -1;
            for (int i = 0; i < restingPoints.Length; i++)
            {
                var pose = restingPoints[i];
                var distance = Vector3.Distance(player.transform.position, pose.position);
                if (distance > 2.3f || distance < .95f) continue;
                if (!Physics.Raycast(pose.position + Vector3.up * .15f, Vector3.down, out var ground, .3f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) continue;
                if (ground.normal.y < .98f || Mathf.Abs(ground.point.y) > .04f) continue;
                if (Physics.CheckBox(pose.position + Vector3.up * .33f, new Vector3(.48f, .28f, .37f), pose.rotation,
                    Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) continue;
                // No reaching a resting mat through a solid obstacle.
                if (Physics.Linecast(player.view.transform.position, pose.position + Vector3.up * .65f,
                    Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) continue;
                return i;
            }
            return -1;
        }
    }
}
