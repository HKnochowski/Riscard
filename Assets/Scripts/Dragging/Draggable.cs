using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// Ta klasa włącza funkcję przeciągania i upuszczania dla obiektu gry, do którego jest przyłączona
/// Używa innego skryptu - DraggingActions, aby określić, czy możemy przeciągnąć ten obiekt gry teraz, czy też nie
/// czy wyłożenie zakończyło się sukcesem czy nie.
/// </summary>

public class Draggable : MonoBehaviour {

    // PRIVATE FIELDS

    // Flaga informująca czy aktualnie przeciągamy ten gameObject
    private bool dragging = false;

    // odległość od środka tego obiektu gry do miejsca, w którym kliknęliśmy, aby rozpocząć przeciąganie 
    private Vector3 pointerDisplacement;

    // odległość od kamery do myszy na osi Z.
    private float zDisplacement;

    // odniesienie do skryptu DraggingActions. Przeciąganie Akcje powinny być dołączone do tego samego GameObject.
    private DraggingActions da;

    // Właściwość STATIC, która zwraca wystąpienie przeciągania, które jest obecnie przeciągane
    private static Draggable _draggingThis;
    public static Draggable DraggingThis
    {
        get{ return _draggingThis;}
    }

    //  MONOBEHAVIOUR METODY
    void Awake()
    {
        da = GetComponent<DraggingActions>();
    }

    void OnMouseDown()
    {
        if (da!=null && da.CanDrag)
        {
            dragging = true;
            // kiedy coś przeciągamy, wszystkie podglądy powinny być wyłączone
            HoverPreview.PreviewsAllowed = false;
            _draggingThis = this;
            da.OnStartDrag();
            zDisplacement = -Camera.main.transform.position.z + transform.position.z;
            pointerDisplacement = -transform.position + MouseInWorldCoords();
        }
    }

    // Aktualizacja jest wywoływana raz na klatkę
    void Update ()
    {
        if (dragging)
        { 
            Vector3 mousePos = MouseInWorldCoords();
            //Debug.Log(mousePos);
            transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y, transform.position.z);   
            da.OnDraggingInUpdate();
        }
    }
	
    void OnMouseUp()
    {
        if (dragging)
        {
            dragging = false;
            // włącz ponownie wszystkie podglądy
            HoverPreview.PreviewsAllowed = true;
            _draggingThis = null;
            da.OnEndDrag();
        }
    }

    // Zwraca pozycję myszy we współrzędnych światowych, aby nasz GameObject mógł za nią podążać 
    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        //Debug.Log(screenMousePos);
        screenMousePos.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }
        
}
