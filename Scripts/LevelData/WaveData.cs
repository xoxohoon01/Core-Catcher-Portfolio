using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public List<WaveMonster> monsters;
    public float duringTime;
    public float period;
    public bool isSpawned;
}

[System.Serializable]
public struct WaveMonster
{
    public string monsterName;
    public int count;
    public float radius;
}