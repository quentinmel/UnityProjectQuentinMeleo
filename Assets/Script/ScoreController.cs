using UnityEngine;
using TMPro;
using System;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private bool isScoreCounting = true;
    public static float gameTime = 0f;

    void Update()
    {
        TimeManager();
        if (isScoreCounting)
        {
            GameManager.PlayerScore = Mathf.FloorToInt(gameTime);
            scoreText.text = "Score: " + GameManager.PlayerScore.ToString();
        }
    }

    public void StopCountingScore()
    {
        isScoreCounting = false;
    }

    public void TimeManager()
    {
        if (Time.timeScale > 0)
        {
            gameTime += Time.deltaTime;
        }
    }
}
