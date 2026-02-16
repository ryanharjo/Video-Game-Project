using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int coins;

    private void Awake()
    {
        // This logic ensures there is only ever one Player object
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps player data when changing scenes
        }
        else
        {
            Destroy(gameObject); // Destroys duplicates
        }
    }

    // These methods allow you to trigger saves/loads easily
    public void SaveGame()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            coins = data.coins;
        }
    }
}
