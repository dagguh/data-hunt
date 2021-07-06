using System.Threading.Tasks;

namespace model.timing {
    public abstract class ITimingStructure {
        event AsyncAction<ITimingStructure> Initiated;
        event AsyncAction<ITimingStructure> Completed;
        public string Name { get; }
        public readonly ReactionWindow Begins;
        public readonly ReactionWindow Ends;

        protected ITimingStructure(string name) {
            this.Name = name;
            Begins = new ReactionWindow(Name + " begins");
            Ends = new ReactionWindow(Name + " ends");
        }

        async public Task Initiate() {
            await Initiated?.Invoke(this);
            await Proceed();
            await Completed?.Invoke(this);
        }

        protected abstract Task Proceed();
    }
}
