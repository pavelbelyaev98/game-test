using System;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace SomethingDownThere
{
    internal static class ToolkitMenuComponents
    {
        public static Button Button(VisualElement parent, string name, string caption, Action action, string variant = "", bool enabled = true)
        {
            var button = new Button(action) { name = name, text = caption };
            button.AddToClassList("menu-button");
            foreach (string token in variant.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)) button.AddToClassList(token);
            button.SetEnabled(enabled); parent.Add(button); return button;
        }
    }

    internal sealed class ToolkitTabs
    {
        private readonly List<Button> buttons;
        public ToolkitTabs(VisualElement root, Action<int> selected)
        {
            buttons = root.Query<Button>().ToList();
            for (int i = 0; i < buttons.Count; i++)
            {
                int index = i;
                buttons[i].clicked += () => selected(index);
            }
        }
        public void Select(int index)
        {
            for (int i = 0; i < buttons.Count; i++) buttons[i].EnableInClassList("selected-tab", i == index);
        }
        public void AddNavigation(List<VisualElement> controls) => controls.AddRange(buttons);
    }

    // The shared menu frame becomes a compact dialog; all dialogs use its single heading,
    // message area and standard action components rather than introducing another layout.
    internal sealed class ToolkitDialog
    {
        private readonly Label title, subtitle;
        private readonly VisualElement body;
        public ToolkitDialog(Label title, Label subtitle, VisualElement body) { this.title = title; this.subtitle = subtitle; this.body = body; }
        public Label Set(string heading, string message)
        {
            title.text = heading; subtitle.text = ""; body.Clear();
            var label = new Label(message) { name = "Body", enableRichText = false, pickingMode = PickingMode.Ignore };
            label.AddToClassList("body"); body.Add(label); return label;
        }
    }
}
