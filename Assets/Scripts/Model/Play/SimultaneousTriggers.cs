using model.player;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace model.play
{
    public class SimultaneousTriggers
    {
        public IList<Ability> untriggered;

        public SimultaneousTriggers(IList<Ability> effects)
        {
            untriggered = effects;
        }

        async public Task AllTriggered(IPilot pilot)
        {
            while (untriggered.Count > 0)
            {
                UnityEngine.Debug.Log("Picking the next effect to fire among " + untriggered);
                var ability = await pilot.TriggerFromSimultaneous(untriggered);
                await ability.Ability.Trigger();
                untriggered.Remove(ability);
            }
        }
    }
}
