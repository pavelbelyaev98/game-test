namespace SomethingDownThere
{
    public enum EquipmentKind { Shovel, Inventory, Fuel }

    // Authored capacity increments also preserve unusual capacities in older saves.
    public static class EquipmentProgression
    {
        public const int LevelCount = 5;
        public const float FuelPerCredit = 100f;
        private static readonly int[] Prices = { 6, 14, 28, 48 };
        private static readonly int[] Slots = { 5, 5, 10, 10 };
        private static readonly float[] Fuel = { 50, 50, 100, 100 };
        public static int Price(int ownedLevel) => Prices[ownedLevel - 1];
        public static int InventoryIncrease(int ownedLevel) => Slots[ownedLevel - 1];
        public static float FuelIncrease(int ownedLevel) => Fuel[ownedLevel - 1];
        public static string Name(EquipmentKind kind) => kind == EquipmentKind.Inventory ? "Backpack"
            : kind == EquipmentKind.Fuel ? "Fuel tank" : "Shovel";
    }
}
