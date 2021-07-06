using System;
using System.Threading.Tasks;
using model.cards;

namespace model.zones.runner
{
    public class Score
    {
        public readonly Zone zone = new Zone("Runner score", false);
        private int score = 0;
        public event Action StolenEnough = delegate { };

        async public Task Add(Card card, int agendaPoints)
        {
            card.FlipFaceUp(); // CR: 1.16.4
            await card.MoveTo(zone);
            score += agendaPoints;
            if (score >= 7) // TODO Harmony MedTech
            {
                StolenEnough();
            }
        }
    }
}
