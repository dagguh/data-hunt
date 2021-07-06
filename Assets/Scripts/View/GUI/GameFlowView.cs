using controller;
using model;
using model.timing;
using UnityEngine;
using view.gui.timecross;
using static view.gui.GameObjectExtensions;

namespace view.gui
{
    public class GameFlowView
    {
        public DropZone PaidChoice { get; private set; }
        public TimeCross TimeCross { get; private set; }

        public void Display(GameObject board, Game game)
        {
            var dayNight = new DayNightCycle();
            dayNight.Wire(game);
            TimeCross = new TimeCross(game, dayNight);
            WirePaidWindow(game.runner.paidWindow);
            var gameFinish = CreateGameFinish(game);
            gameFinish.AttachTo(board);
        }

        private PaidWindowView WirePaidWindow(PaidWindow window)
        {
            var pass = FindOrFail("Pass").AddComponent<PaidWindowPass>();
            PaidChoice = FindOrFail("Paid choice").AddComponent<DropZone>();
            return new PaidWindowView(window, pass, PaidChoice);
        }

        private GameObject CreateGameFinish(Game game)
        {
            var view = new GameObject("Game finish");
            var rectangle = view.AddComponent<RectTransform>();
            rectangle.anchorMin = new Vector2(0.30f, 0.30f);
            rectangle.anchorMax = new Vector2(0.70f, 0.70f);
            rectangle.offsetMin = Vector2.zero;
            rectangle.offsetMax = Vector2.zero;
            game.Timing.Finished += view.AddComponent<GameFinishPanel>().PopUp;
            return view;
        }
    }
}
