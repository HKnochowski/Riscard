using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ManaPoolVisual : MonoBehaviour {

    public int TestFullCrystals;
    public int TestTotalCrystalsThisTurn;

    public Image[] Crystals;
    public Text ProgressText;

    private int totalCrystals;
    public int TotalCrystals
    {
        get{ return totalCrystals; }

        set
        {
            //Debug.Log("Zmieniono pulę PPJ na: " + value);

            if (value > Crystals.Length)                        //jeśli całkowita ilość manu jest większa niż długość tablicy many
                totalCrystals = Crystals.Length;                //ustaw długość tablicy jako ilość many
            else if (value < 0)                                 //jeśli całkowita ilość many jest mniejsza niż 0
                totalCrystals = 0;                              //ustaw 0 many
            else
                totalCrystals = value;                          //ustaw całkowitą ilość many

            for (int i = 0; i < Crystals.Length; i++)           //zaczynając od 0 przechodząc co 1 do długośći tablicy many
            {
                if (i < totalCrystals)                          //jeśli i jest mniejsze niż mana
                {
                    if (Crystals[i].color == Color.clear)       
                        Crystals[i].color = Color.gray;         
                }
                else
                    Crystals[i].color = Color.clear;
            }

            // aktualizuj text
            ProgressText.text = string.Format("{0}/{1}", availableCrystals.ToString(), totalCrystals.ToString());
        }
    }

    private int availableCrystals;
    public int AvailableCrystals
    {
        get{ return availableCrystals; }

        set
        {
            //Debug.Log("Zmieniono pulę PPJ na: " + value);

            if (value > totalCrystals)
                availableCrystals = totalCrystals;
            else if (value < 0)
                availableCrystals = 0;
            else
                availableCrystals = value;

            for (int i = 0; i < totalCrystals; i++)
            {
                if (i < availableCrystals)
                    Crystals[i].color = Color.white;
                else
                    Crystals[i].color = Color.gray;
            }

            // zmień text
            ProgressText.text = string.Format("{0}/{1}", availableCrystals.ToString(), totalCrystals.ToString());

        }
    }

    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            TotalCrystals = TestTotalCrystalsThisTurn;
            AvailableCrystals = TestFullCrystals;
        }
    }
	
}
