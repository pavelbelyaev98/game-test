using System;

namespace JustAFewPeppers
{
    // Session pose ownership only. Props have no food, score or completion state.
    public sealed class LoosePropState
    {
        public string Id { get; }
        public bool IsHeld { get; private set; }
        public CarrierPose Pose { get; private set; }
        public CarrierPose SafePose { get; private set; }

        public LoosePropState(string id, CarrierPose initial)
        {
            if (string.IsNullOrWhiteSpace(id) || !initial.IsValid) throw new ArgumentException("Invalid authored prop identity/pose.");
            Id = id;
            Pose = SafePose = initial;
        }

        public void PickUp() => IsHeld = true;
        public bool Release(CarrierPose pose, bool safe)
        {
            if (!pose.IsValid) return false;
            IsHeld = false;
            Record(pose, safe);
            return true;
        }

        public void Record(CarrierPose pose, bool safe)
        {
            if (!pose.IsValid) return;
            Pose = pose;
            if (safe && !IsHeld) SafePose = pose;
        }
    }
}
