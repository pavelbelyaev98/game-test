using UnityEngine;

namespace JustAFewPeppers
{
    // Two protected, directly driven sliders. Food transactions remain in HarvestState.
    public sealed class MachineOperation : MonoBehaviour
    {
        public YardTarget operationTarget;
        public Transform rack;
        public Transform guide;
        public Transform[] preparedFood;
        public AudioSource audioSource;
        public AudioClip contactClip;
        public float rackDistance = .65f;
        public float guideDistance = .85f;
        public float mouseStrokePixels = 240;
        public float keyboardStrokeRate = .9f;
        public bool Engaged { get; private set; }
        public float OperationHeldSeconds { get; private set; }
        public float GroupingHeldSeconds { get; private set; }
        public int ContactCues { get; private set; }
        YardSession session;
        bool grouping, keyboard;
        Vector3 rackRest, guideRest;
        Vector3[] preparedRest, queuedRest;
        string lastDenial;
        int nextContact;
        HarvestState State => session.handling.State;
        YardTarget Output => session.handling.finished.outputTarget;
        YardTarget SelectedTarget => grouping ? Output : operationTarget;

        public bool ControlsPlayer
        {
            get
            {
                if (session == null || !session.handling.InputArmed) return false;
                var input = session.Input;
                if (Engaged) return keyboard ? input.Interact.IsPressed() : input.Use.IsPressed();
                bool target = session.targeting.Current == operationTarget || session.targeting.Current == Output;
                return target && !session.handling.HasHeldObject && (input.Use.WasPressedThisFrame() || input.Interact.WasPressedThisFrame());
            }
        }

        public void Initialize(YardSession owner)
        {
            session = owner; rackRest = rack.localPosition; guideRest = guide.localPosition;
            preparedRest = new Vector3[preparedFood.Length];
            for (int i = 0; i < preparedRest.Length; i++) preparedRest[i] = preparedFood[i].localPosition;
            var queued = owner.handling.station.queuedPeppers;
            queuedRest = new Vector3[queued.Length];
            for (int i = 0; i < queued.Length; i++) queuedRest[i] = queued[i].transform.localPosition;
            Render();
        }

        public bool HandleInput(float dt)
        {
            var input = session.Input;
            if ((Engaged || State.HasStroke) && (input.Grab.WasPressedThisFrame() || input.Drop.WasPressedThisFrame()))
            { Cancel(); session.hud.Notice("Stroke cancelled - food stays here"); return true; }
            if (Engaged)
            {
                if (!(keyboard ? input.Interact.IsPressed() : input.Use.IsPressed()))
                { Engaged = false; return true; } // Rest at this uncommitted position until resumed or cancelled.
                if (session.targeting.Current != SelectedTarget || session.handling.HasHeldObject)
                { Cancel(); return true; }
                float delta = keyboard ? (grouping ? input.Move.ReadValue<Vector2>().x : input.Move.ReadValue<Vector2>().y) * keyboardStrokeRate * dt
                    : (grouping ? input.Look.ReadValue<Vector2>().x : input.Look.ReadValue<Vector2>().y) / mouseStrokePixels;
                delta = Mathf.Clamp(delta, -.35f, .35f);
                if (grouping) GroupingHeldSeconds += dt; else OperationHeldSeconds += dt;
                int committed = grouping ? State.MoveGrouping(delta) : State.MoveOperation(delta);
                float stroke = grouping ? State.GroupingStroke : State.OperationStroke;
                if (delta > 0 && stroke >= nextContact * .25f) { Sound(.08f); nextContact++; }
                if (committed > 0)
                {
                    bool received = grouping;
                    session.handling.Interrupt();
                    Sound(.25f);
                    if (received) session.handling.finished.carrier.BeginReceive();
                    session.handling.Render();
                    session.hud.Notice(received ? "Grouped " + committed + " prepared peppers - ready for handoff" : "Batch loaded - processing " + committed + " peppers");
                }
                Render(); return true;
            }
            bool freshMouse = input.Use.WasPressedThisFrame(), freshKey = input.Interact.WasPressedThisFrame();
            var target = session.targeting.Current;
            if (target != operationTarget && target != Output)
            {
                if (State.HasStroke && (freshMouse || freshKey || input.Grab.WasPressedThisFrame())) Cancel();
                return false;
            }
            if (!freshMouse && !freshKey) return false;
            if (session.handling.HasHeldObject)
            {
                if (freshKey) { Deny("Release the held object before operating the machine"); return true; }
                return false;
            }
            bool nextGrouping = target == Output;
            if (State.HasStroke && nextGrouping != grouping) Cancel();
            grouping = nextGrouping;
            bool begun = grouping ? State.BeginGrouping() : State.BeginOperation();
            if (!begun) { Deny(Reason(grouping)); return true; }
            lastDenial = null; keyboard = !freshMouse; Engaged = true;
            nextContact = Mathf.FloorToInt((grouping ? State.GroupingStroke : State.OperationStroke) * 4) + 1;
            return true; // Initial press captures the gesture; displacement begins with subsequent input.
        }

        void Sound(float volume) { audioSource.PlayOneShot(contactClip, volume); ContactCues++; }
        void Deny(string reason)
        {
            session.hud.Notice(reason);
            if (lastDenial == reason) return;
            lastDenial = reason; Sound(.08f);
        }

        string Reason(bool forGrouping)
        {
            if (forGrouping)
            {
                if (!State.FinishedDocked) return "Hand off the loaded carrier first";
                if (State.OutputUnits == 0) return "Prepared food will wait here";
                return "Release the held object before grouping";
            }
            if (State.PourOpen) return "Finish pouring first";
            if (State.ActiveUnits > 0) return "Batch working - no further operation needed";
            if (State.QueuedUnits == 0) return "Pour a load into the intake first";
            if (State.OutputFull) return "Output full - group the prepared food first";
            return "Release the held object before operating";
        }

        public void Cancel()
        {
            Engaged = false; lastDenial = null;
            if (session == null) return;
            State.CancelStrokes(); audioSource.Stop(); Render();
        }

        public void ResetMeasurements() { OperationHeldSeconds = GroupingHeldSeconds = 0; ContactCues = 0; }

        public void Render()
        {
            if (session == null) return;
            rack.localPosition = rackRest + Vector3.forward * (rackDistance * State.OperationStroke);
            guide.localPosition = guideRest + Vector3.right * (guideDistance * State.GroupingStroke);
            for (int i = 0; i < preparedFood.Length; i++)
            {
                var food = preparedFood[i]; food.gameObject.SetActive(i < State.OutputUnits);
                var packed = preparedRest[i]; packed.x = guideRest.x + guideDistance - .16f + i % 3 * .045f;
                packed.z = guideRest.z + (i / 3 % 2 - .5f) * .08f; packed.y += i / 6 * .035f;
                food.localPosition = i < State.GroupingSelection ? Vector3.Lerp(preparedRest[i], packed, State.GroupingStroke) : preparedRest[i];
            }
            var queued = session.handling.station.queuedPeppers;
            for (int i = 0; i < queued.Length; i++) queued[i].transform.localPosition = queuedRest[i] +
                (i < State.OperationSelection ? Vector3.forward * (rackDistance * State.OperationStroke) : Vector3.zero);
        }

        public bool ShowStatus()
        {
            var target = session.targeting.Current;
            if (target != operationTarget && target != Output) return false;
            bool output = target == Output;
            if (session.handling.HasHeldObject)
                session.hud.targetText.text = "Release the held object to " + (output ? "group prepared food" : "operate the rack");
            else if (output && State.CanGroup() != GroupingStatus.Ready && State.CanGroup() != GroupingStatus.Grouping)
                session.hud.targetText.text = State.FinishedDocked ? "Prepared food will wait here" : "Hand off the loaded carrier first";
            else if (!output && State.CanOperate() != OperationStatus.Ready && State.CanOperate() != OperationStatus.Operating)
            {
                session.hud.targetText.text = Reason(false);
            }
            else session.hud.targetText.text = (output ? "Group " + (State.GroupingSelection > 0 ? State.GroupingSelection : Mathf.Min(State.OutputUnits, State.FinishedCapacity)) + " prepared peppers"
                    : "Slide the loaded rack into the machine") +
                (output ? "\nHold left mouse + drag RIGHT   |   Hold E + D / A" : "\nHold left mouse + drag UP   |   Hold E + W / S") +
                (State.HasStroke ? "\nRelease to stop   Right click to cancel" : "");
            return true;
        }
    }
}
