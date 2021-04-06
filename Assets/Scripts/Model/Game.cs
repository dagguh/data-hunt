using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model.player;
using model.timing;
using model.zones;

namespace model
{
    public class Game
    {
        public readonly Corp corp;
        public readonly Runner runner;
        private readonly Zone playArea;
        public readonly Checkpoint checkpoint;
        public event Action<ITurn> CurrentTurn = delegate { };
        public event Action<ITurn> NextTurn = delegate { };
        public event Action<GameFinish> Finished = delegate { };
        private bool ended = false;
        private Queue<ITurn> turns = new Queue<ITurn>();
        private Shuffling shuffling;

        public Game(IPilot corpPilot, IPilot runnerPilot, Shuffling shuffling)
        {
            this.shuffling = shuffling;
            playArea = new Zone("Play area", true);
            corp = CreateCorp(corpPilot);
            runner = CreateRunner(runnerPilot);
            this.checkpoint = new Checkpoint(this);
            corp.zones.rd.Decked += DeckCorp;
            runner.zones.score.StolenEnough += StealEnough;
        }

        private Corp CreateCorp(IPilot pilot)
        {
            var turn = new timing.corp.CorpTurn(this);
            var paidWindow = new PaidWindow("corp");
            return new Corp(pilot, turn, paidWindow, playArea, shuffling, new Random());
        }

        private Runner CreateRunner(IPilot pilot)
        {
            var turn = new timing.runner.RunnerTurn(this);
            var paidWindow = new PaidWindow("runner");
            return new Runner(pilot, turn, paidWindow, playArea, shuffling, this);
        }

        async public void Start(Deck corpDeck, Deck runnerDeck)
        {
            await corp.Start(this, corpDeck);
            await runner.Start(this, runnerDeck);
            await StartTurns();
        }

        async private Task StartTurns()
        {
            turns.Enqueue(corp.turn);
            turns.Enqueue(runner.turn);
            try
            {
                while (!ended)
                {
                    turns.Enqueue(corp.turn);
                    turns.Enqueue(runner.turn);
                    await StartNextTurn();
                    await StartNextTurn();
                }
            }
            catch (Exception e)
            {
                if (ended)
                {
                    UnityEngine.Debug.Log("The game is over! " + e.Message);
                }
                else
                {
                    throw new Exception("Failed a turn", e);
                }
            }
        }

        private async Task StartNextTurn()
        {
            var currentTurn = turns.Dequeue();
            CurrentTurn(currentTurn);
            NextTurn(turns.Peek());
            await currentTurn.Start();
        }

        async public Task OpenPaidWindow(PaidWindow acting, PaidWindow reacting)
        {
            var bothPlayersCouldAct = false;
            while (true)
            {
                var actingDeclined = await acting.AwaitPass();
                if (actingDeclined && bothPlayersCouldAct)
                {
                    break;
                }
                var reactingDeclined = await reacting.AwaitPass();
                bothPlayersCouldAct = true;
                if (reactingDeclined && bothPlayersCouldAct)
                {
                    break;
                }
            }
        }

        private void DeckCorp(Corp corp)
        {
            Finish(new GameFinish(
                winner: "The Runner",
                loser: "The Corp",
                reason: "Corp R&D is empty"
            ));
        }

        private void StealEnough()
        {
            Finish(new GameFinish(
                winner: "The Runner",
                loser: "The Corp",
                reason: "Runner has stolen enough"
            ));
        }

        private void Finish(GameFinish finish)
        {
            ended = true;
            Finished(finish);
            throw new Exception("Game over, " + finish.reason);
        }

        async public Task Checkpoint()
        {
            await checkpoint.Check();
        }
    }

    public class GameFinish
    {
        public string winner;
        public string loser;
        public string reason;

        public GameFinish(string winner, string loser, string reason)
        {
            this.winner = winner;
            this.loser = loser;
            this.reason = reason;
        }
    }
}
