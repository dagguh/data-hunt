using model;
using model.timing;
using UnityEngine;

namespace view.gui.brackets
{
    class RunnerGameBracket
    {
        private Bracket bracket;

        public RunnerGameBracket(GameObject container, Game game)
        {
            bracket = new Bracket("Game", container);
            container.AddComponent<HorizontalLayoutWithAnchoredSize>();
            game.CurrentTurn += DisplayTurn;
        }

        private void DisplayTurn(ITurn turn)
        {
            switch (turn.Side)
            {
                case Side.RUNNER: DisplayRunnerTurn(turn); break;
                case Side.CORP: DisplayCorpTurn(turn); break;
            }
        }

        private void DisplayRunnerTurn(ITurn turn)
        {
            var turnContainer = bracket.Nest("Runner turn");
            new TurnBracket(turnContainer, turn);
        }

        private void DisplayCorpTurn(ITurn turn)
        {
            var turnContainer = bracket.Nest("Corp turn");
            new TurnBracket(turnContainer, turn);
        }
    }
}
