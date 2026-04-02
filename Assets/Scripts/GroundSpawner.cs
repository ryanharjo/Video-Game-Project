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
        if (finishSpawned) return;
        if (player.position.z > spawnZ - (tilesOnScreen * tileLength))
        {
            SpawnTile();
        }
    }

    
    public void SpawnTile()
    {
        GameObject tileToSpawn = groundTilePrefab;

        
        if (!finishSpawned && GameManager.Instance.tokens >= GameManager.Instance.targetTokens)
        {
            tileToSpawn = finishLinePrefab;
            finishSpawned = true;
        }

        Instantiate(tileToSpawn, Vector3.forward * spawnZ, Quaternion.identity);
        spawnZ += tileLength;
    }
}
