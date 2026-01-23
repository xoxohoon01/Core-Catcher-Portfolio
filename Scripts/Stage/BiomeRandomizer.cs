using UnityEngine;
using System.Collections.Generic;


public class BiomeRandomizer : MonoBehaviour
{
    public Terrain terrain;

    [Header("Noise Settings")]
    [Range(0.001f, 0.2f)] public float noiseScale = 0.05f;
    [Range(0.01f, 0.5f)] public float blend = 0.1f;

    [Header("Layer Weights")]
    [Tooltip("레이어 순서대로 기준점을 정하세요. (예: 0.2, 0.5, 0.8)")]
    public List<BiomeSettings> layerWeights = new List<BiomeSettings>();

    [Header("Preview Settings")]
    public bool autoUpdate = false;

    private void OnValidate()
    {
        if (autoUpdate && terrain != null && terrain.terrainData != null)
        {
            ApplyNoiseLayout();
        }
    }

    [ContextMenu("Apply Noise Layout")]
    public void ApplyNoiseLayout()
    {
        if (terrain == null) return;

        TerrainData terrainData = terrain.terrainData;
        int mapWidth = terrainData.alphamapWidth;
        int mapHeight = terrainData.alphamapHeight;
        int layerCount = terrainData.alphamapLayers;

        // 설정된 가중치 리스트가 레이어 개수와 맞지 않으면 최소한의 기본값 생성
        if (layerWeights.Count != layerCount)
        {
            Debug.LogWarning($"설정된 가중치 개수({layerWeights.Count})와 터레인 레이어 개수({layerCount})가 일치하지 않습니다.");
        }

        float[,,] alphamaps = new float[mapHeight, mapWidth, layerCount];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float noise = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                float totalWeight = 0f;

                for (int i = 0; i < layerCount; i++)
                {
                    // 설정된 threshold가 없으면 균등 분할로 대체
                    float targetThreshold = (i < layerWeights.Count)
                        ? layerWeights[i].threshold
                        : (float)i / layerCount;

                    // 노이즈 값과 설정된 threshold 사이의 거리 계산
                    float dist = Mathf.Abs(noise - targetThreshold);

                    // blend 값을 기준으로 가중치 계산 (Gaussian-like curve)
                    float weight = Mathf.Exp(-Mathf.Pow(dist / blend, 2));

                    alphamaps[y, x, i] = weight;
                    totalWeight += weight;
                }

                // 가중치 정규화
                for (int i = 0; i < layerCount; i++)
                {
                    if (totalWeight > 0.001f)
                        alphamaps[y, x, i] /= totalWeight;
                    else
                        alphamaps[y, x, 0] = 1f;
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphamaps);
    }
}