using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of the player movement.")]
    private float moveSpeed = 5f;

    //[SerializeField, Tooltip("Speed of the player rotation.")]
    //private float rotationSpeed = 360f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private float gravity = -9.81f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Player movement
        Vector3 movement = transform.forward * vertical + transform.right * horizontal;

        if (movement.magnitude >= 0.1f)
        {
            // Move the player
            controller.Move(movement * moveSpeed * Time.deltaTime);

            // Update Animator parameters
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
        }
        else
        {
            // Reset Animator parameters to Idle when not moving
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
        }

        // Gravity application
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Reset vertical velocity when grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }
    }
}
