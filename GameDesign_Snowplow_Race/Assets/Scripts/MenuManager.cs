using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene("IntroScene"); // scene 1 must be the first level
    }

    // Update is called once per frame
    // void Update()
    // {
    //     SceneManager.LoadScene("SelinScene");
    // }


    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OpenCredits() {
        SceneManager.LoadScene("CreditsSceneST");
    }
}
