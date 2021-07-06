using System;
using model.player;
using model.timing;
using model.zones;

namespace model
{
    public class Game
    {
        public readonly Corp corp;
        public readonly Runner runner;
        public readonly Timing Timing;
        private readonly Zone playArea;
        private Shuffling shuffling;

        public Game(IPilot corpPilot, IPilot runnerPilot, Shuffling shuffling)
        {
            this.shuffling = shuffling;
            playArea = new Zone("Play area", true);
            corp = new Corp(corpPilot, playArea, shuffling, new Random());
            runner = new Runner(runnerPilot, playArea, shuffling, this);
            this.Timing = new Timing(this);
        }

        async public void Start(Deck corpDeck, Deck runnerDeck)
        {
            await corp.Start(this, corpDeck);
            await runner.Start(this, runnerDeck);
            await Timing.StartTurns();
        }

        public IPilot Pilot(Side side)
        {
            switch (side)
            {
                case Side.CORP: return corp.pilot;
                case Side.RUNNER: return runner.pilot;
                default: throw new Exception("Unclear how to pilot an unknown side: " + side);
            }
        }
    }
}
