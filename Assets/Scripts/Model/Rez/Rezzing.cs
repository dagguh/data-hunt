using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.cards;
using model.costs;
using model.play;

namespace model.rez {
    public class Rezzing {
        private Corp corp;
        public RezWindow Window { get; }

        public Rezzing(Corp corp) {
            this.corp = corp;
            Window = new RezWindow();
        }

        public void Track(Card card) {
            Window.Add(Rez(card));
        }

        // CR: 8.1.2.a
        public IPlayOption Rez(Card card) {
            var rezCost = new Conjunction(
                card.PlayCost,
                new FaceDown(card),
                new Installed(card),
                Window.Permission()
            );
            return new Ability(rezCost, new RezUnconditionally(card));
        }

        private class FaceDown : ICost {
            private Card card;
            public bool Payable => !card.Faceup;
            public event Action<ICost, bool> ChangedPayability;

            public FaceDown(Card card) {
                this.card = card;
            }

            async public Task Pay() {
                if (!Payable) {
                    throw new Exception(card + " should be face down");
                }
                await Task.CompletedTask;
            }
        }

        private class Installed : ICost {
            private Card card;
            public bool Payable => card.Zone.InPlayArea;
            public event Action<ICost, bool> ChangedPayability;

            public Installed(Card card) {
                this.card = card;
            }

            async public Task Pay() {
                if (!Payable) {
                    throw new Exception(card + " should be installed, but it's in " + card.Zone);
                }
                await Task.CompletedTask;
            }
        }

        private class RezUnconditionally : IEffect {
            private Card card;
            public bool Impactful => true;
            public event Action<IEffect, bool> ChangedImpact;
            public IEnumerable<string> Graphics => new string[] { };

            public RezUnconditionally(Card card) {
                this.card = card;
            }

            async public Task Resolve() {
                card.FlipFaceUp();
                await card.PlayCost.Pay();
                await card.Activate();
            }
        }
    }
}
