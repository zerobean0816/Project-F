
using UnityEngine;

public class PowerBullet : MonoBehaviour
{
    public int damage {get; private set;}
    public float moveSpeed = 30f;


    Rigidbody2D rb;


    public void SetDamageValue( int damage)
    {
        this.damage = damage;
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == GameManager.Instance.playerManager.player)
        {
            return;
        }

        if (collision.CompareTag("Wall"))
        {
            return;
        }

        if (collision.CompareTag("Boss"))
        {
            
        }

        Destroy(collision.gameObject);
    }
}
