using System.Collections.Generic;
using model.steal;
using model.zones;

namespace model.cards.types
{
    public class Asset : IType
    {
        bool IType.Corp => true;
        bool IType.Runner => false;
        bool IType.Playable => false;
        bool IType.Installable => true;
        bool IType.Rezzable => true;
        IList<IInstallDestination> IType.FindInstallDestinations() => game.corp.zones.RemoteInstalls();
        IList<IStealOption> IType.DefaultStealing(Card card) => new List<IStealOption>();
        private Game game;
        public Asset(Game game) => this.game = game;
    }
}
