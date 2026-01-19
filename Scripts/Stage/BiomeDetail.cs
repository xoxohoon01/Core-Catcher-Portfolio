using UnityEngine;
using System;

[Serializable]
public class BiomeDetail
{
    public string name;
    public int layerIndex;

    // 인스펙터에서 각 항목을 고유하게 식별하기 위한 ID
    [HideInInspector] public string guid = Guid.NewGuid().ToString();

    [Header("구역 설정 (Noise)")]
    [Range(0, 1)] public float threshold = 0.5f;
    [Tooltip("값이 클수록 패턴이 더 많이 반복(자잘하게) 됩니다.")]
    public float noiseScale = 5f;

    // 바이옴마다 고유한 위치를 갖게 만드는 시드
    [HideInInspector] public float biomeSeed;

    [Header("밀도 설정 (Strength)")]
    public int minStrength = 3;
    public int maxStrength = 12;

    [HideInInspector] public Texture2D previewTexture;
}