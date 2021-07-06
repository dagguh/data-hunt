using System.Threading.Tasks;
using model.cards;

namespace model.zones.corp
{
    public class Root : IInstallDestination
    {
        Task IInstallDestination.Host(Card card)
        {
            // if (card.Type.Rezzable)
            // {
            //     corp.turn.rezWindow.Add(new Rezzable(card));
            // }
            throw new System.NotImplementedException();
        }

        Task IInstallDestination.PayInstallCost(Card card)
        {
            throw new System.NotImplementedException();
        }

        Task IInstallDestination.TrashAlike(Card card)
        {
            throw new System.NotImplementedException();
        }
    }
}
