using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    [Header("Tile Settings")]
    public GameObject[] tilePrefabs; // Array of different obstacle tiles
    public float tileLength = 10f;   // Length of one tile
    public int tilesOnScreen = 15;    // Number of tiles visible at once

    [Header("References")]
    public Transform playerTransform; // Drag the Player object here

    private List<GameObject> activeTiles = new List<GameObject>();
    private float nextSpawnZ = 0f;

    void Start()
    {
        // 1. Spawn initial tiles
        for (int i = 0; i < tilesOnScreen; i++)
        {
            // First 2 tiles should probably be empty/safe tiles
            if (i < 2)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0, tilePrefabs.Length));
        }
    }

    void Update()
    {
        // 2. Check if player has moved far enough to need a new tile
        // We spawn a new one when the player is within 'X' distance of the end
        if (playerTransform.position.z - 35 > (nextSpawnZ - tilesOnScreen * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteOldTile();
        }
    }

    public void SpawnTile(int prefabIndex)
    {
        GameObject go = Instantiate(tilePrefabs[prefabIndex], transform.forward * nextSpawnZ, Quaternion.identity);

        // Tell the tile that THIS is its spawner
        go.GetComponent<GroundTile>().groundSpawner = this;

        activeTiles.Add(go);
        nextSpawnZ += tileLength;
    }

    private void DeleteOldTile()
    {
        // Remove the oldest tile from the game and the list
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
