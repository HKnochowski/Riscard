using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneReloader: MonoBehaviour {

    //Skrypt, który ponownie uruchamia scenę od nowa
    public void ReloadScene()
    {
        Debug.Log("Scene reloaded");
        // Resetowanie ID wszystkich kart i bastionów
        IDFactory.ResetIDs(); //Wywołanie metody ResetIDs z klasy(skryptu) IDFactory
        IDHolder.ClearIDHoldersList(); //Wywołanie metody ClearIDHoldersList z klasy(skryptu) IDHolder
        Command.CommandQueue.Clear(); //Wywołanie pola CommandQueue z klasy Command i wyczyszczenie go (pozostawienie pustego pola)
        Command.CommandExecutionComplete(); //Wywołanie metody CommandExecutionComplete() z klasy Command
        //Załadowanie nowej sceny
        //Ładuje aktualnie aktywną scenę i pobiera jej nazwę
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
