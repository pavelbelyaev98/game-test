using NUnit.Framework;

namespace SomethingDownThere.Tests
{
    public sealed class SessionStateTests
    {
        [Test]
        public void BatteryRejectsInvalidOrUnaffordableSpendingWithoutMutation()
        {
            var battery = new Battery(10);
            Assert.That(battery.TrySpend(4), Is.True);
            foreach (float amount in new[] { -1f, 7f, float.NaN, float.PositiveInfinity })
            {
                Assert.That(battery.TrySpend(amount), Is.False);
                Assert.That(battery.Charge, Is.EqualTo(6));
            }
            Assert.That(battery.TrySpend(6), Is.True);
            Assert.That(battery.Charge, Is.Zero);
            battery.Recharge();
            Assert.That(battery.Charge, Is.EqualTo(10));
        }

        [Test]
        public void FullInventoryRetainsExistingFindsAndAllowsCollectionAfterSale()
        {
            var inventory = new SessionInventory(2);
            Assert.That(inventory.TryAdd("Coin"), Is.True);
            Assert.That(inventory.TryAdd("Tin"), Is.True);
            Assert.That(inventory.TryAdd("Bone"), Is.False);
            Assert.That(inventory.Items, Is.EqualTo(new[] { "Coin", "Tin" }));
            Assert.That(inventory.TryRemoveAt(5), Is.False);
            Assert.That(inventory.TryRemoveAt(0), Is.True);
            Assert.That(inventory.TryAdd("Bone"), Is.True);
            Assert.That(inventory.Items, Is.EqualTo(new[] { "Tin", "Bone" }));
        }
    }
}
