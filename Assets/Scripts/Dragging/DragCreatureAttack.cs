using UnityEngine;
using System.Collections;

public class DragCreatureAttack : DraggingActions {

    // odniesienie do sprite'a z okrągłą grafiką "Cel"
    private SpriteRenderer sr;
    // LineRenderer, który jest dołączony do obiektu gry potomnej, aby narysować strzałkę
    private LineRenderer lr;
    // odnośnik do WhereIsTheCardOrCreature, aby śledzić stan tego obiektu w grze
    private WhereIsTheCardOrCreature whereIsThisCreature;
    
    private Transform triangle;
    // SpriteRenderer z trójkąta. Potrzebujemy tego, aby wyłączyć spiczasty koniec, jeśli cel jest zbyt blisko.
    private SpriteRenderer triangleSR;
    // gdy przestaniemy przeciągać, obiekt GameObject, na który celowaliśmy, będzie przechowywany w tej zmiennej.
    private GameObject Target;
    // Odwołanie do CreatureManager, dołączonego do nadrzędnego obiektu gry
    private OneCreatureManager manager;

    void Awake()
    {
        // ustal wszystkie połączenia
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

        manager = GetComponentInParent<OneCreatureManager>();
        whereIsThisCreature = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override bool CanDrag
    {
        get
        {
            // TEST LINE: tylko do testu
            //return true;

            // możemy przeciągnąć tę kartę, jeśli
            // a) możemy kontrolować naszego gracza (jest to zaznaczone w base.canDrag)
            // b) stworzenie "CanAttackNow" - ta informacja pochodzi z logicznej części naszego kodu do skryptu CreatureManager
            return base.CanDrag && manager.CanAttackNow;
        }
    }

    public override void OnStartDrag()
    {
        whereIsThisCreature.VisualState = VisualStates.Dragging;
        // włącz grafikę celownika
        sr.enabled = true;
        // włącz LineRenderer, aby rozpocząć rysowanie linii.
        lr.enabled = true;
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction*2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            // narysuj linię między stworem a celem
            lr.SetPositions(new Vector3[]{ transform.parent.position, transform.position - direction*2.3f });
            lr.enabled = true;

            // ustaw koniec strzałki w pobliżu celu.
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f*direction;

            // właściwe obracanie końca strzałki
            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            // jeśli cel nie znajduje się wystarczająco daleko od stwora, nie pokazuj strzałki
            lr.enabled = false;
            triangleSR.enabled = false;
        }
            
    }

    public override void OnEndDrag()
    {
        Target = null;
        RaycastHit[] hits;
        // TODO: Zapisywanie w czymś wyników 
        hits = Physics.RaycastAll(origin: Camera.main.transform.position, 
            direction: (-Camera.main.transform.position + this.transform.position).normalized, 
            maxDistance: 30f) ;

        foreach (RaycastHit h in hits)
        {
            if ((h.transform.tag == "TopPlayer" && this.tag == "LowCreature") ||
                (h.transform.tag == "LowPlayer" && this.tag == "TopCreature"))
            {
                
                Target = h.transform.gameObject;
            }
            else if ((h.transform.tag == "TopCreature" && this.tag == "LowCreature") ||
                    (h.transform.tag == "LowCreature" && this.tag == "TopCreature"))
            {
                // uderzyć stwora, zapisać transformację do rodzica
                Target = h.transform.parent.gameObject;
            }
               
        }

        bool targetValid = false;

        if (Target != null)
        {
            int targetID = Target.GetComponent<IDHolder>().UniqueID;
            Debug.Log("Target ID: " + targetID);
            if (targetID == GlobalSettings.Instance.LowPlayer.PlayerID || targetID == GlobalSettings.Instance.TopPlayer.PlayerID)
            {
                // Atakuj bastion
                Debug.Log("Attacking "+Target);
                Debug.Log("TargetID: " + targetID);
                CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].GoFace();
                targetValid = true;
            }
            else if (CreatureLogic.CreaturesCreatedThisGame[targetID] != null)
            {
                // jeśli docelowe stworzenie wciąż żyje, atakuj stwora
                targetValid = true;
                CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].AttackCreatureWithID(targetID);
                Debug.Log("Attacking "+Target);
            }
                
        }

        if (!targetValid)
        {
            // Jeśli nie jest prawidłowym celem, wróć
            if (tag.Contains("Low"))
                whereIsThisCreature.VisualState = VisualStates.LowTable;
            else
                whereIsThisCreature.VisualState = VisualStates.TopTable;
            whereIsThisCreature.SetTableSortingOrder();
        }

        // powrót strzałki i celu do pierwotnej postaci
        transform.localPosition = Vector3.zero;
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;

    }

    // Nie używane w tym skrypcie
    protected override bool DragSuccessful()
    {
        return true;
    }
}
