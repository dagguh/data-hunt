using System.Threading.Tasks;
using model.cards;
using model.zones.runner;

namespace model.steal
{
    public class MustSteal : IStealOption
    {
        private Card card;
        private int agendaPoints;
        private Zones zones;
        string IStealOption.Art => "Images/UI/hand-click";

        public MustSteal(Card card, int agendaPoints, Zones zones)
        {
            this.card = card;
            this.agendaPoints = agendaPoints;
            this.zones = zones;
        }

        bool IStealOption.IsLegal() => true;
        async Task<bool> IStealOption.Perform()
        {
            await zones.score.Add(card, agendaPoints); // CR: 1.16.3
            return true;
        }
    }
}
