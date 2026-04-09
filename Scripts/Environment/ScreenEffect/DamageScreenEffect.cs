using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScreenEffect : ScreenEffect
{
    public MeshRenderer background;

    private Material backgroundMaterial;

    private readonly int radiusID = Shader.PropertyToID("_Radius");

    private float backgroundValue = 1f;
    private bool isEffectActive = false;

    private void Start()
    {
        backgroundMaterial = new Material(background.sharedMaterial);

        background.material = backgroundMaterial;

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

        float backgroundTargetValue = Mathf.Lerp(backgroundValue, 2f, t);

        backgroundMaterial.SetFloat(radiusID, backgroundTargetValue);

        if (t >= 1f)
        {
            isEffectActive = false;

            backgroundMaterial.SetFloat(radiusID, 2f);
        }
    }

    public override void BeginEffect(float time)
    {
        span = time;
        decay = 0;
        isEffectActive = true;

        backgroundMaterial.SetFloat(radiusID, backgroundValue);
    }
}
