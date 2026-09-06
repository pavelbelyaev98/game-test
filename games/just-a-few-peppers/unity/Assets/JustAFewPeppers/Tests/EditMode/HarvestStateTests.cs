using System;
using NUnit.Framework;

namespace JustAFewPeppers.Tests
{
    public class HarvestStateTests
    {
        static HarvestState NewState() => new HarvestState(new[] { "edge", "mound" }, new[] { 3, 22 }, 12, CarrierPose.Origin);
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
            Assert.That(state.Release(default, false), Is.False);
            Assert.That(state.Release(new CarrierPose(new UnityEngine.Vector3(float.NaN, 0, 0), UnityEngine.Quaternion.identity), true), Is.False);
            Assert.That(state.IsHeld, Is.True);
            for (int i = 0; i < 30; i++)
            {
                Assert.That(state.Release(CarrierPose.Origin, true), Is.True);
                Assert.That(state.Release(CarrierPose.Origin, false), Is.False);
                Assert.That(state.Gather("mound", 3), Is.Zero);
                state.RecoverCarrier();
                Assert.That(state.RawPose.Position, Is.EqualTo(state.SafeRawPose.Position));
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
            var state = new HarvestState(ids, amounts, 12, CarrierPose.Origin);
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
        public void FreeAndFallingPosesRetainLastSafePoseAndRejectMalformedDataWithoutChangingFood()
        {
            var state = NewState();
            var placed = new CarrierPose(new UnityEngine.Vector3(2.4f, .82f, -3.2f), UnityEngine.Quaternion.Euler(0, 37, 0));
            state.PickUp(); state.Gather("mound", 8);
            Assert.That(state.Release(placed, true), Is.True);
            state.PickUp();
            var falling = new CarrierPose(new UnityEngine.Vector3(1, 2, -1), UnityEngine.Quaternion.Euler(74, 15, 32));
            Assert.That(state.Release(falling, false), Is.True);
            Assert.That(state.RawPose.Position, Is.EqualTo(falling.Position));
            Assert.That(state.SafeRawPose.Position, Is.EqualTo(placed.Position));
            Assert.That(state.RecordCarrierPose(new CarrierPose(new UnityEngine.Vector3(float.PositiveInfinity, 0, 0), UnityEngine.Quaternion.identity), true), Is.False);
            Assert.That(state.RecoverCarrier(default), Is.False);
            state.RecoverCarrier();
            Assert.That(state.RawPose.Position, Is.EqualTo(placed.Position));
            Assert.That(state.RawPose.Rotation, Is.EqualTo(placed.Rotation));
            Assert.That(state.CarrierId, Is.EqualTo("raw-crate"));
            Assert.That(state.RawUnits, Is.EqualTo(8));
            Conserved(state);
        }

        [Test]
        public void DuplicateUnknownOrInvalidAuthoredConfigurationIsRejected()
        {
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "same", "same" }, new[] { 1, 2 }, 12, CarrierPose.Origin));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "" }, new[] { 1 }, 12, CarrierPose.Origin));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "edge" }, new[] { -1 }, 12, CarrierPose.Origin));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "edge" }, new[] { 1 }, 0, CarrierPose.Origin));
            Assert.Throws<ArgumentException>(() => new HarvestState(new[] { "edge" }, new[] { 1 }, 12, default));
        }
    }
}
