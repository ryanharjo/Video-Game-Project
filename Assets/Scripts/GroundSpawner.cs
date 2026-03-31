using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public GameObject groundTilePrefab;
    public GameObject finishLinePrefab;

    [Header("Player & Tiles")]
    public Transform player;
    private float tileLength = 10f;
    private int tilesOnScreen = 5;
    private float spawnZ = 0f;

    private bool finishSpawned = false; // finish line spawned?

    void Start()
    {
        // Spawn initial tiles
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // Spawn tiles as player moves forward
        if (player.position.z > spawnZ - (tilesOnScreen * tileLength))
        {
            SpawnTile();
        }
    }

    // Spawn tile method
    public void SpawnTile()
    {
        GameObject tileToSpawn = groundTilePrefab;

        // Spawn finish line immediately when token goal reached
        if (!finishSpawned && GameManager.Instance.tokens >= GameManager.Instance.targetTokens)
        {
            tileToSpawn = finishLinePrefab;
            finishSpawned = true;
        }

        Instantiate(tileToSpawn, Vector3.forward * spawnZ, Quaternion.identity);
        spawnZ += tileLength;
    }
}
