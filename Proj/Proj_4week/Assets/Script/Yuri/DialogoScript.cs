using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DialogoScript : MonoBehaviour
{
    [SerializeField] private GameObject dialogoPanel, message;
    [SerializeField] private string nomePG;
    [SerializeField] private Image iconPlace;
    [SerializeField] private Sprite iconPG; 
    [SerializeField] private Text dialogoTxt, nameTxt;
    [SerializeField] private string[] testi;
    [SerializeField] private float textSpeed = 0.3f;
    private int indice;
    public static bool dialogueActive;
    public Animator animator;
    [SerializeField] private AudioSource suoneria;
    private bool isIn = false;
    private bool arguing = false;
    private float attesa;

    private void Start()
    {
        dialogueActive = false;
        message.SetActive(false);
    }

    void Update()
    {
        if (GameManager.inst.inputManager.UI.Submit.WasPressedThisFrame() || GameManager.inst.inputManager.UI.Click.WasPressedThisFrame() || GameManager.inst.inputManager.Giocatore.Interazione.WasPressedThisFrame())
        {
            if ( arguing)
            {
                StopAllCoroutines();
                ProssimoDialogo();
            }
            
        }

        if (!dialogueActive)
        {
            dialogoPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(!isIn) 
            {
                isIn=true;
                suoneria.Play();
                return;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            message.SetActive(true);

            if (GameManager.inst.inputManager.Giocatore.Interazione.WasPressedThisFrame() && !dialogueActive)
            {
                if (!arguing) 
                {
                    arguing=true;
                    dialogoPanel.SetActive(true);
                    dialogueActive = true;
                    animator.SetBool("dialogoAttivo", true);
                    Dialogo();
                    return; 
                }
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            message.SetActive(false);
            dialogueActive = false;
            animator.SetBool("dialogoAttivo", false);
            dialogoPanel.SetActive(false);
        }
    }

    public void Dialogo()
    {
        iconPlace.sprite = iconPG;
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
            
            animator.SetBool("dialogoAttivo", false);
            dialogueActive = false;
            attesa = Time.time;
            if (attesa >=3) {arguing = false; }
            

        }
    }
}
