using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBarVisuals : BarVisuals
{
    [SerializeField] Color autoReloadingColor;
    [SerializeField] ReloadBehavior behavior;
    [SerializeField] ItemHolder itemHolder;
    GunStack gunStack;

    protected override void Awake()
    {
        base.Awake();

        itemHolder.SwitchedItem += SwitchedItem;

        behavior.Shot += Shot;
        behavior.Reloaded += OnOneNormalReload;
        behavior.OnAutoReloading += OnAutoReloading;
        behavior.StartedAutoReload += OnStartedAutoReload;
        behavior.AutoReloadComplete += OnAutoReloadComplete;
    }

    void OnOneNormalReload()
    {
        bar.UpdateFillAndFlash(gunStack.ShotsRemaining, gunStack.MaxShots);
    }

    void OnStartedAutoReload()
    {
        bar.SetColor(autoReloadingColor);
    }

    void OnAutoReloadComplete()
    {
        bar.SetDefaultColor();
    }

    void OnAutoReloading(float timer)
    {
        bar.UpdateFill((timer + gunStack.ShotsRemaining), gunStack.MaxShots);
    }

    void Shot()
    {
        bar.UpdateFillAndFlash(gunStack.ShotsRemaining, gunStack.MaxShots);
    }

    void SwitchedItem(ItemStack gun) => gunStack = gun as GunStack;
}
