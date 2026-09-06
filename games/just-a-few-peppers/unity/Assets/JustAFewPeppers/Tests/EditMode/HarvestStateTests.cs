using System;
using NUnit.Framework;

namespace JustAFewPeppers.Tests
{
    public class HarvestStateTests
    {
        static HarvestState NewState() => new HarvestState(new[] { "edge", "mound" }, new[] { 3, 22 }, 12, 2);
        static void Conserved(HarvestState state)
        {
            Assert.That(state.RawUnits + state.Remaining, Is.EqualTo(state.InitialHarvest));
            Assert.That(state.RawUnits, Is.InRange(0, state.Capacity));
            Assert.That(state.Remaining, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void FullAndPartialGatherOnlyMoveAcceptedUnitsFromTheTouchedRegion()
        {
            var state = NewState();
            Assert.That(state.Gather("edge", 2), Is.Zero, "Gather requires holding the crate.");
            Assert.That(state.PickUp(), Is.True);
            Assert.That(state.Gather("edge", 2), Is.EqualTo(2));
            Assert.That(state.UnitsIn("edge"), Is.EqualTo(1));
            Assert.That(state.UnitsIn("mound"), Is.EqualTo(22));
            Assert.That(state.Gather("edge", 2), Is.EqualTo(1), "Partial region remainder.");
            Assert.That(state.Gather("edge", 2), Is.Zero);
            Assert.That(state.Gather("mound", 8), Is.EqualTo(8));
            Assert.That(state.Gather("mound", int.MaxValue), Is.EqualTo(1), "Partial capacity acceptance.");
            Assert.That(state.CanGather("mound"), Is.EqualTo(GatherStatus.Full));
            Assert.That(state.Gather("mound", 3), Is.Zero);
            Conserved(state);
        }

        [Test]
        public void InvalidCommandsAndRepeatedOwnershipChangesDoNotDuplicateOrLoseFood()
        {
            var state = NewState();
            state.PickUp();
            Assert.That(state.PickUp(), Is.False);
            foreach (var id in new[] { "unknown", "", null }) Assert.That(state.Gather(id, 3), Is.Zero);
            Assert.That(state.Gather("mound", 0), Is.Zero);
            Assert.That(state.Gather("mound", -2), Is.Zero);
            state.Gather("mound", 7);
            Assert.That(state.Park(-1), Is.False);
            Assert.That(state.Park(2), Is.False);
            Assert.That(state.IsHeld, Is.True);
            for (int i = 0; i < 30; i++)
            {
                Assert.That(state.Park(1), Is.True);
                Assert.That(state.Park(0), Is.False);
                Assert.That(state.Gather("mound", 3), Is.Zero);
                state.RecoverCarrier();
                Assert.That(state.RestingPoint, Is.Zero);
                Assert.That(state.RawUnits, Is.EqualTo(7));
                Conserved(state);
                state.PickUp();
            }
        }

        [Test]
        public void RecoveryPreservesDepletionAndExplicitPrototypeResetRestoresAuthoredState()
        {
            var ids = new[] { "edge", "mound" };
            var amounts = new[] { 3, 22 };
            var state = new HarvestState(ids, amounts, 12, 2);
            ids[0] = "changed"; amounts[0] = 999;
            state.PickUp(); state.Gather("edge", 3); state.Gather("mound", 4);
            state.RecoverCarrier();
            Assert.That(state.UnitsIn("edge"), Is.Zero);
            Assert.That(state.UnitsIn("mound"), Is.EqualTo(18));
            Assert.That(state.RawUnits, Is.EqualTo(7));
            state.ResetPrototype(); state.ResetPrototype();
            Assert.That(state.UnitsIn("edge"), Is.EqualTo(3));
            Assert.That(state.UnitsIn("mound"), Is.EqualTo(22));
            Assert.That(state.RawUnits, Is.Zero);
            Assert.That(state.IsHeld, Is.False);
            Conserved(state);
        }

        [Test]
        public void DuplicateUnknownOrInvalidAuthoredConfigurationIsRejected()
        {
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "same", "same" }, new[] { 1, 2 }, 12, 2));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "" }, new[] { 1 }, 12, 2));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "edge" }, new[] { -1 }, 12, 2));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "edge" }, new[] { 1 }, 0, 2));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "edge" }, new[] { 1 }, 12, 0));
        }
    }
}
