using UnityEngine;

namespace SomethingDownThere
{
    [RequireComponent(typeof(Collider))]
    public sealed class ValidationRecharge : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            var player = other.GetComponentInParent<FpsPlayer>();
            if (player != null && !player.IsMenuOpen) player.Battery.Recharge();
        }
    }
}
