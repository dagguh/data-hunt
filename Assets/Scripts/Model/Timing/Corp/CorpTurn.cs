using System.Threading.Tasks;
using model.player;

namespace model.timing.corp {
    public class CorpTurn : ITurn {
        public CorpDrawPhase drawPhase { get; }
        public CorpActionPhase actionPhase { get; }
        public CorpDiscardPhase discardPhase { get; }
        private Corp corp;
        private Timing timing;
        override public ClickPool Clicks => corp.clicks;
        override public Side Side => Side.CORP;
        override public IPilot Owner => corp.pilot;

        public CorpTurn(Corp corp, Timing timing, int number) : base("Corp turn " + number) {
            this.corp = corp;
            this.timing = timing;
            drawPhase = new CorpDrawPhase(corp, timing, Begins);
            actionPhase = new CorpActionPhase(corp, timing);
            discardPhase = new CorpDiscardPhase();
        }

        public CorpTurn(Corp corp, Timing timing) {
            this.corp = corp;
            this.timing = timing;
        }

        async protected override Task Proceed() {
            await drawPhase.Open();
            await actionPhase.Open();
            await discardPhase.Open();
        }

        async private Task DiscardPhase() {
            await Discard(); // CR: 5.6.3.a
            var rez = OpenRezWindow();  // CR: 5.6.3.b
            var paid = OpenPaidWindow();  // CR: 5.6.3.b
            await rez;
            await paid;
            corp.clicks.Reset(); // CR: 5.6.3.c
            TriggerTurnEnding(); // CR: 5.6.3.d
            await timing.Checkpoint(); // CR: 5.6.3.e
        }

        async private Task Discard() {
            var hq = corp.zones.hq;
            while (hq.Zone.Count > 5) {
                await hq.Discard();
            }
        }

        private void TriggerTurnEnding() {
        }
    }
}
