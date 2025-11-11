using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;
    public Text throwsText;
    public Image cooldownImage; 
    [HideInInspector] public bool isPaused = false;

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void UpdateThrowsUI(int throwsLeft)
    {
        if (throwsText != null) throwsText.text = "Throws: " + throwsLeft;
    }

    public void ShowCooldown(float duration)
    {
        if (cooldownImage == null) return;
        StopAllCoroutines();
        StartCoroutine(CooldownFill(duration));
    }

    System.Collections.IEnumerator CooldownFill(float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cooldownImage.fillAmount = Mathf.Clamp01(t / duration);
            yield return null;
        }
        cooldownImage.fillAmount = 0f;
    }
}