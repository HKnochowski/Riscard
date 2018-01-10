using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragSpellNoTarget: DraggingActions{

    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;

        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

    }

    public override void OnDraggingInUpdate()
    {
        
    }

    public override void OnEndDrag()
    {
        // 1) Sprawdz czy trzymamy kartę nad polem bitwy
        if (DragSuccessful())
        {
            // zagraj tą kartę
            playerOwner.PlayASpellFromHand(GetComponent<IDHolder>().UniqueID, -1);
        }
        else
        {
            // Ustaw stary porządek sortowania 
            whereIsCard.Slot = savedHandSlot;
            if (tag.Contains("Low"))
                whereIsCard.VisualState = VisualStates.LowHand;
            else
                whereIsCard.VisualState = VisualStates.TopHand;
            // Przenieś tę kartę z powrotem do jej gniazda
            HandVisual PlayerHand = playerOwner.PArea.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
        } 
    }

    protected override bool DragSuccessful()
    {
        //bool TableNotFull = (TurnManager.Instance.whoseTurn.table.CreaturesOnTable.Count < 8);

        return TableVisual.CursorOverSomeTable; //&& TableNotFull;
    }


}
