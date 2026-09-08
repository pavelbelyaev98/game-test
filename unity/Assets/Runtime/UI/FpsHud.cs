using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace SomethingDownThere
{
    [DisallowMultipleComponent, RequireComponent(typeof(FpsPlayer))]
    public sealed class FpsHud : MonoBehaviour
    {
        private FpsPlayer player;
        private GameObject canvasRoot, eventRoot, menuRoot;
        private Text reticle, status, prompt, feedback, menuTitle, menuBody;
        private Text shovelStatus, adminHint;
        private UnityEngine.UI.Text batteryStatus, returnWarning;
        private UnityEngine.UI.Image batteryFill;
        private RectTransform xrayRoot;
        private readonly List<Text> xrayMarkers = new List<Text>();
        private RectTransform commandsRoot;
        private readonly List<Button> commandButtons = new List<Button>();
        private Font font;
        private bool rebuildPending;
        private InputActionAsset uiActions;
        private readonly List<InputActionReference> uiReferences = new List<InputActionReference>();

        private void Start()
        {
            player = GetComponent<FpsPlayer>();
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Build();
            player.MenuChanged += QueueRebuild;
            RebuildMenu();
        }

        private void LateUpdate()
        {
            if (player == null || player.Battery == null) return;
            if (rebuildPending) RebuildMenu();
            bool gameplay = !player.IsMenuOpen;
            reticle.gameObject.SetActive(gameplay);
            prompt.text = gameplay ? player.TargetPrompt : "";
            feedback.text = player.Feedback;
            status.text = "FINDS  " + player.Inventory.Count + " / " + player.Inventory.Capacity;
            if (player.GameplayActive) UpdateBattery();
            returnWarning.gameObject.SetActive(gameplay);
            bool digging = player.ExcavationAvailable;
            shovelStatus.gameObject.SetActive(digging);
            shovelStatus.text = $"SHOVEL {player.EffectiveShovelLevel} / {player.Shovel.LevelCount}    |    {player.EffectiveShovel.Radius * 2:F2} m scoop"
                + $"\nREACH {player.EffectiveDigReach:F1} m    |    DEPTH {player.Depth:F1} m";
            adminHint.text = !player.AdminAvailable || !gameplay ? ""
                : "DEVELOPER ADMIN"
                    + (player.HasAdminOverrides ? "  |  Overrides active" : "")
                    + (player.UnlimitedBattery ? "  |  Unlimited battery" : "");
            reticle.rectTransform.localScale = Vector3.one * (1 + player.DigPulse * 0.3f);
            reticle.color = Color.Lerp(Color.white, new Color(1, 0.82f, 0.35f), player.DigPulse);
            UpdateXray(gameplay && player.AdminXray);
        }

        private void UpdateBattery()
        {
            float fraction = player.Battery.Charge / player.Battery.Capacity;
            var risk = player.ReturnWarning.Evaluate(player.Battery);
            bool unlimited = player.UnlimitedBattery;
            Color color = unlimited ? new Color(0.55f, 0.88f, 1f)
                : risk == ReturnRisk.Critical ? new Color(1f, 0.7f, 0.64f)
                : risk == ReturnRisk.Risky ? new Color(1f, 0.8f, 0.35f) : new Color(0.72f, 0.94f, 0.83f);
            string band = unlimited ? "UNLIMITED" : fraction <= 0 ? "EMPTY"
                : risk == ReturnRisk.Critical ? "CRITICAL" : risk == ReturnRisk.Risky ? "RISKY" : "SAFE";
            batteryStatus.text = "BATTERY  " + Mathf.CeilToInt(100f * player.Battery.Charge / player.Battery.Capacity) + "%  |  " + band;
            batteryStatus.color = batteryFill.color = color;
            batteryFill.rectTransform.anchorMax = new Vector2(unlimited ? 1f : fraction, 1);
            returnWarning.color = new Color(0.96f, 0.98f, 0.97f);
            var recharge = player.SurfaceRecharge;
            if (recharge != null && recharge.RecentlyRecharged)
                returnWarning.text = "FULLY RECHARGED";
            else if (recharge != null && recharge.IsPlayerInZone)
                returnWarning.text = "SURFACE RECHARGE";
            else if (unlimited) returnWarning.text = "";
            else if (fraction <= 0)
                returnWarning.text = "NO POWER FOR DIGGING OR FLIGHT";
            else if (risk == ReturnRisk.Critical)
                returnWarning.text = "CHARGE CRITICAL";
            else if (risk == ReturnRisk.Risky)
                returnWarning.text = "RESERVE RUNNING LOW";
            else returnWarning.text = "";
            if (!unlimited && fraction < 1f && recharge != null && recharge.IsNearby && !recharge.IsPlayerInZone)
                returnWarning.text = "SURFACE RECHARGE";
        }

        private void UpdateXray(bool visible)
        {
            xrayRoot.gameObject.SetActive(visible);
            if (!visible) return;
            var finds = player.Discoveries.Finds;
            while (xrayMarkers.Count < finds.Count)
            {
                var marker = Label(xrayRoot, "Buried find marker", "o", Vector2.zero, Vector2.zero,
                    new Vector2(34, 34), 23, TextAnchor.MiddleCenter);
                marker.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                xrayMarkers.Add(marker);
            }
            for (int i = 0; i < xrayMarkers.Count; i++)
            {
                var find = i < finds.Count ? finds[i] : null;
                Vector3 view = find == null ? Vector3.zero : player.ViewCamera.WorldToViewportPoint(find.transform.position);
                bool show = find != null && find.isActiveAndEnabled && !find.Collected && view.z > 0
                    && Vector3.Distance(player.ViewCamera.transform.position, find.transform.position) <= 18
                    && view.x > 0.02f && view.x < 0.98f && view.y > 0.12f && view.y < 0.85f;
                var marker = xrayMarkers[i];
                marker.gameObject.SetActive(show);
                if (!show) continue;
                marker.rectTransform.anchoredPosition = new Vector2(view.x * xrayRoot.rect.width, view.y * xrayRoot.rect.height);
                marker.color = find.Exposure > 0 && find.Collectible ? new Color(0.5f, 1f, 0.48f) : new Color(0.3f, 0.94f, 1f, 0.9f);
            }
        }

        private void QueueRebuild() => rebuildPending = true;

        private void Build()
        {
            canvasRoot = new GameObject("FPS HUD", typeof(RectTransform), typeof(Canvas),
                typeof(HudCanvasScaler), typeof(GraphicRaycaster));
            canvasRoot.transform.SetParent(transform, false);
            var canvas = canvasRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            var scaler = canvasRoot.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;

            xrayRoot = new GameObject("Admin X-ray", typeof(RectTransform)).GetComponent<RectTransform>();
            xrayRoot.SetParent(canvasRoot.transform, false);
            xrayRoot.anchorMin = Vector2.zero;
            xrayRoot.anchorMax = Vector2.one;
            xrayRoot.offsetMin = xrayRoot.offsetMax = Vector2.zero;
            xrayRoot.gameObject.SetActive(false);

            reticle = Label(canvasRoot.transform, "Reticle", "+", new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(30, 30), 24, TextAnchor.MiddleCenter);
            status = Label(canvasRoot.transform, "Status", "", new Vector2(0, 1),
                new Vector2(355, -24), new Vector2(220, 36), 18, TextAnchor.MiddleLeft);
            batteryStatus = Label(canvasRoot.transform, "Battery status", "", new Vector2(0, 1),
                new Vector2(24, -24), new Vector2(325, 36), 18, TextAnchor.MiddleLeft);
            var track = new GameObject("Battery reserve", typeof(RectTransform), typeof(UnityEngine.UI.Image));
            track.transform.SetParent(canvasRoot.transform, false);
            Place(track.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(24, -61), new Vector2(300, 4));
            track.GetComponent<UnityEngine.UI.Image>().color = new Color(0.025f, 0.05f, 0.06f, 0.8f);
            track.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
            var fill = new GameObject("Charge", typeof(RectTransform), typeof(UnityEngine.UI.Image));
            fill.transform.SetParent(track.transform, false);
            batteryFill = fill.GetComponent<UnityEngine.UI.Image>();
            batteryFill.raycastTarget = false;
            batteryFill.rectTransform.anchorMin = Vector2.zero;
            batteryFill.rectTransform.anchorMax = Vector2.one;
            batteryFill.rectTransform.offsetMin = batteryFill.rectTransform.offsetMax = Vector2.zero;
            returnWarning = Label(canvasRoot.transform, "Return warning", "", new Vector2(0, 1),
                new Vector2(24, -158), new Vector2(490, 62), 18, TextAnchor.UpperLeft);
            prompt = Label(canvasRoot.transform, "Target", "", new Vector2(0.5f, 0.5f),
                new Vector2(0, -48), new Vector2(600, 36), 19, TextAnchor.MiddleCenter);
            feedback = Label(canvasRoot.transform, "Feedback", "", new Vector2(0.5f, 0),
                new Vector2(0, 100), new Vector2(700, 44), 18, TextAnchor.MiddleCenter);
            shovelStatus = Label(canvasRoot.transform, "Shovel status", "", new Vector2(0, 1),
                new Vector2(24, -82), new Vector2(550, 62), 18, TextAnchor.UpperLeft);
            adminHint = Label(canvasRoot.transform, "Developer controls", "", new Vector2(1, 1),
                new Vector2(-24, -24), new Vector2(460, 94), 16, TextAnchor.UpperRight);

            menuRoot = new GameObject("Menu", typeof(RectTransform), typeof(Image));
            menuRoot.transform.SetParent(canvasRoot.transform, false);
            Place(menuRoot.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(620, 540));
            menuRoot.GetComponent<Image>().color = new Color(0.035f, 0.055f, 0.065f, 0.98f);
            menuTitle = Label(menuRoot.transform, "Title", "", new Vector2(0, 1),
                new Vector2(28, -20), new Vector2(560, 45), 28, TextAnchor.MiddleLeft);
            var scrollRoot = new GameObject("Body scroll", typeof(RectTransform), typeof(Image), typeof(ScrollRect), typeof(RectMask2D));
            scrollRoot.transform.SetParent(menuRoot.transform, false);
            Place(scrollRoot.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(28, -75), new Vector2(564, 260));
            scrollRoot.GetComponent<Image>().color = new Color(0, 0, 0, 0.01f);
            menuBody = Label(scrollRoot.transform, "Body", "", new Vector2(0, 1),
                Vector2.zero, new Vector2(550, 260), 20, TextAnchor.UpperLeft);
            var bodySize = menuBody.gameObject.AddComponent<ContentSizeFitter>();
            bodySize.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            var scroll = scrollRoot.GetComponent<ScrollRect>();
            scroll.viewport = scrollRoot.GetComponent<RectTransform>();
            scroll.content = menuBody.rectTransform;
            scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 25;
            commandsRoot = new GameObject("Commands", typeof(RectTransform)).GetComponent<RectTransform>();
            commandsRoot.SetParent(menuRoot.transform, false);
            Place(commandsRoot, new Vector2(0, 0), new Vector2(28, 20), new Vector2(564, 168));

            if (EventSystem.current == null)
            {
                eventRoot = new GameObject("FPS Event System");
                eventRoot.SetActive(false);
                eventRoot.transform.SetParent(transform, false);
                eventRoot.AddComponent<EventSystem>();
                var module = eventRoot.AddComponent<InputSystemUIInputModule>();
                uiActions = ScriptableObject.CreateInstance<InputActionAsset>();
                var ui = uiActions.AddActionMap("UI");
                var navigate = ui.AddAction("Navigate", InputActionType.Value);
                navigate.AddCompositeBinding("2DVector")
                    .With("Up", "<Keyboard>/upArrow").With("Down", "<Keyboard>/downArrow")
                    .With("Left", "<Keyboard>/leftArrow").With("Right", "<Keyboard>/rightArrow");
                module.actionsAsset = uiActions;
                module.move = Reference(navigate);
                module.submit = Reference(ui.AddAction("Submit", InputActionType.Button, "<Keyboard>/enter"));
                module.point = Reference(ui.AddAction("Point", InputActionType.PassThrough, "<Mouse>/position"));
                module.leftClick = Reference(ui.AddAction("Click", InputActionType.PassThrough, "<Mouse>/leftButton"));
                module.scrollWheel = Reference(ui.AddAction("Scroll", InputActionType.PassThrough, "<Mouse>/scroll"));
                module.cancel = null; // Escape belongs exclusively to the player's menu state.
                eventRoot.SetActive(true);
            }
        }

        private InputActionReference Reference(InputAction action)
        {
            var reference = InputActionReference.Create(action);
            uiReferences.Add(reference);
            return reference;
        }

        private void RebuildMenu()
        {
            rebuildPending = false;
            foreach (var button in commandButtons)
            {
                button.gameObject.SetActive(false);
                Destroy(button.gameObject);
            }
            commandButtons.Clear();
            menuRoot.SetActive(player.IsMenuOpen);
            if (!player.IsMenuOpen) return;
            bool admin = player.Menu == PlayerMenu.DeveloperAdmin;
            menuRoot.GetComponent<RectTransform>().sizeDelta = admin ? new Vector2(700, 640) : new Vector2(620, 540);
            menuBody.transform.parent.GetComponent<RectTransform>().sizeDelta = admin ? new Vector2(644, 130) : new Vector2(564, 260);
            menuBody.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, admin ? 630 : 550);
            commandsRoot.sizeDelta = admin ? new Vector2(644, 370) : new Vector2(564, 168);

            menuTitle.text = player.Menu == PlayerMenu.Pause ? "Paused"
                : admin ? "Developer admin"
                : player.Menu == PlayerMenu.ConfirmRescue ? "Call rescue?"
                : player.Menu == PlayerMenu.ConfirmTerrainReset ? "Reset the excavation?"
                : player.Menu == PlayerMenu.Inventory ? "Inventory" : player.Station?.Title ?? "Station unavailable";
            if (player.Menu == PlayerMenu.Pause)
            {
                menuBody.text = "WASD Move   |   Mouse Look   |   LMB Dig / collect\nSpace Jump; keep holding for Jetpack\nE Interact   |   Tab Inventory\nEsc Pause / release mouse";
                AddButton("Resume", true, player.CloseMenu);
                if (player.RescueAvailable) AddButton("Call rescue...", true, player.RequestRescue);
                if (player.AdminAvailable) AddButton("Developer admin  /  Ctrl+Shift+F10", true, player.ShowAdminMenu);
            }
            else if (admin)
            {
                menuBody.text = $"Shovel {player.EffectiveShovelLevel}: {player.EffectiveDigReach:F1} m reach / {player.EffectiveShovel.Radius * 2:F2} m scoop\n"
                    + "Ctrl+Shift: 1-6 Shovel / R Refill / Home Return / X X-ray\n"
                    + "Overrides last this session; owned upgrades are kept.\n"
                    + $"This site: {player.SuccessfulStrokes} strokes, {player.ExcavatedVolume:F1} m3 removed.";
                for (int i = 1; i <= player.Shovel.LevelCount; i++)
                {
                    int level = i;
                    AddButton($"{(i == player.EffectiveShovelLevel ? "Selected: " : "")}Shovel {i} / {player.DigReachAtLevel(i):F1} m reach",
                        true, () => player.SelectAdminLevel(level));
                }
                AddButton("Refill battery", true, player.RefillAdminBattery);
                AddButton("Return to surface", true, player.AdminReturnToSurface);
                AddButton("Reset ground...", true, player.RequestTerrainReset);
                AddButton("Unlimited battery: " + (player.UnlimitedBattery ? "ON" : "OFF"), true, player.ToggleAdminUnlimitedBattery);
                AddButton("X-ray: " + (player.AdminXray ? "ON" : "OFF"), player.Discoveries != null, player.ToggleAdminXray);
                AddButton("Restore normal rules", player.HasAdminOverrides, player.RestoreAdminOverrides);
                AddButton("Resume digging", true, player.CloseMenu);
            }
            else if (player.Menu == PlayerMenu.ConfirmRescue)
            {
                var quote = player.Rescue.Quote;
                menuBody.text = string.IsNullOrEmpty(player.RescueNotice) ? "" : player.RescueNotice + "\n\n";
                if (quote != null)
                {
                    menuBody.text += $"Carried finds lost: {quote.LostItems.Count}  |  Sale value: {quote.LostSaleValue}\n"
                        + $"Rescue fee: {quote.Fee} credits\nBalance afterward: {quote.RemainingBalance} credits\n\n"
                        + "Excavation and shovel upgrades are kept.\nYou return to the surface with a full battery.";
                    if (quote.LostItems.Count > 0)
                    {
                        menuBody.text += "\n\nFinds you will lose:\n";
                        foreach (var item in quote.LostItems) menuBody.text += item.DisplayName + "\n";
                    }
                }
                AddButton("Cancel", true, player.CancelRescue);
                AddButton("Confirm rescue", quote != null, () => player.ConfirmRescue());
            }
            else if (player.Menu == PlayerMenu.ConfirmTerrainReset)
            {
                menuBody.text = "This fills every hole in this site and returns you\nto the safe surface. Your excavation will be lost.\n\nYour inventory and owned shovel level are kept.\nThe battery is refilled.";
                AddButton("Keep excavation", true, player.CancelTerrainReset);
                AddButton("Reset ground", true, () => player.ConfirmTerrainReset());
            }
            else
            {
                menuBody.text = player.Menu == PlayerMenu.Station && player.Station != null
                    ? player.Station.Description(player) + "\n\n" : "";
                menuBody.text += "Carried finds: " + player.Inventory.Count + " / " + player.Inventory.Capacity + "\n\n";
                if (player.Inventory.Count == 0) menuBody.text += "No carried finds.";
                else
                {
                    foreach (var item in player.Inventory.Items)
                        menuBody.text += item.DisplayName + "  |  Sale value: " + item.SaleValue + "\n";
                }
                if (player.Menu == PlayerMenu.Station && player.Station != null)
                {
                    for (int i = 0; i < player.Station.CommandCount; i++)
                    {
                        int command = i;
                        AddButton(player.Station.CommandLabel(i, player), player.Station.CanExecute(i, player),
                            () => player.ExecuteStationCommand(command));
                    }
                }
            }
            if (player.Menu == PlayerMenu.Inventory || player.Menu == PlayerMenu.Station)
                AddButton("Close", true, player.CloseMenu);
            menuBody.rectTransform.anchoredPosition = Vector2.zero;
            foreach (var button in commandButtons)
            {
                if (!button.interactable) continue;
                if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(button.gameObject);
                break;
            }
        }

        private void AddButton(string label, bool interactable, UnityEngine.Events.UnityAction callback)
        {
            var root = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button));
            root.transform.SetParent(commandsRoot, false);
            bool admin = player.Menu == PlayerMenu.DeveloperAdmin;
            int index = commandButtons.Count;
            float width = admin ? (index == 12 ? 644 : 314) : 564;
            Vector2 offset = admin ? new Vector2(index % 2 * 330, -(index / 2) * 54) : new Vector2(0, -index * 54);
            Place(root.GetComponent<RectTransform>(), new Vector2(0, 1), offset, new Vector2(width, 46));
            root.GetComponent<Image>().color = new Color(0.22f, 0.29f, 0.3f);
            var button = root.GetComponent<Button>();
            button.interactable = interactable;
            var colors = button.colors;
            colors.selectedColor = new Color(1f, 0.8f, 0.35f);
            colors.highlightedColor = new Color(1f, 0.9f, 0.6f);
            button.colors = colors;
            button.onClick.AddListener(callback);
            Label(root.transform, "Label", label, new Vector2(0.5f, 0.5f), Vector2.zero,
                new Vector2(width - 16, 44), admin ? 18 : 20, TextAnchor.MiddleCenter);
            commandButtons.Add(button);
        }

        private Text Label(Transform parent, string name, string content, Vector2 anchor, Vector2 offset,
            Vector2 size, int fontSize, TextAnchor alignment)
        {
            var root = new GameObject(name, typeof(RectTransform), typeof(Text));
            root.transform.SetParent(parent, false);
            Place(root.GetComponent<RectTransform>(), anchor, offset, size);
            var text = root.GetComponent<Text>();
            text.font = font;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = Color.white;
            text.supportRichText = false;
            text.raycastTarget = false;
            text.text = content;
            var shadow = root.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0, 0.65f);
            shadow.effectDistance = new Vector2(1, -1);
            shadow.useGraphicAlpha = true;
            return text;
        }

        private static void Place(RectTransform rect, Vector2 anchor, Vector2 offset, Vector2 size)
        {
            rect.anchorMin = rect.anchorMax = rect.pivot = anchor;
            rect.anchoredPosition = offset;
            rect.sizeDelta = size;
        }

        private void OnDestroy()
        {
            if (player != null) player.MenuChanged -= QueueRebuild;
            if (canvasRoot != null) Destroy(canvasRoot);
            if (eventRoot != null) Destroy(eventRoot);
            foreach (var reference in uiReferences) if (reference != null) Destroy(reference);
            if (uiActions != null)
            {
                uiActions.Disable();
                Destroy(uiActions);
            }
        }
    }
}
