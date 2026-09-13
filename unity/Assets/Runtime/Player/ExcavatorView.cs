using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SomethingDownThere
{
    [DisallowMultipleComponent]
    public sealed class ExcavatorView : MonoBehaviour
    {
        public const string ExperimentResource = "ExperimentalExcavator";
        public const int ToolLayer = 30;
        [SerializeField] private Transform model, rotor, jawLeft, jawRight, coils, powerPack, brace;
        [SerializeField] private Camera toolCamera;
        private FpsPlayer player;
        private Camera ownerCamera;
        private int originalCameraMask;
        private Vector3 restPosition;
        private Quaternion leftRest, rightRest, rotorRest;
        private int strokes, level;
        private ExcavationMode mode;
        private float kick, spin, speed, spread;

        public void Configure(Transform authored, Transform cutter, Transform left, Transform right,
            Transform coil, Transform pack, Transform reinforcement, Camera overlay)
        {
            model = authored; rotor = cutter; jawLeft = left; jawRight = right;
            coils = coil; powerPack = pack; brace = reinforcement;
            toolCamera = overlay;
        }

        public static ExcavatorView AttachExperiment(FpsPlayer owner)
        {
            if (owner == null || !owner.AdminAvailable) return null;
            var prefab = Resources.Load<GameObject>(ExperimentResource);
            if (prefab == null) return null;
            var instance = Instantiate(prefab, owner.ViewCamera.transform).GetComponent<ExcavatorView>();
            instance.ownerCamera = owner.ViewCamera;
            instance.originalCameraMask = owner.ViewCamera.cullingMask;
            owner.ViewCamera.cullingMask &= ~(1 << ToolLayer);
            owner.ViewCamera.GetUniversalAdditionalCameraData().cameraStack.Add(instance.toolCamera);
            return instance;
        }

        public void ReleaseExperiment()
        {
            Detach();
            if (Application.isPlaying) Destroy(gameObject); else DestroyImmediate(gameObject);
        }

        private void Detach()
        {
            if (toolCamera != null) toolCamera.enabled = false;
            if (model != null) model.gameObject.SetActive(false);
            if (ownerCamera == null) return;
            ownerCamera.GetUniversalAdditionalCameraData().cameraStack.Remove(toolCamera);
            ownerCamera.cullingMask = originalCameraMask;
            ownerCamera = null;
        }

        private void OnDestroy() => Detach();

        private void Awake()
        {
            player = GetComponentInParent<FpsPlayer>();
            if (player == null || model == null) { enabled = false; return; }
            restPosition = model.localPosition;
            leftRest = jawLeft.localRotation; rightRest = jawRight.localRotation; rotorRest = rotor.localRotation;
            strokes = player.SuccessfulStrokes; mode = player.DigMode;
            model.gameObject.SetActive(false); toolCamera.enabled = false;
        }

        private void LateUpdate()
        {
            if (player == null || player.Shovel == null) return;
            bool visible = player.ExperimentalExcavation && player.GameplayActive && player.HeldFind == null;
            model.gameObject.SetActive(visible);
            toolCamera.enabled = visible; toolCamera.fieldOfView = player.ViewCamera.fieldOfView;
            if (!visible)
            {
                strokes = player.SuccessfulStrokes; mode = player.DigMode;
                kick = speed = 0; return;
            }
            if (level != player.EffectiveShovelLevel)
            {
                level = player.EffectiveShovelLevel;
                coils.gameObject.SetActive(level >= 2); powerPack.gameObject.SetActive(level >= 4); brace.gameObject.SetActive(level >= 6);
            }
            mode = player.DigMode;
            if (strokes != player.SuccessfulStrokes)
            {
                strokes = player.SuccessfulStrokes;
                kick = mode == ExcavationMode.Shave ? .3f : 1f;
                speed = mode == ExcavationMode.Bore ? 760f : mode == ExcavationMode.Shave ? 520f : 230f;
            }
            float dt = Time.deltaTime;
            kick = Mathf.MoveTowards(kick, 0, dt * 5);
            speed = Mathf.MoveTowards(speed, 0, dt * 660);
            spin = (spin + speed * dt) % 360;
            // Steady mode keeps the body planted. Local mechanism motion confirms
            // contact; the camera is never shaken or displaced.
            float motion = player.CameraSettings.SteadyCrosshair ? 0 : kick;
            model.localPosition = restPosition + new Vector3(0, -.018f * motion, -.035f * motion);
            rotor.localRotation = rotorRest * Quaternion.AngleAxis(spin, Vector3.up);
            spread = Mathf.MoveTowards(spread, mode == ExcavationMode.Fan ? 24 : mode == ExcavationMode.Bore ? -12 : 0, dt * 140);
            jawLeft.localRotation = leftRest * Quaternion.AngleAxis(-spread, Vector3.forward);
            jawRight.localRotation = rightRest * Quaternion.AngleAxis(spread, Vector3.forward);
        }

        private void OnDisable() { if (toolCamera != null) toolCamera.enabled = false; }
    }
}
