using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBehavior : MonoBehaviour
{
    public event System.Action<float> OnAutoReloading;
    public event System.Action AutoReloadComplete;
    public event System.Action StartedAutoReload;
    public event System.Action Reloaded;
    public event System.Action Shot;

    [SerializeField] ItemHolder itemHolder;

    public bool AutoReloading { get; private set; }
    public bool Reloading { get; private set; }
    GunStack gunStack;

    private void Awake()
    {
        itemHolder.SwitchedItem += SwitchedItem;
    }

    void SwitchedItem(ItemStack gun)
    {
        gunStack = gun as GunStack;

        if (gunStack == null) return;

        Reloading = false;

        if (AutoReloading)
        {
            AutoReloadComplete?.Invoke();
            AutoReloading = false;
        }

        // Update UI for this gun. 
        // Should really do this with a separate event (what if something else is subbed to this?)
        Shot?.Invoke();

        gunStack.Shot += Shot;
        gunStack.ShotsGone += AutoReload;
        interval = gunStack.GunType.ReloadTime / gunStack.MaxShots;
    }

    public void ToggleReload()
    {
        if (AutoReloading) return;

        if (Reloading == false)
            StartReloading();
        else
            StopReloading();
    }

    public void FullReload()
    {
        gunStack.FullReload();
        Reloading = false;
        AutoReloading = false;
    }

    void AutoReload()
    {
        StartReloading();
        AutoReloading = true;
        StartedAutoReload?.Invoke();
    }

    public void StartReloading()
    {
        if (gunStack.FullyReloaded) return;
        Reloading = true;
    }

    public void StopReloading()
    {
        if (AutoReloading) return;
        Reloading = false;
    }

    float interval;
    float timer;
    void Update()
    {
        if (Reloading)
        {
            timer += Time.deltaTime;

            if (AutoReloading)
                OnAutoReloading?.Invoke(timer);

            if (timer > interval)
            {
                gunStack.Reload();
                timer = 0;

                if (AutoReloading == false)
                    Reloaded?.Invoke();

                if (gunStack.FullyReloaded)
                {
                    if (AutoReloading)
                        AutoReloadComplete?.Invoke();

                    Reloading = false;
                    AutoReloading = false;
                    return;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (gunStack == null) return;

        gunStack.ShotsGone -= StartReloading;
        gunStack.Shot -= Shot;
    }
}
