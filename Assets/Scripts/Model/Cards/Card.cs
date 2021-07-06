using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.choices.trash;
using model.play;
using model.player;
using model.steal;
using model.timing;
using model.zones;

namespace model.cards {
    public abstract class Card : ISource {
        public event NotifyMoved Moved = delegate { };
        public event NotifyInfo ChangedInfo = delegate { };
        public event Action<ISource> ChangedActivation = delegate { };
        public abstract string Name { get; }
        public abstract IType Type { get; }
        public Zone Zone { get; private set; }
        public abstract ICost PlayCost { get; }
        public abstract Faction Faction { get; }
        public abstract int InfluenceCost { get; }
        public abstract string FaceupArt { get; }
        public bool Faceup { get; private set; } = false;
        public Information Information { get; private set; } = Information.HIDDEN_FROM_ALL;
        public bool Active { get; private set; } = false;
        public IList<ITimingStructure> Used { get; private set; } = new List<ITimingStructure>();
        public IPilot Controller { get; private set; }
        private bool Installed = false; // CR: 8.1.1
        private bool Rezzed => Installed && Faceup && Type.Rezzable; // CR: 8.1.2
        private bool Unrezzed => Installed && !Faceup && !Type.Playable && !Type.Runner; // CR: 8.1.2
        public virtual IList<IStealOption> StealOptions() => Type.DefaultStealing(this);
        public virtual IList<ITrashOption> TrashOptions() => new List<ITrashOption>();
        protected Game game;

        public Card(Game game) {
            this.game = game;
            this.Controller = game.Pilot(Faction.Side);
            this.Zone = new Zone("Outside of the game", false);
            this.Zone.Add(this);
        }

        async public Task BecomeActive() {
            await Activate();
            Active = true;
            ChangedActivation(this);
        }

        protected abstract Task Activate();

        async protected virtual Task BecomeInactive() {
            await Deactivate();
            Active = false;
            ChangedActivation(this);
        }

        protected virtual Task Deactivate() {
            return Task.CompletedTask;
        }

        async public Task MoveTo(Zone target) {
            var source = Zone;
            if (source == target) {
                throw new System.Exception("Tried to move " + Name + " from " + source.Name + " to " + target.Name);
            }
            source.Remove(this);
            target.Add(this);
            Zone = target;
            if (!Zone.InPlayArea) {
                Installed = false;
            }
            await UpdateActivity();
            Moved(this, source, target);
        }

        async private Task UpdateActivity() {
            if (Zone.InPlayArea && Faceup) // CR: 1.8.3.a
            {
                if (!Active) {
                    Active = true;
                    await Activate();
                }
            } else {
                if (Active) {
                    Active = false;
                    await Deactivate();
                }
            }
        }

        async public Task SetInstalled() {
            Installed = true;
            await UpdateActivity();
            if (Type.Rezzable) {
                game.corp.Rezzing.Track(this);
            }
        }

        internal void FlipPreInstall() {
            if (Type.Corp) {
                FlipFaceDown();
            }
            if (Type.Runner) {
                FlipFaceUp();
            }
        }

        public IList<IInstallDestination> FindInstallDestinations() {
            return Type.FindInstallDestinations();
        }

        public void FlipFaceUp() {
            Faceup = true;
            UpdateInfo(Information.OPEN);
        }

        private void FlipFaceDown() {
            Faceup = false;
            if (Type.Corp) {
                UpdateInfo(Information.HIDDEN_FROM_RUNNER);
            }
            if (Type.Runner) {
                UpdateInfo(Information.HIDDEN_FROM_CORP);
            }
        }

        public void UpdateInfo(Information information) {
            Information = information;
            ChangedInfo(this, Information);
        }

        public override string ToString() {
            return Name + " [" + GetHashCode() + "]";
        }
    }

    public delegate void NotifyActivity(Card card, bool active);
    public delegate void NotifyMoved(Card card, Zone source, Zone target);
    public delegate void NotifyInfo(Card card, Information information);
}
