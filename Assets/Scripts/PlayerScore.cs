using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public static PlayerScore instance;

    public int score = 0;
    public TMP_Text scoreText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + score;
    }
}
