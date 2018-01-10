using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginAsset : ScriptableObject
{
    [Header("Player info")]
    public string Email;
    public string Password;
    public int PlayerID;
    public Sprite PlayerAvatar;
    public int CardMagazineID; //Do stworzenia, tutaj powinno być odwołanie dla magazynu kart per gracz?
    
}
