using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.play;
using model.player;

namespace model.timing {

    // CR: 9.2.8
    public class ReactionWindow : PriorityWindow {
        private IPilot acting;
        private IPilot reacting;
        private Dictionary<IPilot, Ability> mandatory = new Dictionary<IPilot, Ability>();
        private Dictionary<IPilot, Ability> optional = new Dictionary<IPilot, Ability>();

        public ReactionWindow(string name) : base(name) {

        }

        internal void Offer(IPilot pilot, Ability ability) {
            if (ability.Mandatory) {
                mandatory[pilot] = ability;
            } else {
                optional[pilot] = ability;
            }
        }

        // CR: 9.2.8.b
        async override protected Task Proceed() {
            await AwaitPass(acting);
            await AwaitPass(reacting);
        }

        async private Task<Priority> AwaitPass(IPilot pilot) {
            var priority = new Priority(pilot, canPass: true); // CR: 9.2.4.b
            PriorityGiven(priority);
            while (!priority.Passed) // CR: 9.2.4.c
            {
                await priority.Choose(); // CR: 9.2.7.f
            }
            return priority;
        }
    }
}
