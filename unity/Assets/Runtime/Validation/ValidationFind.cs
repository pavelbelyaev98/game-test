using UnityEngine;

namespace SomethingDownThere
{
    public sealed class ValidationFind : MonoBehaviour, IInteractionTarget
    {
        [SerializeField] private string itemName = "Old tin";
        [Tooltip("Fixture eligibility only. A future exposure system will determine this state.")]
        [SerializeField] private bool exposed = true;
        private bool collected;
        public bool Exposed { get => exposed; set => exposed = value; }

        public string GetPrompt(FpsPlayer player)
        {
            if (collected || !isActiveAndEnabled) return "";
            if (!exposed) return "Expose more";
            return player.Inventory.IsFull ? "Inventory full" : "E Collect " + itemName;
        }

        public bool TryInteract(FpsPlayer player)
        {
            if (collected || !isActiveAndEnabled || !exposed) return false;
            if (!player.Inventory.TryAdd(itemName))
            {
                player.ShowFeedback("Inventory full - return to sell");
                return false;
            }
            collected = true;
            player.ShowFeedback(itemName + " collected (" + player.Inventory.Count + "/" + player.Inventory.Capacity + ")");
            gameObject.SetActive(false);
            return true;
        }
    }
}
