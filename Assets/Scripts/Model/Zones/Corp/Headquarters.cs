using model.cards;
using model.player;
using model.timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace model.zones.corp
{
    public class Headquarters : IServer
    {
        public Zone Zone { get; } = new Zone("HQ", false);
        public IceColumn Ice { get; }
        public event Action DiscardingOne = delegate {};
        public event Action<Card> DiscardedOne = delegate {};
        private TaskCompletionSource<bool> discarding;
        private Shuffling shuffling;

        public Headquarters(Shuffling shuffling, CreditPool credits)
        {
            this.shuffling = shuffling;
            Ice = new IceColumn(this, credits);
        }

        async public Task Discard()
        {
            discarding = new TaskCompletionSource<bool>();
            DiscardingOne();
            await discarding.Task;
        }

        async public Task Discard(Card card, Archives archives)
        {
            await card.MoveTo(archives.Zone);
            DiscardedOne(card);
            discarding.SetResult(true);
        }

        public Card Random() => Zone.Cards[shuffling.Random.Next(0, Zone.Count)];

        async Task IServer.Access(int accessCount, IPilot pilot, Game game)
        {
            if (Zone.Cards.Count == 0)
            {
                return;
            }
            var unaccessed = new List<Card>(Zone.Cards);
            for (var accessesLeft = accessCount; accessesLeft > 0; accessesLeft--)
            {
                var randomCard = unaccessed
                    .OrderBy(it => shuffling.Random.Next())
                    .First();
                var cardToAccess = await pilot.ChooseACard().Declare("Which card to access now?", new List<Card> { randomCard }); 
                unaccessed.Remove(cardToAccess); // TODO : draw already accessed cards on the side
                await new AccessCard(cardToAccess, game).AwaitEnd();
            }
        }
    }
}
