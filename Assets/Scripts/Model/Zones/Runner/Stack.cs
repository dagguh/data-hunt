using System.Threading.Tasks;

namespace model.zones.runner
{
    public class Stack
    {
        public readonly Zone zone = new Zone("Stack", false);
        private Shuffling shuffling;

        public Stack(Shuffling shuffling)
        {
            this.shuffling = shuffling;
        }

        async public Task AddDeck(Deck deck)
        {
            foreach (var card in deck.cards)
            {
                await card.MoveTo(zone);
            }
            Shuffle();
        }

        public void Shuffle()
        {
            shuffling.Shuffle(zone.Cards);
        }

        public bool HasCards() => zone.Cards.Count > 0;

        async public Task Draw(int cards, Grip grip)
        {
            for (int i = 0; i < cards; i++)
            {
                if (HasCards())
                {
                    await zone.Cards[0].MoveTo(grip.zone);
                }
            }
        }
    }
}
