using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStatsManager : MonoBehaviour, IPlayer
{
    //DeathManager deathMng;
    //PlayerMovRB playerMovScr;

    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] PlayerAnimationManager animMng;
    [SerializeField] PauseMenu pauseScr;

    [Space(20)]
    [Range(-100, 0)]
    [SerializeField] float yMinDeath = -10;
    [Min(1)]
    [SerializeField] int maxHealth = 3;
    public int health;
    bool canBeDamaged,
         isDead;

    [Space(10)]
    [Min(0.1f)]
    [SerializeField] float invSec = 3;

    [Space(10)]
    [SerializeField] Transform playerToRespawn;
    [Min(0.1f)]
    [SerializeField] float secBeforeCheckpoint = 2,
                           secBeforeReload = 10;

    [Header("�� Feedback ��")]
    [SerializeField] AudioSource deathSfx;
    [SerializeField] Color invColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] List<SpriteRenderer> playerSprites;
    [SerializeField] GameObject gameOverCanvas;

    [Space(10)]
    [SerializeField] AudioSource jumpSfx;
    [SerializeField] AudioSource damageSfx;
    [SerializeField] AudioSource powUpPickUpSfx;
    [Space(5)]
    [SerializeField] AudioSource collectablePickUpSfx;

    [Header("�� DEBUG ��")]
    [SerializeField] float deathZoneSize = 15;



    private void Awake()
    {
        //deathMng = FindObjectOfType<DeathManager>();
        //playerMovScr = FindObjectOfType<PlayerMovRB>();

        ResetAllHealthVariables();
        stats_SO.SetCheckpointPos(playerToRespawn.position);

        //Reset degli sprite
        ActivatePlayer(true);
        gameOverCanvas.gameObject.SetActive(false);
        pauseScr.SelectGameOver();
        //Reset del punteggio
        stats_SO.ResetScore();


        //Fissa il frame-rate da raggiungere dal gioco a 60 fps
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        //Muore quando supera il limite minimo sulla Y
        if (playerToRespawn.position.y <= yMinDeath)
        {
            SetHealthToZero();
        }


        #region Feedback

        //Cambia lo sprite quando
        //pu� essere danneggiato
        foreach (SpriteRenderer spr in playerSprites)
        {
            spr.color = canBeDamaged
                         ? Color.white
                         : invColor;
        }

        #endregion
    }


    #region Danno e Morte

    public void SetHealthToZero()
    {
        health = 0;

        Pl_CheckDeath();
    }

    public void Pl_TakeDamage()
    {
        //Se pu� essere danneggiato...
        if (canBeDamaged)
        {
            if (health - 1 >= 0)     //Se ha ancora punti vita
            {
                health--;


                #region Feedback

                damageSfx.pitch = Random.Range(0.8f, 1.75f);
                damageSfx.Play();

                animMng.TriggerDamage();    //Animazione

                #endregion


                canBeDamaged = false;
                Invoke(nameof(EnableCanBeDamaged), invSec);
            }
        }

        Pl_CheckDeath();
    }

    void EnableCanBeDamaged()
    {
        canBeDamaged = true;
    }

    public void Pl_CheckDeath()
    {
        bool canDie = health <= 0;

        if (canDie && !isDead)   //Se si puo' uccidere
        {
            stats_SO.RemoveLife();    //Toglie una vita

            isDead = true;


            animMng.SetDeath(true);    //Animazione


            ResetAllPowerUps();

            pauseScr.EnableAllScripts(false);    //Disabilita tutti gli script


            if (stats_SO.GetLives() <= 0)    //Se NON hai pi� vite
            {
                Pl_Die();

                #region Feedback

                //TODO

                #endregion
            }
            else    //Se hai ancora altre vite
            {
                Die_RespawnFromCheckpoint();

                #region Feedback

                //TODO

                #endregion
            }
        }
    }

    public void Die_RespawnFromCheckpoint()
    {
        //Ricarica l'ultimo checkpoint
        Invoke(nameof(Respawn), secBeforeCheckpoint);

        //Nasconde lo sprite del
        //giocatore e lo blocca
        ActivatePlayer(false);
        //playerMovScr.GetRB().bodyType = RigidbodyType2D.Static;


        #region Feedback

        //Audio
        //deathMng.ActivateLevelMusic(false);    //Disattiva la musica
        deathSfx.Play();                    //Riproduce il suono di morte

        #endregion
    }

    public void Pl_Die()
    {
        //Ricarica l'intero livello
        Invoke(nameof(ReloadScene), secBeforeReload);

        //Nasconde lo sprite del giocatore
        ActivatePlayer(false);

        //Mostra la schermata di Game Over
        gameOverCanvas.gameObject.SetActive(true);

        pauseScr.SelectGameOver();
        #region Feedback

        //Audio
        //deathMng.ActivateLevelMusic(false);    //Disattiva la musica
        deathSfx.Play();                    //Riproduce il suono di morte

        #endregion
    }


    void ActivatePlayer(bool value)
    {
        foreach (SpriteRenderer spr in playerSprites)
        {
            spr.enabled = value;
        }
    }

    void Respawn()
    {
        //Mostra il giocatore
        ActivatePlayer(true);
        
        //Abilita tutti gli script disabilitati prima
        pauseScr.EnableAllScripts(true);

        //Riporta il giocatore al checkpoint
        playerToRespawn.position = stats_SO.GetCheckpointPos();

        //Reset delle variabili della vita
        ResetHealthVariables_Respawn();

        //Fa muovere il giocatore
        //playerMovScr.GetRB().bodyType = RigidbodyType2D.Dynamic;

        //Animazione
        animMng.SetDeath(false);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion


    #region Rinascita (Checkpoint + dal livello)

    public void RespawnFromCheckpoint()
    {
        //shootScr.FullyRechargeAmmo();

        //deathMng.ActivateScripts(true);

        //Rimette il giocatore nella posizione dell'ultimo checkpoint
        playerToRespawn.position = stats_SO.GetCheckpointPos();
    }

    public void RespawnLevel()
    {
        //Ricarica il livello in s�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion



    public int GetHealth() => health;



    #region Funzioni Reset

    void ResetAllPowerUps()
    {
        StopAllCoroutines();

        //SetHasBonusHealth(false);

        //shootScr.SetIsShootBoostActive(false);
    }


    void ResetAllHealthVariables()
    {
        health = maxHealth;
        stats_SO.ResetLives();

        canBeDamaged = true;
        isDead = false;

        //hasBonusHealth = false;
    }


    void ResetHealthVariables_Respawn()
    {
        health = maxHealth;

        canBeDamaged = true;
        isDead = false;

        //hasBonusHealth = false;
    }


    public void ResetMaxHealth()
    {
        maxHealth = 3;
    }

    public bool AddOneHealthPoint()
    {
        if (health + 1 <= maxHealth)
        {
            health++;

            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion



    #region EXTRA - Gizmos

    private void OnDrawGizmosSelected()
    {
        Vector3 deathOffset = Vector3.up * yMinDeath,
                cubeOffset = Vector3.up * (deathZoneSize / 2),
                pos_yDeath = Vector3.zero + deathOffset - cubeOffset;
        pos_yDeath.x = playerToRespawn.position.x;

        //Disegna un rettangolo dove il giocatore muore
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(pos_yDeath, new Vector3(deathZoneSize * 2,
                                                    deathZoneSize ,
                                                    deathZoneSize * 2));
    }

    #endregion
}
