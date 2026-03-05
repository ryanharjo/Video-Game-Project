using UnityEngine;

public class GroundTileV2 : MonoBehaviour
{
    public GroundSpawner groundSpawner;
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;


    public Transform[] spawnPoints;

    void Start()
    {
        SpawnObstacles();
        SpawnCoins();
    }

    void SpawnObstacles()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(obstaclePrefab, spawnPoints[randomIndex].position, Quaternion.identity, transform);
    }

    void SpawnCoins()
    {
        foreach (Transform point in spawnPoints)
        {
            if (Random.value < 0.3f)
            {
                Instantiate(coinPrefab, point.position, Quaternion.identity, transform);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            groundSpawner.SpawnTile(Random.Range(0, groundSpawner.tilePrefabs.Length));
        }
    }
}
