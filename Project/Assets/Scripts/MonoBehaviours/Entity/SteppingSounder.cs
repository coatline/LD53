using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppingSounder : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Sound steppingSound;
    [SerializeField] Rigidbody2D rb;

    private void Start()
    {
        StartCoroutine(Step());
    }


    void Update()
    {
        if (rb.velocity.magnitude > 0)
            if (stepping)
            {
                audioSource.PlayOneShot(steppingSound.RandomSound);
                stepping = false;
                StartCoroutine(Step());
            }
    }

    IEnumerator Step()
    {
        yield return new WaitForSeconds(Random.Range(.2f, .25f));
        stepping = true;
    }

    bool stepping;
}
