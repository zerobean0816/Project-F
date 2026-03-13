
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Basic Player Setting")]
    float moveSpeed = 20f;
    float jumpForce = 25f;


    [Header("Layer that is Jumpable")]
    public LayerMask groundLayer;
    
    [Header("Get Checkers")]
    [SerializeField] Transform groundChecker;

    // 
    private ShotGun shotgun;
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
        shotgun = GetComponentInChildren<ShotGun>();
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
        ApplyMovPhysics(out Vector2 currentVel);

        ApplyKnockBack(currentVel);

        rb.linearVelocity = currentVel;
    }

    void ApplyMovPhysics(out Vector2 currentVel)
    {
        currentVel.x = xAxis * moveSpeed;

        // 2. The Jump "Pop"
        if (jumpRequested)
        {
            currentVel.y = jumpForce; 
            jumpRequested = false;
        }
        currentVel.y = rb.linearVelocityY;
    }

    void ApplyKnockBack(Vector2 currentVel)
    {
        // shotgun KnockBack
        if (shotgun.isPressed)
        {
            currentVel += shotgun.KnockBack();
        }

        // Enemy KnockBack
        if (true)
        {
            
        }
    }

    #endregion
}
