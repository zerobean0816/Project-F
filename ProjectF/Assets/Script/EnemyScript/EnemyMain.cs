using UnityEngine;

[RequireComponent(typeof(EnemyLook))]
public class EnemyMain : MonoBehaviour
{
    private EnemyLook enemyLook;
    private EnemyGun enemyGun;

    void Start()
    {
        enemyLook = GetComponentInChildren<EnemyLook>();
        enemyGun = GetComponentInChildren<EnemyGun>();

        if (enemyLook == null)
        {
            Debug.Log("[EenemyMain] : enemyLook is missing in perfab, add one new");
            enemyLook = GetComponentInChildren<EnemyLook>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemyLook.LookUpdate();

        enemyGun.GunUpdate();
    }
}
