using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class PileRegion : MonoBehaviour
    {
        public string regionId;
        [Min(1)] public int initialUnits = 13;
        public Transform volume;
        public Vector3 fullScale;
        public GameObject[] surfaceClumps;
        public Renderer focusMarker;

        public void Render(int units)
        {
            float fill = Mathf.Clamp01((float)units / initialUnits);
            volume.gameObject.SetActive(units > 0);
            // Each region shrinks locally, keeping a visible final clump and its collision surface aligned.
            float footprint = Mathf.Lerp(.45f, 1, fill);
            volume.localScale = new Vector3(fullScale.x * footprint, fullScale.y * fill, fullScale.z * footprint);
            volume.localPosition = new Vector3(0, .025f + fullScale.y * fill * .5f, 0);
            for (int i = 0; i < surfaceClumps.Length; i++)
            {
                // Surface peppers retain readable size even when one unit remains in a shallow region.
                var clump = surfaceClumps[i].transform;
                if (clump.parent != transform) clump.SetParent(transform, false);
                float x = (i % 3 - 1) * .21f;
                float z = (i / 3 - 1) * .22f;
                float y = Mathf.Sqrt(Mathf.Max(0, .25f - x * x - z * z));
                clump.localPosition = new Vector3(x * fullScale.x * footprint, .025f + fullScale.y * fill * (.5f + y), z * fullScale.z * footprint);
                clump.localScale = new Vector3(.18f, .12f, .28f);
                clump.localRotation = Quaternion.Euler(0, i * 57, 15);
                surfaceClumps[i].SetActive(i < Mathf.CeilToInt(surfaceClumps.Length * fill));
            }
        }

        public void Highlight(bool active) => focusMarker.enabled = active;
    }
}
