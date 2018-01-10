using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Membership //przynależność pola
{
    Player1,
    Player2,
    none
}

public enum Building //co znajduje się na danym polu
{
    bastion,
    menhir,
    tower,
    empty
}

public enum Warfog //dla kogo jest wyświetlana mgła wojny
{
    Player1,
    Player2,
    none
}

public class HexAsset : ScriptableObject
{
    // Ten obiekt przechowuje podstawowe informacje na temat pojedynczego hexa
    [Header("Podstawowe informacje")]
    [Header("Id Hexa")]
    public int IdHex; // id hexa
    [Header("Czy poje jest widoczne")]
    public bool HexVisible; //czy pole jest widoczne?
    [Header("Czy na polu jest mgła wojny")]
    public bool WarFogHere; //czy na polu jest mgła wojny
    [Header("Czy gracz ma możliwość położenia karty")]
    public bool putCard; //czy gracz ma możliwość położenia karty
    [Header("Czy na tym polu jest karta gracza")]
    public bool isPlayerHere; //czy na tym polu jest karta gracza
    public string Color;

    [Header("Przynależność pola")]
    public Membership mTarget;
    [Header("Co znajduje się na danym polu")]
    public Building bTarget;
    [Header("Dla kogo jest wyświetlana mgła wojny")]
    public Warfog wTarget;

}