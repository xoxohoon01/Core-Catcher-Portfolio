using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundIndicator : MonoBehaviour
{
    public MeshRenderer mesh;
    private float decay;
    private float lifeTime;

    private Material material;

    private static readonly int FillAmountProp = Shader.PropertyToID("_FillAmount");
    private static readonly int RadiusProp = Shader.PropertyToID("_Radius");
    private static readonly int InnerRadiusProp = Shader.PropertyToID("_InnerRadius");
    private static readonly int AngleProp = Shader.PropertyToID("_Angle");

    private void Awake()
    {
        if (mesh != null && mesh.sharedMaterial != null)
        {
            material = new Material(mesh.sharedMaterial);
            mesh.material = material;
        }
    }

    public void Initialize(Vector3 size, float radius, float innerRadius, float angle, float time)
    {
        decay = 0;
        lifeTime = time;
        transform.localScale = size;

        if (material != null)
        {
            material.SetFloat(RadiusProp, radius);
            material.SetFloat(InnerRadiusProp, innerRadius);
            material.SetFloat(AngleProp, angle);
            material.SetFloat(FillAmountProp, 0f);
        }
    }

    private void PrepareForDespawn()
    {
        decay = 0f;
        if (material != null)
        {
            material.SetFloat(FillAmountProp, 0f);
        }
    }

    private void Update()
    {
        if (lifeTime <= 0)
            return;

        decay += Time.deltaTime;

        material.SetFloat(FillAmountProp, decay / lifeTime);
        if (decay >= lifeTime)
        {
            PrepareForDespawn();
            ObjectPoolManager.Instance.Despawn(gameObject);
        }
    }
}
