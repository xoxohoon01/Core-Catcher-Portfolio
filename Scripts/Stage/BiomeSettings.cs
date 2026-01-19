using System;
using UnityEngine;

[Serializable]
public class BiomeSettings
{
    public string name;            // 바이옴 이름 (예: 꽃밭, 잔디뭉치)
    public int layerIndex;         // 터레인에 등록된 디테일 레이어 번호
    [Range(0, 1)] public float threshold; // 노이즈 값이 얼마 이상일 때 나타날지 (0~1)
    [Range(0, 20)] public float noiseScale = 0.05f; // 노이즈의 크기 (작을수록 덩어리가 커짐)
    public int minStrength = 3;    // 최소 밀도
    public int maxStrength = 12;   // 최대 밀도
}