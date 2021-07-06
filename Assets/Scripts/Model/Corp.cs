using System;
using System.Threading.Tasks;
using model.install;
using model.play;
using model.player;
using model.rez;
using model.timing;
using model.zones;
using model.zones.corp;

namespace model
{
    public class Corp
    {
        public readonly IPilot pilot;
        public readonly zones.corp.Zones zones;
        public readonly ClickPool clicks;
        public readonly CreditPool credits;
        public CorpActing Acting { get; }
        public Installing Installing { get; }
        public Rezzing Rezzing { get; }

        public Corp(
            IPilot pilot,
            Zone playArea,
            Shuffling shuffling,
            Random random
        )
        {
            this.pilot = pilot;
            clicks = new ClickPool(3);
            credits = new CreditPool();
            zones = new Zones(this, playArea, shuffling);
            this.Acting = new CorpActing(this);
            this.Installing = new Installing(pilot, playArea);
            this.Rezzing = new Rezzing(this);
        }

        async public Task Start(Game game, Deck deck)
        {
            await zones.rd.AddDeck(deck);
            var identity = deck.identity;
            zones.identity.Add(identity);
            identity.FlipFaceUp();
            await identity.Activate();
            pilot.Play(game);
            credits.Gain(5);
            await zones.rd.Draw(5, zones.hq);
        }
    }
}
