using UnityEngine;

public class TokenSpawner : MonoBehaviour
{
    public GameObject tokenPrefab;

    // Lane positions (match your player lanes)
    public float[] lanes = { -3f, 0f, 3f };

    public Transform player;

    public float spawnDistance = 40f; // how far ahead of player
    public float spawnRate = 1.5f;    // time between spawns
    public float spawnY = 1f;

    private float lastSpawnZ = 0f;

    void Start()
    {
        InvokeRepeating("SpawnToken", 1f, spawnRate);
    }

    void SpawnToken()
    {
        int randomLane = Random.Range(0, lanes.Length);

        float spawnZ = Mathf.Max(
            player.position.z + spawnDistance,
            lastSpawnZ + 2f // prevents stacking
        );

        Vector3 spawnPos = new Vector3(
            lanes[randomLane],
            spawnY,
            spawnZ
        );

        Instantiate(tokenPrefab, spawnPos, Quaternion.identity);

        lastSpawnZ = spawnZ;
    }
}
