
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
            MoveBoss();
        }
    }

    void MoveBoss()
    {
    // 1. Get the direction the boss is currently facing (its "Up" direction)
        Vector2 forwardDirection = transform.up;

        // 2. Calculate distance logic (keep your player height check)
        float absDistance = 0;
        if (player != null)
        {
            Vector2 bossOffset = (Vector2)transform.position + ((Vector2)transform.up * (bossYHeight / 2));
            absDistance = Mathf.Abs(player.transform.position.y - bossOffset.y);
        }

        // 3. Determine speed based on distance (Rubber-banding)
        float desiredSpeed;
        if (player == null)                     desiredSpeed = normalSpeed;
        else if (absDistance < slowDistance)    desiredSpeed = slowSpeed;
        else if (absDistance < catchUpDistance) desiredSpeed = normalSpeed;
        else if (absDistance < catchUpDistance + 20f) desiredSpeed = fastSpeed;
        else                                    desiredSpeed = fastestSpeed;

        // 4. Calculate the Target Velocity
        // This multiplies the direction the boss is pointing by the speed
        Vector2 targetVelocity = forwardDirection * desiredSpeed;

        // 5. Apply Force to reach that velocity
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
        //Debug.Log("Pushing Boss Back");
        rb.linearVelocityY *= 0.5f;
        rb.AddForce(Vector2.down * pushValue, ForceMode2D.Impulse);
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
