using System.Collections;
using UnityEngine;

public class ShotGun : MonoBehaviour,IKnockbackSource
{   
    [Header("Bullet Stats")]
    [SerializeField] float shootStrength = 30f;
    [SerializeField] int maxAmmo = 3;
    [SerializeField] float reloadTime = 1f;
    [SerializeField] int bulletDamage = 1;
    [SerializeField] float fireDegree = 30f;
    [SerializeField] int bulletsPerShoot = 5;

    [Header("Bullet State UI")]
    [SerializeField] GameObject bullet1UI;
    [SerializeField] GameObject bullet2UI;
    [SerializeField] GameObject bullet3UI;
    [SerializeField] GameObject bullet4UI;

    [Header("Perfab")]
    [SerializeField] GameObject bulletPerFab;
    [SerializeField] public Transform firePoint;

    // Player Info
    GameObject player;
    Rigidbody2D playerRb;


    public Vector2 knockBackForce {get; private set;}
    public bool isPressed {get; private set;}
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
    public bool isReloading {get; private set;}
    public float reloadTimer {get; private set;}

    // Connect IknockbackSource;
    public bool IsRequesting => isPressed;
    public Vector2 GetForce() => SetKnockBackForce();
    public void Consume() => FinishePress();

    private int _currentBullet;
    private float halfAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.playerManager.player;
        playerRb = player.GetComponent<Rigidbody2D>();

        if (player == null)
        {
            Debug.Log("[ShotGun] : Player is NULL in GmaeManger.PlayerManager");
        }
        isReloading = false;
        currentBullet = maxAmmo;
        halfAngle = fireDegree / 2;
    }

    #region  Gun Control
    // Update is called once per frame
    public void ReloadWhenEmpty()
    {
        if (currentBullet < maxAmmo )
        {
            StartCoroutine(ReloadOnTime(reloadTime));
        }

        // if (currentBullet < maxAmmo && player.GetComponent<PlayerController>().isGrounded)
        // {
        //     StartCoroutine(ReloadOnTime(reloadTime));
        // }
    }
    
    public void FireGun()
    {
        isPressed = true;
        currentBullet --;

        BrustBulletWithinAngle();
    }

    void BrustBulletWithinAngle()
    {
        for (int i = 0 ; i < bulletsPerShoot; i++)
        {
            float randomValue = Random.Range(-halfAngle, halfAngle);
            SpawnBulletOnRotation(randomValue);
        }
    }

    void SpawnBulletOnRotation(float randomAngle)
    {
        Quaternion spwnRotate =  Quaternion.Euler(0,0, (randomAngle + transform.eulerAngles.z ));

        GameObject bullet = Instantiate(bulletPerFab, firePoint.position, spwnRotate);
        bullet.GetComponent<ShotgunBullet>().SetDamageValue(bulletDamage);
    }
    
    public void ShowBulletByCount()
    {
        bullet1UI.SetActive(currentBullet >= 1);
        bullet2UI.SetActive(currentBullet >= 2);
        bullet3UI.SetActive(currentBullet >= 3);
        bullet4UI.SetActive(currentBullet >= 4);
    }

    private IEnumerator ReloadOnTime(float duration)
    {
        if (isReloading) yield break;
        isReloading = true;

        reloadTimer = 0;
        while (reloadTimer < duration)
        {
            // if (!player.GetComponent<PlayerController>().isGrounded)
            // {
                //isReloading = false;
                //yield break;
            //}

            reloadTimer += Time.deltaTime;
            yield return null;
        }

        AddBullet();
        isReloading = false;
    }

    public void AddBullet()
    {
        currentBullet ++;
    }

    #endregion

    #region Shotgun knockBack
    Vector2 SetKnockBackForce()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePOs = GameManager.Instance.mousePos;
        Vector2 oppositeDirection = (playerPos- mousePOs).normalized;

        SetRecoilOnYSpeed(oppositeDirection , out Vector2 finalVector);
        //Debug.Log($"Opposite Direction {oppositeDirection} | KnockBackFroce: {oppositeDirection * shootStrength}");

        return finalVector * shootStrength;
    }

    void SetRecoilOnYSpeed(Vector2 offset , out Vector2 finalVector)
    {
        finalVector = offset;
        float bonusYForce = 0f;

        if ( offset.y > .1f)
        {
            bonusYForce += GetForceBonusByYSpeed(-10f, -80f, 0.08f );
            bonusYForce += GetFixedByYSpeed(10f, -10f, 1f );
        }
        finalVector.y += bonusYForce;
    }

    float GetForceBonusByYSpeed(float min, float max, float bonusForce)
    {
        float bonus = 0f;

        if ( playerRb.linearVelocityY < min && playerRb.linearVelocityY >= max )
        {
            bonus = Mathf.Abs(playerRb.linearVelocityY) * bonusForce;
        }

        return bonus;
    }

    float GetFixedByYSpeed(float min, float max, float bonusForce)
    {
        float bonus = 0f;

        if ( playerRb.linearVelocityY < min && playerRb.linearVelocityY >= max )
        {
            bonus = bonusForce;
        }

        return bonus;
    }


    public void FinishePress()
    {
        isPressed = false;
    }
    #endregion

}
