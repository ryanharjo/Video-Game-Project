using UnityEngine;

public class TokenSpawner : MonoBehaviour
{
    public GameObject tokenPrefab;

    
    public float[] lanes = { -3f, 0f, 3f };

    public Transform player;

    public float spawnDistance = 30f; 
    public float spawnRate = 35f;    
    public float spawnY = 1f;

    private float lastSpawnZ = 0f;

    void Start()
    {
        InvokeRepeating("SpawnToken", 1f, spawnRate);
    }

    void SpawnToken()
    {
        int randomLane = Random.Range(0, lanes.Length);

        float spawnZ = Mathf.Max(player.position.z + spawnDistance, lastSpawnZ + 2f);

        Vector3 spawnPos = new Vector3(lanes[randomLane], spawnY, spawnZ);

        Instantiate(tokenPrefab, spawnPos, Quaternion.identity);

        lastSpawnZ = spawnZ;
    }
}
