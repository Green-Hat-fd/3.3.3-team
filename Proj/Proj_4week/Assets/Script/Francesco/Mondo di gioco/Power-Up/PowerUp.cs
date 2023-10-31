using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource pickUpSfx;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player-tongue"))
        {
            //


            //Nasconde il power-up
            gameObject.SetActive(false);
        }
    }
}
