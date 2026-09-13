using System;

namespace SomethingDownThere
{
    public sealed class Battery
    {
        public float Capacity { get; private set; }
        public float Charge { get; private set; }
        public int Level { get; private set; }
        public long Revision { get; private set; }

        public Battery(float capacity, int level = 1)
        {
            if (!IsValid(capacity) || capacity <= 0f)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            Charge = capacity;
            if (level < 1 || level > EquipmentProgression.LevelCount) throw new ArgumentOutOfRangeException(nameof(level));
            Level = level;
        }

        public bool CanSpend(float amount) => IsValid(amount) && amount >= 0f && Charge >= amount;

        public bool TrySpend(float amount)
        {
            if (!CanSpend(amount)) return false;
            Charge = Math.Max(0f, Charge - amount);
            if (amount > 0) Revision++;
            return true;
        }

        public void Recharge() => TryFillTo(Capacity);

        public bool TryAdd(float amount)
        {
            if (!IsValid(amount) || amount <= 0 || amount > Capacity - Charge) return false;
            Charge = Math.Min(Capacity, Charge + amount);
            Revision++;
            return true;
        }

        internal bool TryFillTo(float target)
        {
            if (!IsValid(target) || target <= Charge || target > Capacity) return false;
            Charge = target;
            Revision++;
            return true;
        }

        internal bool TryUpgradeTo(int level)
        {
            if (level != Level + 1 || level > EquipmentProgression.LevelCount) return false;
            Capacity += EquipmentProgression.FuelIncrease(Level);
            Level = level;
            Revision++;
            return true;
        }

        public void RestoreCharge(float charge)
        {
            if (!IsValid(charge) || charge < 0 || charge > Capacity) throw new ArgumentOutOfRangeException(nameof(charge));
            Charge = charge;
            Revision++;
        }

        private static bool IsValid(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    }
}
