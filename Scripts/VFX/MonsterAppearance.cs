using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAppearance : MonoBehaviour
{
    private float currentLifeTime;
    private float lifeTime;

    public void Initialize(float startLifeTime)
    {
        lifeTime = startLifeTime;
        GetComponent<ParticleSystem>().Play();
        transform.GetComponentInChildren<ParticleSystem>().Play();
    }

    private void Update()
    {
        currentLifeTime += Time.deltaTime;

        if (currentLifeTime > lifeTime)
        {
            ObjectPoolManager.Instance.Despawn(gameObject);
            currentLifeTime = 0;
        }
    }
}
