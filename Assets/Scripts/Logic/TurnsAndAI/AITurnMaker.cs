using UnityEngine;
using System.Collections;

/// <summary>
/// Ta klasa zawiera wszystkie decyzje dla AI.
/// </summary>

public class AITurnMaker: TurnMaker {

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        // wyświetl wiadomość kiedy jest tura przeciwnika
        new ShowMessageCommand("Enemy`s Turn!", 2.0f).AddToQueue();
        p.DrawACard();
        StartCoroutine(MakeAITurn());
    }

    /// <summary>
    /// LOGIKA AI
    /// </summary>
    IEnumerator MakeAITurn()
    {
        bool strategyAttackFirst = false;
        if (Random.Range(0, 2) == 0)
            strategyAttackFirst = true;

        while (MakeOneAIMove(strategyAttackFirst))
        {
            yield return null;
        }

        InsertDelay(1f);

        TurnManager.Instance.EndTurn();
    }

    bool MakeOneAIMove(bool attackFirst)
    {
        if (Command.CardDrawPending())
            return true;
        else if (attackFirst)
            return AttackWithACreature() || PlayACardFromHand() || UseHeroPower();
        else 
            return PlayACardFromHand() || AttackWithACreature() || UseHeroPower();
    }

    bool PlayACardFromHand()
    {
        foreach (CardLogic c in p.hand.CardsInHand)
        {
            if (c.CanBePlayed)
            {
                if (c.ca.MaxHealth == 0)
                {
                    // kod, by zagrać czar z ręki
                    // TODO: w zależności od opcji kierowania, wybierz losowy cel.
                    if (c.ca.Targets == TargetingOptions.NoTarget)
                    {
                        p.PlayASpellFromHand(c, null);
                        InsertDelay(1.5f);
                        //Debug.Log("Card: " + c.ca.name + " can be played");
                        return true;
                    }                        
                }
                else
                {
                    // to karta istoty (CreatureCard)
                    p.PlayACreatureFromHand(c, 0);
                    InsertDelay(1.5f);
                    return true;
                }

            }
            //Debug.Log("Card: " + c.ca.name + " can NOT be played");
        }
        return false;
    }

    bool UseHeroPower()
    {
        if (p.ManaLeft >= 2 && !p.usedHeroPowerThisTurn)
        {
            // użyj HeroPower
            p.UseHeroPower();
            InsertDelay(1.5f);
            //Debug.Log("AI użyło swojej mocy");
            return true;
        }
        return false;
    }

    bool AttackWithACreature()
    {
        foreach (CreatureLogic cl in p.table.CreaturesOnTable)
        {
            if (cl.AttacksLeftThisTurn > 0)
            {
                // atakuj losowy cel ze stworami
                if (p.otherPlayer.table.CreaturesOnTable.Count > 0)
                {
                    int index = Random.Range(0, p.otherPlayer.table.CreaturesOnTable.Count);
                    CreatureLogic targetCreature = p.otherPlayer.table.CreaturesOnTable[index];
                    cl.AttackCreature(targetCreature);
                }                    
                else
                    cl.GoFace();
                
                InsertDelay(1f);
                //Debug.Log("AI zaatakowało stworzenie");
                return true;
            }
        }
        return false;
    }

    void InsertDelay(float delay)
    {
        new DelayCommand(delay).AddToQueue();
    }

}
