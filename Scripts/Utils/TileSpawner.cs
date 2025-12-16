using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [Header("타일 설정")]
    public GameObject[] tilePrefabs;
    public int width = 10;
    public int height = 10;
    public float tileSize = 1f;

    [Header("오브젝트 설정")]
    public GameObject[] objectPrefabs;
    public int objectCount = 20;

    [Header("중앙 비우기 (타일 단위)")]
    public int centerEmptySizeX = 2; // 가로 반경
    public int centerEmptySizeZ = 2; // 세로 반경

    private Vector3 centerOffset;
    private HashSet<Vector2Int> occupiedTiles = new HashSet<Vector2Int>();

    private Vector2Int centerTile;

    public NavMeshSurface surface;

    void Start()
    {
        centerOffset = new Vector3(
            (width - 1) * tileSize * 0.5f,
            0,
            (height - 1) * tileSize * 0.5f
        );

        centerTile = new Vector2Int(
            width / 2,
            height / 2
        );

        SpawnTiles();
        SpawnRandomObjects();
        surface.BuildNavMesh();
    }

    void SpawnTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize) - centerOffset;
                GameObject tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                Instantiate(tilePrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    void SpawnRandomObjects()
    {
        int safety = 0;

        for (int i = 0; i < objectCount; i++)
        {
            if (safety++ > 1000)
                break;

            int x = Random.Range(0, width);
            int z = Random.Range(0, height);

            Vector2Int tilePos = new Vector2Int(x, z);

            // 중앙 비우기 영역 체크
            if (IsInCenterEmptyArea(tilePos))
            {
                i--;
                continue;
            }

            // 이미 점유된 타일 체크
            if (occupiedTiles.Contains(tilePos))
            {
                i--;
                continue;
            }

            occupiedTiles.Add(tilePos);

            Vector3 spawnPos = new Vector3(
                x * tileSize,
                0,
                z * tileSize
            ) - centerOffset;

            GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

            int[] angles = { 0, 90, 180, 270 };
            int angle = angles[Random.Range(0, angles.Length)];

            Instantiate(prefab, spawnPos, Quaternion.Euler(0, angle, 0), transform);
        }
    }

    bool IsInCenterEmptyArea(Vector2Int tilePos)
    {
        return Mathf.Abs(tilePos.x - centerTile.x) <= centerEmptySizeX &&
               Mathf.Abs(tilePos.y - centerTile.y) <= centerEmptySizeZ;
    }
}
