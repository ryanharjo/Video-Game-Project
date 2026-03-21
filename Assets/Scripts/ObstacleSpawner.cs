using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    // Lane positions
    public float[] lanes = { -3f, 0f, 3f };

    public float spawnZ = 50f;
    public float spawnY = 4f;


    public float spawnRate = 2f;

    void Start()
    {
        InvokeRepeating("SpawnObstacle", 1f, spawnRate);
    }

    void SpawnObstacle()
    {
        int randomLane = Random.Range(0, lanes.Length);

        Vector3 spawnPosition = new Vector3(lanes[randomLane], spawnY, spawnZ);

        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}
