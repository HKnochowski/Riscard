using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IDHolder : MonoBehaviour {

    public int UniqueID;        //zmienna id
    private static List<IDHolder> allIDHolders = new List<IDHolder>();      //lista przechowująca wszystkie zmienne id

    void Awake()
    {
        allIDHolders.Add(this);   //uzupełnienie listy przy inicjalizacji GameObject
    }

    public static GameObject GetGameObjectWithID(int ID) // Pobranie obiektu po jego ID
    {
        foreach (IDHolder i in allIDHolders)    //dla każdego zmiennej id w liście
        {
            if (i.UniqueID == ID)               
                return i.gameObject;            //zwraca id gameobjecta
        }
        return null;
    }

    public static void ClearIDHoldersList()
    {
        allIDHolders.Clear();       //wyczyszczenie listy ID
    }
}
