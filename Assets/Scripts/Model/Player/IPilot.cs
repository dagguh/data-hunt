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
    public interface IPilot
    {
        void Play(Game game);
        Task<Ability> TriggerFromSimultaneous(IEnumerable<Ability> abilities);
        IDecision<string, Card> ChooseACard();
        // IDecision<Card> ChooseAZone(); TODO for central access
        IDecision<string, IInstallDestination> ChooseAnInstallDestination();
        IDecision<Card, ITrashOption> ChooseTrashing();
        IDecision<Card, IStealOption> ChooseStealing();
        IPlayOption Choose(IList<IPlayOption> options);
    }
}
