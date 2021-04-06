using controller;
using model;
using static view.gui.GameObjectExtensions;

namespace view.gui
{
    public class RunnerViewConfig
    {
        public void Display(Runner runner, GameFlowView flowView, CorpView corpView, BoardParts parts)
        {
            var presentBox = flowView.TimeCross.PresentBox;
            RigGrid rigGrid = new RigGrid(FindOrFail("Rig"), runner, flowView.PaidChoice, parts);
            HeapPile heapPile = new HeapPile(FindOrFail("Heap"), runner, parts);
            GripFan gripFan = new GripFan(FindOrFail("Grip"), runner, presentBox.RunnerActionPhase.AddComponent<DropZone>(), rigGrid.DropZone, heapPile.DropZone, parts);
            new StackPile(FindOrFail("Stack"), runner, gripFan.DropZone, parts);
            new ZoneBox(FindOrFail("Runner/Left hand/Score"), parts).Represent(runner.zones.score.zone);
            runner.zones.identity.Added += (zone, identity) => parts.Print(FindOrFail("Runner/Right hand/Core/Identity")).Print(identity);
            new RunInitiation(
                gameObject: FindOrFail("Run"),
                serverRow: corpView.serverRow,
                runner: runner
            );
            var credits = FindOrFail("Runner/Credits");
            presentBox
                .BankCredit
                .AddComponent<Droppable>()
                .Represent(
                    new InteractiveAbility(runner.Acting.credit, credits.AddComponent<DropZone>())
                );
            runner.credits.Changed += credits.AddComponent<CreditSpiral>().UpdateBalance;
            runner.credits.Changed += credits.AddComponent<PileCount>().UpdateBalance;
        }
    }
}
