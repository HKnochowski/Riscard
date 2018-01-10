using UnityEngine;
using System.Collections;

//przykładowa funkcja działania raycastu, który jest przypisany do myszki
//Jeśli chodzi o raycast to jest to specyficzny detektor kolizji, który działa na zasadzie wystrzelenia wiązki w lini prostej
//I który zczytuje to co znalazło się na jego drodze
public class RaycastExample : MonoBehaviour {

    void Update () {
        //Sprawdza czy został naciśnięty lewy przycisk myszy, jeśli tak - zwraca true
        // "0" w nawiasie oznacza lewy przycisk myszy - dlaczego? - bo tak sobie wymyślili projektanci API Unity (jest to wartość stała)
        ///Ogólnie na przyszłość: Funkcja Input.GetMouseButtonDown powinna być wywoływana z funkcji Update() - dlatego właśnie się tutaj znajduje
        ///Dlaczego? Ponieważ zostaje resetowana wraz z nową klatką; Nie zwróci wartości true dopóki gracz nie zwolni przycisku myszy
        ///Standardowe oznaczenia klawiszy myszy to: 0 - dla lewego przycisku, 1 - dla prawego przycisku, 2 - dla środkowego przycisku

        if (Input.GetMouseButtonDown (0)) {

            //Zainicjowanie obiektu "Ray" i nazwanie go - ray
            /// Ray to specyficzny obiekt, którym jest nieskończona linia, która wychodzi z jakiegoś przypisanego elementu i biegnie w nieskończoność
            /// Aż nie napotka po swojej drodze jakiejś kolizji
            /// Na przyszłość: Raycast nie wykryje collidera, dla którego pochodzenie Raycasta znajduje się wewnątrz collidera
            /// Na chłopski rozum -> strzelając Raycastem z obiektu, który również jest colliderem, to wystrzelony Raycast nie wykryje kolizji
            /// Z tym obiektem/colliderem z którego został wystrzelony
            // Przypisanie do ray elementu podstawowej kamery w grze "MainCamera"
            // Dodatkowo zwraca promień, który wychodzi z miejsca w które gracz kliknął lewym przyciskiem myszy
            // Odpowiedzialna jest za to funkcja - ScreenPointToRay(...)
            ///W skrócie do stworzonej zmiennej typu "Ray" przypisuje/zwraca kolizję (lub nie) z miejsca, w które gracz kliknął myszką :)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Zainicjowanie obiektu hit typu - RaycastHit
            /// RaycastHit to struktura, którą wykorzystuje się do odzyskiwania informacji z Raycast'a
            RaycastHit hit;

            //Sprawdzanie w co uderzył Raycast (co mu stanęło na drodze) i wypisanie w konsoli tego w co Raycast uderzył
            ///Physics.Raycast to funkcja która uruchamia wystrzelenie Raycast'a
            ///Pierwszy parametr funkcji czyli "ray" - oznacza początek wystrzelenia Raycast'a i jego kierunek
            ///Drugi parametr funkcji czyli "hit" - zapisuje to w co uderzył collider
            ///Ten zaimek "out" sprawia że zmienna "hit" wykorzystana staje się tak jakby alliasem do wcześniej utworzonej
            ///I faktycznie zostaje modyfikowana i nadpisywana w pamięci
            ///Dlaczego tak się dzieje? Bo standardowo zmienne, które są inicjowane, a pózniej gdzieś wykorzystywane tak naprawdę nie nadpisują
            ///Wartości jąką się na nich modyfikuje, a zaimek "out" albo "ref" sprawia, że te zmienne są w pełni nadpisywane
            ///Czemu takie zastosowanie? Aby mieć pewność, że podczas wykorzystywania zmiennej hit w innych skryptach nic się nie popierdoli
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log ("Name = " + hit.collider.name); //Pobiera z uderzonego collidera jego nazwę i ją wypisuje
                Debug.Log ("Tag = " + hit.collider.tag); //Pobiera z uderzonego collidera tag i go wypisuje
                Debug.Log ("Hit Point = " + hit.point); //Wypisuje globalny punkt, w którym Raycast uderzył w collider
                //Wypisuje zmianę pozycji obiektu, w którego uderzył collider (jeśli taka zmiana miała miejsce)
                Debug.Log ("Object position = " + hit.collider.gameObject.transform.position); 
                Debug.Log ("--------------");
            }
        }
    }
}