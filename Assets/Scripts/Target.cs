using UnityEngine;

public class Target : MonoBehaviour
{
    public int scoreValue = 10;
    public bool destroyAxeOnHit = false;
    public AudioClip hitSound;
    AudioSource audioSource;
    public Renderer targetRenderer;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (targetRenderer == null) targetRenderer = GetComponent<Renderer>();
    }

    
    void OnHitByAxe(object infoObj)
    {
        
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.AddScore(scoreValue);

        
        if (hitSound != null) audioSource.PlayOneShot(hitSound);

        
        if (targetRenderer != null) StartCoroutine(Flash());
    }

    System.Collections.IEnumerator Flash()
    {
        Color orig = targetRenderer.material.color;
        targetRenderer.material.color = Color.yellow;
        yield return new WaitForSeconds(0.12f);
        targetRenderer.material.color = orig;
    }
}

