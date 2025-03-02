using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject timeText;
    public int playerScore = 0;
    public static int scoreToWin = 850;
    public GameObject textFinalScore;

    // Game time in seconds
    public int gameTime = 120;
    // Internal timer to accumulate deltaTime
    private float gameTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTime();
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        // Only update timer if we're not in the EndScene
        if (SceneManager.GetActiveScene().name != "EndScene")
        {
            gameTimer += Time.deltaTime;
            if (gameTimer >= 1f)
            {
                gameTimer -= 1f;
                gameTime--;
                UpdateTime();

                // If time is up, load the EndScene
                if (gameTime <= 0)
                {
                    SceneManager.LoadScene("EndScene");
                }
            }
        }
    }

    public void AddScore(int points)
    {
        playerScore += points;
        UpdateScore();
    }

    public void UpdateTime()
    {
        Text timeTextB = timeText.GetComponent<Text>();
        int minutes = gameTime / 60;
        int seconds = gameTime % 60;
        timeTextB.text = "Time Left: " + minutes.ToString() + ":" + seconds.ToString("D2");
    }


    void UpdateScore()
    {
        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            Text finalScoreText = textFinalScore.GetComponent<Text>();
            finalScoreText.text = "FINAL SCORE: " + playerScore;
        }
        else
        {
            Text scoreTextB = scoreText.GetComponent<Text>();
            scoreTextB.text = "Score: " + playerScore;

            //check for winning condition
            if (playerScore >= scoreToWin)
            {
                // playerScore = 0;
                SceneManager.LoadScene("EndScene");
            }
        }
    }

    void RestartGame()
    {
        playerScore = 0;
        SceneManager.LoadScene("peterPlaytestSnowplow");
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
