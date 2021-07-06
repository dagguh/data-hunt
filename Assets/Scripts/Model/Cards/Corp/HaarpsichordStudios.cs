using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.cards.types;
using model.steal;
using model.timing;

namespace model.cards.corp
{
    public class HaarpsichordStudios : Card
    {
        public HaarpsichordStudios(Game game) : base(game) { }
        override public string FaceupArt => "haarpsichord-studios";
        override public string Name => "Haarpsichord Studios";
        override public Faction Faction => Factions.NBN;
        override public int InfluenceCost { get { throw new System.Exception("Identities don't have an influence cost"); } }
        override public ICost PlayCost { get { throw new System.Exception("Identities don't have a play cost"); } }
        override public IEffect Activation => new HaarpsichordEffect(game);
        override public IType Type => new Identity();

        private class HaarpsichordEffect : IEffect
        {
            private HaarpsichordMemory memory;
            private HaarpsichordModifier mod;
            private Game game;
            public bool Impactful => true;
            public event Action<IEffect, bool> ChangedImpact = delegate { };
            IEnumerable<string> IEffect.Graphics => new string[] { };

            public HaarpsichordEffect(Game game)
            {
                this.game = game;
                memory = new HaarpsichordMemory();
                mod = new HaarpsichordModifier(memory);
                game.corp.turn.Opened += memory.Reset;
                game.runner.turn.Opened += memory.Reset;
                game.runner.Stealing.ModifyStealing(mod);
            }

            async Task IEffect.Resolve()
            {
                mod.Enabled = true;
                await Task.CompletedTask;
            }

            void IEffect.Disable()
            {
                mod.Enabled = false;
            }
        }

        private class HaarpsichordMemory
        {
            public bool AgendaStolenThisTurn = false;

            async public Task Reset(ITurn turn)
            {
                AgendaStolenThisTurn = false;
                await Task.CompletedTask;
            }
        }

        private class HaarpsichordModifier : IStealModifier
        {
            private HaarpsichordMemory memory;
            public bool Enabled { get; set; } = true;

            public HaarpsichordModifier(HaarpsichordMemory memory)
            {
                this.memory = memory;
            }

            IStealOption IStealModifier.Modify(IStealOption option)
            {
                if (Enabled)
                {
                    return new StealingWhileHaarpsichordWatches(memory, option);
                }
                else
                {
                    return option;
                }
            }
        }

        private class StealingWhileHaarpsichordWatches : IStealOption
        {
            private HaarpsichordMemory memory;
            private IStealOption original;
            public string Art => original.Art;

            public StealingWhileHaarpsichordWatches(HaarpsichordMemory memory, IStealOption original)
            {
                this.memory = memory;
                this.original = original;
            }

            public bool IsLegal()
            {
                return !memory.AgendaStolenThisTurn && original.IsLegal();
            }

            async public Task<bool> Perform()
            {
                if (memory.AgendaStolenThisTurn)
                {
                    throw new System.Exception("Illegal steal from Haarpsichord");
                }
                var stolen = await original.Perform();
                if (stolen)
                {
                    memory.AgendaStolenThisTurn = true;
                }
                return stolen;
            }
        }
    }
}
