using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] //Atrybut Serializable pozwala osadzić klasę z właściwościami podrzędnymi w inspektorze.
/// Czasami jest tak, że nie wyświetlają się w inspektorze parametry do których można coś podpiąć, serializable wymusza takie wyświetlanie
public class CreatureLogic: ICharacter 
{
    // PUBLIC FIELDS
    public Player owner; // odwołanie do klasy Payer, odpowiada za przydzielenie karty do konkretnego gracza
    public CardAsset ca; //referencja, który asset trzyma informacje o karcie
    public CreatureEffect effect; // skrypt, który dodaje do stworzenia specjalne zdolności
    public int UniqueCreatureID; //Id stworzenia na stole
    public bool Frozen = false; // Zmienna określająca to czy karta jest zamrożona (jest niedostępna w aktualnej turze)

    // PROPERTIES
    public int ID // Odwołanie do zmiennej UniqueCardID poprzez ;get
    {
        get{ return UniqueCreatureID; }
    }

   
    private int baseHealth; // podstawowe życie jakie mamy w CardAsset
    public int MaxHealth // Odwołanie do zmiennej baseHealth poprzez ;get
    {
        get{ return baseHealth;}
    }

    private int health;    // aktualne życie dla tej postaci
    public int Health 
    {
        get{ return health; } // Odwołanie do zmiennej health poprzez ;get

        set // Przypisywanie życia stworzeniu i sprawdzanie czy przypadkiem nie powinna już zginąć
        {
            // Jeśli health jest większe od MaxHealth
            if (value > MaxHealth)
                health = MaxHealth; // Przypisz do health - MaxHealth
            else if (value <= 0) // Jeśli health jest mniejsze bądz równe 0
                Die(); // umrzyj kartę xD
            else // Jeśli nie
                health = value; // przypisz do health - health
        }
    }

    // zwróć prawdę jeśli atakowaliśmy tą postacią teraz
    public bool CanAttack
    {
        get
        {
            // Sprawdzenie do kogo należy aktualna tura
            // Sprawdzamy poprzez zwracanie wartości boolowskiej
            // Odwołujemy się do funkcji whoseTurn, która zwraca nam turę aktualnego gracza
            // Wykonujemy równanie logiczne i porównujemy zwróconą wartość whoseTurn do wartości jaka jest w zmiennej owner
            // W zależności od równania system zwróci albo true albo false, zapisujemy tą wartość w zmiennej ownersTurn
            bool ownersTurn = (TurnManager.Instance.whoseTurn == owner);
            // Zwraca wartość z równania logicznejgo (tautologii)
            // Jeśli tura należy do gracza (TO TRUE) _i_ Jeśli liczba ataków w turze jest większa od 0 (TO TRUE)
            // _i_ Jeśli Nie jest Frozen (TO TRUE)
            return (ownersTurn && (AttacksLeftThisTurn > 0) && !Frozen);
        }
    }

    // przygotowanie do ataku
    private int baseAttack; // przechowuje informacje o ilości ataku karty
    //atak z bonusem
    public int Attack // ;get do baseAttack
    {
        get{ return baseAttack; }
    }

    // liczba ataków w jednej turze; jeśli (attacksForOneTurn==2 => Furia
    private int attacksForOneTurn = 1;
    public int AttacksLeftThisTurn
    {
        get;
        set;
    }

    // CONSTRUCTOR
    public CreatureLogic(Player owner, CardAsset ca)
    {
        this.ca = ca;
        baseHealth = ca.MaxHealth;
        Health = ca.MaxHealth;
        baseAttack = ca.Attack;
        attacksForOneTurn = ca.AttacksForOneTurn;
        // AttacksLeftThisTurn jest teraz równy 0
        if (ca.Charge)
            AttacksLeftThisTurn = attacksForOneTurn;
        this.owner = owner;
        UniqueCreatureID = IDFactory.GetUniqueID();
        if (ca.CreatureScriptName!= null && ca.CreatureScriptName!= "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.CreatureScriptName), new System.Object[]{owner, this, ca.specialCreatureAmount}) as CreatureEffect;
            effect.RegisterEffect();
        }
        CreaturesCreatedThisGame.Add(UniqueCreatureID, this);
    }

    // METHODS
    public void OnTurnStart()
    {
        AttacksLeftThisTurn = attacksForOneTurn;
    }

    public void Die()
    {   
        owner.table.CreaturesOnTable.Remove(this);

        new CreatureDieCommand(UniqueCreatureID, owner).AddToQueue();
    }

    public void GoFace()
    {
        AttacksLeftThisTurn--;
        int targetHealthAfter = owner.otherPlayer.Health - Attack;
        new CreatureAttackCommand(owner.otherPlayer.PlayerID, UniqueCreatureID, 0, Attack, Health, targetHealthAfter).AddToQueue();
        owner.otherPlayer.Health -= Attack;
    }

    public void AttackCreature (CreatureLogic target)
    {
        AttacksLeftThisTurn--;
        // obliczyć wartości, aby stwór nie wystrzelił polecenia DIE przed wysłaniem komendy Attack
        int targetHealthAfter = target.Health - Attack;
        int attackerHealthAfter = Health - target.Attack;
        new CreatureAttackCommand(target.UniqueCreatureID, UniqueCreatureID, target.Attack, Attack, attackerHealthAfter, targetHealthAfter).AddToQueue();

        target.Health -= Attack;
        Health -= target.Attack;
    }

    public void AttackCreatureWithID(int uniqueCreatureID)
    {
        CreatureLogic target = CreatureLogic.CreaturesCreatedThisGame[uniqueCreatureID];
        AttackCreature(target);
    }

    // STATIC do zarządzania IDs
    public static Dictionary<int, CreatureLogic> CreaturesCreatedThisGame = new Dictionary<int, CreatureLogic>();

}
