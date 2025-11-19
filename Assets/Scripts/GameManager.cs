using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    public float timeLeft = 60f;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timeLeft);

        if (timeLeft <= 0)
        {
            Time.timeScale = 0;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Coins: " + score;
    }
}