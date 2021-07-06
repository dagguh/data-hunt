using model.player;

namespace model.timing {
    public abstract class ITurn : ITimingStructure {

        public abstract ClickPool Clicks { get; }
        public abstract Side Side { get; }
        public abstract IPilot Owner { get; }

        protected ITurn(string name) : base(name) {
        }
    }
}
