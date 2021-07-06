using model.cards;
using model.choices;
using model.choices.trash;
using model.play;
using model.steal;
using model.zones;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace model.player
{
    public class DelegatingPilot : IPilot
    {
        private IPilot basic;

        public DelegatingPilot(IPilot basic)
        {
            this.basic = basic;
        }

        public virtual void Play(Game game)
        {
            basic.Play(game);
        }

        public virtual Task<CardAbility> TriggerFromSimultaneous(IList<CardAbility> abilities)
        {
            return basic.TriggerFromSimultaneous(abilities);
        }

        public virtual IDecision<string, Card> ChooseACard()
        {
            return basic.ChooseACard();
        }

        public virtual IDecision<string, IInstallDestination> ChooseAnInstallDestination()
        {
            return basic.ChooseAnInstallDestination();
        }

        public virtual IDecision<Card, ITrashOption> ChooseTrashing()
        {
            return basic.ChooseTrashing();
        }

        public virtual IDecision<Card, IStealOption> ChooseStealing()
        {
            return basic.ChooseStealing();
        }
    }
}
