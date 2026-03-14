
using System.Collections;
using UnityEngine;


public class EnemyMain : MonoBehaviour
{
    [SerializeField] float stunForce = 20f;
    [SerializeField] int HP = 3;

    private EnemyLook enemyLook;
    private EnemyGun enemyGun;
    private Collider2D _collider;


    private bool isDead;
    private bool isType2;

    void Start()
    {
        enemyLook = GetComponentInChildren<EnemyLook>();
        enemyGun = GetComponentInChildren<EnemyGun>();
        _collider = gameObject.GetComponentInChildren<Collider2D>();

        if (enemyLook == null)
        {
            Debug.Log("[EenemyMain] : enemyLook is missing in perfab, add one new");
            enemyLook = GetComponentInChildren<EnemyLook>();
        }

        isDead = false;

        isType2 = (enemyGun == null && enemyLook == null);
        HP = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isType2 && !isDead)
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
        GameManager.Instance.playerManager.AddUltValue(10);

        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }
}
