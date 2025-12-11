using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform player;

    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnZ = 0.0f;
    private float tileLength = 50.0f; // Must match your GroundTile Z scale
    private int safeZone = 55;
    private int startTiles = 5;

    void Start()
    {
        // Spawn initial tiles
        for (int i = 0; i < startTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // Check if we need to spawn a new tile
        // (Player Position - SafeZone > Start of the first tile)
        if (player.position.z - safeZone > (spawnZ - startTiles * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    void SpawnTile()
    {
        GameObject go = Instantiate(tilePrefab, transform.forward * spawnZ, transform.rotation);
        activeTiles.Add(go);
        spawnZ += tileLength;
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
