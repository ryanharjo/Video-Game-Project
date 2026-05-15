using UnityEngine;
using System.Collections.Generic;

public class FinishlineSpawner : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject finishLinePrefab;
    public int poolSize = 1;

    [Header("Spawn Settings")]
    public Transform player;
    public float spawnDistance = 50f;

    private List<GameObject> finishLinePool = new List<GameObject>();
    private int currentIndex = 0;

    void Start()
    {
        CreatePool();
    }

    void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(finishLinePrefab);
            obj.SetActive(false);

            finishLinePool.Add(obj);
        }
    }

    public void SpawnFinishline()
    {
        GameObject finishLine = finishLinePool[currentIndex];

        Vector3 spawnPos = new Vector3(
            0f,
            0f,
            player.position.z + spawnDistance
        );

        finishLine.transform.position = spawnPos;
        finishLine.transform.rotation = Quaternion.identity;

        finishLine.SetActive(true);

        currentIndex++;

        if (currentIndex >= finishLinePool.Count)
        {
            currentIndex = 0;
        }
    }

    public void DisableFinishline(GameObject finishLine)
    {
        finishLine.SetActive(false);
    }
}
