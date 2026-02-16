using System;

[Serializable]
public class SaveData
{
    public int coins;

    public SaveData(Player player)
    {
        coins = player.coins;
    }
}
