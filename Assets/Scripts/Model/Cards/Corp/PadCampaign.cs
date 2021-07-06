using System.Collections.Generic;
using System.Threading.Tasks;
using model.cards.types;
using model.choices.trash;
using model.costs;
using model.play;
using model.timing.corp;

namespace model.cards.corp {
    public class PadCampaign : Card {

        override public string FaceupArt => "pad-campaign";
        override public string Name => "PAD Campaign";
        override public Faction Faction => Factions.SHADOW;
        override public int InfluenceCost => 0;
        override public ICost PlayCost => game.corp.credits.PayingForPlaying(this, 2);
        override public IType Type => new Asset(game);
        override public IList<ITrashOption> TrashOptions() => new List<ITrashOption> {
            new Leave(),
            new PayToTrash(4, this, game)
        };

        private Ability drip;
        private IList<CorpDrawPhase> phases = new List<CorpDrawPhase>();

        public PadCampaign(Game game) : base(game) {
            drip = new Ability(
                cost: new Free(),
                effect: game.corp.credits.Gaining(1),
                source: this,
                mandatory: true
            );
        }

        protected override Task Activate() {
            game.Timing.CorpTurnDefined += DeferDrip;
            return Task.CompletedTask;
        }

        override protected Task Deactivate() {
            game.Timing.CorpTurnDefined -= DeferDrip;
            return Task.CompletedTask;
        }

        private void DeferDrip(CorpTurn turn) {
            turn.Begins.Offer(game.corp.pilot, drip);
        }
    }
}
