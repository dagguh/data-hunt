using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.player;

namespace model
{
    public class ClickPool
    {
        public int NextReplenishment { get; private set; }
        private int allotted = 0;
        public int Spent { get; private set; } = 0;
        public int Remaining { get { return allotted - Spent; } }
        public event Action<ClickPool> Changed = delegate { };

        public ClickPool(int defaultReplenishment)
        {
            NextReplenishment = defaultReplenishment;
        }

        public void Spend(int clicks)
        {
            if (Remaining >= clicks)
            {
                Spent += clicks;
                Changed(this);
            }
            else
            {
                throw new Exception("Cannot spend a click, because all of them are spent");
            }
        }

        public void Replenish()
        {
            Gain(NextReplenishment);
        }

        private void Gain(int clicks)
        {
            allotted += clicks;
            Changed(this);
        }

        public void Lose(int loss)
        {
            if (allotted >= loss)
            {
                allotted -= loss;
            }
            else
            {
                allotted = 0;
            }
            Changed(this);
        }

        public void Reset()
        {
            allotted = 0;
            Spent = 0;
            Changed(this);
        }

        public ICost Spending(int clicksToSpend) => new SpendClicks(this, clicksToSpend);

        private class SpendClicks : ICost
        {
            private ClickPool pool;
            private int clicksToSpend;
            public bool Payable => pool.Remaining >= clicksToSpend;
            public event Action<ICost, bool> ChangedPayability = delegate { };

            public SpendClicks(ClickPool pool, int clicksToSpend)
            {
                this.pool = pool;
                this.clicksToSpend = clicksToSpend;
                pool.Changed += (_) => ChangedPayability(this, Payable);
            }

            async public Task Pay()
            {
                pool.Spend(clicksToSpend);
                await Task.CompletedTask;
            }
        }

        public IEffect Losing(int clicksToLose) => new LoseClicks(this, clicksToLose);

        private class LoseClicks : IEffect
        {
            private ClickPool pool;
            private int clicksToLose;
            public bool Impactful => pool.Remaining >= clicksToLose;
            public event System.Action<IEffect, bool> ChangedImpact = delegate { };
            IEnumerable<string> IEffect.Graphics => new string[] { "" };

            public LoseClicks(ClickPool pool, int clicks)
            {
                this.pool = pool;
                this.clicksToLose = clicks;
                pool.Changed += (_) => ChangedImpact(this, Impactful);
            }

            async Task IEffect.Resolve()
            {
                pool.Lose(clicksToLose);
                await Task.CompletedTask;
            }
        }
    }
}
