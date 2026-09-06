using UnityEngine;

namespace Chushkopek.Stage0
{
    // Placement/completion rules stay in PepperState; this component owns only the tray position.
    public sealed class FinishedSocket : MonoBehaviour
    {
        public Transform socket;
    }
}
