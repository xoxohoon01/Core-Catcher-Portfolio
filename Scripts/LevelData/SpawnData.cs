using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public string monsterName;
    public float spawnTime;
    public int count;
    public float radius;
    public bool isSpawned;

    public SpawnData(string monsterName, float spawnTime, int count, float radius)
    {
        this.monsterName = monsterName;
        this.spawnTime = spawnTime;
        this.count = count;
        this.radius = radius;
        isSpawned = false;
    }

    public SpawnData(SpawnData data)
    {
        this.monsterName = data.monsterName;
        this.spawnTime = data.spawnTime;
        this.count = data.count;
        this.radius = data.radius;
        isSpawned = false;
    }

    public SpawnData(WaveData data)
    {
        this.monsterName = data.monsterName;
        this.spawnTime = data.duringTime;
        this.count = data.count;
        this.radius = data.radius;
        isSpawned = false;
    }
}
