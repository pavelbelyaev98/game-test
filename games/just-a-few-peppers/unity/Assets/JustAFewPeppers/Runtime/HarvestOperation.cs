using System;

namespace JustAFewPeppers
{
    public enum OperationStatus { Empty, Ready, Operating, Working, OutputBlocked, HandsOccupied, Pouring }
    public enum GroupingStatus { Empty, Ready, Grouping, CarrierAway, HandsOccupied }

    public sealed partial class HarvestState
    {
        public bool DirectOperation { get; }
        public float OperationStroke { get; private set; }
        public float GroupingStroke { get; private set; }
        public int OperationSelection { get; private set; }
        public int GroupingSelection { get; private set; }
        public int OperationsCompleted { get; private set; }
        public int GroupsCompleted { get; private set; }
        public bool HasStroke => OperationSelection > 0 || GroupingSelection > 0;

        public OperationStatus CanOperate()
        {
            if (IsHeld || FinishedHeld || SingleHeld) return OperationStatus.HandsOccupied;
            if (PourOpen) return OperationStatus.Pouring;
            if (ActiveUnits > 0) return OperationStatus.Working;
            if (OperationSelection > 0) return OperationStatus.Operating;
            if (QueuedUnits == 0) return OperationStatus.Empty;
            return OutputUnits == OutputCapacity ? OperationStatus.OutputBlocked : OperationStatus.Ready;
        }

        public bool BeginOperation()
        {
            if (!DirectOperation || GroupingSelection > 0) return false;
            if (CanOperate() == OperationStatus.Operating) return true;
            if (CanOperate() != OperationStatus.Ready) return false;
            OperationSelection = Math.Min(QueuedUnits, OutputCapacity - OutputUnits);
            OperationStroke = 0;
            return true;
        }

        // Signed useful movement, not elapsed time or an animation completion callback.
        public int MoveOperation(float delta)
        {
            if (!ValidStrokeDelta(delta) || OperationSelection == 0 || CanOperate() != OperationStatus.Operating) return 0;
            OperationStroke = Math.Clamp(OperationStroke + delta, 0, 1);
            if (OperationStroke < .94f) return 0;
            int amount = OperationSelection;
            if (amount > QueuedUnits || amount > OutputCapacity - OutputUnits) { CancelStrokes(); return 0; }
            QueuedUnits -= amount; ActiveUnits = amount; BatchRemaining = BatchDuration;
            OperationSelection = 0; OperationStroke = 0; OperationsCompleted++;
            return amount;
        }

        public GroupingStatus CanGroup()
        {
            if (!FinishedDocked) return GroupingStatus.CarrierAway;
            if (IsHeld || FinishedHeld || SingleHeld) return GroupingStatus.HandsOccupied;
            if (GroupingSelection > 0) return GroupingStatus.Grouping;
            return OutputUnits > 0 ? GroupingStatus.Ready : GroupingStatus.Empty;
        }

        public bool BeginGrouping()
        {
            if (!DirectOperation || OperationSelection > 0) return false;
            if (CanGroup() == GroupingStatus.Grouping) return true;
            if (CanGroup() != GroupingStatus.Ready) return false;
            GroupingSelection = Math.Min(OutputUnits, FinishedCapacity);
            GroupingStroke = 0;
            return true;
        }

        public int MoveGrouping(float delta)
        {
            if (!ValidStrokeDelta(delta) || GroupingSelection == 0 || CanGroup() != GroupingStatus.Grouping) return 0;
            GroupingStroke = Math.Clamp(GroupingStroke + delta, 0, 1);
            if (GroupingStroke < .94f) return 0;
            int amount = GroupingSelection;
            if (amount > OutputUnits || amount > FinishedCapacity) { CancelStrokes(); return 0; }
            GroupingSelection = 0; GroupingStroke = 0;
            TransferOutput(amount); GroupsCompleted++;
            return amount;
        }

        static bool ValidStrokeDelta(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

        public void CancelStrokes()
        {
            OperationSelection = GroupingSelection = 0;
            OperationStroke = GroupingStroke = 0;
        }

        void TransferOutput(int amount)
        {
            OutputUnits -= amount; FinishedUnits = amount;
            FinishedDocked = false; FinishedHeld = true;
        }
    }
}
