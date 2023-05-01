using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemUser))]
public class ItemUserDelay : MonoBehaviour
{
    public event System.Action<float> OnDelay;

    public bool CantUseItem { get; private set; }
    [SerializeField] Inputs controller;
    [SerializeField] ItemUser user;
    bool needInputReleased;
    bool timeUp;

    private void Start()
    {
        controller.UseItemInputReleased += UseInputReleased;
        user.Used += Wait;
    }

    IEnumerator ItemUseTimer()
    {
        OnDelay?.Invoke(useTime);
        yield return new WaitForSeconds(useTime);

        timeUp = true;

        if (needInputReleased == false)
            CantUseItem = false;
    }

    public void UseInputReleased()
    {
        needInputReleased = false;

        if (timeUp == true)
            CantUseItem = false;
    }

    float useTime;

    void Wait(float time, bool autoUse)
    {
        useTime = time;
        timeUp = false;
        CantUseItem = true;

        needInputReleased = !autoUse;

        StartCoroutine(ItemUseTimer());
    }

    public void Respawn()
    {
        StopAllCoroutines();
        CantUseItem = false;
    }
}
