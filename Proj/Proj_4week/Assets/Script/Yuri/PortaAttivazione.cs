using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaAttivazione : MonoBehaviour
{
    [SerializeField] private Transform key;
    [SerializeField] private float movimentoXItem = 5f;
    //[SerializeField] private bool portaAperta;
    [SerializeField] private List<Rigidbody> rbPorte = new List<Rigidbody>();
    //[SerializeField] private AudioSource suonoOk;
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))//&& !portaAperta)
        {
            //portaAperta = true;
            AttivaPorte();
            //suonoOk.Play();
        }
    }

    private void AttivaPorte()
    {
        foreach (Rigidbody rbPorta in rbPorte)
        {
            rbPorta.isKinematic = false;
        }
        if (!activated)
        {
            key.Translate(Vector3.left * movimentoXItem);
            //activated = true;
        }
        
    }
}
