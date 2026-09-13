using System;

namespace SomethingDownThere
{
    // Whole session money. Transactions own when money moves; opening UI never does.
    public sealed class SessionWallet
    {
        public const decimal MinimumUnit = 1m;
        public const decimal MaximumBalance = int.MaxValue;
        public int Balance { get; private set; }
        public int WholeCredits => Balance;
        public int CreditFraction => 0;
        public long Revision { get; private set; }

        public SessionWallet(int startingBalance = 0, int creditFraction = 0)
        {
            if (startingBalance < 0 || creditFraction < 0 || creditFraction > 99
                || (startingBalance == int.MaxValue && creditFraction != 0))
                throw new ArgumentOutOfRangeException(nameof(startingBalance));
            // Retain purchasing power from the short-lived fractional save format.
            Balance = startingBalance + (creditFraction > 0 ? 1 : 0);
        }

        public bool TryCredit(decimal amount)
        {
            if (amount < 0 || amount > MaximumBalance - Balance || amount != decimal.Truncate(amount)) return false;
            Balance += (int)amount;
            if (amount != 0) Revision++;
            return true;
        }

        public bool TrySpend(decimal amount)
        {
            if (amount < 0 || amount > Balance || amount != decimal.Truncate(amount)) return false;
            Balance -= (int)amount;
            if (amount != 0) Revision++;
            return true;
        }
    }
}
