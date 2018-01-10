using UnityEngine;
using System.Collections;

public class PlayASpellCardCommand: Command
{
    private CardLogic card;
    private Player p;
    //private ICharacter target;

    public PlayASpellCardCommand(Player p, CardLogic card)
    {
        this.card = card;
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        // przenieś tą kartę do sloptu
        p.PArea.handVisual.PlayASpellFromHand(card.UniqueCardID);
        // zrobić wszystkie wizualne rzeczy (dla każdego z nich osobno ????)
    }
}
