using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile;
    public float tileLength = 10f;   
    public int tilesOnScreen = 15;

    private Vector3 nextSpawnPoint;

    private void Start()
    {
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint += Vector3.forward * tileLength;
    }
}
