using UnityEngine;

public class PlayerRunner : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float horizontalSpeed = 10f;

    void Update()
    {
        // 1. Move Forward constantly
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 2. Move Left/Right based on Input
        float xInput = Input.GetAxis("Horizontal"); // Returns -1 (Left) to 1 (Right)
        transform.Translate(Vector3.right * xInput * horizontalSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            // Simple way to pause the game
            Time.timeScale = 0;

            // Optional: Reload the scene to restart
            // UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 500f);
    }
}
