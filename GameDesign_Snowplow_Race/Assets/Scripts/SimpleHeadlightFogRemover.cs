// original source: ChatGPT
using UnityEngine;

/// <summary>
/// Very simple script that clears a circular area of the fog texture
/// around this object's center, with radius = 1/20 of the screen height.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SimpleHeadlightFogRemover : MonoBehaviour
{
    [Header("Fog Setup")]
    [Tooltip("SpriteRenderer for the large fog texture covering the screen.")]
    public SpriteRenderer fogSpriteRenderer;

    // We'll store a clone of the fog texture so we can modify it in-play.
    private Texture2D fogTexture;

    // Cached reference to the camera (assuming a main orthographic camera).
    private Camera mainCam;

    private void Start()
    {
        if (fogSpriteRenderer == null)
        {
            Debug.LogError("Fog SpriteRenderer is not assigned!");
            enabled = false;
            return;
        }

        // Grab the original fog sprite
        Sprite fogSprite = fogSpriteRenderer.sprite;
        if (fogSprite == null)
        {
            Debug.LogError("Fog SpriteRenderer has no sprite assigned.");
            enabled = false;
            return;
        }

        // Clone the fog texture so we can write to it
        fogTexture = Instantiate(fogSprite.texture);
        fogTexture.Apply();

        // Reassign the cloned texture to the Fog SpriteRenderer
        fogSpriteRenderer.sprite = Sprite.Create(
            fogTexture,
            new Rect(0, 0, fogTexture.width, fogTexture.height),
            new Vector2(0.5f, 0.5f), // pivot
            fogSprite.pixelsPerUnit
        );

        // Grab the main camera (for orthographic size)
        mainCam = Camera.main;
        if (mainCam == null || mainCam.orthographic == false)
        {
            Debug.LogWarning("Main camera not found or not orthographic. This script assumes an orthographic camera.");
        }
    }

    private void Update()
    {
        // In each frame, clear the fog in a circle around this object's position
        ClearFogCircle();
    }

    private void ClearFogCircle()
    {
        // 1) Find the radius in world units
        // Screen height in orthographic is 2 * orthographicSize
        // 1/20 of screen height = (2 * orthoSize)/20 = orthoSize/10
        float radiusWorld = 0f;
        if (mainCam != null && mainCam.orthographic)
        {
            radiusWorld = mainCam.orthographicSize / 10f;
        }
        else
        {
            // Fallback if no camera
            radiusWorld = 1f; // arbitrary default
        }

        // 2) This object's center in world space
        Vector2 centerWorldPos = transform.position;

        // 3) We'll need the fog's bounding box in world space
        Bounds fogBounds = fogSpriteRenderer.bounds;

        // 4) Convert world positions to fog texture pixel positions
        //    We'll do a bounding box in pixel space to limit the loop
        //    so we don't iterate over the entire texture unnecessarily.

        // Fog texture width & height
        int texW = fogTexture.width;
        int texH = fogTexture.height;

        // We'll find approximate min/max in world space for the circle,
        // then map that to pixel coordinates

        // A bounding box in world space around the circle
        float worldXMin = centerWorldPos.x - radiusWorld;
        float worldXMax = centerWorldPos.x + radiusWorld;
        float worldYMin = centerWorldPos.y - radiusWorld;
        float worldYMax = centerWorldPos.y + radiusWorld;

        // Now we clamp that box to the fog's overall bounds
        worldXMin = Mathf.Max(worldXMin, fogBounds.min.x);
        worldXMax = Mathf.Min(worldXMax, fogBounds.max.x);
        worldYMin = Mathf.Max(worldYMin, fogBounds.min.y);
        worldYMax = Mathf.Min(worldYMax, fogBounds.max.y);

        // Convert the min/max corners to texture pixel coordinates
        // We'll do an InverseLerp approach for each axis
        int xMin = Mathf.RoundToInt(
            Mathf.InverseLerp(fogBounds.min.x, fogBounds.max.x, worldXMin) * (texW - 1)
        );
        int xMax = Mathf.RoundToInt(
            Mathf.InverseLerp(fogBounds.min.x, fogBounds.max.x, worldXMax) * (texW - 1)
        );
        int yMin = Mathf.RoundToInt(
            Mathf.InverseLerp(fogBounds.min.y, fogBounds.max.y, worldYMin) * (texH - 1)
        );
        int yMax = Mathf.RoundToInt(
            Mathf.InverseLerp(fogBounds.min.y, fogBounds.max.y, worldYMax) * (texH - 1)
        );

        // Make sure the bounding box is within the texture
        xMin = Mathf.Clamp(xMin, 0, texW - 1);
        xMax = Mathf.Clamp(xMax, 0, texW - 1);
        yMin = Mathf.Clamp(yMin, 0, texH - 1);
        yMax = Mathf.Clamp(yMax, 0, texH - 1);

        // 5) Loop over that bounding box and clear any pixels within the radius
        //    We'll do a standard distance check in *world space*.

        // To avoid converting every pixel coordinate to world space,
        // we can do the "inverse" for each pixel: map pixel -> world, then check distance.

        for (int px = xMin; px <= xMax; px++)
        {
            for (int py = yMin; py <= yMax; py++)
            {
                // Convert (px, py) to [0..1]
                float u = px / (float)(texW - 1);
                float v = py / (float)(texH - 1);

                // Convert to world space within fog bounds
                float worldX = Mathf.Lerp(fogBounds.min.x, fogBounds.max.x, u);
                float worldY = Mathf.Lerp(fogBounds.min.y, fogBounds.max.y, v);
                Vector2 worldPos = new Vector2(worldX, worldY);

                // Check distance to center of headlight
                float dist = Vector2.Distance(worldPos, centerWorldPos);
                if (dist <= radiusWorld)
                {
                    Color fogPixel = fogTexture.GetPixel(px, py);
                    if (fogPixel.a > 0f)
                    {
                        fogPixel.a = 0f;
                        fogTexture.SetPixel(px, py, fogPixel);
                    }
                }
            }
        }

        // 6) Apply changes so the texture updates visually
        fogTexture.Apply();
    }
}
