using UnityEngine;

public class Collectible : MonoBehaviour
{

    public int value = 10;          // How many points this item gives
    public AudioClip collectSound; // Optional sound

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Safety check
        if (PlayerScore.instance != null)
        {
            PlayerScore.instance.AddScore(value);
        }
        else
        {
            Debug.LogError("PlayerScore instance is missing in the scene!");
        }

        // Play sound
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // Destroy collectible
        Destroy(gameObject);
    }
}
