using UnityEngine;

public class Ground : MonoBehaviour
{
    public float tileLength = 30f;
    public Transform spawnPoint;
    private GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        // When player leaves a tile, spawn/recycle it
        if (other.CompareTag("Player"))
        {
            gm.SpawnNextTile();
            Destroy(gameObject, 2f);
        }
    }
}
