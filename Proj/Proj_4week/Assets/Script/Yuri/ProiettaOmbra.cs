using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProiettaOmbra : MonoBehaviour
{
    public GameObject oggettoProiettato; // L'oggetto che vuoi proiettare
    public LayerMask layerTerreno; // Imposta il layer del terreno o delle superfici su cui proiettare l'oggetto

    private void Update()
    {
        // Effettua un raycast verso il basso dalla posizione del giocatore
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerTerreno))
        {
            // Posiziona l'oggetto proiettato sulla superficie colpita
            oggettoProiettato.transform.position = hit.point;
        }
    }
}
