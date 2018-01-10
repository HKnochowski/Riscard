using UnityEngine;
using System.Collections;

public class PlayerTurnMaker : TurnMaker 
{
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        // wyświetl wiadomość kiedy jest tura gracza
        new ShowMessageCommand("Your Turn!", 2.0f).AddToQueue();
        p.DrawACard();
    }
}
