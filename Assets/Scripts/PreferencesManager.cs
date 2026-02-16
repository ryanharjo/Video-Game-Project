using UnityEngine;

public class PreferencesManager : MonoBehaviour
{
    private const string MasterKey = "MasterVol";
    private const string MusicKey = "MusicVol";

    /// <summary> Saves the Master Volume to PlayerPrefs </summary>
    public static void SetMasterVolume(float soundLevel) => PlayerPrefs.SetFloat(MasterKey, soundLevel);

    public static float GetMasterVolume() => PlayerPrefs.GetFloat(MasterKey, 0f); // Default 0dB

    /// <summary> Saves the Music Volume to PlayerPrefs </summary>
    public static void SetMusicVolume(float soundLevel) => PlayerPrefs.SetFloat(MusicKey, soundLevel);

    public static float GetMusicVolume() => PlayerPrefs.GetFloat(MusicKey, 0f);
}

