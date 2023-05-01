using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    [SerializeField] SmartAIController[] allyPrefabs;
    [SerializeField] float gravity;

    float yVelocity;
    float targetY;
    bool done;

    public void DropOn(Vector2 position)
    {
        transform.position = new Vector3(position.x, Camera.main.GetComponent<CameraFollow>().CameraSizeInUnits.y + position.y);
        targetY = position.y;
    }

    void Update()
    {
        if (done) return;

        if (transform.position.y <= targetY)
        {
            StartCoroutine(DelayDestroy());
            done = true;
            return;
        }

        yVelocity += gravity * Time.deltaTime;
        transform.Translate(new Vector3(0, yVelocity));
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(.25f);

        Instantiate(allyPrefabs[Random.Range(0, allyPrefabs.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
