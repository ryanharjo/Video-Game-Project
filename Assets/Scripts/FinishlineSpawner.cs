using UnityEngine;

public class FinishlineSpawner : MonoBehaviour
{
    public GameObject finishLinePrefab;
    public Transform player;
    public float spawnDistance = 50f;

    public void SpawnFinishline()
    {
        Vector3 spawnPos = new Vector3(0f, 0f, player.position.z + spawnDistance);

        Instantiate(finishLinePrefab, spawnPos, Quaternion.identity);
    }
}
