using UnityEngine;

namespace SomethingDownThere
{
    // Animate the approved model's flap/drawer. No runtime art or material creation.
    public sealed class StationMotion : MonoBehaviour
    {
        [SerializeField] private Transform movingPart;
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Vector3 translation;
        private Quaternion restRotation;
        private Vector3 restPosition;
        private float started;

        public void Configure(Transform part, Vector3 turn, Vector3 slide)
        { movingPart = part; rotation = turn; translation = slide; }

        private void Awake()
        {
            if (movingPart != null) { restRotation = movingPart.localRotation; restPosition = movingPart.localPosition; }
            enabled = false;
        }

        public void Play()
        {
            if (movingPart == null) return;
            started = Time.unscaledTime;
            enabled = true;
        }

        private void Update()
        {
            float t = Mathf.Clamp01((Time.unscaledTime - started) / 0.7f);
            float amount = Mathf.Sin(t * Mathf.PI);
            movingPart.localRotation = restRotation * Quaternion.Euler(rotation * amount);
            movingPart.localPosition = restPosition + translation * amount;
            if (t >= 1) enabled = false;
        }

        private void OnDisable()
        {
            if (movingPart == null) return;
            movingPart.localRotation = restRotation;
            movingPart.localPosition = restPosition;
        }
    }
}
