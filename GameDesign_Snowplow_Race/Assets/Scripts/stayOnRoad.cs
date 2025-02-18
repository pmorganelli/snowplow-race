using UnityEngine;

public class stayOnRoad : MonoBehaviour
{
    public string keyMapping = "WASD"; // Expected format: "ForwardBackwardLeftRight"

    private KeyCode forwardKey;
    private KeyCode backwardKey;
    private KeyCode rotateLeftKey;
    private KeyCode rotateRightKey;

    public float moveSpeed = 5f;  // Speed of forward/backward movement
    public float rotationSpeed = 180f; // Speed of rotation in degrees per second

    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (keyMapping.Length != 4)
        {
            Debug.LogError("Key mapping must be exactly 4 characters long.");
            return;
        }

        // Convert characters to KeyCodes
        forwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyMapping[0].ToString().ToUpper());
        backwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyMapping[1].ToString().ToUpper());
        rotateLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyMapping[2].ToString().ToUpper());
        rotateRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyMapping[3].ToString().ToUpper());
    }

    void Update()
    {
        // Get movement input
        float moveInput = (Input.GetKey(forwardKey) ? 1 : 0) + (Input.GetKey(backwardKey) ? -1 : 0);
        float rotateInput = (Input.GetKey(rotateLeftKey) ? -1 : 0) + (Input.GetKey(rotateRightKey) ? 1 : 0);

        // Apply rotation
        transform.Rotate(0, 0, -rotateInput * rotationSpeed * Time.deltaTime);

        // Determine movement direction
        movementInput = transform.up * moveInput; 
    }

    void FixedUpdate()
    {
        // Move using Rigidbody2D physics
        rb.MovePosition(rb.position + (movementInput * moveSpeed * Time.fixedDeltaTime));
    }
}
