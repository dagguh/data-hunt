﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace model.effects
{
    public class Pass : IEffect
    {
        public bool Impactful => true;
        public event Action<IEffect, bool> ChangedImpact;
        async Task IEffect.Resolve() => await Task.CompletedTask;
        IEnumerable<string> IEffect.Graphics => new string[] { };
    }
}
