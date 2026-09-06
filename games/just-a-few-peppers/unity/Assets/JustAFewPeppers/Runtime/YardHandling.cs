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
        public FinishedFoodHandling finished;
        public LoosePropHandling looseProps;
        public PepperBatch peppers;
        public MachineOperation machine;
        public Text statusText;
        [Min(1)] public int crateCapacity = 12;
        [Min(1)] public int unitsPerScoop = 1;
        [Min(.1f)] public float scoopInterval = .5f;
        public HarvestState State { get; private set; }
        YardSession session;
        bool inputArmed;
        public bool InputArmed => inputArmed;
        bool gathering;
        LooseProp throwProp;
        float throwCharge;
        public bool ThrowCharging => throwProp != null;
        public bool HasHeldObject => State != null && (State.IsHeld || State.FinishedHeld || (looseProps != null && looseProps.Held != null) || (peppers != null && peppers.Held != null));
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
                station.inputCapacity, station.outputCapacity, station.batchDuration, finished.carrier.DockPose, finished.carrier.capacity);
            finished.Initialize(owner);
            if (looseProps != null) looseProps.Initialize(owner);
            if (peppers != null) peppers.Initialize(owner);
            if (machine != null) machine.Initialize(owner);
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
            finished.Tick(dt);
            if (looseProps != null) looseProps.Tick();
            if (peppers != null) peppers.Tick(dt);
            if (machine != null) machine.Render();
            presentation.Tick(dt);
            if (finished.carrier.IsReceiving) { ShowStatus(); return; }
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
                if (!input.Use.IsPressed() && !input.Interact.IsPressed() && !input.Drop.IsPressed() && !input.Grab.IsPressed() && !input.Rotate.IsPressed() && (input.Pour == null || !input.Pour.IsPressed())) inputArmed = true;
                ShowStatus();
                return;
            }
            if (State.IsHeld)
            {
                crate.Rotate(input.Rotate.ReadValue<float>() * 90 * dt);
                crate.portable.QueryPlacement(session.player.view, session.player.transform.eulerAngles.y + crate.RotationOffset);
            }
            else crate.portable.HidePreview();
            if (State.FinishedHeld) finished.QueryPlacement(dt);
            if (looseProps != null) looseProps.QueryPlacement(dt);
            if (peppers != null && peppers.HandleInput(dt)) { ShowStatus(); return; }
            if (machine != null && machine.HandleInput(dt)) { ShowStatus(); return; }
            // Physical release wins over use/placement, including simultaneous throw release.
            if (input.Drop.WasPressedThisFrame()) ReleaseHeld(false);
            else if (input.Grab.WasPressedThisFrame())
            {
                if (HasHeldObject) ReleaseHeld(false);
                else if (looseProps != null && looseProps.TryGrab()) { }
                else if (finished.TryGrab()) { }
                else if (session.targeting.Current == crate.target && State.PickUp())
                {
                    Interrupt(); presentation.HandleCrate(); Render(); session.hud.Notice("Crate picked up");
                }
            }
            else if (input.Interact.WasPressedThisFrame())
            {
                if (looseProps != null && looseProps.Held != null) looseProps.Release(true);
                else if (finished.TryInteract()) { }
                else if (session.targeting.Current == station.intakeTarget)
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
                else if (State.FinishedHeld) finished.Release(true);
                else if (State.IsHeld)
                {
                    if (crate.Release(State, true)) { Interrupt(); presentation.HandleCrate(); Render(); session.hud.Notice("Crate placed - contents kept"); }
                    else session.hud.Notice(crate.portable.PlacementReason);
                }
            }
            if (!inputArmed) { ShowStatus(); return; }
            if (input.Use.WasPressedThisFrame())
            {
                gathering = State.IsHeld;
                throwProp = looseProps != null ? looseProps.Held : null;
                throwCharge = 0;
            }
            if (throwProp != null)
            {
                if (looseProps.Held != throwProp) { throwProp = null; throwCharge = 0; }
                else if (input.Use.WasReleasedThisFrame()) looseProps.Release(false, Mathf.Clamp01(throwCharge / .8f));
                else if (input.Use.IsPressed()) throwCharge = Mathf.Min(.8f, throwCharge + dt);
            }
            if (!input.Use.IsPressed()) gathering = false;
            if (!inputArmed || (looseProps != null && looseProps.Held != null)) { ShowStatus(); return; }

            if (peppers != null) { ShowStatus(); return; }
            var region = session.targeting.CurrentRegion;
            string id = region != null ? region.regionId : null;
            var status = State.CanGather(id);
            // Release/repress alone does not replay denial; only a meaningful valid state resets it.
            if (status == GatherStatus.Ready) lastDenial = null;
            if (gathering && input.Use.IsPressed())
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

        void ReleaseHeld(bool careful)
        {
            if (looseProps != null && looseProps.Held != null) looseProps.Release(careful);
            else if (State.FinishedHeld) finished.Release(careful);
            else if (State.IsHeld)
            {
                if (crate.Release(State, careful)) { Interrupt(); Render(); }
                else session.hud.Notice("Move the held crate clear of the obstruction to release");
            }
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
            if (machine != null) machine.Cancel();
            if (peppers != null) peppers.Cancel();
            gathering = false;
            throwProp = null;
            throwCharge = 0;
            crate.portable.ClearHeldMotion();
            if (finished != null) finished.carrier.portable.ClearHeldMotion();
            if (looseProps != null && looseProps.Held != null) looseProps.Held.portable.ClearHeldMotion();
            presentation.Interrupt();
            tipping.Interrupt();
            station.Interrupt();
            crate.portable.HidePreview();
            if (State != null) { finished.Interrupt(); crate.Render(State); finished.Render(); }
        }

        public void Recover()
        {
            Interrupt();
            crate.Recover(State);
            finished.carrier.Recover(State);
            if (looseProps != null) looseProps.Recover();
            if (peppers != null) peppers.Recover();
            Render();
        }

        public void ResetPrototype()
        {
            Interrupt();
            State.ResetPrototype();
            crate.ResetPose(State);
            finished.carrier.ReturnToDock();
            if (peppers != null) peppers.ResetViews();
            cooldown = 0;
            lastDenial = null;
            lastTipDenial = null;
            Render();
        }

        public void Render()
        {
            if (peppers == null) foreach (var region in regions) region.Render(State.UnitsIn(region.regionId));
            crate.Render(State);
            station.Render(State);
            finished.Render();
            if (machine != null) machine.Render();
            ShowStatus();
        }

        void ShowStatus()
        {
            statusText.text = "Crate " + State.RawUnits + " / " + State.Capacity + (State.IsHeld ? "  |  Carrying" : "  |  Released") +
                "    Peppers left " + State.Remaining;
            if (session.IsPaused) return;
            if (machine != null && machine.ShowStatus()) return;
            if (peppers != null && peppers.ShowStatus()) return;
            if (looseProps != null && looseProps.ShowStatus()) return;
            if (finished.ShowStatus()) return;
            if (session.targeting.Current == station.intakeTarget)
            {
                var tipStatus = State.CanTip();
                session.hud.targetText.text = tipping.IsPlaying ? "Tipping load\n" + State.RawUnits + " left in crate" :
                    tipStatus == TipStatus.Ready ? "E  Tip load\nIntake accepts " + Mathf.Min(State.RawUnits, State.InputCapacity - State.QueuedUnits) + " peppers" :
                    tipStatus == TipStatus.InputFull ? "Input full - load kept\nCollect finished food from the receiving tray" :
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
                    status == GatherStatus.Empty ? "Ground cleared here\nAim at another clump" : "Aim at a reachable pepper clump";
            }
            else if (session.targeting.Current == crate.target) session.hud.targetText.text = "Crate  " + State.RawUnits + " / 12\nRight click  Grab";
            else if (session.targeting.CurrentRegion != null) session.hud.targetText.text = "Pick up the crate first";
        }
    }
}
