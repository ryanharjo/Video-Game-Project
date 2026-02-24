using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ---------- GAME STATES ----------
    public enum GameState
    {
        Countdown,
        Playing,
        Paused,
        GameOver,
        LevelComplete
    }

    public GameState currentState;

    // ---------- SCORE ----------
    [Header("Score & Goals")]
    public int score = 0;
    public int highScore = 0;
    public int coinsNeededToWin = 50;

    [Header("Countdown Settings")]
    public float countdownTime = 3f;

    private void Awake()
    {
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
        ChangeState(GameState.Countdown);
    }

    // ---------- STATE MANAGER ----------
    public void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Countdown:
                StartCoroutine(StartCountdown());
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                UIManager.Instance.ShowGameplayUI();
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                UIManager.Instance.ShowPauseUI();
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                UIManager.Instance.ShowGameOverUI();
                break;

            case GameState.LevelComplete:
                Time.timeScale = 0f;
                UIManager.Instance.ShowWinUI();
                break;
        }
    }

    // ---------- COUNTDOWN ----------
    IEnumerator StartCountdown()
    {
        UIManager.Instance.ShowCountdownUI();

        float timer = countdownTime;

        while (timer > 0)
        {
            UIManager.Instance.UpdateCountdownText(Mathf.Ceil(timer).ToString());
            yield return new WaitForSeconds(1f);
            timer--;
        }

        UIManager.Instance.UpdateCountdownText("GO!");
        yield return new WaitForSeconds(1f);

        ChangeState(GameState.Playing);
    }

    // ---------- SCORE ----------
    public void AddScore(int amount)
    {
        if (currentState != GameState.Playing)
            return;

        score += amount;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (score >= coinsNeededToWin)
        {
            ChangeState(GameState.LevelComplete);
        }
    }

    // ---------- PUBLIC CONTROLS ----------
    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    public void TogglePause()
    {
        if (currentState == GameState.Playing)
            ChangeState(GameState.Paused);
        else if (currentState == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
