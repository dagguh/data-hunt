using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.cards;

namespace model.zones
{
    internal class Play : IEffect
    {
        private Card card;
        private Zone bin;
        private Zone playZone = new Zone("Play", true);
        public bool Impactful => card.Activation.Impactful;
        public event Action<IEffect, bool> ChangedImpact = delegate { };
        IEnumerable<string> IEffect.Graphics => new string[] { "Images/Cards/" + card.FaceupArt };

        public Play(Card card, Zone bin)
        {
            if (!card.Type.Playable)
            {
                throw new Exception("Cannot play an unplayable card " + card);
            }
            this.card = card;
            this.bin = bin;
            card.Activation.ChangedImpact += UpdateImpact;
        }

        private void UpdateImpact(IEffect cardActivation, bool impactful)
        {
            ChangedImpact(this, impactful);
        }

        async Task IEffect.Resolve()
        {
            card.FlipFaceUp();
            await card.MoveTo(playZone);
            await card.Activate();
            await card.Deactivate();
            await card.MoveTo(bin);
        }

        public override string ToString() => "Play(card=" + card + ")";
    }
}
