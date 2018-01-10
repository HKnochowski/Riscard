using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, ICharacter
{


    // PUBLIC FIELDS
    // int ID, który otrzymujemy z ID Factory
    public int PlayerID;
    // Zasób postaci, który zawiera dane o tym Bohaterze
    public CharacterAsset charAsset;
    // skrypt z odniesieniami do wszystkich wizualnych obiektów gry dla tego gracza
    public PlayerArea PArea;
    // skrypt typu Efekt czaru, który zostanie użyty dla mocy naszego bohatera
    // jest w pewien sposób zaklęciem
    public SpellEffect HeroPowerEffect;
    // ta wartość jest używana wyłącznie do zaklęcia monety
    private int bonusManaThisTurn = 0;
    // Flaga, aby uniemożliwić używanie mocy bohatera dwa razy
    public bool usedHeroPowerThisTurn = false;

    // ODNOŚNIKI DO LOGICZNYCH MATERIAŁÓW, KTÓRE NALEŻY PODAĆ DO TEGO GRACZA
    public Deck deck;
    public Hand hand;
    public Table table;

    // statyczna tablica, która będzie przechowywać obu graczy, powinna zawsze mieć 2 graczy
    public static Player[] Players;




    // PROPERTIES 
    // ta właściwość jest częścią interfejsu ICharacter
    public int ID
    {
        get{ return PlayerID; }
    }

    // przeciwnik gracza
    public Player otherPlayer
    {
        get
        {
            if (Players[0] == this)
                return Players[1];
            else
                return Players[0];
        }
    }

    // całkowite kryształy many, które gracz ma w tej turze
    private int manaThisTurn;
    public int ManaThisTurn
    {
        get{ return manaThisTurn;}
        set
        {
            manaThisTurn = value;
            //PArea.ManaBar.TotalCrystals = manaThisTurn;
            new UpdateManaCrystalsCommand(this, manaThisTurn, manaLeft).AddToQueue();
        }
    }

    // pełne kryształy many dostępne już teraz, aby zagrać kartę / użyć mocy bohatera
    private int manaLeft;
    public int ManaLeft
    {
        get
        { return manaLeft;}
        set
        {
            manaLeft = value;
            //PArea.ManaBar.AvailableCrystals = manaLeft;
            new UpdateManaCrystalsCommand(this, ManaThisTurn, manaLeft).AddToQueue();
            //Debug.Log(ManaLeft);
            if (TurnManager.Instance.whoseTurn == this)
                HighlightPlayableCards();
        }
    }

    private int health;
    public int Health
    {
        get { return health;}
        set
        {
            health = value;
            if (value <= 0)
                Die(); 
        }
    }

    
    public delegate void VoidWithNoArguments();
    //public event VoidWithNoArguments CreaturePlayedEvent;
    //public event VoidWithNoArguments SpellPlayedEvent;
    //public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;



    // WSZYSTKIE METODY
    void Awake()
    {
        // znajdź wszystkie skrypty typu Player i zapisz je w tablicy Players
        // (powinno być tylko 2 graczy na scenie)
        Players = GameObject.FindObjectsOfType<Player>();
        // uzyskaj unikalny identyfikator od IDFactory
        PlayerID = IDFactory.GetUniqueID();
    }

    public virtual void OnTurnStart()
    {
        // dodaj jeden punkt ppj do puli;
        Debug.Log("In ONTURNSTART for "+ gameObject.name);
        usedHeroPowerThisTurn = false;
        ManaThisTurn++;
        ManaLeft = ManaThisTurn;
        foreach (CreatureLogic cl in table.CreaturesOnTable)
            cl.OnTurnStart();
        PArea.HeroPower.WasUsedThisTurn = false;
    }

    public void OnTurnEnd()
    {
        if(EndTurnEvent != null)
            EndTurnEvent.Invoke();
        ManaThisTurn -= bonusManaThisTurn;
        bonusManaThisTurn = 0;
        GetComponent<TurnMaker>().StopAllCoroutines();
    }

    // RZECZY KTORE NASZ GRACZ MOŻE ZROBIC

    // zdobycie PPJ z monet lub innych zaklęć 
    public void GetBonusMana(int amount)
    {
        bonusManaThisTurn += amount;
        ManaThisTurn += amount;
        ManaLeft += amount;
    }

    // TYLKO DO TESTU
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            DrawACard();
    }

    // narysuj pojedynczą kartę z talii
    public void DrawACard(bool fast = false)
    {
        if (deck.cards.Count > 0)
        {
            if (hand.CardsInHand.Count < PArea.handVisual.slots.Children.Length)
            {
                // 1) logika: dodaj kartę do ręki
                CardLogic newCard = new CardLogic(deck.cards[0]);
                newCard.owner = this;
                hand.CardsInHand.Insert(0, newCard);
                // Debug.Log(hand.CardsInHand.Count);
                // 2) logika: wyjmij kartę z talii
                deck.cards.RemoveAt(0);
                // 2) Utwórz polecenie
                new DrawACardCommand(hand.CardsInHand[0], this, fast, fromDeck: true).AddToQueue(); 
            }
        }
        else
        {
            // w talii nie ma żadnych kart, zadajesz obrażenia od zmęczenia.
        }

    }

    // dobierz kartę nie z talii (żeton lub monetę)
    public void GetACardNotFromDeck(CardAsset cardAsset)
    {
        if (hand.CardsInHand.Count < PArea.handVisual.slots.Children.Length)
        {
            // 1) logika: dodaj kartę do ręki
            CardLogic newCard = new CardLogic(cardAsset);
            newCard.owner = this;
            hand.CardsInHand.Insert(0, newCard);
            // 2) wyślij wiadomość do talii wizualnej
            new DrawACardCommand(hand.CardsInHand[0], this, fast: true, fromDeck: false).AddToQueue(); 
        }
        // brak usuwania z talii, ponieważ karta nie znajdowała się w talii
    }

    // 2 METODY ODTWARZANIA SPELLÓW
    // 1. przeciążenie - pobiera ids jako argumenty
    // wygodnie jest wywoływać tę metodę z części wizualnej
    public void PlayASpellFromHand(int SpellCardUniqueID, int TargetUniqueID)
    {
        // TODO: !!!
        // jeśli TargetUnique ID <0, na przykład = -1, nie ma celu.
        if (TargetUniqueID < 0)
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], null);
        else if (TargetUniqueID == ID)
        {
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], this);
        }
        else if (TargetUniqueID == otherPlayer.ID)
        {
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], this.otherPlayer);
        }
        else
        {
            // celem jest stworzenie
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], CreatureLogic.CreaturesCreatedThisGame[TargetUniqueID]);
        }
          
    }

    // Drugie przeciążenie - wymaga interfejsu CardLogic i ICharacter - 
    // ta metoda jest wywoływana z Logic, na przykład przez AI
    public void PlayASpellFromHand(CardLogic playedCard, ICharacter target)
    {
        ManaLeft -= playedCard.CurrentManaCost;
        // natychmiast wywołać efekt:
        if (playedCard.effect != null)
            playedCard.effect.ActivateEffect(playedCard.ca.specialSpellAmount, target);
        else
        {
            Debug.LogWarning("No effect found on card " + playedCard.ca.name);
        }
        // bez względu na to, co się stanie, przenieś tę kartę do PlayACardSpot
        new PlayASpellCardCommand(this, playedCard).AddToQueue();
        // usuń tę kartę z ręki
        hand.CardsInHand.Remove(playedCard);
        // sprawdź, czy jest to istota lub zaklęcie
    }

    // METODY DO GRANIA STWORZENIAMI 
    // Pierwsze przeciążenie - według identyfikatora
    public void PlayACreatureFromHand(int UniqueID, int tablePos)
    {
        PlayACreatureFromHand(CardLogic.CardsCreatedThisGame[UniqueID], tablePos);
    }

    // Drugie przeciążenie - przez jednostki logiczne
    public void PlayACreatureFromHand(CardLogic playedCard, int tablePos)
    {
        // Debug.Log(ManaLeft);
        // Debug.Log(playedCard.CurrentManaCost);
        ManaLeft -= playedCard.CurrentManaCost;
        // Debug.Log("Mana Left po zagraniu stworzenia: " + ManaLeft);
        // utwórz nowy obiekt stworzenia i dodaj go do tabeli
        CreatureLogic newCreature = new CreatureLogic(this, playedCard.ca);
        table.CreaturesOnTable.Insert(tablePos, newCreature);
        // bez względu na to, co się stanie, przenieś tę kartę do PlayACardSpot
        new PlayACreatureCommand(playedCard, this, tablePos, newCreature.UniqueCreatureID).AddToQueue();
        // usuń tę kartę z ręki
        hand.CardsInHand.Remove(playedCard);
        HighlightPlayableCards();
    }

    public void Die()
    {
        // game over
        // zablokuj obu graczy od podejmowania nowych ruchów  
        PArea.ControlsON = false;
        otherPlayer.PArea.ControlsON = false;
        TurnManager.Instance.StopTheTimer();
        new GameOverCommand(this).AddToQueue();
    }

    // użyj mocy bohatera - aktywacja jest efektem, jakbyś zagrał zaklęcie
    public void UseHeroPower()
    {
        ManaLeft -= 2;
        usedHeroPowerThisTurn = true;
        HeroPowerEffect.ActivateEffect();
    }

    // METODY POKAZUJĄCE GLOWS
    public void HighlightPlayableCards(bool removeAllHighlights = false)
    {
        //Debug.Log("HighlightPlayable remove: "+ removeAllHighlights);
        foreach (CardLogic cl in hand.CardsInHand)
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
            if (g!=null)
                g.GetComponent<OneCardManager>().CanBePlayedNow = (cl.CurrentManaCost <= ManaLeft) && !removeAllHighlights;
        }

        foreach (CreatureLogic crl in table.CreaturesOnTable)
        {
            GameObject g = IDHolder.GetGameObjectWithID(crl.UniqueCreatureID);
            if(g!= null)
                g.GetComponent<OneCreatureManager>().CanAttackNow = (crl.AttacksLeftThisTurn > 0) && !removeAllHighlights;
        }
        // Podkreśl moc bohatera
        PArea.HeroPower.Highlighted = (!usedHeroPowerThisTurn) && (ManaLeft > 1) && !removeAllHighlights;
    }

    // METODY ROZPOCZĘCIA GRY
    public void LoadCharacterInfoFromAsset()
    {
        Health = charAsset.MaxHealth;
        // zmień wizualizacje portretu, mocy bohatera, itp ...
        PArea.Portrait.charAsset = charAsset;
        PArea.Portrait.ApplyLookFromAsset();
        // TODO: wstaw tutaj kod do załączania skryptu mocy bohatera. 
        if (charAsset.HeroPowerName != null && charAsset.HeroPowerName != "")
        {
            HeroPowerEffect = System.Activator.CreateInstance(System.Type.GetType(charAsset.HeroPowerName)) as SpellEffect;
        }
        else
        {
            Debug.LogWarning("Check hero powr name for character " + charAsset.ClassName);
        }
    }

    public void TransmitInfoAboutPlayerToVisual()
    {
        PArea.Portrait.gameObject.AddComponent<IDHolder>().UniqueID = PlayerID;
        if (GetComponent<TurnMaker>() is AITurnMaker)
        {
            // wyłączyć turę dla tej postaci
            PArea.AllowedToControlThisPlayer = false;
        }
        else
        {
            // Pozwól, aby tura była dla tej postaci
            PArea.AllowedToControlThisPlayer = true;
        }
    }
       
        
}
