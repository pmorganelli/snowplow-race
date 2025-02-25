using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement; //change #1: added namespace


public class snowClear : MonoBehaviour
{
    public GameHandler gameHandlerObj;
    public Tilemap snowTilemap; // Assign in the Inspector

    private void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        if (GameObject.FindWithTag("GameHandler") != null)
        {
            gameHandlerObj = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        }
    }



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("SnowTilemap")) // Make sure the SnowTilemap has this tag
        {
            Vector3Int tilePosition = snowTilemap.WorldToCell(transform.position);
            if (snowTilemap.HasTile(tilePosition))
            {
                gameHandlerObj.AddScore(1);

                snowTilemap.SetTile(tilePosition, null); // Remove the tile
            }
        }
    }
}

