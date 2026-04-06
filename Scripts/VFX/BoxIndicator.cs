using UnityEngine;

public class BoxIndicator : MonoBehaviour
{
    public MeshRenderer mesh;
    private float decay;
    private float lifeTime;

    private Material material;

    private static readonly int FillAmountProp = Shader.PropertyToID("_FillAmount");

    private void Awake()
    {
        if (mesh != null && mesh.sharedMaterial != null)
        {
            material = new Material(mesh.sharedMaterial);
            mesh.material = material;
        }
    }

    public void Initialize(Vector3 size, float time)
    {
        decay = 0;
        lifeTime = time;
        transform.localScale = size;

        material = new Material(mesh.sharedMaterial);
        mesh.material = material;

        if (material != null)
        {
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
