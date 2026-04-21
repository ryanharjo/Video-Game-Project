using UnityEngine;

public class GroundTile : MonoBehaviour
{
    void Start()
    {
        // Obstacle spawning logic has been removed
    }

    private void OnTriggerExit(Collider other)
    {
        // Triggers when the player moves off the current tile
        if (other.CompareTag("Player"))
        {
            // Request the spawner to create a new tile at the end of the chain
            FindFirstObjectByType<GroundSpawner>().SpawnTile();

            // Self-destruct after 2 seconds to free up memory
            Destroy(gameObject, 2f);
        }
    }
}