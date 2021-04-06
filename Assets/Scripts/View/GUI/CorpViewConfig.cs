using model;
using static view.gui.GameObjectExtensions;

namespace view.gui
{
    public class CorpViewConfig
    {
        public CorpView Display(Game game, BoardParts parts)
        {
            var zones = game.corp.zones;
            var credits = FindOrFail("Corp/Credits");
            game.corp.credits.Changed += credits.AddComponent<CreditSpiral>().UpdateBalance;
            game.corp.credits.Changed += credits.AddComponent<PileCount>().UpdateBalance;
            var servers = new ServerRow(FindOrFail("Servers"), parts, zones);
            var archivesBox = servers.Box(zones.archives);
            archivesBox.Printer.Sideways = true;
            var rd = servers.Box(zones.rd).Printer.PrintCorpFacedown("Top of R&D");
            var hq = servers.Box(zones.hq);
            game.corp.zones.identity.Added += (zone, identity) => hq.Printer.Print(identity);
            zones.archives.Zone.Changed += archivesBox.gameObject.AddComponent<PileCount>().UpdateCardCount; // TODO renders under the pile due to children-order
            archivesBox.ShowCards();
            zones.hq.Zone.Changed += hq.gameObject.AddComponent<PileCount>().UpdateCardCount;
            zones.rd.Zone.Changed += rd.AddComponent<PileCount>().UpdateCardCount;
            return new CorpView(servers);
        }
    }
}
