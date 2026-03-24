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
            LoadSettings();
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
        // Convert 0-1 to Decibels
        float dB = soundLevel > 0 ? Mathf.Log10(soundLevel) * 20 : -80f;
        masterMixer.SetFloat("MasterVol", dB);
        PreferencesManager.SetMasterVolume(soundLevel);
    }

    public void ChangeMusicVolume(float soundLevel)
    {
        float dB = soundLevel > 0 ? Mathf.Log10(soundLevel) * 20 : -80f;
        masterMixer.SetFloat("MusicVol", dB);
        PreferencesManager.SetMusicVolume(soundLevel);
    }
}
