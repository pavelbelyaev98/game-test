namespace SomethingDownThere
{
    public sealed class UpgradeStation : StationTarget
    {
        private readonly StationTrade.UpgradeOffer[] offers = new StationTrade.UpgradeOffer[3];
        public const int RefillCommand = 3;
        public StationTrade.UpgradeOffer Offer => offers[0];
        public StationTrade.UpgradeOffer OfferAt(int index) => index >= 0 && index < offers.Length ? offers[index] : null;
        public StationTrade.RefillOffer Refill { get; private set; }
        public override string Title => "Workshop";
        public override int CommandCount => 4;
        public override string Description(FpsPlayer player) => $"${player.Wallet.Balance}";
        public override void RefreshOffers(FpsPlayer player)
        {
            for (int i = 0; i < offers.Length; i++) offers[i] = player.Trade.OfferUpgrade((EquipmentKind)i);
            Refill = player.Trade.OfferRefill();
        }
        public override string CommandLabel(int index, FpsPlayer player)
        {
            if (index == RefillCommand) return Refill == null || Refill.Full ? "Tank full"
                : Refill.Cost == 0 ? "Need $1" : $"Refill / ${Refill.Cost:0}";
            var offer = OfferAt(index);
            return offer == null || offer.Complete ? "Max level" : $"Upgrade / ${offer.Cost}";
        }
        public override bool CanExecute(int index, FpsPlayer player) => isActiveAndEnabled
            && (index == RefillCommand ? player.Trade.Check(Refill) : player.Trade.Check(OfferAt(index))) == TradeResult.Ready;

        public override bool TryExecute(int index, FpsPlayer player)
        {
            if (!player.CanUseStation(this) || !CanExecute(index, player)) return false;
            if (index == RefillCommand)
            {
                if (!player.Trade.TryRefill(Refill)) return false;
                player.ShowStationFeedback($"+{Refill.Amount:0.#} fuel  |  -${Refill.Cost:0}");
            }
            else
            {
                var offer = OfferAt(index);
                if (!player.Trade.TryUpgrade(offer)) return false;
                player.ShowStationFeedback($"{EquipmentProgression.Name(offer.Kind)} upgraded  |  -${offer.Cost}");
            }
            GetComponent<StationMotion>()?.Play();
            return true;
        }
    }
}
