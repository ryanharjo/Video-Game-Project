using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Countdown, Playing, Paused, GameOver, LevelComplete }

    [Header("State")]
    public GameState currentState;

    [Header("Tokens & Goals")]
    public int tokens = 0;
    public int highScore = 0;
    public int tokensNeededToWin = 50;

    [Header("Countdown Settings")]
    public float countdownTime = 3f;

    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction pauseAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize Input
            if (inputActions != null)
            {
                pauseAction = inputActions.FindActionMap("UI").FindAction("Pause");
                // Subscribe to the performed event instead of checking in Update
                pauseAction.performed += ctx => TogglePause();
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable() => pauseAction?.Enable();
    private void OnDisable() => pauseAction?.Disable();

    void Start()
    {
        highScore = PlayerPrefs.GetInt("High Score", 0);
        InitializeLevel();
    }

    // Call this whenever a new level starts to reset local variables
    private void InitializeLevel()
    {
        tokens = 0;
        ChangeState(GameState.Countdown);
    }

    private void TogglePause()
    {
        if (currentState == GameState.Playing) ChangeState(GameState.Paused);
        else if (currentState == GameState.Paused) ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        // Safety check: Ensure UIManager is found if the scene just changed
        if (UIManager.Instance == null) return;

        switch (currentState)
        {
            case GameState.Countdown:
                Time.timeScale = 1f; // Ensure time is moving for coroutines
                StartCoroutine(StartCountdown());
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                UIManager.Instance.HideCountdownUI();
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

    IEnumerator StartCountdown()
    {
        if (UIManager.Instance != null) UIManager.Instance.ShowCountdownUI();

        float timer = countdownTime;
        while (timer > 0)
        {
            if (UIManager.Instance != null)
                UIManager.Instance.UpdateCountdownText(Mathf.Ceil(timer).ToString());

            // Use WaitForSecondsRealtime if you ever pause during countdown
            yield return new WaitForSeconds(1f);
            timer--;
        }

        if (UIManager.Instance != null) UIManager.Instance.UpdateCountdownText("GO!");
        yield return new WaitForSeconds(0.5f);

        ChangeState(GameState.Playing);
    }

    public void AddToken(int amount)
    {
        if (currentState != GameState.Playing) return;

        tokens += amount;
        UIManager.Instance?.UpdateTokenText(tokens);

        if (tokens > highScore)
        {
            highScore = tokens;
            PlayerPrefs.SetInt("High Score", highScore);
        }

        if (tokens >= tokensNeededToWin) ChangeState(GameState.LevelComplete);
    }

    public void RestartGame()
    {
        // Resetting variables before load
        tokens = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Note: ChangeState(Countdown) will be called by Start() in the new scene
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
