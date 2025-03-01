using UnityEngine;
using UnityEngine.UI; // If using an Image UI element for darkness
using System.Collections;

public class HighBeamEffect : MonoBehaviour
{
    public float fadeDuration = 0.5f; // Time to fade in/out
    public float highBeamDuration = 2f; // How long the high beam stays on
    public int maxHighBeams = 3; // Max high beam uses

    private int remainingHighBeams;
    private SpriteRenderer darknessSprite;
    private bool isFading = false;

    void Start()
    {
        darknessSprite = GetComponent<SpriteRenderer>();
        remainingHighBeams = maxHighBeams; // Set available uses
    }

    void Update()
    {
        // Press 'F' to trigger high beams if available
        if (Input.GetKeyDown(KeyCode.F) && remainingHighBeams > 0 && !isFading)
        {
            remainingHighBeams--; // Reduce count
            StartCoroutine(HighBeamFlash());
        }
    }

    IEnumerator HighBeamFlash()
    {
        isFading = true;

        // Fade Out (Make Darkness Transparent)
        yield return StartCoroutine(FadeDarkness(0f, fadeDuration));

        // Wait for the high beam duration
        yield return new WaitForSeconds(highBeamDuration);

        // Fade In (Restore Darkness)
        yield return StartCoroutine(FadeDarkness(1f, fadeDuration));

        isFading = false;
    }

    IEnumerator FadeDarkness(float targetAlpha, float duration)
    {
        float startAlpha = darknessSprite.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            darknessSprite.color = new Color(0, 0, 0, newAlpha); // Adjust transparency
            yield return null;
        }

        darknessSprite.color = new Color(0, 0, 0, targetAlpha); // Final alpha
    }
}
