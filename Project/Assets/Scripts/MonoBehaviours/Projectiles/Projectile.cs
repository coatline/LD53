using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Play shooting sound on projectiles so that fast firing weapons do not cut off the sounds

    [SerializeField] protected ParticleSystem particles;
    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] AudioSource audioSource;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Animator animator;
    [SerializeField] Sound soundOnHit;
    [SerializeField] Collider2D col;
    [SerializeField] int durability;

    [Header("Ally")]
    [SerializeField] Sprite allyBulletSprite;
    [SerializeField] LayerMask allyLayer;


    protected float knockback;
    protected float damage;
    Vector2 linearDrag;
    float minimumVel;
    bool dead;

    public void Setup(Vector3 direction, Weapon weapon, Collider2D[] ignoreColliders = null)
    {
        rb.velocity = direction * weapon.AttackForce;

        rb.gravityScale = weapon.AttackGravity;
        linearDrag = weapon.AttackLinearDrag;
        minimumVel = weapon.FadeOutMagnitude;
        knockback = weapon.Knockback;
        damage = weapon.Damage;

        // So that the physics system does not determine knockback.
        rb.mass = 0.000001f;

        audioSource.PlayOneShot(weapon.SoundOnUse.RandomSound);

        StartCoroutine(LifeTime(weapon.MaxLifeTime));
        IgnoreColliders(ignoreColliders);
    }

    public void ChangeToAllyBullet()
    {
        sr.sprite = allyBulletSprite;
        gameObject.layer = LayerMask.NameToLayer("AllyBullet");
    }

    void IgnoreColliders(Collider2D[] cols)
    {
        if (col != null)
            foreach (Collider2D collider in cols)
                Physics2D.IgnoreCollision(col, collider);
    }

    IEnumerator LifeTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (gameObject != null)
            DestroyProjectile(true);
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude < minimumVel)
        {
            // Play fade out animation
            DestroyProjectile(true);
        }

        //float newX = rb.velocity.x / (linearDrag.x + 1);
        //float difference = Mathf.Abs(rb.velocity.x - newX);
        //rb.velocity = new Vector2(newX, rb.velocity.y - difference);
        rb.velocity = rb.velocity / (linearDrag + Vector2.one);
    }

    public virtual void DestroyProjectile(bool fadeOut)
    {
        if (dead) { return; }
        dead = true;

        if (animator)
        {
            if (fadeOut)
                animator.Play("Fade_Out");
            else
                animator.Play("Bullet_Explode");

            //ads.PlayOneShot(soundOnHit.sound.GetClip());
        }
        else
        {
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

    protected void Hit()
    {
        //if (soundOnHit)
        //    audioSource.PlayOneShot(soundOnHit.sound.GetClip());

        durability--;

        if (durability <= 0)
        {
            // Play explode animation
            DestroyProjectile(false);
        }
    }

    protected void Hit(Damageable d)
    {
        d.TakeDamage(damage);

        Hit();
    }
}