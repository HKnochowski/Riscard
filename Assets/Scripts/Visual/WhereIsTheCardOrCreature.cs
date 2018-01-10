using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// enum do przechowywania informacji o tym, gdzie znajduje się ten obiekt
public enum VisualStates
{
    Transition, //oznacza czy obiekt jest w trakcie zmiany pozycji, czy jest modyfikowany przez system
    LowHand,  //czy obiekt jest w dłoni gracza na dole ekranu
    TopHand, //czy obiekt jest w dłoni gracza u góry ekranu
    LowTable, //czy obiekt jest na polu bitwy gracza na dole ekranu
    TopTable, //czy obiekt jest na polu bitwy gracza u góry ekranu
    Dragging //czy obiekt jest w trakcie przesuwania go przez gracza
}

public class WhereIsTheCardOrCreature : MonoBehaviour {

    // odwołanie do komponentu HoverPreview
    private HoverPreview hover;

    // odniesienie do Canvasa na tym obiekcie, aby ustawić kolejność sortowania
    private Canvas canvas;

    // wartość dla kolejności sortowania na Canvasie, gdy chcemy pokazać ten obiekt ponad wszystkim
    private int TopSortingOrder = 500;

    // PROPERTIES
    //Ustawienie zmiennej slot na wartość -1
    private int slot = -1;

    // Standardowe funkcje ;get ;set które pobierają i dostarczają modyfikacje na tej zmiennej wykonywane
    public int Slot
    {
        get{ return slot;}

        set
        {
            slot = value;
            /*if (value != -1)
            {
                canvas.sortingOrder = HandSortingOrder(slot);
            }*/
        }
    }

    private VisualStates state; // Zainicjowanie zmiennej state typu VisualStates (enum)

    //Funkcja typu VisualStates, która zwraca to pod jakim parametrem aktualnie jest karta
    // Służy do poprawnego operowania kartami, w sensie jeśli karta będzie na LowHand to gracz Top nie może mieć do niej dostepu itp.
    public VisualStates VisualState
    {
        get{ return state; }  //Otrzymanie dostępu do wartości zapisanej w zmiennej state

        set //Otrzymujemy dostęp do zapisania wartości w zmiennej state
        {
            state = value; // do state przypisywana jest wartość value
            /// Na przyszłość: Wartość value jest częścią akcesora set i reprezentuje przypisaną wartość w tym akcesorze
            switch (state) // wywołanie funkcji switch z parametrem state
            {
                // Jeśli state wynosi LowHand to:
                case VisualStates.LowHand:
                    hover.ThisPreviewEnabled = true;        //możliwość podglądu kart w dolnej ręce
                    break;
                    // Jeśli state wynosi LowTable
                case VisualStates.LowTable:
                    hover.ThisPreviewEnabled = true;        //możliwość podglądu kart na dolnym Table
                    break;
                    // Jeśli state wynosi TopTable
                case VisualStates.TopTable:
                    hover.ThisPreviewEnabled = true;        //możliwość podglądu kart na górnym Table
                    break;
                    // Jeśli state wynosi Transition
                case VisualStates.Transition:
                    hover.ThisPreviewEnabled = false;       //wyłączenie podglądu podczas przemieszczania karty
                    break;
                    // Jeśli state wynosi Dragging
                case VisualStates.Dragging:
                    hover.ThisPreviewEnabled = false;        //wyłącznie podglądu podczas przemieszczania karty
                    break;
                    // Jeśli state wynosi TopHand
                case VisualStates.TopHand:
                    hover.ThisPreviewEnabled = false;       //możliwość podglądu kart w górnej ręce
                    break;
            }
        }
    }

    void Awake()
    {
        // Do hover przypisujemy pobrany komponent z HoverPreview
        hover = GetComponent<HoverPreview>();
        // Sprawdzamy czy hover jest pusty po pobraniu
        if (hover == null)
            hover = GetComponentInChildren<HoverPreview>(); // Jeśli jest pusty to do hover przypisuje komponent z HoverPreview po pierwszym wyszukiwaniu
        /// Nie wchodzi za głęboko w wyszukiwanie
        canvas = GetComponentInChildren<Canvas>(); // Do zmiennej canvas jest przypisywany komponent typu Canvas
    }

    // PRZESUNIĘCIE NA WIERZCH
    public void BringToFront()
    {
        canvas.sortingOrder = TopSortingOrder;          //do warstw sortowania w Canvasie przypisanie wartości z TopSortingOrder
        canvas.sortingLayerName = "AboveEverything";    //przypisanie nazwy warstwy sortowania oznaczonej AboveEverything
    }

    // USTAWIENIE SORTOWANIA W DŁONI
    // nie ustawiamy porządku sortowania wewnątrz właściwości VisualStaes, ponieważ podczas rysowania karty, 
    // chcemy najpierw ustalić indeks i ustawić porządek sortowania tylko wtedy, gdy karta zostanie przekazana do ręki.
    public void SetHandSortingOrder()
    {
        if (slot != -1)                                     // Sprawdzamy czy wartość slot jest różna od -1
            canvas.sortingOrder = HandSortingOrder(slot);   // Do warstw sortowania w Canvasie przypisujemy wartość którą uzyskamy z HandSortingOrder
        canvas.sortingLayerName = "Cards";                  //przypisujemy nazwę warstwy "Cards" do tego canvasa
    }

    // USTAWIENIE SORTOWANIA W TABLE
    public void SetTableSortingOrder()
    {
        canvas.sortingOrder = 0;                        //przypisanie wartości aby element znalazł się na stole z pozostałymi elementami 
        canvas.sortingLayerName = "Creatures";          //przypisanie nazwę warstwy "Creatures" do tego canvasa
    }

    // KOLEJNOŚC SORTOWANIA W DŁONI
    private int HandSortingOrder(int placeInHand)
    {
        return (-(placeInHand + 1) * 10);               //zwraca wartość sortowania w ręce
        /// czyli przekazuje tutaj wartość slot (standardowo -1) i wykonuje obliczenia po czym przekazuje dalej do funkcji SetHandSortingOrder
    }


}
