using UnityEngine;
using UnityEngine.UI;

namespace JustAFewPeppers
{
    // All geometry, stage names and jars are reconstructible views of the sole harvest model.
    public sealed class StationView : MonoBehaviour
    {
        public YardTarget intakeTarget;
        public Transform intake;
        [Min(1)] public int inputCapacity = 12;
        [Min(1)] public int outputCapacity = 12;
        [Min(.1f)] public float batchDuration = 4;
        public GameObject[] queuedPeppers;
        public GameObject[] jars;
        public Transform[] jarFood;
        public Renderer[] stageMarkers;
        public Transform feeder;
        public TextMesh stageLabel;
        public Text statusText;
        public AudioSource audioSource;
        public AudioClip completionClip;
        public int CompletionCues { get; private set; }
        MaterialPropertyBlock markerProperties;

        public string Stage(HarvestState state)
        {
            if (state.ActiveUnits == 0) return state.OutputFull ? "Output full - safely waiting" : "Ready for a load";
            float progress = 1 - (float)(state.BatchRemaining / state.BatchDuration);
            return progress < .35f ? "Roasting" : progress < .55f ? "Covered rest" : progress < .75f ? "Preparing" : "Packing / cooling";
        }

        public void Render(HarvestState state)
        {
            for (int i = 0; i < queuedPeppers.Length; i++) queuedPeppers[i].SetActive(i < state.QueuedUnits);
            for (int i = 0; i < jars.Length; i++)
            {
                int units = Mathf.Clamp(state.OutputUnits - i * 3, 0, 3);
                jars[i].SetActive(units > 0);
                var scale = jarFood[i].localScale;
                scale.y = .12f * units / 3;
                jarFood[i].localScale = scale;
                jarFood[i].localPosition = new Vector3(0, .02f + scale.y, 0);
            }
            float progress = state.ActiveUnits == 0 ? 0 : 1 - (float)(state.BatchRemaining / state.BatchDuration);
            int stage = progress < .35f ? 0 : progress < .55f ? 1 : progress < .75f ? 2 : 3;
            if (markerProperties == null) markerProperties = new MaterialPropertyBlock();
            for (int i = 0; i < stageMarkers.Length; i++)
            {
                markerProperties.SetColor("_Color", state.ActiveUnits > 0 && stage == i ? new Color(1, .7f, .16f) : new Color(.22f, .28f, .23f));
                stageMarkers[i].SetPropertyBlock(markerProperties);
            }
            feeder.localRotation = Quaternion.Euler(0, 0, state.ActiveUnits > 0 ? Mathf.Sin(progress * Mathf.PI * 16) * 12 : 0);
            stageLabel.text = Stage(state) + "\n" + state.OutputUnits + " / " + state.OutputCapacity + " ready";
            statusText.text = "Input " + state.QueuedUnits + " / " + state.InputCapacity + "    Working " + state.ActiveUnits +
                "    Finished " + state.OutputUnits + " / " + state.OutputCapacity + "    " + Stage(state);
        }

        public void Completed()
        {
            audioSource.PlayOneShot(completionClip, .25f);
            CompletionCues++;
        }

        public void Interrupt() => audioSource.Stop();
    }
}
