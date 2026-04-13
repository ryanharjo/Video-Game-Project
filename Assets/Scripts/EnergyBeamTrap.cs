using UnityEngine;

public class EnergyBeamTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit → Game Over");

            GameManager.Instance.GameOver();
        }
    }
}
