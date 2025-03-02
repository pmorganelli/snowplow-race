using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.517f); // Adjust time to match animation length
    }
}