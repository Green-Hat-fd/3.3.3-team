using TMPro;
using UnityEngine;

public class PressioneSollevamentoPorta : MonoBehaviour
{
    [SerializeField] private Transform key;
    [SerializeField] private Rigidbody rbPorta;
    [SerializeField] private float movimentoXItem = 5f;

    private int itemsInTrigger = 0; //servira' per dare un effetto sonoro quando la porta sara' abbastanza alta
    [SerializeField] private float numeroNecessario;
    //[SerializeField] private AudioSource suonoOk;
    [SerializeField] private TextMeshPro richiesta;

    private void Update()
    {
       richiesta.text = "Oggetti Necessari: " + numeroNecessario.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemsInTrigger++;
            if (itemsInTrigger >= numeroNecessario)
            {
                rbPorta.isKinematic = false;
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
        key.Translate(Vector3.left * movimentoXItem);
    }

    private void AbbassaPorta()
    {
        key.Translate(Vector3.right * movimentoXItem);
    }
}
