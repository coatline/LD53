using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStack : ItemStack
{
    public event System.Action ShotsGone;
    public event System.Action Shot;
    int shotsRemaining;
    public int ShotsRemaining
    {
        get => shotsRemaining;
        private set
        {
            if (shotsRemaining == value) return;
            shotsRemaining = value;

            if (value == 0)
                ShotsGone?.Invoke();
        }
    }
    public readonly int MaxShots;

    public Gun GunType => Type as Gun;
    public GunStack(Item item, int count) : base(item, count)
    {
        MaxShots = (item as Gun).ShotsPerClip;
        ShotsRemaining = MaxShots;
    }

    public bool TryShoot()
    {
        if (ShotsRemaining <= 0) return false;

        ShotsRemaining--;
        Shot?.Invoke();
        return true;
    }

    public bool FullyReloaded => shotsRemaining == MaxShots;
    public void Reload() => ShotsRemaining++;
    public void FullReload() => ShotsRemaining = MaxShots;
}
