using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    [Header("Tile Settings")]
    public GameObject[] tilePrefabs; 
    public float tileLength = 10f;   
    public int tilesOnScreen = 15;    

    [Header("References")]
    public Transform playerTransform; 

    private List<GameObject> activeTiles = new List<GameObject>();
    private float nextSpawnZ = 0f;

    void Start()
    {
       
        for (int i = 0; i < tilesOnScreen; i++)
        {
            
            if (i < 2)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0, tilePrefabs.Length));
        }
    }

    void Update()
    {
       
        if (playerTransform.position.z - 35 > (nextSpawnZ - tilesOnScreen * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteOldTile();
        }
    }

    public void SpawnTile(int prefabIndex)
    {
        GameObject go = Instantiate(tilePrefabs[prefabIndex], transform.forward * nextSpawnZ, Quaternion.identity);

        
        go.GetComponent<GroundTile>().groundSpawner = this;

        activeTiles.Add(go);
        nextSpawnZ += tileLength;
    }

    private void DeleteOldTile()
    {
        
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
