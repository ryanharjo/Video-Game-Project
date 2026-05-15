using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public GameObject groundTilePrefab;
    public GameObject finishLinePrefab;

    [Header("Player & Tiles")]
    public Transform player;
    public float tileLength = 10f;
    public int tilesOnScreen = 5;

    [Header("Pool Settings")]
    public int groundPoolSize = 10;
    public int finishPoolSize = 2;

    private float spawnZ = 0f;
    private bool finishSpawned = false;

    private List<GameObject> groundPool = new List<GameObject>();
    private List<GameObject> finishPool = new List<GameObject>();

    private int groundIndex = 0;
    private int finishIndex = 0;

    void Start()
    {
        CreatePools();

        // Spawn starting tiles
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (finishSpawned) return;

        if (player.position.z > spawnZ - (tilesOnScreen * tileLength))
        {
            SpawnTile();
        }
    }

    void CreatePools()
    {
        // Ground pool
        for (int i = 0; i < groundPoolSize; i++)
        {
            GameObject obj = Instantiate(groundTilePrefab);
            obj.SetActive(false);

            groundPool.Add(obj);
        }

        // Finish line pool
        for (int i = 0; i < finishPoolSize; i++)
        {
            GameObject obj = Instantiate(finishLinePrefab);
            obj.SetActive(false);

            finishPool.Add(obj);
        }
    }

    public void SpawnTile()
    {
        GameObject tile;

        // Spawn finish line when target reached
        if (!finishSpawned &&
            GameManager.Instance.tokens >= GameManager.Instance.targetTokens)
        {
            tile = finishPool[finishIndex];

            finishIndex++;

            if (finishIndex >= finishPool.Count)
            {
                finishIndex = 0;
            }

            finishSpawned = true;
        }
        else
        {
            tile = groundPool[groundIndex];

            groundIndex++;

            if (groundIndex >= groundPool.Count)
            {
                groundIndex = 0;
            }
        }

        tile.transform.position = Vector3.forward * spawnZ;
        tile.transform.rotation = Quaternion.identity;

        tile.SetActive(true);

        spawnZ += tileLength;
    }

    public void DisableTile(GameObject tile)
    {
        tile.SetActive(false);
    }
}
