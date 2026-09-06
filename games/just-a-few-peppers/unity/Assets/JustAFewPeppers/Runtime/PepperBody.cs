using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class PepperBody : MonoBehaviour
    {
        public Rigidbody body;
        public CapsuleCollider shape;
        public Renderer skin;
        public PepperRecord Record { get; private set; }
        public int Contacts { get; private set; }
        public float GatherUntil { get; set; }
        public Vector3 GatherFrom { get; set; }
        public PepperOwner ShownOwner { get; set; }
        PepperBatch batch;
        MaterialPropertyBlock tint;

        public void Bind(PepperBatch owner, PepperRecord record)
        {
            batch = owner; Record = record; ShownOwner = record.Owner;
            tint = new MaterialPropertyBlock();
            Pose(record.Pose, false);
            Physics.IgnoreCollision(shape, owner.Session.player.body);
        }

        public void Highlight(bool value)
        {
            tint.SetColor("_Color", value ? new Color(1, .83f, .24f) : new Color(.82f, .13f, .035f));
            skin.SetPropertyBlock(tint);
        }

        public void Pose(CarrierPose pose, bool kinematic)
        {
            body.isKinematic = kinematic;
            body.collisionDetectionMode = kinematic ? CollisionDetectionMode.ContinuousSpeculative : CollisionDetectionMode.ContinuousDynamic;
            shape.isTrigger = kinematic;
            transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            body.position = pose.Position; body.rotation = pose.Rotation;
            if (!kinematic) { body.linearVelocity = body.angularVelocity = Vector3.zero; body.WakeUp(); }
        }

        void OnCollisionEnter(Collision collision)
        {
            Contacts++;
            if (Record == null) return;
            if (Record.Owner == PepperOwner.Transit && collision.collider.transform.IsChildOf(batch.Session.handling.station.intake))
                batch.ContactIntake(this);
            else if (collision.collider.attachedRigidbody == batch.Session.handling.crate.portable.body)
                batch.ContactCarrier(this);
        }
    }
}
