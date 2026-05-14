using System.Collections.Generic;
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

    [Header("Pools ����")]
    [Tooltip("���⿡ Ǯ�� ���� �����հ� �±�, �ʱ� ũ�⸦ �߰��ϼ���.")]
    public List<Pool> pools;

    // 태그별 오브젝트 큐
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // 풀 확장용 프리팹 딕셔너리 (List 순회 제거)
    private Dictionary<string, GameObject> prefabDictionary;

    // 파티클/트레일 캐시 — 인스턴스 ID 기준으로 저장
    private Dictionary<int, ParticleSystem[]> particleCache;
    private Dictionary<int, TrailRenderer[]> trailCache;

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
            // 풀 소진 시 확장 — 캐시도 함께 등록
            objectToSpawn = Instantiate(prefabDictionary[targetTag]);
            objectToSpawn.name = targetTag;
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            CacheEffects(objectToSpawn);
        }

        int id = objectToSpawn.GetInstanceID();

        // 캐싱된 파티클 초기화
        if (particleCache.TryGetValue(id, out var particles))
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Clear(true);
                particles[i].Play(true);
            }
        }

        // 캐싱된 트레일 초기화
        if (trailCache.TryGetValue(id, out var trails))
        {
            for (int i = 0; i < trails.Length; i++)
                trails[i].Clear();
        }

        return objectToSpawn;
    }

    // 오브젝트를 비활성화하여 풀에 반환
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

        poolDictionary  = new Dictionary<string, Queue<GameObject>>();
        prefabDictionary = new Dictionary<string, GameObject>();
        particleCache   = new Dictionary<int, ParticleSystem[]>();
        trailCache      = new Dictionary<int, TrailRenderer[]>();

        // 풀 초기화 — 각 오브젝트의 파티클/트레일을 미리 캐싱
        foreach (var pool in pools)
        {
            pool.tag = pool.prefab.name;
            prefabDictionary[pool.tag] = pool.prefab;

            var objectQueue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.name = pool.tag;
                obj.SetActive(false);
                CacheEffects(obj);
                objectQueue.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectQueue);
        }
    }

    // 오브젝트의 파티클/트레일을 인스턴스 ID 기준으로 캐싱
    private void CacheEffects(GameObject obj)
    {
        int id = obj.GetInstanceID();
        particleCache[id] = obj.GetComponentsInChildren<ParticleSystem>(true);
        trailCache[id]    = obj.GetComponentsInChildren<TrailRenderer>(true);
    }
}
