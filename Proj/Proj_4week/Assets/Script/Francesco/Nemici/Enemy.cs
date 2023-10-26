using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] AudioSource damageSfx;
    [SerializeField] AudioSource deathSfx;



    void Start()
    {
        health = maxHealth;    //Reset della vita
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject collObj = collision.gameObject;
        IPlayer playerCheck = collObj.GetComponent<IPlayer>();

        if (playerCheck != null)    //Se ha colliso con il giocatore
        {
            //Danneggia il giocatore
            //collObj.GetComponent<PlayerStatsManager>().Pl_TakeDamage();
        }
    }



    public void En_TakeDamage(int damage)
    {
        if (health > 0)    //Se ha ancora punti vita...
        {
            health -= damage;
        }


        //Feedback
        deathSfx.PlayOneShot(deathSfx.clip);


        En_CheckDeath();
    }

    public void En_CheckDeath()
    {
        bool idDead = health <= 0;

        if (idDead)   //Se viene ucciso...
        {
            //Aggiunge il punteggio al giocatore
            stats_SO.AddScore(scoreAtDeath);

            //Nasconde il nemico
            gameObject.SetActive(false);
        }
    }
}
