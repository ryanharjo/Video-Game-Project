using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    [Header("Pool Settings")]
    public int poolSize = 50;

    private bool spawning = true;

    private List<GameObject> tokenPool = new List<GameObject>();
    private int currentIndex = 0;

    void Start()
    {
        CreatePool();
        StartCoroutine(SpawnTokens());
    }

    void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject token = Instantiate(tokenPrefab);

            token.SetActive(false);

            tokenPool.Add(token);
        }
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
            float currentZ = startZ + (i * spacingZ);

            Vector3 spawnPos =
                new Vector3(lanes[randomLane], spawnY, currentZ);

            // Prevent spawning inside obstacles
            bool blocked = Physics.CheckSphere(spawnPos, 1f);

            if (!blocked)
            {
                SpawnToken(spawnPos);
            }
        }
    }

    void SpawnToken(Vector3 position)
    {
        GameObject token = GetPooledToken();

        if (token == null) return;

        token.transform.position = position;
        token.transform.rotation = Quaternion.identity;

        token.SetActive(true);
    }

    GameObject GetPooledToken()
    {
        for (int i = 0; i < tokenPool.Count; i++)
        {
            if (!tokenPool[i].activeInHierarchy)
            {
                return tokenPool[i];
            }
        }

        return null;
    }

    public void StopSpawning()
    {
        spawning = false;
    }
}
