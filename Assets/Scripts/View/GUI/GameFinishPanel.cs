using model.timing;
using UnityEngine;
using UnityEngine.UI;

namespace view.gui
{
    public class GameFinishPanel : MonoBehaviour
    {
        public void PopUp(GameFinish finish)
        {
            gameObject.AddComponent<Image>();
            var message = new GameObject("Game finish message");
            var text = message.AddComponent<Text>();
            text.text = finish.winner + " wins!\nBecause " + finish.reason;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.rectTransform.anchorMin = new Vector2(0.20f, 0.20f);
            text.rectTransform.anchorMax = new Vector2(0.80f, 0.80f);
            message.AttachTo(gameObject);
        }
    }
}
