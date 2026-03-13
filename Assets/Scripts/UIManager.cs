using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Gameplay UI")]
    public TextMeshProUGUI tokenText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI highScoreText; // Added to show the saved high score

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private void Awake()
    {
        // Standard singleton assignment for scene-based UI
        Instance = this;
    }

    void Start()
    {
        HideAllPanels();

        // Initialize text from GameManager values if they exist
        if (GameManager.Instance != null)
        {
            UpdateTokenText(GameManager.Instance.tokens);
            if (highScoreText != null)
                highScoreText.text = "High Score: " + GameManager.Instance.highScore;
        }
    }

    public void HideAllPanels()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    // ----------------- TOKENS -----------------
    public void UpdateTokenText(int tokens)
    {
        if (tokenText != null)
            tokenText.text = $"Tokens: {tokens}";
    }

    // ----------------- COUNTDOWN -----------------
    public void ShowCountdownUI() => countdownText?.gameObject.SetActive(true);
    public void HideCountdownUI() => countdownText?.gameObject.SetActive(false);

    public void UpdateCountdownText(string value)
    {
        if (countdownText != null)
            countdownText.text = value;
    }

    // ----------------- PANEL CONTROLS -----------------
    public void ShowPauseUI() => pausePanel?.SetActive(true);
    public void HidePauseUI() => pausePanel?.SetActive(false);

    public void ShowGameOverUI() => gameOverPanel?.SetActive(true);
    public void ShowWinUI() => winPanel?.SetActive(true);
}
