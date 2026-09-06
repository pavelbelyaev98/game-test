using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class RawCarrierView : MonoBehaviour
    {
        public YardTarget target;
        public Transform carryAnchor;
        // Retained only for the one-time migration of the older scene.
        [HideInInspector] public Transform[] restingPoints;
        [HideInInspector] public Collider[] parkedColliders;
        public PortableBody portable;
        public GameObject label;
        public GameObject[] contents;
        public Transform contentDestination;
        public float TipBlend { get; set; }
        public Vector3 TipPosition { get; set; }
        public float RotationOffset { get; private set; }
        YardPlayer player;

        public void Initialize(YardPlayer owner)
        {
            player = owner;
            portable.Initialize(owner.body);
        }

        public void Rotate(float amount) => RotationOffset = Mathf.Repeat(RotationOffset + amount, 360);

        public void Render(HarvestState state)
        {
            if (state.IsHeld)
            {
                var position = carryAnchor.position;
                var rotation = carryAnchor.rotation * Quaternion.Euler(0, RotationOffset, 0);
                if (TipBlend > 0)
                {
                    var forward = Vector3.ProjectOnPlane(carryAnchor.forward, Vector3.up).normalized;
                    position = Vector3.Lerp(position, TipPosition, TipBlend);
                    rotation = Quaternion.Slerp(rotation, Quaternion.LookRotation(forward) * Quaternion.Euler(0, 0, -58), TipBlend);
                }
                var origin = TipBlend > 0 ? player.view.transform.position + Vector3.up * .4f :
                    player.transform.position + Vector3.up * .85f;
                portable.Hold(new CarrierPose(position, rotation), origin);
                state.RecordCarrierPose(portable.Pose, false);
            }
            label.SetActive(!state.IsHeld);
            for (int i = 0; i < contents.Length; i++) contents[i].SetActive(i < state.RawUnits);
        }

        public void ObserveReleased(HarvestState state)
        {
            if (state.IsHeld) return;
            if (!portable.InBounds) { Recover(state, true); return; }
            state.RecordCarrierPose(portable.Pose, portable.Settled && portable.Clear(portable.Pose) && portable.Supported(portable.Pose));
        }

        public bool Release(HarvestState state, bool careful)
        {
            var pose = careful ? portable.Placement : portable.Pose;
            if (careful ? !portable.PlacementValid : !portable.Clear(pose, false)) return false;
            if (!state.Release(pose, careful)) return false;
            portable.SetPose(pose, careful);
            return true;
        }

        public void Recover(HarvestState state, bool force = false)
        {
            // A valid player arrangement stays where it was left. Only held/lost/moving/stuck crates move.
            if (!force && !state.IsHeld && portable.InBounds && portable.Settled &&
                portable.Clear(portable.Pose) && portable.Supported(portable.Pose)) return;
            if (!portable.TryRecovery(state.SafeRawPose, out var pose)) return;
            state.RecoverCarrier(pose);
            portable.SetPose(pose, true);
            RotationOffset = 0;
        }

        public void ResetPose(HarvestState state)
        {
            RotationOffset = 0;
            portable.SetPose(state.RawPose, true);
        }
    }
}
