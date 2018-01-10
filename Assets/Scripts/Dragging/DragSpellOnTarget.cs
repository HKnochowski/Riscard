using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragSpellOnTarget : DraggingActions {

    public TargetingOptions Targets = TargetingOptions.AllCharacters;
    private SpriteRenderer sr;
    private LineRenderer lr;
    private WhereIsTheCardOrCreature whereIsThisCard;
    private VisualStates tempVisualState;
    private Transform triangle;
    private SpriteRenderer triangleSR;
    private GameObject Target;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

        whereIsThisCard = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override void OnStartDrag()
    {
        tempVisualState = whereIsThisCard.VisualState;
        whereIsThisCard.VisualState = VisualStates.Dragging;
        sr.enabled = true;
        lr.enabled = true;
    }

    public override void OnDraggingInUpdate()
    {
        // Ten kod rysuje tylko strzałkę
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
        // TODO: Zapisywanie gdzieś rezultatów 
        hits = Physics.RaycastAll(origin: Camera.main.transform.position, 
            direction: (-Camera.main.transform.position + this.transform.position).normalized, 
            maxDistance: 30f) ;

        foreach (RaycastHit h in hits)
        {
            if (h.transform.tag.Contains("Player"))
            {
                // wybranie gracza
                Target = h.transform.gameObject;
            }
            else if (h.transform.tag.Contains("Creature"))
            {
                // zaatakuj stwora, zapisz transformację
                Target = h.transform.parent.gameObject;
            }
        }

        bool targetValid = false;

        if (Target != null)
        {
            // określić właściciela tej karty
            Player owner = null; 
            if (tag.Contains("Low"))
                owner = GlobalSettings.Instance.LowPlayer;
            else
                owner = GlobalSettings.Instance.TopPlayer;

            // sprawdź, czy powinniśmy zagrać to zaklęcie w zależności od opcji kierowania
            int targetID = Target.GetComponent<IDHolder>().UniqueID;
            switch (Targets)
            {
                case TargetingOptions.AllCharacters: 
                    owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                    targetValid = true;
                    break;
                case TargetingOptions.AllCreatures:
                    if (Target.tag.Contains("Creature"))
                    {
                        owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                        targetValid = true;
                    }
                    break;
                case TargetingOptions.EnemyCharacters:
                    if (Target.tag.Contains("Creature") || Target.tag.Contains("Player"))
                    {
                        // Sprawdzenie czy cel nie jest kartą
                        if ((tag.Contains("Low") && Target.tag.Contains("Top"))
                           || (tag.Contains("Top") && Target.tag.Contains("Low")))
                        {
                            owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                            targetValid = true;
                        }
                    }
                    break;
                case TargetingOptions.EnemyCreatures:
                    if (Target.tag.Contains("Creature"))
                    {
                        // Sprawdzenie czy cel nie jest kartą, lub graczem
                        if ((tag.Contains("Low") && Target.tag.Contains("Top"))
                            || (tag.Contains("Top") && Target.tag.Contains("Low")))
                        {
                            owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                            targetValid = true;
                        }
                    }
                    break;
                case TargetingOptions.YourCharacters:
                    if (Target.tag.Contains("Creature") || Target.tag.Contains("Player"))
                    {
                        // Sprawdzenie czy cel nie jest kartą
                        if ((tag.Contains("Low") && Target.tag.Contains("Low"))
                            || (tag.Contains("Top") && Target.tag.Contains("Top")))
                        {
                            owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                            targetValid = true;
                        }
                    }
                    break;
                case TargetingOptions.YourCreatures:
                    if (Target.tag.Contains("Creature"))
                    {
                        // Sprawdzenie czy cel nie jest kartą lub graczem
                        if ((tag.Contains("Low") && Target.tag.Contains("Low"))
                            || (tag.Contains("Top") && Target.tag.Contains("Top")))
                        {
                            owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                            targetValid = true;
                        }
                    }
                    break;
                default:
                    Debug.LogWarning("Reached default case in DragSpellOnTarget! Suspicious behaviour!!");
                    break;
            }
        }

        if (!targetValid)
        {
            // Jeśli nie jest prawidłowym celem, wróć
            whereIsThisCard.VisualState = tempVisualState;
            whereIsThisCard.SetHandSortingOrder();
        }

        // powrót strzałki i celu do pierwotnej pozycji
        // ta pozycja jest szczególna dla kart zaklęć, aby pokazać strzałkę na górze
        transform.localPosition = new Vector3(0f, 0f, 0.4f);
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;

    }

    // NIE UŻYWANE W TYM SKRYPCIE
    protected override bool DragSuccessful()
    {
        return true;
    }
}
