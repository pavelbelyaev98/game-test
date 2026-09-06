using UnityEngine;

namespace Chushkopek.Stage0
{
    public sealed class SteamingStation : MonoBehaviour
    {
        public Transform socket;
        public Transform cover;
        public ParticleSystem steam;
        public Stage0Session session;
        Vector3 openPosition;
        Vector3 closedPosition;
        bool wasSteaming;
        int revision = -1;

        void Start() { openPosition = cover.localPosition; closedPosition = openPosition + new Vector3(0, -.27f, -.16f); }
        void Update()
        {
            if (revision != session.Revision) { steam.Clear(); cover.localPosition = openPosition; revision = session.Revision; wasSteaming = false; }
            bool active = session.State.Stage == PepperStage.Steaming;
            if (active && !wasSteaming) steam.Emit(16);
            if (!active && wasSteaming) { steam.Emit(10); session.sound.Cue(Stage0Audio.CueKind.Cover); }
            var emission = steam.emission;
            emission.rateOverTime = active ? 12f : 0f;
            cover.localPosition = Vector3.Lerp(cover.localPosition, active ? closedPosition : openPosition, 1f - Mathf.Exp(-12f * Time.deltaTime));
            session.sound.SetSteaming(active);
            wasSteaming = active;
        }
    }
}
