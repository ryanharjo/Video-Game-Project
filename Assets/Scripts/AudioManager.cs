using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer masterMixer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings(); // Apply saved volumes on startup
        }
        else { Destroy(gameObject); }
    }

    private void LoadSettings()
    {
        ChangeMasterVolume(PreferencesManager.GetMasterVolume());
        ChangeMusicVolume(PreferencesManager.GetMusicVolume());
    }

    public void ChangeMasterVolume(float soundLevel)
    {
        masterMixer.SetFloat("MasterVol", soundLevel);
        PreferencesManager.SetMasterVolume(soundLevel);
    }

    public void ChangeMusicVolume(float soundLevel)
    {
        masterMixer.SetFloat("MusicVol", soundLevel);
        PreferencesManager.SetMusicVolume(soundLevel);
    }
}
