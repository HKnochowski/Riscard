using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable] //Atrybut Serializable pozwala osadzić klasę z właściwościami podrzędnymi w inspektorze.
/// Czasami jest tak, że nie wyświetlają się w inspektorze parametry do których można coś podpiąć, serializable wymusza takie wyświetlanie
public class CardLogic: IIdentifiable
{
    public Player owner; // odwołanie do klasy Payer, odpowiada za przydzielenie karty do konkretnego gracza
    public int UniqueCardID; //Id karty
    public CardAsset ca; //referencja, który asset trzyma informacje o karcie
    public SpellEffect effect; // skrypt, który dodaje do karty specjalne zdolności


    // STATIC (dla managera ID)
    // Klasa dictionary Reprezentuje kolekcję kluczy i wartości.
    // W tym przypadku jest to zbiór kart wykorzystywanych podczas gry, do których zostanie przypisana konkretne unikatowa liczba (int)
    public static Dictionary<int, CardLogic> CardsCreatedThisGame = new Dictionary<int, CardLogic>();


    // PROPERTIES
    public int ID // Odwołanie do zmiennej UniqueCardID poprzez ;get
    {
        get{ return UniqueCardID; }
    }

    public int CurrentManaCost{ get; set; } // Odwołanie ;get ;set do kosztu karty jaki jest określony w CardAsset danej karty

    public bool CanBePlayed //Sprawdzanie czy karta może być zagrana
        // TODO_ Do modyfikacji, gdy będzie gotowa mapa
    {
        get
        {
            // Sprawdzenie do kogo należy aktualna tura
            // Sprawdzamy poprzez zwracanie wartości boolowskiej
            // Odwołujemy się do funkcji whoseTurn, która zwraca nam turę aktualnego gracza
            // Wykonujemy równanie logiczne i porównujemy zwróconą wartość whoseTurn do wartości jaka jest w zmiennej owner
            // W zależności od równania system zwróci albo true albo false, zapisujemy tą wartość w zmiennej ownersTurn
            bool ownersTurn = (TurnManager.Instance.whoseTurn == owner);

            // w przypadku czarów ilość postaci na polu nie ma znaczenia
            // Sprawdzamy czy pole - Table nie jest zapełnione (standardowo ustawiamy na true - czyli nie jest)
            bool fieldNotFull = true;

            // ale jeśli to jest stworzenie, musimy sprawdzić, czy jest na nim miejsce (table)
            // Sprawdzamy czym jest karta - czarem czy postacią, poprzez sprawdzenie jego życia (karty czaru nie posiadają życia)
            if (ca.MaxHealth > 0)
                //  Odwołujemy się do tablicy CreaturesOnTable i zliczamy jej wielkość (count)
                // Sprawdzamy czy jest mniejsze od 7 (7 standardowo oznacza ilość kart na polu Table)
                // Jest to równanie logiczne, dlatego w zależności od wyniku zwróci nam albo true albo false, które przekazujemy do fieldNotFull
                fieldNotFull = (owner.table.CreaturesOnTable.Count < 7);
            //Debug.Log("Card: " + ca.name + " has params: ownersTurn=" + ownersTurn + "fieldNotFull=" + fieldNotFull + " hasMana=" + (CurrentManaCost <= owner.ManaLeft));

            // Cała funkcja to wartość boolowska (logiczna) dlatego zwracamy wynik poniższego równania logicznego
            // Pamiętacie Dyskretną? No właśnie więc tautologia ->
            // Jeśli tura należy do gracza, którego jest aktualna tura (TO TRUE) _i_ jeśli pole Table nie jest zapełnione (TO TRUE) _i_
            // Jeśli koszt karty jest mniejszy bądz równy od Ilości PPJ (TO TRUE) = jeśli wszędzie jest TRUE to zwraca TRUE
            return ownersTurn && fieldNotFull && (CurrentManaCost <= owner.ManaLeft);
        }
    }

    // CONSTRUCTOR
    public CardLogic(CardAsset ca)
    {
        // ustaw referencje CardAsset
        /// NA PRZYSZŁOŚć: this - odwołanie się klasy samej do siebie, Dlaczego tutaj tak? Bo parametrem funkcji jest ca więc odwołujemy się do niego
        this.ca = ca;

        //zdobądz unikalny int ID
        // Do UniqueCardID przypisujemy wynik pobrany z funkcji GetUniqueID, która znajduje się w klasie IDFactory
        UniqueCardID = IDFactory.GetUniqueID();
        //UniqueCardID = IDFactory.GetUniqueID();

        ResetManaCost(); // Wywołujemy funkcję ResetManaCost()

        //Tworzymy instancję SpellEffect z nazwą z CardAsset i dołączamy ją
        // Sprawdzamy czy zmienna SpellScriptName nie jest pusta i przechowuje jakąś nazwę skryptu
        if (ca.SpellScriptName!= null && ca.SpellScriptName!= "")
        {
            // Tworzymy instancję aktywatora (pobiera typ(nazwę skryptu czaru)) nadaje mu typ spelleffect
            /// NA przyszłosc: getType sprawdza również duże i małe litery
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.SpellScriptName)) as SpellEffect;
        }
        //Dodaj tę kartę do magazynu z identyfikatorem jako klucz
        // Odwołuje się do tablicy CardsCreatedThisGame i dodaje kartę (add) z jej identyfikatorem i assetem, który przechowywany jest w ca (odwołanie this)
        CardsCreatedThisGame.Add(UniqueCardID, this);
    }

    // method to set or reset mana cost
    public void ResetManaCost()
    {
        // Resetujemy wartość CurrentManaCost do wartości jaką przechowuje CardAsset danej karty
        // Tak dla zabezpieczenia, aby potem krzaki nie wyłaziły
        CurrentManaCost = ca.ManaCost; //podstawowy koszt PPJ
    }

}
