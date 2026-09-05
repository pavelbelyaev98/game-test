using UnityEngine;

namespace Chushkopek.Stage0
{
    public sealed class RoasterStation : MonoBehaviour
    {
        public Transform socket;
        public Stage0Session session;
        public Renderer indicator;
        public ParticleSystem smoke;
        Material lightMaterial;
        int revision = -1;

        void Start() { lightMaterial = indicator.material; }
        void Update()
        {
            if (revision != session.Revision) { smoke.Clear(); revision = session.Revision; }
            var p = session.State;
            Color color = !p.IsHeating ? new Color(.12f, .12f, .1f)
                : p.Stage == PepperStage.Perfect ? new Color(.35f, 1f, .25f)
                : p.Stage == PepperStage.Burnt ? new Color(1f, .15f, .04f) : new Color(1f, .4f, .04f);
            lightMaterial.color = color;
            lightMaterial.SetColor("_EmissionColor", color * (p.IsHeating ? 1.8f : .1f));
            var emission = smoke.emission;
            emission.rateOverTime = p.IsHeating && p.WasBurnt ? 5f : 0f;
            session.sound.SetRoasting(p.IsHeating, p.RoastFraction);
        }
        void OnDestroy() { if (lightMaterial) Destroy(lightMaterial); }
    }
}
