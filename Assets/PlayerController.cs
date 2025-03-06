using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxJumpTime = 0.5f; // Maximum time the jump force is applied
    [SerializeField] private LayerMask groundLayer;   // Ground layer mask for collision checking

    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimeCounter = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (inputManager != null)
        {
            inputManager.OnMove.AddListener(MovePlayer);
            inputManager.OnReset.AddListener(ResetPlayer);
            inputManager.OnJump.AddListener(StartJump);
        }
        else
        {
            Debug.LogWarning("InputManager is not assigned in PlayerController.");
        }
    }

    private void Update()
    {
        // If in the middle of a jump, check if the jump key is still held and time remains.
        if (isJumping)
        {
            if (Input.GetKey(KeyCode.Space) && jumpTimeCounter > 0)
            {
                // Maintain upward velocity while jump is held.
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                // Jump key released or time expired: stop extra upward force.
                isJumping = false;
            }
        }
    }

    // Called when movement input is received.
    private void MovePlayer(Vector2 moveInput)
    {
        // Convert 2D input to 3D movement.
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // Directly set horizontal velocity while preserving the current vertical velocity.
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
    }

    // Called when the jump button is pressed.
    private void StartJump()
    {
        // Only start a jump if player is on the ground.
        if (isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;

            // Immediately apply upward velocity.
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    // Resets the player to the origin with zero velocity.
    private void ResetPlayer()
    {
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }

    // Check if currently colliding with an object in the ground layer.
    private void OnCollisionEnter(Collision collision)
    {
        // Use bitwise check to see if the collided object is in the groundLayer.
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
        {
            isGrounded = false;
        }
    }
}
