using UnityEngine;
using System.Collections;

// umieść ręcznie pierwszy i ostatni element w tablicy
// inne zostaną umieszczone automatycznie z równą odległością między pierwszymi i ostatnimi elementami
// Skrypt służy do rozmieszczania kart w dłoni lub na Table
public class SameDistanceChildren : MonoBehaviour {

    // Zainicjowanie tablicy Children typu Transform
    public Transform[] Children;

	// Użyte do inicjalizacji
	void Awake () 
    {
        // Zainicjowanie zmiennej typu Vector3 i przypisanie do niej wartości pozycji pierwszego elementu w tablicy Children
        Vector3 firstElementPos = Children[0].transform.position;
        // Zainicjowanie zmiennej typu Vector3 i przypisanie do niej wartości pozycji przedostatniego elementu w tablicy Children
        Vector3 lastElementPos = Children[Children.Length - 1].transform.position;

        // Do zmiennej XDist - która przechowuje wartość dystansu po osi X przypisujemy wynik równania:
        // Pozycja przedostatniego elementu w tablicy po osi x - pozycja pierwszego elementu w tablicy po osi x
        // Dzielone przez (wcześniej narzucamy, aby wartość wyniku była podana w FLOAT
        /// Musimy wymusić float ponieważ Children.Length jest podawana w INT
        // Wielkość tablicy - 1
        float XDist = (lastElementPos.x - firstElementPos.x)/(float)(Children.Length - 1);
        // To samo co wyżej tylko po osi Y
        float YDist = (lastElementPos.y - firstElementPos.y)/(float)(Children.Length - 1);
        // To samo co wyżej tylko po osi Z
        float ZDist = (lastElementPos.z - firstElementPos.z)/(float)(Children.Length - 1);

        // Zainicjowanie nowego obiektu Dist - opdowiedzialnego za przechowywanie wartości z wcześniejszych obliczeń
        // Przypisanie do niego wcześniejszych obliczeń jako pozycji
        Vector3 Dist = new Vector3(XDist, YDist, ZDist);

        // Wykonujemy pętlę do momentu aż "i" nie będzie większe od wielkości tablicy Children
        for (int i = 1; i < Children.Length; i++)
        {
            // Pobieramy wartość pozycji obiektu o indeksie "i" w tablicy Children i przekazujemy do niej:
            // Wartość pozycji poprzedniego obiektu zwiększone o wartość przechowywaną w zmiennej Dist
            Children[i].transform.position = Children[i - 1].transform.position + Dist;
        }
	}
	
	
}
