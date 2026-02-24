using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject gameplayPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject countdownPanel;

    [Header("Countdown Text")]
    public TextMeshProUGUI countdownText;

    private void Awake()
    {
        Instance = this;
    }

    void HideAll()
    {
        gameplayPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        countdownPanel.SetActive(false);
    }

    public void ShowGameplayUI()
    {
        HideAll();
        gameplayPanel.SetActive(true);
    }

    public void ShowPauseUI()
    {
        HideAll();
        pausePanel.SetActive(true);
    }

    public void ShowGameOverUI()
    {
        HideAll();
        gameOverPanel.SetActive(true);
    }

    public void ShowWinUI()
    {
        HideAll();
        winPanel.SetActive(true);
    }

    public void ShowCountdownUI()
    {
        HideAll();
        countdownPanel.SetActive(true);
    }

    public void UpdateCountdownText(string text)
    {
        countdownText.text = text;
    }
}
