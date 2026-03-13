
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 15f;
    private PlayerManager playerManager;
    private Rigidbody2D rb;

    //[SerializeField] LayerMask passableLayer;

    void Start()
    {
        playerManager = GameManager.Instance.playerManager;
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * bulletSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject ==playerManager.player)
        {
            playerManager.GiveStun();
        }

        if (collision.gameObject.tag != "Passable")
        {
            Destroy(gameObject);
        }
        
    }
}
