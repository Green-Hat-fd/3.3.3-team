using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject attacco, proiettilePrefab, puntoSparo;
    private bool isAttacking = false;
    [SerializeField] private float velocitaProiettile = 700f;
    private float ricarica = 1f;
    private bool canShoot;

    void Start()
    {
        attacco.SetActive(false);
    }

    void Update()
    {
        ricarica = Mathf.Clamp(ricarica, 0, 1);
        ricarica += 0.5f * Time.deltaTime;

        if (ricarica >= 1)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }

        if (GameManager.inst.inputManager.Giocatore.MeleeAtk.WasPressedThisFrame())
        {
            isAttacking = true;
            attacco.SetActive(true);
            Invoke("DisableAttack", 1.0f);
        }

        if (GameManager.inst.inputManager.Giocatore.Sparo.WasPressedThisFrame() && canShoot)
        {
            ricarica = 0;
            canShoot = false;
            GameObject bullet = Instantiate(proiettilePrefab, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, velocitaProiettile, 0));
        }
    }

    void DisableAttack()
    {
        attacco.SetActive(false);
        isAttacking = false;
    }
}
