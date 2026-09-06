using System;
using System.Collections.Generic;
using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class LoosePropHandling : MonoBehaviour
    {
        public LooseProp[] props;
        public Transform carryAnchor;
        public LooseProp Held { get; private set; }
        YardSession session;
        YardHandling handling;

        public void Initialize(YardSession owner)
        {
            session = owner;
            handling = owner.handling;
            var ids = new HashSet<string>();
            foreach (var prop in props)
            {
                if (!ids.Add(prop.propId)) throw new InvalidOperationException("Duplicate loose prop ID: " + prop.propId);
                prop.Initialize(owner.player);
            }
        }

        public void Tick()
        {
            foreach (var prop in props) prop.Observe();
            if (Held == null) return;
            var pose = new CarrierPose(carryAnchor.position, carryAnchor.rotation * Quaternion.Euler(0, Held.RotationOffset, 0));
            Held.portable.Hold(pose, session.player.transform.position + Vector3.up * .85f);
            Held.State.Record(Held.portable.Pose, false);
        }

        public void QueryPlacement(float dt)
        {
            if (Held == null) return;
            Held.RotationOffset = Mathf.Repeat(Held.RotationOffset + session.Input.Rotate.ReadValue<float>() * 90 * dt, 360);
            Held.portable.QueryPlacement(session.player.view, session.player.transform.eulerAngles.y + Held.RotationOffset);
        }

        public bool Release(bool careful, float throwCharge = -1)
        {
            if (Held == null) return false;
            var body = Held.portable;
            var pose = careful ? body.Placement : body.Pose;
            if (careful ? !body.PlacementValid : !body.Clear(pose, false))
            {
                session.hud.Notice(careful ? body.PlacementReason : "Move the object clear of the obstruction");
                return false;
            }
            Held.State.Release(pose, careful);
            if (throwCharge >= 0) body.Toss(session.player.view.transform.forward * Mathf.Lerp(1.5f, Held.tossSpeed, throwCharge) + Vector3.up * .6f);
            else body.ReleaseFromHand(pose, careful);
            Held = null;
            handling.Interrupt();
            return true;
        }

        public bool TryGrab()
        {
            if (handling.HasHeldObject) return false;
            foreach (var prop in props)
            {
                if (session.targeting.Current != prop.target) continue;
                prop.State.PickUp();
                Held = prop;
                handling.Interrupt();
                Tick();
                return true;
            }
            return false;
        }

        public bool ShowStatus()
        {
            if (Held != null)
            {
                session.hud.targetText.text = handling.ThrowCharging ? "Release left mouse to throw" : "";
                return true;
            }
            if (handling.HasHeldObject) return false;
            foreach (var prop in props)
                if (session.targeting.Current == prop.target)
                {
                    session.hud.targetText.text = prop.target.displayName + "\nRight click  Grab";
                    return true;
                }
            return false;
        }

        public void Recover()
        {
            foreach (var prop in props) prop.Recover();
            if (Held != null && !Held.State.IsHeld) Held = null;
        }
    }
}
