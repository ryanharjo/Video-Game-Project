using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public int targetTokens = 50;
    public float countdownTime = 3f;

    private void Start()
    {
        GameManager.Instance.targetTokens = targetTokens;
        GameManager.Instance.countdownTime = countdownTime;
    }
}
