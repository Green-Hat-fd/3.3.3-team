using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] PlayerStatsManager statsMng;
    [SerializeField] PlayerHoldItems holdItemsScr;

    
    [Header("—— UI ——")]
    [SerializeField] Text scoreTxt;

    [Space(10)]
    [SerializeField] List<Image> healthImages;
    [SerializeField] Sprite healthSpr;
    [SerializeField] Sprite healthLostSpr;
    [SerializeField] Text livesTxt;

    [Space(10)]
    [SerializeField] Image playerIcon;
    [SerializeField] Sprite normalSprite,
                            pickUpSprite;
    //[SerializeField] SpriteRenderer playerObjBubble;



    void Update()
    {
        //Cambio del testo (punteggio)
        scoreTxt.text = stats_SO.GetScore() + "";


        //Cambia la vita (health)
        for (int i = 0; i < healthImages.Count; i++)
        {
            // Cambia il cuore rispetto a quanta vita rimane
            healthImages[i].sprite = i < statsMng.GetHealth()
                                      ? healthSpr
                                      : healthLostSpr;
        }


        //Cambia il testo delle vite (lives)
        livesTxt.text = "x" + stats_SO.GetLives();


        //Cambia l'icona del giocatore
        playerIcon.sprite = holdItemsScr.GetIsHoldingItem()
                              ? pickUpSprite
                              : normalSprite;


        //Cambia con cosa
    }
}
