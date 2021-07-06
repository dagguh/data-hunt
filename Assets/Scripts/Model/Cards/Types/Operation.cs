using model.steal;
using model.zones;
using System.Collections.Generic;

namespace model.cards.types
{
    public class Operation : IType
    {
        bool IType.Corp => true;
        bool IType.Runner => false;
        bool IType.Playable => true;
        bool IType.Installable => false;
        bool IType.Rezzable => false;
        IList<IInstallDestination> IType.FindInstallDestinations() => new List<IInstallDestination>();
        IList<IStealOption> IType.DefaultStealing(Card card) => new List<IStealOption>();
    }
}
