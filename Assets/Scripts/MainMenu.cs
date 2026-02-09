using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "New Scene";

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject creditsPanel;
    public GameObject settingsPanel;


    private void Start()
    {
        // Ensures the menu starts in the right state
        ShowMain();
    }

  

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


    public void QuitGame()
    {
        Debug.Log("Game Exiting..."); // Just to see it working in the editor
        Application.Quit();
    }

  
    void ShowMain()
    {
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
