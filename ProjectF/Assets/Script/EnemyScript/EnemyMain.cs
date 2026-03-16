
using System.Collections;
using UnityEngine;


public class EnemyMain : MonoBehaviour
{
    [SerializeField] float stunForce = 20f;
    [SerializeField] int HP = 3;

    private EnemyLook enemyLook;
    private EnemyGun enemyGun;
    private Collider2D _collider;
    
    private bool hasNoGun;

    private bool isDead;
    private bool isType2;

    void Start()
    {
        enemyLook = GetComponent<EnemyLook>();
        enemyGun = GetComponent<EnemyGun>();
        _collider = gameObject.GetComponent<Collider2D>();

        hasNoGun = false;

        if (enemyLook == null)
        {
            Debug.Log("[EenemyMain] : enemy has No gun");
            hasNoGun = true;
        }
        else
        {
            enemyLook.LookStart();
            enemyGun.GunStart();
        }

        isDead = false;

        isType2 = (enemyGun == null && enemyLook == null);
        HP = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isType2 && !isDead && !hasNoGun)
        {
            enemyLook.LookUpdate();
            enemyGun.GunUpdate();
        }

        if (HP <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(KillEnemyAfterEffect(.3f));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.playerManager.GivePlayerKnockBack(transform , stunForce);
            GameManager.Instance.playerManager.GiveStun();
        }
    }

    public void GiveDamage(int damage)
    {
        HP -= damage;
    }

    IEnumerator KillEnemyAfterEffect(float duration)
    {
        if (_collider != null)
        {
            _collider.enabled = false;
        }

        GameManager.Instance.playerManager.shotGun.AddBullet();
        GameManager.Instance.playerManager.shotGun.AddBullet();

        GameManager.Instance.playerManager.AddUltValue(10);

        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }
}
