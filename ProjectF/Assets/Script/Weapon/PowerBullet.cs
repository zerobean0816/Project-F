
using UnityEngine;

public class PowerBullet : MonoBehaviour
{
    [SerializeField] float bulletPower = 500f;
    public int damage {get; private set;}
    public float moveSpeed = 50f;

    public LayerMask dontDestoryLayer;

    GameObject boss;
    Rigidbody2D rb;

    void Start()
    {
        // Set MoveSpeed of power bullet
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * moveSpeed;

        boss = GameManager.Instance.enemyManager.currentBoss;
        
        // Add addition buff to player
        GameManager.Instance.playerManager.shotGun.AddBullet();
        GameManager.Instance.playerManager.ResetUltValue();

        // Debug Message
        Debug.Log("[PowerBullet] : Bullet Spawned!");
    }

    public void SetDamageValue( int damage)
    {
        // Apply Damage
        this.damage = damage;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        bool isProtected = ( ( ( 1<<collision.gameObject.layer) & dontDestoryLayer) == 0 );

        if (collision.gameObject == GameManager.Instance.playerManager.player)
        {
            return;
        }

        if (!isProtected)
        {
            Destroy (gameObject);
            return;
        }

        if (collision.gameObject.tag == "Boss")
        {
            if (boss == null)
            {
                Debug.LogError("Boss is missing here");
            }
            Debug.Log("[PowerBullet]: Boss Hitted, Pushing Back");
            boss.GetComponent<BossMain>().PushBack(bulletPower);
            return;
        }

        if (collision != null)
        {
            Destroy(collision.gameObject);
        }
    }
}
