using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragCreatureOnTable : DraggingActions {

    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private IDHolder idScript;
    private VisualStates tempState;
    private OneCardManager manager;

    public override bool CanDrag
    {
        get
        {
            //TEST LINE: to jest tylko do testu grania stworzeniami, przed kompletną grą
            //return true;

            // TODO : obejmuj pełną kontrolę pola
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        manager = GetComponent<OneCardManager>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;
        tempState = whereIsCard.VisualState;
        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

    }

    public override void OnDraggingInUpdate()
    {

    }

    public override void OnEndDrag()
    {

        // 1) Sprawdź, czy trzymamy kartę nad polem bitwy
        if (DragSuccessful())
        {
            // określ pozycję na polu bitwy
            int tablePos = playerOwner.PArea.tableVisual.TablePosForNewCreature(Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x);
            // Debug.Log("Table Pos dla nowego stworzenia: " + tablePos.ToString());
            // zagraj kartę
            playerOwner.PlayACreatureFromHand(GetComponent<IDHolder>().UniqueID, tablePos);
        }
        else
        {
            // Ustaw stary porządek sortowania
            whereIsCard.SetHandSortingOrder();
            whereIsCard.VisualState = tempState;
            // Przenieś tę kartę z powrotem do jej gniazda
            HandVisual PlayerHand = playerOwner.PArea.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
        } 
    }

    protected override bool DragSuccessful()
    {
        bool TableNotFull = (playerOwner.table.CreaturesOnTable.Count < 8);

        return TableVisual.CursorOverSomeTable && TableNotFull;
    }
}
