using UnityEngine;

public class Postable : MonoBehaviour
{
    [Tooltip("Whether the GameObject should be visible at startup.")]
    public bool visibleAtStartup = false;

    private void Start()
    {
        gameObject.SetActive(visibleAtStartup);
    }

    /// <summary>
    /// Makes the GameObject visible (active) in the scene.
    /// </summary>
    public void Post()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Makes the GameObject disappear (inactive) from the scene.
    /// </summary>
    public void Unpost()
    {
        gameObject.SetActive(false);
    }
}
