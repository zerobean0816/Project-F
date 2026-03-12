
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Basic Player Setting")]
    float moveSpeed = 20f;
    float jumpForce = 25f;

    [Header("Mario Jump Setting")]
    float FallMultiplier = 4f;
    float LowJumpMultiplier = 15f;

    [Header("Layer that is Jumpable")]
    public LayerMask groundLayer;
    
    [Header("Get Checkers")]
    [SerializeField] Transform groundChecker;

    // 
    private Rigidbody2D rb;
    private float xAxis;

    // Jump Request values
    private bool jumpRequested;
    private bool isHoldingJump;
    public bool isGrounded {get; private set;}

    // Varaible for more flexible Jump
    private float jumpBufferTime = 0.1f;
    private float jumpBufrferCounter;


    #region Basic Process
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        isGrounded = CheckGroundCollide();
        xAxis = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            jumpBufrferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufrferCounter -= Time.deltaTime;
        }

        if (jumpBufrferCounter > 0 && isGrounded)
        {
            jumpRequested = true;
            jumpBufrferCounter = 0;
        }

        isHoldingJump = Input.GetKey(KeyCode.Space);

    }

    void FixedUpdate()
    {
        ApplyPhysics();
    }
    #endregion

    #region GoundState
    bool CheckGroundCollide()
    {
        float checkerRadius = 0.2f;
        return Physics2D.OverlapCircle(groundChecker.position, checkerRadius, groundLayer);
    }


    void ApplyPhysics()
    {
        Vector2 currentVel = rb.linearVelocity;

        // 1. Snappy Horizontal Movement
        // Use GetAxisRaw in Update for instant start/stop (no sliding)
        currentVel.x = xAxis * moveSpeed;

        // 2. The Jump "Pop"
        if (jumpRequested)
        {
            // Use = instead of += to ensure the jump strength is consistent 
            // regardless of current falling speed
            currentVel.y = jumpForce; 
            jumpRequested = false;
        }

        // 3. HOLLOW KNIGHT JUMP PHYSICS
        if (currentVel.y > 0) // Rising
        {
            if (!isHoldingJump)
            {
                // The "Cut-off": Apply MASSIVE gravity when button is released
                // Hollow Knight feels like you hit a ceiling
                currentVel.y += Physics2D.gravity.y * (LowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
        else if (currentVel.y < 0) // Falling
        {
            // Faster falling for a "heavy" feel
            currentVel.y += Physics2D.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime;
        }

        rb.linearVelocity = currentVel;
    }
    #endregion
}
