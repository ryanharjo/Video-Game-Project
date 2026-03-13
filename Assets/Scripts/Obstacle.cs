using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private bool canKill = false; 

    PlayerRunner playerRunner;

    private void Start()
    {
        playerRunner = GameObject.FindFirstObjectByType<PlayerRunner>();
        Invoke(nameof(EnableKill), 0.5f); 
    }

    void EnableKill()
    {
        canKill = true;
    }

    void Update()
    {
        if (transform.position.z < -10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canKill) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over!");

            GameManager.Instance.GameOver();
        }
    }
}
