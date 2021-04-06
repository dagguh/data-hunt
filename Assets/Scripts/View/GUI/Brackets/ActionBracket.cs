using model.play;
using model.timing;
using System.Threading.Tasks;

namespace view.gui.brackets
{
    public class ActionBracket
    {
        private readonly ITurn turn;
        private readonly int actionOrder;
        private readonly Bracket bracket;

        public ActionBracket(ITurn turn, int actionOrder, Bracket bracket)
        {
            this.turn = turn;
            this.actionOrder = actionOrder;
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
            return turn.Clicks.Spent == actionOrder - 1;
        }

        private bool IsPast()
        {
            return turn.Clicks.Spent >= actionOrder;
        }

        async private Task UpdateEffect(ITurn turn, Ability action)
        {
            if (IsPast())
            {
                bracket.Collapse();
            }
            // action.effect.Graphics;
            await Task.CompletedTask;
        }
    }
}