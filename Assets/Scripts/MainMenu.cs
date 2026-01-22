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

<<<<<<< HEAD
    
=======
    private void Start()
    {
        // Ensures the menu starts in the right state
        ShowMain();
    }

    // ===== BUTTON FUNCTIONS =====
>>>>>>> 190ad1d0becc5f92732e031bb6ae940b93d67d97

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

<<<<<<< HEAD
    
=======
    public void QuitGame()
    {
        Debug.Log("Game Exiting..."); // Just to see it working in the editor
        Application.Quit();
    }

    // ===== HELPERS =====
>>>>>>> 190ad1d0becc5f92732e031bb6ae940b93d67d97
    void ShowMain()
    {
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
