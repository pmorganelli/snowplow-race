// original source: ChatGPT
using UnityEngine;

/// <summary>
/// Permanently clears the fog where the headlight sprite is opaque,
/// accounting for 2D rotation and uniform scaling via the transform.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class AdvancedHeadlightRevealFog : MonoBehaviour
{
    [Header("Fog Setup")]
    [Tooltip("SpriteRenderer for the large fog texture that covers the map or screen.")]
    public SpriteRenderer fogSpriteRenderer;

    // We'll store a clone of the fog texture so we can modify it in-play.
    private Texture2D fogTexture;

    // The local sprite renderer for the headlight (on this GameObject).
    private SpriteRenderer headlightSpriteRenderer;

    // We'll record the fog sprite's world-space bounding box for coordinate mapping.
    private Bounds fogBounds;

    private void Start()
    {
        if (fogSpriteRenderer == null)
        {
            Debug.LogError("Fog SpriteRenderer is not assigned on the AdvancedHeadlightRevealFog script.");
            enabled = false;
            return;
        }

        // Get the original fog sprite
        Sprite originalFogSprite = fogSpriteRenderer.sprite;
        if (originalFogSprite == null)
        {
            Debug.LogError("Fog SpriteRenderer has no sprite assigned.");
            enabled = false;
            return;
        }

        // Clone the fog texture so we can modify it
        fogTexture = Instantiate(originalFogSprite.texture);
        fogTexture.Apply();

        // Reassign the cloned texture to the Fog SpriteRenderer
        fogSpriteRenderer.sprite = Sprite.Create(
            fogTexture,
            new Rect(0, 0, fogTexture.width, fogTexture.height),
            new Vector2(0.5f, 0.5f),  // pivot
            originalFogSprite.pixelsPerUnit
        );

        // Record the fog sprite's bounding box in world space
        // This will help us map world positions → fog texture coordinates.
        fogBounds = fogSpriteRenderer.bounds;

        // Get the headlight sprite renderer on this object
        headlightSpriteRenderer = GetComponent<SpriteRenderer>();
        if (headlightSpriteRenderer.sprite == null)
        {
            Debug.LogError("Headlight SpriteRenderer has no sprite assigned!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        // Stamp the headlight's pixels onto the fog each frame
        RevealFogUsingTransform();
    }

    /// <summary>
    /// Reads each pixel from the headlight sprite,
    /// transforms its local position to world space using the object's transform,
    /// and then clears the corresponding pixel in the fog.
    /// </summary>
    private void RevealFogUsingTransform()
    {
        // Get the headlight sprite we're reading from
        Sprite headlightSprite = headlightSpriteRenderer.sprite;
        Texture2D headlightTex = headlightSprite.texture;
        if (headlightTex == null) return;

Debug.Log("Fog bounds: " + fogBounds + ", Headlight pos: " + transform.position);

        // We'll need the sprite's 'rect' and 'pivot' for correct pixel offsets
        Rect hlRect = headlightSprite.rect;     // The sub-rectangle in the texture atlas
        Vector2 hlPivot = headlightSprite.pivot; // The pivot in *pixel* coordinates
        float hlPPU = headlightSprite.pixelsPerUnit;

        // Loop over each pixel in the headlight sprite
        int xStart = (int)hlRect.x;
        int xEnd   = xStart + (int)hlRect.width;
        int yStart = (int)hlRect.y;
        int yEnd   = yStart + (int)hlRect.height;

        for (int y = yStart; y < yEnd; y++)
        {
            for (int x = xStart; x < xEnd; x++)
            {
                // Read the pixel from the headlight texture
                Color hlPixel = headlightTex.GetPixel(x, y);
                if (hlPixel.a > 0.01f)
                {
                    // Convert this pixel's (x,y) to local space relative to the headlight's pivot
                    // The pivot is in the same coordinate system as rect.x, rect.y
                    float localPx = x - (hlRect.x + hlPivot.x);
                    float localPy = y - (hlRect.y + hlPivot.y);

                    // Convert from pixels to Unity units, so that 1 unit = 1 meter (or similar)
                    // This also places the "center" at pivot = (0,0) in local space
                    Vector3 localPos = new Vector3(
                        localPx / hlPPU,
                        localPy / hlPPU,
                        0f
                    );

                    // Transform localPos by the headlight object's transform to get worldPos
                    // This accounts for position, rotation, and scale
                    Vector3 worldPos = transform.TransformPoint(localPos);

                    // Now map that world position to the fog texture coordinates
                    // We'll do a simple bounding-box approach: 0..1 across the fog sprite's bounds
                    float fogU = Mathf.InverseLerp(fogBounds.min.x, fogBounds.max.x, worldPos.x);
                    float fogV = Mathf.InverseLerp(fogBounds.min.y, fogBounds.max.y, worldPos.y);

                    // Convert to fog texture pixel coordinates
                    int fogX = Mathf.RoundToInt(fogU * (fogTexture.width - 1));
                    int fogY = Mathf.RoundToInt(fogV * (fogTexture.height - 1));

                    // If we are inside the fog texture, set alpha=0
                    if (fogX >= 0 && fogX < fogTexture.width && fogY >= 0 && fogY < fogTexture.height)
                    {
                        Color fogPixel = fogTexture.GetPixel(fogX, fogY);
                        if (fogPixel.a > 0f)
                        {
                            fogPixel.a = 0f;
                            fogTexture.SetPixel(fogX, fogY, fogPixel);
                        }
                    }
                }
            }
        }

        // Apply changes so the fog visually updates
        fogTexture.Apply();
    }
}
