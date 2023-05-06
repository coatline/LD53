using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiController : Inputs
{
    [SerializeField] float mouseReleaseDelayRange;
    [SerializeField] protected TopDownMover mover;
    [SerializeField] protected ItemHolder itemHolder;
    [SerializeField] float findClosestEnemyDelay;
    [SerializeField] float shootRangeMultiplier;
    [SerializeField] bool dontPickupWeapons;
    [SerializeField] float detectionRadius;
    [SerializeField] ItemGrabber grabber;
    [SerializeField] ItemUserDelay delay;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] ItemUser itemUser;
    [SerializeField] string enemyTag;
    [SerializeField] bool armed;

    [SerializeField] float aim;
    List<Transform> enemies;

    protected Rigidbody2D targetRb;
    Transform target;
    protected Transform Target
    {
        get => target;
        set
        {
            target = value;

            if (value != null)
                targetRb = value.GetComponent<Rigidbody2D>();
        }
    }

    protected virtual void Start()
    {
        enemies = new List<Transform>();

        if (armed)
            if (itemHolder.Item == null)
                itemHolder.ChangeItem(new GunStack(DataLibrary.I.Guns.GetRandom(), 1));

        delay.OnDelay += DoReleaseMouse;
        grabber.OverItem += PickupItem;

        StartCoroutine(DoScan(Random.Range(3, 8)));
        StartCoroutine(PeriodicallyChooseTarget());
    }

    bool cantPickupItem;

    void PickupItem(LooseItem loo)
    {
        if (cantPickupItem) return;

        StartCoroutine(PickupItemDelay());
        grabber.TryPickupItem();
    }

    IEnumerator PickupItemDelay()
    {
        cantPickupItem = true;
        yield return new WaitForSeconds(1.5f);
        cantPickupItem = false;
    }

    protected virtual void Aim()
    {
        itemHolder.Aim(Target.position, Vector2.zero);
    }

    GameObject seeing;

    protected void Attack()
    {
        Aim();

        if (itemHolder.Item != null)
        {


            if (CanSeeTarget())
            {
                //canSee = true;
                itemUser.TryUseItem();
            }
            else
            {
                //int layerMask = (1 << Target.gameObject.layer);

                //float distance = (itemHolder.Item as Gun).Range * 1.5f;
                //Vector2 direction = (Target.position - transform.position).normalized;

                //RaycastHit2D hit = Physics2D.Raycast(itemUser.BulletHole(), direction, distance, (layerMask | LayerMask.GetMask("Default")));
                //if (this as SmartAIController != null)
                //{
                //    if (hit.collider)
                //    {
                //        print(hit.collider.gameObject.name);
                //    }
                //    else
                //    {
                //        print("Dfd");
                //    }
                //}

            }
        }


        NavigateTarget();
    }

    protected virtual void NavigateTarget()
    {
        mover.RawInput(Target.position - transform.position);
    }

    protected virtual void SawCollision(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            enemies.Add(collision.transform);
            TryChooseTarget();
        }
    }

    //protected virtual void LeftCollision(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag(enemyTag))
    //    {
    //        if (enemies.Contains(collision.transform))
    //        {
    //            enemies.Remove(collision.transform);
    //            TryChooseTarget();
    //        }
    //    }
    //}

    void DoReleaseMouse(float delay)
    {
        if (itemHolder.Item.AutoUse) return;

        StartCoroutine(Release(delay));
    }

    IEnumerator Release(float delay)
    {
        yield return new WaitForSeconds(delay + Random.Range(-mouseReleaseDelayRange, mouseReleaseDelayRange));
        ReleasedUseItemInput();
    }

    bool CanSeeTarget()
    {
        int layerMask = (1 << Target.gameObject.layer);

        float distance = (itemHolder.Item as Gun).Range * shootRangeMultiplier;
        Vector2 direction = (Target.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(itemHolder.ItemSpritePosition, direction, distance, (layerMask | LayerMask.GetMask("Default")));

        if (hit.collider)
            seeing = hit.collider.gameObject;
        else
            seeing = null;

        if (hit.collider == null) return false;

        return hit.collider.CompareTag(Target.gameObject.tag);
    }

    //private void OnDrawGizmos()
    //{
    //    if (itemHolder.Item == null || Target == null) return;
    //    print($"{itemHolder.ItemSpritePosition} + {((Target.position - transform.position).normalized * (itemHolder.Item as Gun).Range)}");

    //    Gizmos.DrawLine(itemHolder.ItemSpritePosition, new Vector3(itemHolder.ItemSpritePosition.x, itemHolder.ItemSpritePosition.y) + ((Target.position - transform.position).normalized * (itemHolder.Item as Gun).Range));
    //}

    protected void TryChooseTarget()
    {
        float closestDist = Mathf.Infinity;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            float dist = Vector2.Distance(transform.position, enemies[i].position);
            if (closestDist > dist)
            {
                closestDist = dist;
                Target = enemies[i];
            }
        }
    }

    IEnumerator DoScan(int numDelay)
    {
        while (true)
        {
            // Spread out the time
            for (int i = 0; i < numDelay; i++)
                yield return new WaitForEndOfFrame();

            Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, detectionRadius, ~(enemyLayer >> 1));

            for (int i = 0; i < col.Length; i++)
                if (enemies.Contains(col[i].transform) == false)
                    SawCollision(col[i]);
        }
    }

    IEnumerator PeriodicallyChooseTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(findClosestEnemyDelay);
            TryChooseTarget();
        }
    }
}
