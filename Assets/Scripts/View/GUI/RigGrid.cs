using System.Collections.Generic;
using controller;
using model;
using model.cards;
using model.play;
using model.timing;
using model.zones;
using UnityEngine;

namespace view.gui
{
    public class RigGrid
    {
        private DropZone paidWindowTrigger;
        public DropZone DropZone { get; private set; }
        private Dictionary<ISource, GameObject> visuals = new Dictionary<ISource, GameObject>();
        private CardPrinter printer;

        public RigGrid(GameObject gameObject, Runner runner, DropZone paidWindowTrigger, BoardParts parts)
        {
            this.paidWindowTrigger = paidWindowTrigger;
            this.printer = parts.Print(gameObject);
            runner.paidWindow.Added += LinkPaidAbility;
            runner.zones.rig.zone.Added += RenderInstalledCard;
            runner.zones.rig.zone.Removed += DestroyUninstalledCard;
            this.DropZone = gameObject.AddComponent<DropZone>();
        }
        private void RenderInstalledCard(Zone zone, Card card)
        {
            visuals[card] = printer.Print(card);
        }

        private void DestroyUninstalledCard(Zone zone, Card card)
        {
            Object.Destroy(visuals[card]);
        }

        private void LinkPaidAbility(PaidWindow window, Ability ability)
        {
            visuals[ability.source]
                .AddComponent<Droppable>()
                .Represent(new InteractiveAbility(ability, paidWindowTrigger));
        }
    }
}
