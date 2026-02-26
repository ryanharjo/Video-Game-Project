using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

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
        // New Input System check for Pause toggle
        if (pauseAction != null && pauseAction.triggered)
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
