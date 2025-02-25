using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("here!!");
        if(collision.gameObject.CompareTag("Snowpile"))
        {
            Debug.Log("collided!!");
        }
    }
}
