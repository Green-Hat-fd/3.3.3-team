using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldItems : MonoBehaviour
{
    public float pickupRange = 3f;
    public float heldItemHeight = 1f;
    public float placementSpeed = 10f;
    public LayerMask itemLayerMask;
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
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * pickupRange, Color.green, 1.0f);

        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange, itemLayerMask))
        {
            heldItem = hit.collider.gameObject;
            heldItem.GetComponent<Rigidbody>().isKinematic = true;
            heldItem.GetComponent<Collider>().enabled = false;
            heldItem.transform.SetParent(itemContainer.transform);
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
