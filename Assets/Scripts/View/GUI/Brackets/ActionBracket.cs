using model.play;
using model.timing;
using System.Threading.Tasks;

namespace view.gui.brackets
{
    public class ActionBracket
    {
        private readonly ITurn turn;
        private readonly int actionNumber;
        private readonly Bracket bracket;

        public ActionBracket(ITurn turn, int actionNumber, Bracket bracket)
        {
            this.turn = turn;
            this.actionNumber = actionNumber;
            this.bracket = bracket;
            turn.TakingAction += UpdateActivation;
            turn.ActionTaken += UpdateEffect;
        }

        async private Task UpdateActivation(ITurn turn)
        {
            if (IsPresent())
            {
                bracket.Open();
            }
            await Task.CompletedTask;
        }

        private bool IsPresent()
        {
            return turn.Clicks.Spent == actionNumber;
        }

        private bool IsPast()
        {
            return turn.Clicks.Spent >= actionNumber;
        }

        async private Task UpdateEffect(ITurn turn, Ability action)
        {
            bracket.Collapse();
            // action.effect.Graphics;
            await Task.CompletedTask;
        }
    }
}