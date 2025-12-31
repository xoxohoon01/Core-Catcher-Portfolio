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

        float angleOffset = Random.Range(0f, 360f);

        // count 개수만큼 반복
        for (int i = 0; i < spawnData.count; i++)
        {
            // 0 ~ 360° 까지 균등 분할
            float angle = (i * 360f / spawnData.count) + angleOffset;
            // 라디안 단위로 변환
            float rad = angle * Mathf.Deg2Rad;

            // 2D: x, y / 3D: x, z 좌표 계산
            Vector3 offset = new Vector3(
                Mathf.Cos(rad) * spawnData.radius,
                0f,
                Mathf.Sin(rad) * spawnData.radius
            );

            // 월드상 위치 = 이 스크립트를 붙인 오브젝트 위치 + 오프셋
            Vector3 spawnPos = player.transform.position + offset;

            // 프리팹 생성 (필요시 parent 설정)
            ObjectPoolManager.Instance.Spawn(spawnData.monsterName, spawnPos, Quaternion.identity).GetComponent<MonsterController>().Initialize();

            // 생성된 오브젝트를 이 스크립트 오브젝트의 자식으로 두고 싶다면
            // go.transform.SetParent(transform, true);

            // (선택) 회전을 중심으로 향하게 하려면:
            // go.transform.rotation = Quaternion.LookRotation(offset, Vector3.up);
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
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            return;
        }

        if (!isStop && stageData != null)
        {
            entireTime += Time.deltaTime;
            time += Time.deltaTime;

            foreach(var wave in stageData.waveDatas)
            {
                if (wave != stageData.waveDatas.Last())
                {
                    if (entireTime <= wave.duringTime)
                    {
                        if (time >= wave.period)
                        {
                            foreach(var monster in wave.monsters)
                            {
                                SpawnData newSpawnData = new SpawnData(monster.monsterName, wave.duringTime, monster.count, monster.radius);
                                Spawn(newSpawnData);
                            }
                            time -= wave.period;
                        }
                        break;
                    }
                }
                else
                {
                    if (entireTime < stageData.bossTime && time >= 2)
                    {
                        foreach (var monster in wave.monsters)
                        {
                            SpawnData newSpawnData = new SpawnData(monster.monsterName, wave.duringTime, monster.count, monster.radius);
                            Spawn(newSpawnData);
                        }
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
