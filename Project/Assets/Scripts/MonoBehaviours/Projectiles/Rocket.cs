using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rocket : Bullet
{
    //public event System.Action Exploded;

    //[SerializeField] protected RocketProperties properties;
    //public bool BlewUp { get; private set; }
    //bool accelerating;

    //void Start()
    //{
    //    StartCoroutine(DelayMovement());
    //}

    //public void RocketSetup(RocketProperties properties)
    //{
    //    this.properties = properties;
    //}

    //public override void DestroyProjectile(bool fadeOut)
    //{
    //    FindObjectOfType<ExplosionManager>().ExplodeAt(transform.position, damage, player, properties.DestructionRadius);
    //    Exploded?.Invoke();
    //    BlewUp = true;
    //    Destroy(gameObject);
    //}

    //Vector2 acceleration;
    //float time;

    //void FixedUpdate()
    //{
    //    if (accelerating)
    //    {
    //        acceleration = (properties.Acceleration /** Time.fixedDeltaTime*/) * transform.up;
    //        //acceleration *= -Vector2.Dot(rb.velocity, acceleration.normalized) + (properties.StartupSpeed * 4);
    //        //print(acceleration);
    //        //rb.velocity += v;
    //        rb.velocity = ((new Vector2(properties.StartupSpeed, properties.StartupSpeed)) * transform.up);
    //        rb.velocity += acceleration * time;
    //        rb.velocity = Vector2.ClampMagnitude(rb.velocity, properties.MaxSpeed);
    //        //rb.velocity += ((new Vector2(properties.StartupSpeed, properties.StartupSpeed)) * transform.up);
    //        //rb.velocity = Vector2.ClampMagnitude(rb.velocity, properties.StartupSpeed + 5);
    //        //rb.velocity += acceleration;
    //        time += Time.fixedDeltaTime;
    //    }
    //    else
    //    {
    //        rb.velocity = new Vector2(properties.StartupSpeed, properties.StartupSpeed) * transform.up;
    //    }
    //    //print(Vector2.Dot(rb.velocity.normalized, acceleration.normalized));
    //    //if (Vector2.Dot(rb.velocity.normalized, acceleration.normalized) < 1)
    //    //{
    //    //    acceleration *= 2;
    //    //}

    //}

    //IEnumerator DelayMovement()
    //{
    //    yield return new WaitForSeconds(properties.StartupDuration);
    //    accelerating = true;
    //}
}
