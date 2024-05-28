using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private bool isScoreCounting = true;

    void Update()
    {
        if (isScoreCounting)
        {
            GameManager.PlayerScore = Mathf.FloorToInt(Time.time);

            scoreText.text = "Score: " + GameManager.PlayerScore.ToString();
        }
    }

    public void StopCountingScore()
    {
        isScoreCounting = false;
    }
}
