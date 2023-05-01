using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlash : MonoBehaviour
{
    [Tooltip("GUI/Text")]
    [SerializeField] Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] float duration = .075f;

    [SerializeField] SpriteRenderer sr;
    [SerializeField] Damageable damageable;

    // The material that was in use, when the script started.
    Material originalMaterial;

    // The currently running coroutine.
    Coroutine flashRoutine;

    void Start()
    {
        // Get the material that the SpriteRenderer uses, 
        // so we can switch back to it after the flash ended.
        originalMaterial = sr.material;
        damageable.Damaged += Damaged;
    }

    void Damaged(float f) => Flash();

    public void Flash()
    {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }

        // Start the Coroutine, and store the reference for it.
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Swap to the flashMaterial.
        sr.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);

        // After the pause, swap back to the original material.
        sr.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }
}
