using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CreatureAttackVisual : MonoBehaviour 
{
    private OneCreatureManager manager;  //scrypt tworzący wygląd karty
    private WhereIsTheCardOrCreature w;  //określa gdzie znajduje się karta

    void Awake()    
    {
        manager = GetComponent<OneCreatureManager>();   //pobiera wygląd karty jeszcze przed rozpoczęciem gry
        w = GetComponent<WhereIsTheCardOrCreature>();   //pobiera właściwość kart określająca gdzie znajduje się karta
    }

    public void AttackTarget(int targetUniqueID, int damageTakenByTarget, int damageTakenByAttacker, int attackerHealthAfter, int targetHealthAfter)
    {
        Debug.Log(targetUniqueID);          //wypisuje na konsoli ID karty
        manager.CanAttackNow = false;       //po wyłożeniu karty blokuje możliwość ataku daną kartą
        GameObject target = IDHolder.GetGameObjectWithID(targetUniqueID);   //przypisuje 

        // przypisz temu stworowi pierwszeństwo
        w.BringToFront();                           
        VisualStates tempState = w.VisualState;
        w.VisualState = VisualStates.Transition;

        transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InCubic).OnComplete(() =>
            {
                if(damageTakenByTarget>0)
                    DamageEffect.CreateDamageEffect(target.transform.position, damageTakenByTarget);        //tworzenie wyglądu ataku atakującego
                if(damageTakenByAttacker>0)
                    DamageEffect.CreateDamageEffect(transform.position, damageTakenByAttacker);             //tworzenie wyglądu ataku broniącego się

                if (targetUniqueID == GlobalSettings.Instance.LowPlayer.PlayerID || targetUniqueID == GlobalSettings.Instance.TopPlayer.PlayerID)
                {
                    // celem jest gracz
                    target.GetComponent<PlayerPortraitVisual>().HealthText.text = targetHealthAfter.ToString();
                }
                else
                    target.GetComponent<OneCreatureManager>().HealthText.text = targetHealthAfter.ToString();       //celem jest potwór

                w.SetTableSortingOrder();                   //ustawienie sortowania na stole
                w.VisualState = tempState;                  //przypisuje wartości tymczasowe w danym miejscu 

                manager.HealthText.text = attackerHealthAfter.ToString();
                Sequence s = DOTween.Sequence();
                s.AppendInterval(1f);
                s.OnComplete(Command.CommandExecutionComplete);
                //Command.CommandExecutionComplete();
            });
    }
        
}
