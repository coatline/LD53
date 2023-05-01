using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarAnimation : MonoBehaviour
{
    [SerializeField] AnimationCurve alphaCurve;
    [SerializeField] Image flash;
    [SerializeField] Image fill;
    [SerializeField] float time;
    Color defaultColor;
    float x;

    private void Awake()
    {
        defaultColor = fill.color;
    }

    private void Update()
    {
        if (x < time)
        {
            x += Time.deltaTime;
            flash.color = Color.white - new Color(0, 0, 0, alphaCurve.Evaluate(Mathf.Clamp01(x) / time));
        }
    }

    public void SetDefaultColor() => fill.color = defaultColor;
    public void SetColor(Color color) => fill.color = color;

    public void Flash()
    {
        x = 0;
        flash.enabled = true;
    }

    public void UpdateFill(float val, float max) => fill.fillAmount = (float)val / max;
    public void UpdateFillAndFlash(float val, float max)
    {
        UpdateFill(val, max);
        Flash();
    }
}
