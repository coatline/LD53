using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemHolder))]
public class ItemUser : MonoBehaviour
{
    public event System.Action<float, bool> Used;

    [SerializeField] MuzzleFlashAnimation muzzleFlashAnimation;
    [SerializeField] ReloadBehavior reloadBehavior;
    [SerializeField] ItemUserDelay itemUserDelay;
    [SerializeField] AudioSource itemAudioSource;
    [SerializeField] SpriteRenderer muzzleFlash;
    [SerializeField] SpriteRenderer itemSprite;
    //[SerializeField] PlayerInputs playerInputs;
    [SerializeField] Transform handTransform;
    [SerializeField] RecoilAnimation recoil;
    [SerializeField] ItemHolder itemHolder;
    [SerializeField] Damageable damageable;
    [SerializeField] Collider2D[] hitBoxes;
    public Collider2D[] HitBoxes { get { return hitBoxes; } }
    public bool Bursting { get; private set; }
    public bool UseItemLock { get; set; }

    public void TryUseItem()
    {
        if (UseItemLock || itemUserDelay.CantUseItem || itemHolder.Item == null) return;

        Item item = itemHolder.Item;
        float useTime = item.UseRate;

        switch (item.Type)
        {
            case ItemType.Gun:

                GunStack gunStack = itemHolder.ItemStack as GunStack;
                Gun gun = gunStack.GunType;

                if (gunStack.ShotsRemaining == 0 || reloadBehavior.AutoReloading)
                    return;

                if (reloadBehavior.Reloading)
                    reloadBehavior.StopReloading();

                if (gun.Burst)
                    StartCoroutine(Burst(gunStack));
                else
                    Fire(gunStack);

                break;
        }

        Used?.Invoke(useTime, item.AutoUse);
    }

    void Fire(GunStack gunStack)
    {
        for (int i = 0; i < gunStack.GunType.AttackCount; i++)
            if (gunStack.TryShoot())
                Shoot(i, gunStack.GunType);
            else
                break;

        recoil.Recoil();
    }

    void Shoot(int bulletIndex, Gun gun)
    {
        float randRot = 0;
        float xOffset = 0;

        if (!gun.ParellelBullets)
        {
            float spread = (((float)gun.AttackCount * (float)gun.AttackSpacing) / 2f);
            float weaponSpreadVal = gun.Spread;

            randRot = -(spread) + Random.Range(-weaponSpreadVal, weaponSpreadVal) + ((float)bulletIndex * gun.AttackSpacing);
        }
        else
        {
            xOffset = -((gun.AttackCount * gun.AttackSpacing) / 2) + (bulletIndex * gun.AttackSpacing);
        }

        CreateProjectile(randRot, xOffset, gun);
    }

    IEnumerator Burst(GunStack gunStack)
    {
        Gun gun = gunStack.GunType;

        Bursting = true;

        float burstTime = gun.TimeBetweenAttacks;
        int bursts = gun.AttacksPerBurst;

        for (int i = 0; i < bursts; i++)
        {
            // If we change items partway through the burst then stop bursting
            if (itemHolder.Item == null || gun != itemHolder.Item || gunStack.ShotsRemaining == 0 || reloadBehavior.Reloading) { break; }

            Fire(gunStack);

            // Do not wait again if this is the last bullet
            if (i < bursts - 1 && gunStack.ShotsRemaining > 0)
                yield return new WaitForSeconds(burstTime);
        }

        Bursting = false;
    }

    Projectile CreateProjectile(float randRot, float xOffset, Gun gun)
    {
        var bulletHole = recoil.GetOffsetFromHand(new Vector2(gun.AttackOffset.x, gun.AttackOffset.y));

        if (gun.MuzzleFlash.DoFlash)
        {
            muzzleFlashAnimation.Flash(gun.MuzzleFlash.MuzzleFlashSpeed, gun.MuzzleFlash.MuzzleFlashSize, gun.MuzzleFlash.MuzzleFlashColor);
            muzzleFlash.transform.position = bulletHole + itemSprite.transform.position;
        }

        Projectile newProjectile = Instantiate(gun.ProjectilePrefab, itemSprite.transform.position, Quaternion.Euler(handTransform.eulerAngles - new Vector3(0, 0, 90 + randRot)));

        newProjectile.transform.localPosition += new Vector3(bulletHole.x, bulletHole.y);
        // Z value is strange.
        newProjectile.transform.Translate(newProjectile.transform.right * xOffset, Space.World);

        if (gameObject.tag == "Ally")
            newProjectile.ChangeToAllyBullet();

        newProjectile.Setup(newProjectile.transform.up, gun, HitBoxes);

        return newProjectile;
    }
}
