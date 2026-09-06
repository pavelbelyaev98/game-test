using UnityEngine;

namespace JustAFewPeppers
{
    // A fixed pool of visual proxies, with no delayed transfer or completion callback.
    public sealed class TipPresentation : MonoBehaviour
    {
        public RawCarrierView crate;
        public Transform destination;
        public Transform[] flyingPeppers;
        public AudioSource audioSource;
        public AudioClip impactClip;
        public AudioClip finishClip;
        public float duration = .75f;
        public bool IsPlaying { get; private set; }
        public int TipCues { get; private set; }
        float elapsed;
        int visibleCount;
        bool impactPlayed;

        public void Begin(int accepted)
        {
            elapsed = 0;
            visibleCount = Mathf.Min(flyingPeppers.Length, accepted);
            impactPlayed = false;
            IsPlaying = true;
            TipCues++;
        }

        public void Tick(float dt, HarvestState state)
        {
            if (!IsPlaying) return;
            elapsed += dt;
            float t = Mathf.Clamp01(elapsed / duration);
            // Raise the pouring edge beside the tray before food moves, keeping the intake visible.
            crate.TipBlend = Mathf.SmoothStep(0, 1, t / .12f) * Mathf.SmoothStep(0, 1, (1 - t) / .16f);
            var forward = Vector3.ProjectOnPlane(crate.carryAnchor.forward, Vector3.up).normalized;
            crate.TipPosition = destination.position - crate.carryAnchor.right * .9f + Vector3.up * .45f - forward * .35f;
            crate.Render(state);
            for (int i = 0; i < flyingPeppers.Length; i++)
            {
                float flight = (t - .12f - i * .025f) / .48f;
                bool visible = i < visibleCount && flight >= 0 && flight <= 1;
                flyingPeppers[i].gameObject.SetActive(visible);
                if (!visible) continue;
                var offset = new Vector3((i % 3 - 1) * .12f, 0, i / 3 * .04f);
                var from = crate.transform.TransformPoint(new Vector3(.48f, .42f, 0));
                flyingPeppers[i].position = Vector3.Lerp(from + offset, destination.position + offset, flight) +
                    Vector3.up * (Mathf.Sin(flight * Mathf.PI) * .04f);
                flyingPeppers[i].rotation = Quaternion.Euler(flight * 240, i * 73, flight * 100);
            }
            if (!impactPlayed && t >= .45f)
            {
                impactPlayed = true;
                audioSource.PlayOneShot(impactClip, .4f);
            }
            if (t >= 1)
            {
                Interrupt();
                crate.Render(state);
                if (state.RawUnits == 0) audioSource.PlayOneShot(finishClip, .25f);
            }
        }

        public void Interrupt()
        {
            IsPlaying = false;
            crate.TipBlend = 0;
            foreach (var pepper in flyingPeppers) pepper.gameObject.SetActive(false);
            audioSource.Stop();
        }
    }
}
