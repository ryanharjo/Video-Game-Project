using UnityEngine;

public class EnergyBeamTrap : MonoBehaviour
{
    public Animator playerAnimator;

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
            playerAnimator.SetTrigger("Die");
            Invoke("DelayedGameOver", 2f);
            Debug.Log("⚡ Player killed by energy beam");
            GameManager.Instance.GameOver();
        }
    }

    void DelayedGameOver()
    {
        GameManager.Instance.GameOver();
    }
}
