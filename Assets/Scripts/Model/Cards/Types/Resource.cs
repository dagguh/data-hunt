using System.Collections.Generic;
using model.steal;
using model.zones;

namespace model.cards.types
{
    public class Resource : IType
    {
        bool IType.Corp => false;
        bool IType.Runner => true;
        bool IType.Playable => false;
        bool IType.Installable => true;
        bool IType.Rezzable => false;
        IList<IInstallDestination> IType.FindInstallDestinations() => new List<IInstallDestination>() { game.runner.zones.rig };
        IList<IStealOption> IType.DefaultStealing(Card card) => new List<IStealOption>();
        private Game game;
        public Resource(Game game) => this.game = game;
    }
}
