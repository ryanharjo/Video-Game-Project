using UnityEngine;

public class TokenSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject tokenPrefab;
    public Transform player;

    [Header("Lane Settings")]
    public float[] lanes = { -3f, 0f, 3f };

    [Header("Spawn Settings")]
    public float spawnDistance = 30f;
    public float spawnRate = 1.5f;
    public float spawnY = 2f;

    [Header("Row Settings")]
    public int tokensPerRow = 5;
    public float spacing = 2.5f;

    private float nextSpawnZ;

    void Start()
    {
        nextSpawnZ = player.position.z + spawnDistance;

        InvokeRepeating(nameof(SpawnTokenRow), 1f, spawnRate);
    }

    void SpawnTokenRow()
    {
        // Random starting lane
        int randomLaneIndex = Random.Range(0, lanes.Length);
        float laneX = lanes[randomLaneIndex];

        // Spawn a row ahead of the player
        float rowStartZ = nextSpawnZ;

        for (int i = 0; i < tokensPerRow; i++)
        {
            // Optional lane switch halfway through row
            if (i == tokensPerRow / 2)
            {
                laneX = lanes[Random.Range(0, lanes.Length)];
            }

            Vector3 spawnPos = new Vector3(laneX, spawnY, rowStartZ + (i * spacing));
            Instantiate(tokenPrefab, spawnPos, Quaternion.identity);
        }

        // Move next row farther ahead
        nextSpawnZ += tokensPerRow * spacing + 15f;
    }
}
