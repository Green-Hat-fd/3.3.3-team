using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerAttack : MonoBehaviour
{
    public PlayerAnimationManager animMng;

    [SerializeField] private GameObject attacco, proiettilePrefab, puntoSparo;
    private bool isAttacking = false;
    [SerializeField] private float velocitaProiettile = 700f;
    private float ricarica = 1f;
    public static bool canShoot;

    void Start()
    {
        attacco.SetActive(false);
        canShoot = false;
    }

    void FixedUpdate()
    {
       if (!DialogoScript.dialogueActive)
       {
        /*ricarica = Mathf.Clamp(ricarica, 0, 1);
        ricarica += 0.5f * Time.deltaTime;

        if (ricarica >= 1)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }*/

        if (GameManager.inst.inputManager.Giocatore.MeleeAtk.WasPressedThisFrame() && !isAttacking)
        {
            isAttacking = true;
            attacco.SetActive(true);
            Invoke("DisableAttack", 1.0f);
            animMng.TriggerAttack();
        }

        if (GameManager.inst.inputManager.Giocatore.Sparo.WasPressedThisFrame() && canShoot)
        {
            //ricarica = 0;
            canShoot = false;
            GameObject bullet = Instantiate(proiettilePrefab, puntoSparo.transform.position, puntoSparo.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, velocitaProiettile));
        }
       }
    }

    void DisableAttack()
    {
        attacco.SetActive(false);
        isAttacking = false;
    }

    public void SetCanShoot(bool value)
    {
        canShoot = value;
    }
}
