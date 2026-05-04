using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Health UI")]
    public Slider healthBar;

    [Header("Gameplay UI")]
    public TextMeshProUGUI tokenText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI highScoreText; 

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject countdownPanel;

    private void Awake()
    {
        Instance = this;
        HideAllPanels();
    }

    public void UpdateHealthBar(int current, int max)
    {
        if (healthBar == null) return;

        healthBar.maxValue = max;
        healthBar.value = current;
    }

    void Start()
    {    
        if (GameManager.Instance != null)
        {
            UpdateTokenText(GameManager.Instance.tokens);
            UpdateHighScoreText(GameManager.Instance.highScore);
        }
    }

    public void HideAllPanels()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        GameManager.Instance?.RestartGame();
    }

    public void LoadMainMenu()
    {
        GameManager.Instance?.LoadMainMenu();
    }

    public void UpdateHighScoreText(int score)
    {
        if (highScoreText != null) highScoreText.text = "High Score: " + score;
    }

    // ----------------- TOKENS -----------------
    public void UpdateTokenText(int tokens)
    {
        if (tokenText != null) tokenText.text = $"Tokens: {tokens}";
    }

    // ----------------- COUNTDOWN -----------------
    public void ShowCountdownUI()
    {
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(true);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }
           
    }

    public void HideCountdownUI()
    {
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false);
        }
    }

    public void UpdateCountdownText(string value)
    {
        if (countdownText != null) countdownText.text = value;
    }

    // ----------------- PANEL CONTROLS -----------------
    public void ShowPauseUI() => pausePanel?.SetActive(true);
    public void HidePauseUI() => pausePanel?.SetActive(false);

    public void ShowGameOverUI() => gameOverPanel?.SetActive(true);

    public void ShowWinUI() => winPanel?.SetActive(true);
}
