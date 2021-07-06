using System.Threading.Tasks;
using model;
using model.timing;
using model.timing.corp;
using UnityEngine;

namespace view.gui.brackets
{
    class RunnerGameBracket
    {
        private Bracket bracket;
        private Timing timing;

        public RunnerGameBracket(GameObject container, Game game)
        {
            bracket = new Bracket("Game", container);
            container.AddComponent<HorizontalLayoutWithAnchoredSize>();
            game.Timing.CorpTurnDefined += DisplayCorpTurn;
        }

        private void DisplayCorpTurn(CorpTurn turn)
        {
            var turnContainer = bracket.Nest(turn.Name);
            turn.ActionPhaseDefined += DisplayCorpActionPhase;

        }

        private void DisplayCorpActionPhase(CorpActionPhase phase)
        {
            phase.ActionWindowDefined += DisplayCorpActionWindow;
            for (int i = 0; i < turn.Clicks.NextReplenishment; i++)
            {
                int actionOrder = i + 1;
                var actionBracket = turnContainer.Nest("Action " + actionOrder);
                new ActionBracket(turn, actionOrder, actionBracket);
            }
            new TurnBracket(turnContainer, turn);
            await Task.CompletedTask;
        }

        private void DisplayCorpActionWindow(CorpActionWindow window)
        {

        }

        async private Task DisplayRunnerTurn(ITurn turn)
        {
            var turnContainer = bracket.Nest("Runner turn " + turn.Number);
            new TurnBracket(turnContainer, turn);
            await Task.CompletedTask;
        }
    }
}
