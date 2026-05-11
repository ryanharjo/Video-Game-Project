using UnityEngine;

public class TokenPickup : MonoBehaviour
{
    public AudioClip collectSound;
    public GameObject collectEffect;
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
            // 1. Spawn the particles at the token's position
            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            // 2. Play Audio
            if (audioSource != null && collectSound != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(collectSound);
            }

            // 3. Update UI/Manager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddToken(tokenValue);
            }

            Destroy(gameObject);
        }
    }
}




