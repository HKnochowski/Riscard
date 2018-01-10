using UnityEngine;
using System.Collections;

public class CreatureAttackCommand : Command 
{
    // pozycja stwora na stole wroga, który zostanie zaatakowany
    // jeśli enemyindex == -1, atakuj wrogą postać
    private int TargetUniqueID;
    private int AttackerUniqueID;
    private int AttackerHealthAfter;
    private int TargetHealthAfter;
    private int DamageTakenByAttacker;
    private int DamageTakenByTarget;

    public CreatureAttackCommand(int targetID, int attackerID, int damageTakenByAttacker, int damageTakenByTarget, int attackerHealthAfter, int targetHealthAfter)
    {
        this.TargetUniqueID = targetID;
        this.AttackerUniqueID = attackerID;
        this.AttackerHealthAfter = attackerHealthAfter;
        this.TargetHealthAfter = targetHealthAfter;
        this.DamageTakenByTarget = damageTakenByTarget;
        this.DamageTakenByAttacker = damageTakenByAttacker;
    }

    public override void StartCommandExecution()
    {
        GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);

        //Debug.Log(TargetUniqueID);
        Attacker.GetComponent<CreatureAttackVisual>().AttackTarget(TargetUniqueID, DamageTakenByTarget, DamageTakenByAttacker, AttackerHealthAfter, TargetHealthAfter);
    }
}
