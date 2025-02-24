using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPlowMaskController : MonoBehaviour
{
    public Texture2D brushTexture; // A round black texture for erasing
    public RenderTexture renderTexture;
    public Camera maskCamera;

    private void Start()
    {
        // Set the RenderTexture as the active target
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;
    }

    private void Update()
    {
        DrawMask(transform.position);
    }

    void DrawMask(Vector3 position)
    {
        // Convert world position to UV coordinates
        Vector3 screenPos = maskCamera.WorldToViewportPoint(position);
        if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1)
        {
            RenderTexture.active = renderTexture;
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, renderTexture.width, renderTexture.height, 0);

            // Draw the brush at the plow's position
            Graphics.DrawTexture(new Rect(screenPos.x * renderTexture.width - 25, 
                                          screenPos.y * renderTexture.height - 25, 50, 50), brushTexture);

            GL.PopMatrix();
            RenderTexture.active = null;
        }
    }
}
