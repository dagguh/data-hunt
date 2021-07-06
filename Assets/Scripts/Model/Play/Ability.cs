using System;
using System.Threading.Tasks;

namespace model.play {
    public class Ability : IPlayOption {
        public readonly ICost cost;
        public readonly IEffect effect;
        public readonly ISource source;
        public bool Mandatory { get; }
        private bool active;
        public event Action<Ability, bool> UsabilityChanged = delegate { };
        public event Action<Ability> Resolved = delegate { };

        public bool Usable => cost.Payable && effect.Impactful; // CR: 1.2.5
        public bool Legal => active && Usable;

        public Ability(ICost cost, IEffect effect, ISource source, bool mandatory) {
            this.cost = cost;
            this.effect = effect;
            this.source = source;
            this.Mandatory = mandatory;
            active = source.Active; // CR: 9.1.8
            source.ChangedActivation += UpdateActivity;
            cost.ChangedPayability += UpdateCost;
            effect.ChangedImpact += UpdateEffect;
        }

        private void UpdateActivity(ISource source) {
            active = source.Active;
        }

        private void UpdateCost(ICost source, bool payable) {
            UsabilityChanged(this, Usable);
        }

        private void UpdateEffect(IEffect source, bool impactful) {
            UsabilityChanged(this, Usable);
        }

        async public Task Resolve() {
            await Trigger();
        }

        async public Task Trigger() {
            if (active) {
                await cost.Pay();
                await effect.Resolve();
                Resolved(this);
            } else {
                throw new System.Exception("Cannot trigger an inactive ability " + this);
            }
        }

        public override string ToString() => cost + " : " + effect;
    }
}
