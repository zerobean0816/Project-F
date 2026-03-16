using UnityEngine;

public class TriggerBossMove : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            BossMain boss = GameObject.Find("Boss").GetComponent<BossMain>();
            boss.isMoving = true;
        }   
    }
}
