using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public List<WaveMonster> monsters;
    public bool isSpawned;

    public SpawnData(float spawnTime, List<WaveMonster> monsters)
    {
        this.spawnTime = spawnTime;
        this.monsters = monsters;
        isSpawned = false;
    }

    public SpawnData(SpawnData data)
    {
        spawnTime = data.spawnTime;
        monsters = new List<WaveMonster>(data.monsters);
        isSpawned = false;
    }
}