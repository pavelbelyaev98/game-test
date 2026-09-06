using System;
using NUnit.Framework;

namespace JustAFewPeppers.Tests
{
    public class ProcessingStateTests
    {
        static HarvestState State(int total = 107) => new HarvestState(new[] { "mound" }, new[] { total }, 12, 2);
        static void Fill(HarvestState state, int amount = 12) { state.PickUp(); state.Gather("mound", amount); }

        static void Conserved(HarvestState state)
        {
            Assert.That(state.AccountedUnits, Is.EqualTo(state.InitialHarvest));
            Assert.That(state.RawUnits, Is.InRange(0, state.Capacity));
            Assert.That(state.QueuedUnits, Is.InRange(0, state.InputCapacity));
            Assert.That(state.ActiveUnits, Is.GreaterThanOrEqualTo(0));
            Assert.That(state.OutputUnits, Is.GreaterThanOrEqualTo(0));
            Assert.That(state.OutputUnits + state.ActiveUnits, Is.LessThanOrEqualTo(state.OutputCapacity));
            Assert.That(state.BatchRemaining, Is.InRange(0, state.BatchDuration));
        }

        [Test]
        public void FullOutputReservesBeforeWorkAndKeepsQueuedAndCarriedLoadsSafe()
        {
            var state = State();
            Assert.That(state.Tip(), Is.Zero);
            Fill(state);
            Assert.That(state.Tip(), Is.EqualTo(12));
            Assert.That(state.RawUnits, Is.Zero);
            Assert.That(state.ActiveUnits, Is.EqualTo(12));
            Assert.That(state.QueuedUnits, Is.Zero);
            Assert.That(state.Tip(), Is.Zero, "Repeating an empty dump cannot duplicate food.");
            Fill(state); Assert.That(state.Tip(), Is.EqualTo(12));
            Fill(state); Assert.That(state.Tip(), Is.Zero);
            Assert.That(state.CanTip(), Is.EqualTo(TipStatus.InputFull));
            Conserved(state);
            Assert.That(state.AdvanceProcessing(4), Is.EqualTo(12));
            Assert.That(state.OutputFull, Is.True);
            Assert.That(state.ActiveUnits, Is.Zero);
            Assert.That(state.QueuedUnits, Is.EqualTo(12));
            Assert.That(state.AdvanceProcessing(86400), Is.Zero, "Waiting neither spoils nor creates food.");
            state.RecoverCarrier();
            Assert.That(state.RawUnits, Is.EqualTo(12));
            Assert.That(state.Tip(), Is.Zero, "A recovered parked crate cannot tip remotely.");
            Conserved(state);
        }

        [Test]
        public void LimitedInputAcceptsOnlyRoomAndPartialOutputStartsOnlyWhatFits()
        {
            var state = State();
            Fill(state, 3); state.Tip();
            Fill(state, 9); state.Tip();
            Fill(state);
            Assert.That(state.Tip(), Is.EqualTo(3));
            Assert.That(state.RawUnits, Is.EqualTo(9));
            Assert.That(state.QueuedUnits, Is.EqualTo(12));
            Conserved(state);
            state.AdvanceProcessing(4);
            Assert.That(state.OutputUnits, Is.EqualTo(3));
            Assert.That(state.ActiveUnits, Is.EqualTo(9));
            Assert.That(state.QueuedUnits, Is.EqualTo(3));
            Assert.That(state.BatchRemaining, Is.EqualTo(4));
            Conserved(state);
            state.AdvanceProcessing(4);
            Assert.That(state.OutputUnits, Is.EqualTo(12));
            Assert.That(state.QueuedUnits, Is.EqualTo(3));
            Conserved(state);
        }

        [TestCase(1)]
        [TestCase(7)]
        [TestCase(11)]
        public void FinalPartialHarvestNeedsNoMinimumBatchOrFullJar(int total)
        {
            var state = State(total);
            Fill(state);
            Assert.That(state.Tip(), Is.EqualTo(total));
            Assert.That(state.Remaining + state.RawUnits + state.QueuedUnits, Is.Zero);
            state.AdvanceProcessing(3.5);
            Assert.That(state.OutputUnits, Is.Zero);
            state.AdvanceProcessing(.5);
            Assert.That(state.OutputUnits, Is.EqualTo(total));
            Assert.That(state.ActiveUnits, Is.Zero);
            Conserved(state);
        }

        [Test]
        public void LongAndSmallTicksAgreeAndInvalidTimeCannotCorruptState()
        {
            var a = State(); var b = State();
            foreach (var state in new[] { a, b }) { Fill(state, 2); state.Tip(); Fill(state, 7); state.Tip(); }
            a.AdvanceProcessing(20);
            for (int i = 0; i < 80; i++) b.AdvanceProcessing(.25);
            Assert.That(a.OutputUnits, Is.EqualTo(9).And.EqualTo(b.OutputUnits));
            foreach (var seconds in new[] { -1d, double.NaN, double.PositiveInfinity })
                Assert.Throws<ArgumentOutOfRangeException>(() => b.AdvanceProcessing(seconds));
            Assert.That(b.OutputUnits, Is.EqualTo(9));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "x" }, new[] { 1 }, 12, 2, 0));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "x" }, new[] { 1 }, 12, 2, 12, 0));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "x" }, new[] { 1 }, 12, 2, 12, 12, double.NaN));
            Conserved(a); Conserved(b);
        }

        [Test]
        public void MixedCommandsPreserveBoundsAndResetAllStationOwners()
        {
            var state = State();
            var random = new Random(103);
            for (int i = 0; i < 2000; i++)
            {
                switch (random.Next(7))
                {
                    case 0: state.PickUp(); break;
                    case 1: state.Gather("mound", random.Next(1, 16)); break;
                    case 2: state.Tip(); break;
                    case 3: state.AdvanceProcessing(random.NextDouble() * 6); break;
                    case 4: state.RecoverCarrier(); break;
                    case 5: state.Park(1); break;
                    case 6: if (i % 13 == 0) state.ResetPrototype(); break;
                }
                Conserved(state);
            }
            state.ResetPrototype();
            Assert.That(state.Remaining, Is.EqualTo(107));
            Assert.That(state.RawUnits + state.QueuedUnits + state.ActiveUnits + state.OutputUnits, Is.Zero);
            Assert.That(state.BatchRemaining, Is.Zero);
            Conserved(state);
        }
    }
}
