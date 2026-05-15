using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoSingleton<BattleManager>
{
    public bool isStop;
    public float entireTime;
    public float time;

    public List<GameObject> characters;

    public StageData stageData;
    private List<SpawnData> spawnDatas = new List<SpawnData>();

    [ContextMenu("Spawn")]
    public void Spawn(SpawnData spawnData)
    {
        PlayerController player = PlayerManager.Instance.GetPlayer();

        int typeCount = spawnData.monsters.Count;

        // 몬스터 종류마다 원형 위에서 균등하게 방향을 배분
        float baseAngle = Random.Range(0f, 360f);

        for (int typeIndex = 0; typeIndex < typeCount; typeIndex++)
        {
            var monster = spawnData.monsters[typeIndex];

            // 종류별 군집 중심 방향 — 전체를 typeCount로 나눠 고르게 배치
            float clusterAngle = baseAngle + (typeIndex * 360f / typeCount);
            float clusterRad = clusterAngle * Mathf.Deg2Rad;

            // 군집 중심점
            Vector3 clusterCenter = player.transform.position + new Vector3(
                Mathf.Cos(clusterRad) * monster.radius,
                0f,
                Mathf.Sin(clusterRad) * monster.radius
            );

            // 군집 내 개별 스폰 — 중심점 주변 원형 범위에 랜덤 배치
            float clusterSpread = 3f;

            for (int i = 0; i < monster.count; i++)
            {
                Vector2 rand = Random.insideUnitCircle * clusterSpread;
                Vector3 spawnPos = clusterCenter + new Vector3(rand.x, 0f, rand.y);

                ObjectPoolManager.Instance
                    .Spawn(monster.monsterName, spawnPos, Quaternion.identity)
                    .GetComponent<MonsterController>()
                    .Initialize((int)(entireTime / 100) + 1);
            }
        }
    }

    public void Initialize(StageData targetStageData)
    {
        entireTime = 0;
        time = 0;
        stageData = targetStageData;
        for (int i = 0; i < stageData.spawnDatas.Count; i++)
        {
            spawnDatas.Add(new SpawnData(stageData.spawnDatas[i]));
        }

        foreach (GameObject character in characters)
        {
            character.SetActive(false);

            if (character.name == GameManager.Instance.characterName)
                character.SetActive(true);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainScene" && SceneManager.GetActiveScene().name != "MainLabScene")
        {
            return;
        }

        if (!isStop && stageData != null)
        {
            entireTime += Time.deltaTime;
            time += Time.deltaTime;

            foreach (var wave in stageData.waveDatas)
            {
                if (wave != stageData.waveDatas.Last())
                {
                    if (entireTime <= wave.duringTime)
                    {
                        if (time >= wave.period)
                        {
                            SpawnData newSpawn = new SpawnData(wave.duringTime, wave.monsters);
                            Spawn(newSpawn);

                            time -= wave.period;
                        }
                        break;
                    }
                }
                else
                {
                    if (entireTime < stageData.bossTime && time >= 2)
                    {
                        SpawnData newSpawn = new SpawnData(wave.duringTime, wave.monsters);
                        Spawn(newSpawn);

                        time -= 2;
                    }
                }
            }

            foreach (SpawnData spawnData in spawnDatas)
            {
                if (!spawnData.isSpawned)
                {
                    if (entireTime > spawnData.spawnTime)
                    {
                        Spawn(spawnData);
                        spawnData.isSpawned = true;
                    }
                }
            }
        }
    }
}
