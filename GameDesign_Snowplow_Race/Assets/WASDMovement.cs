// Initial source: ChatGPT

using UnityEngine;

public class WASDMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of forward/backward movement
    public float rotationSpeed = 180f; // Speed of rotation in degrees per second

    void Update()
    {
        // Get input values (return zero on no keypress)
        float moveInput = Input.GetAxisRaw("Vertical");  // W/S → Forward/Backward
        float rotateInput = Input.GetAxisRaw("Horizontal"); // A/D → Rotate Left/Right

        // Rotate the player around the Z-axis (2D rotation)
        transform.Rotate(0, 0, -rotateInput * rotationSpeed * Time.deltaTime);

        // Move the player in its forward direction
        transform.position += transform.up * moveInput * moveSpeed * Time.deltaTime;
    }
}
