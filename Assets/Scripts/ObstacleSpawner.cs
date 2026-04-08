using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs Setup")]
    public GameObject[] obstaclePrefabs; // The array of different obstacles

    [Header("Spawning Settings")]
    public float[] lanes = { -3f, 0f, 3f };
    public float spawnDistance = 50f;
    public float spawnY = 1f;
    public float spawnRate = 2.5f;
    public float minDistanceBetweenObstacles = 35f;

    public Transform player;
    private float lastSpawnZ;

    void Start()
    {
        // Safety check: Make sure you actually assigned prefabs in the inspector
        if (obstaclePrefabs.Length == 0)
        {
            Debug.LogError("Please assign at least one prefab to the Obstacle Prefabs array!");
            return;
        }

        lastSpawnZ = player.position.z;
        InvokeRepeating("SpawnObstacle", 1f, spawnRate);
    }

    void SpawnObstacle()
    {
        // 1. Pick a random prefab from the array
        int randomPrefabIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject selectedPrefab = obstaclePrefabs[randomPrefabIndex];

        // 2. Pick a random lane
        int randomLane = Random.Range(0, lanes.Length);

        // 3. Calculate Z position logic
        float spawnZPos = Mathf.Max(player.position.z + spawnDistance, lastSpawnZ + minDistanceBetweenObstacles);
        Vector3 spawnPosition = new Vector3(lanes[randomLane], spawnY, spawnZPos);

        // 4. Spawn it
        Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        lastSpawnZ = spawnZPos;
    }
}
