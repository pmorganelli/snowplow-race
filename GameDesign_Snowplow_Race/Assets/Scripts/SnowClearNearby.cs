// help from chatgpt

using UnityEngine;
using UnityEngine.Tilemaps;

public class SnowClearNearby : MonoBehaviour
{
    public GameHandler gameHandlerObj;
    public Tilemap snowTilemap;           // found by tag at startup
    public Collider2D pickupCollider;     // The only trigger collider

    private void Start()
    { 

        // initialize the game handler
        if (GameObject.FindWithTag("GameHandler") != null) {
            gameHandlerObj = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        }

        // find the snow tilemap

        GameObject tilemapObject = GameObject.FindGameObjectWithTag("SnowTilemap");

        if (tilemapObject == null) {
            Debug.LogError("No GameObject found with tag 'SnowTilemap'.");
        } else {
            snowTilemap = tilemapObject.GetComponent<Tilemap>();
            
            if (snowTilemap == null)
            {
                Debug.LogError("No Tilemap component found on the GameObject tagged 'RoadTilemap'.");
            }
        }

        // find the collider that triggers pickup (the only trigger collider)
        
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D col in colliders) {
            if (col.isTrigger) {
                pickupCollider = col;
                break; // Stop once we find the trigger
            }
        }

        if (pickupCollider == null) {
            Debug.LogError("No trigger collider found on this GameObject!");
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        // Only run if we collided with the tilemap
        if (other.CompareTag("SnowTilemap"))
        {
            PickupClosestTile();
        }
    }

    private void PickupClosestTile()
    {
        // 1. Get the bounding box (in world space) of the big trigger collider
        Bounds triggerBounds = pickupCollider.bounds;
        Vector3 minWorldPos = triggerBounds.min;
        Vector3 maxWorldPos = triggerBounds.max;
        
        // 2. Convert those min/max points to cell coordinates
        Vector3Int minCellPos = snowTilemap.WorldToCell(minWorldPos);
        Vector3Int maxCellPos = snowTilemap.WorldToCell(maxWorldPos);

        //Debug.Log("min trigger position is " + minCellPos);
        //Debug.Log("max trigger position is " + maxCellPos);

        TileBase closestTile = null;
        float closestDistanceSqr = float.MaxValue;
        Vector3Int closestCellPos = Vector3Int.zero;
        
        // 3. Iterate over each cell in that bounding box
        for (int x = minCellPos.x; x <= maxCellPos.x; x++)
        {
            for (int y = minCellPos.y; y <= maxCellPos.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                TileBase tile = snowTilemap.GetTile(cellPos);
                //Debug.Log("got a snow tile at " + cellPos);
                if (tile != null)
                {
                    // 4. Check distance from the tile’s center to the player’s position
                    Vector3 tileCenterWorld = snowTilemap.GetCellCenterWorld(cellPos);
                    float distSqr = (tileCenterWorld - transform.position).sqrMagnitude;

                    if (distSqr < closestDistanceSqr)
                    {
                        closestDistanceSqr = distSqr;
                        closestTile = tile;
                        closestCellPos = cellPos;
                    }
                }
            }
        }

        // If we found a valid tile in range, destroy it and add score
        if (closestTile != null) {
            gameHandlerObj.AddScore(1);
            snowTilemap.SetTile(closestCellPos, null); // Remove the tile
        }
    }
}
