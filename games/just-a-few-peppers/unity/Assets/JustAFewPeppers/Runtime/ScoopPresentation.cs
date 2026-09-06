using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class ScoopPresentation : MonoBehaviour
    {
        public Transform[] flyingClumps;
        public AudioSource actionAudio;
        public AudioSource feedbackAudio;
        public AudioClip scoopClip;
        public AudioClip crateClip;
        public AudioClip softCue;
        public float flightDuration = .32f;
        Vector3 origin;
        Transform destination;
        float elapsed;
        bool flying;
        public int ScoopCues { get; private set; }
        public int FeedbackCues { get; private set; }

        public void Scoop(Vector3 from, Transform to)
        {
            origin = from;
            destination = to;
            elapsed = 0;
            flying = true;
            foreach (var clump in flyingClumps) clump.gameObject.SetActive(true);
            actionAudio.pitch = 1 + (ScoopCues % 3 - 1) * .06f;
            actionAudio.PlayOneShot(scoopClip, .32f);
            ScoopCues++;
            Tick(0);
        }

        public void Tick(float dt)
        {
            if (!flying) return;
            elapsed += dt;
            float t = Mathf.Clamp01(elapsed / flightDuration);
            for (int i = 0; i < flyingClumps.Length; i++)
            {
                var offset = new Vector3((i - 1) * .07f, 0, i % 2 * .06f);
                flyingClumps[i].position = Vector3.Lerp(origin + offset, destination.position + offset, t) + Vector3.up * (Mathf.Sin(t * Mathf.PI) * .28f);
                flyingClumps[i].rotation = Quaternion.Euler(t * 180, i * 70, t * 100);
            }
            if (t >= 1) CancelMotion();
        }

        public void Feedback()
        {
            feedbackAudio.PlayOneShot(softCue, .2f);
            FeedbackCues++;
        }

        public void HandleCrate() => actionAudio.PlayOneShot(crateClip, .35f);

        public void CancelMotion()
        {
            flying = false;
            foreach (var clump in flyingClumps) clump.gameObject.SetActive(false);
        }

        public void Interrupt()
        {
            CancelMotion();
            actionAudio.Stop();
            feedbackAudio.Stop();
        }
    }
}
