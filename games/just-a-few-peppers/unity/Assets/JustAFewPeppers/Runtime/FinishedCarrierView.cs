using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class FinishedCarrierView : MonoBehaviour
    {
        public PortableBody portable;
        public YardTarget target;
        public Transform dock;
        public Transform carryAnchor;
        public FoodGroupView food;
        public int capacity = 12;
        public float RotationOffset { get; private set; }
        public bool IsReceiving => receiving > 0;
        public CarrierPose DockPose => new CarrierPose(dock.position, dock.rotation);
        YardPlayer player;
        CarrierPose receiveStart;
        float receiving;

        public void Initialize(YardPlayer owner)
        {
            player = owner;
            portable.Initialize(owner.body);
            portable.Dock(DockPose);
        }

        public void Rotate(float amount) => RotationOffset = Mathf.Repeat(RotationOffset + amount, 360);
        public void BeginReceive()
        {
            receiveStart = DockPose;
            receiving = .45f;
        }

        public void Interrupt() => receiving = 0;

        public void Tick(float dt, HarvestState state)
        {
            receiving = Mathf.Max(0, receiving - dt);
            if (!state.FinishedDocked && !state.FinishedHeld)
            {
                if (!portable.InBounds) Recover(state, true);
                else state.RecordFinishedPose(portable.Pose,
                    portable.Settled && portable.Clear(portable.Pose) && portable.Supported(portable.Pose));
            }
            Render(state);
        }

        public void Render(HarvestState state)
        {
            food.Render(state.FinishedUnits);
            if (!state.FinishedHeld) return;
            var desired = new CarrierPose(carryAnchor.position, carryAnchor.rotation * Quaternion.Euler(0, RotationOffset, 0));
            if (IsReceiving)
            {
                float t = Mathf.SmoothStep(0, 1, 1 - receiving / .45f);
                desired.Position = Vector3.Lerp(receiveStart.Position, desired.Position, t);
                desired.Rotation = Quaternion.Slerp(receiveStart.Rotation, desired.Rotation, t);
            }
            portable.Hold(desired, player.transform.position + Vector3.up * .85f);
            state.RecordFinishedPose(portable.Pose, false);
        }

        public bool Release(HarvestState state, bool careful)
        {
            var pose = careful ? portable.Placement : portable.Pose;
            if (careful ? !portable.PlacementValid : !portable.Clear(pose, false)) return false;
            if (!state.ReleaseFinished(pose, careful)) return false;
            portable.ReleaseFromHand(pose, careful);
            return true;
        }

        public void Recover(HarvestState state, bool force = false)
        {
            Interrupt();
            if (state.FinishedDocked) { portable.Dock(DockPose); return; }
            if (!force && !state.FinishedHeld && portable.InBounds && portable.Settled &&
                portable.Clear(portable.Pose) && portable.Supported(portable.Pose)) return;
            if (!portable.TryRecovery(state.SafeFinishedPose, out var pose)) return;
            state.RecoverFinished(pose);
            portable.SetPose(pose, true);
            RotationOffset = 0;
        }

        public void ReturnToDock()
        {
            Interrupt();
            RotationOffset = 0;
            portable.Dock(DockPose);
        }
    }
}
