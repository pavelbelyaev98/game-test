using System;

namespace SomethingDownThere
{
    // Scene-session credits. Transactions own when money moves; opening UI never does.
    public sealed class SessionWallet
    {
        public int Balance { get; private set; }
        public long Revision { get; private set; }

        public SessionWallet(int startingBalance = 0)
        {
            if (startingBalance < 0) throw new ArgumentOutOfRangeException(nameof(startingBalance));
            Balance = startingBalance;
        }

        public bool TryCredit(int amount)
        {
            if (amount < 0 || amount > int.MaxValue - Balance) return false;
            Balance += amount;
            if (amount != 0) Revision++;
            return true;
        }

        public bool TrySpend(int amount)
        {
            if (amount < 0 || amount > Balance) return false;
            Balance -= amount;
            if (amount != 0) Revision++;
            return true;
        }
    }
}
