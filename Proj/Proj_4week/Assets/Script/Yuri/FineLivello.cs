using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FineLivello : MonoBehaviour
{
    [SerializeField] private GameObject endButton, vittoriaUI;
    [SerializeField] private Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(endButton);
            Cursor.lockState = CursorLockMode.None;
            vittoriaUI.SetActive(true);
            animator.SetBool("traguardo", true);
            
        }
    }

    public void ProssimoLivello()
    {
        animator.SetBool("traguardo", false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
