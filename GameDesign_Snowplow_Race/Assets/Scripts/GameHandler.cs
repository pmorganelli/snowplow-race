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
    public GameObject clearPopup;

    // Start is called before the first frame update
    void Start()
    {
        clearPopup = GameObject.FindGameObjectWithTag("PopupClear");
        if (clearPopup != null) {
            Text textComponent = clearPopup.GetComponentInChildren<Text>();

            if (textComponent != null)
            {
                // Update the text
                textComponent.text = "Clear all " + scoreToWin + " snow";
            }
            else
            {
                Debug.LogWarning("No Text component found in children of PopupClear.");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject found with tag PopupClear.");
        }


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
    public void RestartGame()
    {
        playerScore = 0;
        Debug.Log("restarting with scene 1");
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Debug.Log("quitting game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
