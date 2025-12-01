using UnityEngine;
using UnityEngine.SceneManagement;

public class TheOtherGameManager : MonoBehaviour
{
    public static TheOtherGameManager Instance;
    public float surviveTime = 30f;

    private float timer;
    private bool gameActive = true;
    private string message = "";

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!gameActive)
        {
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

       
        timer += Time.deltaTime;
        if (timer >= surviveTime)
        {
            WinGame();
        }
    }

    public void GameOver()
    {
        gameActive = false;
        message = "Game Over! Press R to Restart.";
        StopTime();
    }

    public void WinGame()
    {
        gameActive = false;
        message = "You Win! Press R to Restart.";
        StopTime();
    }

    private void StopTime()
    {
        Time.timeScale = 0f; 
    }

    void OnGUI()
    {
        if (!string.IsNullOrEmpty(message))
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 40;
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(200, 200, 800, 100), message, style);
        }
    }
}
