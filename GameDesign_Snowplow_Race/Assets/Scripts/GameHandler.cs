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
    public string endSceneName = "EndScene";
    private GameObject clearPopup;

    // Start is called before the first frame update
    void Start()
    {

        ListAllTagsInScene("tags at startup");

        SetClearPopupsTextInScene("Clear all " + scoreToWin + " snow");

        UpdateScore();
    }
    public void AddScore(int points)
    {
        playerScore += points;
        UpdateScore();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static string sceneWonName;
    private static int winningScore;
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == endSceneName) {
            ListAllTagsInScene("Tags after end scene loaded");
            GameObject textFinalScore = GameObject.FindGameObjectWithTag("TextFinalScore");
            if (textFinalScore == null) {
                Debug.LogError("No GameObject found with tag TextFinalScore.");
            } else {
                Text finalTextB = textFinalScore.GetComponent<Text>();
                if (finalTextB == null) {
                    Debug.LogError("Object tagged TextFinalScore doesn't have a Text component");
                } else {
                    finalTextB.text = "FINAL SCORE: " + winningScore + "! You *beat* " + sceneWonName;
                }
            }
        }
    }

    private void WinGame() {
        Debug.Log("Win!!! (with " + playerScore + " points");
        winningScore = playerScore;
        playerScore = 0;
        sceneWonName = SceneManager.GetActiveScene().name;
        ListAllTagsInScene("Tags before end scene");
        SceneManager.LoadScene(endSceneName);
        // not actually loaded until update is complete
    }




    // Update is called once per frame
    void UpdateScore()
    {
        Text scoreTextB = scoreText.GetComponent<Text>();
        if (playerScore >= scoreToWin) {
            WinGame();
        } else {
            scoreTextB.text = "SCORE: " + playerScore;
        }
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

    private void SetClearPopupsTextInHierarchy(GameObject obj, string text) {
        if (obj.tag == "PopupClear") {
            Text textComponent = obj.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = text;
                Debug.Log("text successfully updated to");
            }
            else
            {
                Debug.LogWarning("No Text component found in children of PopupClear.");
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

}
