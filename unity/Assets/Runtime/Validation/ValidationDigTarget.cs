using UnityEngine;

namespace SomethingDownThere
{
    // A disposable test block, not an excavation terrain implementation.
    public sealed class ValidationDigTarget : MonoBehaviour, IDigTarget
    {
        [SerializeField, Min(1)] private int hitsRemaining = 3;
        public bool CanDig => isActiveAndEnabled && hitsRemaining > 0;
        public string DigPrompt => "LMB Dig";
        public int HitsRemaining => hitsRemaining;

        public bool TryDig(RaycastHit hit)
        {
            if (!CanDig) return false;
            hitsRemaining--;
            if (hitsRemaining == 0) gameObject.SetActive(false);
            return true;
        }
    }
}
