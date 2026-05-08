using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Settings")]
    public float activationDelay = 0.5f; // time before obstacle can kill

    private bool canKill = false;
    private bool hasHit = false;

    private void Start()
    {
        Invoke(nameof(EnableKill), activationDelay);
    }

    private void EnableKill()
    {
        canKill = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canKill || hasHit) return;

        if (other.CompareTag("Player"))
        {
            hasHit = true;

            GameManager.Instance.GameOver();
        }
    }
}
