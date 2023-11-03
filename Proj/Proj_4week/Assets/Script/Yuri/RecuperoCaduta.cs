using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecuperoCaduta : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        // Salva la posizione e la rotazione iniziale dell'oggetto
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        // Controlla se l'oggetto è caduto al di sotto di una certa altezza
        if (transform.position.y < -12f) // Modifica il valore dell'altezza se necessario
        {
            // Riporta l'oggetto alla posizione e rotazione iniziale
            ResetToInitialPosition();
        }
    }

    private void ResetToInitialPosition()
    {
        // Riporta l'oggetto alla posizione e rotazione iniziale
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Resettare anche la velocità e l'accelerazione se necessario
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
