using UnityEngine;
using System.Collections;

/// <summary>
/// Ten skrypt powinien być dołączony do obiektu gry reprezentującego kartę, aby poprawnie obrócić kartą
/// </summary>

[ExecuteInEditMode]
public class BetterCardRotation : MonoBehaviour
{

    // Główny gameobject odpowiadający za przód karty
    public RectTransform CardFront;

    // Główny gameobject odpowiadający za tył karty
    public RectTransform CardBack;

    // Pusty gameobject, który znajduje się nieco ponad powierzchnią karty, pośrodku karty
    public Transform targetFacePoint;

    // 3D collider dołączony do karty
    public Collider col;

    // Jeśli ta zmienna jest "true" pokazuje tył karty
    private bool showingBack = false;

    void Update()
    {
        // Raycast z kamery do punktu docelowego na powierzchni karty
        // Jeśli przejdzie przez collider karty, pokaże się tył karty
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
                                  direction: (-Camera.main.transform.position + targetFacePoint.position).normalized,
            maxDistance: (-Camera.main.transform.position + targetFacePoint.position).magnitude);
        bool passedThroughColliderOnCard = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                passedThroughColliderOnCard = true;
        }
        //Debug.Log("TotalHits: " + hits.Length); 
        if (passedThroughColliderOnCard != showingBack)
        {
            showingBack = passedThroughColliderOnCard;
            if (showingBack)
            {
                // pokaż tył kartu
                CardFront.gameObject.SetActive(false);
                CardBack.gameObject.SetActive(true);
            }
            else
            {
                // pokaż front karty
                CardFront.gameObject.SetActive(true);
                CardBack.gameObject.SetActive(false);
            }

        }

    }
}