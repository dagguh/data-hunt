﻿using model.effects.runner;
using model.costs;
using model.effects;
using model.cards.types;

namespace model.cards.runner
{
    public class SureGamble : ICard
    {
        string ICard.FaceupArt { get { return "sure-gamble"; } }

        string ICard.Name { get { return "Sure Gamble"; } }

        bool ICard.Faceup { get { return false; } }

        Faction ICard.Faction { get { return Factions.MASK; } }

        int ICard.InfluenceCost { get { return 0; } }

        ICost ICard.PlayCost { get { return new RunnerCreditCost(5); } }

        IEffect ICard.Activation => new Gain(9);

        IType ICard.Type { get { return new Event(); } }
    }
}