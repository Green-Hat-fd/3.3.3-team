using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnemyScript : Enemy
{
    [Space(20)]
    [SerializeField] Rigidbody fishRb;
    [SerializeField] Transform fishModel;
    [SerializeField] Transform playerTr;
    Transform fishTr/*,
              playerTr*/;

    [Header("—— Azioni ——")]
    [SerializeField] Transform returnPoint;
    [Min(0)]
    [SerializeField] float returnVel = 5;
    [Min(0.1f)]
    [SerializeField] float jumpForce = 8.5f;
    [SerializeField] Vector2 secToWaitRange = new Vector2(0.1f, 1);

    bool canReturn,
         canJump = true;

    [Header("—— Feedback ——")]
    [SerializeField] ParticleSystem fishJump_part;


    #region Costanti

    const float MIN_DIST = 0.05f;

    #endregion



    void Awake()
    {
        fishTr = fishRb.transform;
        canReturn = true;
    }

    void Update()
    {
        if (canJump  &&  playerTr != null)
        {
            //Calcola la direzione del giocatore
            Vector3 playerDir = (playerTr.position - returnPoint.position).normalized;

            //if ()
            //{
                //Fa saltare il pesce verso il giocatore
                fishRb.AddForce(playerDir * jumpForce, ForceMode.Impulse);

                //Feedback
                Instantiate(fishJump_part, fishTr.position, Quaternion.identity);

                //Reset della variabile
                canJump = false;
            //}
        }

        //Se può tornare al suo punto iniziale
        if (canReturn)
        {
            //Muove il pesce verso il suo punto di ritorno
            fishTr.position = Vector3.MoveTowards(fishTr.position,
                                                   returnPoint.position,
                                                   Time.deltaTime * returnVel);

            //Fa guardare il pesce sempre verso il punto di ritorno
            fishModel.LookAt(returnPoint);



            float fishDist = Vector3.Distance(fishTr.position, returnPoint.position);

            //Se è arrivato a destinazione...
            if (fishDist <= MIN_DIST)
            {
                //Fa guardare il pesce nella stessa direzione del punto
                fishModel.rotation = returnPoint.rotation;

                //Reset delle variabili
                float randomSec = Random.Range(secToWaitRange.x, secToWaitRange.y);
                Invoke(nameof(SetTrueCanJump), randomSec);

                canReturn = false;
            }

        }
        else
        {
            //Ruota il pesce sempre verso la sua velocità
            fishModel.rotation = Quaternion.LookRotation(fishRb.velocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == fishRb.gameObject)
        {
            Invoke(nameof(SetTrueCanReturn), 0.15f);
            canJump = false;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            playerTr = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerTr = null;
        }
    }

    void SetTrueCanJump()
    {
        canJump = true;
    }

    void SetTrueCanReturn()
    {
        canReturn = true;
    }



    #region EXTRA - Cambiare l'Inspector

    private void OnValidate()
    {
        //Limita il range della rotazione verticale della telecamera
        //(con un min di -90° e un max di 90°)
        secToWaitRange.x = Mathf.Clamp(secToWaitRange.x, -90, secToWaitRange.x);
        secToWaitRange.y = Mathf.Clamp(secToWaitRange.y, secToWaitRange.y, 90);
    }

    #endregion


    #region EXTRA - Gizmo

    private void OnDrawGizmos()
    {
        if (playerTr != null)
        {
            Vector3 dir = (playerTr.position - returnPoint.position).normalized;
        
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(returnPoint.position, dir * jumpForce / 2);
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Solo quando deve tornare...
        if (canReturn)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(fishTr.position, returnPoint.position);
        }
    }

    #endregion
}
