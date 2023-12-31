using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour, IEnemy
{
    [Min(1)]
    [SerializeField] protected int maxHealth = 2;
    protected int health = 0;

    [Space(10)]
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [Min(0)]
    [SerializeField] int scoreAtDeath;

    [Space(10)]
    [SerializeField] NavMeshAgent navMeshAgent;

    [Space(10)]
    [SerializeField] AudioSource damageSfx;
    [SerializeField] AudioSource deathSfx;
    [SerializeField] Animator enemyAnim;



    void Start()
    {
        health = maxHealth;    //Reset della vita
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject collObj = collision.gameObject;

        if (collision.gameObject.CompareTag("Player"))    //Se ha colliso con il giocatore
        {
            //Danneggia il giocatore
            collObj.GetComponent<PlayerStatsManager>().Pl_TakeDamage();
        }

        if (collision.gameObject.CompareTag("Attack"))
        {
            En_TakeDamage(1);
        }
    }



    public void En_TakeDamage(int damage)
    {
        if (health > 0)    //Se ha ancora punti vita...
        {
            health -= damage;
        }


        //Feedback
        damageSfx.PlayOneShot(damageSfx.clip);

        enemyAnim.SetTrigger("damage");


        En_CheckDeath();
    }

    public void En_CheckDeath()
    {
        bool idDead = health <= 0;

        if (idDead)   //Se viene ucciso...
        {
            //Aggiunge il punteggio al giocatore
            stats_SO.AddScore(scoreAtDeath);

            //Toglie il navmesh se c'� l'ha
            if(navMeshAgent != null)
            {
                navMeshAgent.speed = 0;
            }

            //Feedback
            deathSfx.PlayOneShot(deathSfx.clip);
            enemyAnim.SetTrigger("death");

            //Nasconde il nemico
            Invoke(nameof(RemoveEnemy), 5f);
        }
    }

    void RemoveEnemy()
    {
        gameObject.SetActive(false);
    }
}
