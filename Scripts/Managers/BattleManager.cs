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

        foreach (var monster in spawnData.monsters)
        {
            float angleOffset = Random.Range(0f, 360f);

            for (int i = 0; i < monster.count; i++)
            {
                float angle = (i * 360f / monster.count) + angleOffset;
                float rad = angle * Mathf.Deg2Rad;

                Vector3 offset = new Vector3(
                    Mathf.Cos(rad) * monster.radius,
                    0f,
                    Mathf.Sin(rad) * monster.radius
                );

                Vector3 spawnPos = player.transform.position + offset;

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
