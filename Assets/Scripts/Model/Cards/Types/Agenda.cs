using System.Collections.Generic;
using model.steal;
using model.zones;

namespace model.cards.types
{
    public class Agenda : IType
    {
        bool IType.Corp => true;
        bool IType.Runner => false;
        bool IType.Playable => false;
        bool IType.Installable => true;
        bool IType.Rezzable => false;
        IList<IInstallDestination> IType.FindInstallDestinations() => game.corp.zones.RemoteInstalls();
        IList<IStealOption> IType.DefaultStealing(Card card) => new List<IStealOption> { game.runner.Stealing.MustSteal(card, printedAgendaPoints) };
        private int printedAgendaPoints;
        private Game game;

        public Agenda(int printedAgendaPoints, Game game)
        {
            this.printedAgendaPoints = printedAgendaPoints;
            this.game = game;
        }
    }
}
