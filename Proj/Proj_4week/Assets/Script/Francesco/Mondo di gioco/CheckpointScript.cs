using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckpointScript : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] SaveManager saveMng;

    [Space(10)]
    [SerializeField] Vector3 offset;

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource activatedSfx;



    private void Awake()
    {
        saveMng = FindObjectOfType<SaveManager>();

        //Prende il giocatore e lo rimette nella posizione dell'ultimo checkpoint
        //(solo se entra dal menu principale)
        if(stats_SO.GetIsLoadingASave())
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = stats_SO.GetCheckpointPos();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 checkPos = transform.position + offset;

        //Controlla che sia il giocatore a entrare
        //& che non l'ha già preso
        if (other.gameObject.CompareTag("Player")
            &&
            stats_SO.GetCheckpointPos() != checkPos)
        {
            //Aggiorna la posizione del checkpoint
            stats_SO.SetCheckpointPos(checkPos);

            //Salva il gioco
            saveMng.SaveGame();


            //Riproduce il suono quando viene raggiunto un checkpoint
            activatedSfx.Play();
        }
    }

    private void OnDrawGizmos()
    {
        //Mostra il punto con un pallino
        Gizmos.color = new Color(0, 1, 0.5f, 0.5f);
        Gizmos.DrawSphere(transform.position + offset, 0.05f);
    }
}
