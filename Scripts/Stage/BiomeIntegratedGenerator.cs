using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BiomeDetail
{
    public string detailName;
    public int detailLayerIndex;

    [Header("구역 설정 (Noise)")]
    [Range(0f, 1f)] public float threshold = 0.5f;
    public float noiseScale = 5f;

    [Header("밀도 설정 (Strength)")]
    public int minStrength = 3;
    public int maxStrength = 12;

    [Header("크기 설정 (Size)")]
    public float minWidth = 1f;
    public float maxWidth = 2f;
    public float minHeight = 1f;
    public float maxHeight = 2f;
}

[Serializable]
public class BiomeSettings
{
    public string layerName;
    public int terrainLayerIndex;
    [Range(0f, 1f)] public float threshold;
    public List<BiomeDetail> details = new List<BiomeDetail>();
}

public class BiomeIntegratedGenerator : MonoBehaviour
{
    public Terrain terrain;

    [Header("Biome Settings")]
    [Range(0.001f, 0.2f)] public float noiseScale = 0.05f;
    [Range(0.01f, 0.5f)] public float blend = 0.1f;

    [Header("Biome Layout")]
    public List<BiomeSettings> biomeWeights = new List<BiomeSettings>();

    [Header("Details Global Settings")]
    public bool useDetailRandomSeed;
    public float detailSeed = 0;

    [Header("Preview Settings")]
    public bool biomeAutoUpdate = false;
    public bool detailsAutoUpdate = false;

    private void OnValidate()
    {
        if (terrain != null && terrain.terrainData != null)
        {
            SyncLayersWithTerrain();
            if (biomeAutoUpdate) ApplyBiomeLayout();
            if (detailsAutoUpdate) ApplyDetailsLayout();
        }
    }

    [ContextMenu("Sync Layers With Terrain")]
    public void SyncLayersWithTerrain()
    {
        if (terrain == null || terrain.terrainData == null) return;
        TerrainLayer[] layers = terrain.terrainData.terrainLayers;
        Dictionary<int, BiomeSettings> existingData = new Dictionary<int, BiomeSettings>();
        foreach (var bw in biomeWeights) if (!existingData.ContainsKey(bw.terrainLayerIndex)) existingData.Add(bw.terrainLayerIndex, bw);

        biomeWeights.Clear();
        for (int i = 0; i < layers.Length; i++)
        {
            BiomeSettings newBiome = new BiomeSettings { terrainLayerIndex = i, layerName = layers[i] != null ? layers[i].name : $"Layer {i}" };
            if (existingData.ContainsKey(i)) { newBiome.threshold = existingData[i].threshold; newBiome.details = existingData[i].details; }
            else { newBiome.threshold = (float)i / Mathf.Max(1, layers.Length - 1); }
            biomeWeights.Add(newBiome);
        }
    }

    [ContextMenu("Apply Biome Layout")]
    public void ApplyBiomeLayout()
    {
        if (terrain == null || terrain.terrainData == null) return;
        TerrainData data = terrain.terrainData;
        int w = data.alphamapWidth, h = data.alphamapHeight, layerCount = data.alphamapLayers;
        float[,,] alphamaps = new float[h, w, layerCount];

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float noise = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                float totalWeight = 0f;
                float[] tempWeights = new float[layerCount];
                for (int i = 0; i < layerCount; i++)
                {
                    float target = i < biomeWeights.Count ? biomeWeights[i].threshold : (float)i / layerCount;
                    tempWeights[i] = Mathf.Exp(-Mathf.Pow(Mathf.Abs(noise - target) / blend, 2));
                    totalWeight += tempWeights[i];
                }
                for (int i = 0; i < layerCount; i++) alphamaps[y, x, i] = totalWeight > 0.001f ? tempWeights[i] / totalWeight : (i == 0 ? 1f : 0f);
            }
        }
        data.SetAlphamaps(0, 0, alphamaps);
    }

    [ContextMenu("Clear Detail Prototypes (전체 삭제)")]
    public void ClearDetailPrototypes()
    {
        if (terrain == null || terrain.terrainData == null) return;
        TerrainData data = terrain.terrainData;

        data.detailPrototypes = new DetailPrototype[0];
        data.RefreshPrototypes(); // 프로토타입 변경 후 내부 참조를 갱신해 "Can't assign prototype" 경고를 정리
    }

    [ContextMenu("Apply Details Layout")]
    public void ApplyDetailsLayout()
    {
        if (terrain == null || terrain.terrainData == null) return;
        TerrainData data = terrain.terrainData;

        UpdateDetailPrototypes(data);
        ClearAllDetails(data);

        if (useDetailRandomSeed) detailSeed = UnityEngine.Random.Range(0f, 10000f);
        int dW = data.detailWidth, dH = data.detailHeight;
        int aW = data.alphamapWidth, aH = data.alphamapHeight;
        float[,,] currentAlphamaps = data.GetAlphamaps(0, 0, aW, aH);

        foreach (var biome in biomeWeights)
        {
            foreach (var detail in biome.details)
            {
                int[,] map = new int[dH, dW];
                for (int y = 0; y < dH; y++)
                {
                    for (int x = 0; x < dW; x++)
                    {
                        float normX = (float)x / (dW - 1);
                        float normY = (float)y / (dH - 1);

                        // 변수명 aX, aY로 통일하여 매칭
                        int aX = Mathf.Clamp(Mathf.FloorToInt(normX * (aW - 1)), 0, aW - 1);
                        int aY = Mathf.Clamp(Mathf.FloorToInt(normY * (aH - 1)), 0, aH - 1);

                        // 바이옴 강도 체크
                        if (currentAlphamaps[aY, aX, biome.terrainLayerIndex] > 0.5f)
                        {
                            float sX = (normX * detail.noiseScale) + detailSeed + (detail.detailLayerIndex * 100f);
                            float sY = (normY * detail.noiseScale) + detailSeed + (detail.detailLayerIndex * 100f);
                            if (Mathf.PerlinNoise(sX, sY) > detail.threshold)
                                map[y, x] = UnityEngine.Random.Range(detail.minStrength, detail.maxStrength + 1);
                        }
                    }
                }
                AddDetailLayerCustom(data, detail.detailLayerIndex, map);
            }
        }
    }

    private void UpdateDetailPrototypes(TerrainData data)
    {
        DetailPrototype[] prototypes = data.detailPrototypes;
        foreach (var biome in biomeWeights)
        {
            foreach (var detail in biome.details)
            {
                if (detail.detailLayerIndex >= 0 && detail.detailLayerIndex < prototypes.Length)
                {
                    prototypes[detail.detailLayerIndex].minWidth = detail.minWidth;
                    prototypes[detail.detailLayerIndex].maxWidth = detail.maxWidth;
                    prototypes[detail.detailLayerIndex].minHeight = detail.minHeight;
                    prototypes[detail.detailLayerIndex].maxHeight = detail.maxHeight;
                }
            }
        }
        data.detailPrototypes = prototypes;
    }

    private void AddDetailLayerCustom(TerrainData data, int prototypeIndex, int[,] newMap)
    {
        int w = data.detailWidth, h = data.detailHeight;
        int[,] existingMap = data.GetDetailLayer(0, 0, w, h, prototypeIndex);
        for (int y = 0; y < h; y++) for (int x = 0; x < w; x++) existingMap[y, x] += newMap[y, x];
        data.SetDetailLayer(0, 0, prototypeIndex, existingMap);
    }

    private void ClearAllDetails(TerrainData data)
    {
        for (int i = 0; i < data.detailPrototypes.Length; i++)
            data.SetDetailLayer(0, 0, i, new int[data.detailHeight, data.detailWidth]);
    }
}