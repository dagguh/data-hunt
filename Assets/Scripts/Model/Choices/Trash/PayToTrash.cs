using System.Threading.Tasks;
using model.cards;
using model.zones.corp;

namespace model.choices.trash
{
    public class PayToTrash : ITrashOption
    {
        private ICost cost;
        private Card card;
        private Archives archives;

        public PayToTrash(int trashCost, Card card, Game game)
        {
            this.cost = game.runner.credits.PayingForTrashing(card, trashCost);
            this.card = card;
            this.archives = game.corp.zones.archives;
        }

        bool ITrashOption.IsLegal() => cost.Payable;

        async Task<bool> ITrashOption.Perform()
        {
            await cost.Pay();
            await card.MoveTo(archives.Zone);
            return true;
        }

        string ITrashOption.Art => "Images/UI/trash-can";
    }
}
