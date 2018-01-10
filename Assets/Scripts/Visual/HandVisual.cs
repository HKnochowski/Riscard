using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HandVisual : MonoBehaviour
{
    // PUBLIC FIELDS
    public AreaPosition owner; //Zainicjowanie obiektu "owner" typu "AreaPosition" - jest to obiekt tak jakby strefy dla każdego gracza (więcej informacji w jego skrypcie)
    public bool TakeCardsOpenly = true; //Zmienna boolowska do sprawdzania czy można wyłożyć kolejną kartę z talii do ręki
    public SameDistanceChildren slots; //Zainicjowanie obiektu "slots" typu "SameDistanceChildren" - więcej o tym w jego skrypcie

    [Header("Transform References")]
    ///Na przyszłosć: "transform" nie musisz oznaczać tylko pozycji, ale również oznacza skalę i rotację
    public Transform DrawPreviewSpot; //Oznacza pierwszą pozycją, do której wędruje karta z talli przed tym jak trafi do ręki gracza
    public Transform DeckTransform; //Oznacza pozycję talii w grze, z której będzie wędrować karta do DrawPreviewSpot
    public Transform OtherCardDrawSourceTransform; //Oznacza pozycję z której będą wędrować do ręki karty, które nie idą bezpośredni z talii
    public Transform PlayPreviewSpot; //Oznacza pozycję do której wędrują wszystkie karty czary

    // Zainicjowanie listy obiektów typu GameObject o nazwie CardsInHand i przypisanie do niego taką właśnie listę
    private List<GameObject> CardsInHand = new List<GameObject>();


    // Funkcja która dodaje nową kartę typu GameObject do ręki
    public void AddCard(GameObject card)
    {
        // zawsze wstawiamy nową kartę jako 0-wy element na liście CardsInHand
        ///Funkcja Insert jest odpowiedzialna za wstawianie nowego obiektu jej parametry: 0 - oznacza indeks, card - oznacza typ obiektu
        CardsInHand.Insert(0, card);

        // przypisz tę kartę do naszego Slota GameObject
        //Modyfikuje pozycję nowo dodanej karty (transform) na pozycję rodzica (SetParent), którym jest - slots
        //Musi również pobrać jego pozycję dlatego na końcu jest transform (slots.transform)
        card.transform.SetParent(slots.transform);

        // ponownie oblicz pozycję ręki
        PlaceCardsOnNewSlots(); //uruchamia funkcję (jest opisana niżej)
        UpdatePlacementOfSlots(); //uruchamia funkcję (jest opisana niżej)
    }

    // FUNKCJA KTURA USUWA KARTĘ Z RĘKI
    public void RemoveCard(GameObject card)
    {
        // usuń kartę z listy
        CardsInHand.Remove(card);

        // ponownie oblicz pozycję ręki
        PlaceCardsOnNewSlots(); //uruchamia funkcję (jest opisana niżej)
        UpdatePlacementOfSlots(); //uruchamia funkcję (jest opisana niżej)
    }

    // FUNKCJA KTORA USUWA KARTĘ O DANYM INDEKSIE Z RĘKI
    public void RemoveCardAtIndex(int index)
    {
        ///RemoveAt() jest podobny do Remove(); różni się tylko tym że usuwa po indeksie w tablicy/liście
        CardsInHand.RemoveAt(index);
        // ponownie oblicz pozycję ręki
        PlaceCardsOnNewSlots(); //uruchamia funkcję (jest opisana niżej)
        UpdatePlacementOfSlots(); //uruchamia funkcję (jest opisana niżej)
    }

    // ZDOBĄDŹ KARTĘ GAMEOBJECT Z DANYM INDEKSEM W RĘCE
    public GameObject GetCardAtIndex(int index) //Zwykła funkcja typu ;get / ;set
    {
        ///Po co? CardsInHand jest prywatna, aby móc się do niej dostać z innego skryptu trzeba wykonać właśnie taki trik
        ///Tak jakby zmieniamy jej status z prywatnej na publiczną, nie bojąc się tego że możemy ją na stałe zmodyfikować i coś popierodlić
        return CardsInHand[index];
    }

    // ZARZĄDZANIE KARTAMI I SLOTAMI

    // PRZESUŃ SLOTY GAMEOBJECT ZGODNIE Z LICZBĄ KART W RĘCE
    void UpdatePlacementOfSlots()
    {
        float posX; //Inicjalizuje zmienną, która będzie przechowywać pozycję po osi X

        // Zlicza liczbę kart w dłoni (.Count) i sprawdza czy jest większe od 0 jeśli tak jedzie z kodem dalej
        if (CardsInHand.Count > 0)
            //do "posX" przypisuje wynik:
            //macierzystej pozycji na osi X obiektu slots.Children[0] -
            //macierzystej pozycji na osi X przedostatniego obiektu slots.Children
            //i dzieli wszystko przez 2
            ///Na przyszłość: to "f" po dwójce - 2f oznacza wymuszenie na niej tego, aby była zapisana w typie FLOAT
            ///Tak samo się robi aby wymusić inne typy np. 2u - typu unsigned itp.
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CardsInHand.Count - 1].transform.localPosition.x) / 2f;

        else // jeśli liczba kart w dłoni jest mniejsza od 0, do "posX" przypisuje 0
            posX = 0f;

        // Przesuń sloty gameObject do nowej pozycji w 0,3 sekundy
        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    // PRZESUŃ WSZYSTKIE KARTY DO NOWYCH MIEJSC
    void PlaceCardsOnNewSlots()
    {
        //Sprawdza wszystkie elementy typu GameObject, które znajdują się w CardsInHand
        //Pierwszy argument to zmienna do której zostanie przypisana wartość listy
        // "in" standardowo wykorzystuje się w funkcji foreach
        // CardsInHand to lista po której będą sprawdzane obiekty
        foreach (GameObject g in CardsInHand)
        {
            // przenieś kartę do nowego slotu
            // Na aktualnym obiekcie "g" odczytuje jego pozycję (transform) i przenosi go do nowej pozycji po osi X (DOLocalMoveX)
            ///DOLocalMoveX to funkcja która pochodzi z dołączonej biblioteki DOTWEEN
            // Pierwszy parametr tej funkcji pobiera pozycję obiektu na osi x (slots.Children[CardsInHand.IndexOf(g)]) poprzez .transform.localPosition.x
            ///IndexOf(g) wyszukuje określony obiekt (g) i zwraca liczony od zera indeks pierwszego wystąpienia w obrębie całego CardsInHand
            /// transform.localPosition.x - to pozycja względem pozycji macierzystej po osi X
            /// W tym przypadku pozycją macierzystą jest pozycja slots.Children
            // Drugi parametr funkcji DOLocalMoveX odpowiada za czas w jakim ma nastąpić przesunięcie
            g.transform.DOLocalMoveX(slots.Children[CardsInHand.IndexOf(g)].transform.localPosition.x, 0.3f);

            // zastosuj prawidłową kolejność sortowania i wartość HandSlot na później 
            //Inicjalizuje obiekt "w" typu WhereIsTheCardOrCreature
            ///Ogólnie jest to klasa odpowiedzialna za sprawdzanie gdzie znajduje się karta lub stworzenie w grze, więcej opisane w jego skrypcie
            // Przypisuje do "w" wszystkie komponenty, które znajdują się w obiekcie (GameObject) "g", tyle że przypisuje te komponenty
            // Które wymaga klasa WhereIsTheCardOrCreature; to tak jakby przerabiał GameObject na WhereIsTheCardOrCreature
            WhereIsTheCardOrCreature w = g.GetComponent<WhereIsTheCardOrCreature>();

            //WITCOC = WhereIsTheCardOrCreature
            //Pobiera zmienną slot z WITCOC i przypisuje do niego obiekt "g" z CardInHand, wcześniej wyszukany przez - IndexOf()
            w.Slot = CardsInHand.IndexOf(g);
            w.SetHandSortingOrder(); //wywołuje funkcję z WITCOC; funkcja ogólnie ustawia kolejność sortowania w ręce (więcej w jego skrypcie)
        }
    }

    // METODY WYKRYWANIA KART

    // TWORZY NOWĄ KARTĘ I ZWRACA JAKO GAMEOBJECT
    ///CardAsset c to zmienna która odwołuje się do klasy CardAsset
    GameObject CreateACardAtPosition(CardAsset c, Vector3 position, Vector3 eulerAngles)
    {
        // Wywołaj kartę w zależności od jej typu
        //Inicjalizuje zmienną "card" typu GameObject
        GameObject card;
        if (c.MaxHealth > 0) //Odwołuje się do pola MaxHealth w klasie CardAsset i sprawdza czy wartość jest większa od 0
        {
            // Jeśli jest większe od 0 to jest to karta istoty więc jedzie z tym kodem
            ///Instantiate() - klonuje obiekt i zwraca ten klon więc:
            // Klonuje opiekt GameObject i zwraca klon o parametrach:
            // GlobalSettings.Instance.CreatureCardPrefab - odnosi się do pozycji w skrypcie GlobalSetting i klonuje Preefab karty postaci
            // Która jest do tej instancji przypisana
            // do argumentu posisiotn przypisywana jest pozycja karty
            /// Quaternion.Euler jest odpowiedzialny za rotację; samo Euler() - zwraca obót (kąt Eulera)
            // do argumentu eulerAngles jest przypisywany obrót który jest wykonywany przy obracaniu karty gdy się ją wykłada
            // as GameObject na końcu określa, jakiego typu jest obiekt w tym wypadku dalej GameObject
            card = GameObject.Instantiate(GlobalSettings.Instance.CreatureCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
        }
        else
        {
            // jśli c.MaxHealth jest równe 0, to jest to karta czaru, więc następuje sprawdzanie czy jest to czar z celownikiem czy nie
            // Jeśli chodzi o celownik to, mam na myśli czary, przy których trzeba wyznaczyć obiekt w grze na który ma mieć ta karta wpływ
            // c.Targets - to odwołanie się do zmiennej Targets typu TargetingOptions (zmienna znajduje się w skrypcie CardAsset)
            // TargetingOptions.NoTarget - to odwołanie się do argumentu "NoTarget", który znajduje się w enum "TargetingOptions"
            // Jeżeli c.Targets równa się TargetingOptions.NoTarget to jedzie dalej z kodem ->
            if (c.Targets == TargetingOptions.NoTarget)
                // To co powyżej w tej funkcji, z tą różnicą że teraz tworzymy klon z preefabem NoTargetSpellCardPrefab
                card = GameObject.Instantiate(GlobalSettings.Instance.NoTargetSpellCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;

            // Jeśli c.Targets nie jest równe TargetingOptions.NoTarget to jest to czar ??globalny?? (bez celownika)
            // Więc jedzie z kodem poniżej ->
            else
            {
                // To co powyżej w tej funkcji, z tą różnicą, że teraz tworzymy klon z preefabem TargetedSpellCardPrefab
                card = GameObject.Instantiate(GlobalSettings.Instance.TargetedSpellCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
                
                // Zainicjowanie obiektu dragSpell typy DragSpellOnTarget (jest to klasa)
                // Do dragSpell przypisuje zwrócony komponent (GetComponentInChildren) z obiektu card, typu DragSpellOnTarget
                /// GetComponentInChildren - zwraca komponent wyszukany po pierwszym wyszukiwaniu głębi, w sensie nie wchodzi za głęboko w obiekt
                /// Na przyszłość: Komponent zostanie zwrócony tylko wtedy gdy obiekt z którego będzie wyszukiwany będzie aktywny na scenie
                DragSpellOnTarget dragSpell = card.GetComponentInChildren<DragSpellOnTarget>();

                // Odwołuje się do zmiennej Targets w klasie dragSpell i przypisuje do niej zmienną Targets z klasy CardAsset
                dragSpell.Targets = c.Targets;

                // Ogólnie chodzi tutaj o to aby przekazać do skryptu odpowiedzialnego za poruszanie kartami opcje jak ma kartę kierować
            }

        }

        // Zastosowanie wyglądu karty na podstawie informacji z CardAsset
        // Zainicjowanie zmiennej manager typu OneCardManager (to jest klasa/skrypt)
        // Przypisanie do manager komponentu pozyskanego (GetComponent) z obiektu card, typu OneCardManager 
        OneCardManager manager = card.GetComponent<OneCardManager>();
        // Odwołanie się do zmiennej "cardAsset" znajdującej się w "OneCardManager" i przypisanie do niej obiektu "c" (jest to cardAsset)
        manager.cardAsset = c;
        manager.ReadCardFromAsset(); //wywołanie funkcji znajdującej się w OneCardManager (więcej o niej w danym skrypcie)

        return card; //Zwrócenie obiektu card
    }

    // DAJE GRACZOWI NOWĄ KARTĘ Z POZYCJI
    public void GivePlayerACard(CardAsset c, int UniqueID, bool fast = false, bool fromDeck = true)
    {
        // Zainicjowanie obiektu card typu GameObject
        GameObject card;

        // Sprawdzenie dla pewności czy karta pochodzi z talii; fromDeck musi być true
        if (fromDeck)
            // Przypisanie do obiektu card obiektu stworzonego za pomocą funkcji - CreateACardAtPosition() -> funkcja opisana wyżej
            // W nawiasach znajdują się argumenty na których ma operować funkcja
            card = CreateACardAtPosition(c, DeckTransform.position, new Vector3(0f, -179f, 0f));
        else
            // Jeżeli fromDeck to false to karta nie jest z talii więc tworzy tą kartę, ale umieszcza ją w innym miejscu w grze
            card = CreateACardAtPosition(c, OtherCardDrawSourceTransform.position, new Vector3(0f, -179f, 0f));

        // Ustawienie tagu, aby odzwierciedlić miejsce, w którym znajduje się ta karta
        // card.GetComponentsInChildren<Transform> - to zwrócony komponent typu Transform z obiektu card
        // Chodzi o przypisanie tagu do tego obiektu, a tag znajduje się w klasie Transform (jest to klasa systemowa)
        foreach (Transform t in card.GetComponentsInChildren<Transform>())
            // Odwołanie się do tagu z Transform i przypisanie do niego zwróconego łańcucha znaków [ToString()] z "Owner" i dodanie do niego "Card"
            t.tag = owner.ToString()+"Card";
     
        AddCard(card); //wywołanie funkcji (jest opisana wyżej)

        // Zainicjowanie zmiennej w typu WITCOC przypisanie do niego komponentu z obiektu card typu WITCOC
        WhereIsTheCardOrCreature w = card.GetComponent<WhereIsTheCardOrCreature>();
        w.BringToFront(); //odwołanie się do funkcji w klasie WITCOC i wywołanie jej (działanie funkcji opisane w danym skrypcie)
        w.Slot = 0; // Odwołanie się do zmiennej slot w klasie WITCOC i przypisanie jej wartości 0
        w.VisualState = VisualStates.Transition; //Odwołanie się do zmiennej VisualState w klasie WITCOC (jest to zmienna typu enum)
        // I nadanie jej argumentu "Transition", który znajduje się w zbiorze enum VisualStates
        /// Ogólnie tutaj chodzi o to aby podczas gdy karta będzie podróżować z miejsca do ręki miała priorytet i była ponad wszystkimi
        /// Innymi obiektami (była najbardziej widoczna)

        // NADAJ TEJ KARCIE - ID
        // Zainicjowanie zmiennej id typu IDHolder i przypisanie do niej stworzonego komponentu z obiektu card, typu IDHolder
        IDHolder id = card.AddComponent<IDHolder>();
        id.UniqueID = UniqueID; // Odwołanie się do zmiennej w klasie IDHolder i przypisanie do niej - jej (tzn jej ale będącej argumentem tej funkcji) xD
        /// Wiem to może być dziwne, ale to co przetwarza tamta klasa zapisuje się właśnie w UniqueID; chcemy to co ona wykona przekazać tą funkcją dalej
        /// Więc musimy to przekazać do jej argumentu - tym właśnie jest to drugie UniqueID po znaku równa się


        //  PRZENIESIENIE KARTY DO RĘKI
        // Zainicjowanie zmiennej s typu Sequence
        /// Na przyszłość: jeśli chodzi o tym Sequence to jest to typ udostępniany przez bibliotekę DOTWEEN
        /// Tworzy ona zasób w której będzie można przetrzymywać stworzoną sekwencję animacji jaka ma być wykonywana
        /// Nie będę tutaj za dużo o niej pisać, ale polecam zapoznać się z dokumentacją DOTWEEN jest tam wszystko dokładnie wytłumaczone
        // Do "s" przypisujemy funkcję która będzie generować tą sekwencję (chodzi po prostu o dbanie o zdrowie swoich palcy i nie pisania za dużo) xD
        Sequence s = DOTween.Sequence();

        // Na początku tej funkcji zainicjowaliśmy tą zmienną jako false, chodzi tutaj o wywołanie jej jako true, bez zmieniania jej na true
        if (!fast)
        {
            // Debug.Log ("Not fast!!!");
            // s.Append() - Wywołanie funkcji Append zawartej w Sequence()
            /// Append() - dodaje animację do końca sekwencji
            // DrawPreviewSpot.position - przekazuje referencje pozycji jakie znajdują się w polu DrawPreviewSpot
            // GlobalSettings.Instance.CardTransitionTime - przekazuje referencje czasu jakie znajdują się w polu CardTransitionTime
            /// Jeśli chodzi o to ostatnie to jest to wytłumaczone w skrypcie GlobalSetting
            s.Append(card.transform.DOMove(DrawPreviewSpot.position, GlobalSettings.Instance.CardTransitionTime));

            // Sprawdzanie czy można wyłożyć kartę, jeśli tak to jedzie z kodem ->
            if (TakeCardsOpenly)
                // s.Insert() - Wywołanie funkcji Insert zawartej w Sequence()
                /// Insert() - umożliwia dodanie animacji do konkretnej pozycji czasowej
                // 0f - oznacza czas w którym ma zostać animacja umieszczona
                /// ogólnie w tej funkcji pierwszy argument zawsze oznacza czas
                // ard.transform.DORotate - wywołanie funkcji DORotate, która będzie działąć na Transform w obrębie card
                /// DORotate() - obraca cel do podanej wartości
                // Vector3.zero - pierwszy argument DORotate, jest końcowym stanem do jakiego ma zostać zmodyfikowany obiekt
                /// Na przyszłość: Vector3.zero - jest skrótem od Vector3 (0,0,0); ustawia obiekt na 0
                // lobalSettings.Instance.CardTransitionTime - TO CO BYŁO OPISANE WYŻEJ ^
                s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTime)); 

            //else 
                //s.Insert(0f, card.transform.DORotate(new Vector3(0f, -179f, 0f), GlobalSettings.Instance.CardTransitionTime)); 

            // s.AppendInterval() - Wywołanie funkcji AppendInterval zawartej w Sequence()
            // AppendInterval() - dodaje podany interwał do końca sekwencji
            // W nawiasie to co było wytłumaczane wyżej ^
            s.AppendInterval(GlobalSettings.Instance.CardPreviewTime);
            // Dodajemy animację do końca sekwencji; w nawiasie podajemy jaka animacja ma się stworzyć
            // card.transform.DOLocalMove - odwołujemy się do tramsformacji w obrębie "card" i wywołujemy funkcję DOLocalMove
            /// DOLocalMove() - funkcja która przenosi obiekt do podanej wartości w nawiasie
            // slots.Children[0].transform.localPosition - odwołujemy się do lokalnej pozycji jaka jest zawarta w Children[0], który jest w "slots"
            /// Jest to pierwszy argument funkcji DOLocalMove(), zawsze oznacza pozycję do której ma się udać obiekt
            // Drugi argument to czas w jakim ma się przenieść obiekt, tutaj bez zmian to co powyżej było wyjaśniane ^
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTime));
        }
        // Jeśli zmienna fast jest false to wykonuje poniższy kod ->
        else
        {
            // To samo co parę linikej wyżej z tą różnicą, że tym razem czas w jakim ma się wykonać animacja pochodzi od zmiennej CardTransitionTimeFast
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTimeFast));

            // Sprawdzenie czy można wyłożyć kartę z talii do ręki, TakeCardsOplenly musi być true aby ruszył się dalej kod ->
            if (TakeCardsOpenly)
                // To samo co było wajaśniane wyżej w tej funkcji, dodajemy animację do konkretnej pozycji czasowej
                s.Insert(0f,card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTimeFast)); 
        }

        // Ustawienie wywołania zwrotnego (OnComplete), które zostanie wywołanie po zakończeniu animacji; w nawiasie podajemy co ma być wywołane
        /// => - to wyrażenie lambda, służy do wywoływania funkcji/metod. Przydatne przy delegatach. 
        /// Nie wiem jak to wytłumaczyć na zrozumiały język. .może przykład:
        /// mamy delegację del Delegate -> przypisujemy do niego np x -> del Delegate = x / i teraz możemy użyć wyrażenia lambda aby określić argumenty
        /// del Delegate = x => x + x;
        /// Potem możemy wywołać Delegate(np. 5) i dostaniemy wynik 10, ponieważ podaną 5 lambda przypisuje do argumentów x+x
        /// I tak samo lambdę moża wykorzystać w funkcjach tak jak poniżej, mamy funkcję ChangeLastCardStatusToInHand i podane argumenty
        /// Lambda przekazuje te parametry do funkcji, potem ta funkcja się wykonuje i zwraca wynik do niej
        /// Wynik wędruje do OnComplete aby wykonać swoją robotę na podstawie swojego argumentu, który składa się z argumentów ChangeLastCardStatusToInHand
        /// W ten sposób stworzyliśmy tzw. Drzewo wyrażeń - coś się składa z czegoś, a to coś jeszcze z czegoś i tak do najniższej pozycji drzewa
        // A dlaczego tak? Bo funkcja ChangeLastCardStatusToInHand zmienia status karty; chcemy aby ten status wykonał się po tym jak karta trafi do ręki
        // Dlatego umieszczamy ją w OnComplete, która oznacza koniec animacji która przenosi kartę do ręki i w ten sposób mamy pewność że ta funkcja
        // Wykona się dopiero wtedy gdy karta będzie w ręce
        s.OnComplete(()=>ChangeLastCardStatusToInHand(card, w));
    }

    // ZMIANA OSTATNIEJ KARTY NA: InHand
    void ChangeLastCardStatusToInHand(GameObject card, WhereIsTheCardOrCreature w)
    {
        //Debug.Log("Zmiana stanu na kartę w ręce: " + card.gameObject.name);

        // Tutaj sprawdzamy do której dłoni ma trafić karta, czy do przeciwnika czy do naszej
        // Jeśli owner jest równy pozycji Low (jest to pozycja gracza na dole ekranu) to jedzie z kodem ->
        if (owner == AreaPosition.Low)
            // odwołanie się do VisualState w WITCOC i nadanie jej argumentu "LowHand" z enum VisualStates
            w.VisualState = VisualStates.LowHand;

        // Jeśli owner nie jest Low to jest Top (czyli jest na pozycji gracza na górze ekranu) i jedzie z tym kodem ->
        else
            // odwołanie się do VisualState w WITCOC i nadanie jej argumentu "TopHand" z enum VisualStates
            w.VisualState = VisualStates.TopHand;

        // ustawienie prawidłowej kolejności sortowania
        w.SetHandSortingOrder(); // odwołanie się do metody w WITCOC i wykonanie tej metody
        Command.CommandExecutionComplete(); // odwołanie się do metody w klasie Command i jej wywołanie
    }


    // ODTWARZANIE CZAROW

    // Przeciążona metoda, aby pokazać zaklęcie grane z ręki
    public void PlayASpellFromHand(int CardID)
    {
        // Zainicjowanie zmiennej card typu GameObject
        // Przypisanie do niej obiektu który pobraliśmy poprzez jego ID (GetGameObjectWithID(CardID)), metoda znajduje się w klasie IDHolder dlatego
        // musimy najpierw się do niej odwołać
        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        PlayASpellFromHand(card); //Wykonujemy tą metodę
        /// Na przyszłość: Jeśli jeszcze nie zauważyliście to nie ma problemu w nadawaniu takich samych nazw metodom, pod warunkiem że mają inne argumenty
        /// Po co tak? Nazwa ta sama bo za to samo odpowiadają, z tą różnicą że metoda z argumentem CardID przekazuje dalej ID karty co jest potrzebne
        /// do części logicznej gry, a metoda która przekazuje GameObject nie może zostać przekazana dalej do logiki
        /// Dlatego wykonując tą funkcję wyświetlimy obiekt gry, którym jest karta, ale do logiki przekażemy tylko jej ID
    }

    public void PlayASpellFromHand(GameObject CardVisual)
    {

        Command.CommandExecutionComplete(); // Wywołanie funkcji z klasy Command (więcej o niej w tej klasie)
        // Pobieramy komponent z WITCOC którym jest VisualState i nadajemy mu parametr "Transition" który znajduje się w enum VisualStates
        CardVisual.GetComponent<WhereIsTheCardOrCreature>().VisualState = VisualStates.Transition;
        // Przekazujemy do funkcji tą kartę i ją wywołujemy
        // Po prostu usuwamy kartę z dłoni, ponieważ ją zagraliśmy
        RemoveCard(CardVisual);

        //wywołujemy rodzica (SetParent) z transformacji która znajduje się w CardVisual i ustawiamy go na pusty (null)
        CardVisual.transform.SetParent(null);

        // Zainicjowanie s w typie Sequence i przypisanie do niej fukncji Sequence()
        Sequence s = DOTween.Sequence();

        //Dodajemy animację do końca sekwencji (Append)
        // Pierwszy argument to to co ma być wykonane, w tym przypadku przemieszczenie
        // Przemieszczenie jest wykonywane na Transform który jest pobierany z CardVisual i którym jest position pobrane z PlayPreviewSpot
        // Drugi argument to czas w jakim ma zostać wykonany ruch, w tym przypadku to 1 sekunda
        s.Append(CardVisual.transform.DOMove(PlayPreviewSpot.position, 1f));
        // Wstawiamy daną animację w dany przedział czasowy, w tym przypadku to 0 sekund czyli początek - 1 argument
        // Drugi argument określa to co ma się zadziać, czyli rotacja karty po tramsform z CardVisual
        // Vector3.zero określa do jakiego stanu ma się obrócić, drugi argument to w jakim czasie ma to zrobić
        s.Insert(0f, CardVisual.transform.DORotate(Vector3.zero, 1f));
        // AppendInterval() - dodaje podany interwał do końca sekwencji, w tym przypadku 2 sekundowy interwał
        s.AppendInterval(2f);
        // To co tłumaczyłem wcześniej do OneComplete() poprzez wyrażenie lambda przypisujemy funkcję Destroy z argumentem CardVisual
        s.OnComplete(()=>
            {
                //Command.CommandExecutionComplete();
                Destroy(CardVisual); // Destroy() to systemowa metoda, która cos po prostu niszczy, w tym przypadku CardVisual na którym pracowaliśmy
            });
    }


}
