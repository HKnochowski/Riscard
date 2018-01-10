using UnityEngine;
using System.Collections;

public class StartATurnCommand : Command {

    private Player p;

    public StartATurnCommand(Player p)
    {
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        TurnManager.Instance.whoseTurn = p;
        // to polecenie jest natychmiast zakończone
        CommandExecutionComplete();
    }
}
