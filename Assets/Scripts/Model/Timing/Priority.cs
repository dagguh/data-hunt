using System.Collections.Generic;
using System.Threading.Tasks;
using model.play;

namespace model.timing {
    public class Priority {
        public bool Passed { get; private set; } = false;
        public bool Declined { get; private set; } = true;
        private IList<IPlayOption> options = new List<IPlayOption>();
        private IPlayOption pass = new Pass();

        internal Priority(bool canPass) {
            if (canPass) {
                options.Add(pass);
            }
        }

        public void Offer(IPlayOption option) {
            options.Add(option);
        }

        public IList<IPlayOption> BrowseOptions() => new List<IPlayOption>(options);

        async public Task Choose(IPlayOption option) {
            if (!option.Legal) {
                throw new System.Exception(this + " chose an illegal option " + option);
            }
            if (!options.Contains(option)) {
                throw new System.Exception(this + " chose an unavailable option " + option);
            }
            await option.Resolve();
            if (option == pass) {
                Passed = true;
            } else {
                Declined = false;
            }
        }

        private class Pass : IPlayOption {
            public bool Legal => true;
            public bool Mandatory => false;

            async public Task Resolve() {
                await Task.CompletedTask;
            }
        }
    }
}
