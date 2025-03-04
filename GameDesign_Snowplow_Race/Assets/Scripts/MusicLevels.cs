using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicLevels : MonoBehaviour
{
    private static MusicLevels instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void onDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        if(scene.name == "EndSceneByNR") {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
//     void Update()
//     {
        
//     }
}
