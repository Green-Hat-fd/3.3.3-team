using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaAttivazione : MonoBehaviour
{
    [SerializeField] private Transform key;
    [SerializeField] private float movimentoXItem = 5f;
    //[SerializeField] private bool portaAperta;
    [SerializeField] private Rigidbody rbPorta;
    //[SerializeField] private AudioSource suonoOk;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))//&& !portaAperta)
        {
            //portaAperta = true;
            AttivaPorta();
            //suonoOk.Play();
        }
    }

    private void AttivaPorta()
    {
        rbPorta.isKinematic = false;
        key.Translate(Vector3.left * movimentoXItem);
    }
}
