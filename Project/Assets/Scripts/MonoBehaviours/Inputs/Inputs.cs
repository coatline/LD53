using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inputs : MonoBehaviour
{
    public event System.Action UseItemInputReleased;

    protected void ReleasedUseItemInput() => UseItemInputReleased?.Invoke();
    
    [SerializeField] Damageable damageable;
    [SerializeField] Animator animator;

    private void Awake()
    {
        damageable.Damaged += Damaged;
    }

    void Damaged(float f)
    {
        animator.Play("TakeDamage");
    }
}
