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
        readonly CarrierPose initialCarrierPose;
        readonly CarrierPose finishedDock;
        public int Capacity { get; }
        public int InitialHarvest { get; }
        public int Remaining { get; private set; }
        public int RawUnits { get; private set; }
        public bool IsHeld { get; private set; }
        public string CarrierId => "raw-crate";
        public CarrierPose RawPose { get; private set; }
        public CarrierPose SafeRawPose { get; private set; }
        public int InputCapacity { get; }
        public int OutputCapacity { get; }
        public double BatchDuration { get; }
        public int QueuedUnits { get; private set; }
        public int ActiveUnits { get; private set; }
        public int OutputUnits { get; private set; }
        public double BatchRemaining { get; private set; }
        public int FinishedCapacity { get; }
        public int FinishedUnits { get; private set; }
        public int StoredUnits { get; private set; }
        public bool FinishedHeld { get; private set; }
        public bool FinishedDocked { get; private set; }
        public string FinishedCarrierId => "finished-carrier";
        public CarrierPose FinishedPose { get; private set; }
        public CarrierPose SafeFinishedPose { get; private set; }
        public int AccountedUnits => Remaining + RawUnits + QueuedUnits + ActiveUnits + OutputUnits + FinishedUnits + StoredUnits;
        public bool OutputFull => OutputUnits == OutputCapacity;

        public HarvestState(string[] ids, int[] quantities, int capacity, CarrierPose initialPose,
            int inputCapacity = 12, int outputCapacity = 12, double batchDuration = 4,
            CarrierPose? dock = null, int finishedCapacity = 12)
        {
            if (ids == null || quantities == null || ids.Length == 0 || ids.Length != quantities.Length || capacity <= 0 || !initialPose.IsValid)
                throw new ArgumentException("Invalid harvest configuration.");
            Capacity = capacity;
            if (inputCapacity <= 0 || outputCapacity <= 0 || double.IsNaN(batchDuration) || double.IsInfinity(batchDuration) || batchDuration <= 0)
                throw new ArgumentException("Invalid station configuration.");
            InputCapacity = inputCapacity;
            OutputCapacity = outputCapacity;
            BatchDuration = batchDuration;
            initialCarrierPose = initialPose;
            finishedDock = dock ?? CarrierPose.Origin;
            if (!finishedDock.IsValid || finishedCapacity <= 0) throw new ArgumentException("Invalid finished carrier configuration.");
            FinishedCapacity = finishedCapacity;
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

        public bool PickUp(CarrierPose? finishedPlacement = null)
        {
            if (IsHeld || (FinishedHeld && (!finishedPlacement.HasValue || !finishedPlacement.Value.IsValid))) return false;
            if (FinishedHeld) ReleaseFinished(finishedPlacement.Value, true);
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

        // Geometry is validated by handling. Dropping needs a clear pose, but no support.
        public bool Release(CarrierPose pose, bool supported)
        {
            if (!IsHeld || !pose.IsValid) return false;
            IsHeld = false;
            RecordCarrierPose(pose, supported);
            return true;
        }

        public bool RecordCarrierPose(CarrierPose pose, bool safe)
        {
            if (!pose.IsValid) return false;
            RawPose = pose;
            if (!IsHeld && safe) SafeRawPose = pose;
            return true;
        }

        public void RecoverCarrier() => RecoverCarrier(SafeRawPose);

        public bool RecoverCarrier(CarrierPose pose)
        {
            if (!pose.IsValid) return false;
            IsHeld = false;
            RecordCarrierPose(pose, true);
            return true;
        }

        // The scene validates reach and nearby geometry before these atomic hand switches.
        public int CollectOutput(CarrierPose? rawPlacement = null)
        {
            if (!FinishedDocked || FinishedHeld || OutputUnits == 0 ||
                (IsHeld && (!rawPlacement.HasValue || !rawPlacement.Value.IsValid))) return 0;
            if (IsHeld) Release(rawPlacement.Value, true);
            int accepted = Math.Min(OutputUnits, FinishedCapacity);
            OutputUnits -= accepted;
            FinishedUnits = accepted;
            FinishedDocked = false;
            FinishedHeld = true;
            StartBatch(); // Released output room can serve queued work; active reservations remain intact.
            return accepted;
        }

        public bool PickUpFinished(CarrierPose? rawPlacement = null)
        {
            if (FinishedDocked || FinishedHeld || FinishedUnits == 0 ||
                (IsHeld && (!rawPlacement.HasValue || !rawPlacement.Value.IsValid))) return false;
            if (IsHeld) Release(rawPlacement.Value, true);
            FinishedHeld = true;
            return true;
        }

        public bool ReleaseFinished(CarrierPose pose, bool safe)
        {
            if (!FinishedHeld || !pose.IsValid) return false;
            FinishedHeld = false;
            return RecordFinishedPose(pose, safe);
        }

        public bool RecordFinishedPose(CarrierPose pose, bool safe)
        {
            if (!pose.IsValid || FinishedDocked) return false;
            FinishedPose = pose;
            if (!FinishedHeld && safe) SafeFinishedPose = pose;
            return true;
        }

        public bool RecoverFinished(CarrierPose pose)
        {
            if (!pose.IsValid || FinishedDocked) return false;
            FinishedHeld = false;
            return RecordFinishedPose(pose, true);
        }

        public int DepositFinished()
        {
            if (!FinishedHeld || FinishedUnits == 0) return 0;
            int accepted = FinishedUnits;
            StoredUnits += accepted;
            FinishedUnits = 0;
            DockFinished();
            return accepted;
        }

        void DockFinished()
        {
            FinishedHeld = false;
            FinishedDocked = true;
            FinishedPose = SafeFinishedPose = finishedDock;
        }

        public void ResetPrototype()
        {
            foreach (var region in initial) remaining[region.Key] = region.Value;
            Remaining = InitialHarvest;
            RawUnits = 0;
            QueuedUnits = ActiveUnits = OutputUnits = 0;
            BatchRemaining = 0;
            FinishedUnits = StoredUnits = 0;
            DockFinished();
            RecoverCarrier(initialCarrierPose);
        }
    }
}
