using UnityEngine;

namespace SomethingDownThere
{
    public sealed class PermanentTerrainBoundary : MonoBehaviour, IDigTarget
    {
        public bool CanDig => false;
        public string DigPrompt => "Permanent boundary - cannot dig";
        public bool TryDig(RaycastHit hit) => false;
    }
}
