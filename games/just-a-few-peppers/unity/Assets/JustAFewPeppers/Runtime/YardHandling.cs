using UnityEngine;
using UnityEngine.UI;

namespace JustAFewPeppers
{
    public sealed class YardHandling : MonoBehaviour
    {
        public PileRegion[] regions;
        public RawCarrierView crate;
        public ScoopPresentation presentation;
        public StationView station;
        public TipPresentation tipping;
        public Text statusText;
        [Min(1)] public int crateCapacity = 12;
        [Min(1)] public int unitsPerScoop = 1;
        [Min(.1f)] public float scoopInterval = .5f;
        public HarvestState State { get; private set; }
        YardSession session;
        bool inputArmed;
        float cooldown;
        GatherStatus? lastDenial;
        TipStatus? lastTipDenial;

        public void Initialize(YardSession owner)
        {
            session = owner;
            var ids = new string[regions.Length];
            var quantities = new int[regions.Length];
            for (int i = 0; i < regions.Length; i++) { ids[i] = regions[i].regionId; quantities[i] = regions[i].initialUnits; }
            crate.Initialize(owner.player);
            State = new HarvestState(ids, quantities, crateCapacity, crate.portable.Fallback,
                station.inputCapacity, station.outputCapacity, station.batchDuration);
            Render();
        }

        public void Step(float dt)
        {
            if (State.AdvanceProcessing(dt) > 0) station.Completed();
            station.Render(State);
            if (State.CanTip() == TipStatus.Ready) lastTipDenial = null;
            cooldown = Mathf.Max(0, cooldown - dt);
            crate.ObserveReleased(State);
            crate.Render(State);
            presentation.Tick(dt);
            if (tipping.IsPlaying)
            {
                if (session.targeting.Current != station.intakeTarget) tipping.Interrupt();
                else tipping.Tick(dt, State);
                crate.Render(State);
                ShowStatus();
                return;
            }
            var input = session.Input;
            if (!inputArmed)
            {
                if (!input.Scoop.IsPressed() && !input.Interact.IsPressed() && !input.Drop.IsPressed() && !input.Rotate.IsPressed()) inputArmed = true;
                ShowStatus();
                return;
            }
            if (State.IsHeld)
            {
                crate.Rotate(input.Rotate.ReadValue<float>() * 90 * dt);
                crate.portable.QueryPlacement(session.player.view, session.player.transform.eulerAngles.y + crate.RotationOffset);
            }
            else crate.portable.HidePreview();
            // Deliberate release wins over an E transfer on the same frame.
            if (State.IsHeld && input.Drop.WasPressedThisFrame())
            {
                if (crate.Release(State, false)) { Interrupt(); Render(); session.hud.Notice("Crate dropped - contents kept"); }
                else session.hud.Notice("Move the held crate clear of the obstruction to drop");
            }
            else if (input.Interact.WasPressedThisFrame())
            {
                if (session.targeting.Current == station.intakeTarget)
                {
                    var tipStatus = State.CanTip();
                    int accepted = State.Tip();
                    if (accepted > 0)
                    {
                        Interrupt();
                        tipping.Begin(accepted);
                        Render();
                        session.hud.Notice("Tipped " + accepted + " peppers" + (State.RawUnits > 0 ? " - " + State.RawUnits + " kept in crate" : " - crate empty"));
                    }
                    else if (lastTipDenial != tipStatus)
                    {
                        lastTipDenial = tipStatus;
                        presentation.Feedback();
                    }
                }
                else if (State.IsHeld)
                {
                    if (crate.Release(State, true)) { Interrupt(); presentation.HandleCrate(); Render(); session.hud.Notice("Crate placed - contents kept"); }
                    else session.hud.Notice(crate.portable.PlacementReason);
                }
                else if (session.targeting.Current == crate.target && State.PickUp())
                {
                    Interrupt(); presentation.HandleCrate(); Render(); session.hud.Notice("Crate ready - hold left mouse at the pepper pile");
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
            tipping.Interrupt();
            station.Interrupt();
            crate.portable.HidePreview();
            if (State != null) crate.Render(State);
        }

        public void Recover()
        {
            Interrupt();
            crate.Recover(State);
            Render();
        }

        public void ResetPrototype()
        {
            Interrupt();
            State.ResetPrototype();
            crate.ResetPose(State);
            cooldown = 0;
            lastDenial = null;
            lastTipDenial = null;
            Render();
        }

        public void Render()
        {
            foreach (var region in regions) region.Render(State.UnitsIn(region.regionId));
            crate.Render(State);
            station.Render(State);
            ShowStatus();
        }

        void ShowStatus()
        {
            statusText.text = "Crate " + State.RawUnits + " / " + State.Capacity + (State.IsHeld ? "  |  Carrying" : "  |  Released") +
                "    Peppers left " + State.Remaining + "    Hold left mouse to scoop";
            if (session.IsPaused) return;
            if (session.targeting.Current == station.intakeTarget)
            {
                var tipStatus = State.CanTip();
                session.hud.targetText.text = tipping.IsPlaying ? "Tipping load\n" + State.RawUnits + " left in crate" :
                    tipStatus == TipStatus.Ready ? "E  Tip load\nIntake accepts " + Mathf.Min(State.RawUnits, State.InputCapacity - State.QueuedUnits) + " peppers" :
                    tipStatus == TipStatus.InputFull ? "Input full - load kept\nOutput collection comes next; F8 restarts the test" :
                    tipStatus == TipStatus.Empty ? "Crate empty - gather another load\n" + station.Stage(State) :
                    "Automatic processor\nBring the crate to tip a load  |  " + station.Stage(State);
            }
            else if (State.IsHeld && session.targeting.CurrentRegion == null)
                session.hud.targetText.text = "";
            else if (State.IsHeld && State.RawUnits == State.Capacity)
                session.hud.targetText.text = "Crate full - " + State.RawUnits + " / " + State.Capacity + "\nBring it to the broad intake and press E to tip";
            else if (State.IsHeld)
            {
                var status = State.CanGather(session.targeting.CurrentRegion != null ? session.targeting.CurrentRegion.regionId : null);
                session.hud.targetText.text = status == GatherStatus.Ready ? "Collect peppers\nHold left mouse and sweep across the pepper pile" :
                    status == GatherStatus.Empty ? "Ground cleared here\nAim at another clump" : "Aim at a reachable pepper clump\nE  Place    Z / X  Rotate    G  Drop";
            }
            else if (session.targeting.Current == crate.target) session.hud.targetText.text = "Crate  " + State.RawUnits + " / 12\nE  Pick up";
            else if (session.targeting.CurrentRegion != null) session.hud.targetText.text = "Pick up the crate first\nE while looking at the crate  |  R recovers a lost crate";
        }
    }
}
