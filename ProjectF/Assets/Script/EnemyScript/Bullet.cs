using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 15f;
    private PlayerManager playerManager;
    private Rigidbody2D rb;
    public GameObject owner {get; private set;}

    //[SerializeField] LayerMask passableLayer;

    void Start()
    {
        playerManager = GameManager.Instance.playerManager;
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("[Bullet] : Hitted Player! Destorying myself");
            playerManager.GiveStun();
            Destroy(gameObject);
            return;
        }

        else if ( collision.CompareTag("Passable") || owner == collision.gameObject)
        {
            return;
        }

        else if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyMain>().GiveDamage(1);
        }
        
        Destroy(gameObject);
    }

    public void SetSpowner(GameObject owner)
    {
        this.owner= owner;
    }
}
