using UnityEngine;

public class EnemyType2 : MonoBehaviour
{
    [SerializeField] float knockBackForce = 300f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.playerManager.GivePlayerKnockBack( transform, knockBackForce);
        }
    }
}
