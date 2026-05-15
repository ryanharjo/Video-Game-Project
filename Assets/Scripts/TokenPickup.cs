using UnityEngine;

public class TokenPickup : MonoBehaviour
{
    [Header("Effects")]
    public AudioClip collectSound;
    public GameObject collectEffect;

    [Header("Settings")]
    public int tokenValue = 10;
    public float disableDelay = 0.05f;

    private AudioSource audioSource;

    void Start()
    {
        // Cache audio source
        if (Camera.main != null)
        {
            audioSource = Camera.main.GetComponent<AudioSource>();
        }
    }

    void OnEnable()
    {
        // Reset token when reused from pool
        gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Prevent double collect
        GetComponent<Collider>().enabled = false;

        // Spawn effect
        if (collectEffect != null)
        {
            GameObject effect =
                Instantiate(collectEffect, transform.position, Quaternion.identity);

            Destroy(effect, 2f);
        }

        // Play sound
        if (audioSource != null && collectSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(collectSound);
        }

        // Add tokens
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddToken(tokenValue);
        }

        // Disable instead of destroy (POOLING)
        Invoke(nameof(DisableToken), disableDelay);
    }

    void DisableToken()
    {
        GetComponent<Collider>().enabled = true;
        gameObject.SetActive(false);
    }
}




