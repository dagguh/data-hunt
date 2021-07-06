using System.Threading.Tasks;

namespace model.timing.corp {
    public class CorpDrawPhase : ITimingStructure {
        private Corp corp;
        private Timing timing;
        private ReactionWindow turnBegins;

        internal CorpDrawPhase(Corp corp, Timing timing, ReactionWindow turnBegins) : base("Corp draw phase") {
            this.corp = corp;
            this.timing = timing;
        }

        async protected override Task Proceed() {
            corp.clicks.Replenish(); // CR: 5.6.1.a
            await timing.OpenPaidWindow(rezzing: true, scoring: true); // CR: 5.6.1.b
            RefillRecurringCredits(); // CR: 5.6.1.c
            await turnBegins.Open(); // CR: 5.6.1.d
            await timing.Checkpoint();// CR: 5.6.1.e
            await MandatoryDraw(); // CR: 5.6.1.f
            await timing.Checkpoint(); // CR: 5.6.1.g
        }

        private void RefillRecurringCredits() {
        }

        async private Task MandatoryDraw() {
            await corp.zones.Drawing(1).Resolve();
        }
    }
}
