using System;
using System.Collections.Generic;

namespace SomethingDownThere
{
    public enum TradeResult { Ready, Empty, Changed, CreditLimit, Unaffordable, Complete }

    // Quotes bind the visible offer to exact session state. Mutations are synchronous,
    // prevalidated and callback-free; UI notifications happen only after the commit.
    public sealed class StationTrade
    {
        public sealed class SaleOffer
        {
            internal readonly StationTrade Owner;
            internal readonly long InventoryRevision, WalletRevision;
            internal bool Used;
            public IReadOnlyList<InventoryItem> Items { get; }
            public long Value { get; }

            internal SaleOffer(StationTrade owner, string id)
            {
                Owner = owner;
                InventoryRevision = owner.inventory.Revision;
                WalletRevision = owner.wallet.Revision;
                var selected = new List<InventoryItem>();
                foreach (var item in owner.inventory.Items)
                    if (id == null || item.InstanceId == id) { selected.Add(item); Value += item.SaleValue; }
                Items = selected.AsReadOnly();
            }
        }

        public sealed class UpgradeOffer
        {
            internal readonly StationTrade Owner;
            internal readonly long WalletRevision;
            internal bool Used;
            public int OwnedLevel { get; }
            public int NextLevel => OwnedLevel + 1;
            public int Cost { get; }
            public bool Complete { get; }

            internal UpgradeOffer(StationTrade owner)
            {
                Owner = owner;
                WalletRevision = owner.wallet.Revision;
                OwnedLevel = owner.shovel.Level;
                Complete = OwnedLevel == owner.shovel.LevelCount;
                Cost = Complete ? 0 : owner.prices[OwnedLevel - 1];
            }
        }

        private readonly SessionInventory inventory;
        private readonly SessionWallet wallet;
        private readonly ShovelState shovel;
        private readonly int[] prices;
        public static int[] DefaultPrices() => new[] { 10, 25, 55, 100, 180 };

        public StationTrade(SessionInventory inventory, SessionWallet wallet, ShovelState shovel, int[] prices)
        {
            this.inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            this.wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
            this.shovel = shovel ?? throw new ArgumentNullException(nameof(shovel));
            if (prices == null || prices.Length != shovel.LevelCount - 1)
                throw new ArgumentException("Provide one price for each shovel upgrade.", nameof(prices));
            this.prices = (int[])prices.Clone();
            foreach (int price in this.prices)
                if (price <= 0) throw new ArgumentException("Upgrade prices must be positive.", nameof(prices));
        }

        public SaleOffer OfferSale(string instanceId = null) => new SaleOffer(this, instanceId);
        public UpgradeOffer OfferUpgrade() => new UpgradeOffer(this);

        public TradeResult Check(SaleOffer offer)
        {
            if (offer == null || offer.Owner != this || offer.Used
                || offer.InventoryRevision != inventory.Revision || offer.WalletRevision != wallet.Revision) return TradeResult.Changed;
            if (offer.Items.Count == 0) return TradeResult.Empty;
            return offer.Value > int.MaxValue - wallet.Balance ? TradeResult.CreditLimit : TradeResult.Ready;
        }

        public TradeResult Check(UpgradeOffer offer)
        {
            if (offer == null || offer.Owner != this || offer.Used || offer.WalletRevision != wallet.Revision
                || offer.OwnedLevel != shovel.Level) return TradeResult.Changed;
            if (offer.Complete) return TradeResult.Complete;
            return wallet.Balance < offer.Cost ? TradeResult.Unaffordable : TradeResult.Ready;
        }

        public bool TrySell(SaleOffer offer)
        {
            if (Check(offer) != TradeResult.Ready) return false;
            offer.Used = true;
            foreach (var item in offer.Items) inventory.TryRemove(item.InstanceId, out _);
            wallet.TryCredit((int)offer.Value);
            return true;
        }

        public bool TryUpgrade(UpgradeOffer offer)
        {
            if (Check(offer) != TradeResult.Ready) return false;
            offer.Used = true;
            wallet.TrySpend(offer.Cost);
            shovel.TryUpgradeTo(offer.NextLevel);
            return true;
        }
    }
}
