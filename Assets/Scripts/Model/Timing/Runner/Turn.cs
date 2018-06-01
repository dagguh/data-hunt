﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace model.timing.runner
{
    public class Turn
    {
        private Game game;
        private HashSet<IStepObserver> steps = new HashSet<IStepObserver>();
        private HashSet<IRunnerTurnStartObserver> starts = new HashSet<IRunnerTurnStartObserver>();
        private HashSet<IRunnerActionObserver> actions = new HashSet<IRunnerActionObserver>();

        public Turn(Game game)
        {
            this.game = game;
        }

        async public Task Start()
        {
            await ActionPhase();
            await DiscardPhase();
        }

        async private Task ActionPhase()
        {
            Step(1, 1);
            game.runner.clicks.Gain(4);
            Step(1, 2);
            await OpenPaidWindow();
            OpenRezWindow();
            Step(1, 3);
            RefillRecurringCredits();
            Step(1, 4);
            TriggerTurnBeginning();
            Step(1, 5);
            await OpenPaidWindow();
            OpenRezWindow();

            Step(1, 6);
            await TakeActions();
        }

        async private Task OpenPaidWindow()
        {
            await game.flow.paidWindow.Open();
        }

        private void OpenRezWindow()
        {

        }

        private void RefillRecurringCredits()
        {

        }

        private void TriggerTurnBeginning()
        {
            foreach (var observer in starts)
            {
                observer.NotifyTurnStarted(game);
            }
        }

        async private Task TakeActions()
        {
            while (game.runner.clicks.Remaining() > 0)
            {
                Task actionTaking = game.runner.actionCard.TakeAction();
                foreach (var observer in actions)
                {
                    observer.NotifyActionTaking();
                }
                await actionTaking;
                await OpenPaidWindow();
                OpenRezWindow();
            }
        }

        async private Task DiscardPhase()
        {
            Step(2, 1);
            await Discard();
            Step(2, 2);
            await OpenPaidWindow();
            OpenRezWindow();
            Step(2, 3);
            game.runner.clicks.Reset();
            Step(2, 4);
            TriggerTurnEnding();
        }

        async private Task Discard()
        {
            var grip = game.runner.zones.grip;
            while (grip.Count > 5)
            {
                await grip.Discard();
            }
        }

        private void TriggerTurnEnding()
        {

        }

        private void Step(int phase, int step)
        {
            foreach (var observer in steps)
            {
                observer.NotifyStep("Runner turn", phase, step);
            }
        }

        internal void ObserveSteps(IStepObserver observer)
        {
            steps.Add(observer);
        }

        internal void ObserveStart(IRunnerTurnStartObserver observer)
        {
            starts.Add(observer);
        }

        internal void UnobserveStart(IRunnerTurnStartObserver observer)
        {
            starts.Remove(observer);
        }

        internal void ObserveActions(IRunnerActionObserver observer)
        {
            actions.Add(observer);
        }
    }

    internal interface IRunnerTurnStartObserver
    {
        void NotifyTurnStarted(Game game);
    }

    internal interface IRunnerActionObserver
    {
        void NotifyActionTaking();
    }
}