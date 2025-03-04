using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HighBeamEffect : MonoBehaviour
{
    public float highBeamDuration = 2f; // How long high beams last
    public float flickerSpeed = 0.1f; // Time between flickers
    //public int flickerCount = 1; // How many times it flickers
    public int maxHighBeams = 4; // Max high beam uses

    private int remainingHighBeams;
    private bool isFading = false;

    public GameObject darknessOverlay; // Reference to the Darkness Object
    public GameObject [] highBeamIndicators; // UI indicators for high beam count
    public GameObject warningTextBox; // UI text for "Dang, high beams are busted..."
    public float warningFadeTime = 1.5f; // Time for warning text fade-out

    void Start()
    {
        remainingHighBeams = maxHighBeams;
        Debug.Log("remaining: " + remainingHighBeams);
        warningTextBox.SetActive(false);
    }

    void Update()
    {
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

        // Temporarily disable darkness overlay
        if (darknessOverlay != null)
            darknessOverlay.SetActive(false);

        yield return new WaitForSeconds(highBeamDuration);

        // Flicker effect before turning darkness back on
        if (darknessOverlay != null)
            yield return StartCoroutine(FlickerDarkness());

        // Fully re-enable darkness
        if (darknessOverlay != null)
            darknessOverlay.SetActive(true);

        isFading = false;
    }

    IEnumerator FlickerDarkness()
    {
        darknessOverlay.SetActive(true);
        yield return new WaitForSeconds(flickerSpeed);
        darknessOverlay.SetActive(false);
        yield return new WaitForSeconds(flickerSpeed);

        // for (int i = 0; i < flickerCount; i++)
        // {
        //     darknessOverlay.SetActive(true);
        //     yield return new WaitForSeconds(flickerSpeed);
        //     darknessOverlay.SetActive(false);
        //     yield return new WaitForSeconds(flickerSpeed);
        // }
    }

    void UpdateHighBeamIndicators()
    {
        // Debug.Log("remaining " + remainingHighBeams);
        // Debug.Log("len " + highBeamIndicators.Length);
        for (int i = 0; i < highBeamIndicators.Length; i++)
        {
            // Activate the indicator if its index is less than the number of remaining high beams.
            highBeamIndicators[i].SetActive(i < remainingHighBeams);
        }
    }

    IEnumerator ShowWarningMessage()
    {
        warningTextBox.SetActive(true);
        Text warningText = warningTextBox.GetComponent<Text>();
        warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, 1f);

        yield return new WaitForSeconds(warningFadeTime);

        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 0.5f);
            warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, alpha);
            yield return null;
        }

        warningTextBox.SetActive(false);
    }
}
