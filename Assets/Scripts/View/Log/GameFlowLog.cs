using System.Threading.Tasks;
using model;
using model.timing;
using UnityEngine;

namespace view.log
{
    public class GameFlowLog
    {
        private string currentStep = "";

        public void Display(Game game)
        {
            game.corp.paidWindow.Opened += LogOpened;
            game.corp.paidWindow.Closed += LogClosed;
            game.runner.paidWindow.Opened += LogOpened;
            game.runner.paidWindow.Closed += LogClosed;
            var runnerTurn = game.runner.turn;
            var corpTurn = game.corp.turn;
            corpTurn.Opened += TurnStarted;
            corpTurn.TakingAction += (turn) => LogAsync("corp taking action");
            corpTurn.ActionTaken += (turn, action) => LogAsync("corp took action");
            game.corp.Rezzing.Window.Opened += (window, rezzables) => LogAsync("rez window opened, up to " + rezzables.Count + " could be rezzed");
            game.corp.Rezzing.Window.Closed += (window) => Log("rez window closed");
            game.corp.zones.hq.DiscardingOne += () => Log("discarding");
            runnerTurn.Opened += TurnStarted;
            runnerTurn.TakingAction += (turn) => LogAsync("runner taking action");
            runnerTurn.ActionTaken += (turn, action) => LogAsync("runner took action");
            game.runner.zones.grip.DiscardingOne += () => Log("discarding");
        }

        async private Task LogAsync(string message)
        {
            Debug.Log(currentStep + message);
            await Task.CompletedTask;
        }

        private void Log(string message)
        {
            Debug.Log(currentStep + message);
        }

        private void LogOpened(PaidWindow window)
        {
            Log(window + " opened");
        }

        private void LogClosed(PaidWindow window)
        {
            Log(window + " closed");
        }

        async private Task TurnStarted(ITurn turn)
        {
            Log("turn " + turn + " beginning");
            await Task.CompletedTask;
        }
    }
}
