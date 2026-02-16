using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider masterSlider;
    public Slider musicSlider;
    public TextMeshProUGUI coinDisplay; // Drag your Coin Text here in the Inspector

    void Start()
    {
        // 1. SETTINGS (Volume)
        masterSlider.value = PreferencesManager.GetMasterVolume();
        musicSlider.value = PreferencesManager.GetMusicVolume();

        masterSlider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeMasterVolume(val));
        musicSlider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeMusicVolume(val));

        // 2. PROGRESS (Coins)
        LoadPlayerData();
    }

    void LoadPlayerData()
    {
        SaveData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            // Update the UI with the saved coin count
            coinDisplay.text = "Coins: " + data.coins;

            // If you have a Player script in the scene, update its value too
            if (Player.Instance != null)
            {
                Player.Instance.coins = data.coins;
            }
        }
        else
        {
            coinDisplay.text = "Coins: 0";
        }
    }
}
