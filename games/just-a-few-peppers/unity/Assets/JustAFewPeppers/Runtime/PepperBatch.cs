using System;
using System.Collections.Generic;
using UnityEngine;

namespace JustAFewPeppers
{
    public enum PepperSimulation { PhysicalBatch, GroupedRest }
    [Serializable] public struct PepperSeed { public string source; public Vector3 position; public float yaw; }

    // Input/representation bridge. HarvestState alone owns units and transitions.
    public sealed class PepperBatch : MonoBehaviour
    {
        public PepperBody pepperPrefab;
        public PepperSeed[] seeds;
        public Transform handAnchor;
        public GameObject[] groupedViews;
        public PepperSimulation simulation = PepperSimulation.PhysicalBatch;
        public int nearbyLimit = 36;
        public YardSession Session { get; private set; }
        public PepperBody[] Bodies { get; private set; }
        public PepperBody Held { get; private set; }
        public PepperBody Target { get; private set; }
        public IReadOnlyList<string> Preview => preview;
        public bool Pouring => State.PourOpen;
        public int IntakeContacts { get; private set; }
        public int CarrierContacts { get; private set; }
        HarvestState State => Session.handling.State;
        readonly List<string> preview = new List<string>(3);
        readonly List<string> nextPreview = new List<string>(3);
        readonly List<PepperBody> candidates = new List<PepperBody>(128);
        readonly RaycastHit[] hits = new RaycastHit[128];
        float previewSince, nextGather, pourEnd, throwTime;
        bool bulkHeld, throwing;
        bool crateWasHeld;
        GatherStatus? lastDenial;
        Comparison<PepperBody> byDistance, byTarget;
        Vector3 lastHand, handVelocity;
        Renderer destinationSkin;
        MaterialPropertyBlock destinationTint;
        Color destinationColor;

        public void Initialize(YardSession owner)
        {
            Session = owner;
            byDistance = (a, b) => Distance(a).CompareTo(Distance(b));
            byTarget = (a, b) => (a.transform.position - Target.transform.position).sqrMagnitude.CompareTo((b.transform.position - Target.transform.position).sqrMagnitude);
            var sources = new string[seeds.Length]; var poses = new CarrierPose[seeds.Length];
            for (int i = 0; i < seeds.Length; i++) { sources[i] = seeds[i].source; poses[i] = new CarrierPose(seeds[i].position, Quaternion.Euler(0, seeds[i].yaw, 0)); }
            State.RegisterPeppers(sources, poses);
            Bodies = new PepperBody[seeds.Length];
            for (int i = 0; i < Bodies.Length; i++)
            {
                Bodies[i] = Instantiate(pepperPrefab, transform);
                Bodies[i].name = State.Peppers[i].Id;
                Bodies[i].Bind(this, State.Peppers[i]);
            }
            destinationSkin = Session.handling.crate.target.marker;
            destinationTint = new MaterialPropertyBlock();
            if (destinationSkin != null) destinationColor = destinationSkin.sharedMaterial.color;
            foreach (var region in Session.handling.regions)
            {
                foreach (var c in region.GetComponentsInChildren<Collider>(true)) c.enabled = false;
                foreach (var r in region.GetComponentsInChildren<Renderer>(true)) r.enabled = false;
            }
            Render();
        }

        public void Tick(float dt)
        {
            if (Pouring && Time.time >= pourEnd) Cancel();
            foreach (var p in Bodies)
            {
                if (!p.gameObject.activeSelf || p.Record.Owner == PepperOwner.Processed) continue;
                if (p == Held) continue;
                if (p.Record.Owner == PepperOwner.Carrier && (State.IsHeld || Time.time < p.GatherUntil)) continue;
                var position = p.transform.position;
                if (position.y < -2 || Mathf.Abs(position.x) > 8.6f || Mathf.Abs(position.z) > 8.6f)
                { if (p.Record.Owner == PepperOwner.Carrier) State.SpillPepper(p.Record.Id); State.RecoverPepper(p.Record.Id); p.Pose(p.Record.Pose, false); continue; }
                if (!p.body.isKinematic)
                {
                    State.ObservePepper(p.Record.Id, new CarrierPose(position, p.transform.rotation), p.body.IsSleeping());
                    if (p.Record.Owner == PepperOwner.Carrier)
                    {
                        var local = Session.handling.crate.transform.InverseTransformPoint(position);
                        if (Mathf.Abs(local.x) > .65f || Mathf.Abs(local.z) > .53f || local.y < -.12f)
                            State.SpillPepper(p.Record.Id);
                    }
                }
            }
            Render();
            Select();
        }

        public void Render()
        {
            if (Bodies == null) return;
            candidates.Clear();
            if (simulation == PepperSimulation.GroupedRest)
            {
                foreach (var p in Bodies) if (p.Record.Owner == PepperOwner.Source) candidates.Add(p);
                candidates.Sort(byDistance);
            }
            for (int i = 0; i < groupedViews.Length; i++) groupedViews[i].SetActive(false);
            int slot = 0;
            foreach (var p in Bodies)
            {
                var owner = p.Record.Owner;
                bool grouped = simulation == PepperSimulation.GroupedRest && owner == PepperOwner.Source &&
                    (candidates.IndexOf(p) >= nearbyLimit || Distance(p) > 25);
                if (grouped)
                {
                    for (int i = 0; i < Session.handling.regions.Length; i++)
                        if (Session.handling.regions[i].regionId == p.Record.Source) groupedViews[i].SetActive(true);
                }
                bool visible = owner != PepperOwner.Processed && !grouped;
                if (p.gameObject.activeSelf != visible)
                {
                    p.gameObject.SetActive(visible);
                    if (visible) p.Pose(p.Record.Pose, false);
                }
                if (!visible) { p.ShownOwner = owner; continue; }
                if (owner == PepperOwner.Carrier)
                {
                    var local = new Vector3((slot % 3 - 1) * .23f, .18f + slot / 6 * .14f, (slot / 3 % 2 == 0 ? -.16f : .16f));
                    slot++;
                    if (crateWasHeld && !State.IsHeld)
                    {
                        p.Pose(new CarrierPose(Session.handling.crate.transform.TransformPoint(local), Session.handling.crate.transform.rotation), false);
                        p.body.linearVelocity = Session.handling.crate.portable.body.linearVelocity;
                    }
                    if (State.IsHeld || Time.time < p.GatherUntil)
                    {
                        Vector3 destination = Session.handling.crate.transform.TransformPoint(local);
                        float t = Mathf.Clamp01(1 - (p.GatherUntil - Time.time) / .3f);
                        var position = Time.time < p.GatherUntil ? Vector3.Lerp(p.GatherFrom, destination, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * .3f : destination;
                        p.Pose(new CarrierPose(position, Session.handling.crate.transform.rotation), true);
                        // Held cargo remains targetable when the crate is subsequently set down.
                    }
                    else if (p.body.isKinematic)
                        p.Pose(new CarrierPose(p.transform.position, p.transform.rotation), false);
                }
                else if (owner == PepperOwner.Held)
                {
                    Vector3 position = handAnchor.position;
                    var origin = Session.player.view.transform.position;
                    if (Physics.SphereCast(origin, .13f, (position - origin).normalized, out var hit, Vector3.Distance(origin, position), Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                        position = hit.point - (position - origin).normalized * .16f;
                    handVelocity = Time.deltaTime > 0 ? Vector3.ClampMagnitude((position - lastHand) / Time.deltaTime, 3) : Vector3.zero;
                    lastHand = position;
                    p.Pose(new CarrierPose(position, handAnchor.rotation), true);
                }
                p.ShownOwner = owner;
            }
            crateWasHeld = State.IsHeld;
        }

        float Distance(PepperBody p) => (p.Record.Pose.Position - Session.player.transform.position).sqrMagnitude;

        bool Visible(PepperBody p)
        {
            if (!p.gameObject.activeSelf || p == Held) return false;
            var delta = p.transform.position - Session.player.view.transform.position;
            if (delta.magnitude > 3) return false;
            int count = Physics.RaycastNonAlloc(Session.player.view.transform.position, delta.normalized, hits, delta.magnitude + .12f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            float nearest = float.PositiveInfinity; Collider first = null;
            for (int i = 0; i < count; i++) if (hits[i].distance < nearest && hits[i].collider != Session.player.body)
            { nearest = hits[i].distance; first = hits[i].collider; }
            return first == p.shape;
        }

        void Select()
        {
            Target = null;
            var ray = new Ray(Session.player.view.transform.position, Session.player.view.transform.forward);
            if (Physics.Raycast(ray, out var hit, 3, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                Target = hit.collider.GetComponent<PepperBody>();
            // Small forgiveness still respects the first solid obstruction.
            if (Target == null && Physics.SphereCast(ray, .045f, out hit, 3, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                Target = hit.collider.GetComponent<PepperBody>();
            nextPreview.Clear();
            if (State.IsHeld && Target != null && (Target.Record.Owner == PepperOwner.Source || Target.Record.Owner == PepperOwner.Loose))
            {
                candidates.Clear();
                foreach (var p in Bodies)
                    if ((p.Record.Owner == PepperOwner.Source || p.Record.Owner == PepperOwner.Loose) &&
                        (p.transform.position - Target.transform.position).sqrMagnitude < .65f * .65f && Visible(p)) candidates.Add(p);
                candidates.Sort(byTarget);
                for (int i = 0; i < Mathf.Min(3, Mathf.Min(candidates.Count, State.Capacity - State.RawUnits)); i++) nextPreview.Add(candidates[i].Record.Id);
            }
            bool same = preview.Count == nextPreview.Count;
            for (int i = 0; same && i < preview.Count; i++) same = preview[i] == nextPreview[i];
            if (!same) { preview.Clear(); preview.AddRange(nextPreview); previewSince = Time.time; }
            foreach (var p in Bodies) p.Highlight(Held == null && (State.IsHeld ? preview.Contains(p.Record.Id) : p == Target));
            if (destinationSkin != null)
            {
                destinationTint.SetColor("_Color", preview.Count > 0 ? new Color(1, .83f, .24f) : destinationColor);
                destinationSkin.SetPropertyBlock(destinationTint);
            }
        }

        public bool HandleInput(float dt)
        {
            var input = Session.Input;
            if (Held != null)
            {
                if (input.Drop.WasPressedThisFrame() || input.Grab.WasPressedThisFrame()) Release(false, false);
                else if (input.Interact.WasPressedThisFrame()) Release(true, false);
                else if (input.Use.WasPressedThisFrame()) { throwing = true; throwTime = Time.time; }
                else if (throwing && input.Use.WasReleasedThisFrame()) Release(false, true);
                return true;
            }
            if (Pouring)
            {
                if (input.Drop.WasPressedThisFrame() || input.Grab.WasPressedThisFrame()) { Cancel(); return false; }
                return true;
            }
            if (!Session.handling.HasHeldObject && Target != null && input.Grab.WasPressedThisFrame())
            {
                if (State.PickPepper(Target.Record.Id))
                {
                    Held = Target; lastHand = handAnchor.position;
                    Session.handling.Interrupt(); Render();
                }
                return true;
            }
            if (!State.IsHeld) return false;
            if (input.Drop.WasPressedThisFrame() || input.Grab.WasPressedThisFrame()) return false;
            if (input.Pour.WasPressedThisFrame() || (input.Interact.WasPressedThisFrame() && Session.targeting.Current == Session.handling.station.intakeTarget))
            { BeginPour(); return true; }
            if (input.Drop.WasPressedThisFrame() || input.Grab.WasPressedThisFrame() || input.Interact.WasPressedThisFrame()) return false;
            if (input.Use.WasPressedThisFrame()) bulkHeld = true;
            if (!input.Use.IsPressed()) bulkHeld = false;
            if (preview.Count > 0) lastDenial = null;
            else if (bulkHeld)
            {
                var denial = State.RawUnits == State.Capacity ? GatherStatus.Full : GatherStatus.InvalidTarget;
                if (lastDenial != denial) { lastDenial = denial; Session.handling.presentation.Feedback(); }
            }
            if (bulkHeld && preview.Count > 0 && Time.time >= previewSince + .12f && Time.time >= nextGather)
            {
                // Revalidate the complete preview without extending its membership.
                nextPreview.Clear();
                foreach (string id in preview)
                {
                    var p = Bodies[int.Parse(id.Substring(7))];
                    if (Visible(p)) { nextPreview.Add(id); p.GatherFrom = p.transform.position; p.GatherUntil = Time.time + .3f; }
                }
                int accepted = State.GatherPeppers(nextPreview);
                if (accepted > 0) { nextGather = Time.time + .5f; Session.handling.presentation.HandleCrate(); }
                Render();
            }
            return false;
        }

        void Release(bool careful, bool toss)
        {
            var p = Held;
            var pose = new CarrierPose(p.transform.position, p.transform.rotation);
            bool container = false;
            if (careful)
            {
                var ray = new Ray(Session.player.view.transform.position, Session.player.view.transform.forward);
                if (!Physics.Raycast(ray, out var hit, 3, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                { Session.hud.Notice("Aim at a reachable surface or inside the crate"); return; }
                container = hit.collider.attachedRigidbody == Session.handling.crate.portable.body;
                if (!container && hit.normal.y < .8f) { Session.hud.Notice("Aim at a supported surface to set down"); return; }
                if (container && State.RawUnits >= State.Capacity) { Session.hud.Notice("Crate full - pepper kept"); return; }
                pose = new CarrierPose(hit.point + Vector3.up * .13f, Quaternion.identity);
                if (container) pose = new CarrierPose(Session.handling.crate.transform.TransformPoint(new Vector3(0, .62f, 0)), Session.handling.crate.transform.rotation);
            }
            State.ReleasePepper(p.Record.Id, pose);
            if (container) State.PutPepperInCarrier(p.Record.Id);
            var velocity = careful ? Vector3.zero : handVelocity;
            if (toss) velocity += Session.player.view.transform.forward * Mathf.Lerp(1.5f, 5, Mathf.Clamp01((Time.time - throwTime) / .8f));
            p.Pose(pose, false); p.body.linearVelocity = Vector3.ClampMagnitude(velocity, 6);
            Held = null; Session.handling.Interrupt();
        }

        void BeginPour()
        {
            bool into = Session.targeting.Current == Session.handling.station.intakeTarget;
            var load = new List<PepperBody>();
            foreach (var p in Bodies) if (p.Record.Owner == PepperOwner.Carrier) load.Add(p);
            if (State.BeginPepperPour(into) == 0) { Session.hud.Notice(State.RawUnits == 0 ? "Crate empty" : "Input full - load kept"); return; }
            bulkHeld = false;
            var crate = Session.handling.crate;
            crate.TipBlend = 1;
            crate.TipPosition = into ? Session.handling.station.intake.position + Vector3.up * .65f - Session.player.view.transform.right * .6f : crate.carryAnchor.position + Vector3.up * .3f;
            crate.Render(State);
            int i = 0;
            foreach (var p in load)
            {
                if (p.Record.Owner == PepperOwner.Carrier) continue;
                var position = into ? Session.handling.station.intake.position + new Vector3((i % 4 - 1.5f) * .22f, .48f + i / 4 * .2f, (i / 4 - 1) * .18f)
                    : crate.transform.position + Session.player.view.transform.forward * .7f + Vector3.up * (.2f + i * .08f);
                p.gameObject.SetActive(true);
                p.Pose(new CarrierPose(position, Quaternion.Euler(0, i * 31, 0)), false);
                p.body.linearVelocity = into ? Vector3.down * .35f : Session.player.view.transform.forward * 1.3f;
                State.ObservePepper(p.Record.Id, new CarrierPose(position, p.transform.rotation), false);
                i++;
            }
            pourEnd = Time.time + 1.15f;
            Session.hud.Notice(into ? "Pouring into intake" : "Pouring onto ground - peppers stay recoverable");
        }

        public void ContactIntake(PepperBody p)
        {
            if (State.AcceptPepperAtIntake(p.Record.Id)) { IntakeContacts++; p.gameObject.SetActive(false); }
        }

        public void ContactCarrier(PepperBody p)
        {
            CarrierContacts++;
            var local = Session.handling.crate.transform.InverseTransformPoint(p.transform.position);
            if (Vector3.Dot(Session.handling.crate.transform.up, Vector3.up) > .9f &&
                Mathf.Abs(local.x) < .41f && Mathf.Abs(local.z) < .3f && local.y > .07f && local.y < .6f)
                State.PutPepperInCarrier(p.Record.Id);
        }

        public void Cancel()
        {
            bulkHeld = throwing = false; handVelocity = Vector3.zero; lastHand = handAnchor.position;
            if (Pouring) State.EndPepperPour();
            Session.handling.crate.TipBlend = 0;
            preview.Clear();
            if (Bodies != null) foreach (var p in Bodies) p.Highlight(false);
        }

        public void Recover()
        {
            Cancel(); Held = null;
            foreach (var p in Bodies)
            {
                if (p.Record.Owner == PepperOwner.Carrier) { p.GatherUntil = Time.time + .3f; p.GatherFrom = Session.handling.crate.contentDestination.position; continue; }
                State.RecoverPepper(p.Record.Id);
                if (p.Record.Owner != PepperOwner.Processed) p.Pose(p.Record.Pose, false);
            }
            Render();
        }

        public void ResetViews()
        {
            Cancel(); Held = null; nextGather = 0;
            foreach (var p in Bodies) { p.GatherUntil = 0; p.Pose(p.Record.Pose, false); }
            Render();
        }

        public bool ShowStatus()
        {
            string text;
            if (Pouring) text = "Pouring";
            else if (Held != null) text = throwing ? "Release left mouse to throw" : "Pepper in hand\nE  Set down / put in crate";
            else if (State.IsHeld && Target != null) text = State.RawUnits == State.Capacity ? "Crate full - load kept" :
                "Hold left mouse  Gather " + preview.Count + " highlighted peppers into crate\nCrate " + State.RawUnits + " / " + State.Capacity;
            else if (!Session.handling.HasHeldObject && Target != null) text = "One pepper\nRight click  Grab this pepper";
            else if (State.IsHeld && Session.targeting.Current != Session.handling.station.intakeTarget) text = "";
            else return false;
            Session.hud.targetText.text = text; return true;
        }
    }
}
