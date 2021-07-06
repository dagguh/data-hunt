using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.cards.types;
using model.costs;
using model.play;
using model.timing;
using model.zones;

namespace model.cards.runner
{
    public class SportsHopper : Card
    {
        public SportsHopper(Game game) : base(game) { }
        override public string FaceupArt => "sports-hopper";
        override public string Name => "Sports Hopper";
        override public Faction Faction => Factions.MASQUE;
        override public int InfluenceCost => 0;
        override public ICost PlayCost => game.runner.credits.PayingForPlaying(this, 3);
        override public IEffect Activation => new SportsHopperActivation(this, game);
        override public IType Type => new Hardware(game);

        private class SportsHopperActivation : IEffect
        {
            private Card hopper;
            private Game game;
            private CardAbility pop;
            public bool Impactful => true;
            public event Action<IEffect, bool> ChangedImpact = delegate { };
            IEnumerable<string> IEffect.Graphics => new string[] { };

            public SportsHopperActivation(Card hopper, Game game)
            {
                this.hopper = hopper;
                this.game = game;
                var zones = game.runner.zones;
                pop = new Ability(new Trash(hopper, zones.heap.zone), zones.Drawing(3), hopper).BelongingTo(hopper);
            }

            async Task IEffect.Resolve()
            {
                game.Timing.PaidWindowDefined += Register;
                await Task.CompletedTask;
            }

            private void Register(PaidWindow paidWindow)
            {
                paidWindow.PriorityGiven += Register;
            }

            private void Register(Priority priority)
            {
                if (priority.Pilot == game.runner.pilot)
                {
                    priority.Add(pop);
                }
            }
        }
    }
}
