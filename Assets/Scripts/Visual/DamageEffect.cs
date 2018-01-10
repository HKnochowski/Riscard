using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

/// Klasa odpowiedzialna za pokazywanie obrażeń zadawanych istotom i bastionom

public class DamageEffect : MonoBehaviour
{

    // tablica sprite'ów z różnymi grafikami rozprysku krwi
    public Sprite[] Splashes;
    public CardAsset[] Decks;

    // UI Image do pokazania plam krwi
    public Image DamageImage;

    // Służy do zanikania wartości alpha
    public CanvasGroup cg;

    // Składnik tekstowy pokazujący ilość obrażeń zadanych przez cel
    public Text AmountText;

    void Awake()
    {
        // wybierz losowy obraz
        DamageImage.sprite = Splashes[Random.Range(0, Splashes.Length)];
    }

    // Do kontrolowania zaniku tego efektu (alpha)
    private IEnumerator ShowDamageEffect()
    {
        // spraw aby efekt był nieprzejrzysty
        cg.alpha = 1f;
        // odczekaj 1 sekundę przed zanikaniem
        yield return new WaitForSeconds(1f);
        // stopniowo zmiejszaj efekt, zmniejając jego wartość alpha
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        // po pokazaniu efektu zostaje zniszczony
        Destroy(this.gameObject);
    }


    public static void CreateDamageEffect(Vector3 position, int amount)
    {
        // wywołaj preefab DamageEffect
        GameObject newDamageEffect = new GameObject();
        // DO_ZROBIENIA GameObject.Instantiate(GlobalSettings.Instance.DamageEffectPrefab, position, Quaternion.identity) as GameObject;
        newDamageEffect = GameObject.Instantiate(DamageEffectTest.Instance.DamagePrefab, position, Quaternion.identity) as GameObject;
        // Uzyskaj składnik DamageEffect w tym nowym obiekcie gry
        DamageEffect de = newDamageEffect.GetComponent<DamageEffect>();
        // Zmień tekst, aby odzwierciedlić ilość zadawanych obrażeń
        de.AmountText.text = "-" + amount.ToString();
        // uruchom ShowDamageEffect()
        de.StartCoroutine(de.ShowDamageEffect());
    }
}
