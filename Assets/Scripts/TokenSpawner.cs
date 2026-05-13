using UnityEngine;
using System.Collections;

public class TokenSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject tokenPrefab;
    public Transform player;

    [Header("Lane Settings")]
    public float[] lanes = { -3f, 0f, 3f };

    [Header("Spawn Settings")]
    public float spawnDistance = 60f;
    public float spawnRate = 4f;
    public float spawnY = 2f;

    [Header("Rows")]
    public int tokensPerRow = 5;
    public float spacingZ = 2f;

    private bool spawning = true;

    void Start()
    {
        StartCoroutine(SpawnTokens());
    }

    IEnumerator SpawnTokens()
    {
        while (spawning)
        {
            SpawnRow();

            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnRow()
    {
        float startZ = player.position.z + spawnDistance;
        int randomLane = Random.Range(0, lanes.Length);
        for (int i = 0; i < tokensPerRow; i++)
        {
            // Calculate Z position for each token in the row
            float currentZ = startZ + (i * spacingZ);

            Vector3 spawnPos = new Vector3(lanes[randomLane], spawnY, currentZ);

            // Check if obstacle already exists here
            bool blocked = Physics.CheckSphere(spawnPos, 1f);

            if (!blocked)
            {
                Instantiate(tokenPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    public void StopSpawning()
    {
        spawning = false;
    }
}
