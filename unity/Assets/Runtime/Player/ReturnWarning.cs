using System;
using UnityEngine;

namespace SomethingDownThere
{
    public enum ReturnRisk { Safe, Risky, Critical }

    // Charge bands describe the reserve, never the cost or certainty of a return.
    [Serializable]
    public sealed class ReturnWarning
    {
        [SerializeField, Range(0f, 1f)] private float riskyFraction = 0.35f;
        [SerializeField, Range(0f, 1f)] private float criticalFraction = 0.15f;

        public float RiskyFraction => ValidFraction(riskyFraction, 0.35f);
        public float CriticalFraction => Mathf.Min(RiskyFraction, ValidFraction(criticalFraction, 0.15f));

        public ReturnWarning(float risky = 0.35f, float critical = 0.15f)
        {
            if (float.IsNaN(risky) || float.IsNaN(critical) || critical < 0 || risky >= 1 || critical >= risky)
                throw new ArgumentOutOfRangeException(nameof(risky), "Charge bands require 0 <= critical < risky < 1.");
            riskyFraction = risky;
            criticalFraction = critical;
        }

        public ReturnRisk Evaluate(Battery battery)
        {
            if (battery == null) throw new ArgumentNullException(nameof(battery));
            float fraction = battery.Charge / battery.Capacity;
            if (fraction <= CriticalFraction) return ReturnRisk.Critical;
            return fraction <= RiskyFraction ? ReturnRisk.Risky : ReturnRisk.Safe;
        }

        private static float ValidFraction(float value, float fallback)
            => float.IsNaN(value) || float.IsInfinity(value) ? fallback : Mathf.Clamp01(value);
    }
}
