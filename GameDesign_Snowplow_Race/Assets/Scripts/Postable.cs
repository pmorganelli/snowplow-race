using UnityEngine;

public class Postable : MonoBehaviour
{
    [Tooltip("Whether the GameObject should be visible at startup.")]
    public bool visibleAtStartup = false;

    // a Postable object should be active at startup so a client
    // can find it by its tag.  The client should then call the
    // Initialize method, which inactivates the object if needed.

    public void Initialize() // called by cl
    {
        gameObject.SetActive(visibleAtStartup);
    }

    /// Makes the GameObject visible in the scene.
    public void Post()
    {
        gameObject.SetActive(true);
    }

    /// Makes the GameObject disappear from the scene.
    public void Unpost()
    {
        gameObject.SetActive(false);
    }
}
