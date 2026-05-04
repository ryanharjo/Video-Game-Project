using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private bool canKill = false;
    private bool hasHit = false;

    private void Start()
    {
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
        if (!canKill || hasHit) return;

        if (other.CompareTag("Player"))
        {
            hasHit = true;

            Debug.Log("Player Hit!");

            GameManager.Instance.TakeDamage(GameManager.Instance.obstacleDamage);
        }
    }
}
