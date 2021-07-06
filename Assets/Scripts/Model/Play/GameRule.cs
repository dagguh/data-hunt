using System;
using System.Collections.Generic;
using model.player;
using model.timing;

namespace model.play {
    public class GameRule : ISource {
        public bool Active { get; }
        public event Action<ISource> ChangedActivation;
        public IList<ITimingStructure> Used { get; } // CR 9.1.6
        public IPilot Controller { get; }
    }
}
