using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ColliderPopup : MonoBehaviour
{
    [Tooltip("Tag name of a GameObject that must have a Postable component.")]
    public string popupTag;
    [Tooltip("Reference to a GameObject that must have a Postable component.")]
    private GameObject popup;
    private Postable popupComponent;

    void Start()
    {
        if (string.IsNullOrEmpty(popupTag))
        {
            Debug.LogError("popupTag is not set! Assign a valid tag in the Inspector.");
            return;
        }
        popup = GameObject.FindWithTag(popupTag);
        if (popup == null)
        {
            Debug.LogError($"No GameObject found with tag '{popupTag}'");
        }
        popupComponent = popup.GetComponent<Postable>();
        if (popupComponent == null) {
            Debug.LogError($"GameObject lacks a Postable component");
        }
        popupComponent.Initialize();
    }

    private void Reset()
    {
        // Ensure the attached BoxCollider2D is set to trigger
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Post the popup on trigger enter
        popupComponent.Post();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        popupComponent.Unpost();
    }
}
