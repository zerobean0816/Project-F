
using UnityEngine;

public class PowerBullet : MonoBehaviour
{
    public int damage {get; private set;}
    public float moveSpeed = 50f;
    public LayerMask dontDestoryLayer;

    GameObject boss;
    Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * moveSpeed;

        boss = GameManager.Instance.enemyManager.currentBoss;
    }

    public void SetDamageValue( int damage)
    {
        this.damage = damage;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.playerManager.player)
        {
            return;
        }

        if (( 1<<collision.gameObject.layer) == dontDestoryLayer)
        {
            Destroy (gameObject);
            return;
        }

        if (collision.gameObject.tag == "Boss")
        {
            Debug.Log("[PowerBullet]: Boss Hitted, Pushing Back");
            boss.GetComponent<BossMain>().PushBack(50f);
            return;
        }

        Destroy(collision.gameObject);
    }
}
