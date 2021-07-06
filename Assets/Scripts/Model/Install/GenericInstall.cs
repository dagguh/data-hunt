using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.cards;
using model.play;
using model.player;
using model.zones;

namespace model.install
{

    internal class GenericInstall : IEffect
    {
        private readonly Card card;
        private readonly IPilot pilot;
        private readonly Zone playArea;
        public bool Impactful { get; private set; }
        public event Action<IEffect, bool> ChangedImpact = delegate { };

        IEnumerable<string> IEffect.Graphics
        {
            get
            {
                if (card.Faction.Side == Side.CORP)
                {
                    return new string[] { "Images/UI/server-rack" };
                }
                else
                {
                    return new string[] { "Images/Cards/" + card.FaceupArt };
                }
            }
        }

        public GenericInstall(Card card, IPilot pilot, Zone playArea)
        {
            this.card = card;
            this.pilot = pilot;
            this.playArea = playArea;
            if (card.Faction.Side == Side.RUNNER)
            {
                Impactful = card.PlayCost.Payable;
                card.PlayCost.ChangedPayability += CheckIfRunnerCanInstall;
            }
            if (card.Faction.Side == Side.CORP)
            {
                Impactful = true;
                ChangedImpact(this, Impactful);
            }
        }

        private void CheckIfRunnerCanInstall(ICost cost, bool payable)
        {
            Impactful = payable;
            ChangedImpact(this, Impactful);
        }

        // CR: 8.3
        async public Task Resolve()
        {
            await PutOut();
            var destination = await ChooseDestination();
            await TrashLikeCards(destination);
            await PayInstallCost(destination);
            await Place(destination);
            await TriggerPostInstall();
        }

        // CR: 8.3.1
        async private Task PutOut()
        {
            await card.MoveTo(playArea);
            card.FlipPreInstall();
        }

        // CR: 8.3.2
        async private Task<IInstallDestination> ChooseDestination()
        {
            var destinations = card.FindInstallDestinations();
            return await pilot.ChooseAnInstallDestination().Declare("Choose installation destination", destinations);
        }

        // CR: 8.3.3
        // CR: 8.2.5
        async private Task TrashLikeCards(IInstallDestination destination)
        {
            await destination.TrashAlike(card);
        }

        // CR: 8.3.4
        // CR: 8.2.11
        async private Task PayInstallCost(IInstallDestination destination)
        {
            await destination.PayInstallCost(card);
        }

        // CR: 8.3.5
        async private Task Place(IInstallDestination destination)
        {
            await destination.Host(card);
            await card.SetInstalled();
            if (card.Faction.Side == Side.RUNNER)
            {
                // CR: 8.2.3
                card.FlipFaceUp();
                await card.BecomeActive();
            }
        }

        // CR: 8.3.6
        async private Task TriggerPostInstall()
        {
            await Task.FromResult("TODO: Implement TriggerPostInstall");
        }
    }
}
