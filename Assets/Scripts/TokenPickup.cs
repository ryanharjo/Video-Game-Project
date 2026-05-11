using UnityEngine;

public class TokenPickup : MonoBehaviour
{
    public AudioClip collectSound;
    public int tokenValue = 10;

    private AudioSource audioSource;

    void Start()
    {
        // Cache the audio source on the camera once to save performance
        if (Camera.main != null)
        {
            audioSource = Camera.main.GetComponent<AudioSource>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        if (audioSource != null && collectSound != null)
        {
            // The pitch variance you added is great for rows of tokens!
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(collectSound);
        }

        // Check if GameManager exists before calling it to prevent console errors
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddToken(tokenValue);
        }

        // Destroy the token immediately
        Destroy(gameObject);
    }
}
