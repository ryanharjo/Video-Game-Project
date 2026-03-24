using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    
    public float[] lanes = { -3f, 0f, 3f };

    public float spawnDistance = 50f; 
    public float spawnY = 1f;

    public float spawnRate = 2.5f;
    float lastSpawnZ;
    public float minDistanceBetweenObstacles = 35f;

    public Transform player; 

    void Start()
    {
        lastSpawnZ = player.position.z;
        InvokeRepeating("SpawnObstacle", 1f, spawnRate);
    }

    void SpawnObstacle()
    {
        int randomLane = Random.Range(0, lanes.Length);
        float spawnZPos = Mathf.Max(player.position.z + spawnDistance, lastSpawnZ + minDistanceBetweenObstacles);

        Vector3 spawnPosition = new Vector3(lanes[randomLane], spawnY, spawnZPos);

        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        lastSpawnZ = spawnZPos;
    }
}
