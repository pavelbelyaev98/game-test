using System;
using System.Collections.Generic;

namespace SomethingDownThere
{
    public sealed class RescueQuote
    {
        public IReadOnlyList<InventoryItem> LostItems { get; }
        public long LostSaleValue { get; }
        public decimal Balance { get; }
        public decimal Fee { get; }
        public decimal RemainingBalance => Balance - Fee;

        internal RescueQuote(SessionInventory inventory, SessionWallet wallet, int maximumFee)
        {
            var items = new InventoryItem[inventory.Count];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = inventory.Items[i];
                LostSaleValue += items[i].SaleValue;
            }
            LostItems = Array.AsReadOnly(items);
            Balance = wallet.Balance;
            Fee = Math.Min(maximumFee, Balance);
        }
    }

    // Main-thread session transaction; movement/menu ownership stays with FpsPlayer.
    public sealed class RescueController
    {
        private readonly SessionInventory inventory;
        private readonly SessionWallet wallet;
        private readonly int maximumFee;
        public RescueQuote Quote { get; private set; }

        public RescueController(SessionInventory inventory, SessionWallet wallet, int maximumFee = 10)
        {
            this.inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            this.wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
            if (maximumFee < 0) throw new ArgumentOutOfRangeException(nameof(maximumFee));
            this.maximumFee = maximumFee;
        }

        public void Prepare() => Quote = new RescueQuote(inventory, wallet, maximumFee);
        public void Cancel() => Quote = null;

        public bool TryConfirm(out RescueQuote receipt)
        {
            receipt = null;
            var quote = Quote;
            if (quote == null || wallet.Balance != quote.Balance || inventory.Count != quote.LostItems.Count) return false;
            for (int i = 0; i < inventory.Count; i++)
                if (!ReferenceEquals(inventory.Items[i], quote.LostItems[i])) return false;

            // Everything is validated before mutation. These session owners have no
            // callbacks, so a matching inventory cannot change during this transaction.
            if (!wallet.TrySpend(quote.Fee)) return false;
            Quote = null; // Consume the confirmation before any presentation callbacks.
            foreach (var item in quote.LostItems) inventory.TryRemove(item.InstanceId, out _);
            receipt = quote;
            return true;
        }
    }
}
