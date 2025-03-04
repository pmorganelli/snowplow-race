using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject timerText;
    public static int playerScore = 0;
    public int scoreToWin = 200;
    public string endSceneName = "EndScene";
    public string successSceneName = "SuccessScene";

    private float timer = 120;
    private bool isGameOver = false;

    // Static variables for end-of-level information.
    private static string sceneWonName; // The name of the level the player just won.
    private static int winningScore;    // The score achieved when winning the level.

    // Static variable to store the build index of the last completed level.
    public static int lastCompletedLevel = -1;

    void Start()
    {
        SetClearPopupsTextInScene("Clear all " + scoreToWin + " snow");
        UpdateUIWithScore();
        UpdateUIWithTimer();
    }

    void Update() 
    {
        if (!isGameOver)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f; // prevent negative time
                UpdateUIWithTimer();
                isGameOver = true;
                LoseGame();
            }
            else
            {
                UpdateUIWithTimer();
            }
        }
    }

    public void AddScore(int points)
    {
        playerScore += points;
        UpdateUIWithScore();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // When the end scene is loaded, update its final score text.
        if (scene.name == endSceneName)
        {
            GameObject textFinalScore = GameObject.FindGameObjectWithTag("TextFinalScore");
            if (textFinalScore == null)
            {
                Debug.LogError("No GameObject found with tag TextFinalScore.");
            }
            else
            {
                Text finalTextB = textFinalScore.GetComponent<Text>();
                if (finalTextB == null)
                {
                    Debug.LogError("Object tagged TextFinalScore doesn't have a Text component");
                }
                else
                {
                    finalTextB.text = "FINAL SCORE: " + winningScore + "! You beat " + sceneWonName;
                }
            }
        }
    }

    // Called when the player wins a level.
    private void WinGame()
    {
        winningScore = playerScore;
        playerScore = 0;
        sceneWonName = SceneManager.GetActiveScene().name;
        // Store the build index of the completed level.
        lastCompletedLevel = SceneManager.GetActiveScene().buildIndex;
        // Load the success scene regardless of which level was completed.
        SceneManager.LoadScene(successSceneName);
    }

    private void LoseGame()
    {
        winningScore = playerScore;
        playerScore = 0;
        sceneWonName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("EndSceneByNR");
    }

    void UpdateUIWithScore()
    {
        Text scoreTextB = scoreText.GetComponent<Text>();
        if (playerScore >= scoreToWin)
        {
            WinGame();
        }
        else
        {
            scoreTextB.text = "SCORE: " + playerScore;
        }
    }

    void UpdateUIWithTimer()
    {
        if (timerText != null)
        {
            Text timerTextComponent = timerText.GetComponent<Text>();
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerTextComponent.text = string.Format("TIME: {0:0}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogError("timerText GameObject is not assigned in the inspector.");
        }
    }

    public void RestartGame()
    {
        playerScore = 0;
        SceneManager.LoadScene(1);
    }

    // When NextLevel() is called, we check if the last level completed was Level4.
    // If so, we load the Credits scene; otherwise, we load the next level in the build order.
    public void NextLevel()
    {
        if (sceneWonName == "Level4")
        {
            SceneManager.LoadScene("CreditsSceneST");
            return;
        }
        
        int nextLevelIndex = lastCompletedLevel + 1;
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            SceneManager.LoadScene(endSceneName);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // --- Methods for updating popup text in the scene ---
    private void SetClearPopupsTextInHierarchy(GameObject obj, string text)
    {
        if (obj.tag == "PopupClear")
        {
            Text textComponent = obj.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = text;
            }
            else
            {
                Debug.LogError("No Text component found in children of PopupClear.");
            }
        }
        else
        {
            foreach (Transform child in obj.transform)
            {
                SetClearPopupsTextInHierarchy(child.gameObject, text);
            }
        }
    }
            
    private void SetClearPopupsTextInScene(string text)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            SetClearPopupsTextInHierarchy(obj, text);
        }
    }

    // --- Debugging methods ---
    public void ListAllTagsInScene(string prefix)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        HashSet<string> uniqueTags = new HashSet<string>();
        foreach (GameObject obj in rootObjects)
        {
            CollectTags(obj, uniqueTags);
        }
        Debug.Log(prefix + " (scene " + activeScene.name + "): " + string.Join(", ", uniqueTags));
    }

    private void CollectTags(GameObject obj, HashSet<string> uniqueTags)
    {
        if (!string.IsNullOrEmpty(obj.tag))
        {
            string activity = obj.activeInHierarchy ? "-A" : "-I";
            uniqueTags.Add(obj.tag + activity);
        }
        foreach (Transform child in obj.transform)
        {
            CollectTags(child.gameObject, uniqueTags);
        }
    }
}
