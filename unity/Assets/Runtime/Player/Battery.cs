using System;

namespace SomethingDownThere
{
    public sealed class Battery
    {
        public float Capacity { get; }
        public float Charge { get; private set; }

        public Battery(float capacity)
        {
            if (!IsValid(capacity) || capacity <= 0f)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            Charge = capacity;
        }

        public bool CanSpend(float amount) => IsValid(amount) && amount >= 0f && Charge >= amount;

        public bool TrySpend(float amount)
        {
            if (!CanSpend(amount)) return false;
            Charge = Math.Max(0f, Charge - amount);
            return true;
        }

        public void Recharge() => Charge = Capacity;

        public void RestoreCharge(float charge)
        {
            if (!IsValid(charge) || charge < 0 || charge > Capacity) throw new ArgumentOutOfRangeException(nameof(charge));
            Charge = charge;
        }

        private static bool IsValid(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    }
}
