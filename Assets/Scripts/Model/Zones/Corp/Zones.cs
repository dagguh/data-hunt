using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.cards;

namespace model.zones.corp
{
    public class Zones
    {
        public readonly Zone identity = new Zone("Corp identity", false);
        public readonly Headquarters hq;
        public readonly ResearchAndDevelopment rd;
        public readonly Archives archives;
        public readonly List<Remote> remotes = new List<Remote>();
        public event Action<Remote> RemoteAdded = delegate { };
        public event Action<Remote> RemoteRemoved = delegate { };
        public readonly Zone playArea;
        private Corp corp;

        public Zones(Corp corp, Zone playArea, Shuffling shuffling)
        {
            hq = new Headquarters(shuffling, corp.credits);
            rd = new ResearchAndDevelopment(corp, shuffling);
            archives = new Archives(corp.credits);
            this.playArea = playArea;
            this.corp = corp;
        }

        public List<IInstallDestination> RemoteInstalls()
        {
            return new List<IInstallDestination>(remotes)
            {
                new NewRemote(this)
            };
        }

        public Remote CreateRemote()
        {
            var remote = new Remote(corp);
            remotes.Add(remote);
            RemoteAdded(remote);
            return remote;
        }

        public void RemoveRemote(Remote remote)
        {
            remotes.Remove(remote);
            RemoteRemoved(remote);
        }

        public IEffect Drawing(int cards)
        {
            return new Draw(cards, rd, hq);
        }

        private class Draw : IEffect
        {
            public bool Impactful { get; private set; }
            public event Action<IEffect, bool> ChangedImpact = delegate { };
            private int cards;
            private ResearchAndDevelopment rd;
            private Headquarters hq;
            IEnumerable<string> IEffect.Graphics => new string[] { "Images/UI/card-draw" };

            public Draw(int cards, ResearchAndDevelopment rd, Headquarters hq)
            {
                this.cards = cards;
                this.rd = rd;
                this.hq = hq;
                rd.Zone.Changed += CountCardsInTheStack;
            }

            private void CountCardsInTheStack(Zone stack)
            {
                Impactful = stack.Count > 0;
                ChangedImpact(this, Impactful);
            }

            async Task IEffect.Resolve()
            {
                await rd.Draw(cards, hq);
                await Task.CompletedTask;
            }
        }

        public IEffect Playing(Card card) => new Play(card, archives.Zone);
    }
}
