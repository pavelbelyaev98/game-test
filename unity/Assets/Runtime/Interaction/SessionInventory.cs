using System;
using System.Collections.Generic;

namespace SomethingDownThere
{
    public sealed class SessionInventory
    {
        private readonly List<InventoryItem> items = new List<InventoryItem>();
        public IReadOnlyList<InventoryItem> Items { get; }
        public int Capacity { get; }
        public int Count => items.Count;
        public long Revision { get; private set; }
        public bool IsFull => Count >= Capacity;

        public SessionInventory(int capacity = 10)
        {
            if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            Items = items.AsReadOnly();
        }

        public bool TryAdd(InventoryItem item)
        {
            if (IsFull || item == null || IndexOf(item.InstanceId) >= 0) return false;
            items.Add(item);
            Revision++;
            return true;
        }

        // Return the exact record so a station can use its identity and value after removal.
        public bool TryRemove(string instanceId, out InventoryItem removedItem)
        {
            removedItem = null;
            int index = IndexOf(instanceId);
            if (index < 0) return false;
            removedItem = items[index];
            items.RemoveAt(index);
            Revision++;
            return true;
        }

        private int IndexOf(string instanceId)
        {
            for (int i = 0; i < items.Count; i++)
                if (string.Equals(items[i].InstanceId, instanceId, StringComparison.Ordinal)) return i;
            return -1;
        }
    }
}
