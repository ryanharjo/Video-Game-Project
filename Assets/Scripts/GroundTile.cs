using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject obstaclePrefab;

    void Start()
    {
        SpawnObstacle();
    }

    void SpawnObstacle()
    {
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(
            obstaclePrefab,
            spawnPoints[randomIndex].position,
            Quaternion.identity,
            transform
        );
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<GroundSpawner>().SpawnTile();
            Destroy(gameObject, 2);
        }
    }
}