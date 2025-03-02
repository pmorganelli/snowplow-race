using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroDialogueManager : MonoBehaviour
{
    public GameObject introBox; // The UI panel containing dialogue
    public Text introText; // The legacy UI Text component

    [TextArea(2, 5)] // Makes text input easier in the Inspector
    public string[] dialogueLines; // Array of dialogue text
    private int currentLine = 0;
    private bool isTyping = false;
    private bool textFullyDisplayed = false;

    public float textSpeed = 0.05f; // Speed of the typewriter effect

    void Start()
    {
        introBox.SetActive(true); // Show dialogue box at start
        StartCoroutine(TypeText(dialogueLines[currentLine])); // Start first dialogue line
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping) // If text is still appearing, instantly show full text
            {
                StopAllCoroutines();
                introText.text = dialogueLines[currentLine]; // Instantly display full line
                isTyping = false;
                textFullyDisplayed = true;
            }
            else if (textFullyDisplayed) // Move to next dialogue if full text is already shown
            {
                NextDialogue();
            }
        }
    }

    void NextDialogue()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            StartCoroutine(TypeText(dialogueLines[currentLine]));
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        textFullyDisplayed = false;
        introText.text = ""; // Clear text

        foreach (char letter in text)
        {
            introText.text += letter;
            yield return new WaitForSeconds(textSpeed); // Simulate typing effect
        }

        isTyping = false;
        textFullyDisplayed = true;
    }

    void EndDialogue()
    {
        introBox.SetActive(false); // Hide the intro dialogue box
        // Transition to main game scene (optional)
        // SceneManager.LoadScene("MainGameScene");
    }
}
