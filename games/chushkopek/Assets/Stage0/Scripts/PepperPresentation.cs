using UnityEngine;

namespace Chushkopek.Stage0
{
    public sealed class PepperPresentation : MonoBehaviour
    {
        public Stage0Session session;
        public Renderer flesh;
        public MeshFilter[] skin = new MeshFilter[2];
        public Renderer[] skinRenderers = new Renderer[2];
        public Transform[] looseEdges = new Transform[2];
        public Collider pickVolume;
        Mesh[] movingSkin = new Mesh[2];
        Vector3[][] vertices = new Vector3[2][];
        float[] lastProgress = { -1f, -1f };
        float[] releasedAt = { -1f, -1f };
        MaterialPropertyBlock properties;
        int revision = -1;
        public int HoverStrip { get; set; } = -1;

        void Start()
        {
            properties = new MaterialPropertyBlock();
            for (int i = 0; i < 2; i++)
            {
                movingSkin[i] = Instantiate(skin[i].sharedMesh);
                movingSkin[i].MarkDynamic();
                skin[i].sharedMesh = movingSkin[i];
                vertices[i] = movingSkin[i].vertices;
            }
            ResetPose();
        }

        public void ResetPose()
        {
            Transform socket = session.CurrentSocket();
            transform.SetPositionAndRotation(socket.position, socket.rotation);
            HoverStrip = -1;
            for (int i = 0; i < 2; i++)
            {
                releasedAt[i] = lastProgress[i] = -1f;
                skin[i].transform.localPosition = Vector3.zero;
                skin[i].transform.localRotation = Quaternion.identity;
                skinRenderers[i].enabled = true;
            }
        }

        void LateUpdate()
        {
            if (revision != session.Revision) { ResetPose(); revision = session.Revision; }
            Transform socket = session.CurrentSocket();
            float follow = 1f - Mathf.Exp(-24f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, socket.position, follow);
            transform.rotation = Quaternion.Slerp(transform.rotation, socket.rotation, follow);
            var p = session.State;
            pickVolume.enabled = p.Location != PepperLocation.Held;
            properties.Clear();
            properties.SetFloat("_Roast", p.RoastFraction);
            properties.SetFloat("_Burn", p.WasBurnt ? 1f : 0f);
            properties.SetFloat("_Ready", p.Stage >= PepperStage.Peelable ? 1f : 0f);
            properties.SetFloat("_Hover", 0f);
            flesh.SetPropertyBlock(properties);
            for (int i = 0; i < 2; i++)
            {
                float progress = p.StripProgress(i);
                if (Mathf.Abs(progress - lastProgress[i]) > .00001f)
                {
                    PepperGeometry.WriteVertices(vertices[i], i, progress);
                    movingSkin[i].vertices = vertices[i];
                    movingSkin[i].RecalculateNormals();
                    movingSkin[i].RecalculateBounds();
                    lastProgress[i] = progress;
                }
                properties.SetFloat("_Hover", p.Stage == PepperStage.Peelable && HoverStrip == i ? 1f : 0f);
                skinRenderers[i].SetPropertyBlock(properties);
                looseEdges[i].gameObject.SetActive(p.Stage == PepperStage.Peelable && progress < 1f);
                looseEdges[i].localPosition = new Vector3(i == 0 ? .07f : -.07f, PepperGeometry.Length * (1f - Mathf.Max(.08f, progress)), -.075f);
                if (releasedAt[i] >= 0f)
                {
                    float t = Time.time - releasedAt[i];
                    skin[i].transform.localPosition = new Vector3((i == 0 ? 1 : -1) * t * .25f, -t * .16f, .08f * Mathf.Sin(t * 6f));
                    skin[i].transform.localRotation = Quaternion.Euler(0f, 0f, (i == 0 ? -1 : 1) * t * 75f);
                    skinRenderers[i].enabled = t < .48f;
                }
            }
        }

        public int ChooseStrip(Vector3 worldPoint)
        {
            int preferred = transform.InverseTransformPoint(worldPoint).x >= 0f ? 0 : 1;
            return session.State.StripProgress(preferred) < 1f ? preferred : 1 - preferred;
        }

        public Vector2 ScreenPeelDirection(Camera camera)
        {
            Vector3 start = camera.WorldToScreenPoint(transform.TransformPoint(0f, .5f, 0f));
            Vector3 end = camera.WorldToScreenPoint(transform.TransformPoint(0f, .04f, 0f));
            Vector2 direction = end - start;
            return direction.sqrMagnitude < 25f ? Vector2.down : direction.normalized;
        }

        public void ReleaseStrip(int strip) { releasedAt[strip] = Time.time; }
        void OnDestroy() { foreach (Mesh mesh in movingSkin) if (mesh) Destroy(mesh); }
    }
}
