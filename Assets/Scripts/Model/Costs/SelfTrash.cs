﻿using model.cards;

namespace model.costs
{
    public class SelfTrash : ICost
    {
        private readonly Card card;

        public SelfTrash(Card card)
        {
            this.card = card;
        }

        void ICost.Observe(IPayabilityObserver observer, Game game)
        {
            observer.NotifyPayable(true, this);
        }

        void ICost.Pay(Game game)
        {
            card.MoveTo(game.runner.zones.heap.zone);
        }
    }
}