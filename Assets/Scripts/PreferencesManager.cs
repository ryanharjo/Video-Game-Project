using UnityEngine;

public class PreferencesManager : MonoBehaviour
{
    private const string MasterKey = "MasterVol";
    private const string MusicKey = "MusicVol";

    
    public static void SetMasterVolume(float value) => PlayerPrefs.SetFloat(MasterKey, value);
    public static float GetMasterVolume() => PlayerPrefs.GetFloat(MasterKey, 0.75f);

    public static void SetMusicVolume(float value) => PlayerPrefs.SetFloat(MusicKey, value);
    public static float GetMusicVolume() => PlayerPrefs.GetFloat(MusicKey, 0.75f);
}

