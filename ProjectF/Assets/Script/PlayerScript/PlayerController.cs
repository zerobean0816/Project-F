using Unity.VisualScripting;
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
    
    [Header("Get Checkers")]
    [SerializeField] Transform groundChecker;
    public LayerMask jumpableLayer;

    // Reference Variables
    private PlayerSkill playerSkill;
    private ShotGun shotgun;
    private PlayerGunRotator playerGunRotator;
    private Rigidbody2D rb;

    // Physics Variables
    private float xAxis;
    private bool jumpRequested;
    private float xSpeedLimit = 60f;
    private float ySpeedLimit = 45f;

    public bool isGrounded {get; private set;}


    #region Basic Process
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        shotgun = GetComponentInChildren<ShotGun>();
        playerGunRotator = GetComponent<PlayerGunRotator>();
        playerSkill = GetComponent<PlayerSkill>();

        if (shotgun == null)
        {
            Debug.LogError("[PlayerController] : Shotgun is not found in child of Player");
        }
    }


    void Update()
    {
        isGrounded = CheckGroundCollide();
        xAxis = Input.GetAxis("Horizontal");

        if (!GameManager.Instance.playerManager.isStuned)
        {   
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerSkill.CallSkill();
            }

            if ((Input.GetKeyDown(KeyCode.Space) ) && isGrounded)
            {
                jumpRequested= true;
            }

            if (Input.GetMouseButtonDown(0) && shotgun.currentBullet > 0)
            {
                shotgun.FireGun();
            }

            playerGunRotator.rotatorUpdate();

            shotgun.ReloadWhenEmpty();
            shotgun.ShowBulletByCount();
        }
    }

    void FixedUpdate()
    {
        // Check if Player is Stunned;
        if (GameManager.Instance.playerManager.isStuned)  
        { 
            SetStunnedMovement();
            return;
        }

        // Handle XY Basic ForeAdd based on Player Input
        HandleXMovementPhysics();
        HandleYMovementPhysics();

        AddKnockbackByType(shotgun);
        AddKnockbackByType(playerSkill);

        LimitVelocity();
    }
    #endregion

    #region Physics

    void SetStunnedMovement()
    {
        // Only decrease X force
        Vector2 stunMove;
        stunMove.x = rb.linearVelocityX * -2f;
        stunMove.y = rb.linearVelocityY;

        rb.AddForce(stunMove);
    }

    void HandleXMovementPhysics()
    {
        // Target speed from input
        float targetX = xAxis * moveSpeed;
        
        // The "Difference" between where we are and where we want to be
        float speedDif = targetX - rb.linearVelocityX;

        // Acceleration value (Higher = Snappier, Lower = Weightier)
        float accel = isGrounded ? groundResistnace : airResistance;

        // If player is trying to move to other direction, give additional acceleration
        bool isTurning = (targetX > 0 && rb.linearVelocityX < 0) || (targetX < 0 && rb.linearVelocityX > 0);
        if (isTurning)
        {
            accel *= 4f; // trun speed;
        }
        // Also apply extra friction if the player releases the keys (stopping)
        else if (Mathf.Abs(xAxis) < 0.01f)
        {
            accel *= 1.5f;
        }

        // Apply X force to player;
        rb.AddForce(Vector2.right * speedDif * accel);
    }

    void HandleYMovementPhysics()
    {
        if (jumpRequested && isGrounded)
        {
            // calculate jump force needed for player
            float forceNeeded = jumpForce - rb.linearVelocityY;
            
            // apply force in y direction of player
            rb.AddForce(Vector2.up * forceNeeded, ForceMode2D.Impulse);
            
            jumpRequested = false;
        }
    }

    void AddKnockbackByType(IKnockbackSource source)
    {
        if (!source.IsRequesting)
        {
            return;
        }
        rb.AddForce(source.GetForce());
        source.Consume();
    }

    // Vector2 AddKnockBackEffect(Vector2 targetVelocity)
    // {
    //     targetVelocity += ShotGunKnockBack(targetVelocity);
    //     targetVelocity += SkillKnockBack(targetVelocity);

    //     return targetVelocity;
    // }

    // Vector2 ShotGunKnockBack(Vector2 targetVelocity)
    // {
    //     if (shotgun.isPressed)
    //     {
    //         // Set shotgun knowckback
    //         Vector2 shotForce = shotgun.KnockBackForce;

    //         // 
    //         if (rb.linearVelocityY < -0.1f && shotForce.y > 0)
    //         {

    //             float recoilBonus = Mathf.Abs(rb.linearVelocityY);
    //             recoilBonus *= 0.8f; 
    //             shotForce.y += recoilBonus;
    //             //Debug.Log($"Shotgun Knockback Force: {targetVelocity}");
    //         }
            
    //         // Set Y knowckback force
    //         targetVelocity.y += shotForce.y;

    //         // Apply x Force to active Knockback
    //         activeKnockBack.x += shotForce.x;

    //         //  Finish Press mechanism
    //         shotgun.FinishePress();
    //     }

    //     return targetVelocity;
    // }

    // Vector2 SkillKnockBack(Vector2 targetVelocity)
    // {
    //     if (playerSkill.isPressed)
    //     {
    //         Vector2 skillForce = playerSkill.knockBackForce;

    //         if (rb.linearVelocityY < -0.1f && skillForce.y > 0)
    //         {
    //             float recoilBonus = Mathf.Abs(rb.linearVelocityY);
    //             skillForce.y += recoilBonus;
    //         }

    //         targetVelocity.y += skillForce.y;

    //         activeKnockBack.x += skillForce.x;

    //         playerSkill.FinishePress();
    //     }

    //     return targetVelocity;
    // }



    void LimitVelocity()
    {
        Vector2 v = rb.linearVelocity;
        // Clamping ensures that even with massive knockbacks, 
        // the player doesn't phase through walls.
        v.x = Mathf.Clamp(v.x, -xSpeedLimit, xSpeedLimit);
        v.y = Mathf.Clamp(v.y, -ySpeedLimit, ySpeedLimit);
        rb.linearVelocity = v;
    }

    #endregion

    #region GoundState
    bool CheckGroundCollide()
    {
        float checkerRadius = 0.2f;
        return Physics2D.OverlapCircle(groundChecker.position, checkerRadius, jumpableLayer);
    }

    #endregion
}


public interface IKnockbackSource
{
    bool IsRequesting {get;}
    Vector2 GetForce();
    void Consume();
}
