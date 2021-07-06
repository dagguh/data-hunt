using System.Threading.Tasks;
using model;
using model.cards;
using model.zones.runner;

namespace controller
{
    public class InteractiveDiscard : IInteractive
    {
        private Card card;
        private Grip grip;
        private Heap heap;
        public DropZone Activation { get; }
        public bool Active { get; private set; }
        public event Update Updated = delegate { };

        public InteractiveDiscard(Card card, DropZone activation, Runner runner)
        {
            this.card = card;
            this.Activation = activation;
            this.Active = false;
            this.grip = runner.zones.grip;
            this.heap = runner.zones.heap;
            this.grip.DiscardingOne += ActivateDiscarding;
            this.grip.DiscardedOne += DeactivateDiscarding;
        }

        async Task IInteractive.Interact()
        {
            await grip.Discard(card, heap);
            await Task.CompletedTask;
        }

        private void ActivateDiscarding()
        {
            Active = true;
            Updated();
        }

        private void DeactivateDiscarding(Card discarded)
        {
            Active = false;
            Updated();
        }
    }
}
