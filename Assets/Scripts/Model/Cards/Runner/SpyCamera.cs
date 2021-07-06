using System.Threading.Tasks;
using model.cards.types;

namespace model.cards.runner
{
    public class SpyCamera : Card
    {
        public SpyCamera(Game game) : base(game) { }
        override public string FaceupArt { get { return "spy-camera"; } }
        override public string Name { get { return "Spy Camera"; } }
        override public Faction Faction { get { return Factions.CRIMINAL; } }
        override public int InfluenceCost { get { return 1; } }
        override public ICost PlayCost => game.runner.credits.PayingForPlaying(this, 0);
        override public IType Type => new Hardware(game);

        protected override Task Activate() {
            return Task.FromResult("Add the abilities");
        }
    }
}
