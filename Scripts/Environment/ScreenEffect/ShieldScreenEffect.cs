using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScreenEffect : ScreenEffect
{
    public MeshRenderer grid;
    public MeshRenderer background;

    private Material gridMaterial;
    private Material backgroundMaterial;

    private readonly int radiusID = Shader.PropertyToID("_Radius");

    private float gridValue = 1f;
    private float backgroundValue = 1.2f;
    private bool isEffectActive = false;

    private void Start()
    {
        gridMaterial = new Material(grid.sharedMaterial);
        backgroundMaterial = new Material(background.sharedMaterial);

        grid.material = gridMaterial;
        background.material = backgroundMaterial;

        gridMaterial.SetFloat(radiusID, 2f);
        backgroundMaterial.SetFloat(radiusID, 2f);
    }

    private void Update()
    {
        if (!isEffectActive) return;

        if (span > 0)
        {
            span -= Time.deltaTime;
            return;
        }

        decay += Time.deltaTime;
        float t = Mathf.Clamp01(decay / targetDecay);

        float gridTargetValue = Mathf.Lerp(gridValue, 2f, t);
        float backgroundTargetValue = Mathf.Lerp(backgroundValue, 2f, t);

        gridMaterial.SetFloat(radiusID, gridTargetValue);
        backgroundMaterial.SetFloat(radiusID, backgroundTargetValue);

        if (t >= 1f)
        {
            isEffectActive = false;

            gridMaterial.SetFloat(radiusID, 2f);
            backgroundMaterial.SetFloat(radiusID, 2f);
        }
    }

    public override void BeginEffect(float time)
    {
        span = time;
        decay = 0;
        isEffectActive = true;

        gridMaterial.SetFloat(radiusID, gridValue);
        backgroundMaterial.SetFloat(radiusID, backgroundValue);
    }
}
