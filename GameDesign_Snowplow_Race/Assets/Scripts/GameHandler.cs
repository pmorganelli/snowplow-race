using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //change #1: added namespace



public class GameHandler : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject timerText;
    public static int playerScore = 0;
    public int scoreToWin = 200;
    public string endSceneName = "EndScene";
    private GameObject clearPopup;

    private float timer = 120;
    private bool isGameOver = false;

    void Start()
    {
        // ListAllTagsInScene("tags at startup"); // debugging

        SetClearPopupsTextInScene("Clear all " + scoreToWin + " snow");
           // make text in the "clear the snow" popup show exactly how
           // much snow needs to be cleared

        UpdateUIWithScore();
        UpdateUIWithTimer();
    }

    void Update() {
        if (!isGameOver) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                timer = 0f; //prevent from going negative
                UpdateUIWithTimer();
                isGameOver = true;
                LoseGame();
            } else {
                UpdateUIWithTimer();
            }
        }
    }

    public void AddScore(int points)
    {
        playerScore += points;
        UpdateUIWithScore();
    }

    // These next methods are there to make code run when the 
    // scene named `endSceneName` is loaded.  That's what makes
    // the end scene have the correct winning score.

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static string sceneWonName; // name of the level the player won
    private static int winningScore;    // and the number of points they won with
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // called *every* time a new scene is loaded
        if (scene.name == endSceneName) {
            // ListAllTagsInScene("Tags after end scene loaded"); // debug
            GameObject textFinalScore = GameObject.FindGameObjectWithTag("TextFinalScore");
            if (textFinalScore == null) {
                Debug.LogError("No GameObject found with tag TextFinalScore.");
            } else {
                Text finalTextB = textFinalScore.GetComponent<Text>();
                if (finalTextB == null) {
                    Debug.LogError("Object tagged TextFinalScore doesn't have a Text component");
                } else {
                    finalTextB.text =
                        "FINAL SCORE: " + winningScore + "! You beat " + sceneWonName;
                }
            }
        }
    }

    // to win the game, set the two variables above and then load the end scene

    private void WinGame() {
        // Debug.Log("Win!!! (with " + playerScore + " points)");
        winningScore = playerScore;
        playerScore = 0;
        sceneWonName = SceneManager.GetActiveScene().name;
        // ListAllTagsInScene("Tags before end scene"); // debug
        SceneManager.LoadScene(endSceneName);
        // not actually loaded until update is complete
    }

    private void LoseGame() {
        winningScore = playerScore;
        playerScore = 0;
        sceneWonName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("EndSceneByNR");
    }

    void UpdateUIWithScore()
    {
        Text scoreTextB = scoreText.GetComponent<Text>();
        if (playerScore >= scoreToWin) {
            WinGame();
        } else {
            scoreTextB.text = "SCORE: " + playerScore;
        }
    }

    void UpdateUIWithTimer() {
        if (timerText != null) {
            Text timerTextComponent = timerText.GetComponent<Text>();
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerTextComponent.text = string.Format("TIME: {0:0}:{1:00}", minutes, seconds);
        } else {
            Debug.LogError("timerText GameObject is not assigned in the inspector.");
        }
    }



    // restart and quit buttons

    public void RestartGame()
    {
        playerScore = 0;
        // Debug.Log("restarting with scene 1");
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        // Debug.Log("quitting game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    //// This code is a horrow show.  But it's the only way I have found
    //// to update the content of a popup that (a) is not active
    //// and (b) might not even be in the scene.


    //// The primary method does all the descendants of a single object.
    //// The next method below does all the root objects in the current scene.

    private void SetClearPopupsTextInHierarchy(GameObject obj, string text) {
        if (obj.tag == "PopupClear") {
            Text textComponent = obj.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = text;
                // Debug.Log("text successfully updated to");
            }
            else
            {
                Debug.LogError("No Text component found in children of PopupClear.");
            }
        } else {
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
        foreach (GameObject obj in rootObjects) {
            SetClearPopupsTextInHierarchy(obj, text);
        }
    }



    //// DEBUGGING CODE
    ////
    //// List the tags of objects with their current activity.

    public void ListAllTagsInScene(string prefix)
    {
        // Get the active scene
        Scene activeScene = SceneManager.GetActiveScene();

        // Get all root GameObjects in the scene
        GameObject[] rootObjects = activeScene.GetRootGameObjects();

        // Use a HashSet to store unique tags (avoiding duplicates)
        HashSet<string> uniqueTags = new HashSet<string>();

        foreach (GameObject obj in rootObjects)
        {
            // Recursively check the object and its children
            CollectTags(obj, uniqueTags);
        }

        // Log all unique tags found
        Debug.Log(prefix + " (scene " + activeScene.name + "): " + string.Join(", ", uniqueTags));
    }

    private void CollectTags(GameObject obj, HashSet<string> uniqueTags)
    {
        if (!string.IsNullOrEmpty(obj.tag)) // Ensure the object has a tag
        {
            string activity;
            if (obj.activeInHierarchy) {
                activity = "-A";
            } else {
                activity = "-I";
            }
            uniqueTags.Add(obj.tag + activity);
        }

        // Recursively check children
        foreach (Transform child in obj.transform)
        {
            CollectTags(child.gameObject, uniqueTags);
        }
    }


}
