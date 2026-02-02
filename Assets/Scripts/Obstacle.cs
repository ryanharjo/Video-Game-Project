using UnityEngine;

public class Obstacle : MonoBehaviour
{
    PlayerRunner playerRunner;

    private void Start()
    {
        playerRunner = GameObject.FindFirstObjectByType<PlayerRunner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            Time.timeScale = 0f;
        }
    }
}
