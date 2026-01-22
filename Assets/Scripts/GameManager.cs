using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool isGameOver = false;
    public bool isPaused = false;

    [Header("Score")]
    public int score = 0;
    public int highScore = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // ---------- SCORE ----------
    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    // ---------- GAME STATES ----------
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        Debug.Log("Game Over");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        score = 0;
        isGameOver = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // ---------- PAUSE ----------
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }
}
