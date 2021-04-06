using UnityEngine;
using UnityEngine.UI;
using model;
using model.timing;
using static view.gui.GameObjectExtensions;

namespace view.gui.timecross
{
    public class DayNightCycle
    {
        private Color daylight = new Color(38, 195, 219, 255) / 255;
        private Color midnight = new Color(23, 17, 44, 255) / 255;
        private Sprite dayCity = Resources.Load<Sprite>("Images/Background/photo-of-cityscape-on-a-gloomy-day-2137195");
        private Sprite nightCity = Resources.Load<Sprite>("Images/Background/high-rise-photography-of-city-2039630");
        private Image background = FindOrFail("Board").GetComponent<Image>();

        public void Wire(Game game)
        {
            game.CurrentTurn += UpdateBackground;
        }

        void UpdateBackground(ITurn turn)
        {
            switch (turn.Side)
            {
                case Side.CORP: background.sprite = dayCity; break;
                case Side.RUNNER: background.sprite = nightCity; break;
            }
        }

        public void Paint(Image background, Side side)
        {
            switch (side)
            {
                case Side.CORP: background.color = daylight; break;
                case Side.RUNNER: background.color = midnight; break;
            }
        }
    }
}
