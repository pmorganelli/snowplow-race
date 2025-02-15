// Initial Source: chatgpt

using UnityEngine;

public class WASDMovement : MonoBehaviour
{
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode rotateLeftKey = KeyCode.A;
    public KeyCode rotateRightKey = KeyCode.D;

    public float moveSpeed = 5f;  // Forward/backward speed
    public float rotationSpeed = 180f; // Rotation speed in degrees per second

    void Update()
    {
        // Get movement input
        float moveInput = (Input.GetKey(forwardKey) ? 1 : 0) + (Input.GetKey(backwardKey) ? -1 : 0);
        float rotateInput = (Input.GetKey(rotateLeftKey) ? -1 : 0) + (Input.GetKey(rotateRightKey) ? 1 : 0);

        // Rotate the player
        transform.Rotate(0, 0, -rotateInput * rotationSpeed * Time.deltaTime);

        // Move the player forward/backward
        transform.position += transform.up * moveInput * moveSpeed * Time.deltaTime;
    }
}
