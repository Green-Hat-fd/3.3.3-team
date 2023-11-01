using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaAttivazione : MonoBehaviour
{
    [SerializeField] private Transform porta;
    [SerializeField] private float movimentoXItem = 5f;
    [SerializeField] private bool portaAperta;
    //[SerializeField] private AudioSource suonoOk;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key")&& !portaAperta)
        {
            portaAperta = true;
            SollevaPorta();
            //suonoOk.Play();
        }
    }

    private void SollevaPorta()
    {
        porta.Translate(Vector3.up * movimentoXItem);
    }
}
