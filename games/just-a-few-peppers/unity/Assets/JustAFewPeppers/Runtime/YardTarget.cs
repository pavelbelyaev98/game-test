using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class YardTarget : MonoBehaviour
    {
        public string displayName;
        public string description;
        public Renderer marker;
        MaterialPropertyBlock properties;

        public void SetHighlighted(bool highlighted)
        {
            if (marker == null) return;
            if (properties == null) properties = new MaterialPropertyBlock();
            properties.SetColor("_Color", highlighted ? new Color(1f, .83f, .35f) : new Color(.38f, .45f, .4f));
            marker.SetPropertyBlock(properties);
        }
    }
}
