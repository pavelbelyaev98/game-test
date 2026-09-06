using System;
using System.Collections.Generic;

namespace JustAFewPeppers
{
    public enum GatherStatus { Ready, NeedCrate, Full, InvalidTarget, Empty }
    public enum TipStatus { Ready, NeedCrate, Empty, InputFull }

    // The only food owner. Configuration is copied; views never mutate quantities.
    public sealed class HarvestState
    {
        readonly Dictionary<string, int> initial = new Dictionary<string, int>();
        readonly Dictionary<string, int> remaining = new Dictionary<string, int>();
        readonly int restingPointCount;
        public int Capacity { get; }
        public int InitialHarvest { get; }
        public int Remaining { get; private set; }
        public int RawUnits { get; private set; }
        public bool IsHeld { get; private set; }
        public int RestingPoint { get; private set; }
        public int InputCapacity { get; }
        public int OutputCapacity { get; }
        public double BatchDuration { get; }
        public int QueuedUnits { get; private set; }
        public int ActiveUnits { get; private set; }
        public int OutputUnits { get; private set; }
        public double BatchRemaining { get; private set; }
        public int AccountedUnits => Remaining + RawUnits + QueuedUnits + ActiveUnits + OutputUnits;
        public bool OutputFull => OutputUnits == OutputCapacity;

        public HarvestState(string[] ids, int[] quantities, int capacity, int safePointCount,
            int inputCapacity = 12, int outputCapacity = 12, double batchDuration = 4)
        {
            if (ids == null || quantities == null || ids.Length == 0 || ids.Length != quantities.Length || capacity <= 0 || safePointCount <= 0)
                throw new ArgumentException("Invalid harvest configuration.");
            Capacity = capacity;
            if (inputCapacity <= 0 || outputCapacity <= 0 || double.IsNaN(batchDuration) || double.IsInfinity(batchDuration) || batchDuration <= 0)
                throw new ArgumentException("Invalid station configuration.");
            InputCapacity = inputCapacity;
            OutputCapacity = outputCapacity;
            BatchDuration = batchDuration;
            restingPointCount = safePointCount;
            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(ids[i]) || quantities[i] <= 0 || initial.ContainsKey(ids[i]))
                    throw new ArgumentException("Pile regions need unique stable IDs and positive quantities.");
                initial.Add(ids[i], quantities[i]);
                InitialHarvest = checked(InitialHarvest + quantities[i]);
            }
            ResetPrototype();
        }

        public int UnitsIn(string id) => id != null && remaining.TryGetValue(id, out int units) ? units : 0;

        public GatherStatus CanGather(string id)
        {
            if (!IsHeld) return GatherStatus.NeedCrate;
            if (RawUnits == Capacity) return GatherStatus.Full;
            if (id == null || !remaining.ContainsKey(id)) return GatherStatus.InvalidTarget;
            return remaining[id] == 0 ? GatherStatus.Empty : GatherStatus.Ready;
        }

        public int Gather(string id, int requested)
        {
            if (requested <= 0 || CanGather(id) != GatherStatus.Ready) return 0;
            int accepted = Math.Min(requested, Math.Min(remaining[id], Capacity - RawUnits));
            remaining[id] -= accepted;
            Remaining -= accepted;
            RawUnits += accepted;
            return accepted;
        }

        public bool PickUp()
        {
            if (IsHeld) return false;
            IsHeld = true;
            return true;
        }

        public TipStatus CanTip()
        {
            if (!IsHeld) return TipStatus.NeedCrate;
            if (RawUnits == 0) return TipStatus.Empty;
            return QueuedUnits == InputCapacity ? TipStatus.InputFull : TipStatus.Ready;
        }

        // Reach is validated by the scene. One press commits once, before decorative motion starts.
        public int Tip()
        {
            if (CanTip() != TipStatus.Ready) return 0;
            int accepted = Math.Min(RawUnits, InputCapacity - QueuedUnits);
            RawUnits -= accepted;
            QueuedUnits += accepted;
            StartBatch();
            return accepted;
        }

        void StartBatch()
        {
            if (ActiveUnits > 0 || QueuedUnits == 0) return;
            int amount = Math.Min(QueuedUnits, OutputCapacity - OutputUnits);
            if (amount == 0) return;
            QueuedUnits -= amount;
            ActiveUnits = amount; // This is also the output reservation, never a second inventory.
            BatchRemaining = BatchDuration;
        }

        // Only the unpaused composition calls this. Long frames can finish and start successive batches.
        public int AdvanceProcessing(double seconds)
        {
            if (double.IsNaN(seconds) || double.IsInfinity(seconds) || seconds < 0)
                throw new ArgumentOutOfRangeException(nameof(seconds));
            int completed = 0;
            StartBatch();
            while (ActiveUnits > 0 && seconds > 0)
            {
                double used = Math.Min(seconds, BatchRemaining);
                BatchRemaining -= used;
                seconds -= used;
                if (BatchRemaining > 0) break;
                completed += ActiveUnits;
                OutputUnits += ActiveUnits;
                ActiveUnits = 0;
                StartBatch();
            }
            return completed;
        }

        // The scene validates reach, ground support and clearance before requesting a known resting point.
        public bool Park(int point)
        {
            if (!IsHeld || point < 0 || point >= restingPointCount) return false;
            IsHeld = false;
            RestingPoint = point;
            return true;
        }

        public void RecoverCarrier()
        {
            IsHeld = false;
            RestingPoint = 0;
        }

        public void ResetPrototype()
        {
            foreach (var region in initial) remaining[region.Key] = region.Value;
            Remaining = InitialHarvest;
            RawUnits = 0;
            QueuedUnits = ActiveUnits = OutputUnits = 0;
            BatchRemaining = 0;
            RecoverCarrier();
        }
    }
}
