using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.loop = true;
        audio.Play();
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
