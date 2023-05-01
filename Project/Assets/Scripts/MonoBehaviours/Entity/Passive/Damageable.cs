using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public event System.Action<float, float> HealthChanged;
    /// <summary>
    /// float: Health remaining
    /// </summary>
    public event System.Action<float> Damaged;
    public event System.Action Died;

    [SerializeField] UnityEvent Healed;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Sound hitSound;

    [SerializeField] float maxHealth;
    public float MaxHealth => maxHealth;

    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            HealthChanged?.Invoke(health, maxHealth);
        }
    }
    float health;

    public bool Dead { get; private set; }

    void Awake()
    {
        Health = maxHealth;
    }

    /// <returns>Dead</returns>
    public bool TakeDamage(float damage)
    {
        if (Dead) return false;

        if (audioSource)
            audioSource.PlayOneShot(hitSound.RandomSound);

        Health -= damage;
        Damaged?.Invoke(health);

        if (health <= 0)
        {
            Kill();
            Died?.Invoke();
            return true;
        }
        else
            return false;
    }

    public void Kill()
    {
        Dead = true;
        Destroy(gameObject);
    }

    public void Heal(float amount)
    {
        Health = Mathf.Min(health + amount, maxHealth);
        print($"Healed: {amount} Health: {Health}");
    }
}
