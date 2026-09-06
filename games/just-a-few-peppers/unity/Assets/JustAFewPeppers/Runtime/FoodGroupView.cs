using UnityEngine;

namespace JustAFewPeppers
{
    // Bounded, non-collectable representation. Exact units belong exclusively to HarvestState.
    public sealed class FoodGroupView : MonoBehaviour
    {
        public GameObject[] jars;
        public Transform[] food;
        public int unitsPerJar = 3;

        public void Render(int units)
        {
            for (int i = 0; i < jars.Length; i++)
            {
                int amount = Mathf.Clamp(units - i * unitsPerJar, 0, unitsPerJar);
                jars[i].SetActive(amount > 0);
                var scale = food[i].localScale;
                scale.y = .12f * amount / unitsPerJar;
                food[i].localScale = scale;
                food[i].localPosition = new Vector3(0, .02f + scale.y, 0);
            }
        }
    }
}
