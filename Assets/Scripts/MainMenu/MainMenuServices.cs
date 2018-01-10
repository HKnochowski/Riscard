using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuServices : MonoBehaviour
{

    void Start()
    {

    }

    void OnGUI() //Odpalamy menu gry, UWAGA - nie wiem dlaczego, ale jeśli tutaj nastąpi zmiana w nazwie to okno wypada z obiegu - potrzebuję wsparcia i wyjaśnienia
    {

            Menu();

    }

    void Menu() 
    {
        GUI.Box(new Rect(360, 120, (Screen.width / 8) + 100, (Screen.height / 16) + 250), "Menu Gry");  //Opis okna
        if (GUI.Button(new Rect(390, 170, 225, 80), "Rozpocznij grę")) //Rozmiar i pozycja przycisku odpowiadającego za rozpoczęcie gry
        {
            Application.LoadLevel("BattleScene"); //Przejście do sceny z grą
           
        }

        if (GUI.Button(new Rect(390, 260, 225, 80), "Talia i karty")) //Rozmiar i pozycja przycisku odpowiadającego za talię i karty

        {
             Application.LoadLevel("PlayerPanel"); //Przejście do talii i kart
            
        }
    }
}