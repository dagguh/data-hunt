﻿using System.Collections.Generic;
using model.cards;

namespace model.zones.corp
{
    public class ResearchAndDevelopment : IServer
    {
        public Zone Zone { get; } = new Zone("R&D");
        public IceColumn Ice { get; } = new IceColumn();
        private Shuffling shuffling;

        public ResearchAndDevelopment(Deck deck, Shuffling shuffling)
        {
            this.shuffling = shuffling;
            foreach (var card in deck.cards)
            {
                card.MoveTo(Zone);
            }
        }

        public void Shuffle()
        {
            shuffling.Shuffle(Zone.Cards);
        }

        public bool HasCards() => Zone.Cards.Count > 0;

        public void Draw(int cards, Headquarters hq)
        {
            for (int i = 0; i < cards; i++)
            {
                if (HasCards())
                {
                    Zone.Cards[0].MoveTo(hq.Zone);
                }
            }
        }
    }
}