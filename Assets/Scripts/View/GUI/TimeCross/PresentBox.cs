using System.Threading.Tasks;
using model;
using model.cards;
using model.play;
using model.timing;
using UnityEngine;
using UnityEngine.UI;
using static view.gui.GameObjectExtensions;

namespace view.gui.timecross
{
    public class PresentBox : MonoBehaviour
    {
        public GameObject RunnerActionPhase { get; private set; }
        private GameObject corpActionPhase;
        public GameObject BankCredit { get; private set; }
        private GameObject discardPhase;
        private Image discardBackground;
        private Game game;
        private DayNightCycle dayNight;
        private FutureTrack future;

        internal void Wire(Game game, DayNightCycle dayNight, FutureTrack future)
        {
            this.game = game;
            this.dayNight = dayNight;
            this.future = future;
            WireRunnerActionPhase(game);
            WireCorpActionPhase(game);
            WireDiscardPhase(game);
        }

        private void WireRunnerActionPhase(Game game)
        {
            RunnerActionPhase = FindOrFail("Runner action phase");
            var background = RunnerActionPhase.GetComponent<Image>();
            dayNight.Paint(background, Side.RUNNER);
            BankCredit = FindOrFail("Bank/Credit");
            SetRunnerActions(false);
            game.runner.turn.TakingAction += BeginRunnerAction;
            game.runner.turn.ActionTaken += EndRunnerAction;
        }

        private void SetRunnerActions(bool takingAction)
        {
            RunnerActionPhase.gameObject.SetActive(takingAction);
            BankCredit.SetActive(takingAction);
        }

        private void WireCorpActionPhase(Game game)
        {
            corpActionPhase = FindOrFail("Corp action phase");
            var background = corpActionPhase.GetComponent<Image>();
            dayNight.Paint(background, Side.CORP);
            corpActionPhase.SetActive(false);
            game.corp.turn.TakingAction += BeginCorpAction;
            game.corp.turn.ActionTaken += EndCorpAction;
        }

        private void WireDiscardPhase(Game game)
        {
            discardPhase = FindOrFail("Discard phase");
            discardBackground = discardPhase.GetComponent<Image>();
            discardPhase.SetActive(false);
            game.runner.zones.grip.DiscardingOne += RenderRunnerDiscarding;
            game.runner.zones.grip.DiscardedOne += RenderRunnerNotDiscardingAnymore;
            game.corp.zones.hq.DiscardingOne += RenderCorpDiscarding;
            game.corp.zones.hq.DiscardedOne += RenderCorpNotDiscardingAnymore;
        }

        async private Task BeginRunnerAction(ITurn turn)
        {
            SetRunnerActions(true);
            future.CurrentTurn.UpdateClicks(game.runner.clicks.Remaining - 1);
            await Task.CompletedTask;
        }

        async private Task EndRunnerAction(ITurn turn, Ability action)
        {
            SetRunnerActions(false);
            await Task.CompletedTask;
        }

        async private Task BeginCorpAction(ITurn turn)
        {
            corpActionPhase.SetActive(true);
            future.CurrentTurn.UpdateClicks(game.corp.clicks.Remaining - 1);
            await Task.CompletedTask;
        }

        async private Task EndCorpAction(ITurn turn, Ability action)
        {
            corpActionPhase.SetActive(false);
            await Task.CompletedTask;
        }

        private void RenderRunnerDiscarding()
        {
            discardPhase.SetActive(true);
            dayNight.Paint(discardBackground, Side.RUNNER);
        }

        private void RenderRunnerNotDiscardingAnymore(Card discarded)
        {
            discardPhase.SetActive(false);
        }

        private void RenderCorpDiscarding()
        {
            discardPhase.SetActive(true);
            dayNight.Paint(discardBackground, Side.CORP);
        }

        private void RenderCorpNotDiscardingAnymore(Card discarded)
        {
            discardPhase.SetActive(false);
        }
    }
}
