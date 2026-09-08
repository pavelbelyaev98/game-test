namespace SomethingDownThere
{
    public sealed class UpgradeStation : StationTarget
    {
        public StationTrade.UpgradeOffer Offer { get; private set; }
        public override string Title => "Shovel upgrades";
        public override int CommandCount => 1;
        public override string Description(FpsPlayer player) => $"Balance: {player.Wallet.Balance} credits\nOwned shovel: {player.Shovel.Level} / {player.Shovel.LevelCount}";
        public override void RefreshOffers(FpsPlayer player) => Offer = player.Trade.OfferUpgrade();
        public override string CommandLabel(int index, FpsPlayer player) => Offer == null || Offer.Complete
            ? "Fully upgraded" : $"Buy shovel {Offer.NextLevel}  /  {Offer.Cost} credits";
        public override bool CanExecute(int index, FpsPlayer player) => index == 0 && isActiveAndEnabled
            && player.Trade.Check(Offer) == TradeResult.Ready;

        public override bool TryExecute(int index, FpsPlayer player)
        {
            if (!player.CanUseStation(this) || !CanExecute(index, player) || !player.Trade.TryUpgrade(Offer)) return false;
            player.ShowStationFeedback($"Shovel {player.Shovel.Level} purchased  |  -{Offer.Cost} credits");
            GetComponent<StationMotion>()?.Play();
            return true;
        }
    }
}
