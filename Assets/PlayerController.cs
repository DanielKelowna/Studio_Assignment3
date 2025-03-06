using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float airControlMultiplier = 0.3f; // Reduced air control
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimeCounter = 0f;
    private Vector2 currentMoveInput = Vector2.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (inputManager != null)
        {
            inputManager.OnMove.AddListener(UpdateMoveInput);
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
        // Handle variable jump height
        if (isJumping)
        {
            if (Input.GetKey(KeyCode.Space) && jumpTimeCounter > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(currentMoveInput);
    }

    private void UpdateMoveInput(Vector2 moveInput)
    {
        currentMoveInput = moveInput;
    }

    private void MovePlayer(Vector2 moveInput)
    {
        // Get camera-relative directions
        Transform camTransform = Camera.main.transform;
        Vector3 camForward = camTransform.forward;
        camForward.y = 0;
        camForward.Normalize();
        Vector3 camRight = camTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Calculate move direction
        Vector3 moveDirection = (camForward * moveInput.y + camRight * moveInput.x);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            moveDirection.Normalize();

            if (isGrounded)
            {
                // Direct control on ground
                rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
            }
            else
            {
                // Reduced control in air
                float currentSpeed = airControlMultiplier * speed;
                rb.linearVelocity = new Vector3(moveDirection.x * currentSpeed, rb.linearVelocity.y, moveDirection.z * currentSpeed);
            }
        }
        else if (isGrounded)
        {
            // No input and on ground: immediately stop horizontal movement
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
        // When in air with no input, maintain horizontal velocity
    }

    private void StartJump()
    {
        if (isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    private void ResetPlayer()
    {
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }

    // Ground detection using collision
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}