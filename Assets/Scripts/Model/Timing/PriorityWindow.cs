using System;
using System.Threading.Tasks;

namespace model.timing {

    public abstract class PriorityWindow {
        public event AsyncAction Opened;
        public event AsyncAction Closed;
        public event Action<Priority> PriorityGiven = delegate { };
        public string Name { get; }

        public PriorityWindow(string name) {
            Name = name;
        }

        async public Task Open() {
            await Opened?.Invoke();
            await Proceed();
            await Closed?.Invoke();
        }

        protected abstract Task Proceed();

        protected 
    }
}
