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
        private Text reticle, status, prompt, feedback, hint, menuTitle, menuBody;
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
            hint.text = gameplay ? player.Hint : "";
            feedback.text = player.Feedback;
            status.text = "Battery " + Mathf.CeilToInt(player.Battery.Charge) + " / " + player.Battery.Capacity
                + "     Finds " + player.Inventory.Count + " / " + player.Inventory.Capacity;
        }

        private void QueueRebuild() => rebuildPending = true;

        private void Build()
        {
            canvasRoot = new GameObject("FPS HUD", typeof(RectTransform), typeof(Canvas),
                typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasRoot.transform.SetParent(transform, false);
            var canvas = canvasRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            var scaler = canvasRoot.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;

            reticle = Label(canvasRoot.transform, "Reticle", "+", new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(30, 30), 24, TextAnchor.MiddleCenter);
            status = Label(canvasRoot.transform, "Status", "", new Vector2(0, 1),
                new Vector2(24, -24), new Vector2(800, 40), 20, TextAnchor.MiddleLeft);
            prompt = Label(canvasRoot.transform, "Target", "", new Vector2(0.5f, 0.5f),
                new Vector2(0, -48), new Vector2(650, 40), 22, TextAnchor.MiddleCenter);
            feedback = Label(canvasRoot.transform, "Feedback", "", new Vector2(0.5f, 0),
                new Vector2(0, 100), new Vector2(800, 50), 20, TextAnchor.MiddleCenter);
            hint = Label(canvasRoot.transform, "Hint", "", new Vector2(0.5f, 0),
                new Vector2(0, 45), new Vector2(900, 45), 20, TextAnchor.MiddleCenter);

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

            menuTitle.text = player.Menu == PlayerMenu.Pause ? "Paused"
                : player.Menu == PlayerMenu.Inventory ? "Inventory" : player.Station?.Title ?? "Station unavailable";
            if (player.Menu == PlayerMenu.Pause)
            {
                menuBody.text = "WASD Move   |   Mouse Look\nLMB Dig   |   Hold Space Jetpack\nE Interact   |   Tab Inventory\nEsc Back / pause\n\nDigging and jetpack share a battery.\nReturn to the surface to recharge.";
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
            AddButton(player.Menu == PlayerMenu.Pause ? "Resume" : "Close", true, player.CloseMenu);
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
            Place(root.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(0, -commandButtons.Count * 54), new Vector2(564, 46));
            root.GetComponent<Image>().color = new Color(0.22f, 0.29f, 0.3f);
            var button = root.GetComponent<Button>();
            button.interactable = interactable;
            var colors = button.colors;
            colors.selectedColor = new Color(1f, 0.8f, 0.35f);
            colors.highlightedColor = new Color(1f, 0.9f, 0.6f);
            button.colors = colors;
            button.onClick.AddListener(callback);
            Label(root.transform, "Label", label, new Vector2(0.5f, 0.5f), Vector2.zero,
                new Vector2(540, 44), 20, TextAnchor.MiddleCenter);
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
