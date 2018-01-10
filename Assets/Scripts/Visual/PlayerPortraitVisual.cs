using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPortraitVisual : MonoBehaviour {                         //wygląd bastionu

    public CharacterAsset charAsset;            
    [Header("Text Component References")]
    //public Text NameText;
    public Text HealthText;                     
    [Header("Image References")]
    public Image HeroPowerIconImage;
    public Image HeroPowerBackgroundImage;
    public Image PortraitImage;
    public Image PortraitBackgroundImage;

    void Awake()
	{
		if(charAsset != null)                                               //jeśli wygląd nie zatwierdzony
			ApplyLookFromAsset();                                           //zatwierdz wygląd z assetu
	}
	
	public void ApplyLookFromAsset()        //zatwierdź wygląd z assetu
    {
        HealthText.text = charAsset.MaxHealth.ToString();                   //
        HeroPowerIconImage.sprite = charAsset.HeroPowerIconImage;           //
        HeroPowerBackgroundImage.sprite = charAsset.HeroPowerBGImage;       //
        PortraitImage.sprite = charAsset.AvatarImage;                       //
        PortraitBackgroundImage.sprite = charAsset.AvatarBGImage;           //

        HeroPowerBackgroundImage.color = charAsset.HeroPowerBGTint;         //
        PortraitBackgroundImage.color = charAsset.AvatarBGTint;             //

    }

    public void TakeDamage(int amount, int healthAfter)             //metoda przypisująca nową wartość życia
    {
        if (amount > 0)                                             //jeśli atak jest większy od 0
        {
            DamageEffect.CreateDamageEffect(transform.position, amount);    
            HealthText.text = healthAfter.ToString();               //przypisanie nowej wartości życia
        }
    }

    public void Explode()
    {
        /* TODO
        Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.PrependInterval(2f);
        s.OnComplete(() => GlobalSettings.Instance.GameOverCanvas.SetActive(true));
        */
    }



}
