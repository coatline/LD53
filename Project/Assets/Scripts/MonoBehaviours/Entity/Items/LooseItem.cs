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
    public ItemPackage Item { get; private set; }

    public void Setup(ItemPackage itemPackage, bool doForce = true)
    {
        sr.sprite = itemPackage.Item.Sprite;
        Item = itemPackage;

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

    public void Pickup()
    {
        Destroy(gameObject);
    }
}
