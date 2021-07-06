using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.player;
using model.timing.corp;
using model.timing.runner;

namespace model.timing {
    public class Timing {
        private Game game;
        private Checkpoint checkpoint;
        public event Action<CorpTurn> CorpTurnDefined = delegate { };
        public event Action<RunnerTurn> RunnerTurnDefined = delegate { };
        public event Action<PaidWindow> PaidWindowDefined = delegate { };
        public event Action<ITurn> CurrentTurnQueued = delegate { };
        public event Action<ITurn> NextTurnPredicted = delegate { };
        public event Action<GameFinish> Finished = delegate { };
        private bool gameEnded = false;
        private Queue<ITurn> turns = new Queue<ITurn>();
        private IPilot active;
        private IPilot inactive;
        private int corpTurns = 0;
        private int runnerTurns = 0;

        public Timing(Game game) {
            this.game = game;
            game.corp.zones.rd.Decked += DeckCorp;
            game.runner.zones.score.StolenEnough += StealEnough;
        }

        async public Task StartTurns() {
            try {
                while (!gameEnded) {
                    var corpTurn = new CorpTurn(game.corp, this);
                    var runnerTurn = new RunnerTurn(game.runner, this, runnerTurns++);
                    CorpTurnDefined(corpTurn);
                    RunnerTurnDefined(runnerTurn);
                    turns.Enqueue(corpTurn);
                    turns.Enqueue(runnerTurn);
                    await StartNextTurn();
                    await StartNextTurn();
                }
            }
            catch (Exception e) {
                if (gameEnded) {
                    UnityEngine.Debug.Log("The game is over! " + e.Message);
                } else {
                    throw new Exception("Failed a turn", e);
                }
            }
        }

        private async Task StartNextTurn() {
            var currentTurn = turns.Dequeue();
            CurrentTurnQueued(currentTurn);
            NextTurnPredicted(turns.Peek());
            UpdateActivePlayer(currentTurn.Owner);
            await currentTurn.Initiate();
        }

        private void UpdateActivePlayer(IPilot active) {
            this.active = active;
            if (active == game.corp.pilot) {
                inactive = game.runner.pilot;
            } else {
                inactive = game.corp.pilot;
            }
        }

        async public Task OpenActionWindow() {
            var window = new ActionWindow();
            await window.Open();
        }

        public PaidWindow DefinePaidWindow(bool rezzing, bool scoring) {
            var window = new PaidWindow(
                 rezzing,
                 scoring,
                 acting: active,
                 reacting: inactive
              );
            PaidWindowDefined(window);
            return window;
        }

        async public Task OpenPaidWindow(PaidWindow acting, PaidWindow reacting) {
            var bothPlayersCouldAct = false;
            while (true) {
                var actingDeclined = await acting.AwaitPass();
                if (actingDeclined && bothPlayersCouldAct) {
                    break;
                }
                var reactingDeclined = await reacting.AwaitPass();
                bothPlayersCouldAct = true;
                if (reactingDeclined && bothPlayersCouldAct) {
                    break;
                }
            }
        }

        private void DeckCorp(Corp corp) {
            Finish(new GameFinish(
                winner: "The Runner",
                loser: "The Corp",
                reason: "Corp R&D is empty"
            ));
        }

        private void StealEnough() {
            Finish(new GameFinish(
                winner: "The Runner",
                loser: "The Corp",
                reason: "Runner has stolen enough"
            ));
        }

        private void Finish(GameFinish finish) {
            gameEnded = true;
            Finished(finish);
            throw new Exception("Game over, " + finish.reason);
        }

        async public Task Checkpoint() {
            await checkpoint.Check();
        }
    }

    public class GameFinish {
        public string winner;
        public string loser;
        public string reason;

        public GameFinish(string winner, string loser, string reason) {
            this.winner = winner;
            this.loser = loser;
            this.reason = reason;
        }
    }
}
