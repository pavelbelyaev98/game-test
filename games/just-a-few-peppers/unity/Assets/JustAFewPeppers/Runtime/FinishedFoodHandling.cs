using UnityEngine;
using UnityEngine.UI;

namespace JustAFewPeppers
{
    // Resolves finished-food targets; the existing session still owns input, time and the sole model.
    public sealed class FinishedFoodHandling : MonoBehaviour
    {
        public FinishedCarrierView carrier;
        public YardTarget outputTarget;
        public YardTarget rackTarget;
        public FoodGroupView storedFood;
        public Text statusText;
        public TextMesh rackLabel;
        public AudioSource audioSource;
        public AudioClip transferClip;
        public int DepositCues { get; private set; }
        YardSession session;
        YardHandling handling;
        HarvestState State => handling.State;
        Vector3 storedRest;
        float settling;

        public void Initialize(YardSession owner)
        {
            session = owner;
            handling = owner.handling;
            storedRest = storedFood.transform.localPosition;
            carrier.Initialize(owner.player);
        }

        public void Tick(float dt)
        {
            carrier.Tick(dt, State);
            settling = Mathf.Max(0, settling - dt);
            storedFood.transform.localPosition = storedRest + Vector3.up * (.06f * settling / .3f);
            Render();
        }

        public void QueryPlacement(float dt)
        {
            carrier.Rotate(session.Input.Rotate.ReadValue<float>() * 90 * dt);
            carrier.portable.QueryPlacement(session.player.view, session.player.transform.eulerAngles.y + carrier.RotationOffset);
        }

        public bool TryGrab()
        {
            if (handling.HasHeldObject || session.targeting.Current != carrier.target || !State.PickUpFinished()) return false;
            handling.Interrupt(); handling.Render();
            session.hud.Notice("Finished food picked up - contents kept");
            return true;
        }

        public bool TryInteract()
        {
            var target = session.targeting.Current;
            if (target == outputTarget)
            {
                if (target == outputTarget && !State.FinishedDocked)
                {
                    session.hud.Notice("Use the loaded finished carrier first - it returns here after handoff");
                    return true;
                }
                if (State.FinishedDocked && State.OutputUnits == 0)
                {
                    session.hud.Notice("No finished food ready yet");
                    return true;
                }
                if (handling.HasHeldObject)
                {
                    session.hud.Notice("Release the held object before collecting finished food");
                    return true;
                }
                if (State.CollectOutput() == 0) return true;
                handling.Interrupt();
                carrier.BeginReceive();
                audioSource.PlayOneShot(transferClip, .3f);
                handling.Render();
                session.hud.Notice("Received " + State.FinishedUnits + " finished peppers - bring them to the handoff rack");
                return true;
            }
            if (target == rackTarget)
            {
                int accepted = State.DepositFinished();
                if (accepted == 0) { session.hud.Notice("Bring the finished-food carrier from the receiving tray"); return true; }
                handling.Interrupt();
                carrier.ReturnToDock();
                settling = .3f;
                DepositCues++;
                audioSource.PlayOneShot(transferClip, .35f);
                handling.Render();
                session.hud.Notice("Stored " + accepted + " peppers for winter - empty carrier returned to the receiving tray");
                return true;
            }
            if (target == handling.station.intakeTarget && State.FinishedHeld)
            {
                session.hud.Notice("Finished food goes to the handoff rack");
                return true;
            }
            return false;
        }

        public void Release(bool careful)
        {
            if (carrier.Release(State, careful))
            {
                handling.Interrupt(); handling.Render();
                session.hud.Notice(careful ? "Finished carrier placed - food kept" : "Finished carrier dropped - food kept");
            }
            else session.hud.Notice(careful ? carrier.portable.PlacementReason : "Move the held carrier clear of the obstruction to drop");
        }

        public void Interrupt()
        {
            carrier.Interrupt();
            carrier.portable.HidePreview();
            audioSource.Stop();
            settling = 0;
            storedFood.transform.localPosition = storedRest;
        }

        public void Render()
        {
            carrier.Render(State);
            storedFood.Render(State.StoredUnits);
            statusText.text = "Winter food stored  " + State.StoredUnits + " / " + State.InitialHarvest +
                "    Finished carrier  " + State.FinishedUnits + " / " + State.FinishedCapacity;
            rackLabel.text = "FINISHED FOOD HANDOFF RACK\nOne handoff - jars go to the household";
        }

        public bool ShowStatus()
        {
            if (session.IsPaused) return false;
            var target = session.targeting.Current;
            string text;
            if (carrier.IsReceiving) text = "Receiving finished food";
            else if (target == rackTarget) text = State.FinishedHeld ? "E  Hand off " + State.FinishedUnits + " finished peppers\nEmpty carrier returns automatically" : "Finished Food Handoff Rack\nBring the finished carrier from the receiving tray";
            else if (target == outputTarget) text = !State.FinishedDocked ? "Output " + State.OutputUnits + " / " + State.OutputCapacity + "\nHandoff the loaded carrier to reuse it" :
                State.OutputUnits > 0 ? "E  Collect " + Mathf.Min(State.OutputUnits, State.FinishedCapacity) + " finished peppers\nPartial loads are ready too" : "Receiving tray\nFinished food will wait here";
            else if (target == carrier.target) text = "Finished food  " + State.FinishedUnits + "\nRight click  Grab carrier";
            else if (State.FinishedHeld) text = target == handling.station.intakeTarget ? "Bring finished food to the handoff rack" : "";
            else return false;
            session.hud.targetText.text = text;
            return true;
        }
    }
}
