using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using model.cards;
using model.player;

namespace model.zones.runner
{
    public class Grip
    {
        public readonly Zone zone = new Zone("Grip", false);
        public event Action DiscardingOne = delegate { };
        public event Action<Card> DiscardedOne = delegate { };
        private TaskCompletionSource<bool> discarded;

        public Grip()
        {
            zone.Added += (zone, card) => card.UpdateInfo(Information.HIDDEN_FROM_CORP);
        }

        async public Task Discard()
        {
            discarded = new TaskCompletionSource<bool>();
            DiscardingOne();
            await discarded.Task;
        }

        async public Task Discard(Card card, Heap heap)
        {
            await card.MoveTo(heap.zone);
            DiscardedOne(card);
            discarded.SetResult(true);
        }

        public Card Find<T>() where T : Card => zone.Cards.OfType<T>().First();
    }
}
