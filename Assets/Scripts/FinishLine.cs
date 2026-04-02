using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var movement = other.GetComponent<PlayerRunner>();
            if (movement != null)
                movement.enabled = false;

            // 2. Disable physics/collisions
            CharacterController playerController = other.GetComponent<CharacterController>();
            if (playerController != null)
                playerController.enabled = false;

            // 3. Stop ground spawning
            GroundSpawner spawner = FindFirstObjectByType<GroundSpawner>();
            if (spawner != null)
                spawner.enabled = false;

            // 4. Trigger Win State
            GameManager.Instance.ChangeState(GameManager.GameState.LevelComplete);
        }
    }
}
