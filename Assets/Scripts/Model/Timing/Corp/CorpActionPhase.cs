using System.Threading.Tasks;
using model.play;

namespace model.timing.corp
{
    public class CorpActionPhase : ITimingStructure<CorpActionPhase>
    {
        private Corp corp;
        private Timing timing;
        public string Name => "Corp action phase";
        public event AsyncAction<CorpActionPhase> Opened;
        public event AsyncAction<CorpActionPhase> Closed;
        public event AsyncAction TakingAction;
        public event AsyncAction<Ability> ActionTaken;

        internal CorpActionPhase(Corp corp, Timing timing)
        {
            this.corp = corp;
            this.timing = timing;
        }

        public async Task Open()
        {
            Opened?.Invoke(this);
            var paidWindow = timing.DefinePaidWindow(rezzing: true, scoring: true);
            await paidWindow.Open(); // CR: 5.6.2.a
            while (corp.clicks.Remaining > 0) // CR: 5.6.2.c
            {
                await TakeAction(); // CR: 5.6.2.b
            }
            await timing.Checkpoint(); // CR: 5.6.2.d
        }

        async private Task TakeAction()
        {
            var actionTaking = corp.Acting.TakeAction();
            TakingAction?.Invoke();
            var action = await actionTaking;
            ActionTaken?.Invoke(action);
            await timing.OpenPaidWindow(rezzing: true, scoring: true);
        }
    }
}
