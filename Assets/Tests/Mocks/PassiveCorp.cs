﻿using System.Threading.Tasks;
using model;

namespace tests.mocks
{
    public class PassiveCorp
    {
        private Game game;

        public PassiveCorp(Game game)
        {
            this.game = game;
        }

        async public Task SkipTurn()
        {
            SkipPaidWindow();
            var clickForCredit = game.corp.Acting.credit;
            SkipPaidWindow();
            for (int i = 0; i < 3; i++)
            {
                await clickForCredit.Trigger();
                SkipPaidWindow();
            }
            DiscardRandomCards();
            SkipPaidWindow();
        }

        private void SkipPaidWindow()
        {
            game.corp.paidWindow.Pass();
            game.runner.paidWindow.Pass();
        }

        public void DiscardRandomCards()
        {
            var hq = game.corp.zones.hq;
            hq.Discard(hq.Random(), game.corp.zones.archives);
        }
    }
}
