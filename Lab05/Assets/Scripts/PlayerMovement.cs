using UnityEngine;

//Moves the player with a Rigidbody component attached to the GameObject
//A/D keys to rotate, W/S keys to move forward/backward, Space to jump

[RequireComponent(typeof(Rigidbody))] // Ensures that a Rigidbody component is attached to the GameObject
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f; // Speed of movement
    [SerializeField] private float turnSpeed = 150f; // Speed of rotation
    [SerializeField] private float jumpForce = 5f; // Force applied when jumping

    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f; // Distance to check for ground
    [SerializeField] private LayerMask groundMask; // Layer mask to identify ground objects

    private Rigidbody rb; // Reference to the Rigidbody component
    private Vector3 moveDirection; // Direction of movement
    private float turnInput; // Input for turning
    private bool isGrounded; // Flag to check if the player is grounded
    private bool jumpRequested = false; // Flag to check if jump is requested

    public bool IsGrounded => isGrounded; // Public property to access the isGrounded flag

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the GameObject

        rb.constraints = RigidbodyConstraints.FreezeRotation;// Freeze rotation to prevent the player from falling over when colliding with objects
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position +
        transform.up*groundDistance/2, -transform.up, groundDistance, groundMask); // Check if the player is grounded using a raycast

        turnInput = Input.GetAxis("Horizontal"); // Get horizontal input for turning
        float moveZ = Input.GetAxis("Vertical"); // Get vertical input for moving forward/backward

        transform.Rotate(0f, turnInput * turnSpeed * Time.deltaTime, 0f); // Rotate the player based on input

        moveDirection = transform.forward * moveZ; // Calculate the movement direction based on the player's forward direction and input

        if (Input.GetButtonDown("Jump") && isGrounded) // Check if the jump button is pressed and the player is grounded
        {
            jumpRequested = true; // Set the jumpRequested flag to true
        }

    }

    private void FixedUpdate()
    {
        MovePlayer(); // Call the MovePlayer method to handle movement

        if (jumpRequested) // Check if a jump is requested
        {
            Jump(); // Call the Jump method to apply jump force
            jumpRequested = false; // Reset the jumpRequested flag
        }
    }

    private void MovePlayer()
    {
        Vector3 targetVelocity = moveDirection * moveSpeed; // Calculate the target velocity based on movement direction and speed

        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z); // Set the Rigidbody's velocity to the target velocity while preserving the current vertical velocity   

    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset the vertical velocity to 0 before applying jump force

        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange); // Apply an upward force to the Rigidbody to make the player jump   
    }
    
}