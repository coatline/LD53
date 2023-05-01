using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] bool bounceOffWall;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if ((collision.gameObject.CompareTag("Enemy")))
        //{
        //    if (playerAttack)
        //    {
        //        collision.gameObject.GetComponent<IDamageable>().Damage(damage, (collision.transform.position - transform.position).normalized * knockback);
        //    }
        //    else
        //    {
        //        Reflect(collision.contacts[0]);
        //        return;
        //    }
        //}
        //else if ((collision.gameObject.CompareTag("Player")))
        //{
        //    if (!playerAttack)
        //    {
        //        collision.gameObject.GetComponent<IDamageable>().Damage(damage, (collision.transform.position - transform.position).normalized * knockback);
        //    }
        //    else
        //    {
        //        Reflect(collision.contacts[0]);
        //        return;
        //    }
        //}
        //else if (bounceOffWall && collision.gameObject.CompareTag("Wall"))
        //{
        //    Reflect(collision.contacts[0]);
        //    return;
        //}

        Damageable d = collision.gameObject.GetComponent<Damageable>();

        if (d)
            Hit(d);
        else
            Hit();
    }

    void Reflect(ContactPoint2D point)
    {
        Vector2 dir = Vector2.Reflect(transform.up, point.normal);
        transform.rotation = Quaternion.Euler(0, 0, Extensions.AngleFromPosition(transform.position, transform.position + new Vector3(dir.x, dir.y, 0)));

        if (particles)
            particles.Emit(1);
    }
}
