using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] obstaclePrefabs;
    public Transform[] spawnPoints;

    [Range(0, 1)]
    public float spawnChance = 0.7f;

    [Header("Pool Settings")]
    public int poolSizePerPrefab = 10;

    // Pool dictionary
    private Dictionary<GameObject, List<GameObject>> obstaclePools =
        new Dictionary<GameObject, List<GameObject>>();

    void Start()
    {
        CreatePools();
        SpawnRandomObstacle();
    }

    void CreatePools()
    {
        foreach (GameObject prefab in obstaclePrefabs)
        {
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < poolSizePerPrefab; i++)
            {
                GameObject obj = Instantiate(prefab);

                obj.SetActive(false);

                pool.Add(obj);
            }

            obstaclePools.Add(prefab, pool);
        }
    }

    public void SpawnRandomObstacle()
    {
        // Chance to skip spawning
        if (Random.value > spawnChance) return;

        if (obstaclePrefabs.Length == 0 || spawnPoints.Length == 0)
            return;

        // Pick random spawn point
        int pointIndex = Random.Range(0, spawnPoints.Length);

        // Pick random prefab
        int prefabIndex = Random.Range(0, obstaclePrefabs.Length);

        GameObject selectedPrefab = obstaclePrefabs[prefabIndex];

        // Get pooled object
        GameObject obstacle = GetPooledObject(selectedPrefab);

        if (obstacle == null) return;

        obstacle.transform.position = spawnPoints[pointIndex].position;
        obstacle.transform.rotation = Quaternion.identity;

        obstacle.SetActive(true);
    }

    GameObject GetPooledObject(GameObject prefab)
    {
        List<GameObject> pool = obstaclePools[prefab];

        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        return null;
    }
}
