
using UnityEngine;

public class BossMain : MonoBehaviour
{
    GameObject player;
    Vector2 moveVector;

    float basicYSpeed = 2.5f;

    void Start()
    {
        moveVector = transform.position;
        player = GameManager.Instance.playerManager.player;
    }

    void Update()
    {
        if (Mathf.Abs(player.transform.position.y - transform.position.y) < 40f)
        {
            moveVector.y += (basicYSpeed * Time.deltaTime);
        }
        else
        {
            moveVector.y += (basicYSpeed * Time.deltaTime * 2f);
        }

        transform.position = moveVector;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}
