using UnityEngine;

public class TokenPickup : MonoBehaviour
{
    public AudioClip collectSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource audio = Camera.main.GetComponent<AudioSource>();

            audio.pitch = Random.Range(0.9f, 1.1f);
            audio.PlayOneShot(collectSound);

            Destroy(gameObject);
        }
    }
}
