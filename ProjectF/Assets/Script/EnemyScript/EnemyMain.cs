
using UnityEngine;


public class EnemyMain : MonoBehaviour
{
    [SerializeField] float stunForce = 20f;
    [SerializeField] int HP = 3;

    private EnemyLook enemyLook;
    private EnemyGun enemyGun;
    
    private bool isType2;

    void Start()
    {
        enemyLook = GetComponentInChildren<EnemyLook>();
        enemyGun = GetComponentInChildren<EnemyGun>();

        if (enemyLook == null)
        {
            Debug.Log("[EenemyMain] : enemyLook is missing in perfab, add one new");
            enemyLook = GetComponentInChildren<EnemyLook>();
        }

        isType2 = (enemyGun == null && enemyLook == null);
        HP = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isType2)
        {
            enemyLook.LookUpdate();
            enemyGun.GunUpdate();
        }

        if (HP <= 0)
        {
            Debug.Log("[EnemyMain] : Enemy has 0 HP, Destorying..");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.playerManager.GivePlayerKnockBack(transform , stunForce);
        }
    }

    public void GiveDamage(int damage)
    {
        HP -= damage;
    }

    void OnDisable()
    {
        Debug.Log("[EnemyMain] : Destroying Conplete");
    }
}
