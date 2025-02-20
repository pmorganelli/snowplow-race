// source: chatgpt

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RBSteering : MonoBehaviour
{
    [Tooltip("4-character string defining forward, rotate-left, backward, rotate-right keys.")]
    public string keyMapping = "WASD";

    [Tooltip("Forward/backward movement speed.")]
    public float moveSpeed = 5f;

    [Tooltip("Rotation speed in degrees per second.")]
    public float rotationSpeed = 180f;

    private Rigidbody2D rb;
    private KeyCode forwardKey;
    private KeyCode rotateLeftKey;
    private KeyCode backwardKey;
    private KeyCode rotateRightKey;

    void Start()
    {
        // Get the Rigidbody2D on this object
        rb = GetComponent<Rigidbody2D>();
        
        // Typically set a kinematic body if you're moving via MovePosition/MoveRotation
        rb.isKinematic = true;

        // Validate the keyMapping string
        if (keyMapping.Length != 4)
        {
            Debug.LogError("Key mapping must be exactly 4 characters long (e.g., 'WASD').");
            return;
        }

        // Assign letters to key bindings
        // 0 -> forward, 1 -> backward, 2 -> rotate left, 3 -> rotate right
        forwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyMapping[0].ToString().ToUpper());
        rotateLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyMapping[1].ToString().ToUpper());
        backwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyMapping[2].ToString().ToUpper());
        rotateRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyMapping[3].ToString().ToUpper());
    }

    void FixedUpdate()
    {
        // If the string was invalid, bail out
        if (keyMapping.Length != 4) return;

        // Determine forward/backward input
        float moveInput = 0f;
        if (Input.GetKey(forwardKey))   moveInput += 1f;
        if (Input.GetKey(backwardKey))  moveInput -= 1f;

        // Determine rotation input
        float rotateInput = 0f;
        if (Input.GetKey(rotateLeftKey))  rotateInput += 1f;
        if (Input.GetKey(rotateRightKey)) rotateInput -= 1f;

        // Rotate the player (Z-axis rotation in 2D)
        float rotationAmount = rotateInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);

        // Move the player forward/backward along its "up" vector
        Vector2 forward = transform.up;  // local "up" is forward in Unity 2D
        Vector2 newPosition = rb.position + forward * (moveInput * moveSpeed * Time.fixedDeltaTime);

        // Update the Rigidbody2D position for proper physics/collision
        rb.MovePosition(newPosition);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with {collision.gameObject.name}");
    }


}
