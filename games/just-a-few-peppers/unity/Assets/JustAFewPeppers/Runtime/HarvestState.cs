using System;
using System.Collections.Generic;

namespace JustAFewPeppers
{
    public enum GatherStatus { Ready, NeedCrate, Full, InvalidTarget, Empty }

    // The only food owner in 1_02. Configuration is copied; views never mutate quantities.
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

        public HarvestState(string[] ids, int[] quantities, int capacity, int safePointCount)
        {
            if (ids == null || quantities == null || ids.Length == 0 || ids.Length != quantities.Length || capacity <= 0 || safePointCount <= 0)
                throw new ArgumentException("Invalid harvest configuration.");
            Capacity = capacity;
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
            RecoverCarrier();
        }
    }
}
