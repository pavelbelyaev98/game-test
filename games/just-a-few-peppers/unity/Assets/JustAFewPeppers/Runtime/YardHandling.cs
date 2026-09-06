using UnityEngine;
using UnityEngine.UI;

namespace JustAFewPeppers
{
    public sealed class YardHandling : MonoBehaviour
    {
        public PileRegion[] regions;
        public RawCarrierView crate;
        public ScoopPresentation presentation;
        public Text statusText;
        [Min(1)] public int crateCapacity = 12;
        [Min(1)] public int unitsPerScoop = 1;
        [Min(.1f)] public float scoopInterval = .5f;
        public HarvestState State { get; private set; }
        YardSession session;
        bool inputArmed;
        float cooldown;
        GatherStatus? lastDenial;

        public void Initialize(YardSession owner)
        {
            session = owner;
            var ids = new string[regions.Length];
            var quantities = new int[regions.Length];
            for (int i = 0; i < regions.Length; i++) { ids[i] = regions[i].regionId; quantities[i] = regions[i].initialUnits; }
            State = new HarvestState(ids, quantities, crateCapacity, crate.restingPoints.Length);
            Render();
        }

        public void Step(float dt)
        {
            cooldown = Mathf.Max(0, cooldown - dt);
            // The crate follows the movement owner exactly. It has no Rigidbody or second simulated pose.
            crate.Render(State);
            presentation.Tick(dt);
            var input = session.Input;
            if (!inputArmed)
            {
                if (!input.Scoop.IsPressed() && !input.Interact.IsPressed()) inputArmed = true;
                ShowStatus();
                return;
            }
            if (input.Interact.WasPressedThisFrame())
            {
                if (State.IsHeld)
                {
                    int point = crate.FindParkingPoint(session.player);
                    if (State.Park(point)) { Interrupt(); presentation.HandleCrate(); Render(); session.hud.Notice("Crate parked - contents kept"); }
                    else session.hud.Notice("Stand beside a clear crate mat to park");
                }
                else if (session.targeting.Current == crate.target && State.PickUp())
                {
                    Interrupt(); presentation.HandleCrate(); Render(); session.hud.Notice("Crate ready - scoop the mound");
                }
            }
            if (!inputArmed) { ShowStatus(); return; }

            var region = session.targeting.CurrentRegion;
            string id = region != null ? region.regionId : null;
            var status = State.CanGather(id);
            // Release/repress alone does not replay denial; only a meaningful valid state resets it.
            if (status == GatherStatus.Ready) lastDenial = null;
            if (input.Scoop.IsPressed())
            {
                if (status != GatherStatus.Ready) Deny(status);
                else if (cooldown <= 0)
                {
                    int accepted = State.Gather(id, unitsPerScoop);
                    if (accepted > 0)
                    {
                        cooldown = scoopInterval; // No catch-up burst or extra scoop from rapidly clicking.
                        Render();
                        presentation.Scoop(session.targeting.HitPoint, crate.contentDestination);
                        if (State.RawUnits == State.Capacity) Deny(GatherStatus.Full);
                        else if (State.UnitsIn(id) == 0) session.hud.Notice("Ground cleared here - sweep to another clump");
                    }
                }
            }
            ShowStatus();
        }

        void Deny(GatherStatus status)
        {
            if (lastDenial == status) return;
            lastDenial = status;
            presentation.Feedback();
        }

        public void Interrupt()
        {
            inputArmed = false;
            presentation.Interrupt();
        }

        public void Recover()
        {
            Interrupt();
            State.RecoverCarrier();
            Render();
        }

        public void ResetPrototype()
        {
            Interrupt();
            State.ResetPrototype();
            cooldown = 0;
            lastDenial = null;
            Render();
        }

        public void Render()
        {
            foreach (var region in regions) region.Render(State.UnitsIn(region.regionId));
            crate.Render(State);
            ShowStatus();
        }

        void ShowStatus()
        {
            statusText.text = "Crate " + State.RawUnits + " / " + State.Capacity + (State.IsHeld ? "  |  Carrying" : "  |  Parked") +
                "    Mound " + State.Remaining + "    Hold left mouse to scoop";
            if (session.IsPaused) return;
            if (State.IsHeld && State.RawUnits == State.Capacity)
                session.hud.targetText.text = "Crate full - 12 / 12\nE beside a mat to park  |  F8 restarts the scoop test";
            else if (State.IsHeld)
            {
                var status = State.CanGather(session.targeting.CurrentRegion != null ? session.targeting.CurrentRegion.regionId : null);
                session.hud.targetText.text = status == GatherStatus.Ready ? "Collect peppers\nHold left mouse and sweep across the mound" :
                    status == GatherStatus.Empty ? "Ground cleared here\nAim at another clump" : "Aim at a reachable pepper clump\nE beside a mat to park the crate";
            }
            else if (session.targeting.Current == crate.target) session.hud.targetText.text = "Crate  " + State.RawUnits + " / 12\nE  Pick up";
            else if (session.targeting.CurrentRegion != null) session.hud.targetText.text = "Pick up the crate first\nE while looking at the crate  |  R recovers it to the gate mat";
        }
    }
}
