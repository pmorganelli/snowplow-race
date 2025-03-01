using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public float scrollSpeed = 20f;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //moving credits upwards so it looks cute
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed*Time.deltaTime);
    }
}
