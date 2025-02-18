// original source: ChatGPT
using UnityEngine;

/// <summary>
/// Clears the fog by reading the pixels of a headlight sprite
/// and setting fog alpha=0 wherever that sprite is opaque.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class HeadlightRevealFog : MonoBehaviour
{
    [Header("References")]
    [Tooltip("SpriteRenderer that holds the large fog sprite covering the map.")]
    public SpriteRenderer fogSpriteRenderer;

    [Tooltip("World-space minimum corner of the fog (bottom-left).")]
    public Vector2 mapMinWorldPos;

    [Tooltip("World-space maximum corner of the fog (top-right).")]
    public Vector2 mapMaxWorldPos;

    [Header("Headlight Settings")]
    [Tooltip("How wide/tall (in world units) the headlight sprite should be 'projected' onto the fog.")]
    public Vector2 headlightDimensions = new Vector2(5f, 5f);

    // We'll store a clone of the original fog texture so we can modify it without affecting the asset.
    private Texture2D fogTexture;
    // We'll also store a reference to the headlight's own sprite renderer.
    private SpriteRenderer headlightSpriteRenderer;

    private void Start()
    {
        // Grab the Fog Sprite
        if (fogSpriteRenderer == null)
        {
            Debug.LogError("Fog SpriteRenderer is not assigned in Inspector.");
            enabled = false;
            return;
        }

        Sprite fogSprite = fogSpriteRenderer.sprite;
        if (fogSprite == null)
        {
            Debug.LogError("Fog SpriteRenderer has no sprite assigned!");
            enabled = false;
            return;
        }

        // Clone the fog texture so we can modify it
        // Make sure it's Read/Write Enabled in import settings!
        fogTexture = Instantiate(fogSprite.texture);

        // Reassign the cloned texture to the fog sprite so we’re modifying our own copy
        Rect fogRect = fogSprite.rect;
        Vector2 fogPivot = new Vector2(0.5f, 0.5f); // Adjust if your original sprite pivot is different
        float fogPixelsPerUnit = fogSprite.pixelsPerUnit;

        fogSpriteRenderer.sprite = Sprite.Create(
            fogTexture,
            fogRect,
            fogPivot,
            fogPixelsPerUnit
        );

        // Also get the sprite renderer on this headlight
        headlightSpriteRenderer = GetComponent<SpriteRenderer>();
        if (headlightSpriteRenderer.sprite == null)
        {
            Debug.LogError("Headlight object has no sprite assigned!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        // Every frame, we "stamp" the headlight shape onto the fog,
        // clearing any fog pixels behind it.
        RevealFogUsingHeadlightTexture();
    }

    /// <summary>
    /// Reads each pixel of the headlight sprite and, if it's opaque,
    /// clears the corresponding pixel in the fog.
    /// </summary>
    private void RevealFogUsingHeadlightTexture()
    {
        // The headlight sprite we want to read.
        Sprite headlightSprite = headlightSpriteRenderer.sprite;
        Texture2D headlightTex = headlightSprite.texture;
        if (headlightTex == null)
            return;

        // Because the headlight sprite might not be the same size as the "headlightDimensions" in world units,
        // we need to figure out how to map from (x, y in the sprite) → actual world space.

        // For simplicity, assume we want the entire sprite's texture (width x height) to map to "headlightDimensions".
        int texWidth = headlightTex.width;
        int texHeight = headlightTex.height;

        // We'll consider the pivot of the headlight sprite to be its center in local space.
        // (If your sprite pivot is something else, you can adjust or read from headlightSprite.pivot)
        float pivotX = texWidth * 0.5f;
        float pivotY = texHeight * 0.5f;

        // Loop over every pixel in the headlight texture
        for (int px = 0; px < texWidth; px++)
        {
            for (int py = 0; py < texHeight; py++)
            {
                // Check the headlight pixel's alpha
                Color headlightPixel = headlightTex.GetPixel(px, py);
                if (headlightPixel.a > 0.01f)  // a small threshold > 0 means "opaque enough to reveal"
                {
                    // 1) Convert (px, py) from texture space to a local offset around (0,0),
                    //    using the pivot as the "center" of the sprite
                    float localX = (px - pivotX) / texWidth;   // range ~ -0.5 .. +0.5
                    float localY = (py - pivotY) / texHeight;  // range ~ -0.5 .. +0.5

                    // 2) Scale that local offset to the full headlightDimensions in world units
                    float offsetWorldX = localX * headlightDimensions.x;
                    float offsetWorldY = localY * headlightDimensions.y;

                    // 3) Figure out the final world position of this pixel
                    Vector2 pixelWorldPos = new Vector2(
                        transform.position.x + offsetWorldX,
                        transform.position.y + offsetWorldY
                    );

                    // 4) Convert that world position to the fog texture’s coordinate space (pixel indices)
                    // Normalize to [0..1] across the map, then multiply by fogTexture size
                    float fogU = (pixelWorldPos.x - mapMinWorldPos.x) / (mapMaxWorldPos.x - mapMinWorldPos.x);
                    float fogV = (pixelWorldPos.y - mapMinWorldPos.y) / (mapMaxWorldPos.y - mapMinWorldPos.y);

                    int fogX = Mathf.RoundToInt(fogU * fogTexture.width);
                    int fogY = Mathf.RoundToInt(fogV * fogTexture.height);

                    // 5) If that falls within the fog texture, clear the pixel (set alpha=0)
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

        // Apply the changes so the texture updates visually
        fogTexture.Apply();
    }
}
