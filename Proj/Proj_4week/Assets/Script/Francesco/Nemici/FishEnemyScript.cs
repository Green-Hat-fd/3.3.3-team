using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnemyScript : Enemy
{
    [Space(20)]
    [SerializeField] Rigidbody fish_rb;
    [SerializeField] Transform playerTr;
    Transform fishTr/*,
              playerTr*/;

    [Header("-")]
    [SerializeField] Transform returnPoint;
    [Min(0)]
    [SerializeField] float returnVel;
    [Min(0.1f)]
    [SerializeField] float jumpForce;
    [SerializeField] Vector2 secToWaitRange = new Vector2(1, 5);

    bool canReturn,
         canJump = true;


    #region Costanti

    const float MIN_DIST = 0.05f;

    #endregion



    void Awake()
    {
        fishTr = fish_rb.transform;
        canReturn = true;
    }

    void Update()
    {
        if (canJump)
        {

            //Calcola la direzione del giocatore
            Vector3 playerDir = (playerTr.position - fishTr.position).normalized;

            //if ()
            //{
                //Spinge il pesce verso ------------------------------
                fish_rb.AddForce(playerDir * jumpForce, ForceMode.Impulse);

                //Reset della variabile
                canJump = false;
            //}
        }

        //Se può tornare al suo punto iniziale
        if (canReturn)
        {
            fish_rb.isKinematic = true;

            //Muove il pesce verso il suo punto di ritorno
            fishTr.position = Vector3.MoveTowards(fishTr.position,
                                                   returnPoint.position,
                                                   Time.deltaTime * returnVel);
            
            //Fa guardare il pesce sempre verso il punto di ritorno
            fishTr.LookAt(returnPoint);


            float fishDist = Vector3.Distance(fishTr.position, returnPoint.position);

            //Se è arrivato a destinazione...
            if (fishDist <= MIN_DIST)
            {
                //
                fish_rb.isKinematic = false;

                //Fa guardare il pesce nella stessa direzione del punto
                fishTr.rotation = returnPoint.rotation;

                //Reset delle variabili
                canJump = true;
                canReturn = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == fish_rb.gameObject)
        {
            canReturn = true;
            canJump = false;
        }
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
