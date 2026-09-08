using NUnit.Framework;

namespace SomethingDownThere.Tests
{
    public sealed class StationTradeTests
    {
        private SessionInventory bag;
        private SessionWallet wallet;
        private ShovelState shovel;
        private StationTrade trade;

        [SetUp]
        public void SetUp()
        {
            bag = new SessionInventory();
            wallet = new SessionWallet();
            shovel = new ShovelState(ShovelProfile.Defaults());
            trade = new StationTrade(bag, wallet, shovel, StationTrade.DefaultPrices());
            bag.TryAdd(new InventoryItem("first", "Coin", 5));
            bag.TryAdd(new InventoryItem("second", "Coin", 17));
        }

        [Test]
        public void SellOneUsesSelectedIdentityAndEveryOfferCommitsAtMostOnce()
        {
            var first = bag.Items[0];
            var one = trade.OfferSale("second");
            var all = trade.OfferSale();
            Assert.That(wallet.Balance, Is.Zero);
            Assert.That(bag.Count, Is.EqualTo(2));
            Assert.That(trade.TrySell(one), Is.True);
            Assert.That(bag.Items, Is.EqualTo(new[] { first }));
            Assert.That(wallet.Balance, Is.EqualTo(17));
            Assert.That(trade.TrySell(one), Is.False);
            Assert.That(trade.TrySell(all), Is.False, "An earlier Sell All cannot silently change its contents.");
            Assert.That(trade.TrySell(trade.OfferSale()), Is.True);
            Assert.That(wallet.Balance, Is.EqualTo(22));
            Assert.That(bag.Count, Is.Zero);
            Assert.That(trade.Check(trade.OfferSale()), Is.EqualTo(TradeResult.Empty));
        }

        [TestCase("replacement")]
        [TestCase("bag restored")]
        [TestCase("wallet restored")]
        public void MutationsInvalidateDisplayedSalesEvenWhenTheSameValuesReturn(string change)
        {
            var quote = trade.OfferSale();
            if (change == "wallet restored") { wallet.TryCredit(1); wallet.TrySpend(1); }
            else
            {
                bag.TryRemove("first", out var removed);
                bag.TryAdd(change == "replacement" ? new InventoryItem("first", "Coin", 99) : removed);
            }
            int count = bag.Count, balance = wallet.Balance;
            Assert.That(trade.Check(quote), Is.EqualTo(TradeResult.Changed));
            Assert.That(trade.TrySell(quote), Is.False);
            Assert.That(bag.Count, Is.EqualTo(count));
            Assert.That(wallet.Balance, Is.EqualTo(balance));
        }

        [Test]
        public void CreditOverflowRejectsTheEntireSaleAndMissingOrForeignOffersDoNothing()
        {
            wallet.TryCredit(int.MaxValue - 10);
            var quote = trade.OfferSale();
            Assert.That(trade.Check(quote), Is.EqualTo(TradeResult.CreditLimit));
            Assert.That(trade.TrySell(quote), Is.False);
            Assert.That(bag.Count, Is.EqualTo(2));
            Assert.That(wallet.Balance, Is.EqualTo(int.MaxValue - 10));
            Assert.That(trade.TrySell(trade.OfferSale("missing")), Is.False);
            var other = new StationTrade(bag, wallet, shovel, StationTrade.DefaultPrices());
            Assert.That(other.TrySell(trade.OfferSale("first")), Is.False);
            Assert.That(other.TryUpgrade(trade.OfferUpgrade()), Is.False);
        }

        [Test]
        public void UpgradeCostsAreSequentialAffordableAndQuotedWithoutMutation()
        {
            var first = trade.OfferUpgrade();
            Assert.That(first.Cost, Is.EqualTo(10));
            Assert.That(first.NextLevel, Is.EqualTo(2));
            Assert.That(trade.Check(first), Is.EqualTo(TradeResult.Unaffordable));
            Assert.That(trade.TryUpgrade(first), Is.False);
            Assert.That(shovel.Level, Is.EqualTo(1));
            wallet.TryCredit(370);
            Assert.That(trade.TryUpgrade(first), Is.False, "A changed balance needs a fresh displayed offer.");
            int[] prices = StationTrade.DefaultPrices();
            for (int i = 0; i < prices.Length; i++)
            {
                int before = wallet.Balance;
                var quote = trade.OfferUpgrade();
                Assert.That(quote.Cost, Is.EqualTo(prices[i]));
                Assert.That(trade.TryUpgrade(quote), Is.True);
                Assert.That(shovel.Level, Is.EqualTo(i + 2));
                Assert.That(wallet.Balance, Is.EqualTo(before - prices[i]));
                Assert.That(trade.TryUpgrade(quote), Is.False);
            }
            Assert.That(wallet.Balance, Is.Zero);
            Assert.That(trade.Check(trade.OfferUpgrade()), Is.EqualTo(TradeResult.Complete));
            Assert.That(trade.TryUpgrade(trade.OfferUpgrade()), Is.False);
        }

        [Test]
        public void ExternalProgressionAndPriceArrayChangesCannotRewriteAnOffer()
        {
            var prices = StationTrade.DefaultPrices();
            var shop = new StationTrade(bag, wallet, shovel, prices);
            wallet.TryCredit(50);
            var quote = shop.OfferUpgrade();
            prices[0] = 1;
            Assert.That(quote.Cost, Is.EqualTo(10));
            shovel.TryUpgradeTo(2);
            Assert.That(shop.TryUpgrade(quote), Is.False);
            Assert.That(wallet.Balance, Is.EqualTo(50));
            Assert.That(shovel.Level, Is.EqualTo(2));
        }
    }
}
