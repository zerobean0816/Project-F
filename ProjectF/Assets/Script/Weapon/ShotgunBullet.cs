using System;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    [SerializeField]float bulletSpeed = 50f;
    PlayerManager playerManager;
    GameObject shotgun;
    EnemyMain otherMain;
    Rigidbody2D rb;

    int damage = 1;

    void Start()
    {
        playerManager = GameManager.Instance.playerManager;
        shotgun = playerManager.shotGun.GetComponent<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"[ShotgunBullet] : Hitted Something {collision.gameObject.name}");



        if (!collision.gameObject == shotgun)
        {
            ApplyDamageIfEnemyCollide(collision.gameObject);
            Destroy(gameObject);
            return;
        }

        //Debug.Log("[ShotgunBullet] : Has hitted playerGun");
    }

    void ApplyDamageIfEnemyCollide(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("[ShotgunBullet] : Enemy Hitted!");
            otherMain = other.GetComponent<EnemyMain>();
            otherMain.GiveDamage(damage);
        }
    }

    public void SetDamageValue(int damage)
    {
        this.damage = damage;
    }
}
