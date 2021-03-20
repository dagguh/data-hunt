﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace model.costs
{
    /// <summary>
    /// A conjunction of <em>independent</em> costs.
    /// </summary>
    public class Conjunction : ICost
    {
        private ICost[] costs;
        private IDictionary<ICost, bool> payabilities = new Dictionary<ICost, bool>();
        public bool Payable => costs.All(it => it.Payable);
        public event Action<ICost, bool> ChangedPayability = delegate { };

        public Conjunction(params ICost[] costs)
        {
            this.costs = costs;
            foreach (var cost in costs)
            {
                cost.ChangedPayability += UpdatePayability;
            }
        }

        private void UpdatePayability(ICost source, bool payable)
        {
            payabilities[source] = payable;
            var allPayable = payabilities.All(payability => payability.Value);
            ChangedPayability(this, allPayable);
        }

        async Task ICost.Pay()
        {
            foreach (var cost in costs)
            {
                await cost.Pay();
            }
        }

        public override string ToString() => "Conjunction(costs=" + String.Join(", ", costs.ToList()) + ")";
    }
}
