using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JustAFewPeppers
{
    public sealed class YardHud : MonoBehaviour
    {
        public GameObject pausePanel;
        public GameObject helpPanel;
        public GameObject reticle;
        public Text targetText;
        public Text pauseTitle;
        public Text noticeText;
        public Button resumeButton;
        public Button resetButton;
        public Button quitButton;
        public Button restartPrototypeButton;
        public EventSystem events;
        public Text guidanceText;
        public GameObject guidanceBackdrop;
        public Text controlsText;
        public Slider sensitivitySlider;
        public Text sensitivityText;
        YardSession session;
        float noticeUntil;

        public void Bind(YardSession session)
        {
            this.session = session;
            resumeButton.onClick.AddListener(session.Resume);
            resetButton.onClick.AddListener(session.ResetToSpawn);
            quitButton.onClick.AddListener(session.Quit);
            if (restartPrototypeButton != null) restartPrototypeButton.onClick.AddListener(session.RestartPrototype);
            if (sensitivitySlider != null)
            {
                sensitivitySlider.SetValueWithoutNotify(session.player.lookSensitivity / .1f);
                sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
                SetSensitivity(sensitivitySlider.value);
            }
        }

        void SetSensitivity(float multiplier)
        {
            session.player.lookSensitivity = multiplier * .1f;
            sensitivityText.text = "Mouse sensitivity  " + multiplier.ToString("0.00") + "x";
        }

        public void ShowPause(bool paused, string title)
        {
            pauseTitle.text = title;
            pausePanel.SetActive(paused);
            reticle.SetActive(!paused);
            targetText.gameObject.SetActive(!paused);
            if (helpPanel != null) helpPanel.SetActive(false);
            if (guidanceBackdrop != null) guidanceBackdrop.SetActive(false);
            events.SetSelectedGameObject(null);
            if (paused) events.SetSelectedGameObject(resumeButton.gameObject);
        }

        public void ShowHelp()
        {
            ShowGuidance();
            pausePanel.SetActive(false);
            helpPanel.SetActive(true);
            events.SetSelectedGameObject(null);
        }

        public void ShowTarget(YardTarget target)
        {
            targetText.text = target == null ? "" : target.displayName + "\n" + target.description;
        }

        public void Notice(string text)
        {
            noticeText.text = text;
            noticeUntil = Time.unscaledTime + 3;
        }

        // Read committed work and the current hands; no tutorial inventory or forced action order.
        public void ShowGuidance()
        {
            if (guidanceText == null || session.handling == null) return;
            var handling = session.handling;
            var state = handling.State;
            string text;
            if (state.StoredUnits == state.InitialHarvest)
                text = "All peppers stored for winter. The yard is still yours to explore.";
            else if (handling.peppers != null && handling.peppers.Held != null)
                text = "You are holding one pepper. E puts it on a surface or into the crate.\nRight click releases it; hold and release left mouse to throw.";
            else if (state.UncontainedUnits > 0 && !state.PourOpen)
                text = "Loose peppers are still part of the food job.\nGather them with the crate, or press R to regroup strays at the pile.";
            else if (handling.looseProps != null && handling.looseProps.Held != null)
                text = "Right click releases this object with physics.\nHold left mouse, then release to throw; E carefully sets it down.";
            else if (handling.tipping.IsPlaying)
                text = "The tipped food is in the processor.\nAny unaccepted peppers stay in your crate.";
            else if (handling.finished.carrier.IsReceiving)
                text = "Receiving your finished food.\nThe carrier is reusable, including for a small final batch.";
            else if (state.FinishedHeld)
                text = "Bring this food to the handoff rack behind the processor.\nAim at the rack and press E to store it; the empty carrier returns.";
            else if (state.FinishedUnits > 0)
                text = "Your finished food is in the carrier you set down.\nLook at it and right click to pick it up; R can recover a lost carrier.";
            else if (state.OutputUnits > 0 && (state.OutputFull || !state.IsHeld || state.RawUnits == 0 || state.Remaining == 0))
                text = "Finished food is ready at the tray on the processor's right.\nLook at the tray and press E to collect; partial loads are ready too.";
            else if (state.IsHeld && state.QueuedUnits == state.InputCapacity)
                text = "The processor is working; your remaining load is kept.\nYou can set the crate down with E while aiming at the ground.";
            else if (state.IsHeld && state.RawUnits > 0 && (state.RawUnits == state.Capacity || state.Remaining == 0 || session.targeting.Current == handling.station.intakeTarget))
                text = "Bring the crate to the round intake.\nAim at the intake and press E to tip; a partial crate is enough.";
            else if (state.IsHeld && state.Remaining > 0)
                text = "Aim at the pepper pile and hold left mouse to gather.\nRelease to stop. You can pour before the crate is full.";
            else if (state.RawUnits > 0)
                text = "Your raw load is in the crate you set down.\nLook at the crate and right click to pick it up again.";
            else if (state.Remaining == 0)
                text = "All raw peppers are gathered; food is still in the processor.\nFinished food waits safely at the tray on its right.";
            else
                text = "The orange crate beside the pepper pile starts the food loop.\nPick it up, gather peppers, tip into the intake, collect and hand off food.";
            if (handling.peppers != null) text += "\n\nF9 comparison: " + (handling.peppers.simulation == PepperSimulation.PhysicalBatch ? "physical batch" : "grouped resting supply");
            guidanceText.text = text;
        }

        void Update()
        {
            if (Time.unscaledTime > noticeUntil) noticeText.text = "";
        }
    }
}
