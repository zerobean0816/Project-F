using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
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
    private float xSpeedLimit = 50f;
    private float ySpeedLimit = 40f;


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
        // Start with basic movement + current gravity
        Vector2 targetVelocity = new Vector2(xAxis * moveSpeed, rb.linearVelocityY);

        if (jumpRequested)
        {
            targetVelocity.y = jumpForce;
            jumpRequested = false;
        }

        // Apply the Shotgun logic
        targetVelocity = AddKnockBackEffect(targetVelocity);
        
        // Limit and apply
        targetVelocity = LimitXYSpeed(targetVelocity);
        rb.linearVelocity = targetVelocity;

        // CRITICAL: Decay the horizontal influence so it doesn't last forever
        float decay = isGrounded ? 50f : 20f;
        activeKnockBack.x = Vector2.MoveTowards(activeKnockBack, Vector2.zero, decay * Time.fixedDeltaTime).x;
        // We don't decay Y here because gravity handles it once it's applied to the Rigidbody
        activeKnockBack.y = 0; 
    }

    Vector2 AddKnockBackEffect(Vector2 targetVelocity)
    {
        // 1. Process the "Instant" Shotgun Blast
        targetVelocity = ShotGunKnockBack(targetVelocity);
        
        // 2. Add the "Persistent" Horizontal Slide
        targetVelocity.x += activeKnockBack.x;
        
        return targetVelocity;
    }

    Vector2 ShotGunKnockBack(Vector2 targetVelocity)
    {
        if (shotgun.isPressed)
        {
            // Set the force for this specific shot
            Vector2 shotForce = shotgun.KnockBackForce;

            // RECOVERY: If falling and shooting down (shotForce.y is positive)
            if (rb.linearVelocityY < -0.1f && shotForce.y > 0)
            {

                float recoilBonus = Mathf.Abs(rb.linearVelocityY);
                recoilBonus *= 0.8f; 
                shotForce.y += recoilBonus;
            }

            // Apply Vertical immediately (One-time pop)
            targetVelocity.y += shotForce.y;

            // Store Horizontal to activeKnockBack (to be decayed in ApplyFinalMovement)
            activeKnockBack.x += shotForce.x;

            shotgun.isPressed = false;
        }
        return targetVelocity;
    }

    Vector2 LimitXYSpeed(Vector2 targetVelocity)
    {
        targetVelocity.x = Mathf.Clamp( targetVelocity.x, -xSpeedLimit, xSpeedLimit);    
        targetVelocity.y = Mathf.Clamp( targetVelocity.y, -ySpeedLimit, ySpeedLimit);

        return targetVelocity;
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
