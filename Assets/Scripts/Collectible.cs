using UnityEngine;

public class Collectible : MonoBehaviour
{

    public int value = 1;          // How many points this item gives
    public AudioClip collectSound; // Optional sound

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add score
            PlayerScore.instance.AddScore(value);

            // Play sound (optional)
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            // Destroy item
            Destroy(gameObject);
        }
    }
}
