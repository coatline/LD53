using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer itemSprite;
    [SerializeField] SpriteRenderer handSprite;
    [SerializeField] ItemHolder itemHolder;

    Vector3 initialHandPosition;
    bool recovering;

    RecoilSettings currentSettings;
    RecoilSettings defaultSettings;
    RecoilSettings itemSettings;

    private void Awake()
    {
        itemHolder.SwitchedItem += SwitchedItem;
        initialHandPosition = Vector3.zero;
        //initialHandPosition = handSprite.transform.localPosition;
        defaultSettings = new RecoilSettings(.1f, 7, 0);
    }

    void SwitchedItem(ItemStack item)
    {
        if (item != null)
            itemSettings = item.Type.RecoilSettings;
        else
            itemSettings = null;
    }

    private void Update()
    {
        if (recovering)
        {
            var dist = Vector2.Distance(initialHandPosition, handSprite.transform.localPosition);
            if (dist < .005f)
            {
                recovering = false;
            }

            //if (itemSettings != null)
            //    recoverySpeed = itemSettings.RecoverySpeed;

            handSprite.transform.localPosition = Vector3.Lerp(handSprite.transform.localPosition, initialHandPosition, Time.deltaTime * currentSettings.RecoverySpeed);
        }
    }

    public Vector3 GetOffsetFromHand(Vector2 offset)
    {
        float bX = offset.x;
        float bY = offset.y;
        //return offset * itemSprite.transform.up;
        Vector2 bHoleX = new Vector2(bX, bX) * itemSprite.transform.right;
        Vector2 bHoleY = new Vector2(bY, bY) * itemSprite.transform.up;
        return bHoleX + bHoleY;
    }

    IEnumerator DelayRecovery()
    {
        yield return new WaitForSeconds(currentSettings.RecoveryDelay);
        recovering = true;
    }

    public void Recoil(RecoilSettings settings = null, bool useDefault = false)
    {
        if (settings != null)
            currentSettings = settings;
        else if (useDefault || itemSettings == null)
            currentSettings = defaultSettings;
        else
            currentSettings = itemSettings;

        //handSprite.Translate(GetOffsetFromHand(-item.ItemType.Weapon.RecoilAmount), Space.World);
        handSprite.transform.Translate(-transform.right * currentSettings.RecoilAmount, Space.World);

        StopAllCoroutines();

        if (handSprite.gameObject.activeSelf)
            StartCoroutine(DelayRecovery());
    }
}
