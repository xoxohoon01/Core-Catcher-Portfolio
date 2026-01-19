using UnityEngine;
using System.Collections.Generic;

public class DetailRandomizer : MonoBehaviour
{
    public Terrain terrain;
    public List<BiomeDetail> biomeDetails = new List<BiomeDetail>();

    [Header("전역 설정")]
    public bool useRandomSeed = true;
    public float seed = 0f;

    public void Generate()
    {
        if (terrain == null) terrain = GetComponent<Terrain>();
        if (terrain == null) return;

        TerrainData data = terrain.terrainData;
        if (useRandomSeed) seed = UnityEngine.Random.Range(0f, 10000f);

        int width = data.detailWidth;
        int height = data.detailHeight;

        foreach (var detail in biomeDetails)
        {
            int[,] map = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float normX = (float)x / (width - 1);
                    float normY = (float)y / (height - 1);

                    float sampleX = (normX * detail.noiseScale) + seed + (detail.layerIndex * 500f);
                    float sampleY = (normY * detail.noiseScale) + seed + (detail.layerIndex * 500f);

                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);
                    if (noiseValue > detail.threshold)
                    {
                        map[x, y] = UnityEngine.Random.Range(detail.minStrength, detail.maxStrength + 1);
                    }
                }
            }
            data.SetDetailLayer(0, 0, detail.layerIndex, map);
        }
    }

    public void UpdatePreviews()
    {
        if (seed == 0 && useRandomSeed) seed = UnityEngine.Random.Range(0f, 10000f);

        foreach (var detail in biomeDetails)
        {
            // GUID가 없으면 새로 생성 (복사 붙여넣기 시 중복 방지)
            if (string.IsNullOrEmpty(detail.guid)) detail.guid = System.Guid.NewGuid().ToString();

            int res = 128;

            // 기존 텍스처 인스턴스가 다른 바이옴과 공유되고 있는지 체크하고 
            // 독립된 새 인스턴스를 강제로 할당합니다.
            detail.previewTexture = new Texture2D(res, res);
            detail.previewTexture.name = "Texture_" + detail.guid;
            detail.previewTexture.hideFlags = HideFlags.DontSave;

            Color[] pixels = new Color[res * res];
            for (int y = 0; y < res; y++)
            {
                for (int x = 0; x < res; x++)
                {
                    float normX = (float)x / (res - 1);
                    float normY = (float)y / (res - 1);

                    float sampleX = (normX * detail.noiseScale) + seed + (detail.layerIndex * 500f);
                    float sampleY = (normY * detail.noiseScale) + seed + (detail.layerIndex * 200f);

                    float v = Mathf.PerlinNoise(sampleX, sampleY);
                    Color color = v > detail.threshold ? Color.white : Color.black;

                    // 반시계 90도 + 좌우반전
                    int targetX = y;
                    int targetY = x;
                    pixels[targetY * res + targetX] = color;
                }
            }
            detail.previewTexture.SetPixels(pixels);
            detail.previewTexture.Apply();
        }
    }

    public void ClearAll()
    {
        if (terrain == null) return;
        TerrainData data = terrain.terrainData;
        for (int i = 0; i < data.detailPrototypes.Length; i++)
            data.SetDetailLayer(0, 0, i, new int[data.detailWidth, data.detailHeight]);
    }
}