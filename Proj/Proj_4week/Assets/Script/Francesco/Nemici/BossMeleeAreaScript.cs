using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BossMeleeAreaScript : MonoBehaviour
{
    [SerializeField] BossScript bossScr;



    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print($"\"{other.name}\" ({other.tag}, {other.gameObject.CompareTag("Player")})");

        //Se è entrato il giocatore...
        if (other.gameObject.CompareTag("Player"))
        {
            bossScr.SetPlayer(other.gameObject);
            bossScr.SetCanAttackPlayer_Melee(true);
        }
    }
}
