using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTilePrefab;
    public Transform player;

    private float tileLength = 30f;
    private float spawnZ = 0;
    private int tilesOnScreen = 5;

    void Start()
    {
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (player.position.z > spawnZ - (tilesOnScreen * tileLength))
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        GameObject tile = Instantiate(
            groundTilePrefab,
            Vector3.forward * spawnZ,
            Quaternion.identity
        );

        spawnZ += tileLength;
    }
}
