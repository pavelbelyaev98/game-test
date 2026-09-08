using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SomethingDownThere
{
    // Station-only presentation inside the existing HUD/menu canvas.
    public sealed class StationMenuView
    {
        private readonly RectTransform root;
        private readonly Font font;
        private readonly FpsPlayer player;
        private readonly List<Button> buttons = new List<Button>();
        private static readonly Color Ink = new Color(0.90f, 0.95f, 0.94f);
        private static readonly Color Credit = new Color(0.65f, 0.90f, 0.77f);

        public StationMenuView(Transform parent, Font font, FpsPlayer player)
        {
            this.font = font;
            this.player = player;
            root = Rect("Station content", parent);
            Stretch(root, new Vector2(28, 24), new Vector2(-28, -78));
            root.gameObject.SetActive(false);
        }

        public void Hide() => root.gameObject.SetActive(false);

        public void Show()
        {
            root.gameObject.SetActive(true);
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                var child = root.GetChild(i).gameObject;
                child.SetActive(false);
                Object.Destroy(child);
            }
            buttons.Clear();
            if (player.Station is SellStation sell) BuildSale(sell);
            else if (player.Station is UpgradeStation upgrade) BuildUpgrade(upgrade);
            var close = Button("Close station", root, "Close", true, player.CloseMenu);
            Anchor(close.GetComponent<RectTransform>(), new Vector2(1, 0), Vector2.zero, new Vector2(160, 48));
            var navigationButtons = buttons.FindAll(button => button.interactable);
            foreach (var button in buttons)
            {
                // One explicit vertical chain works for all rows and the fixed footer.
                if (!button.interactable) { button.navigation = new Navigation { mode = Navigation.Mode.None }; continue; }
                int index = navigationButtons.IndexOf(button);
                var navigation = new Navigation { mode = Navigation.Mode.Explicit };
                navigation.selectOnUp = navigationButtons[Mathf.Max(0, index - 1)];
                navigation.selectOnDown = navigationButtons[Mathf.Min(navigationButtons.Count - 1, index + 1)];
                navigation.selectOnLeft = navigation.selectOnUp;
                navigation.selectOnRight = navigation.selectOnDown;
                button.navigation = navigation;
            }
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(close.gameObject);
        }

        private void BuildSale(SellStation station)
        {
            long revision = player.StationRevision;
            TopText("Trade balance", $"CREDITS  {player.Wallet.Balance}     |     BAG  {station.Items.Count} / {player.Inventory.Capacity}", 0, 20, Ink);
            string notice = player.StationNotice;
            if (station.TotalValue > int.MaxValue - player.Wallet.Balance) notice = "Credit limit reached. These finds remain in your bag.";
            TopText("Trade result", notice, 36, 18, Credit);
            var scrollRoot = Rect("Finds scroll", root);
            Stretch(scrollRoot, new Vector2(0, 78), new Vector2(0, -78));
            var scroll = scrollRoot.gameObject.AddComponent<ScrollRect>();
            var viewport = Rect("Viewport", scrollRoot);
            Stretch(viewport, Vector2.zero, Vector2.zero);
            viewport.gameObject.AddComponent<Image>().color = new Color(0, 0, 0, 0.18f);
            viewport.gameObject.AddComponent<RectMask2D>();
            var content = Rect("Find rows", viewport);
            content.anchorMin = new Vector2(0, 1);
            content.anchorMax = Vector2.one;
            content.pivot = new Vector2(0.5f, 1);
            content.sizeDelta = Vector2.zero;
            var layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 6;
            layout.childControlWidth = layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scroll.viewport = viewport;
            scroll.content = content;
            scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 30;
            for (int i = 0; i < station.Items.Count; i++)
            {
                int command = i + 1;
                var item = station.Items[i];
                var row = Button("Sell " + item.InstanceId, content, "", station.CanExecute(command, player),
                    () => player.ExecuteStationCommand(command, revision));
                var size = row.gameObject.AddComponent<LayoutElement>();
                size.minHeight = size.preferredHeight = 48;
                var name = Label("Find name", row.transform, item.DisplayName, 19, Ink);
                Stretch(name.rectTransform, new Vector2(16, 0), new Vector2(-210, 0));
                var value = Label("Sell value", row.transform, $"Sell  +{item.SaleValue} credits", 18, Credit);
                Stretch(value.rectTransform, new Vector2(0, 0), new Vector2(-16, 0));
                value.alignment = TextAnchor.MiddleRight;
                row.gameObject.AddComponent<StationRowFocus>().Configure(scroll);
            }
            if (station.Items.Count == 0)
            {
                var empty = Label("Empty bag", viewport, "No carried finds", 22, Ink);
                Stretch(empty.rectTransform, Vector2.zero, Vector2.zero);
                empty.alignment = TextAnchor.MiddleCenter;
            }
            var sellAll = Button("Sell all", root, station.CommandLabel(0, player), station.CanExecute(0, player),
                () => player.ExecuteStationCommand(0, revision));
            FooterAction(sellAll.GetComponent<RectTransform>());
        }

        private void BuildUpgrade(UpgradeStation station)
        {
            var offer = station.Offer;
            long revision = player.StationRevision;
            TopText("Trade balance", $"CREDITS  {player.Wallet.Balance}     |     OWNED SHOVEL  {player.Shovel.Level} / {player.Shovel.LevelCount}", 0, 20, Ink);
            TopText("Trade result", player.StationNotice, 36, 18, Credit);
            var current = player.Shovel.Current;
            int nextLevel = offer.Complete ? player.Shovel.Level : offer.NextLevel;
            var next = player.Shovel.GetProfile(nextLevel);
            TopText("Upgrade heading", offer.Complete ? "Your shovel is fully upgraded" : $"Shovel {offer.OwnedLevel}  →  Shovel {offer.NextLevel}", 92, 27, Ink);
            string stats = offer.Complete
                ? $"Scoop width     {current.Radius * 2:F2} m\nReach                {player.DigReachAtLevel(nextLevel):F1} m\nStroke time       {player.Tuning.DigInterval * current.CadenceMultiplier:F2} s"
                : $"Scoop width     {current.Radius * 2:F2} m  →  {next.Radius * 2:F2} m\n"
                    + $"Reach                {player.DigReachAtLevel(offer.OwnedLevel):F1} m  →  {player.DigReachAtLevel(nextLevel):F1} m\n"
                    + $"Stroke time       {player.Tuning.DigInterval * current.CadenceMultiplier:F2} s  →  {player.Tuning.DigInterval * next.CadenceMultiplier:F2} s";
            var comparison = Label("Upgrade comparison", root, stats, 22, Ink);
            comparison.alignment = TextAnchor.UpperLeft;
            Anchor(comparison.rectTransform, new Vector2(0, 1), new Vector2(0, -150), new Vector2(680, 118));
            string cost = offer.Complete ? "All six shovel levels owned"
                : player.Wallet.Balance < offer.Cost ? $"Cost: {offer.Cost} credits  |  Need {offer.Cost - player.Wallet.Balance} more"
                : $"Cost: {offer.Cost} credits  |  Balance afterward: {player.Wallet.Balance - offer.Cost}";
            TopText("Upgrade cost", cost, 285, 20, Credit);
            if (player.HasAdminOverrides)
                TopText("Upgrade override notice", "Developer overrides are active; this purchase changes your owned shovel.", 326, 17, Ink);
            var buy = Button("Buy upgrade", root, station.CommandLabel(0, player), station.CanExecute(0, player),
                () => player.ExecuteStationCommand(0, revision));
            FooterAction(buy.GetComponent<RectTransform>());
        }

        private void TopText(string name, string content, float y, int size, Color color)
        {
            var label = Label(name, root, content, size, color);
            label.rectTransform.anchorMin = new Vector2(0, 1);
            label.rectTransform.anchorMax = Vector2.one;
            label.rectTransform.pivot = new Vector2(0, 1);
            label.rectTransform.anchoredPosition = new Vector2(0, -y);
            label.rectTransform.sizeDelta = new Vector2(0, 34);
        }

        private Button Button(string name, Transform parent, string caption, bool enabled, UnityEngine.Events.UnityAction action)
        {
            var rect = Rect(name, parent);
            rect.gameObject.AddComponent<Image>().color = new Color(0.22f, 0.29f, 0.30f);
            var button = rect.gameObject.AddComponent<Button>();
            button.interactable = enabled;
            var colors = button.colors;
            colors.selectedColor = new Color(1f, 0.8f, 0.35f);
            colors.highlightedColor = new Color(1f, 0.9f, 0.6f);
            button.colors = colors;
            button.onClick.AddListener(action);
            if (caption.Length > 0)
            {
                var text = Label("Label", rect, caption, 20, Ink);
                Stretch(text.rectTransform, new Vector2(10, 0), new Vector2(-10, 0));
                text.alignment = TextAnchor.MiddleCenter;
            }
            buttons.Add(button);
            return button;
        }

        private Text Label(string name, Transform parent, string text, int size, Color color)
        {
            var label = Rect(name, parent).gameObject.AddComponent<Text>();
            label.font = font;
            label.fontSize = size;
            label.text = text;
            label.color = color;
            label.supportRichText = false;
            label.raycastTarget = false;
            label.alignment = TextAnchor.MiddleLeft;
            return label;
        }

        private static RectTransform Rect(string name, Transform parent)
        {
            var rect = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            return rect;
        }

        private static void Stretch(RectTransform rect, Vector2 min, Vector2 max)
        { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = min; rect.offsetMax = max; }

        private static void Anchor(RectTransform rect, Vector2 anchor, Vector2 offset, Vector2 size)
        { rect.anchorMin = rect.anchorMax = rect.pivot = anchor; rect.anchoredPosition = offset; rect.sizeDelta = size; }

        private static void FooterAction(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.right;
            rect.pivot = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = new Vector2(-176, 48);
        }
    }
}
