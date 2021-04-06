using model.timing;

namespace view.gui.brackets
{
    internal class TurnBracket
    {
        public TurnBracket(Bracket bracket, ITurn turn)
        {
            for (int i = 0; i < turn.Clicks.NextReplenishment; i++)
            {
                int actionOrder = i + 1;
                var actionBracket = bracket.Nest("Action " + actionOrder);
                new ActionBracket(turn, actionOrder, actionBracket);
            }
        }
    }
}
