using TMPro;
using UnityEngine;

public class PressioneSollevamentoPorta : MonoBehaviour
{
    [SerializeField] private Transform porta;
    [SerializeField] private float movimentoXItem = 5f;

    private int itemsInTrigger = 0; //servira' per dare un effetto sonoro quando la porta sara' abbastanza alta
    [SerializeField] private float numeroNecessario;
    //[SerializeField] private AudioSource suonoOk;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemsInTrigger++;
            if (itemsInTrigger >= numeroNecessario)
            {
                SollevaPorta();
                //suonoOk.Play();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemsInTrigger--;
            AbbassaPorta();
        }
    }

    private void SollevaPorta()
    {
        porta.Translate(Vector3.up * movimentoXItem);
    }

    private void AbbassaPorta()
    {
        porta.Translate(Vector3.down * movimentoXItem);
    }
}
