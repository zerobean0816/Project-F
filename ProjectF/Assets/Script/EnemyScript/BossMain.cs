
using UnityEngine;

public class BossMain : MonoBehaviour
{
    [SerializeField] float slowSpeed = 1.5f;
    [SerializeField] float normalSpeed = 3f;
    [SerializeField] float fastSpeed = 8f;
    [SerializeField] float fastestSpeed = 20f;
    [SerializeField] float acceleration = 10f;

    [SerializeField] float slowDistance = 20f;
    [SerializeField] float catchUpDistance = 60f;

    //[SerializeField] float knowckbackForce = 2000f;

    public LayerMask unbreakableLayer;
    public bool isMoving;
    public bool isX = false;

    private Rigidbody2D rb;
    GameObject player;


    float bossYHeight;

    void Start()
    {
        player = GameManager.Instance.playerManager.player;
        rb = gameObject.GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;

        bossYHeight = transform.localScale.y;

        isMoving = false;
    }


    void FixedUpdate()
    {
        if (isMoving)
        {
            if (isX)
            {
                MoveBossX();
                return;
            }
            MoveBoss();
        }
    }

    void MoveBoss()
    {
        Vector2 forwardDirection = transform.up;

        float relativeDistance = 0;
        if (player != null)
        {
            // 1. Get the vector from Boss to Player
            Vector2 toPlayer = (Vector2)player.transform.position - (Vector2)transform.position;

            // 2. PROJECT that vector onto the Boss's local 'Up' axis
            // This gives you the distance "in front" of the boss, even if he is rotated
            relativeDistance = Vector2.Dot(toPlayer, forwardDirection);
        }

        // 3. Determine speed based on distance (Logic stays the same)
        float desiredSpeed;
        if (player == null)                                  desiredSpeed = normalSpeed;
        else if (relativeDistance < slowDistance)           desiredSpeed = slowSpeed;
        else if (relativeDistance < catchUpDistance)        desiredSpeed = normalSpeed;
        else if (relativeDistance < catchUpDistance + 20f) desiredSpeed = fastSpeed;
        else                                                desiredSpeed = fastestSpeed;

        // 4. Calculate the Target Velocity (Local Direction * Speed)
        Vector2 targetVelocity = forwardDirection * desiredSpeed;

        // 5. Apply Force
        Vector2 velocityDelta = targetVelocity - rb.linearVelocity;
        rb.AddForce(velocityDelta * acceleration);
    }

    void MoveBossX()
    {
        Vector2 forwardDirection = transform.up;

        float relativeDistance = 0;
        if (player != null)
        {
            // 1. Get the vector from Boss to Player
            Vector2 toPlayer = (Vector2)player.transform.position - (Vector2)transform.position;

            // 2. PROJECT that vector onto the Boss's local 'Up' axis
            // This gives you the distance "in front" of the boss, even if he is rotated
            relativeDistance = Vector2.Dot(toPlayer, forwardDirection);
        }

        // 3. Determine speed based on distance (Logic stays the same)
        float desiredSpeed;
        if (player == null)                                  desiredSpeed = 15f;
        else if (relativeDistance < slowDistance)           desiredSpeed = 15f;
        else if (relativeDistance < catchUpDistance)        desiredSpeed = 21f;
        else if (relativeDistance < catchUpDistance + 20f) desiredSpeed = 25f;
        else                                                desiredSpeed = 30f;

        // 4. Calculate the Target Velocity (Local Direction * Speed)
        Vector2 targetVelocity = forwardDirection * desiredSpeed;

        // 5. Apply Force
        Vector2 velocityDelta = targetVelocity - rb.linearVelocity;
        rb.AddForce(velocityDelta * acceleration);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1<< other.gameObject.layer) & unbreakableLayer) != 0)
        {
            return;
        }

        if (other.gameObject == player)
        {
            //GameManager.Instance.playerManager.GivePlayerKnockBack(transform , knowckbackForce);
            GameManager.Instance.playerManager.KillPlayer();
            return;
        }

        Destroy(other.gameObject);
    }


    public void PushBack(float pushValue)
    {
        // Use -transform.up to push back exactly opposite of where the boss is facing
        rb.AddForce(-transform.up * pushValue, ForceMode2D.Impulse);
    }


    // Delete this later
    
    void DrawDebug()
    {
        // Get position of the top of the boss
        Vector2 bossOffset = transform.position;
        bossOffset.y += (bossYHeight/2);

        // Get Absolute distance value between player and boss;
        float absDistance = Mathf.Abs(player.transform.position.y - bossOffset.y);

        Color debugColor;

        if (absDistance < slowDistance)
        {
            debugColor = Color.red;
        }
        else if (absDistance < catchUpDistance)
        {
            debugColor = Color.blue;
        }
        else
        {
            debugColor = Color.green;
        }

        Debug.DrawLine(player.transform.position, bossOffset, debugColor);
    }
}
