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
    private PlayerGunGet playerGunGet;
    private Rigidbody2D rb;

    // Physics Variables
    private float xAxis;
    private bool jumpRequested;
    private Vector2 activeKnockBack;   
    private float xSpeedLimit = 60f;
    private float ySpeedLimit = 45f;

    public bool isGrounded {get; private set;}


    #region Basic Process
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        shotgun = GetComponentInChildren<ShotGun>();
        playerGunGet = GetComponent<PlayerGunGet>();

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

        playerGunGet.GunUpdate();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested= true;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.playerManager.isStuned)
        {
            float currentResistance= isGrounded ? groundResistnace : airResistance;
            
            activeKnockBack = Vector2.MoveTowards(activeKnockBack, Vector2.zero,currentResistance);

            ApplyFinalMovement();
        }
    }
    #endregion


    #region 

void ApplyFinalMovement()
    {
        Vector2 targetVelocity = new Vector2(xAxis * moveSpeed, rb.linearVelocityY);

        if (jumpRequested)
        {
            targetVelocity.y = jumpForce;
            jumpRequested = false;
        }

        targetVelocity = AddKnockBackEffect(targetVelocity);
        
        targetVelocity = LimitXYSpeed(targetVelocity);
        rb.linearVelocity = targetVelocity;

        float decay = isGrounded ? 50f : 20f;
        activeKnockBack.x = Vector2.MoveTowards(activeKnockBack, Vector2.zero, decay * Time.fixedDeltaTime).x;
        activeKnockBack.y = 0; 
    }

    Vector2 AddKnockBackEffect(Vector2 targetVelocity)
    {
        targetVelocity = ShotGunKnockBack(targetVelocity);
        targetVelocity.x += activeKnockBack.x;
        
        return targetVelocity;
    }

    Vector2 ShotGunKnockBack(Vector2 targetVelocity)
    {
        if (shotgun.isPressed)
        {
            // Set shotgun knowckback
            Vector2 shotForce = shotgun.KnockBackForce;

            // 
            if (rb.linearVelocityY < -0.1f && shotForce.y > 0)
            {

                float recoilBonus = Mathf.Abs(rb.linearVelocityY);
                recoilBonus *= 0.8f; 
                shotForce.y += recoilBonus;
                //Debug.Log($"Shotgun Knockback Force: {targetVelocity}");
            }
            
            // Set Y knowckback force
            targetVelocity.y += shotForce.y;

            // Apply x Force to active Knockback
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
