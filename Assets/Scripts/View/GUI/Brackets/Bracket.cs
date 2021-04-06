using UnityEngine;
using UnityEngine.UI;

namespace view.gui.brackets
{
    public class Bracket
    {
        private static Font FONT = Resources.Load<Font>("Fonts/cyberdyne");
        private static float EDGE_WIDTH_RATIO = 0.05f;

        private readonly GameObject container;
        private GameObject opening;
        private GameObject content;
        private GameObject closing;
        private float margin;

        public Bracket(string name, GameObject container) : this(name, container, siblingIndex: 0, margin: 0.00f) { }

        private Bracket(string name, GameObject container, int siblingIndex, float margin)
        {
            this.container = container;
            this.margin = margin;
            opening = RenderOpening(siblingIndex, name);
            content = RenderContent(siblingIndex + 1);
            closing = RenderClosing(siblingIndex + 2);
            Collapse();
        }

        private GameObject RenderOpening(int siblingIndex, string name)
        {
            var gameObject = new GameObject("Opening").AttachTo(container);
            gameObject.transform.SetSiblingIndex(siblingIndex);
            var image = gameObject.AddComponent<Image>();
            image.color = Color.magenta;
            var rect = gameObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.00f, margin);
            rect.anchorMax = new Vector2(EDGE_WIDTH_RATIO, 1.00f - margin);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            var label = new GameObject("Label").AttachTo(gameObject);
            var text = label.AddComponent<Text>();
            text.font = FONT;
            text.supportRichText = false;
            text.alignByGeometry = true;
            text.alignment = TextAnchor.MiddleCenter;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.raycastTarget = false;
            text.maskable = false;
            text.text = name;
            label.GetComponent<RectTransform>().Expand();
            label.transform.rotation *= Quaternion.Euler(0.0f, 0.0f, 90.0f);
            return gameObject;
        }

        private GameObject RenderContent(int siblingIndex)
        {
            var gameObject = new GameObject("Content").AttachTo(container);
            gameObject.transform.SetSiblingIndex(siblingIndex);
            var image = gameObject.AddComponent<Image>();
            image.color = Color.white - new Color(0, 0, 0, 0.50f);
            var rect = gameObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.00f, margin);
            rect.anchorMax = new Vector2(0.30f, 1.00f - margin);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return gameObject;
        }

        private GameObject RenderClosing(int siblingIndex)
        {
            var gameObject = new GameObject("Closing").AttachTo(container);
            gameObject.transform.SetSiblingIndex(siblingIndex);
            var image = gameObject.AddComponent<Image>();
            image.color = Color.cyan;
            var rect = gameObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.00f, margin);
            rect.anchorMax = new Vector2(EDGE_WIDTH_RATIO, 1.00f - margin);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return gameObject;
        }

        public Bracket Nest(string name)
        {
            var nestedIndex = closing.transform.GetSiblingIndex() - 1;
            return new Bracket(name, container, nestedIndex, margin + 0.08f);
        }

        public void Open()
        {
            content.SetActive(true);
        }

        public void Collapse()
        {
            content.SetActive(false);
        }
    }
}
