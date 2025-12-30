using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "StageData")]
public class StageData : ScriptableObject
{
    public string stageName;
    public Sprite stageSprite;
    public string stageDisplayName;
    public string stageDescription;
    public int bossTime;

    public List<WaveData> waveDatas;
    public List<SpawnData> spawnDatas;
}
