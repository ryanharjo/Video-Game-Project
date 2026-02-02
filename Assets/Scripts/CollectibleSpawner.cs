using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public float spawnZ = 30f;
    public float spawnInterval = 2f;
    public float laneOffset = 3f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnCoin();
            timer = 0f;
        }
    }

    void SpawnCoin()
    {
        float[] lanes = { -laneOffset, 0, laneOffset };
        float x = lanes[Random.Range(0, lanes.Length)];

        Vector3 pos = new Vector3(x, 1f, spawnZ);
        Instantiate(coinPrefab, pos, Quaternion.identity);
    }
}
