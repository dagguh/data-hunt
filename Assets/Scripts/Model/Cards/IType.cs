using System.Collections.Generic;
using model.steal;
using model.zones;

namespace model.cards
{
    public interface IType
    {
        bool Corp { get; }
        bool Runner { get; }
        bool Playable { get; }
        bool Installable { get; }
        bool Rezzable { get; }
        IList<IInstallDestination> FindInstallDestinations();
        IList<IStealOption> DefaultStealing(Card card);
    }
}
