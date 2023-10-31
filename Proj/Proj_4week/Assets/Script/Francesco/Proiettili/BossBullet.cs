using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BossBullet : MonoBehaviour
{
    [Min(0)]
    [SerializeField] float projectileSpeed = 2;
    [Min(0)]
    [SerializeField] float bulletLife = 5;



    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnEnable()
    {
        Destroy(gameObject, bulletLife);
    }

    void FixedUpdate()
    {
        //Muove costantemente il proiettile in avanti
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))    //Se colpisce il giocatore
        {
            //Lo danneggia
            PlayerStatsManager playerStatsMng = other.GetComponent<PlayerStatsManager>();

            playerStatsMng.Pl_TakeDamage();

            //Toglie il proiettile
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
