using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TableVisual : MonoBehaviour //dziedziczy po MonoBehaviour
{
    // PUBLIC FIELDS
    public AreaPosition owner; //wyliczenie które oznacza do którego gracza należy dany TableVisual. Do wyboru - Low i Top.

    public SameDistanceChildren slots; //odniesienie do obiektu gry, który zaznacza pozycje, w których powinniśmy umieścić nowe stworzenia
    //odpowiedzialny za to jest skrypt SameDistanceChildren.cs

    // PRIVATE FIELDS

    private List<GameObject> CreaturesOnTable = new List<GameObject>(); //lista wszystkich kart stworów na stole - jako gameObject

    private bool cursorOverThisTable = false; //// czy przesuwawmy kursor nad Colliderem tabeli za pomocą myszy

    private BoxCollider col; // 3D collider dołączony do tego obiektu gry (do TableVisual)

    // PROPERTIES

    public static bool CursorOverSomeTable //Zwraca wartość true jeśli zawieszamy kursor nad colliderem tabeli dowolnego gracza
    {
        get //Akcesor wywołany aby pobrać wartość właściwości "CursorOverSomeTable"
        {
            //Tworzy obiekt tabeli klasy "TableVisual" o nazwie - bothTables
            //bothTables definuje obie tabele - Top i Low
            //do bothTables są znajdywane i przypisywane wszystkie obiekty "TableVisual"
            TableVisual[] bothTables = GameObject.FindObjectsOfType<TableVisual>();
            //Zwraca tą wartość nad którą w danej chwili jest kursor myszy
            //jeśli jest to bothTable[0] zwraca go, jeśli jest to bothTables[1] zwraca go
            ///Przy większej ilości hexów będzie trzeba dobudować jakąś właściwość, która będzie zwracać wartość dla każdego hexa osobno
            return (bothTables[0].CursorOverThisTable || bothTables[1].CursorOverThisTable);
        }
    }

    public bool CursorOverThisTable //zwraca wartość true tylko wtedy, gdy przesuniemy kursor nad tą tabelą
    {
        get{ return cursorOverThisTable; }
    }

    // METODY

    // METODY MONOBEHAVIOUR (wskaznik myszy nad collider detection)
    void Awake()
    {
        //za pomocą GetComponent uzyskujemy dostęp do komponentu typu BoxCollider i przypisujemy go do właściwości col, która jest BoxColliderem
        col = GetComponent<BoxCollider>();
    }

    // DETEKCJA MYSZY
    void Update()
    {
        // potrzebujemy Raycast, ponieważ OnMouseEnter, itp. reaguje na Collider na kartach i karty "przykrywają" TableVisual
        // utwórz tablicę RaycastHits
        /// pomaga odzyskać informacje z Raycast'a
        RaycastHit[] hits;
        // raycast do mousePosition i zapisz wszystkie trafienia w tablicy
        // Do hits przypisujemy wszystkie trafienia jakie wykonał Raycast
        // Pierwszy argument określa początek i kierunek w którym ma zmierzać Raycast
        // Camera.main.ScreenPointToRay - określa że Raycast ma wychodzić z płaszczyzny zbliżonej do pozycji kamery
        // Input.mousePosition - oznacza dokładną pozycję z której ma wychodzić, czyli wychodzi z pozycji na której jest aktualnie kursor myszy
        // 30f w tym przypadku to maksymalna odległość na jąką ma zostać wystrzelony Raycast
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        // Zainicjowanie boolowskiej zmiennej passedThrougTableCollider i nadanie mu wartości false
        // Ta zmienna jest odpowiedzialna za sprawdzenie czy przez jakiś table przeszedł Raycast
        bool passedThroughTableCollider = false;

        // Sprawdzanie czy jakiś RaycastHit znajduje się w tabeli hits
        foreach (RaycastHit h in hits)
        {
            // Sprawdzamy czy collider w który trafił RaycastHit h jest colliderem któregoś Table
            if (h.collider == col)
                //Jeśli tak to ustawiamy zmienną na true
                passedThroughTableCollider = true;
        }
        // Do cursorOverThisTable przypisujemy wynik z trafienia Raycastem
        cursorOverThisTable = passedThroughTableCollider;
    }

    // METODA KTORA DODAJE NOWĄ POSTAC DO TABLE VISUAL SPRAWDZAJĄC PO JEGO INDEKSIE
    public void AddCreatureAtIndex(CardAsset ca, int UniqueID ,int index)
    {
        // Tworzy nową postać z preefaba (było już tłumaczone w HandVisual)
        GameObject creature = GameObject.Instantiate(GlobalSettings.Instance.CreaturePrefab, slots.Children[index].transform.position, Quaternion.identity) as GameObject;

        // Do stworzonej postaci przypisuje to co jest w CardAsset (było tłumaczone w HandVisual)
        OneCreatureManager manager = creature.GetComponent<OneCreatureManager>();
        manager.cardAsset = ca;
        manager.ReadCreatureFromAsset();

        // Ustawienie tagu, aby odzwierciedlić miejsce, w którym znajduje się ta karta
        // card.GetComponentsInChildren<Transform> - to zwrócony komponent typu Transform z obiektu card
        // Chodzi o przypisanie tagu do tego obiektu, a tag znajduje się w klasie Transform (jest to klasa systemowa)
        foreach (Transform t in creature.GetComponentsInChildren<Transform>())
            // Odwołanie się do tagu z Transform i przypisanie do niego zwróconego łańcucha znaków [ToString()] z "Owner" i dodanie do niego "Card"
            t.tag = owner.ToString()+"Creature";

        // umieszczamy stworzony obiekt "creature" na pozycji rodzica w slocie który jest na TableVisual
        creature.transform.SetParent(slots.transform);

        // Dodajemy stworzenie do listy stworzeń na Table
        CreaturesOnTable.Insert(index, creature);

        // niech to stworzenie zna swoją pozycję xD
        // Zainicjowanie zmiennej w typu WITCOC i przypisanie do niego creature z komponentem pobranym z WITCOC
        WhereIsTheCardOrCreature w = creature.GetComponent<WhereIsTheCardOrCreature>();
        // Odwołanie się do zmiennej Slot w WITCOC i przypisanie mu wartości ze zmiennej index
        w.Slot = index;
        // Sprawdzamy na co ustawiony jest owner, jeśli jest równy parametrowi "Low" z enum AreaPosition to:
        if (owner == AreaPosition.Low)
            w.VisualState = VisualStates.LowTable; // do zmiennej VisualState z klasy WITCOC przypisujemy parametr LowTable z enum VisualStates
        else
            w.VisualState = VisualStates.TopTable; // jeśli nie to do zmiennej przypisujemy TopTable z enum VisualStates

        // Dodaj ID do tego stworzenia
        // Zainicjowanie zmiennej id typu IDHolder i przypisanie do niego creature z dodanym komponentem IDHolder
        IDHolder id = creature.AddComponent<IDHolder>();
        // do UniqueID z klasy IDHolder przypisujemy UniqueID pobrane z funkcji, gdzieś to już tłumaczyłęm czemu tak (chyba w HandVisual)
        id.UniqueID = UniqueID;

        // po dodaniu nowego stwora zaktualizuj rozmieszczenie wszystkich innych stworzeń
        ShiftSlotsGameObjectAccordingToNumberOfCreatures(); // metoda opisana niżej
        PlaceCreaturesOnNewSlots(); // metoda opisana niżej

        // Zakończ wykonywanie poleceń
        Command.CommandExecutionComplete(); //wywołanie metody z klasy Command
    }


    // zwraca indeks dla nowego stworzenia na podstawie mousePosition
    // zawarte w celu umieszczenia nowego stworzenia w dowolnej pozycji na stole
    public int TablePosForNewCreature(float MouseX)
    {
        // jeśli nie ma żadnych stworzeń lub wskazujemy na prawo wszystkich stworzeń myszką.
        // w prawo - ponieważ miejsca na stole są odwrócone, a 0 po prawej stronie.
        if (CreaturesOnTable.Count == 0 || MouseX > slots.Children[0].transform.position.x)
            return 0;
        else if (MouseX < slots.Children[CreaturesOnTable.Count - 1].transform.position.x) // kursor po lewej stronie względem wszystkich stworzeń na stole
            return CreaturesOnTable.Count;
        for (int i = 0; i < CreaturesOnTable.Count; i++)
        {
            if (MouseX < slots.Children[i].transform.position.x && MouseX > slots.Children[i + 1].transform.position.x)
                return i + 1;
        }
        Debug.Log("Suspicious behavior. Reached end of TablePosForNewCreature method. Returning 0");
        return 0;
    }

    // Zniszcz stworzenie
    public void RemoveCreatureWithID(int IDToRemove)
    {
        // TODO:
        // Dodanie tu opóźnienia nie zadziałało, ponieważ pokazuje jedną istotę umierającą, następnie umiera inna istota. 
        // 
        //Sequence s = DOTween.Sequence();
        //s.AppendInterval(1f);
        //s.OnComplete(() =>
        //   {

        //    });
        GameObject creatureToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
        CreaturesOnTable.Remove(creatureToRemove);
        Destroy(creatureToRemove);

        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();
        Command.CommandExecutionComplete();
    }

    /// <summary>
    /// Przesuwa slot z gameobject zgodnie z ilością stworzeń na stole (TableVisual)
    /// </summary>
    void ShiftSlotsGameObjectAccordingToNumberOfCreatures()
    {
        //Zainicjowanie zmiennej posX typu float; przechowuje pozycję po osi X
        float posX;
        // Wykonuje się tylko wtedy gdy lista postaci w tabeli jest większa od 0
        /// Count - zlicza ilość obiektów w tej tabeli/liście
        if (CreaturesOnTable.Count > 0)
            // Było tłumaczone w skrypcie HandVisual w metodzie - UpdatePlacementOfSlots
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CreaturesOnTable.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    /// <summary>
    /// Po dodaniu nowego stwora lub śmierci starego stwora, ta metoda przesuwa wszystkie stwory i umieszcza je na nowych gniazdach.
    /// </summary>
    void PlaceCreaturesOnNewSlots()
    {
        // Sprawdza wszystkie obiekty w tabeli CreaturesOnTable
        foreach (GameObject g in CreaturesOnTable)
        {
            // BYŁO WSZYSTKO TŁUMACZONE W SKRYPCIE - HANDVISUAL, W METODZIE PlaceCardOnNewSlots
            g.transform.DOLocalMoveX(slots.Children[CreaturesOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
            // zastosuj prawidłową kolejność sortowania i wartość HandSlot na później
            // TODO: dowiedzieć się czy muszę coś tutaj zrobić:
            // g.GetComponent<WhereIsTheCardOrCreature>().SetTableSortingOrder() = CreaturesOnTable.IndexOf(g);
        }
    }

}
