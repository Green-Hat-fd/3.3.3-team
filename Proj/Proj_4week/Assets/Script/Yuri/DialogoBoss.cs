using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogoBoss : MonoBehaviour
{
    [SerializeField] private GameObject dialogoPanel;
    [SerializeField] private string nomePG;
    [SerializeField] private Image iconPlace;
    [SerializeField] private Sprite iconPG;
    [SerializeField] private Text dialogoTxt, nameTxt;
    [SerializeField] private string[] testi;
    [SerializeField] private float textSpeed = 0.3f;
    private int indice;
    public static bool dialogueActive;
    public Animator animator;
    [SerializeField] List<MonoBehaviour> scriptToBlock;


    private void Awake()
    {
        
        dialogueActive = true;
        Dialogo();
        animator.SetBool("dialogoAttivo", true);
    }

    void Update()
    {
        if (GameManager.inst.inputManager.UI.Submit.WasPressedThisFrame() || GameManager.inst.inputManager.UI.Click.WasPressedThisFrame() || GameManager.inst.inputManager.Giocatore.Interazione.WasPressedThisFrame())
        {
            StopAllCoroutines();
                ProssimoDialogo();
            
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
            EnableAllScripts(false);
            dialogoTxt.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void ProssimoDialogo()
    {
        if (indice < testi.Length - 1)
        {
            indice++;
            dialogoTxt.text = string.Empty;
            StartCoroutine(MostraTesto());
        }
        else
        {
            animator.SetBool("dialogoAttivo", false);
            EnableAllScripts(true);
            dialogoPanel.SetActive(false);
        }
    }

    public void EnableAllScripts(bool enabled)
    {
        foreach (MonoBehaviour scr in scriptToBlock)
        {
            scr.enabled = enabled;
        }
    }
}