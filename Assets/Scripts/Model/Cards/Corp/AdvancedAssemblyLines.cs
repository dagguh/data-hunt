using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using model.cards.types;
using model.choices.trash;
using model.costs;
using model.play;
using model.timing;
using model.zones;

namespace model.cards.corp {
    public class AdvancedAssemblyLines : Card {
        private Ability pop;
        override public string FaceupArt => "advanced-assembly-lines";
        override public string Name => "Advanced Assembly Lines";
        override public Faction Faction => Factions.HAAS_BIOROID;
        override public int InfluenceCost => 2;
        override public ICost PlayCost => game.corp.credits.PayingForPlaying(this, 1);
        override public IType Type => new Asset(game);
        override public IList<ITrashOption> TrashOptions() => new List<ITrashOption> {
            new Leave(),
            new PayToTrash(1, this, game)
        };

        public AdvancedAssemblyLines(Game game) : base(game) {
            pop = new Ability(
                cost: new Trash(this, game.corp.zones.archives.Zone),
                effect: new AdvancedAssemblyLinesInstall(game.corp),
                source: this,
                mandatory: false
            );
        }

        async protected override Task Activate() {
            await game.corp.credits.Gaining(3).Resolve();
            game.Timing.PaidWindowDefined += DeferPop;
        }

        protected override Task Deactivate() {
            game.Timing.PaidWindowDefined -= DeferPop;
            return Task.CompletedTask;
        }

        private void DeferPop(PaidWindow paidWindow) {
            paidWindow.GiveOption(game.corp.pilot, pop);
        }

        private class AdvancedAssemblyLinesInstall : IEffect {
            public bool Impactful => Installables().Count > 0;
            public event Action<IEffect, bool> ChangedImpact = delegate { };
            IEnumerable<string> IEffect.Graphics => new string[] { };
            private Corp corp;

            public AdvancedAssemblyLinesInstall(Corp corp) {
                this.corp = corp;
                corp.zones.hq.Zone.Changed += UpdateInstallables;
            }

            private void UpdateInstallables(Zone hqZone) {
                ChangedImpact(this, Impactful);
            }

            private IList<Card> Installables() => corp.zones.hq.Zone.Cards
                .Where(card => (card.Type.Installable && !(card.Type is Agenda)))
                .ToList();

            async Task IEffect.Resolve() {
                var installable = await corp.pilot.ChooseACard().Declare("Which card to install?", Installables());
                await corp.Installing.InstallingCard(installable).Resolve();
            }
        }
    }
}
