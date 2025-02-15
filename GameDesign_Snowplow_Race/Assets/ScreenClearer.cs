// source: ChatGPT

using UnityEngine;

public class ScreenClearer : MonoBehaviour
{
    public GameObject screen; // The opaque screen being cleared,
                              // likely created by GameObject::2D Object::Sprite

    public GameObject maskParent; // The GameObject holding the bitmap mask

    private Texture2D screenTexture;
    private Texture2D maskTexture;
    private SpriteRenderer screenRenderer;
    
    void Start()
    {
        // Get screen's sprite and texture
        screenRenderer = screen.GetComponent<SpriteRenderer>();
        if (screenRenderer == null || !(screenRenderer.sprite.texture is Texture2D))
        {
            Debug.LogError("Screen must have a SpriteRenderer with a Texture2D.");
            return;
        }

        // Clone screen texture so we don't modify the original asset
        screenTexture = Instantiate(screenRenderer.sprite.texture);
        screenTexture.Apply();
        screenRenderer.sprite = Sprite.Create(screenTexture, screenRenderer.sprite.rect, new Vector2(0.5f, 0.5f));

        // Get the bitmap mask texture from maskParent's child
        maskTexture = maskParent.GetComponentInChildren<SpriteRenderer>()?.sprite?.texture;
        if (maskTexture == null)
        {
            Debug.LogError("Mask parent must have a child with a SpriteRenderer containing a Texture2D.");
            return;
        }
    }

    void Update()
    {
        if (screenTexture == null || maskTexture == null) return;

        // Convert world position to screen texture space
        Vector2 screenPos = WorldToTexturePosition(transform.position, screenRenderer);
        
        // Apply the mask to clear alpha
        ApplyMask(screenPos);
    }

    void ApplyMask(Vector2 position)
    {
        int maskWidth = maskTexture.width;
        int maskHeight = maskTexture.height;
        int screenWidth = screenTexture.width;
        int screenHeight = screenTexture.height;

        // Convert position to integer pixel coordinates
        int startX = (int)(position.x - maskWidth / 2);
        int startY = (int)(position.y - maskHeight / 2);

        for (int x = 0; x < maskWidth; x++)
        {
            for (int y = 0; y < maskHeight; y++)
            {
                int screenX = startX + x;
                int screenY = startY + y;

                // Ensure within screen bounds
                if (screenX < 0 || screenX >= screenWidth || screenY < 0 || screenY >= screenHeight)
                    continue;

                // Check if the mask pixel is set
                if (maskTexture.GetPixel(x, y).a > 0.5f)
                {
                    // Make the corresponding screen pixel transparent
                    Color pixel = screenTexture.GetPixel(screenX, screenY);
                    pixel.a = 0;
                    screenTexture.SetPixel(screenX, screenY, pixel);
                }
            }
        }

        // Apply the texture modifications
        screenTexture.Apply();
    }

    Vector2 WorldToTexturePosition(Vector3 worldPosition, SpriteRenderer renderer)
    {
        // Convert world position to local position
        Vector2 localPos = renderer.transform.InverseTransformPoint(worldPosition);

        // Convert local position to texture coordinates
        float pixelsPerUnit = renderer.sprite.pixelsPerUnit;
        Rect spriteRect = renderer.sprite.rect;

        float textureX = (localPos.x + spriteRect.width / (2 * pixelsPerUnit)) * pixelsPerUnit;
        float textureY = (localPos.y + spriteRect.height / (2 * pixelsPerUnit)) * pixelsPerUnit;

        return new Vector2(textureX, textureY);
    }
}
