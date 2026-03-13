
using UnityEngine;

[RequireComponent(typeof(EnemyLook))]
public class EnemyGun : MonoBehaviour
{
    [SerializeField] public GameObject gun;
    [SerializeField] public GameObject firePivit;
    [SerializeField] float fireRate = 2f;

    [SerializeField] GameObject bullet;

    private EnemyLook enemyLook;
    private float currentTime;
    public bool isIdle {get; private set;}
    GameObject spawnedBullet;

    void Start()
    {
        enemyLook = GetComponent<EnemyLook>();
        if (enemyLook == null)
        {
            Debug.LogError("[EnemyGun] : enemyLook is missing in gameobject");
        }

        if (bullet == null)
        {
            Debug.LogError("[EenmyGun] : Bullet is not setted to EnemyGun");
        }

        isIdle = true;
        currentTime = 0f;
    }

    public void GunUpdate()
    {
        if (enemyLook.hasfoundPlayer)
        {
            SpawnBulletOnRate();
        }
    }

    void SpawnBulletOnRate()
    {
        if (currentTime <= 0)
        {
            Debug.Log("EnemyGun : Spawning Bullet");
            Shoot();
            currentTime = fireRate;
        }
        currentTime -= Time.deltaTime;
    }

    void Shoot()
    {
        //Debug.Log(gun.transform.rotation);
        spawnedBullet = Instantiate(bullet, firePivit.transform.position , gun.transform.rotation);
    }
}
