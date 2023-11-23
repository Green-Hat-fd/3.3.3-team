using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKillPlayer : MonoBehaviour
{
    public PlayerStatsManager psm;
    private void OnTriggerEnter(Collider other)
    {
        GameObject collObj = other.gameObject;

        if (other.gameObject.CompareTag("Player"))    //Se ha colliso con il giocatore
        {
            //Danneggia il giocatore
            psm.SetHealthToZero();
        }
    }
}
