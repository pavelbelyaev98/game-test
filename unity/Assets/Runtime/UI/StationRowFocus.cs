using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SomethingDownThere
{
    public sealed class StationRowFocus : MonoBehaviour, ISelectHandler
    {
        private ScrollRect scroll;
        public void Configure(ScrollRect owner) => scroll = owner;

        public void OnSelect(BaseEventData eventData)
        {
            if (scroll == null) return;
            Canvas.ForceUpdateCanvases();
            var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(scroll.viewport, transform);
            var view = scroll.viewport.rect;
            float shift = bounds.min.y < view.yMin ? view.yMin - bounds.min.y
                : bounds.max.y > view.yMax ? view.yMax - bounds.max.y : 0;
            var position = scroll.content.anchoredPosition;
            position.y = Mathf.Clamp(position.y + shift, 0, Mathf.Max(0, scroll.content.rect.height - view.height));
            scroll.StopMovement();
            scroll.content.anchoredPosition = position;
        }
    }
}
