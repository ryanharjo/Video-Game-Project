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

    public float surviveDuration = 60f;
    private float timer;

    [Header("Countdown Settings")]
    public float countdownTime = 3f;

    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction pauseAction;
    private bool finishSpawned = false;

    private Coroutine countdownCoroutine;

    public enum WinCondition
    {
        Tokens,
        ReachEnd,
        SurviveTime
    }

    [Header("Win Condition")]
    public WinCondition winCondition;

    private void Update()
    {
        if (currentState == GameState.Playing && winCondition == WinCondition.SurviveTime)
        {
            timer += Time.deltaTime;

            if (timer >= surviveDuration)
            {
                ChangeState(GameState.LevelComplete);
            }
        }
    }

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

    private void ResetLevelState()
    {
        tokens = 0;
        timer = 0f;
        finishSpawned = false;
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
        StartCoroutine(DelayedInitialize());
    }

    private IEnumerator DelayedInitialize()
    {
        while (UIManager.Instance == null)
        {
            yield return null;
        }

        highScore = PlayerPrefs.GetInt("High Score", 0);
        InitializeLevel();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        Debug.Log("Current State: " + currentState);
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

    private void SaveHighScore()
    {
        if (tokens > highScore)
        {
            highScore = tokens;
            PlayerPrefs.SetInt("High Score", highScore);
            PlayerPrefs.Save();
        }
    }

    private void InitializeLevel()
    {
        Time.timeScale = 1f;
        ResetLevelState();
        UIManager.Instance?.UpdateHighScoreText(highScore);
        UIManager.Instance?.UpdateTokenText(tokens,targetTokens);

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

                Time.timeScale = 0f;

                if (countdownCoroutine != null)
                {
                    StopCoroutine(countdownCoroutine);
                }

                countdownCoroutine = StartCoroutine(StartCountdown());
                break;

            case GameState.Playing:

                Time.timeScale = 1f;

                if (UIManager.Instance != null)
                {
                    UIManager.Instance.HideCountdownUI();
                    UIManager.Instance.HidePauseUI();

                    // Force-hide the panel
                    if (UIManager.Instance.pausePanel != null)
                    {
                        UIManager.Instance.pausePanel.SetActive(false);
                    }
                }
                    break;

            case GameState.Paused:

                Time.timeScale = 0f;
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowPauseUI();

                    if (UIManager.Instance.pausePanel != null)
                    {
                        UIManager.Instance.pausePanel.SetActive(true);
                    }
                }
                break;

            case GameState.GameOver:

                StartCoroutine(GameOverRoutine());
                break;

            case GameState.LevelComplete:

                Time.timeScale = 0f;
                UIManager.Instance.ShowWinUI();
                break;
        }
    }

    IEnumerator GameOverRoutine()
    {
        Debug.Log("Game Over");
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 0f;
        UIManager.Instance?.ShowGameOverUI();
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
        UIManager.Instance?.UpdateTokenText(tokens, targetTokens);
        if (tokens > highScore)
        {
            highScore = tokens;
            PlayerPrefs.SetInt("High Score", highScore);
            UIManager.Instance?.UpdateHighScoreText(highScore);
        }

        if (winCondition == WinCondition.Tokens && tokens >= targetTokens && !finishSpawned)
        {
            finishSpawned = true;

            Object.FindFirstObjectByType<FinishlineSpawner>()?.SpawnFinishline();
        }
    }

    

    public void GameOver()
    {
        if (currentState != GameState.Playing) return;

        SaveHighScore();
        ChangeState(GameState.GameOver);
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
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}  

