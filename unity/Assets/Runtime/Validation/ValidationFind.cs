using UnityEngine;

namespace SomethingDownThere
{
    public sealed class ValidationFind : MonoBehaviour, IInteractionTarget
    {
        [SerializeField] private string itemName = "Old tin";
        [SerializeField, Min(0)] private int saleValue = 5;
        [Tooltip("Fixture eligibility only. A future exposure system will determine this state.")]
        [SerializeField] private bool exposed = true;
        private bool collected;
        public bool Exposed { get => exposed; set => exposed = value; }
        public InventoryItem Item { get; private set; }

        private void Awake()
        {
            // Fixtures have session-only identities; production finds will supply authored IDs.
            Item = new InventoryItem(System.Guid.NewGuid().ToString("N"), itemName, saleValue);
        }

        public string GetPrompt(FpsPlayer player)
        {
            if (collected || !isActiveAndEnabled) return "";
            if (!exposed) return "Expose more";
            return player.Inventory.IsFull ? "Inventory full" : "E Collect " + Item.DisplayName;
        }

        public bool TryInteract(FpsPlayer player)
        {
            if (collected || !isActiveAndEnabled || !exposed) return false;
            if (!player.Inventory.TryAdd(Item))
            {
                player.ShowFeedback(player.Inventory.IsFull ? "Inventory full - return to sell" : "Find already carried");
                return false;
            }
            collected = true;
            player.ShowFeedback(Item.DisplayName + " collected (" + player.Inventory.Count + "/" + player.Inventory.Capacity + ")");
            gameObject.SetActive(false);
            return true;
        }
    }
}
