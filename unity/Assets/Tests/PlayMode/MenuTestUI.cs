using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace SomethingDownThere.Tests
{
    internal static class MenuTestUI
    {
        public static GameMenuView View(FpsPlayer player) => player.GetComponent<FpsHud>().Menus;
        public static string Focused(FpsPlayer player) => (View(player).Focused as VisualElement)?.name;
        public static Button Button(FpsPlayer player, string name) => View(player).CurrentScreen.Query<Button>().ToList().Single(b => b.name == name);
        public static string Text(FpsPlayer player, string name)
        {
            var element = View(player).Root.Q(name);
            return element is Label label ? label.text : string.Join("\n", element.Query<Label>().ToList().Select(l => l.text));
        }
        public static void Click(Button button)
        {
            if (button.panel == null || !button.enabledInHierarchy) return;
            // Navigation events dispatch to panel focus, irrespective of SendEvent's receiver.
            button.Focus();
            using (var submit = NavigationSubmitEvent.GetPooled()) button.SendEvent(submit);
        }
        public static Vector2 ScreenPoint(FpsPlayer player, VisualElement element, float fraction = 0.5f)
        {
            var bounds = element.worldBound;
            float scale = Screen.width / View(player).Root.worldBound.width;
            return new Vector2(Mathf.Lerp(bounds.xMin, bounds.xMax, fraction) * scale, Screen.height - bounds.center.y * scale);
        }
    }
}
