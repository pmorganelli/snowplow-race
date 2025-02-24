using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class snowClear : MonoBehaviour
{
    public Tilemap snowTilemap; // Assign in the Inspector

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("SnowTilemap")) // Make sure the SnowTilemap has this tag
        {
            Vector3Int tilePosition = snowTilemap.WorldToCell(transform.position);
            if (snowTilemap.HasTile(tilePosition))
            {
                snowTilemap.SetTile(tilePosition, null); // Remove the tile
            }
        }
    }
}

