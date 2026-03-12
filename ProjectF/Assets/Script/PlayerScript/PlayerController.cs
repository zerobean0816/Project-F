using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerController : MonoBehaviour
{
    [Header("Basic Player Setting")]
    public float moveSpeed = 20f;
    public float jumpForce = 10f;

    [Header("Mario Jump Setting")]
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;

    [Header("Layer that is Jumpable")]
    public LayerMask groundLayer;
    
    [Header("Get Checkers")]
    [SerializeField] Transform groundChecker;
    [SerializeField] Transform leftChecker;
    [SerializeField] Transform rightChecker;

    private Rigidbody2D rb;
    private float xAxis;

    private bool jumpRequested;
    private bool isHoldingJump;
    public bool isGrounded {get; private set;}

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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }
        isHoldingJump = Input.GetKey(KeyCode.Space);

    }

    void FixedUpdate()
    {
        ApplyPhysics();
    }
    #endregion

    bool CheckGroundCollide()
    {
        float rayDistance = 0.2f;
        return Physics2D.OverlapCircle(groundChecker.position, rayDistance, groundLayer);

    }

    void ApplyPhysics()
    {

        float targetHorizontalVelocity = xAxis * moveSpeed;
        rb.linearVelocity = new Vector2(targetHorizontalVelocity, rb.linearVelocity.y);

        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime;
        }  
        else if (rb.linearVelocity.y > 0 && !isHoldingJump)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}
