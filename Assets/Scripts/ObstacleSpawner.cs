using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float spawnZ = 40f;
    public float spawnInterval = 1.2f;
    public float spawnX = 2f; // lane offset
    public int lanes = 3;
    public float startDelay = 1f;

    private float timer = 0f;
    private bool spawning = true;

    void Start()
    {
        timer = -startDelay;
    }

    void Update()
    {
        if (!spawning) return;
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        // pick lane 0..lanes-1
        int lane = Random.Range(0, lanes);
        float x = (lane - (lanes / 2)) * spawnX;
        Vector3 pos = new Vector3(x, 0.5f, transform.position.z + spawnZ);
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Instantiate(prefab, pos, Quaternion.identity);
    }

    public void StopSpawning()
    {
        spawning = false;
    }
}
