using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    void Start()
    {
        SpawnObstacle();
    }

    void SpawnObstacle()
    {
        // Choose a random point on the tile
        // Assuming tile is 10 wide (-5 to 5) and 50 long (-25 to 25)
        // We leave some buffer so it doesn't spawn exactly on the edge

        int numberOfObstacles = 2; // How many obstacles per tile?

        for (int i = 0; i < numberOfObstacles; i++)
        {
            float randomX = Random.Range(-4f, 4f);
            float randomZ = Random.Range(-20f, 20f);

            Vector3 spawnPos = transform.position + new Vector3(randomX, 1, randomZ);

            // Create the object in the world first (it will use its own normal scale)
            GameObject obs = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

            // Parent it to the tile, but pass 'true' to keep its world scale
            obs.transform.SetParent(transform, true);
        }
    }
}
