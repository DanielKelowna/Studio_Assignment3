using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement Settings
    [SerializeField] private float speed = 10f;                     // Move speed.
    [SerializeField] private float airControlMultiplier = 0.5f;     // Reduced control while airborne.

    // Jump Settings 
    [SerializeField] private float jumpForce = 300f;                 // Jump impulse
    [SerializeField] private float fallMultiplier = 1.3f;           // Falling too slow by default
    [SerializeField] private float lowJumpMultiplier = 1.1f;        // Extra gravity if jump is released early.

    
    [SerializeField] private LayerMask groundLayer;               

    private Rigidbody rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Horizontal Movement 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveInput = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 desiredVelocity = moveInput * speed;
        Vector3 currentVelocity = rb.linearVelocity;

        // When grounded, set horizontal velocity directly; in air, reduced control.
        if (isGrounded)
        {
            currentVelocity.x = desiredVelocity.x;
            currentVelocity.z = desiredVelocity.z;
        }
        else
        {
            currentVelocity.x = Mathf.Lerp(currentVelocity.x, desiredVelocity.x, airControlMultiplier);
            currentVelocity.z = Mathf.Lerp(currentVelocity.z, desiredVelocity.z, airControlMultiplier);
        }
        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);

        // Jump Input
        // On jump button down, if on the ground, reset vertical velocity then add impulse.
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Faster gravity
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Apply a lower jump multiplier if jump held less
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // Ground detection
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
