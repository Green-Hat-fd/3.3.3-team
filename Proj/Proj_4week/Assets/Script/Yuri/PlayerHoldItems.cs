using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldItems : MonoBehaviour
{
    public PlayerStatsSO_Script stats_SO;
    public PlayerAttack attackScr;
    public PlayerMovevent moveventScr;

    public float pickupRange = 3f;
    public float heldItemHeight = 1f;
    public float placementSpeed = 10f;
    public LayerMask itemLayerMask,
                     powerUpLayerMask,
                     collectableLayerMask;
    public GameObject itemContainer;
    private GameObject heldItem;
    private bool isHoldingItem = false;
    private float pickupCooldown = 0.5f;

    private void Update()
    {
        if (isHoldingItem)
        {
            if (GameManager.inst.inputManager.Giocatore.Lingua.WasPressedThisFrame())
            {
                PlaceItemForward();
            }
        }
        else
        {
            if (pickupCooldown <= 0f && GameManager.inst.inputManager.Giocatore.Lingua.WasPressedThisFrame())
            {
                if (heldItem != null)
                {
                    ReleaseItem(); // Rimuovi l'oggetto solo se lo tieni già in mano.
                }
                else
                {
                    PickUpItem();
                }
                pickupCooldown = 0.5f;
            }
            else
            {
                pickupCooldown -= Time.deltaTime;
            }
        }
    }

    private void PickUpItem()
    {
        RaycastHit itemHit, powerUpHit, collectableHit;
        Debug.DrawRay(transform.position, transform.forward * pickupRange, Color.green, 1.0f);

        //Power up
        if (Physics.Raycast(transform.position, transform.forward, out powerUpHit, pickupRange, powerUpLayerMask))
        {
            PowerUp powUpScr = powerUpHit.transform.GetComponent<PowerUp>();
            PowerUp.PowUpType_Enum type = powUpScr.GetPowerUpType();
            int score = powUpScr.GetScoreWhenCollected();

            switch (type)
            {
                case PowerUp.PowUpType_Enum.FireShoot:
                    attackScr.SetCanShoot(true);
                    break;
                
                case PowerUp.PowUpType_Enum.AmplifiedJump:
                    moveventScr.DoubleMaxJumpCharge();
                    break;
            }

            stats_SO.AddScore(score);   //aggiunge il punteggio
            powerUpHit.transform.gameObject.SetActive(false);
        }

        //Collezionabile
        if (Physics.Raycast(transform.position, transform.forward, out collectableHit, pickupRange, collectableLayerMask))
        {
            CollectableScript collScr = collectableHit.transform.GetComponent<CollectableScript>();
            int i = collScr.GetCollectableIndex();
            int score = collScr.GetScoreWhenCollected();

            stats_SO.SetButterflyCollected(i, true);   //aggiunge il collezionabile come raccolto
            stats_SO.AddScore(score);   //aggiunge il punteggio
            collectableHit.transform.gameObject.SetActive(false);   //Nasconde il collezionabile
        }

        //Oggetto
        if (Physics.Raycast(transform.position, transform.forward, out itemHit, pickupRange, itemLayerMask))
        {
            heldItem = itemHit.collider.gameObject;
            heldItem.GetComponent<Rigidbody>().isKinematic = true;
            heldItem.GetComponent<Collider>().enabled = false;
            heldItem.transform.SetParent(itemContainer.transform);
            heldItem.SetActive(false);
            heldItem.transform.localPosition = new Vector3(0f, heldItemHeight, 0f);
            heldItem.transform.localRotation = Quaternion.identity;
            isHoldingItem = true;
        }
    }

    private void PlaceItemForward()
    {
        if (heldItem != null)
        {
            if (heldItem.CompareTag("Item"))
            {
                heldItem.SetActive(true);
                heldItem.GetComponent<Collider>().enabled = true;
                Rigidbody itemRigidbody = heldItem.GetComponent<Rigidbody>();
                itemRigidbody.isKinematic = false;
                itemRigidbody.velocity = transform.forward * placementSpeed;
                isHoldingItem = false;
                ReleaseItem();
            }
        }
    }

    private void ReleaseItem()
    {
        heldItem.transform.SetParent(null, transform);
        isHoldingItem = false;
        heldItem = null;
    }
}
