using System;
using System.Linq;
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
        public void SameNameFindsRetainDistinctIdentitiesAndValues()
        {
            var inventory = new SessionInventory();
            var first = new InventoryItem("coin-01", "Coin", 5);
            var second = new InventoryItem("coin-02", "Coin", 17);
            Assert.That(inventory.TryAdd(first), Is.True);
            Assert.That(inventory.TryAdd(second), Is.True);
            Assert.That(inventory.Items, Is.EqualTo(new[] { first, second }));
            Assert.That(inventory.Items.Select(item => item.InstanceId), Is.EqualTo(new[] { "coin-01", "coin-02" }));
            Assert.That(inventory.Items.Select(item => item.DisplayName), Is.EqualTo(new[] { "Coin", "Coin" }));
            Assert.That(inventory.Items.Select(item => item.SaleValue), Is.EqualTo(new[] { 5, 17 }));
        }

        [Test]
        public void DuplicateIdAndNullAreRejectedWithoutReplacingACarriedRecord()
        {
            var inventory = new SessionInventory();
            var item = new InventoryItem("coin-01", "Coin", 5);
            inventory.TryAdd(item);
            Assert.That(inventory.TryAdd(item), Is.False);
            Assert.That(inventory.TryAdd(new InventoryItem("coin-01", "Changed name", 999)), Is.False);
            Assert.That(inventory.TryAdd(null), Is.False);
            Assert.That(inventory.Items, Is.EqualTo(new[] { item }));
            Assert.That(inventory.Items[0].SaleValue, Is.EqualTo(5));
        }

        [Test]
        public void DefaultCapacityRejectsAnEleventhRecordAndAcceptsItAfterRemoval()
        {
            var inventory = new SessionInventory();
            Assert.That(inventory.Capacity, Is.EqualTo(10));
            var carried = Enumerable.Range(0, 10).Select(i => new InventoryItem("tin-" + i, "Tin", i)).ToArray();
            foreach (var item in carried) Assert.That(inventory.TryAdd(item), Is.True);
            var extra = new InventoryItem("tin-extra", "Tin", 25);
            Assert.That(inventory.IsFull, Is.True);
            Assert.That(inventory.TryAdd(extra), Is.False);
            Assert.That(inventory.Items, Is.EqualTo(carried));
            Assert.That(inventory.TryRemove("tin-4", out var removed), Is.True);
            Assert.That(removed, Is.SameAs(carried[4]));
            Assert.That(inventory.IsFull, Is.False);
            Assert.That(inventory.TryAdd(extra), Is.True);
            Assert.That(inventory.Items, Is.EqualTo(carried.Where(item => item != removed).Concat(new[] { extra })));
        }

        [Test]
        public void InspectionAndRemovalPreserveRecordsWhenSlotIndicesShift()
        {
            var inventory = new SessionInventory(3);
            var first = new InventoryItem("tin-01", "Tin", 0);
            var second = new InventoryItem("tin-02", "Tin", 8);
            var third = new InventoryItem("tin-03", "Tin", 12);
            foreach (var item in new[] { first, second, third }) inventory.TryAdd(item);
            var inspection = inventory.Items;
            Assert.That(inspection, Is.Not.InstanceOf<System.Collections.Generic.List<InventoryItem>>());
            foreach (string missing in new[] { null, "", "unknown" })
            {
                Assert.That(inventory.TryRemove(missing, out var rejected), Is.False);
                Assert.That(rejected, Is.Null);
                Assert.That(inspection, Is.EqualTo(new[] { first, second, third }));
            }
            Assert.That(inventory.TryRemove(first.InstanceId, out var removed), Is.True);
            Assert.That(removed, Is.SameAs(first));
            Assert.That(inventory.TryRemove(third.InstanceId, out removed), Is.True);
            Assert.That(removed, Is.SameAs(third));
            Assert.That(inventory.TryRemove(third.InstanceId, out removed), Is.False);
            Assert.That(removed, Is.Null);
            Assert.That(inspection, Is.EqualTo(new[] { second }));
            Assert.That(inventory.Count, Is.EqualTo(1));
            Assert.That(second.InstanceId, Is.EqualTo("tin-02"));
            Assert.That(second.SaleValue, Is.EqualTo(8));
        }

        [TestCase(null, "Tin", 5)]
        [TestCase("", "Tin", 5)]
        [TestCase(" ", "Tin", 5)]
        [TestCase("tin-01", null, 5)]
        [TestCase("tin-01", "", 5)]
        [TestCase("tin-01", " ", 5)]
        [TestCase("tin-01", "Tin", -1)]
        public void InvalidItemDataCannotEnterTheInventory(string id, string name, int value)
        {
            Assert.That(() => new InventoryItem(id, name, value), Throws.InstanceOf<ArgumentException>());
        }
    }
}
