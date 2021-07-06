using System.Threading.Tasks;
using model.cards.types;

namespace model.cards.runner
{
    public class SureGamble : Card
    {
        public SureGamble(Game game) : base(game) { }
        override public string FaceupArt { get { return "sure-gamble"; } }
        override public string Name { get { return "Sure Gamble"; } }
        override public Faction Faction { get { return Factions.MASQUE; } }
        override public int InfluenceCost { get { return 0; } }
        override public ICost PlayCost => game.runner.credits.PayingForPlaying(this, 5);
        override public IType Type { get { return new Event(); } }

        async protected override Task Activate() {
            await game.runner.credits.Gaining(9).Resolve();
        }
    }
}
