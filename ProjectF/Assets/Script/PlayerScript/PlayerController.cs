

using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Basic Player Setting")]
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float jumpForce = 25f;
    [SerializeField] float groundResistnace = 50f;
    [SerializeField] float airResistance = 20f;


    [Header("Layer that is Jumpable")]
    public LayerMask groundLayer;
    
    [Header("Get Checkers")]
    [SerializeField] Transform groundChecker;

    // Reference Variables
    private ShotGun shotgun;
    private Rigidbody2D rb;

    // Physics Variables
    private float xAxis;
    private float yAxis;
    private bool jumpRequested;
    public bool isGrounded {get; private set;}
    private Vector2 activeKnockBack;   
    private float xSpeedLimit = 90f;
    private float ySpeedLimit = 90f;


    #region Basic Process
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        shotgun = GetComponentInChildren<ShotGun>();

        if (shotgun == null)
        {
            Debug.LogError("[PlayerController] : Shotgun is not found in child of Player");
        }
    }

    void Start()
    {
        activeKnockBack = Vector2.zero;
    }

    void Update()
    {
        isGrounded = CheckGroundCollide();
        xAxis = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested= true;
        }
    }

    void FixedUpdate()
    {
        float currentResistance= isGrounded ? groundResistnace : airResistance;
        activeKnockBack = Vector2.MoveTowards(activeKnockBack, Vector2.zero,currentResistance);

        ApplyFinalMovement();
    }
    #endregion


    #region 

    void ApplyFinalMovement()
    {
        float targetX = xAxis * moveSpeed; ;
        float targetY = rb.linearVelocityY;

        // Handle Jump
        if (jumpRequested)
        {
            Debug.Log("Applying JumpForce");
            targetY = jumpForce; // Replace Y entirely for a consistent jump height
            jumpRequested = false;
        }

        if (shotgun.isPressed)
        {
            activeKnockBack += shotgun.KnockBackForce;
            targetY = targetY + activeKnockBack.y;
            shotgun.isPressed = false;
        }
        
        targetX += activeKnockBack.x;

        targetX = Mathf.Clamp( targetX, -xSpeedLimit, xSpeedLimit);    
        targetY = Mathf.Clamp( targetY, -ySpeedLimit, ySpeedLimit);

        rb.linearVelocity = new Vector2 ( targetX, targetY);
    }
    #endregion


    #region GoundState
    bool CheckGroundCollide()
    {
        float checkerRadius = 0.2f;
        return Physics2D.OverlapCircle(groundChecker.position, checkerRadius, groundLayer);
    }

    #endregion
}
