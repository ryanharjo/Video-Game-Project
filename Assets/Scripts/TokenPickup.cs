using UnityEngine;

public class TokenPickup : MonoBehaviour
{
    public AudioClip collectSound;
    public int tokenValue = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play sound
            AudioSource audio = Camera.main.GetComponent<AudioSource>();
            audio.pitch = Random.Range(0.9f, 1.1f);
            audio.PlayOneShot(collectSound);

            // Add tokens
            GameManager.Instance.AddToken(tokenValue);

            // Destroy token
            Destroy(gameObject);
        }
    }
}
