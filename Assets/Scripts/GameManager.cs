using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Countdown,
        Playing,
        Paused,
        GameOver,
        LevelComplete
    }

    [Header("State")]
    public GameState currentState;

    [Header("Coins & Goals")]
    public int coins = 0;
    public int highScore = 0;
    public int coinsNeededToWin = 50;

    [Header("Countdown Settings")]
    public float countdownTime = 3f;

    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction pauseAction;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Setup Input Action for Pausing
            // Assumes you have an Action Map named "UI" and an Action named "Pause"
            pauseAction = inputActions.FindActionMap("UI").FindAction("Pause");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() => pauseAction?.Enable();
    private void OnDisable() => pauseAction?.Disable();

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        ChangeState(GameState.Countdown);
    }

    void Update()
    {
        // FIXED: Using New Input System for the ESC key
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (currentState == GameState.Playing)
            ChangeState(GameState.Paused);
        else if (currentState == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    // ---------- STATE HANDLER ----------
    public void ChangeState(GameState newState)
    {
        currentState = newState;

        // Safety check: ensure UIManager exists before calling it
        if (UIManager.Instance == null) return;

        switch (currentState)
        {
            case GameState.Countdown:
                StartCoroutine(StartCountdown());
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                UIManager.Instance.HideCountdownUI();
                // Ensure UI is closed if resuming
                UIManager.Instance.HidePauseUI();
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
    }

    // ---------- COUNTDOWN ----------
    IEnumerator StartCountdown()
    {
        if (UIManager.Instance != null) UIManager.Instance.ShowCountdownUI();

        float timer = countdownTime;

        while (timer > 0)
        {
            if (UIManager.Instance != null)
                UIManager.Instance.UpdateCountdownText(Mathf.Ceil(timer).ToString());

            yield return new WaitForSeconds(1f);
            timer--;
        }

        if (UIManager.Instance != null) UIManager.Instance.UpdateCountdownText("GO!");
        yield return new WaitForSeconds(1f);

        ChangeState(GameState.Playing);
    }

    // ---------- COINS ----------
    public void AddCoin(int amount)
    {
        if (currentState != GameState.Playing)
            return;

        coins += amount;
        if (UIManager.Instance != null) UIManager.Instance.UpdateCoinText(coins);

        if (coins > highScore)
        {
            highScore = coins;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (coins >= coinsNeededToWin)
        {
            ChangeState(GameState.LevelComplete);
        }
    }

    // ---------- BUTTON FUNCTIONS ----------
    public void ResumeGame()
    {
        ChangeState(GameState.Playing);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        coins = 0;
        // Corrected: Uses the SceneManagement namespace
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }
}
