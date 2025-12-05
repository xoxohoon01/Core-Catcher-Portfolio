using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [Header("타일 설정")]
    public GameObject tilePrefab;
    public int width = 10;
    public int height = 10;
    public float tileSize = 1f;

    [Header("오브젝트 설정")]
    public GameObject[] objectPrefabs;
    public int objectCount = 20;

    void Start()
    {
        SpawnTiles();
        SpawnRandomObjects();
    }

    void SpawnTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize);
                Instantiate(tilePrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    void SpawnRandomObjects()
    {
        for (int i = 0; i < objectCount; i++)
        {
            float randX = Random.Range(0, width) * tileSize;
            float randZ = Random.Range(0, height) * tileSize;

            GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

            // 4방향 회전: 0, 90, 180, 270
            int[] angles = { 0, 90, 180, 270 };
            int angle = angles[Random.Range(0, angles.Length)];

            Quaternion rot = Quaternion.Euler(0, angle, 0);
            Vector3 spawnPos = new Vector3(randX, 0.5f, randZ);

            Instantiate(prefab, spawnPos, rot, transform);
        }
    }
}
