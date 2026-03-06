using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [Header("Settings")]
    public GameObject coinPrefab;
    public Transform[] spawnPoints;

    [Header("References (Auto-filled)")]
    public GroundSpawner groundSpawner;

    void Start()
    {
        groundSpawner = Object.FindFirstObjectByType<GroundSpawner>();

        
        if (groundSpawner == null)
        {
            Debug.LogError("GroundTile: Could not find a GroundSpawner in the scene! Make sure one exists.");
        }

        SpawnCoins();
    }

    void SpawnCoins()
    {
        // Safety check: Don't try to spawn if the prefab is missing
        if (coinPrefab == null) return;

        foreach (Transform point in spawnPoints)
        {
            // 30% chance to spawn a coin at each point
            if (Random.value < 0.3f)
            {
                Instantiate(coinPrefab, point.position, Quaternion.identity, transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Only trigger if the Player leaves AND we have a valid reference to the spawner
        if (other.CompareTag("Player") && groundSpawner != null)
        {
            // Tell the spawner to create a new tile
            groundSpawner.SpawnTile(Random.Range(0, groundSpawner.tilePrefabs.Length));

            // Destroy this tile after 2 seconds to keep the game running smoothly
            Destroy(gameObject, 2f);
        }
    }
}
