using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;

    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    private List<int> availableSpawnIndices = new List<int> { 2, 3, 4 };

    private void Start()
    {
        groundSpawner = GameObject.FindFirstObjectByType<GroundSpawner>();
        int obstacleIndex = GetRandomSpawnIndex();
        SpawnObstacle(obstacleIndex);

        
        if (Random.value > 0.4f) // 60% chance
        {
            int coinIndex = GetRandomSpawnIndex();
            SpawnCoin(coinIndex);
        }
    }

    private int GetRandomSpawnIndex()
    {
        if (availableSpawnIndices.Count == 0) return -1;
        int randomIndex = Random.Range(0, availableSpawnIndices.Count);
        int spawnIndex = availableSpawnIndices[randomIndex];
        return spawnIndex;
    }

    void SpawnObstacle(int index)
    {
        if (index == -1) return;
        Transform spawnPoint = transform.GetChild(index);
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
    }

    void SpawnCoin(int index)
    {
        if (index == -1) return;
        Transform spawnPoint = transform.GetChild(index);
        Instantiate(coinPrefab, spawnPoint.position + Vector3.up, Quaternion.identity, transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            groundSpawner.SpawnTile();
            Destroy(gameObject, 2f);
        }
    }

}
