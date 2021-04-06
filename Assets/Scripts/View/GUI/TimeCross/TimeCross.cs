using static view.gui.GameObjectExtensions;
using model;

namespace view.gui.timecross
{
    public class TimeCross
    {
        public PresentBox PresentBox { get; private set; }

        public TimeCross(Game game, DayNightCycle dayNight)
        {
            var pastTrack = FindOrFail("Past").AddComponent<PastTrack>();
            pastTrack.DayNight = dayNight;
            pastTrack.Wire(game);
            var futureTrack = FindOrFail("Future").AddComponent<FutureTrack>();
            futureTrack.Wire(game, dayNight);
            PresentBox = FindOrFail("Present").AddComponent<PresentBox>();
            PresentBox.Wire(game, dayNight, futureTrack);
        }
    }
}
