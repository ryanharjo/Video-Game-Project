using UnityEngine;

public class TokenSpawner : MonoBehaviour
{
    public GameObject tokenPrefab;
    public float[] lanes = { -3f, 0f, 3f };
    public Transform player;

    [Header("Spawn Settings")]
    public float spawnDistance = 30f;
    public float spawnRate = 4f; // Time between rows
    public float spawnY = 1f;

    [Header("Row Settings")]
    public int tokensPerRow = 10;
    public float spacing = 2.5f; // Distance between each token in the row

    private float lastSpawnZ = 0f;

    void Start()
    {
        // Start spawning rows after 1 second
        InvokeRepeating("SpawnTokenRow", 1f, spawnRate);
    }

    void SpawnTokenRow()
    {
        // 1. Determine the lane for this row (stays consistent for the whole row)
        int randomLaneIndex = Random.Range(0, lanes.Length);
        float laneX = lanes[randomLaneIndex];

        // 2. Calculate the starting Z position (at least spawnDistance ahead of player)
        float startZ = Mathf.Max(player.position.z + spawnDistance, lastSpawnZ + spacing);

        // 3. Loop to spawn 10 tokens in a line
        for (int i = 0; i < tokensPerRow; i++)
        {
            if (i > 5)
            {
                // Make the second half of the row shift to a different lane
                laneX = lanes[1];
            }

            float currentZ = startZ + (i * spacing);
            Vector3 spawnPos = new Vector3(laneX, spawnY, currentZ);

            Instantiate(tokenPrefab, spawnPos, Quaternion.identity);

            // Update the tracker so the next row doesn't spawn on top of this one
            lastSpawnZ = currentZ;
        }
    }
}
