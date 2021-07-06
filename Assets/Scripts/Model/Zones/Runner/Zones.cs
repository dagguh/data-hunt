using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.cards;

namespace model.zones.runner
{
    public class Zones
    {
        public readonly Zone identity = new Zone("Corp identity", false);
        public readonly Grip grip;
        public readonly Stack stack;
        public readonly Heap heap;
        public readonly Rig rig;
        public readonly Score score;
        public readonly Zone playArea;

        public Zones(Grip grip, Stack stack, Heap heap, Rig rig, Score score, Zone playArea)
        {
            this.grip = grip;
            this.stack = stack;
            this.heap = heap;
            this.rig = rig;
            this.score = score;
            this.playArea = playArea;
        }

        public IEffect Drawing(int cards)
        {
            return new Draw(cards, stack, grip);
        }

        private class Draw : IEffect
        {
            private int cards;
            private Stack stack;
            private Grip grip;
            public bool Impactful => stack.zone.Count > 0;
            public event Action<IEffect, bool> ChangedImpact = delegate { };
            IEnumerable<string> IEffect.Graphics => new string[] { "Images/UI/card-draw" };

            public Draw(int cards, Stack stack, Grip grip)
            {
                this.cards = cards;
                this.stack = stack;
                this.grip = grip;
                stack.zone.Changed += (_) => ChangedImpact(this, Impactful);
            }

            async Task IEffect.Resolve()
            {
                await stack.Draw(cards, grip);
            }
        }

        public IEffect Playing(Card card) => new Play(card, heap.zone);
    }
}
