using UnityEngine;

namespace SomethingDownThere
{
    // One physical stance drives collision, eye height and snapshot state. It does
    // not own input, vertical velocity or grounding; those remain with FpsPlayer.
    public sealed class PlayerCrouch
    {
        private readonly CharacterController motor;
        private readonly Transform eye;
        private readonly Camera camera;
        private readonly float authoredNearClip;
        private readonly FpsTuning tuning;
        private readonly int worldMask;
        private readonly float standingHeight;
        private readonly Vector3 standingEye, feetOffset;
        private readonly Collider[] overlaps = new Collider[32];
        private bool held;

        public float Amount { get; private set; }
        public bool StandBlocked { get; private set; }
        public bool IsPrecision => held || Amount > 0f;
        public float SpeedMultiplier => IsPrecision ? Mathf.Clamp(tuning.CrouchSpeedMultiplier, 0.05f, 1f) : 1f;

        public PlayerCrouch(CharacterController controller, Camera camera, FpsTuning settings, int blockers)
        {
            motor = controller;
            eye = camera.transform;
            this.camera = camera;
            authoredNearClip = camera.nearClipPlane;
            tuning = settings;
            worldMask = blockers;
            standingHeight = motor.height;
            standingEye = eye.localPosition;
            feetOffset = motor.center - Vector3.up * (standingHeight * 0.5f);
        }

        private float CrouchedHeight => Mathf.Clamp(tuning.CrouchHeight, motor.radius * 2f, standingHeight);
        private float Blend(float amount) => amount * amount * (3f - 2f * amount);
        private float Height(float amount) => Mathf.Lerp(standingHeight, CrouchedHeight, Blend(amount));
        // Match the controller's permitted contact skin without expanding through
        // a roof. Keep a small extra margin beyond its effective collision shell.
        private float QueryRadius => Mathf.Max(0.01f, motor.radius - motor.skinWidth + 0.005f);

        public bool Tick(bool requested, float deltaTime)
        {
            bool released = held && !requested;
            held = requested;
            StandBlocked = !held && Amount > 0f && !CanStand();
            float next = Mathf.MoveTowards(Amount, held || StandBlocked ? 1f : 0f,
                deltaTime / Mathf.Max(0.01f, tuning.CrouchTransitionSeconds));
            if (next != Amount) Apply(next);
            UpdateProjection();
            return released && StandBlocked;
        }

        private bool CanStand()
        {
            var root = motor.transform;
            Vector3 feet = root.TransformPoint(feetOffset);
            // Only the additional upper volume needs space. A full capsule test
            // here would confuse the floor/slope contact with a blocked stand-up.
            Vector3 bottom = feet + root.up * (motor.height - motor.radius);
            Vector3 top = feet + root.up * (standingHeight - motor.radius);
            return !Blocked(bottom, top, QueryRadius);
        }

        private bool Blocked(Vector3 bottom, Vector3 top, float radius)
        {
            int count = Physics.OverlapCapsuleNonAlloc(bottom, top, radius, overlaps, worldMask, QueryTriggerInteraction.Ignore);
            if (count == overlaps.Length) return true; // Conservatively handle crowded geometry.
            for (int i = 0; i < count; i++)
            {
                var obstacle = overlaps[i];
                if (obstacle == motor || obstacle.transform.IsChildOf(motor.transform)
                    || Physics.GetIgnoreLayerCollision(motor.gameObject.layer, obstacle.gameObject.layer)
                    || Physics.GetIgnoreCollision(motor, obstacle)) continue;
                return true;
            }
            return false;
        }

        public bool CanRestore(float amount, Vector3 position, Quaternion rotation, TerrainVolume terrain)
        {
            if (!WorldSnapshot.Finite(amount) || amount < 0f || amount > 1f) return false;
            Vector3 up = rotation * Vector3.up;
            Vector3 feet = position + rotation * feetOffset;
            float height = Height(amount);
            Vector3 bottom = feet + up * motor.radius;
            Vector3 top = feet + up * (height - motor.radius);
            if (Blocked(bottom, top, QueryRadius)) return false;
            // A capsule wholly inside soil may overlap no mesh triangles. Check
            // the saved terrain's interior as well as its restored surface meshes.
            if (terrain != null)
            {
                float inset = motor.radius - QueryRadius;
                int steps = Mathf.Max(1, Mathf.CeilToInt((height - 2f * inset) / (terrain.CellSize * 0.5f)));
                for (int i = 0; i <= steps; i++)
                    if (terrain.IsSolid(feet + up * Mathf.Lerp(inset, height - inset, (float)i / steps))) return false;
            }
            return true;
        }

        public void Restore(float amount)
        {
            held = false;
            StandBlocked = false;
            Apply(amount);
            UpdateProjection();
        }

        public void UpdateProjection()
        {
            // Keep every near-plane corner inside the controller's effective
            // capsule even at extreme pitch/FOV/aspect. Projection changes never
            // move the eye or change FOV, input sensitivity or interaction reach.
            Vector3 axisPoint = feetOffset;
            axisPoint.y = Mathf.Clamp(eye.localPosition.y, feetOffset.y + motor.radius,
                feetOffset.y + motor.height - motor.radius);
            float room = Mathf.Max(0.005f, motor.radius - motor.skinWidth - 0.01f
                - Vector3.Distance(eye.localPosition, axisPoint));
            float tangent = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad * 0.5f);
            float cornerFactor = Mathf.Sqrt(1f + tangent * tangent * (1f + camera.aspect * camera.aspect));
            float near = Mathf.Max(0.005f, Mathf.Min(authoredNearClip, room / cornerFactor));
            if (camera.nearClipPlane != near) camera.nearClipPlane = near;
        }

        private void Apply(float amount)
        {
            Amount = amount;
            float blend = Blend(amount);
            float height = Height(amount);
            Vector3 center = feetOffset + Vector3.up * (height * 0.5f);
            if (motor.height != height) motor.height = height;
            if (motor.center != center) motor.center = center;
            Vector3 cameraPosition = standingEye;
            float crouchedEye = Mathf.Clamp(tuning.CrouchEyeHeight, feetOffset.y + motor.radius,
                feetOffset.y + CrouchedHeight - 0.1f);
            cameraPosition.y = Mathf.Lerp(standingEye.y, crouchedEye, blend);
            eye.localPosition = cameraPosition;
        }
    }
}
