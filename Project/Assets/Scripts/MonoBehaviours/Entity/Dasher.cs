using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dasher : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Sound dashSound;
    [SerializeField] ParticleSystem dashParticles;
    [SerializeField] TopDownMover topDownMover;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float minTimeBetweenDashes;
    [SerializeField] float maxDashTime;
    [SerializeField] float minDashTime;
    [SerializeField] float dashSpeed;
    float startedDashTime;
    bool needToReleaseDash;
    bool cantDash;
    bool dashing;

    public void HoldDash()
    {
        if (dashing == false)
        {
            // Have we waited the minimum time between each dash
            if (cantDash == true)
                return;

            // Have we reset our dash input?
            if (needToReleaseDash == true)
                return;

            StartDashing();
        }

        if (Time.time - startedDashTime >= maxDashTime)
            StopDashing();
    }

    void StartDashing()
    {
        dashParticles.Play();
        audioSource.PlayOneShot(dashSound.RandomSound);

        startedDashTime = Time.time;
        needToReleaseDash = true;
        cantDash = true;
        dashing = true;

        topDownMover.SetMaxMovementSpeed(dashSpeed);
        topDownMover.SetMovementToMax();
        topDownMover.MovementLock = true;

        sr.color = Color.white - new Color(0, 0, 0, .5f);
    }

    public void ReleaseDash()
    {
        needToReleaseDash = false;

        // Already maxed out our dash
        if (dashing == false) return;

        float progress = Time.time - startedDashTime;

        if (progress < minDashTime)
            StartCoroutine(StopDashDelay(minDashTime - progress));
        else
            StopDashing();
    }

    void StopDashing()
    {
        dashParticles.Stop();

        sr.color = Color.white;
        topDownMover.MovementLock = false;
        topDownMover.SetMovementSpeedToDefault();

        dashing = false;

        StartCoroutine(DelayBetweenDashes());
    }

    IEnumerator StopDashDelay(float wait)
    {
        yield return new WaitForSeconds(wait);
        StopDashing();
    }

    IEnumerator DelayBetweenDashes()
    {
        yield return new WaitForSeconds(minTimeBetweenDashes);
        cantDash = false;
    }
}
