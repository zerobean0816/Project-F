using System.Collections;

using UnityEngine;
using UnityEngine.Rendering.Universal;


public class ShotGun : MonoBehaviour
{   
    [Header("Bullet Stats")]
    [SerializeField] float shootStrength = 30;
    [SerializeField] int maxAmmo = 3;
    [SerializeField] float reloadTime = 1f;
    [SerializeField] int bulletDamage = 1;

    [Header("Bullet State UI")]
    [SerializeField] GameObject bullet1UI;
    [SerializeField] GameObject bullet2UI;
    [SerializeField] GameObject bullet3UI;

    [Header("Perfab")]
    [SerializeField] GameObject bulletPerFab;
    [SerializeField] Transform firePoint;

    GameObject player;
    public Vector2 KnockBackForce {get; private set;}
    public bool isPressed {get; private set;}
    private int _currentBullet;
    public int currentBullet
    {
        get
        {
            return _currentBullet;
        }

        private set
        {
            _currentBullet = Mathf.Clamp(value, 0, maxAmmo);
        }
    }
    private bool isReloading;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.playerManager.player;
        if (player == null)
        {
            Debug.Log("[ShotGun] : Player is NULL in GmaeManger.PlayerManager");
        }
        isReloading = false;
        currentBullet = maxAmmo;
    }

    // Update is called once per frame
    public void GunUpdate()
    {
        FireGun();

        if (currentBullet <= 0)
        {
            StartCoroutine(ReloadOnTime(reloadTime));
        }

        ShowBulletByCount();
    }

    void FireGun()
    {
        if (Input.GetMouseButtonDown(0) && !isReloading && currentBullet > 0)
        {
            SetKnockBackForce();
            isPressed = true;
            currentBullet --;
            //GameManager.Instance.uIManager.SetBulletCount(currentBullet);

            SpawnBulletOnRotation();
        }
    }
    
    void ShowBulletByCount()
    {
        bullet1UI.SetActive(currentBullet >= 1);
        bullet2UI.SetActive(currentBullet >= 2);
        bullet3UI.SetActive(currentBullet >= 3);
    }

    private IEnumerator ReloadOnTime(float duration)
    {
        if (isReloading) yield break;

        isReloading = true;
        Debug.Log("[PlayerManager] : Reloading...");

        yield return new WaitForSeconds(duration);

        currentBullet = maxAmmo;

        isReloading = false;
        Debug.Log("[PlayerManager] : Reload Complete.");
    }

    void SetKnockBackForce()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePOs = GameManager.Instance.mousePos;
        Vector2 oppositeDirection = (playerPos- mousePOs).normalized;

        KnockBackForce = oppositeDirection * shootStrength;

        //Debug.Log($"Opposite Direction {oppositeDirection} | KnockBackFroce: {KnockBackForce}");
    }

    public void FinishePress()
    {
        isPressed = false;
    }

    void SpawnBulletOnRotation()
    {
        //Debug.Log("[Shotgun] : Spawn Bullet!");
        GameObject bullet = Instantiate(bulletPerFab, firePoint.position, transform.rotation);
        bullet.GetComponent<ShotgunBullet>().SetDamageValue(bulletDamage);
    }
}
