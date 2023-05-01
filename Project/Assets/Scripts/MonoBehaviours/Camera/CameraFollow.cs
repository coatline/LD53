using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform followObject;
    public Vector2 CameraSizeInUnits { get; private set; }

    [Range(.01f, 1f)]
    [SerializeField] float speed;

    private void Awake()
    {
        var cam = GetComponent<Camera>();

        CameraSizeInUnits = new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize);
    }

    void FixedUpdate()
    {
        if (!followObject)
        {
            SmartAIController[] ais = FindObjectsOfType<SmartAIController>();

            for (int i = 0; i < ais.Length; i++)
            {
                if (ais[i].gameObject.tag == "Ally")
                {
                    followObject = ais[i].transform;
                    break;
                }
            }

            return;
        }

        Vector3 movement = new Vector3(followObject.position.x - transform.position.x, followObject.position.y - transform.position.y);

        Vector3 newPos = (transform.position + (movement * speed));

        transform.position = newPos;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    public void SetFollow(Transform transform) => followObject = transform;
}
