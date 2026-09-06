using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class YardTargeting : MonoBehaviour
    {
        public Camera view;
        public LayerMask collisionMask = Physics.DefaultRaycastLayers;
        [Min(.1f)] public float reach = 3f;
        [Min(.01f)] public float radius = .25f;
        public YardTarget Current { get; private set; }

        public void Refresh()
        {
            // The first solid hit wins, including non-target scenery, so prompts cannot pass through walls.
            var ray = new Ray(view.transform.position, view.transform.forward);
            YardTarget next = null;
            if (Physics.SphereCast(ray, radius, out var hit, reach, collisionMask, QueryTriggerInteraction.Ignore))
                next = hit.collider.GetComponentInParent<YardTarget>();
            SetTarget(next);
        }

        public void Clear() => SetTarget(null);

        void SetTarget(YardTarget target)
        {
            if (Current == target) return;
            if (Current != null) Current.SetHighlighted(false);
            Current = target;
            if (Current != null) Current.SetHighlighted(true);
        }

        void OnDisable() => Clear();
    }
}
