using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chushkopek.Stage0
{
    // Small, self-contained synthesized feedback palette. No downloaded or unlicensed audio.
    public sealed class Stage0Audio : MonoBehaviour
    {
        public enum CueKind { Pick, Insert, Cover, Ready, Burnt, Release, Finish, Pop }
        readonly List<AudioClip> clips = new List<AudioClip>();
        readonly Dictionary<CueKind, AudioClip> cues = new Dictionary<CueKind, AudioClip>();
        AudioSource roast, steam, peel, oneShots;
        float peelDrive;
        float nextPop;

        void Awake()
        {
            roast = Source(Noise("Warm sizzle", 1.4f, .55f, false), true);
            steam = Source(Noise("Steam hiss", 1.2f, .82f, false), true);
            peel = Source(Noise("Skin friction", .9f, .4f, false), true);
            oneShots = gameObject.AddComponent<AudioSource>();
            oneShots.playOnAwake = false;
            oneShots.spatialBlend = 0f;
            cues[CueKind.Pick] = Impact("Soft grip", 190f, .08f, .22f);
            cues[CueKind.Insert] = Impact("Roaster insertion", 340f, .15f, .55f);
            cues[CueKind.Cover] = Impact("Cover tap", 520f, .16f, .3f);
            cues[CueKind.Pop] = Impact("Blister pop", 850f, .055f, .85f);
            cues[CueKind.Release] = Noise("Crisp skin release", .17f, .62f, true);
            cues[CueKind.Ready] = Tone("Ready", new[] { 880f, 1174.66f }, .45f);
            cues[CueKind.Burnt] = Tone("Too long", new[] { 220f, 196f }, .45f);
            cues[CueKind.Finish] = Tone("Prepared", new[] { 523.25f, 659.25f, 783.99f }, .8f);
        }

        AudioSource Source(AudioClip clip, bool loop)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.clip = clip;
            source.loop = loop;
            source.spatialBlend = 0f;
            source.volume = 0;
            return source;
        }

        public void SetRoasting(bool active, float fraction)
        {
            SetLoop(roast, active ? Mathf.Lerp(.12f, .3f, fraction) : 0f);
            roast.pitch = Mathf.Lerp(.85f, 1.18f, fraction);
            if (active && !AudioListener.pause && fraction > .55f && Time.time >= nextPop)
            {
                Cue(CueKind.Pop, Mathf.Lerp(.25f, .65f, fraction));
                nextPop = Time.time + Mathf.Lerp(1.3f, .42f, fraction) + UnityEngine.Random.Range(0f, .2f);
            }
        }

        public void SetSteaming(bool active) => SetLoop(steam, active ? .21f : 0f);
        public void PeelMotion(float amount) { peelDrive = Mathf.Clamp01(amount * 38f); }
        void Update() { SetLoop(peel, peelDrive * .28f); peelDrive = Mathf.MoveTowards(peelDrive, 0f, Time.deltaTime * 9f); }
        void SetLoop(AudioSource source, float volume)
        {
            if (volume > .001f && !source.isPlaying) source.Play();
            source.volume = Mathf.MoveTowards(source.volume, volume, Time.deltaTime * 3f);
            if (volume == 0f && source.volume < .001f) source.Stop();
        }
        public void Cue(CueKind kind, float volume = .7f) { oneShots.PlayOneShot(cues[kind], volume); }
        public void StopFeedback()
        {
            roast.Stop(); steam.Stop(); peel.Stop(); oneShots.Stop();
            roast.volume = steam.volume = peel.volume = 0f;
            peelDrive = nextPop = 0f;
        }

        AudioClip Noise(string name, float duration, float roughness, bool envelope)
        {
            return Create(name, duration, (t, noise, previous) => {
                float grain = noise * roughness + previous * (1f - roughness);
                float pulse = .6f + .2f * Mathf.Sin(t * 173f) + .15f * Mathf.Sin(t * 67f);
                float fade = envelope ? Mathf.Exp(-t * 23f) * Mathf.Min(1f, t * 900f) : 1f;
                return grain * pulse * fade * .75f;
            });
        }
        AudioClip Impact(string name, float pitch, float duration, float noiseMix)
        {
            return Create(name, duration, (t, noise, previous) =>
                (Mathf.Sin(t * pitch * Mathf.PI * 2f) * (1f - noiseMix) + noise * noiseMix) * Mathf.Exp(-t * 38f) * .6f);
        }
        AudioClip Tone(string name, float[] notes, float duration)
        {
            return Create(name, duration, (t, noise, previous) => {
                float sum = 0f;
                for (int i = 0; i < notes.Length; i++)
                {
                    float local = t - i * .085f;
                    if (local >= 0f) sum += Mathf.Sin(local * notes[i] * Mathf.PI * 2f) * Mathf.Exp(-local * 8f) * Mathf.Min(1f, local * 150f);
                }
                return sum * .15f;
            });
        }
        AudioClip Create(string name, float duration, Func<float, float, float, float> sample)
        {
            const int rate = 44100;
            var data = new float[(int)(duration * rate)];
            var random = new System.Random(137);
            float previous = 0f;
            for (int i = 0; i < data.Length; i++)
            {
                float noise = (float)random.NextDouble() * 2f - 1f;
                data[i] = sample(i / (float)rate, noise, previous);
                previous = noise * .25f + previous * .75f;
                // Prevent hard edge clicks at clip boundaries.
                data[i] *= Mathf.Min(1f, i / 80f, (data.Length - i - 1) / 120f);
            }
            var clip = AudioClip.Create(name, data.Length, 1, rate, false);
            clip.SetData(data, 0);
            clips.Add(clip);
            return clip;
        }
        void OnDestroy() { foreach (var clip in clips) if (clip) Destroy(clip); }
    }
}
