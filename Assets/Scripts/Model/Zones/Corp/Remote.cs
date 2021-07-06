using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using model.cards;
using model.cards.types;
using model.costs;
using model.player;
using model.timing;

namespace model.zones.corp
{
    public class Remote : IServer, IInstallDestination
    {
        public Zone Zone { get; } = new Zone("Remote", true);
        public IceColumn Ice { get; }
        private Corp corp;

        public Remote(Corp corp)
        {
            this.corp = corp;
            Ice = new IceColumn(this, corp.credits);
        }

        public bool IsEmpty()
        {
            return (Ice.Height == 0) && (Zone.Count == 0);
        }

        async Task IInstallDestination.Host(Card card)
        {
            Zone
                .Cards
                .Select(it => new Trash(it, corp.zones.archives.Zone))
                .ToList()
                .ForEach(async it => await it.TrashIt());
            await card.MoveTo(Zone);
        }

        // CR: 8.2.5.a
        Task IInstallDestination.TrashAlike(Card card)
        {
            if (card.Type is Asset || card.Type is Agenda)
            {
                // TODO
            }
            return Task.CompletedTask;
        }

        Task IInstallDestination.PayInstallCost(Card card)
        {
            return Task.CompletedTask;
        }

        async Task IServer.Access(int accessCount, IPilot pilot, Game game)
        {
            var unaccessed = new List<Card>(Zone.Cards);
            for (var accessesLeft = accessCount; accessesLeft > 0; accessesLeft--)
            {
                var card = await pilot.ChooseACard().Declare("Which card to access now?", unaccessed);
                unaccessed.Remove(card);
                await new AccessCard(card, game).AwaitEnd();
            }
        }

    }
}
