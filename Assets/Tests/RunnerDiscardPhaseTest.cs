using NUnit.Framework;
using model;
using model.cards;
using System.Collections.Generic;
using model.cards.runner;
using tests.observers;
using tests.mocks;

namespace tests
{
    public class RunnerDiscardPhaseTest
    {
        [Test]
        async public void ShouldDiscard()
        {
            var game = MockGames.Unpiloted();
            var runnerCards = new List<Card>();
            for (int i = 0; i < 20; i++)
            {
                runnerCards.Add(new Diesel(game));
            }
            game.Start(Decks.DemoCorp(game), MockGames.MasqueDeck(game, runnerCards));
            var passiveCorp = new PassiveCorp(game);
            await passiveCorp.SkipTurn();
            var grip = game.runner.zones.grip;
            var heap = game.runner.zones.heap;
            var actionCard = game.runner.Acting;
            var gripObserver = new ZoneObserver(grip.zone);
            var heapObserver = new ZoneObserver(heap.zone);
            for (int i = 0; i < 3; i++)
            {
                var diesel = grip.Find<Diesel>();
                await actionCard.TakeAction();
                await actionCard.Play(diesel).Trigger();
            }
            await actionCard.draw.Trigger();

            for (int i = 0; i < 7; i++)
            {
                var card = grip.Find<Diesel>();
                await game.runner.zones.grip.Discard(card, heap);
            }
            await passiveCorp.SkipTurn();

            Assert.AreEqual(10, gripObserver.TotalRemoved);
            Assert.AreEqual(10, heapObserver.TotalAdded);
        }
    }
}
