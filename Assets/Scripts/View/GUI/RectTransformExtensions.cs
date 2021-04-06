using UnityEngine;

namespace view.gui
{
    public static class RectTransformExtensions
    {
        public static RectTransform AddCleanRectangle(this GameObject gameObject)
        {
            return gameObject.AddComponent<RectTransform>().Expand();
        }

        public static RectTransform Expand(this RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return rect;
        }
    }
}
