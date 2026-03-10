using UnityEngine;

public class GroundTile : MonoBehaviour
{
   [Header("Settings")]
    public GameObject obstaclePrefab;
    
    public Transform[] spawnPoints; 

    [Header("References (Auto-filled)")]
    public GroundSpawner groundSpawner;

    void Start()
    {
        groundSpawner = Object.FindFirstObjectByType<GroundSpawner>();
        SpawnObstacle();
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab != null && spawnPoints.Length > 0)
        {
            
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            
            Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && groundSpawner != null)
        {
            groundSpawner.SpawnTile(Random.Range(0, groundSpawner.tilePrefabs.Length));
            Destroy(gameObject, 2f);
        }
    }
}