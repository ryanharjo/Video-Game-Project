using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop player movement
            CharacterController playerController = other.GetComponent<CharacterController>();
            if (playerController != null)
                playerController.enabled = false;

            // Stop ground spawning
            GroundSpawner spawner = FindFirstObjectByType<GroundSpawner>();
            if (spawner != null)
                spawner.enabled = false;

            // Change game state to LevelComplete (this also shows win UI)
            GameManager.Instance.ChangeState(GameManager.GameState.LevelComplete);
        }
    }
}
