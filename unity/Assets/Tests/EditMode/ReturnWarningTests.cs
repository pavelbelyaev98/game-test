using NUnit.Framework;

namespace SomethingDownThere.Tests
{
    public sealed class ReturnWarningTests
    {
        [TestCase(100f)]
        [TestCase(200f)]
        public void ChargeBandsCrossAtConfiguredFractionsAndRecoverAfterRecharge(float capacity)
        {
            var warning = new ReturnWarning();
            var battery = new Battery(capacity);
            Assert.That(warning.Evaluate(battery), Is.EqualTo(ReturnRisk.Safe));
            battery.TrySpend(capacity * 0.64f);
            Assert.That(warning.Evaluate(battery), Is.EqualTo(ReturnRisk.Safe));
            battery.TrySpend(capacity * 0.01f);
            Assert.That(warning.Evaluate(battery), Is.EqualTo(ReturnRisk.Risky));
            battery.TrySpend(capacity * 0.2f);
            Assert.That(warning.Evaluate(battery), Is.EqualTo(ReturnRisk.Critical));
            battery.TrySpend(battery.Charge);
            Assert.That(warning.Evaluate(battery), Is.EqualTo(ReturnRisk.Critical));
            Assert.That(battery.Charge, Is.Zero, "Inspection never replenishes power.");
            battery.Recharge();
            Assert.That(warning.Evaluate(battery), Is.EqualTo(ReturnRisk.Safe));
        }

        [Test]
        public void TuningChangesBandsWithoutChangingBatteryOrPredictingRouteCost()
        {
            var battery = new Battery(100);
            battery.TrySpend(50);
            Assert.That(new ReturnWarning().Evaluate(battery), Is.EqualTo(ReturnRisk.Safe));
            Assert.That(new ReturnWarning(0.6f, 0.3f).Evaluate(battery), Is.EqualTo(ReturnRisk.Risky));
            Assert.That(new ReturnWarning(0.8f, 0.6f).Evaluate(battery), Is.EqualTo(ReturnRisk.Critical));
            Assert.That(battery.Charge, Is.EqualTo(50));
        }
    }
}
