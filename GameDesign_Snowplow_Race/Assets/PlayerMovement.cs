// source: chatgpt

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public string keyMapping = "WASD"; // Expected format: "ForwardBackwardLeftRight"

    private KeyCode forwardKey;
    private KeyCode backwardKey;
    private KeyCode rotateLeftKey;
    private KeyCode rotateRightKey;

    public float moveSpeed = 5f;  // Speed of forward/backward movement
    public float rotationSpeed = 180f; // Speed of rotation in degrees per second

    void Start()
    {
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

        // Rotate the player
        transform.Rotate(0, 0, -rotateInput * rotationSpeed * Time.deltaTime);

        // Move the player forward/backward
        transform.position += transform.up * moveInput * moveSpeed * Time.deltaTime;
    }
}
