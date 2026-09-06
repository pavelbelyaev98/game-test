using NUnit.Framework;

namespace JustAFewPeppers.Tests
{
    public class OperationStateTests
    {
        static HarvestState New(int units = 107) => new HarvestState(new[] { "supply" }, new[] { units }, 12, CarrierPose.Origin);
        static void Load(HarvestState state, int amount)
        {
            Assert.That(state.PickUp(), Is.True); Assert.That(state.Gather("supply", amount), Is.EqualTo(amount));
            Assert.That(state.Tip(), Is.EqualTo(amount)); Assert.That(state.Release(CarrierPose.Origin, true), Is.True);
        }
        static void Operate(HarvestState state)
        {
            Assert.That(state.BeginOperation(), Is.True); Assert.That(state.MoveOperation(.94f), Is.Positive);
        }

        [Test] public void LoadingWaitsForUsefulStrokeAndOutputRoomNeverStartsItAutomatically()
        {
            var s = New(); Load(s, 12); s.AdvanceProcessing(100);
            Assert.That(s.ActiveUnits + s.OutputUnits, Is.Zero); Assert.That(s.QueuedUnits, Is.EqualTo(12));
            Assert.That(s.BeginOperation(), Is.True); s.MoveOperation(.5f); s.MoveOperation(-.2f);
            Assert.That(s.OperationStroke, Is.EqualTo(.3f).Within(.001f)); Assert.That(s.ActiveUnits, Is.Zero);
            s.CancelStrokes(); Assert.That(s.QueuedUnits, Is.EqualTo(12)); Operate(s);
            Assert.That(s.MoveOperation(1), Is.Zero); Load(s, 6); s.AdvanceProcessing(100);
            Assert.That(s.OutputUnits, Is.EqualTo(12)); Assert.That(s.QueuedUnits, Is.EqualTo(6));
            Assert.That(s.CanOperate(), Is.EqualTo(OperationStatus.OutputBlocked)); Assert.That(s.BeginOperation(), Is.False);
            Assert.That(s.CollectOutput(), Is.Zero, "Passive collection cannot bypass grouping.");
            Assert.That(s.BeginGrouping(), Is.True); s.MoveGrouping(.94f); s.DepositFinished();
            s.AdvanceProcessing(100); Assert.That(s.QueuedUnits, Is.EqualTo(6)); Assert.That(s.ActiveUnits, Is.Zero);
            Operate(s); Assert.That(s.ActiveUnits, Is.EqualTo(6)); Assert.That(s.AccountedUnits, Is.EqualTo(107));
        }

        [Test] public void GroupingKeepsItsOriginalSubsetWhenNewOutputArrives()
        {
            var s = New(); Load(s, 3); Operate(s); s.AdvanceProcessing(4);
            Load(s, 6); Operate(s); Assert.That(s.OutputUnits + s.ActiveUnits, Is.EqualTo(9));
            Assert.That(s.BeginGrouping(), Is.True); s.MoveGrouping(.4f); s.AdvanceProcessing(4);
            Assert.That(s.OutputUnits, Is.EqualTo(9)); Assert.That(s.GroupingSelection, Is.EqualTo(3));
            Assert.That(s.MoveGrouping(.55f), Is.EqualTo(3)); Assert.That(s.OutputUnits, Is.EqualTo(6));
            s.CancelStrokes(); Assert.That(s.FinishedUnits, Is.EqualTo(3)); Assert.That(s.MoveGrouping(1), Is.Zero);
            Assert.That(s.DepositFinished(), Is.EqualTo(3)); Assert.That(s.DepositFinished(), Is.Zero);
            Assert.That(s.AccountedUnits, Is.EqualTo(107));
        }

        [Test] public void InterruptedAndInvalidGesturesKeepFoodAndExcludeCompetingHands()
        {
            var s = New(); Load(s, 7); Assert.That(s.BeginOperation(), Is.True);
            Assert.That(s.MoveOperation(float.NaN), Is.Zero); Assert.That(s.MoveOperation(float.PositiveInfinity), Is.Zero);
            s.MoveOperation(.5f); Assert.That(s.PickUp(), Is.False); s.CancelStrokes(); Operate(s); s.AdvanceProcessing(4);
            Assert.That(s.BeginGrouping(), Is.True); s.MoveGrouping(.5f); Assert.That(s.PickUp(), Is.False);
            s.CancelStrokes(); Assert.That(s.OutputUnits, Is.EqualTo(7)); Assert.That(s.FinishedUnits, Is.Zero);
            Assert.That(s.BeginGrouping(), Is.True); s.MoveGrouping(.94f); s.RecoverFinished(CarrierPose.Origin);
            Assert.That(s.FinishedUnits, Is.EqualTo(7)); Assert.That(s.BeginGrouping(), Is.False);
            s.ResetPrototype(); Assert.That(s.HasStroke, Is.False); Assert.That(s.OperationsCompleted + s.GroupsCompleted, Is.Zero);
            Assert.That(s.AccountedUnits, Is.EqualTo(107));
        }

        [TestCase(1)] [TestCase(25)] [TestCase(107)]
        public void CompleteAndFinalPartialJobsNeedOneOperationAndOneGroupPerBatch(int units)
        {
            var s = New(units); int batches = 0;
            while (s.Remaining > 0)
            {
                Load(s, System.Math.Min(12, s.Remaining)); Operate(s); s.AdvanceProcessing(4);
                Assert.That(s.BeginGrouping(), Is.True); Assert.That(s.MoveGrouping(.94f), Is.Positive);
                Assert.That(s.DepositFinished(), Is.Positive); batches++;
                Assert.That(s.AccountedUnits, Is.EqualTo(units));
            }
            Assert.That(s.StoredUnits, Is.EqualTo(units)); Assert.That(s.OperationsCompleted, Is.EqualTo(batches));
            Assert.That(s.GroupsCompleted, Is.EqualTo(batches)); Assert.That(s.HasStroke, Is.False);
        }
    }
}
