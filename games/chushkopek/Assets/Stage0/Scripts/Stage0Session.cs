using UnityEngine;

namespace Chushkopek.Stage0
{
    public sealed class Stage0Session : MonoBehaviour
    {
        [Range(8, 12)] public float idealRoastSeconds = 10f;
        [Range(5, 10)] public float perfectGraceSeconds = 7f;
        [Range(1, 5)] public float steamSeconds = 3f;
        public PepperPresentation pepper;
        public RoasterStation roaster;
        public SteamingStation steaming;
        public FinishedSocket tray;
        public Stage0Audio sound;
        public Stage0Interaction interaction;
        public Transform startSocket;
        public Transform carrySocket;
        public PepperState State { get; private set; }
        public bool Paused { get; private set; }
        public string Notice { get; private set; } = "One pepper. Roast, steam, peel, finish.";
        public float NoticeUntil { get; private set; }
        public int Revision { get; private set; }

        void Awake()
        {
            State = new PepperState(idealRoastSeconds, perfectGraceSeconds, steamSeconds);
            Application.targetFrameRate = 120;
        }

        void Update()
        {
            if (Paused) return;
            PepperStage previous = State.Stage;
            State.Tick(Time.deltaTime);
            if (State.Stage != previous) OnTransition();
        }

        public bool Pick()
        {
            if (!State.TryPick(out string reason)) { Explain(reason); return false; }
            sound.Cue(Stage0Audio.CueKind.Pick);
            Explain(State.Stage == PepperStage.Perfect || State.Stage == PepperStage.Burnt
                ? "Heat stopped. Put the pepper under the steam cover." : "Pepper held. Click a position to place it.");
            return true;
        }

        public bool Place(PepperLocation location)
        {
            if (!State.TryPlace(location, out string reason)) { Explain(reason); return false; }
            if (location == PepperLocation.Roaster) { sound.Cue(Stage0Audio.CueKind.Insert); Explain("Heating. Listen for the brighter pops and ready chime."); }
            else if (location == PepperLocation.Steam) { sound.Cue(Stage0Audio.CueKind.Cover); Explain("Steam loosens the skin..."); }
            else if (location == PepperLocation.Finished) OnTransition();
            else sound.Cue(Stage0Audio.CueKind.Pick);
            return true;
        }

        public void Peel(int strip, float amount)
        {
            float previous = State.StripProgress(strip);
            if (!State.AdvancePeel(strip, amount)) return;
            sound.PeelMotion(amount);
            if (previous < 1f && State.StripProgress(strip) >= 1f)
            {
                sound.Cue(Stage0Audio.CueKind.Release);
                pepper.ReleaseStrip(strip);
                if (State.Stage == PepperStage.Peeled) Explain("Both strips free. Lift the peeled pepper and place it on the tray.");
                else Explain("Skin released. Pull the other broad strip.");
            }
        }

        void OnTransition()
        {
            switch (State.Stage)
            {
                case PepperStage.Perfect:
                    sound.Cue(Stage0Audio.CueKind.Ready);
                    Explain("READY — lift it now. The perfect window is forgiving.");
                    break;
                case PepperStage.Burnt:
                    sound.Cue(Stage0Audio.CueKind.Burnt);
                    Explain("Burnt, but still usable. Lift it and steam as usual.");
                    break;
                case PepperStage.Peelable:
                    sound.Cue(Stage0Audio.CueKind.Ready);
                    Explain("Skin ready. Hold either broad edge and pull along the pepper.");
                    break;
                case PepperStage.Finished:
                    sound.Cue(Stage0Audio.CueKind.Finish);
                    Explain("Pepper prepared. Look at what changed. R starts another cycle.", 30f);
                    break;
            }
        }

        public Transform CurrentSocket()
        {
            switch (State.Location)
            {
                case PepperLocation.Held: return carrySocket;
                case PepperLocation.Roaster: return roaster.socket;
                case PepperLocation.Steam: return steaming.socket;
                case PepperLocation.Finished: return tray.socket;
                default: return startSocket;
            }
        }

        public void CancelCarry()
        {
            if (State.TryCancelCarry()) Explain("Returned to the previous position.");
        }

        public void ResetCycle()
        {
            State.Reset();
            Revision++;
            sound.StopFeedback();
            interaction.ResetView();
            pepper.ResetPose();
            Explain("Fresh pepper. Try the cycle again.");
        }

        public void Explain(string text, float seconds = 4f) { Notice = text; NoticeUntil = Time.unscaledTime + seconds; }

        public void SetPaused(bool value)
        {
            Paused = value;
            Time.timeScale = value ? 0f : 1f;
            AudioListener.pause = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = value;
            if (value) interaction.EndDrag();
        }

        void OnApplicationFocus(bool focused) { if (!focused && State != null) SetPaused(true); }
        void OnDestroy() { Time.timeScale = 1f; AudioListener.pause = false; Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
    }
}
