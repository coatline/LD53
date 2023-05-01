using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReloadType
{
    flip,
    linear
}

[CreateAssetMenu(fileName = "NEW Gun", menuName = "Gun")]
public class Gun : Weapon
{
    [Header("Gun")]
    [SerializeField] Vector2 attackOffset;
    [SerializeField] Vector2 projectileScale;
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] MuzzleFlash muzzleFlash;

    [Header("Reloading")]
    [SerializeField] int shotsPerClip;
    [SerializeField] float reloadTime;
    [SerializeField] ReloadType reloadAnimation;

    public MuzzleFlash MuzzleFlash => muzzleFlash;
    public Projectile ProjectilePrefab => projectilePrefab;
    public Vector2 ProjectileScale => projectileScale;
    public Vector2 AttackOffset => attackOffset;
    public int ShotsPerClip => shotsPerClip;
    public float ReloadTime => reloadTime;
    public ReloadType ReloadAnimation => reloadAnimation;
}
