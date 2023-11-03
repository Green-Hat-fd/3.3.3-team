using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentelaAltalena : MonoBehaviour
{
    private Dictionary<Transform, TransformData> originalTransforms = new Dictionary<Transform, TransformData>();

    private class TransformData
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            // Ottenere il Rigidbody e verificare che esista
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // Mantenere i dati di trasformazione relativi prima di impostare il genitore
            originalTransforms[other.transform] = new TransformData
            {
                localPosition = other.transform.localPosition,
                localRotation = other.transform.localRotation
            };

            // Impostare l'oggetto con il tag "Item" come figlio di questo oggetto
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            // Ripristinare il Rigidbody rendendolo non kinematico
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Ripristinare la posizione e rotazione relative prima di interrompere la parentela
            if (originalTransforms.ContainsKey(other.transform))
            {
                TransformData data = originalTransforms[other.transform];
                other.transform.parent = null;
                other.transform.localPosition = data.localPosition;
                other.transform.localRotation = data.localRotation;
                originalTransforms.Remove(other.transform);
            }
        }
    }
}