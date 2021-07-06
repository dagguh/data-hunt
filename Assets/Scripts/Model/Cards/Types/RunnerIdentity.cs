using System.Collections.Generic;
using model.steal;
using model.zones;

namespace model.cards.types
{
    public class RunnerIdentity : IType
    {
        bool IType.Corp => false;
        bool IType.Runner => true;
        bool IType.Playable => false;
        bool IType.Installable => false;
        bool IType.Rezzable => false;
        IList<IInstallDestination> IType.FindInstallDestinations() => new List<IInstallDestination>();
        IList<IStealOption> IType.DefaultStealing(Card card) => new List<IStealOption>();
    }
}
