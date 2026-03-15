
using UnityEngine;

public class WallKnockback : MonoBehaviour
{
    [SerializeField] float knockbackForce = 500f;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("[WallKnockback] : give player knockback");
            GameManager.Instance.playerManager.GivePlayerKnockBack(transform, knockbackForce);
        }
    }
}
