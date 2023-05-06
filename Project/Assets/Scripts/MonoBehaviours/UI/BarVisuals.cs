using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class BarVisuals : MonoBehaviour
{
    [SerializeField] protected Damageable damageable;
    [SerializeField] BarAnimation barPrefab;
    [SerializeField] Transform position;
    protected BarAnimation bar;

    protected virtual void Awake()
    {
        CreateBar();
        damageable.Died += Died;
    }

    void Died() => Destroy(bar.gameObject);

    void CreateBar()
    {
        Canvas[] canvasas = FindObjectsOfType<Canvas>();
        Canvas c = null;

        foreach (Canvas canvas in canvasas)
        {
            if (canvas.name == "WorldSpaceCanvas")
            {
                c = canvas;
                break;
            }
        }

        bar = Instantiate(barPrefab, c.transform);
    }

    protected virtual void Update()
    {
        bar.transform.position = position.position;
    }
}
