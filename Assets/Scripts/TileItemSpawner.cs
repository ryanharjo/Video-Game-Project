using UnityEngine;

public class TileItemSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    [Range(0, 100)] public float spawnChance = 50f; // 50% chance to have coins on this tile

    void Start()
    {
        // Only spawn if we win the "random roll"
        if (Random.Range(0f, 100f) <= spawnChance)
        {
            SpawnCoins();
        }
    }

    void SpawnCoins()
    {
        // Find all the lane markers we placed on this specific tile
        Transform[] spawnPoints = GetComponentsInChildren<Transform>();

        // Pick one random lane for this tile's coins
        int randomLaneIndex = Random.Range(0, 3);
        int currentPoint = 0;

        foreach (Transform point in spawnPoints)
        {
            if (point.CompareTag("CoinSpawnPoint"))
            {
                // Only spawn in the lane we randomly picked
                if (currentPoint == randomLaneIndex)
                {
                    // Spawn the coin as a child of the tile so it gets deleted with the tile
                    Instantiate(coinPrefab, point.position, Quaternion.identity, transform);
                }
                currentPoint++;
            }
        }
    }
}
