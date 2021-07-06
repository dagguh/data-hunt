using model.cards;
using model.choices;
using model.choices.trash;
using model.play;
using model.steal;
using model.zones;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace model.player
{
    public class SingleChoiceMaker : DelegatingPilot
    {
        public SingleChoiceMaker(IPilot basic) : base(basic) { }

        async override public Task<CardAbility> TriggerFromSimultaneous(IList<CardAbility> abilities)
        {
            if (abilities.Count == 1)
            {
                return abilities.Single();
            }
            else
            {
                return await base.TriggerFromSimultaneous(abilities);
            }
        }

        override public IDecision<string, Card> ChooseACard() => new TheOnlyChoice<string, Card>(base.ChooseACard());
        override public IDecision<string, IInstallDestination> ChooseAnInstallDestination() => new TheOnlyChoice<string, IInstallDestination>(base.ChooseAnInstallDestination());
        override public IDecision<Card, ITrashOption> ChooseTrashing() => new TheOnlyChoice<Card, ITrashOption>(base.ChooseTrashing());
        override public IDecision<Card, IStealOption> ChooseStealing() => new TheOnlyChoice<Card, IStealOption>(base.ChooseStealing());
    }
}
