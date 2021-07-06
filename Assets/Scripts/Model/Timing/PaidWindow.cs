using System.Threading.Tasks;
using model.play;
using model.player;

namespace model.timing {
    public class PaidWindow : PriorityWindow {
        private bool rezzing;
        private bool scoring;
        private IPilot acting;
        private IPilot reacting;

        public PaidWindow(bool rezzing, bool scoring, IPilot acting, IPilot reacting, string name) : base(name) {
            this.rezzing = rezzing;
            this.scoring = scoring;
            this.acting = acting;
            this.reacting = reacting;
        }

        internal void GiveOption(IPilot pilot, IPlayOption pop) {
            throw new System.NotImplementedException();
        }

        async override protected Task Proceed() {
            var bothPlayersCouldAct = false; // CR: 9.2.7.a
            while (true) {
                var action = await AwaitPass(acting);
                if (action.Declined && bothPlayersCouldAct) {
                    break;
                }
                var reaction = await AwaitPass(reacting);
                bothPlayersCouldAct = true;
                if (reaction.Declined && bothPlayersCouldAct) {
                    break;
                }
            }
        }

        async private Task<Priority> AwaitPass(IPilot pilot) {
            var priority = new Priority(canPass: true); // CR: 9.2.4.b
            PriorityGiven(priority);
            while (!priority.Passed) // CR: 9.2.4.c
            {
                await pilot.Receive(priority); // CR: 9.2.7.f
            }
            return priority;
        }
    }
}
