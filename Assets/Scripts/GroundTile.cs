using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;

    public GameObject obstaclePrefab;
    public GameObject coinPrefab;

    private void Start()
    {
        groundSpawner = GameObject.FindFirstObjectByType<GroundSpawner>();
        SpawnObstacle();
        SpawnCoin();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            groundSpawner.SpawnTile();
            Destroy(gameObject, 2);
        }
    }

    void SpawnObstacle()
    {
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex);
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
    }

    void SpawnCoin()
    {
        // 60% chance to spawn a coin
        if (Random.value > 0.4f)
        {
            int coinSpawnIndex = Random.Range(2, 5);
            Transform spawnPoint = transform.GetChild(coinSpawnIndex);
            Instantiate(coinPrefab, spawnPoint.position + Vector3.up, Quaternion.identity, transform);
        }
    }
}
