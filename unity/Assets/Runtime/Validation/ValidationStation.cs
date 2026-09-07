using UnityEngine;

namespace SomethingDownThere
{
    // Session-only commands for exercising the station contract; no production economy/save data.
    public sealed class ValidationStation : StationTarget
    {
        public enum StationKind { Sell, Upgrade }
        [SerializeField] private StationKind kind;
        private int toolLevel = 1;
        public StationKind Kind { get => kind; set => kind = value; }
        public override string Title => kind == StationKind.Sell ? "Sell" : "Upgrade";
        public override int CommandCount => kind == StationKind.Sell ? 2 : 1;

        public override string Description(FpsPlayer player)
        {
            return kind == StationKind.Sell
                ? "Sell carried finds here. Surface service recharges the battery."
                : "Tool level " + toolLevel + " / 2\nNext: faster shovel cadence. Cost: 0 (validation scene).";
        }

        public override string CommandLabel(int index, FpsPlayer player)
        {
            if (kind == StationKind.Upgrade) return toolLevel < 2 ? "Upgrade tool" : "Tool fully upgraded";
            return index == 0 ? "Sell All" : "Sell first item";
        }

        public override bool CanExecute(int index, FpsPlayer player)
        {
            if (index < 0 || index >= CommandCount) return false;
            return kind == StationKind.Sell ? player.Inventory.Count > 0 : toolLevel < 2;
        }

        public override bool TryExecute(int index, FpsPlayer player)
        {
            if (player.Menu != PlayerMenu.Station || player.Station != this || !CanExecute(index, player)) return false;
            if (kind == StationKind.Upgrade)
            {
                toolLevel++;
                player.Tuning.DigInterval *= 0.75f;
                player.ShowFeedback("Shovel upgraded");
            }
            else
            {
                int count = index == 0 ? player.Inventory.Count : 1;
                for (int i = 0; i < count; i++)
                    player.Inventory.TryRemove(player.Inventory.Items[0].InstanceId, out _);
                player.ShowFeedback(count + " find(s) sold");
            }
            return true;
        }
    }
}
