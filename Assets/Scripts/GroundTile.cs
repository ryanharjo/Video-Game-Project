using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [Header("Obstacle Setup")]
    // Taken from ObstacleSpawner: Allows for variety in obstacles
    public GameObject[] obstaclePrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    void Start()
    {
        SpawnObstacle();
    }

    void SpawnObstacle()
    {
        // Safety checks
        if (spawnPoints.Length == 0 || obstaclePrefabs.Length == 0) return;

        // 1. Pick a random spawn point on THIS tile
        int randomPointIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[randomPointIndex];

        // 2. Pick a random prefab from your variety pack
        int randomPrefabIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject selectedPrefab = obstaclePrefabs[randomPrefabIndex];

        // 3. Spawn it as a child of this tile (the 'transform' argument at the end)
        Instantiate(
            selectedPrefab,
            selectedPoint.position,
            Quaternion.identity,
            transform
        );
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tell the spawner to make a new tile
            FindFirstObjectByType<GroundSpawner>().SpawnTile();

            // This destroys the tile AND the obstacle inside it after 2 seconds
            Destroy(gameObject, 2);
        }
    }
}