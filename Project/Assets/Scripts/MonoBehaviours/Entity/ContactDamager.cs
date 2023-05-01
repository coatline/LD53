using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamager : MonoBehaviour
{
    Dictionary<Damageable, Coroutine> damageables;
    [SerializeField] float damagePerTick;
    [SerializeField] float tickInterval;
    [SerializeField] List<string> tags;

    void Awake()
    {
        damageables = new();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tags.Contains(collision.gameObject.tag) == false) 
            return;

        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        if (damageable != null)
        {
            if (damageables.ContainsKey(damageable) == false)
                damageables.Add(damageable, StartCoroutine(Damage(damageable)));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (tags.Contains(collision.gameObject.tag) == false)
            return;

        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        if (damageable != null)
            if (damageables.ContainsKey(damageable))
            {
                StopCoroutine(damageables[damageable]);
                damageables.Remove(damageable);
            }
    }

    IEnumerator Damage(Damageable d)
    {
        while (true)
        {
            d.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
    }
}
