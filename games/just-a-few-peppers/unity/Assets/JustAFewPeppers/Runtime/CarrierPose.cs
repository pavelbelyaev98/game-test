using System;
using UnityEngine;

namespace JustAFewPeppers
{
    // Value data only: no scene references, body or second food inventory.
    [Serializable]
    public struct CarrierPose
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public CarrierPose(Vector3 position, Quaternion rotation) { Position = position; Rotation = rotation; }
        public static CarrierPose Origin => new CarrierPose(Vector3.zero, Quaternion.identity);
        public bool IsValid => Finite(Position.x) && Finite(Position.y) && Finite(Position.z) &&
            Finite(Rotation.x) && Finite(Rotation.y) && Finite(Rotation.z) && Finite(Rotation.w) &&
            Mathf.Abs(Quaternion.Dot(Rotation, Rotation) - 1) < .01f;
        static bool Finite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    }
}
