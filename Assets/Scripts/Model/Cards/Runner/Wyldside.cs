using System.Threading.Tasks;
using model.cards.types;
using model.costs;
using model.effects;
using model.play;
using model.timing.runner;

namespace model.cards.runner {
    public class Wyldside : Card {
        private Ability party;
        override public string FaceupArt => "wyldside";
        override public string Name => "Wyldside";
        override public Faction Faction => Factions.ANARCH;
        override public int InfluenceCost => 3;
        override public ICost PlayCost => game.runner.credits.PayingForPlaying(this, 3);
        override public IType Type => new Resource(game);

        public Wyldside(Game game) : base(game) {
            party = new Ability(
                cost: new Free(),
                new Sequence(game.runner.zones.Drawing(2), game.runner.clicks.Losing(1)),
                source: this,
                mandatory: true
            );
        }

        async protected override Task Activate() {
            game.Timing.RunnerTurnDefined += DeferParty;
            await Task.CompletedTask;
        }

        override protected Task Deactivate() {
            game.Timing.RunnerTurnDefined -= DeferParty;
            return Task.CompletedTask;
        }

        private void DeferParty(RunnerTurn turn) {
            turn.Begins.Offer(game.runner.pilot, party);
        }
    }
}
