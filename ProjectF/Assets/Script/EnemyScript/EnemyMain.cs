using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyLook))]
public class EnemyMain : MonoBehaviour
{
    private EnemyLook enemyLook;
    private EnemyGun enemyGun;

    private int HP;

    void Start()
    {
        enemyLook = GetComponentInChildren<EnemyLook>();
        enemyGun = GetComponentInChildren<EnemyGun>();

        if (enemyLook == null)
        {
            Debug.Log("[EenemyMain] : enemyLook is missing in perfab, add one new");
            enemyLook = GetComponentInChildren<EnemyLook>();
        }

        HP = 5;
    }

    // Update is called once per frame
    void Update()
    {
        enemyLook.LookUpdate();

        enemyGun.GunUpdate();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.playerManager.GiveStun();
        }
    }
}
