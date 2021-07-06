using System.Threading.Tasks;
using model.cards;

namespace model.zones {
    public interface IInstallDestination {
        Task Host(Card card);
        // CR: 8.2.5
        Task TrashAlike(Card card);
        // CR: 8.2.11
        Task PayInstallCost(Card card);
    }
}
