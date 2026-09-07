using System;
using System.Collections.Generic;

namespace SomethingDownThere
{
    public sealed class SessionInventory
    {
        private readonly List<string> items = new List<string>();
        public IReadOnlyList<string> Items { get; }
        public int Capacity { get; }
        public int Count => items.Count;
        public bool IsFull => Count >= Capacity;

        public SessionInventory(int capacity)
        {
            if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            Items = items.AsReadOnly();
        }

        public bool TryAdd(string itemName)
        {
            if (IsFull || string.IsNullOrWhiteSpace(itemName)) return false;
            items.Add(itemName);
            return true;
        }

        public bool TryRemoveAt(int index)
        {
            if (index < 0 || index >= Count) return false;
            items.RemoveAt(index);
            return true;
        }
    }
}
