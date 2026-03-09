using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [Header("Settings")]
    public GameObject coinPrefab;
    public GameObject obstaclePrefab;
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

        SpawnObjects();
    }

    void SpawnObjects()
    {

        foreach (Transform point in spawnPoints)
        {
            float rand = Random.value;

            if (rand < 0.3f && coinPrefab != null)
            {
                Instantiate(coinPrefab, point.position, Quaternion.identity, transform);
            }
            else if (rand < 0.5f && obstaclePrefab != null)
            {
                Instantiate(obstaclePrefab, point.position, Quaternion.identity, transform);
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
