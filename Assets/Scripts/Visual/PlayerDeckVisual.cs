using UnityEngine;
using System.Collections;
using DG.Tweening;

// Ta klasa powinna być dołączona do talii
// Generuje nowe karty i umieszcza je w ręce
public class PlayerDeckVisual : MonoBehaviour {

    public AreaPosition owner; // Zainicjowanie pola owner typu enum - AreaPosition
    public float HeightOfOneCard = 0.012f; // Zmienna przechowująca wysokość karty (chodzi o oś Z)

    void Start()
    {
        // Odniesienie się do tablicy CardAssetów który znajduje się w GlobalSetting i wyliczenie wielkości tej tablicy
        // Przekazanie wyniku do CardsInDeck
        CardsInDeck = GlobalSettings.Instance.Players[owner].deck.cards.Count;
    }

    private int cardsInDeck = 0; // Zmienna przechowująca ilość kart w talli; standardowo ustawione na 0

    // Funkcja z akcesorami ;get ;set do tej zmiennej
    public int CardsInDeck
    {
        get{ return cardsInDeck; } // Otrzymanie dostępu do wartości w zmiennej cardsInDeck

        set // Otrzymujemy dostęp do zapisania wartości w zmiennej cardsInDeck
        {
            cardsInDeck = value; // w skrypcie WhereIsTheCardOrCreature jest wytłumaczone co to - value
            // Odwołanie się do pozycji w cardsInDeck
            // przypisanie do niego powej pozycji z modyfikacją na osi Z:
            // Wartość na osi Z jest przestawiana na wartość ujemną i jest wartością z mnożenia zmiennej HeightOfOneCard z ilością kart w talii
            // Po co tak? W ten sposób z każdą wyłożoną kartą talia będzie się wizualnie zmiejszać (taki bajer)
            transform.position = new Vector3(transform.position.x, transform.position.y, - HeightOfOneCard * value);
        }
    }
   
}
