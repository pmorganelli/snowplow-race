// source: ChatGPT

using UnityEngine;

public class ScreenClearer : MonoBehaviour
{
    public GameObject fog; // The opaque fog being cleared,
                              // likely created by GameObject::2D Object::Sprite

    public GameObject maskParent; // The GameObject holding the bitmap mask

    public Texture2D fogTexture;
    private Texture2D maskTexture;
    private SpriteRenderer fogRenderer;
    
    void Start()
    {
        // Get fog's sprite and texture
        fogRenderer = fog.GetComponent<SpriteRenderer>();
        if (fogRenderer == null || !(fogRenderer.sprite.texture is Texture2D))
        {
            if(fogRenderer == null)
            {
                Debug.LogError("fogRenderer is null.");
            }
            Debug.LogError("fog must have a SpriteRenderer with a Texture2D.");
            return;
        }

        // Clone fog texture so we don't modify the original asset
        fogTexture = Instantiate(fogRenderer.sprite.texture);
        fogTexture.Apply();
        fogRenderer.sprite = Sprite.Create(fogTexture, fogRenderer.sprite.rect, new Vector2(0.5f, 0.5f));

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
        if (fogTexture == null || maskTexture == null) return;

        // Convert world position to fog texture space
        Vector2 fogPos = WorldToTexturePosition(transform.position, fogRenderer);
        
        // Apply the mask to clear alpha
        ApplyMask(fogPos);
    }

    void ApplyMask(Vector2 position)
    {
        int maskWidth = maskTexture.width;
        int maskHeight = maskTexture.height;
        int fogWidth = fogTexture.width;
        int fogHeight = fogTexture.height;

        // Convert position to integer pixel coordinates
        int startX = (int)(position.x - maskWidth / 2);
        int startY = (int)(position.y - maskHeight / 2);

        for (int x = 0; x < maskWidth; x++)
        {
            for (int y = 0; y < maskHeight; y++)
            {
                int fogX = startX + x;
                int fogY = startY + y;

                // Ensure within fog bounds
                if (fogX < 0 || fogX >= fogWidth || fogY < 0 || fogY >= fogHeight)
                    continue;

                // Check if the mask pixel is set
                if (maskTexture.GetPixel(x, y).a > 0.5f)
                {
                    // Make the corresponding fog pixel transparent
                    Color pixel = fogTexture.GetPixel(fogX, fogY);
                    pixel.a = 0;
                    fogTexture.SetPixel(fogX, fogY, pixel);
                }
            }
        }

        // Apply the texture modifications
        fogTexture.Apply();
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
