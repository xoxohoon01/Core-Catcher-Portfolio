using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxIndicator : MonoBehaviour
{
    public float decay;
    public float lifeTime;

    private Material material;

    public void Initialize(Vector3 size, float time)
    {
        transform.localScale = size;

        material = new Material(GetComponent<MeshRenderer>().sharedMaterial);
        GetComponent<MeshRenderer>().material = material;

        material.SetFloat("_FillAmount", 0);

        decay = 0;
        lifeTime = time;
    }

    private void Update()
    {
        decay = Mathf.Min(decay += Time.deltaTime, lifeTime);

        material.SetFloat("_FillAmount", decay / lifeTime);
        if (decay >= lifeTime)
        {
            ObjectPoolManager.Instance.Despawn(gameObject);
        }
    }
}
