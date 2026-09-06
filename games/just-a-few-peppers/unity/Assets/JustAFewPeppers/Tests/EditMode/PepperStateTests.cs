using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace JustAFewPeppers.Tests
{
    public class PepperStateTests
    {
        static HarvestState Create(int count = 25)
        {
            var s = new HarvestState(new[] { "pile" }, new[] { count }, 12, CarrierPose.Origin);
            s.RegisterPeppers(Enumerable.Repeat("pile", count).ToArray(), Enumerable.Repeat(CarrierPose.Origin, count).ToArray());
            return s;
        }

        static void Check(HarvestState s)
        {
            Assert.That(s.AccountedUnits, Is.EqualTo(s.InitialHarvest));
            Assert.That(s.Peppers.Select(p => p.Id).Distinct().Count(), Is.EqualTo(s.InitialHarvest));
            Assert.That(s.Peppers.Count(p => p.Owner == PepperOwner.Carrier), Is.EqualTo(s.RawUnits));
            Assert.That(s.Peppers.Count(p => p.Owner == PepperOwner.Source), Is.EqualTo(s.Remaining));
        }

        [Test]
        public void ExactlyOneContentsPickupDuplicatePreviewAndCapacityKeepOneOwner()
        {
            var s = Create(); s.PickUp();
            Assert.That(s.GatherPeppers(new[] { "pepper-000", "pepper-000", "unknown", "pepper-001" }), Is.EqualTo(2));
            s.Gather("pile", 9);
            Assert.That(s.GatherPeppers(new[] { "pepper-011", "pepper-012", "pepper-013" }), Is.EqualTo(1));
            Assert.That(s.PickPepper("pepper-000"), Is.False);
            s.Release(CarrierPose.Origin, true);
            Assert.That(s.PickPepper("pepper-000"), Is.True);
            Assert.That(s.PickPepper("pepper-001"), Is.False);
            Assert.That(s.RawUnits, Is.EqualTo(11));
            Assert.That(s.ReleasePepper("pepper-000", default), Is.False);
            Assert.That(s.PutPepperInCarrier("pepper-000"), Is.True);
            Assert.That(s.PutPepperInCarrier("pepper-000"), Is.False);
            Check(s);
        }

        [Test]
        public void InterruptedPartialPourAndRepeatedContactsRecoverTheSameFood()
        {
            var s = Create(); s.PickUp(); s.Gather("pile", 12);
            Assert.That(s.BeginPepperPour(true), Is.EqualTo(12));
            Assert.That(s.BeginPepperPour(true), Is.Zero);
            foreach (var p in s.Peppers.Take(5))
            {
                Assert.That(s.AcceptPepperAtIntake(p.Id), Is.True);
                Assert.That(s.AcceptPepperAtIntake(p.Id), Is.False);
            }
            Assert.That(s.AdvanceProcessing(20), Is.Zero, "An open pour assembles one batch.");
            s.EndPepperPour(); Assert.That(s.ActiveUnits, Is.EqualTo(5));
            Assert.That(s.UncontainedUnits, Is.EqualTo(7));
            foreach (var p in s.Peppers) { s.RecoverPepper(p.Id); s.RecoverPepper(p.Id); }
            Assert.That(s.Remaining, Is.EqualTo(20)); Check(s);
            s.AdvanceProcessing(4); s.Release(CarrierPose.Origin, true); s.CollectOutput(); s.DepositFinished();
            Assert.That(s.StoredUnits, Is.EqualTo(5)); Check(s);
        }

        [TestCase(1)] [TestCase(25)] [TestCase(107)]
        public void RepeatedPhysicalLoadsIncludeTheFinalPartialBatch(int count)
        {
            var s = Create(count);
            while (s.Remaining > 0)
            {
                s.PickUp(); s.Gather("pile", 12); int load = s.RawUnits;
                Assert.That(s.BeginPepperPour(true), Is.EqualTo(load));
                foreach (var p in s.Peppers) if (p.Owner == PepperOwner.Transit) s.AcceptPepperAtIntake(p.Id);
                s.EndPepperPour(); s.AdvanceProcessing(4); s.Release(CarrierPose.Origin, true);
                Assert.That(s.CollectOutput(), Is.EqualTo(load)); s.DepositFinished(); Check(s);
            }
            Assert.That(s.StoredUnits, Is.EqualTo(count));
            Assert.That(s.Peppers.All(p => p.Owner == PepperOwner.Processed), Is.True);
            s.ResetPrototype(); Check(s); Assert.That(s.Remaining, Is.EqualTo(count));
        }

        [Test]
        public void OffTargetSpillPoseAndRegistrationValidationDoNotCreateAnotherHarvest()
        {
            var s = Create();
            Assert.Throws<ArgumentException>(() => s.RegisterPeppers(new[] { "pile" }, new[] { CarrierPose.Origin }));
            s.PickUp(); s.Gather("pile", 7); s.BeginPepperPour(false); s.EndPepperPour();
            Assert.That(s.UncontainedUnits, Is.EqualTo(7));
            Assert.That(s.ObservePepper("pepper-000", new CarrierPose(new Vector3(float.NaN, 0, 0), Quaternion.identity), true), Is.False);
            foreach (var p in s.Peppers) s.RecoverPepper(p.Id);
            Assert.That(s.Remaining, Is.EqualTo(25)); Check(s);
        }
    }
}
