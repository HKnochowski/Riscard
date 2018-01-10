using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;


//https://www.youtube.com/watch?v=5l02kT2o5mg
public class LoginScript : MonoBehaviour {
 #region Variables
    //Statyczne zmienne
    public static string Email = "";
    public static string Password = "";

    //Publiczne zmienne

    public string CurrentMenu = "Login";

    //Prywatne zmienne
    private string ConfirmPass = "";
    private string ConfirmEmail = "";
    private string CEmail = "";
    private string CPassword = "";

    //Interfejs graficzny
    public float X;
    public float Y;
    public float Widht;
    public float Heiht;

    #endregion

    void Start()
    {
        
    }

    void OnGUI()
    {
        if(CurrentMenu == "Login")
            {
                //Jeśli aktualne menu jest na loginie to wyświetla okno logowania za pomocą funkcji LoginGUI
                //W przeciwnym wypadku wyświetla okno tworzenia konta CreateAccountGUI
                
                    LoginGUI();
                } 
            else if (CurrentMenu == "CreateAccount")
                {
                    CreateAccountGUI();
                }
            
    }

#region Custom methods
    void LoginGUI() //Funkcja odpowiadająca za okno Zaloguj się
    {
        //Wyświetlenie oraz tworzenie okna logowania  
        GUI.Box(new Rect(280, 120,(Screen.width / 4) + 100, (Screen.height / 4) + 250), "Logowanie"); //tworzy okno gdzie znajdują się pola do logowania
        //Tworzenie przycisku logowania
        if(GUI.Button(new Rect(370, 360, 120, 25), "Stwórz konto"))
        {
            CurrentMenu = "CreateAccount"; //przerzuca użytkownika do okna stwórz konto
        }
        //Tworzenie przycisku do logowania
        if (GUI.Button(new Rect(520, 360, 120, 25), "Zaloguj się"))
           
        {
            if (Email == "Test" && Password == "Test2") //System logowania na potrzeby prototypu
            {
                //SceneManager.LoadLevel("MainMenu"); //Do poprawy z funkcją scenemanager
                Application.LoadLevel("MainMenu");
            }
            else
            {
                Debug.Log("Wpisałeś błędne dane"); //Inaczej wywal nam w konsoli informacje
            }
        }
        //Okno wpisania loginu
        GUI.Label(new Rect(390, 200, 220, 23), "Login"); //Opis pola do wpisywania
        Email = GUI.TextField(new Rect(390, 225, 220, 23), Email); //pole do wpisywania

        //Okno wpisania hasła
        GUI.Label(new Rect(390, 255, 220, 23), "Hasło"); //Opis pola do wpisywania
        Password = GUI.TextField(new Rect(390, 280, 220, 23), Password); //Pole do wpisywania
    }

    void CreateAccountGUI() //Funkcja odpowiadająca za okno załóż konto
    {
        //Wyświetlenie okna tworzenia konta
        GUI.Box(new Rect(280, 120, (Screen.width / 4) + 100, (Screen.height / 4) + 250), "Stwórz konto"); //tworzy okno gdzie znajdują się pola do rejestracji
        GUI.Label(new Rect(390, 200, 220, 23), "Login"); //Opis pola do wpisywania loginu
        CEmail = GUI.TextField(new Rect(390, 225, 220, 23), CEmail); //Pole do wpisywania loginu
        GUI.Label(new Rect(390, 255, 220, 23), "Hasło"); //Opis pola do wpisywania hasła
        CPassword = GUI.TextField(new Rect(390, 280, 220, 23), CPassword); //Pole do wpisania hasła
        GUI.Label(new Rect(390, 310, 220, 23), "Potwierdź login"); //Opis pola do potwierdzenia loginu
        ConfirmEmail = GUI.TextField(new Rect(390, 340, 220, 23), ConfirmEmail); //Pole do wpisania loginu
        GUI.Label(new Rect(390, 370, 220, 23), "Potwierdź hasło"); //Opis pola do potwierdzenia hasła
        ConfirmPass = GUI.TextField(new Rect(390, 400, 220, 23), ConfirmPass); //Pole do wpisania hasła

        //Potwierdzanie danych
        if (GUI.Button(new Rect(370, 460, 120, 25), "Stwórz konto"))
        {
            if (ConfirmPass == CPassword && ConfirmEmail == CEmail)
            {
                //StartCoroutine();
            }
            else
            {
                //StartCoroutine();
            }
        }
        //Okno logowania
        if (GUI.Button(new Rect(520, 460, 120, 25), "Cofnij"))

        {
            CurrentMenu = "Login";
        }
    }

#endregion

}
