using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 8f;         // Upward velocity applied when jumping.
    public float maxJumpTime = 0.3f;     // Maximum duration the jump button affects the jump.
    public float fallMultiplier = 2f;    // Multiplier to boost gravity when falling.

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer;  // Set to the layer(s) representing ground.

    private Rigidbody rb;
    private Camera cam;

    // Jump control variables.
    private bool isJumping = false;
    private float jumpTimeCounter = 0f;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    void Update()
    {
        // --- Movement Relative to Camera ---
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculate camera-based forward and right (ignore vertical component).
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();
        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Determine movement direction.
        Vector3 moveDir = (camForward * vertical + camRight * horizontal).normalized;

        // Set horizontal velocity directly (to avoid sliding), preserving vertical velocity.
        Vector3 newVelocity = moveDir * moveSpeed;
        newVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = newVelocity;

        // --- Jump Logic ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            // Reset vertical velocity then apply jump force.
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        // --- Extra Gravity ---
        // When falling, apply additional downward velocity.
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // --- Ground Detection Using a Trigger Collider ---
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other collider is on one of the ground layers.
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}
