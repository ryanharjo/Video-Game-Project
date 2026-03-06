using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Gameplay UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI tokenText;
    public TextMeshProUGUI countdownText;

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private void Awake()
    {
        Instance = this;
    }

    // ----------------- TOKENS -----------------
    public void UpdateTokenText(int tokens)
    {
        tokenText.text = "Tokens: " + tokens;
    }

    // ----------------- COUNTDOWN -----------------
    public void ShowCountdownUI()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(true);
    }

    public void HideCountdownUI()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    public void UpdateCountdownText(string value)
    {
        if (countdownText != null)
            countdownText.text = value;
    }

    // ----------------- PAUSE -----------------
    public void ShowPauseUI()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void HidePauseUI()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    // ----------------- GAME OVER -----------------
    public void ShowGameOverUI()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // ----------------- WIN -----------------
    public void ShowWinUI()
    {
        if (winPanel != null)
            winPanel.SetActive(true);
    }
}
