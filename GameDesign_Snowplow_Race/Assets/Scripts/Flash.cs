using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HighBeamEffect : MonoBehaviour
{
    public float highBeamDuration = 2f; // How long high beams last
    public int maxHighBeams = 3; // Max high beam uses

    private int remainingHighBeams;
    private bool isFading = false;

    public GameObject darknessOverlay; // Reference to the Darkness Object
    public GameObject[] highBeamIndicators; // UI indicators for high beam count
    public GameObject warningTextBox; // UI text for "Dang, high beams are busted..."
    public float warningFadeTime = 1.5f; // Time for warning text fade-out

    void Start()
    {
        remainingHighBeams = maxHighBeams;
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

        // Re-enable darkness overlay
        if (darknessOverlay != null)
            darknessOverlay.SetActive(true);

        isFading = false;
    }

    void UpdateHighBeamIndicators()
    {
        if (remainingHighBeams < highBeamIndicators.Length)
        {
            highBeamIndicators[remainingHighBeams].SetActive(false);
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
