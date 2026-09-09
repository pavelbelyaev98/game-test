using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SomethingDownThere
{
    public sealed class GameMenuView : IDisposable
    {
        private readonly FpsPlayer player;
        private readonly Label title, subtitle, pauseSave, pauseError;
        private readonly VisualElement pausePage, cameraPage, contentPage, pauseActions, actions;
        private readonly ScrollView scroll;
        private readonly List<VisualElement> navigation = new List<VisualElement>();
        private readonly ToolkitCameraSettings camera;
        private PlayerMenu displayed;
        private bool pending = true;
        private WorldSaveController persistence;
        private int generation;
        public VisualElement Root { get; }
        public VisualElement CurrentScreen { get; private set; }
        public Focusable Focused => Root.focusController?.focusedElement;

        public GameMenuView(VisualElement document, FpsPlayer player)
        {
            this.player = player;
            Root = document.Q("menuRoot");
            title = Root.Q<Label>("menuTitle");
            subtitle = Root.Q<Label>("menuSubtitle");
            pausePage = Root.Q("pausePage");
            cameraPage = Root.Q("cameraPage");
            contentPage = Root.Q("contentPage");
            pauseActions = Root.Q("pauseActions");
            pauseSave = Root.Q<Label>("pauseSaveStatus");
            pauseError = Root.Q<Label>("pauseSettingsError");
            scroll = Root.Q<ScrollView>("menuScroll");
            scroll.mouseWheelScrollSize = 42;
            actions = Root.Q("menuActions");
            camera = new ToolkitCameraSettings(cameraPage, player);
            Root.RegisterCallback<NavigationMoveEvent>(Navigate, TrickleDown.TrickleDown);
            Root.RegisterCallback<KeyDownEvent>(e =>
            {
                if (IsOwnedKey(e.keyCode)) e.StopImmediatePropagation();
            }, TrickleDown.TrickleDown);
            Root.RegisterCallback<KeyUpEvent>(e =>
            {
                if (IsOwnedKey(e.keyCode)) e.StopImmediatePropagation();
            }, TrickleDown.TrickleDown);
            Root.RegisterCallback<PointerDownEvent>(e =>
            {
                // Background clicks do not discard the current keyboard control.
                var target = e.target as VisualElement;
                if (target != null && FindNavigable(target) < 0) Root.focusController.IgnoreEvent(e);
            }, TrickleDown.TrickleDown);
            player.MenuChanged += QueueRefresh;
            player.CameraSettings.Changed += CameraChanged;
        }

        public void Tick()
        {
            if (persistence != player.Persistence)
            {
                if (persistence != null) persistence.Changed -= SaveChanged;
                persistence = player.Persistence;
                if (persistence != null) persistence.Changed += SaveChanged;
                pending = true;
            }
            if (pending) Rebuild();
            if (player.Menu == PlayerMenu.Pause)
            {
                var save = player.Persistence;
                pauseSave.text = save == null ? "" : "Autosaves every 10 seconds and after trades.  "
                    + (save.State == WorldSaveState.Saving ? "Saving..." : save.LastSavedLabel);
            }
        }

        private void QueueRefresh() => pending = true;
        // Native players also send raw key events to sliders. Arrows are handled by
        // Input System navigation once; Escape/Space belong to the player barrier.
        private static bool IsOwnedKey(KeyCode key) => key == KeyCode.Escape || key == KeyCode.Space
            || key == KeyCode.LeftArrow || key == KeyCode.RightArrow || key == KeyCode.UpArrow || key == KeyCode.DownArrow;
        private void SaveChanged() { if (player.Menu == PlayerMenu.Persistence) pending = true; }
        private void CameraChanged() => Show(pauseError, player.CameraSettings.WriteFailed);

        private void Rebuild()
        {
            pending = false;
            generation++;
            bool cameraBack = displayed == PlayerMenu.CameraComfort && player.Menu == PlayerMenu.Pause;
            displayed = player.Menu;
            navigation.Clear();
            Show(Root, player.IsMenuOpen);
            Show(pausePage, displayed == PlayerMenu.Pause);
            Show(cameraPage, displayed == PlayerMenu.CameraComfort);
            Show(contentPage, displayed != PlayerMenu.None && displayed != PlayerMenu.Pause && displayed != PlayerMenu.CameraComfort);
            if (!player.IsMenuOpen) { CurrentScreen = null; return; }
            if (displayed == PlayerMenu.CameraComfort)
            {
                CurrentScreen = cameraPage;
                title.text = "Camera comfort";
                subtitle.text = "Adjust your view. Changes apply immediately.";
                camera.AddNavigation(navigation);
                FocusAfterLayout(camera.Slider);
                return;
            }
            if (displayed == PlayerMenu.Pause)
            {
                CurrentScreen = pausePage;
                title.text = "Paused";
                subtitle.text = "Your excavation is waiting.";
                pauseActions.Clear();
                Button(pauseActions, "Resume", player.CloseMenu, true, "primary");
                var comfort = Button(pauseActions, "Camera comfort", player.ShowCameraComfort);
                if (player.RescueAvailable) Button(pauseActions, "Call rescue...", player.RequestRescue);
                if (player.AdminAvailable) Button(pauseActions, "Developer admin  /  Ctrl+Shift+F10", player.ShowAdminMenu);
                if (player.Persistence != null) Button(pauseActions, "Save and quit", player.Persistence.RequestExit, true, "quiet");
                CameraChanged();
                FocusAfterLayout(cameraBack ? comfort : navigation[0]);
                return;
            }
            CurrentScreen = contentPage;
            scroll.Clear();
            scroll.scrollOffset = Vector2.zero;
            actions.Clear();
            subtitle.text = "";
            if (displayed == PlayerMenu.Persistence) BuildSave();
            else if (displayed == PlayerMenu.DeveloperAdmin) BuildAdmin();
            else if (displayed == PlayerMenu.ConfirmRescue) BuildRescue();
            else if (displayed == PlayerMenu.ConfirmTerrainReset) BuildTerrainReset();
            else if (displayed == PlayerMenu.Station && player.Station is SellStation sell) BuildSale(sell);
            else if (displayed == PlayerMenu.Station && player.Station is UpgradeStation upgrade) BuildUpgrade(upgrade);
            else BuildInventory();
            if (actions.childCount > 0) actions[0].AddToClassList("first-action");
            Show(actions, actions.childCount > 0);
            if (navigation.Count > 0)
            {
                var selected = displayed == PlayerMenu.Station && (player.Station is SellStation || player.Station is UpgradeStation)
                    ? navigation[navigation.Count - 1] : navigation[0];
                FocusAfterLayout(selected);
            }
        }

        private void BuildInventory()
        {
            title.text = displayed == PlayerMenu.Inventory ? "Inventory" : player.Station?.Title ?? "Station unavailable";
            subtitle.text = $"Carried finds: {player.Inventory.Count} / {player.Inventory.Capacity}";
            if (displayed == PlayerMenu.Station && player.Station != null) Text(scroll, "Body", player.Station.Description(player), "body");
            if (player.Inventory.Count == 0) Text(scroll, "Empty bag", "No carried finds", "empty");
            foreach (var item in player.Inventory.Items)
            {
                var row = Element(scroll, "item-row");
                Text(row, "Find name", item.DisplayName, "item-name");
                Text(row, "Sale value", "Sale value: " + item.SaleValue, "item-value");
            }
            if (displayed == PlayerMenu.Station && player.Station != null)
                for (int i = 0; i < player.Station.CommandCount; i++)
                {
                    int command = i;
                    long revision = player.StationRevision;
                    Button(actions, player.Station.CommandLabel(i, player), () => player.ExecuteStationCommand(command, revision), player.Station.CanExecute(i, player));
                }
            Button(actions, "Close", player.CloseMenu, true, "primary");
        }

        private void BuildSale(SellStation station)
        {
            title.text = station.Title;
            subtitle.text = "Choose what to sell. Each find stays yours until you confirm a sale.";
            Text(scroll, "Trade balance", $"CREDITS  {player.Wallet.Balance}     |     BAG  {station.Items.Count} / {player.Inventory.Capacity}", "trade-balance");
            string notice = station.TotalValue > int.MaxValue - player.Wallet.Balance
                ? "Credit limit reached. These finds remain in your bag." : player.StationNotice;
            Text(scroll, "Trade result", notice, "notice");
            long revision = player.StationRevision;
            if (station.Items.Count == 0) Text(scroll, "Empty bag", "No carried finds", "empty");
            for (int i = 0; i < station.Items.Count; i++)
            {
                int command = i + 1;
                var item = station.Items[i];
                var row = Button(scroll, "Sell " + item.InstanceId, () => player.ExecuteStationCommand(command, revision), station.CanExecute(command, player), "item-row", "");
                Text(row, "Find name", item.DisplayName, "item-name");
                Text(row, "Sell value", $"Sell  +{item.SaleValue} credits", "item-value");
            }
            Button(actions, "Sell all", () => player.ExecuteStationCommand(0, revision), station.CanExecute(0, player), "primary", station.CommandLabel(0, player));
            Button(actions, "Close station", player.CloseMenu, true, "", "Close");
        }

        private void BuildUpgrade(UpgradeStation station)
        {
            var offer = station.Offer;
            long revision = player.StationRevision;
            title.text = station.Title;
            subtitle.text = "Compare your next upgrade before purchasing.";
            Text(scroll, "Trade balance", $"CREDITS  {player.Wallet.Balance}     |     OWNED SHOVEL  {player.Shovel.Level} / {player.Shovel.LevelCount}", "trade-balance");
            Text(scroll, "Trade result", player.StationNotice, "notice");
            Text(scroll, "Upgrade heading", offer.Complete ? "Your shovel is fully upgraded" : $"Shovel {offer.OwnedLevel}  →  Shovel {offer.NextLevel}", "upgrade-heading");
            var current = player.Shovel.Current;
            int level = offer.Complete ? player.Shovel.Level : offer.NextLevel;
            var next = player.Shovel.GetProfile(level);
            var comparison = Element(scroll, "comparison");
            comparison.name = "Upgrade comparison";
            Stat(comparison, "Scoop width", $"{current.Radius * 2:F2} m", $"{next.Radius * 2:F2} m", offer.Complete);
            Stat(comparison, "Reach", $"{player.DigReachAtLevel(offer.OwnedLevel):F1} m", $"{player.DigReachAtLevel(level):F1} m", offer.Complete);
            Stat(comparison, "Stroke time", $"{player.Tuning.DigInterval * current.CadenceMultiplier:F2} s", $"{player.Tuning.DigInterval * next.CadenceMultiplier:F2} s", offer.Complete);
            string cost = offer.Complete ? "All six shovel levels owned"
                : player.Wallet.Balance < offer.Cost ? $"Cost: {offer.Cost} credits  |  Need {offer.Cost - player.Wallet.Balance} more"
                : $"Cost: {offer.Cost} credits  |  Balance afterward: {player.Wallet.Balance - offer.Cost}";
            Text(scroll, "Upgrade cost", cost, "notice");
            if (player.HasAdminOverrides) Text(scroll, "Upgrade override notice", "Developer overrides are active; this purchase changes your owned shovel.", "caption");
            Button(actions, "Buy upgrade", () => player.ExecuteStationCommand(0, revision), station.CanExecute(0, player), "primary", station.CommandLabel(0, player));
            Button(actions, "Close station", player.CloseMenu, true, "", "Close");
        }

        private void BuildRescue()
        {
            title.text = "Call rescue?";
            subtitle.text = "Review what you will leave behind.";
            var quote = player.Rescue.Quote;
            Text(scroll, "Rescue notice", player.RescueNotice, "notice");
            if (quote != null)
            {
                Text(scroll, "Body", $"Carried finds lost: {quote.LostItems.Count}  |  Sale value: {quote.LostSaleValue}\n"
                    + $"Rescue fee: {quote.Fee} credits\nBalance afterward: {quote.RemainingBalance} credits\n\n"
                    + "Excavation and shovel upgrades are kept.\nYou return to the surface with a full battery.", "body");
                foreach (var item in quote.LostItems) Text(scroll, "Lost find", item.DisplayName, "item-row");
            }
            Button(actions, "Cancel", player.CancelRescue, true, "primary");
            Button(actions, "Confirm rescue", () => player.ConfirmRescue(), quote != null, "destructive");
        }

        private void BuildTerrainReset()
        {
            title.text = "Reset the excavation?";
            subtitle.text = "Developer action";
            Text(scroll, "Body", "This fills every hole in this site and returns you to the safe surface. Your excavation will be lost.\n\nYour inventory and owned shovel level are kept. The battery is refilled.", "body");
            Button(actions, "Keep excavation", player.CancelTerrainReset, true, "primary");
            Button(actions, "Reset ground", () => player.ConfirmTerrainReset(), true, "destructive");
        }

        private void BuildAdmin()
        {
            title.text = "Developer admin";
            subtitle.text = "Overrides last this session; owned upgrades are kept.";
            Text(scroll, "Body", $"Shovel {player.EffectiveShovelLevel}  |  {player.EffectiveDigReach:F1} m reach  |  {player.EffectiveShovel.Radius * 2:F2} m scoop\n"
                + $"This site: {player.SuccessfulStrokes} strokes, {player.ExcavatedVolume:F1} m³ removed.", "body");
            var grid = Element(scroll, "admin-actions");
            for (int i = 1; i <= player.Shovel.LevelCount; i++)
            {
                int level = i;
                Button(grid, $"{(i == player.EffectiveShovelLevel ? "Selected: " : "")}Shovel {i} / {player.DigReachAtLevel(i):F1} m reach", () => player.SelectAdminLevel(level));
            }
            Button(grid, "Refill battery", player.RefillAdminBattery);
            Button(grid, "Return to surface", player.AdminReturnToSurface);
            Button(grid, "Reset ground...", player.RequestTerrainReset);
            Button(grid, "Unlimited battery: " + (player.UnlimitedBattery ? "ON" : "OFF"), player.ToggleAdminUnlimitedBattery);
            Button(grid, "X-ray: " + (player.AdminXray ? "ON" : "OFF"), player.ToggleAdminXray, player.Discoveries != null);
            Button(grid, "Restore normal rules", player.RestoreAdminOverrides, player.HasAdminOverrides);
            Button(actions, "Resume digging", player.CloseMenu, true, "primary");
        }

        private void BuildSave()
        {
            var save = player.Persistence;
            if (save == null) return;
            title.text = save.ExitRequested ? "Saving your excavation" : save.State == WorldSaveState.Loading ? "Loading your excavation"
                : save.State == WorldSaveState.Recovery ? "Excavation recovered" : save.State == WorldSaveState.ConfirmQuit ? "Leave without saving?"
                : save.State == WorldSaveState.LoadFailed ? "Cannot load this excavation" : "Progress could not be saved";
            if (save.ExitRequested || save.State == WorldSaveState.Loading)
            {
                Text(scroll, "Body", save.ExitRequested ? "Finishing your checkpoint before closing..." : "Restoring your ground, discoveries and equipment...", "body");
                return;
            }
            if (save.State == WorldSaveState.Recovery)
            {
                Text(scroll, "Body", "The latest checkpoint could not be loaded. Your last complete excavation has been recovered.\n\n" + save.LastSavedLabel
                    + "\nOnly changes after that checkpoint may be missing. The damaged file will be kept for recovery.", "body");
                Button(actions, "Continue recovered excavation", save.AcceptRecovery, true, "primary");
            }
            else if (save.State == WorldSaveState.ConfirmQuit)
            {
                Text(scroll, "Body", "Your last complete checkpoint will be kept. Changes since then will be lost.\n\n" + save.LastSavedLabel, "body");
                Button(actions, "Back", save.CancelUnsavedExit, true, "primary");
                Button(actions, "Quit without saving", save.ConfirmUnsavedExit, true, "destructive");
                return;
            }
            else
            {
                Text(scroll, "Body", (save.State == WorldSaveState.LoadFailed ? "Your save files have been kept. No new excavation has been started."
                    : "Your excavation is still here. Check available disk space and access to the save folder, then retry.")
                    + "\n\n" + save.LastSavedLabel + "\n\n" + save.ErrorDetail, "body");
                Button(actions, save.State == WorldSaveState.LoadFailed ? "Retry loading" : "Retry saving", save.Retry, true, "primary");
            }
            Button(actions, "Open save folder", save.OpenSaveFolder);
            Button(actions, save.State == WorldSaveState.WriteFailed ? "Quit..." : "Quit", save.RequestExit, true, "quiet");
        }

        private Button Button(VisualElement parent, string name, Action action, bool enabled = true, string style = "", string caption = null)
        {
            var button = new Button(action) { name = name, text = caption ?? name };
            button.AddToClassList("menu-button");
            if (style.Length > 0) button.AddToClassList(style);
            button.SetEnabled(enabled);
            parent.Add(button);
            if (enabled) navigation.Add(button);
            button.RegisterCallback<FocusInEvent>(_ => { if (scroll.Contains(button)) scroll.ScrollTo(button); });
            return button;
        }

        private static void Stat(VisualElement parent, string name, string current, string next, bool complete)
        {
            var row = Element(parent, "stat-row");
            Text(row, name, name, "stat-name");
            Text(row, name + " value", complete ? current : current + "  →  " + next, "stat-value");
        }

        private static VisualElement Element(VisualElement parent, string style)
        {
            var element = new VisualElement();
            element.AddToClassList(style);
            parent.Add(element);
            return element;
        }

        private static Label Text(VisualElement parent, string name, string text, string style)
        {
            var label = new Label(text) { name = name, pickingMode = PickingMode.Ignore, enableRichText = false };
            label.AddToClassList(style);
            Show(label, !string.IsNullOrEmpty(text));
            parent.Add(label);
            return label;
        }

        internal static void Show(VisualElement element, bool visible) => element.EnableInClassList("hidden", !visible);

        private void FocusAfterLayout(VisualElement element)
        {
            int expected = generation;
            element.Focus();
            Root.schedule.Execute(() => { if (generation == expected && player.IsMenuOpen) element.Focus(); });
        }

        private int FindNavigable(VisualElement element)
        {
            for (int i = 0; i < navigation.Count; i++)
                if (navigation[i] == element || navigation[i].Contains(element)) return i;
            return -1;
        }

        private void Navigate(NavigationMoveEvent e)
        {
            if (!player.IsMenuOpen) return;
            // NavigationMoveEvent changes focus in PostDispatch even when propagation
            // was stopped. Our explicit order is the sole focus movement for this event.
            Root.focusController.IgnoreEvent(e);
            if (e.direction == NavigationMoveEvent.Direction.None) { e.StopImmediatePropagation(); return; }
            if (displayed == PlayerMenu.CameraComfort)
            {
                navigation.Clear();
                camera.AddNavigation(navigation);
                if (camera.Adjust(Focused as VisualElement, e.direction)) { e.StopImmediatePropagation(); return; }
            }
            int direction = e.direction == NavigationMoveEvent.Direction.Up || e.direction == NavigationMoveEvent.Direction.Left
                || e.direction == NavigationMoveEvent.Direction.Previous ? -1 : 1;
            if (navigation.Count == 0) return;
            int index = FindNavigable(Focused as VisualElement);
            index = Mathf.Clamp(index + direction, 0, navigation.Count - 1);
            navigation[index].Focus();
            e.StopImmediatePropagation();
        }

        public void Dispose()
        {
            generation++;
            player.MenuChanged -= QueueRefresh;
            if (persistence != null) persistence.Changed -= SaveChanged;
            player.CameraSettings.Changed -= CameraChanged;
            camera.Dispose();
        }
    }
}
