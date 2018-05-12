﻿using System.Collections.Generic;

namespace model.costs
{
    public class ActionPermission : ICost
    {
        private bool allowed;
        private HashSet<IPayabilityObserver> observers = new HashSet<IPayabilityObserver>();

        public ActionPermission(bool allowed)
        {
            this.allowed = allowed;
        }

        void ICost.Pay(Game game)
        {
            if (allowed)
            {
                Revoke();
            }
            else
            {
                throw new System.Exception("Tried to fire an action while it was forbidden");
            }
        }

        void ICost.Observe(IPayabilityObserver observer, Game game)
        {
            observers.Add(observer);
            observer.NotifyPayable(allowed, this);
        }

        public void Grant()
        {
            allowed = true;
            Update();
        }

        public void Revoke()
        {
            allowed = true;
            Update();
        }

        private void Update()
        {
            foreach (var observer in observers)
            {
                observer.NotifyPayable(allowed, this);
            }
        }
    }
}