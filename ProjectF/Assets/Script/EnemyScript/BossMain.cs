
using UnityEngine;

public class BossMain : MonoBehaviour
{
    [SerializeField] float slowSpeed = 1.5f;
    [SerializeField] float normalSpeed = 3f;
    [SerializeField] float fastSpeed = 8f;
    [SerializeField] float acceleration = 10f;

    [SerializeField] float slowDistance = 20f;
    [SerializeField] float catchUpDistance = 60f;

    [SerializeField] float knowckbackForce = 2000f;

    public LayerMask unbreakableLayer;


    private Rigidbody2D rb;
    GameObject player;


    float bossYHeight;

    void Start()
    {
        player = GameManager.Instance.playerManager.player;
        rb = gameObject.GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;

        bossYHeight = transform.localScale.y;

        
    }


    void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.up * normalSpeed;
            return;
        }

        // Get position of the top of the boss
        Vector2 bossOffset = transform.position;
        bossOffset.y += (bossYHeight/2);

        // Get Absolute distance value between player and boss;
        float absDistance = Mathf.Abs(player.transform.position.y - bossOffset.y);

        // Y direction speed value that change by distance
        float disiredYVelocity;

        if (absDistance < slowDistance)          // slow down when too close
        {
            disiredYVelocity = slowSpeed;
        }
        else if (absDistance < catchUpDistance)  // set normal if in normal range
        {
            disiredYVelocity = normalSpeed;
        }
        else                                     // speed up when too far
        {
            disiredYVelocity = fastSpeed;
        }

        //Debug.Log($"Ydesired speed: {distance}");

        float velocityDeltaY = disiredYVelocity - rb.linearVelocityY;
        rb.AddForce(Vector2.up * velocityDeltaY * acceleration);

        DrawDebug();
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
