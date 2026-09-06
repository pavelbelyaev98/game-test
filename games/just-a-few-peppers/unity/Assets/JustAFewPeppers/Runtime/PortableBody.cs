using UnityEngine;

namespace JustAFewPeppers
{
    // Geometry and motion for a portable object. The caller owns identity, contents and recorded poses.
    public sealed class PortableBody : MonoBehaviour
    {
        public Rigidbody body;
        public BoxCollider shape;
        public SphereCollider roundShape;
        public Collider[] additionalShapes = System.Array.Empty<Collider>();
        // Optional conservative envelope for compound basin/crate/stool placement and holding.
        public Vector3 handlingSize;
        public Vector3 handlingCenter;
        public Collider CollisionShape => roundShape != null ? (Collider)roundShape : shape;
        Collider[] shapes;
        public Transform recoveryPoint;
        public LineRenderer preview; // Retained scene reference; placement previews are currently hidden.
        public AudioSource contactAudio;
        public AudioClip contactClip;
        public float reach = 3f;
        public bool PlacementValid { get; private set; }
        public CarrierPose Placement { get; private set; }
        public string PlacementReason { get; private set; } = "Aim at a surface within reach";
        public CarrierPose Pose => new CarrierPose(transform.position, transform.rotation);
        public CarrierPose Fallback => new CarrierPose(recoveryPoint.position, recoveryPoint.rotation);
        public bool InBounds => Pose.IsValid && Mathf.Abs(transform.position.x) < 8.65f &&
            Mathf.Abs(transform.position.z) < 8.65f && transform.position.y > -2 && transform.position.y < 6;
        public bool Settled => body.IsSleeping() || (body.linearVelocity.sqrMagnitude < .003f && body.angularVelocity.sqrMagnitude < .003f);
        public int ContactCues { get; private set; }
        CharacterController player;
        bool held;
        bool ignoringPlayer;
        float lastContact = -10;
        int motionFrame = -1;
        Vector3 motionPosition;
        Vector3 releaseVelocity;
        readonly Collider[] overlaps = new Collider[64];
        readonly RaycastHit[] hits = new RaycastHit[64];
        Vector3 Center => handlingSize != Vector3.zero ? handlingCenter :
            roundShape != null ? roundShape.center : shape.center;
        Vector3 Half => handlingSize != Vector3.zero ? handlingSize * .5f :
            roundShape != null ? Vector3.one * roundShape.radius : shape.size * .5f;
        float Bottom => Center.y - Half.y;

        public void Initialize(CharacterController controller)
        {
            player = controller;
            shapes = new Collider[1 + additionalShapes.Length];
            shapes[0] = CollisionShape;
            additionalShapes.CopyTo(shapes, 1);
            SetPose(Fallback, true);
            HidePreview();
        }

        bool Own(Collider other) => other == CollisionShape || other.attachedRigidbody == body;
        bool Ignore(Collider other, bool includePlayer) => Own(other) || (!includePlayer && other == player) ||
            (other.TryGetComponent<PepperBody>(out var pepper) && pepper.Record != null &&
             pepper.Record.Owner == PepperOwner.Carrier && GetComponent<RawCarrierView>() != null);

        bool Ray(Vector3 origin, Vector3 direction, float distance, out RaycastHit nearest)
        {
            nearest = default;
            float best = float.PositiveInfinity;
            int count = Physics.RaycastNonAlloc(origin, direction, hits, distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            if (count == hits.Length) return false;
            for (int i = 0; i < count; i++)
                if (!Ignore(hits[i].collider, false) && hits[i].distance < best) { nearest = hits[i]; best = nearest.distance; }
            return best < float.PositiveInfinity;
        }

        public bool Clear(CarrierPose pose, bool includePlayer = true)
        {
            if (!pose.IsValid) return false;
            int count = roundShape != null
                ? Physics.OverlapSphereNonAlloc(pose.Position + pose.Rotation * Center, roundShape.radius - .005f,
                    overlaps, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
                : Physics.OverlapBoxNonAlloc(pose.Position + pose.Rotation * Center,
                    Half - Vector3.one * .005f, overlaps, pose.Rotation, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            if (count == overlaps.Length) return false;
            for (int i = 0; i < count; i++)
            {
                var other = overlaps[i];
                if (Ignore(other, includePlayer)) continue;
                if (additionalShapes.Length == 0) return false;
                // A ball inside an open basin is not an obstruction to that basin's recorded pose.
                foreach (var collider in shapes)
                    if (Physics.ComputePenetration(collider, pose.Position, pose.Rotation, other,
                        other.transform.position, other.transform.rotation, out _, out float depth) && depth > .006f) return false;
            }
            return true;
        }

        public bool Supported(CarrierPose pose)
        {
            if (roundShape == null && Vector3.Dot(pose.Rotation * Vector3.up, Vector3.up) < .98f) return false;
            // Center plus inset corners reject overhang, narrow rails and uneven/moving supports.
            for (int i = roundShape != null ? 4 : 0; i < 5; i++)
            {
                Vector3 foot = i == 4 ? Vector3.zero : new Vector3((i % 2 == 0 ? -1 : 1) * Half.x * .85f,
                    0, (i / 2 == 0 ? -1 : 1) * Half.z * .85f);
                var point = roundShape != null ? pose.Position + pose.Rotation * Center - Vector3.up * roundShape.radius :
                    pose.Position + pose.Rotation * (foot + Vector3.up * Bottom);
                if (!Ray(point + Vector3.up * .045f, Vector3.down, .10f, out var support) || support.normal.y < .96f) return false;
                var supportBody = support.rigidbody;
                if (supportBody != null && !supportBody.isKinematic &&
                    (supportBody.linearVelocity.sqrMagnitude > .01f || supportBody.angularVelocity.sqrMagnitude > .01f)) return false;
            }
            return true;
        }

        public void QueryPlacement(Camera view, float yaw)
        {
            PlacementValid = false;
            PlacementReason = "Aim at ground or a worktop within reach";
            HidePreview();
            if (!Ray(view.transform.position, view.transform.forward, reach, out var hit)) return;
            var pose = new CarrierPose(hit.point + Vector3.up * (.012f - Bottom), Quaternion.Euler(0, yaw, 0));
            Placement = pose;
            if (hit.normal.y < .96f) PlacementReason = "Surface too steep";
            else if (!Clear(pose)) PlacementReason = "Needs more clearance";
            else if (!Supported(pose)) PlacementReason = "Support too narrow, uneven or moving";
            else if (!ApproachClear(view.transform.position, pose)) PlacementReason = "Approach blocked - move around the obstruction";
            else { PlacementValid = true; PlacementReason = "Place here"; }
        }

        public void HidePreview() { if (preview != null) preview.enabled = false; }

        public bool TryNearby(Vector3 origin, float yaw, out CarrierPose pose)
        {
            pose = default;
            for (int ring = 0; ring < 3; ring++)
            for (int i = 0; i < 8; i++)
            {
                float angle = yaw + (i % 2 == 0 ? 1 : -1) * (55 + i / 2 * 35);
                var direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                var start = origin + direction * (1.05f + ring * .5f);
                if (!Ray(start, Vector3.down, reach, out var hit) || hit.normal.y < .96f) continue;
                var candidate = new CarrierPose(hit.point + Vector3.up * (.012f - Bottom), Quaternion.Euler(0, yaw, 0));
                if (Vector3.Distance(origin, candidate.Position) > reach || !Clear(candidate) || !Supported(candidate) || !ApproachClear(origin, candidate)) continue;
                pose = candidate;
                return true;
            }
            return false;
        }

        public bool TryRecovery(CarrierPose safe, out CarrierPose pose)
        {
            pose = safe;
            if (InBoundsAt(pose) && Clear(pose) && Supported(pose)) return true;
            pose = Fallback;
            for (int i = 0; i < 121 && (!InBoundsAt(pose) || !Clear(pose) || !Supported(pose)); i++)
                pose = new CarrierPose(Fallback.Position + new Vector3((i % 11 - 5) * 1.15f, 0, (i / 11 - 5) * 1.15f), Quaternion.identity);
            return InBoundsAt(pose) && Clear(pose) && Supported(pose);
        }

        static bool InBoundsAt(CarrierPose pose) => pose.IsValid && Mathf.Abs(pose.Position.x) < 8.65f &&
            Mathf.Abs(pose.Position.z) < 8.65f && pose.Position.y > -2 && pose.Position.y < 6;

        public void Dock(CarrierPose pose)
        {
            SetPose(pose, true);
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            body.isKinematic = true;
            foreach (var collider in shapes) collider.enabled = false; // The fixed receiving fixture supplies the dock's collision and target.
        }

        bool ApproachClear(Vector3 origin, CarrierPose pose)
        {
            var delta = pose.Position + pose.Rotation * Center - origin;
            int count = roundShape != null
                ? Physics.SphereCastNonAlloc(origin, roundShape.radius - .005f, delta.normalized, hits,
                    delta.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
                : Physics.BoxCastNonAlloc(origin, Half - Vector3.one * .005f, delta.normalized, hits,
                    pose.Rotation, delta.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            if (count == hits.Length) return false;
            for (int i = 0; i < count; i++)
                if (!Ignore(hits[i].collider, false) && hits[i].distance < delta.magnitude - .02f) return false;
            return true;
        }

        public void Hold(CarrierPose desired, Vector3 origin)
        {
            if (!held)
            {
                if (!body.isKinematic) body.linearVelocity = body.angularVelocity = Vector3.zero;
                body.interpolation = RigidbodyInterpolation.None;
                body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                body.isKinematic = true;
                // An enabled trigger supports penetration queries without pushing or intercepting targeting.
                foreach (var collider in shapes) { collider.isTrigger = true; collider.enabled = true; }
                held = true;
                ClearHeldMotion();
            }
            // Sweep from the torso; ignore only this object and its holder, never scenery.
            Vector3 center = desired.Position + desired.Rotation * Center;
            Vector3 delta = center - origin;
            float distance = delta.magnitude;
            int count = roundShape != null
                ? Physics.SphereCastNonAlloc(origin, roundShape.radius, delta.normalized, hits, distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
                : Physics.BoxCastNonAlloc(origin, Half, delta.normalized, hits, desired.Rotation,
                    distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            float allowed = count == hits.Length ? 0 : distance;
            for (int i = 0; i < count; i++)
                if (!Ignore(hits[i].collider, false)) allowed = Mathf.Min(allowed, Mathf.Max(0, hits[i].distance - .02f));
            desired.Position = origin + delta.normalized * allowed - desired.Rotation * Center;
            // The torso can itself begin close to a wall. Resolve the crate, never the character.
            for (int iteration = 0; iteration < 4; iteration++)
            {
                count = Physics.OverlapBoxNonAlloc(desired.Position + desired.Rotation * Center, Half,
                    overlaps, desired.Rotation, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
                bool moved = false;
                for (int i = 0; i < count; i++)
                {
                    var other = overlaps[i];
                    if (Ignore(other, false)) continue;
                    foreach (var collider in shapes)
                        if (Physics.ComputePenetration(collider, desired.Position, desired.Rotation, other,
                            other.transform.position, other.transform.rotation, out var direction, out float depth))
                        { desired.Position += direction * (depth + .012f); moved = true; }
                }
                if (!moved) break;
            }
            var heldCenter = desired.Position + desired.Rotation * Center;
            if (motionFrame != Time.frameCount)
            {
                releaseVelocity = motionFrame >= 0 && Time.deltaTime > 0
                    ? Vector3.ClampMagnitude((heldCenter - motionPosition) / Time.deltaTime, 4.5f) : Vector3.zero;
                motionPosition = heldCenter;
                motionFrame = Time.frameCount;
            }
            transform.SetPositionAndRotation(desired.Position, desired.Rotation);
            body.position = desired.Position; body.rotation = desired.Rotation;
        }

        public void SetPose(CarrierPose pose, bool settle)
        {
            held = false;
            foreach (var collider in shapes) { collider.enabled = true; collider.isTrigger = false; }
            body.isKinematic = false;
            body.interpolation = RigidbodyInterpolation.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            body.position = pose.Position; body.rotation = pose.Rotation;
            transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            body.linearVelocity = body.angularVelocity = Vector3.zero;
            // A tucked release may overlap the holder. Re-enable contact once separated.
            ignoringPlayer = OverlapsPlayer();
            if (player != null) foreach (var collider in shapes) Physics.IgnoreCollision(collider, player, ignoringPlayer);
            if (settle) body.Sleep(); else body.WakeUp();
            HidePreview();
        }

        public void ClearHeldMotion() { motionFrame = -1; releaseVelocity = Vector3.zero; }

        // Player releases always enter simulation. Sleep is reserved for initialization/recovery.
        public void ReleaseFromHand(CarrierPose pose, bool careful)
        {
            var velocity = careful ? Vector3.zero : releaseVelocity;
            SetPose(pose, false);
            body.linearVelocity = velocity;
            ClearHeldMotion();
        }

        bool OverlapsPlayer()
        {
            if (player == null) return false;
            foreach (var collider in shapes)
                if (Physics.ComputePenetration(collider, collider.transform.position, collider.transform.rotation,
                    player, player.transform.position, player.transform.rotation, out _, out _)) return true;
            return false;
        }

        public void Toss(Vector3 velocity)
        {
            velocity += releaseVelocity;
            SetPose(Pose, false);
            body.linearVelocity = Vector3.ClampMagnitude(velocity, 7);
            body.angularVelocity = transform.right * 2;
            ClearHeldMotion();
        }

        void FixedUpdate()
        {
            if (held || !ignoringPlayer) return;
            if (OverlapsPlayer()) return;
            foreach (var collider in shapes) Physics.IgnoreCollision(collider, player, false);
            ignoringPlayer = false;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (held || Time.timeScale == 0 || collision.relativeVelocity.sqrMagnitude < .4f || Time.time - lastContact < .3f) return;
            lastContact = Time.time;
            ContactCues++;
            if (contactAudio != null && contactClip != null) contactAudio.PlayOneShot(contactClip, Mathf.Min(.3f, collision.relativeVelocity.magnitude * .06f));
        }
    }
}
