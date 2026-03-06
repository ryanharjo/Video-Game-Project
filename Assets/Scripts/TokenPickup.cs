using UnityEngine;

public class TokenPickup : MonoBehaviour
{
    public int tokenValue = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddToken(tokenValue);
            Destroy(gameObject);
        }
    }
}
