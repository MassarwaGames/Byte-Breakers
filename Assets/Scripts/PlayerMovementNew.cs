using UnityEngine;

/// <summary>
/// Handles first-person player movement, gravity, and camera rotation.
/// Integrates CharacterController physics, animation support, and smooth mouse look.
/// </summary>
public class PlayerMovementNew : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField, Tooltip("Speed of the player movement.")]
    private float moveSpeed = 5f;
    [SerializeField, Tooltip("Sprint speed multiplier.")]
    private float sprintMultiplier = 1.5f;

    [Header("Gravity & Jumping")]
    [SerializeField, Tooltip("Gravity strength applied to the player.")]
    private float gravity = -9.81f;
    [SerializeField, Tooltip("Jump height of the player.")]
    private float jumpHeight = 1.5f;

    [Header("Mouse Look Settings")]
    [SerializeField, Tooltip("Reference to the player's camera.")]
    private Transform playerCamera;
    [SerializeField, Tooltip("Mouse sensitivity for looking around.")]
    private float mouseSensitivity = 100f;
    [SerializeField, Tooltip("Invert the Y-axis for mouse look.")]
    private bool invertY = false;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private float verticalRotation = 0f;

    private void Start()
    {
        // Get components
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Lock cursor for FPS movement
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        ApplyGravity();
    }

    /// <summary>
    /// Handles player movement, including sprinting and animation updates.
    /// </summary>
    private void HandleMovement()
    {
        // Get movement input
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrows
        float moveZ = Input.GetAxis("Vertical"); // W/S or Up/Down Arrows

        // Calculate movement direction
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        // Apply sprinting
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * sprintMultiplier : moveSpeed;

        // Move the player
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Update Animator parameters for movement
        if (animator)
        {
            animator.SetFloat("Horizontal", moveX);
            animator.SetFloat("Vertical", moveZ);
        }
    }

    /// <summary>
    /// Handles smooth mouse movement for camera and player rotation.
    /// </summary>
    private void HandleMouseLook()
    {
        if (playerCamera == null) return; // Prevent errors if no camera is assigned

        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust vertical rotation (look up/down)
        verticalRotation -= invertY ? -mouseY : mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        // Apply camera rotation
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Rotate player (turn left/right)
        transform.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Applies gravity and ensures the player stays grounded.
    /// </summary>
    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
