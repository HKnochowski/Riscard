using UnityEngine;
using System.Collections;

public class DealDamageCommand : Command {

    private int targetID;
    private int amount;
    private int healthAfter;

    public DealDamageCommand( int targetID, int amount, int healthAfter)
    {
        this.targetID = targetID;
        this.amount = amount;
        this.healthAfter = healthAfter;
    }

    public override void StartCommandExecution()
    {
        Debug.Log("In deal damage command!");

        GameObject target = IDHolder.GetGameObjectWithID(targetID);
        if (targetID == GlobalSettings.Instance.LowPlayer.PlayerID || targetID == GlobalSettings.Instance.TopPlayer.PlayerID)
        {
            // Celem jest bohater
            target.GetComponent<PlayerPortraitVisual>().TakeDamage(amount,healthAfter);
        }
        else
        {
            // Celem jest stworzenie
            target.GetComponent<OneCreatureManager>().TakeDamage(amount, healthAfter);
        }
        CommandExecutionComplete();
    }
}
