using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public static PlayerScore instance;

    public int score = 0;
    public Text scoreText; // UI Text (Legacy) or swap to TMP later

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
