using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Skrypt odpowiedzialny za tasowanie talii kart podczas rozgrywki
/// </summary>
public static class ShufflingExtention {

    //Odwołanie się do systemowego pseudolosowego generatora liczb
    //Zainincjowanie zmiennej generatora liczb o nazwie "rng"
    //Przypisanie do zmiennej "rng" funkcji pseudolosowego generatora liczb
    private static System.Random rng = new System.Random();

    //Funkcja tasująca karty w talii
    //Funkcja odwołuje się do kolekcji obiektów "list", które mogą być udostępniane indywidualnie przez indeks
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  //przypisanie do zmiennej "int n", liczby elementów znajdujących się w "list". Pobieranie liczby odbywa się poprzez funkcję "Count"
        //Pętla while która kończy się gdy n będzie mniejsze od 1
        while (n > 1) {  
            n--;  //z każdą iteracją zmieniaj wartość n o -1
            int k = rng.Next(n + 1);  //Zainicjowanie zmiennej "int k". Przypisanie do zmiennej k losowej liczby nieujemnej powiększonej o 1
            T value = list[k]; //Zainicjowanie obiektu "T" o nazwie "value" i przypisanie do niego obiektu list z indeksem k 
            list[k] = list[n];  //Przypisanie do obiektu list z indeksem k, obiektu list z indeksem n
            list[n] = value;  //Przypisanie do obiektu list z indeksem n obiektu value
        }  
    }
}
