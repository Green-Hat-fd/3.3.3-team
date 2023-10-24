using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [Min(0)]
    [SerializeField] float platformVel = 1;
    [SerializeField] Transform[] positions;
    int nextPos_index = 0;

    [Space(10)]
    [Min(0)]
    [SerializeField] float timeToWait = 1f;
    float elapsedTime;
    bool isWaiting;

    bool isReverse;


    #region Costanti

    const float MIN_DIST = 0.05f;

    #endregion



    void Update()
    {
        float nextPosDist = Vector3.Distance(transform.position,
                                             positions[nextPos_index].position);

        if (isWaiting)
        {
            //Se è passato abbastanza tempo...
            if (elapsedTime >= timeToWait)
            {
                isWaiting = false;    //Si può muovere
                elapsedTime = 0;      //Resetta il timer
            }
            else
            {
                elapsedTime += Time.deltaTime;  //Aumenta il conteggio del tempo trascorso
            }
        }
        else
        {
            //Movimento verso la prossima posizione
            transform.position = Vector3.MoveTowards(transform.position,
                                                     positions[nextPos_index].position,
                                                     Time.deltaTime * platformVel);
        }


        //Controlla se è arrivato nella posizione
        if (nextPosDist <= MIN_DIST)
        {
            //Per sicurezza, si mette alla fine
            transform.position = positions[nextPos_index].position;

            
            //Ferma la piattaforma
            isWaiting = true;


            //Se va al contrario...
            if (isReverse)
            {
                //Torna a fare il giro in avanti
                //se e' arrivato alla fine, se no continua
                if (nextPos_index <= 0)
                    isReverse = false;
                else
                    nextPos_index--;
            }
            else
            {
                //Torna a fare il giro al contrario
                //se e' arrivato all'inizio, se no continua
                if (nextPos_index >= positions.Length - 1)
                    isReverse = true;
                else
                    nextPos_index++;
            }
        }
    }


    #region Funzioni per far entrare/uscire

    private void OnTriggerStay(Collider other)
    {
        //Se il giocatore è entrato...
        if (other.gameObject.CompareTag("Player"))
        {
            //Diventa figlio della piattaforma
            other.transform.SetParent(gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Se il giocatore è uscito...
        if (other.gameObject.CompareTag("Player"))
        {
            //Lo toglie dalla piattaforma
            other.transform.SetParent(null);
        }
    }

    #endregion



    #region EXTRA - Gizmo

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f);    //Colore -> Arancione

        //Traccia una linea che connette tutte le
        //posizioni nell'array, dal primo all'ultimo
        for (int t = 0; t < positions.Length - 1; t++)
        {
            Vector3 thisPos = positions[t].position,
                    nextPos = positions[t + 1].position;

            Gizmos.DrawLine(thisPos, nextPos);
        }
    }

    #endregion
}
