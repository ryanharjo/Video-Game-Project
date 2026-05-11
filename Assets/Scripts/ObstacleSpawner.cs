using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] obstaclePrefabs;
    public Transform[] spawnPoints;

    [Range(0, 1)]
    public float spawnChance = 0.7f; // 70% chance to spawn an obstacle

    void Start()
    {
        SpawnRandomObstacle();
    }

    public void SpawnRandomObstacle()
    {
        // Randomly decide if we should even spawn an obstacle here
        if (Random.value > spawnChance) return;

        if (obstaclePrefabs.Length > 0 && spawnPoints.Length > 0)
        {
            // Pick random locations and prefabs
            int pointIndex = Random.Range(0, spawnPoints.Length);
            int prefabIndex = Random.Range(0, obstaclePrefabs.Length);

            Instantiate(obstaclePrefabs[prefabIndex],
                spawnPoints[pointIndex].position,
                Quaternion.identity,
                transform // Makes it a child of the spawner/tile
            );
        }
    }
}
