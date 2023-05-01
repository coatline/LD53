using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : AiController
{
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (Target)
            Attack();
    }

    protected override void NavigateTarget()
    {
        Vector2 direction = Target.position - transform.position;

        mover.RawInput(direction);

        base.NavigateTarget();
    }
}