using UnityEngine;

public class Weapon : Item
{
    [Header("Projectile Properties")]
    [SerializeField] Vector2 attackLinearDrag;
    [SerializeField] float attackGravity;
    [SerializeField] float attackForce;
    [SerializeField] float attackCount;

    [Header("Fade Out")]
    [SerializeField] bool fadeOut;
    [SerializeField] float fadeOutMagnitude;
    [SerializeField] float maxLifeTime;

    [Header("Attack Properties")]
    [SerializeField] float spread;
    [SerializeField] float damage;
    [SerializeField] float knockBack;
    [Tooltip("If left at 0, will use calculated range not including gravity or linear drag.")]
    [SerializeField] float customRange;

    [Header("Burst")]
    [SerializeField] bool burst;
    [SerializeField] int attacksPerBurst;
    [SerializeField] float timeBetweenAttacks;

    [Header("Multi-Shot")]
    [SerializeField] bool parellelBullets;
    [SerializeField] float attackSpacing = .3f;

    public float Range
    {
        get
        {
            if (customRange != 0)
                return customRange;
            else
                return attackForce * maxLifeTime;
        }
    }

    public float Damage => damage;
    public float AttackForce => attackForce;
    public float AttackGravity => attackGravity;
    public float AttackCount => attackCount;
    public Vector2 AttackLinearDrag => attackLinearDrag;
    public float Knockback => knockBack;
    public bool FadeOut => fadeOut;
    public float FadeOutMagnitude => fadeOutMagnitude;
    public float MaxLifeTime => maxLifeTime;

    public float Spread => spread;
    public bool Burst => burst;
    public int AttacksPerBurst => attacksPerBurst;
    public float TimeBetweenAttacks => timeBetweenAttacks;
    public bool ParellelBullets => parellelBullets;
    public float AttackSpacing => attackSpacing;

}
