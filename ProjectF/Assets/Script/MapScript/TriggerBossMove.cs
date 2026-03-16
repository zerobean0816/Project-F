using UnityEngine;

public class TriggerBossMove : MonoBehaviour
{
    public bool isXaxis = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            BossMain boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossMain>();
            boss.isMoving = true;
            boss.isX = isXaxis;
        }   
    }
}
