using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseItem : MonoBehaviour
{
    [Header("Spawn Properties")]
    [SerializeField] float maxAngularVelocityOnSpawn;
    [SerializeField] float maxInitialForceOnSpawn;
    [SerializeField] float canPickupDelay;

    [Header("References")]
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Collider2D col;
    [SerializeField] Rigidbody2D rb;
    public ItemStack ItemStack { get; private set; }
    public bool PickedUp => pickedUp;
    bool pickedUp;

    public void Setup(ItemStack itemStack, bool doForce = true)
    {
        sr.sprite = itemStack.Type.Sprite;
        ItemStack = itemStack;

        StartCoroutine(PickupDelay());

        //if (doForce)
        //{
        //    Vector2 force = new Vector2(Random.Range(-maxInitialForceOnSpawn, maxInitialForceOnSpawn), Random.Range(-maxInitialForceOnSpawn, maxInitialForceOnSpawn));
        //    rb.angularVelocity = Random.Range(-90f, 90f);
        //    rb.AddForce(force);
        //}
    }

    IEnumerator PickupDelay()
    {
        yield return new WaitForSeconds(canPickupDelay);
        col.enabled = true;
    }

    public bool TryPickup()
    {
        if (pickedUp) return false;
        pickedUp = true;
        Destroy(gameObject);
        return true;
    }
}
