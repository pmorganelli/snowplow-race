using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //change #1: added namespace



public class GameHandler : MonoBehaviour
{
    public GameObject scoreText;
    public static int playerScore = 0;
    public int scoreToWin = 200;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }
    public void AddScore(int points)
    {
        playerScore += points;
        UpdateScore();
    }

    // Update is called once per frame
    void UpdateScore()
    {
        Text scoreTextB = scoreText.GetComponent<Text>();
        if(SceneManager.GetActiveScene().name == "EndScene")
        {
            scoreTextB.text = "FINAL SCORE: " + playerScore;
        }

        if (playerScore >= scoreToWin)
        {
            //scoreTextB.text = "FINAL SCORE:" + playerScore;
            playerScore = 0;
            SceneManager.LoadScene("EndScene"); //change #2: switch scene
                                                //gameOverText.SetActive(true); //change #3: comment gameOverText
        }
        scoreTextB.text = "SCORE: " + playerScore;
    }
    void RestartGame()
    {
        playerScore = 0;
        SceneManager.LoadScene(1);
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
