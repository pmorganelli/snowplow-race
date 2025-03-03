using UnityEngine;
using UnityEngine.Tilemaps;

public class SnowClearNearby : MonoBehaviour
{
    public GameHandler gameHandlerObj;
    public Tilemap snowTilemap;
    public Collider2D pickupCollider;
    // public Animator animator;  // Reference to Animator component
    public GameObject snowAnimPrefab; // Assign this in the Inspector

    public AudioClip snowCollisionSound;
    private AudioSource audioSource;
    private void Start()
    { 
        if (GameObject.FindWithTag("GameHandler") != null) {
            gameHandlerObj = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        }

        GameObject tilemapObject = GameObject.FindGameObjectWithTag("SnowTilemap");

        if (tilemapObject == null) {
            Debug.LogError("No GameObject found with tag 'SnowTilemap'.");
        } else {
            snowTilemap = tilemapObject.GetComponent<Tilemap>();
            if (snowTilemap == null) {
                Debug.LogError("No Tilemap component found on the GameObject tagged 'SnowTilemap'.");
            }
        }

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders) {
            if (col.isTrigger) {
                pickupCollider = col;
                break;
            }
        }
        if (pickupCollider == null) {
            Debug.LogError("No trigger collider found on this GameObject!");
        }

        audioSource = GetComponent<AudioSource>();
        if(audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // audioSource.playOnAwake = false;
        // audioSource.loop = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("SnowTilemap"))
        {
            PickupClosestTile();
        }
    }

    private void PickupClosestTile()
    {
        Bounds triggerBounds = pickupCollider.bounds;
        Vector3Int minCellPos = snowTilemap.WorldToCell(triggerBounds.min);
        Vector3Int maxCellPos = snowTilemap.WorldToCell(triggerBounds.max);

        TileBase closestTile = null;
        float closestDistanceSqr = float.MaxValue;
        Vector3Int closestCellPos = Vector3Int.zero;
        
        for (int x = minCellPos.x; x <= maxCellPos.x; x++)
        {
            for (int y = minCellPos.y; y <= maxCellPos.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                TileBase tile = snowTilemap.GetTile(cellPos);
                if (tile != null)
                {
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

        if (closestTile != null) {
            gameHandlerObj.AddScore(1);
            snowTilemap.SetTile(closestCellPos, null);

            if(snowCollisionSound != null && audioSource != null) {
                audioSource.PlayOneShot(snowCollisionSound);
            }
            // Distance ahead of the plow where the effect should appear
            float forwardOffset = 1.5f; // Adjust this for best appearance

            // Calculate the spawn position based on direction
            Vector3 effectPosition = transform.position + (transform.up * forwardOffset);

            // Spawn the animation prefab at the new position
            if (snowAnimPrefab != null) {
                // GameObject newEffect = Instantiate(snowAnimPrefab, effectPosition, Quaternion.identity);
                GameObject newEffect = Instantiate(snowAnimPrefab, effectPosition, transform.rotation);
                Destroy(newEffect, 0.517f); // Destroy after animation plays
            }
        }

    }

    private Vector3 GetMoveDirection()
    {
            Vector3 direction = Vector3.zero;

            // Check the input and set direction
            if (Input.GetKey(KeyCode.W)) direction = Vector3.up;      // Moving up
            if (Input.GetKey(KeyCode.S)) direction = Vector3.down;    // Moving down
            if (Input.GetKey(KeyCode.A)) direction = Vector3.left;    // Moving left
            if (Input.GetKey(KeyCode.D)) direction = Vector3.right;   // Moving right

            // If no input is detected, keep last known direction
            if (direction == Vector3.zero) direction = transform.right;

            return direction;
    }
}
