using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    public static int playerScore = 0;
    // Start is called before the first frame update
    public void RestartGame()
    {
        playerScore = 0;
        SceneManager.LoadScene("SelinScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}
