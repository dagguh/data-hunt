using model.cards;
using model.cards.types;
using model.player;
using System.Linq;
using System.Threading.Tasks;

namespace model.zones.runner
{
    public class Rig : IInstallDestination
    {
        public readonly Zone zone = new Zone("Rig", true);
 
        private Runner runner;
        private IPilot pilot;

        public Rig(Runner runner, IPilot pilot)
        {
            this.runner = runner;
            this.pilot = pilot;
        }

        async Task IInstallDestination.Host(Card card)
        {
            await card.MoveTo(zone);
        }

        async Task IInstallDestination.TrashAlike(Card card)
        {
            // CR: 8.2.5.d
            if (card.Type is Program && false /* disable until we get proper MU-based trashing below */)
            {
                var programs = zone.Cards.Where(it => it.Type is Program);
                var old = await pilot.ChooseACard().Declare("Which program to trash?", programs); // actually declare zero or many, unless MU is constrained then a minimum
                await old.MoveTo(runner.zones.heap.zone); // TODO reuse an actual trash abstraction
            }
        }

        async Task IInstallDestination.PayInstallCost(Card card)
        {
            // CR: 8.2.11.a
            await card.PlayCost.Pay();
        }
    }
}
