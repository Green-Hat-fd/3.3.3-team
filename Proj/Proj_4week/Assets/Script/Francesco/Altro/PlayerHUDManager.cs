using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] PlayerStatsManager statsMng;

    
    [Header("—— UI ——")]
    [SerializeField] Text scoreTxt;

    [Space(10)]
    [SerializeField] List<Image> healthImages;
    [SerializeField] Sprite healthSpr;
    [SerializeField] Sprite healthLostSpr;
    [SerializeField] Text livesTxt;

    [Space(10)]
    [SerializeField] Slider ammoSlider;



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
    }
}
