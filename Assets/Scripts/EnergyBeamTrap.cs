using UnityEngine;

public class EnergyBeamTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // This makes sure the player STILL dies
        // if the beam turns ON while they are inside it
        if (other.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        if (GameManager.Instance.currentState != GameManager.GameState.GameOver)
        {
            Debug.Log("⚡ Player killed by energy beam");
            GameManager.Instance.GameOver();
        }
    }
}
