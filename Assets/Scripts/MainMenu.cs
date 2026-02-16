using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "New Scene";

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject creditsPanel;
    public GameObject settingsPanel;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;

    // Keys for saving/loading
    private const string MASTER_KEY = "MasterVolValue";
    private const string MUSIC_KEY = "MusicVolValue";

    private void Start()
    {
        ShowMain();
        LoadAudioSettings(); // Load volume from PlayerPrefs on start
    }

    // --- Audio Logic ---

    private void LoadAudioSettings()
    {
        // Get saved values (default to 0.75 if they don't exist yet)
        float masterVal = PlayerPrefs.GetFloat(MASTER_KEY, 0.75f);
        float musicVal = PlayerPrefs.GetFloat(MUSIC_KEY, 0.75f);

        // Update Slider UI
        masterSlider.value = masterVal;
        musicSlider.value = musicVal;

        // Apply values to the Mixer
        SetMasterVolume(masterVal);
        SetMusicVolume(musicVal);
    }

    public void SetMasterVolume(float value)
    {
        // Logarithmic conversion for natural sound attenuation
        mainMixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(MASTER_KEY, value);
    }

    public void SetMusicVolume(float value)
    {
        mainMixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
    }

    // --- Navigation Logic ---

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

    void ShowMain()
    {
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
