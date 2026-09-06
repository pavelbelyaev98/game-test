using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class LooseProp : MonoBehaviour
    {
        public string propId;
        public YardTarget target;
        public PortableBody portable;
        public float tossSpeed = 4;
        public LoosePropState State { get; private set; }
        public float RotationOffset { get; set; }

        public void Initialize(YardPlayer player)
        {
            State = new LoosePropState(propId, portable.Fallback);
            portable.Initialize(player.body);
        }

        public bool Stable => portable.InBounds && portable.Settled && portable.Clear(portable.Pose) && portable.Supported(portable.Pose);

        public void Observe()
        {
            if (State.IsHeld) return;
            if (!portable.InBounds) Recover();
            else State.Record(portable.Pose, Stable);
        }

        public void Recover()
        {
            if (!State.IsHeld && Stable) return;
            if (!portable.TryRecovery(State.SafePose, out var pose)) return;
            State.Release(pose, true);
            portable.SetPose(pose, true);
            RotationOffset = 0;
        }
    }
}
