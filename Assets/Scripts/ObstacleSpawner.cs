using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    public float spawnRate = 2f;
    public float laneDistance = 3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 2f, spawnRate);
    }

    void SpawnObstacle()
    {
        int lane = Random.Range(-1, 2);

        Vector3 spawnPos = new Vector3(
            lane * laneDistance,
            transform.position.y,
            transform.position.z
        );

        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }
}
