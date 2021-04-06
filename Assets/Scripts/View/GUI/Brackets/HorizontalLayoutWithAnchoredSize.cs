using UnityEngine;
using UnityEngine.UI;

namespace view.gui.brackets
{
    public class HorizontalLayoutWithAnchoredSize : MonoBehaviour, ILayoutGroup
    {
        public void LayOutHorizontally()
        {
            var offset = 0.0f;
            foreach (Transform child in gameObject.transform)
            {
                if (!child.gameObject.activeSelf)
                {
                    continue;
                }
                if (child.gameObject.TryGetComponent<RectTransform>(out var rect))
                {
                    rect.offsetMin = new Vector2(offset, 0);
                    rect.offsetMax = new Vector2(offset, 0);
                    UnityEngine.Debug.Log("Rect width: " + rect.rect.width);
                    offset += rect.rect.width;
                }
            }
            UnityEngine.Debug.Log("Shifted for max offset of " + offset);
        }

        void ILayoutController.SetLayoutHorizontal()
        {
            LayOutHorizontally();
        }

        void ILayoutController.SetLayoutVertical()
        {
            LayOutHorizontally();
        }
    }
}
