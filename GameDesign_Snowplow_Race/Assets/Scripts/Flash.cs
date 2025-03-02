using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HighBeamEffect : MonoBehaviour
{
    public float fadeDuration = 0.5f; // Time to fade in/out
    public float highBeamDuration = 2f; // How long the high beam stays on
    public int maxHighBeams = 3; // Max high beam uses

    private int remainingHighBeams;
    private SpriteRenderer darknessSprite;
    private bool isFading = false;

    public GameObject[] highBeamIndicators; // UI objects for high beam count
    public GameObject warningTextBox; // UI text box for "Dang, high beams are really busted now..."
    public float warningFadeTime = 1.5f; // Time for warning text fade-out

    void Start()
    {
        darknessSprite = GetComponent<SpriteRenderer>();
        remainingHighBeams = maxHighBeams; // Set available uses
        warningTextBox.SetActive(false); // Hide warning text at start
    }

    void Update()
    {
        // Press 'F' to trigger high beams if available
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (remainingHighBeams > 0 && !isFading)
            {
                remainingHighBeams--; // Reduce count
                UpdateHighBeamIndicators(); // Remove UI indicator
                StartCoroutine(HighBeamFlash());
            }
            else if (remainingHighBeams <= 0)
            {
                StartCoroutine(ShowWarningMessage());
            }
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

    void UpdateHighBeamIndicators()
    {
        // Hide one high beam indicator per use
        if (remainingHighBeams < highBeamIndicators.Length)
        {
            highBeamIndicators[remainingHighBeams].SetActive(false);
        }
    }

    IEnumerator ShowWarningMessage()
    {
        warningTextBox.SetActive(true);
        Text warningText = warningTextBox.GetComponent<Text>();
        warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, 1f); // Set full visibility

        yield return new WaitForSeconds(warningFadeTime); // Keep text visible

        // Fade out the warning text smoothly
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, alpha);
            yield return null;
        }

        warningTextBox.SetActive(false);
    }
}
