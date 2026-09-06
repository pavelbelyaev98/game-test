using NUnit.Framework;
using UnityEngine;

namespace JustAFewPeppers.Tests
{
    public class FinishedFoodStateTests
    {
        static HarvestState New(int total = 107, int capacity = 12) => new HarvestState(new[] { "pile" }, new[] { total },
            12, CarrierPose.Origin, finishedCapacity: capacity, directOperation: false);
        static readonly CarrierPose Placed = new CarrierPose(new Vector3(2, 0, -3), Quaternion.Euler(0, 35, 0));
        static void Fill(HarvestState state, int amount) { state.PickUp(); state.Gather("pile", amount); state.Tip(); }
        static void Check(HarvestState state)
        {
            Assert.That(state.AccountedUnits, Is.EqualTo(state.InitialHarvest));
            Assert.That(state.IsHeld && state.FinishedHeld, Is.False);
            Assert.That(state.FinishedUnits, Is.InRange(0, state.FinishedCapacity));
            Assert.That(state.OutputUnits + state.ActiveUnits, Is.LessThanOrEqualTo(state.OutputCapacity));
            Assert.That(state.StoredUnits, Is.InRange(0, state.InitialHarvest));
            if (state.FinishedDocked) Assert.That(state.FinishedUnits, Is.Zero);
        }

        [TestCase(1)]
        [TestCase(25)]
        [TestCase(107)]
        public void WholeHarvestIncludingPartialLastLoadReusesOneCarrierAndDepositsOnce(int total)
        {
            var state = New(total);
            Assert.That(state.DepositFinished(), Is.Zero);
            while (state.Remaining > 0)
            {
                Fill(state, 12); state.AdvanceProcessing(4);
                int ready = state.OutputUnits;
                Assert.That(state.CollectOutput(Placed), Is.EqualTo(ready));
                Assert.That(state.CarrierId, Is.EqualTo("raw-crate"));
                Assert.That(state.FinishedCarrierId, Is.EqualTo("finished-carrier"));
                Assert.That(state.CollectOutput(Placed), Is.Zero);
                state.ReleaseFinished(Placed, false);
                Assert.That(state.DepositFinished(), Is.Zero, "Set-down or contact does not deposit.");
                state.RecoverFinished(Placed);
                Assert.That(state.FinishedUnits, Is.EqualTo(ready));
                state.PickUpFinished();
                Assert.That(state.DepositFinished(), Is.EqualTo(ready));
                Assert.That(state.DepositFinished(), Is.Zero);
                Assert.That(state.FinishedDocked, Is.True);
                Check(state);
            }
            Assert.That(state.StoredUnits, Is.EqualTo(total));
            Assert.That(state.RawUnits + state.OutputUnits + state.ActiveUnits + state.QueuedUnits + state.FinishedUnits, Is.Zero);
        }

        [Test]
        public void PickupPreservesActiveReservationAndOutputCanAccumulateWhileCarrierIsParked()
        {
            var state = New();
            Fill(state, 3); Fill(state, 12); state.AdvanceProcessing(4);
            Assert.That(state.OutputUnits, Is.EqualTo(3));
            Assert.That(state.ActiveUnits, Is.EqualTo(9));
            double time = state.BatchRemaining;
            Assert.That(state.CollectOutput(Placed), Is.EqualTo(3));
            Assert.That(state.ActiveUnits, Is.EqualTo(9));
            Assert.That(state.BatchRemaining, Is.EqualTo(time));
            state.ReleaseFinished(Placed, true);
            state.AdvanceProcessing(8);
            Assert.That(state.OutputUnits, Is.EqualTo(12));
            Assert.That(state.FinishedUnits, Is.EqualTo(3));
            Assert.That(state.CollectOutput(), Is.Zero, "An away carrier cannot become a second container.");
            state.PickUpFinished(); state.DepositFinished();
            Assert.That(state.CollectOutput(), Is.EqualTo(12));
            Assert.That(state.DepositFinished(), Is.EqualTo(12));
            Assert.That(state.StoredUnits, Is.EqualTo(15));
            Check(state);
        }

        [Test]
        public void InvalidSwitchesKeepBothOwnersAndSmallCarrierLeavesOutputAtStation()
        {
            var state = New(capacity: 5);
            Fill(state, 12); state.AdvanceProcessing(4);
            Assert.That(state.CollectOutput(), Is.Zero, "Raw hand needs a validated set-down pose first.");
            Assert.That(state.CollectOutput(default(CarrierPose)), Is.Zero);
            Assert.That(state.IsHeld, Is.True);
            Assert.That(state.OutputUnits, Is.EqualTo(12));
            Assert.That(state.CollectOutput(Placed), Is.EqualTo(5));
            Assert.That(state.OutputUnits, Is.EqualTo(7));
            Assert.That(state.PickUp(), Is.False);
            Assert.That(state.PickUp(default(CarrierPose)), Is.False);
            Assert.That(state.FinishedHeld, Is.True);
            Assert.That(state.PickUp(Placed), Is.True);
            Assert.That(state.FinishedPose.Position, Is.EqualTo(Placed.Position));
            Assert.That(state.PickUpFinished(), Is.False);
            Assert.That(state.PickUpFinished(Placed), Is.True);
            Check(state);
            state.ResetPrototype();
            Assert.That(state.Remaining, Is.EqualTo(107));
            Assert.That(state.StoredUnits + state.FinishedUnits + state.OutputUnits, Is.Zero);
            Assert.That(state.FinishedDocked, Is.True);
        }

        [Test]
        public void MixedCommandsConserveFoodAndStoredProgressNeverReplays()
        {
            var state = New();
            var random = new System.Random(104);
            int stored = 0;
            for (int i = 0; i < 4000; i++)
            {
                switch (random.Next(11))
                {
                    case 0: state.PickUp(Placed); break;
                    case 1: state.Gather("pile", random.Next(1, 20)); break;
                    case 2: state.Tip(); break;
                    case 3: state.AdvanceProcessing(random.NextDouble() * 8); break;
                    case 4: state.CollectOutput(Placed); break;
                    case 5: state.ReleaseFinished(Placed, i % 2 == 0); break;
                    case 6: state.PickUpFinished(Placed); break;
                    case 7: state.DepositFinished(); break;
                    case 8: state.RecoverCarrier(Placed); state.RecoverFinished(Placed); break;
                    case 9: state.RecordFinishedPose(default, true); break;
                    case 10: state.Release(Placed, false); break;
                }
                Check(state);
                Assert.That(state.StoredUnits, Is.GreaterThanOrEqualTo(stored));
                stored = state.StoredUnits;
            }
            Assert.That(stored, Is.EqualTo(107));
        }
    }
}
