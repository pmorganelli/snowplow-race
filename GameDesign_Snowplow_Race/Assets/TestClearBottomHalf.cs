// original source: ChatGPT
using UnityEngine;

/// <summary>
/// A simple test script that, at startup, clears (makes fully transparent)
/// the bottom half of the fog texture, then does nothing else.
/// Attach this to any GameObject, and assign the Fog SpriteRenderer in the Inspector.
/// </summary>
public class TestClearBottomHalf : MonoBehaviour
{
    [Tooltip("SpriteRenderer for the fog covering the screen or map.")]
    public SpriteRenderer fogSpriteRenderer;

    private void Start()
    {
        if (fogSpriteRenderer == null)
        {
            Debug.LogError("Fog SpriteRenderer is not assigned!");
            enabled = false;
            return;
        }

        // Get the sprite from the fog SpriteRenderer
        Sprite fogSprite = fogSpriteRenderer.sprite;
        if (fogSprite == null)
        {
            Debug.LogError("Fog SpriteRenderer has no sprite assigned!");
            enabled = false;
            return;
        }

        // Clone the texture so we can modify it
        Texture2D originalFogTexture = fogSprite.texture;

Debug.Log($"Fog texture isReadable: {originalFogTexture.isReadable}");

        Texture2D clonedFogTexture = Instantiate(originalFogTexture);
        clonedFogTexture.Apply();

        // Create a new Sprite that uses our cloned texture
        fogSpriteRenderer.sprite = Sprite.Create(
            clonedFogTexture,
            new Rect(0, 0, clonedFogTexture.width, clonedFogTexture.height),
            new Vector2(0.5f, 0.5f),
            fogSprite.pixelsPerUnit
        );

Debug.Log("New Fog sprite name: " + fogSpriteRenderer.sprite.name);


        // Clear the bottom half of the texture by setting alpha to 0
        int width = clonedFogTexture.width;
        int height = clonedFogTexture.height;
        int halfHeight = height / 2;  // bottom half

        // Loop over the bottom half of the texture rows
        for (int y = 0; y < halfHeight; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixel = clonedFogTexture.GetPixel(x, y);
                pixel.a = 0f;
                clonedFogTexture.SetPixel(x, y, pixel);
                if (x == 0 && y == 0) {
                    Debug.Log("Loop pixel (0,0) set to pixel with alpha = " + pixel.a);
                    clonedFogTexture.Apply();
                    pixel = clonedFogTexture.GetPixel(x, y);
                    Debug.Log("Reloaded pixel (0,0) after Apply() has alpha = " + pixel.a);
                }
            }
        }

        // Apply changes
        clonedFogTexture.Apply();

Color checkPixel = clonedFogTexture.GetPixel(0, 0);
Debug.Log("Pixel (0,0) alpha after clearing = " + checkPixel.a);


        // Done! The bottom half of the fog should now be transparent.
    }
}
