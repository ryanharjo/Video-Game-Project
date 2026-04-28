using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Timeline.DirectorControlPlayable;

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

    [Header("Tokens & Goals")]
    public int tokens = 0;
    public int highScore = 0;
    public int targetTokens = 50;

    [Header("Countdown Settings")]
    public float countdownTime = 3f;

    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction pauseAction;

    private Coroutine countdownCoroutine;

    public enum WinCondition
    {
        Tokens,
        ReachEnd,
        SurviveTime
    }

    [Header("Win Condition")]
    public WinCondition winCondition;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Get pause input action
        if (inputActions != null)
        {
            InputActionMap uiMap = inputActions.FindActionMap("UI");

            if (uiMap != null)
            {
                pauseAction = uiMap.FindAction("Pause");
            }
        }
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed += OnPausePressed;
            pauseAction.Enable();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePressed;
            pauseAction.Disable();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("High Score", 0);
        InitializeLevel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        highScore = PlayerPrefs.GetInt("High Score", 0);

        if (UIManager.Instance == null)
        {
            Debug.Log("Waiting for UIManager...");
        }

        InitializeLevel();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    private void TogglePause()
    {
        // Only allow pause during gameplay
        if (currentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    private void InitializeLevel()
    {
        Time.timeScale = 1f;
        tokens = 0;

        UIManager.Instance?.UpdateTokenText(tokens);
        UIManager.Instance?.UpdateHighScoreText(highScore);

        ChangeState(GameState.Countdown);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        if (currentState == GameState.LevelComplete)
        {
            SaveHighScore();
        }

        if (UIManager.Instance == null)
        {
            Debug.LogWarning("UIManager not found yet.");
            return;
        }

        switch (currentState)
        {
            case GameState.Countdown:

                Time.timeScale = 1f;

                if (countdownCoroutine != null)
                {
                    StopCoroutine(countdownCoroutine);
                }

                countdownCoroutine = StartCoroutine(StartCountdown());
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

    private IEnumerator StartCountdown()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowCountdownUI();
        }

        float timer = countdownTime;

        while (timer > 0)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateCountdownText(Mathf.Ceil(timer).ToString());
            }

            yield return new WaitForSecondsRealtime(1f);
            timer--;
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCountdownText("GO!");
        }

        yield return new WaitForSecondsRealtime(0.5f);

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
            UIManager.Instance?.UpdateHighScoreText(highScore);
        }

        if (winCondition == WinCondition.Tokens && tokens >= targetTokens)
        {
            ChangeState(GameState.LevelComplete);
        }
    }

    public void GameOver()
    {
        if (currentState != GameState.Playing) return;

        SaveHighScore();
        ChangeState(GameState.GameOver);
    }

    private void SaveHighScore()
    {
        if (tokens > highScore)
        {
            highScore = tokens;
            PlayerPrefs.SetInt("High Score", highScore);
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("High Score");
        highScore = 0;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHighScoreText(highScore);
        }

        Debug.Log("High Score has been reset!");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        currentState = GameState.Countdown;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}  

