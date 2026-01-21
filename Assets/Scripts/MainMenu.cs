using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "New Scene";

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject creditsPanel;
    public GameObject settingsPanel;

    // ===== BUTTON FUNCTIONS =====

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenCredits()
    {
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void Back()
    {
        ShowMain();
    }

    // ===== HELPERS =====
    void ShowMain()
    {
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
