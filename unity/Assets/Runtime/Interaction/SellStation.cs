using System.Collections.Generic;

namespace SomethingDownThere
{
    public sealed class SellStation : StationTarget
    {
        private StationTrade.SaleOffer[] offers = new StationTrade.SaleOffer[0];
        public IReadOnlyList<InventoryItem> Items => offers.Length == 0 ? System.Array.Empty<InventoryItem>() : offers[0].Items;
        public long TotalValue => offers.Length == 0 ? 0 : offers[0].Value;
        public override string Title => "Sell finds";
        public override int CommandCount => offers.Length;
        public override string Description(FpsPlayer player) => $"Balance: ${player.Wallet.Balance}\nCarried value: ${TotalValue}";

        public override void RefreshOffers(FpsPlayer player)
        {
            offers = new StationTrade.SaleOffer[player.Inventory.Count + 1];
            offers[0] = player.Trade.OfferSale();
            for (int i = 1; i < offers.Length; i++) offers[i] = player.Trade.OfferSale(offers[0].Items[i - 1].InstanceId);
        }

        public override string CommandLabel(int index, FpsPlayer player) => index == 0
            ? $"Sell all  /  ${TotalValue}" : $"Sell {offers[index].Items[0].DisplayName}  /  ${offers[index].Value}";

        public override bool CanExecute(int index, FpsPlayer player) => isActiveAndEnabled && index >= 0 && index < offers.Length
            && player.Trade.Check(offers[index]) == TradeResult.Ready;

        public override bool TryExecute(int index, FpsPlayer player)
        {
            if (!player.CanUseStation(this) || !CanExecute(index, player)) return false;
            var offer = offers[index];
            if (!player.Trade.TrySell(offer)) return false;
            player.ShowStationFeedback(offer.Items.Count == 1
                ? $"Sold {offer.Items[0].DisplayName}  |  +${offer.Value}"
                : $"Sold {offer.Items.Count} finds  |  +${offer.Value}");
            GetComponent<StationMotion>()?.Play();
            return true;
        }
    }
}
