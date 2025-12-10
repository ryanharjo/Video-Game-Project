using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public float scorePerSecond = 1f;
    public float speedIncreaseInterval = 10f;
    public float speedIncreaseAmount = 0.5f;

    private float score = 0f;
    private float speedTimer = 0f;
    private bool isGameOver = false;
    private PlayerRunner player;
    private ObstacleSpawner spawner;

    void Start()
    {
        player = FindObjectOfType<PlayerRunner>();
        spawner = FindObjectOfType<ObstacleSpawner>();
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;
        score += scorePerSecond * Time.deltaTime;
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();

        speedTimer += Time.deltaTime;
        if (speedTimer >= speedIncreaseInterval)
        {
            player.forwardSpeed += speedIncreaseAmount;
            speedTimer = 0f;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        spawner?.StopSpawning();
        gameOverPanel.SetActive(true);
        // disable player control
        if (player != null)
            player.enabled = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // If using ground tile recycling:
    public GameObject groundTilePrefab;
    public Transform groundSpawnPoint;
    public void SpawnNextTile()
    {
        Instantiate(groundTilePrefab, groundSpawnPoint.position, Quaternion.identity);
    }
}
