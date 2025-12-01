using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [Header("Pools 설정")]
    [Tooltip("여기에 풀을 만들 프리팹과 태그, 초기 크기를 추가하세요.")]
    public List<Pool> pools;

    // 태그별 오브젝트 큐
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // 태그로부터 오브젝트를 꺼내 활성화해서 반환
    public GameObject Spawn(string targetTag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(targetTag))
        {
            Debug.LogWarning($"ObjectPoolManager: 태그 '{targetTag}'에 해당하는 풀을 찾을 수 없습니다.");
            return null;
        }

        GameObject objectToSpawn;

        if (poolDictionary[targetTag].Count > 0)
        {
            objectToSpawn = poolDictionary[targetTag].Dequeue();
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);
        }
        else
        {
            Pool targetPool = pools.FirstOrDefault(pool => pool.tag == targetTag);
            objectToSpawn = Instantiate(targetPool.prefab);
            objectToSpawn.name = targetTag;
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }

        // ParticleSystem 초기화
        var particles = objectToSpawn.GetComponentsInChildren<ParticleSystem>(true);
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Clear(true);
            particles[i].Play(true);
        }

        // ParticleSystem 초기화
        var trails = objectToSpawn.GetComponentsInChildren<TrailRenderer>(true);
        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].Clear();
        }

        return objectToSpawn;
    }

    // 사용이 끝난 오브젝트를 비활성화하여 풀로 반환
    public void Despawn(GameObject obj)
    {
        if (!poolDictionary.ContainsKey(obj.name))
        {
            Debug.LogWarning($"ObjectPoolManager: 태그 '{obj.name}'에 해당하는 풀을 찾을 수 없습니다. 반환 실패.");
            Destroy(obj);
            return;
        }
        if (obj.transform.parent != null)
            obj.transform.parent = null;
        obj.SetActive(false);
        poolDictionary[obj.name].Enqueue(obj);
    }

    protected override void Awake()
    {
        base.Awake();

        // 풀 초기화
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in pools)
        {
            var objectQueue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                pool.tag = pool.prefab.name;
                GameObject obj = Instantiate(pool.prefab);
                obj.name = pool.tag;
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectQueue);
        }
    }
}
