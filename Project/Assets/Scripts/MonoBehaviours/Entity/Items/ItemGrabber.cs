using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    public event System.Action<LooseItem> OverItem;
    public event System.Action<LooseItem> LeftItem;

    [SerializeField] Collider2D pickupCollider;
    [SerializeField] ItemHolder itemHolder;
    LooseItem overItem;

    IEnumerator BlinkCollider()
    {
        pickupCollider.enabled = false;
        yield return new WaitForEndOfFrame();
        pickupCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup"))
        {
            LooseItem li = collision.GetComponent<LooseItem>();
            overItem = li;
            OverItem?.Invoke(overItem);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup"))
        {
            if (collision.gameObject.GetComponent<LooseItem>() == overItem)
            {
                LeftItem?.Invoke(overItem);
                overItem = null;
                StartCoroutine(BlinkCollider());
            }
        }
    }

    public void TryPickupItem()
    {
        if (overItem == null || overItem.PickedUp) return;

        // If my current item is not the same type as what I am trying to pick up
        if (itemHolder.Item != overItem.Item.Item)
        {
            if (itemHolder.Item == null)
            {
                itemHolder.ChangeItem(new GunStack(overItem.Item.Item, 1));
            }
            else
            {
                // Swap items
                LooseItemSpawner.I.SpawnItem(new ItemPackage(itemHolder.Item, 1), transform.position);
                itemHolder.ChangeItem(new GunStack(overItem.Item.Item, 1));
            }

            overItem.TryPickup();
        }

        overItem = null;

        StartCoroutine(BlinkCollider());
        return;
    }
}
