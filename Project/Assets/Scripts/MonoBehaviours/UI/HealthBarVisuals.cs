using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarVisuals : BarVisuals
{
    protected override void Awake()
    {
        base.Awake();

        damageable.HealthChanged += bar.UpdateFillAndFlash;
    }

    private void OnDestroy()
    {
        damageable.HealthChanged -= bar.UpdateFillAndFlash;
    }
}
