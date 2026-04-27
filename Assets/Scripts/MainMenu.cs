using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "Level 1";

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject creditsPanel;
    public GameObject settingsPanel;

    [Header("UI Elements")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        ShowMain();
        SetupSliders();
    }

    private void SetupSliders()
    {
       
        float masterVal = PreferencesManager.GetMasterVolume();
        float musicVal = PreferencesManager.GetMusicVolume();

        
        masterSlider.value = masterVal;
        musicSlider.value = musicVal;

        
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMasterVolume(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.ChangeMasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.ChangeMusicVolume(value);
    }

    // --- Navigation Logic ---
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenCredits() => SwitchPanel(creditsPanel);
    public void OpenSettings() => SwitchPanel(settingsPanel);
    public void ShowMain() => SwitchPanel(mainPanel);

    private void SwitchPanel(GameObject activePanel)
    {
        mainPanel.SetActive(activePanel == mainPanel);
        creditsPanel.SetActive(activePanel == creditsPanel);
        settingsPanel.SetActive(activePanel == settingsPanel);
    }
}
