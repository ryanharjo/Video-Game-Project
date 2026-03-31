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
    public int targetTokens = 50;

    [Header("Countdown Settings")]
    public float countdownTime = 3f;

    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction pauseAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        
        if (inputActions != null)
        {
            pauseAction = inputActions.FindActionMap("UI").FindAction("Pause");
            pauseAction.performed += ctx => TogglePause();
        }
    }

    private void OnEnable() => pauseAction?.Enable();
    private void OnDisable() => pauseAction?.Disable();

    void Start()
    {
        highScore = PlayerPrefs.GetInt("High Score", 0);
        InitializeLevel();
    }

    
    private void InitializeLevel()
    {
        Time.timeScale = 1f;   
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

        
        if (UIManager.Instance == null)
        {
            Debug.LogWarning("UIManager not found yet.");
            return;
        }

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

        // Spawn finish line if token goal reached, but DO NOT complete level yet
        if (tokens >= targetTokens)
        {
            GroundSpawner spawner = FindFirstObjectByType<GroundSpawner>();
            if (spawner != null)
                spawner.SpawnTile(); // This will spawn the finish line
        }
    }

    public void GameOver()
    {
        if (currentState != GameState.Playing) return;

        ChangeState(GameState.GameOver);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        tokens = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
