using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogoScript : MonoBehaviour
{
    [SerializeField] private GameObject dialogoPanel, message;
    [SerializeField] private string nomePG;
    [SerializeField] private Text dialogoTxt, nameTxt;
    [SerializeField] private string[] testi;
    [SerializeField] private float textSpeed = 0.3f;
    private int indice;
    public static bool dialogueActive;

    /*private void Start()
    {
        if (dialogueActive)
        {
            dialogueActive = true;
            dialogoTxt.text = string.Empty;
            Dialogo();
        }
    }*/

    void Update()
    {
        if (dialogueActive && GameManager.inst.inputManager.UI.Submit.WasPressedThisFrame() || GameManager.inst.inputManager.UI.Click.WasPressedThisFrame() || GameManager.inst.inputManager.Giocatore.Interazione.WasPressedThisFrame())
        {
            if (dialogoTxt.text == testi[indice])
            {
                ProssimoDialogo();
            }
            else
            {
                StopAllCoroutines();
                dialogoTxt.text = testi[indice];
                
            }
        }

        if (!dialogueActive)
        {
            dialogoPanel.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            message.SetActive(true);

            if (GameManager.inst.inputManager.Giocatore.Interazione.WasPressedThisFrame() && !dialogueActive)
            {
                dialogoPanel.SetActive(true);
                dialogueActive = true;
                Dialogo();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            message.SetActive(false);
            dialogueActive = false;
            dialogoPanel.SetActive(false);
        }
    }

    public void Dialogo()
    {
        nameTxt.text = nomePG;
        dialogoTxt.text = string.Empty;
        indice = 0;
        StartCoroutine(MostraTesto());
    }

    IEnumerator MostraTesto()
    {
        foreach (char letter in testi[indice].ToCharArray())
        { 
            dialogoTxt.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void ProssimoDialogo()
    { 
        if (indice < testi.Length -1)
        {
            indice++;
            dialogoTxt.text = string.Empty;
            StartCoroutine(MostraTesto());
        }
        else
        {
            dialogueActive = false;
        }
    }
}
