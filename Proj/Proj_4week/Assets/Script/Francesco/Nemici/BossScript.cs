using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : Enemy
{
    [Space(20)]
    [SerializeField] OptionsSO_Script options_SO;

    [Space(10)]
    [Min(0)]
    [SerializeField] float vulnerablePhaseSec = 45;
    [Min(0)]
    [SerializeField] float invulnerablePhaseSec = 45;

    int phaseNum = 1;
    bool isInvincible = false,
         canLookPlayer = true;

    Collider bossColl;
    GameObject player;

    [Header("—— Fase vulnerabile ——")]
    [SerializeField] Collider attackCollider;
    [Min(0)]
    [SerializeField] float attackRate = 5f;
    [Range(0, 1)]
    [SerializeField] float meleeAtkActiveTime = 1f;
    bool canAttackPlayer_melee;

    [Header("—— Fase invulnerabile ——")]
    [SerializeField] GameObject bulletToShoot;
    [SerializeField] Transform shoot_spawnPoint;
    [Min(0)]
    [SerializeField] float fireRate = 2.5f;
    [Space(10)]
    [SerializeField] Transform head;

    bool doOnce_switchPhase = true;
    bool doOnce_melee = true;
    bool doOnce_shoot = true;
    bool doOnce_death = true;

    [Header("—— Morte ——")]
    [Min(0)]
    [SerializeField] float secToWaitAfterDeath = 5;
    [SerializeField] ParticleSystem deathPart;

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource meleeAtkSfx;
    [SerializeField] AudioSource shootSfx;
    [SerializeField] Animator bossAnim;




    void Awake()
    {
        isInvincible = false;

        StartCoroutine(StartInitialLaugh());
    }

    void Update()
    {
        if (canLookPlayer)
        {
            //Guarda sempre il giocatore
            head.LookAt(player.transform);
        }


        #region Cambio della fase ogni tot tempo

        if (doOnce_switchPhase)
        {
            switch (phaseNum)
            {
                case 1:

                    StartCoroutine(SwitchPhase(invulnerablePhaseSec,
                                               2));

                    break;

                case 2:
                    StartCoroutine(SwitchPhase(vulnerablePhaseSec,
                                               1));
                    break;
            }

            doOnce_switchPhase = false;
        }

        #endregion


        #region Sistemazione delle fasi

        switch (phaseNum)
        {
            //---Fase melee (vulnerabile)---//
            case 1:
                if (doOnce_melee && canAttackPlayer_melee)
                {
                    //Attiva il collider per attaccare
                    //e lo nasconde dopo tot tempo
                    attackCollider.gameObject.SetActive(true);
                    Invoke(nameof(HideMeleeCollider), meleeAtkActiveTime);

                    //Feedback
                    meleeAtkSfx.PlayOneShot(meleeAtkSfx.clip);
                    bossAnim.SetTrigger("attack");



                    doOnce_melee = false;
                    canAttackPlayer_melee = false;

                    StartCoroutine(EnableMeleeAttack());
                }

                //Ferma l'altro attacco
                StopCoroutine(EnableShootAttack());
                doOnce_shoot = true;
                break;


            //---Fase spara (invulnerabile)---//
            case 2:
                if (doOnce_shoot)
                {
                    //Prende la rotazione verso il giocatore
                    Quaternion playerDir = Quaternion.LookRotation(player.transform.position);

                    //Crea il proiettile ("sputo")
                    Instantiate(bulletToShoot, shoot_spawnPoint.position, playerDir);


                    //Feedback
                    shootSfx.PlayOneShot(shootSfx.clip);
                    bossAnim.SetTrigger("attack");



                    doOnce_shoot = false;

                    StartCoroutine(EnableShootAttack());
                }

                //Ferma l'altro attacco
                StopCoroutine(EnableMeleeAttack());

                break;


            //---Morte---//
            case -1:
                if (doOnce_death)
                {
                    StopAllCoroutines();    //Ferma tutti gli attacchi

                    StartCoroutine(WaitAndFinishBoss());    //Fa gli effetti di morte e poi
                                                            //mostra lo schermo di vittoria


                    doOnce_death = false;
                }
                break;
        }

        #endregion


        //Cambia se si trova in alto o meno rispetto alla fase
        //(vedi la region "Sistemazione delle fasi")
        bossAnim.SetBool("isUp", phaseNum >= 2);
    }


    #region Funzioni per le Fasi

    IEnumerator SwitchPhase(float secToWait, int phaseToSwitch)
    {
        phaseNum = phaseToSwitch;
        bossAnim.SetTrigger("changePhase");    //Feedback

        yield return new WaitForSeconds(secToWait);
        
        doOnce_switchPhase = true;
    }

    IEnumerator EnableMeleeAttack()
    {
        yield return new WaitForSeconds(attackRate);
                
        doOnce_melee = true;
    }

    IEnumerator EnableShootAttack()
    {
        yield return new WaitForSeconds(fireRate);
                
        doOnce_shoot = true;
    }


    void HideMeleeCollider()
    {
        attackCollider.gameObject.SetActive(false);
    }

    #endregion


    #region Inizio e Morte

    IEnumerator StartInitialLaugh()
    {
        bossAnim.SetBool("start", true);


        //Aspetta finchè non finisce la prima animazione
        yield return new WaitUntil(() => bossAnim
                                         .GetCurrentAnimatorStateInfo(0)
                                         .normalizedTime > 1);


        bossAnim.SetBool("start", false);
    }

    IEnumerator WaitAndFinishBoss()
    {
        //Feedback
        deathPart.gameObject.SetActive(true);
        deathPart.Play();


        yield return new WaitForSeconds(secToWaitAfterDeath);


        //Nasconde il Boss
        gameObject.SetActive(false);
    }

    #endregion



    public void SetCanLookPlayer(bool value)
    {
        canLookPlayer = value;
    }
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }

    public void SetCanAttackPlayer_Melee(bool value)
    {
        canAttackPlayer_melee = value;
    }

    public void SetStart_Anim(bool value)
    {
        bossAnim.SetBool("start", value);
    }


    #region EXTRA - Cambiare l'inspector

    private void OnValidate()
    {
        //Limita il tempo di quanto può rimanere
        attackRate = Mathf.Max(meleeAtkActiveTime, attackRate);
    }

    #endregion
}
