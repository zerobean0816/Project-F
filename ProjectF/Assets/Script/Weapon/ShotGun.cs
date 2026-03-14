using System.Collections;
using UnityEngine;
using UnityEngine.Windows.WebCam;


public class ShotGun : MonoBehaviour,IKnockbackSource
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
    [SerializeField] public Transform firePoint;

    GameObject player;
    Rigidbody2D playerRb;
    public Vector2 knockBackForce {get; private set;}
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
    public bool isReloading {get; private set;}

    public float reloadTimer {get; private set;}

    public bool IsRequesting => isPressed;
    public Vector2 GetForce() => SetKnockBackForce();
    public void Consume() => FinishePress();


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
    }

    // Update is called once per frame
    public void ReloadWhenEmpty()
    {
        if (currentBullet < maxAmmo && player.GetComponent<PlayerController>().isGrounded)
        {
            StartCoroutine(ReloadOnTime(reloadTime));
        }
    }

    public void FireGun()
    {
        isPressed = true;
        currentBullet --;

        SpawnBulletOnRotation();
    }
    
    public void ShowBulletByCount()
    {
        bullet1UI.SetActive(currentBullet >= 1);
        bullet2UI.SetActive(currentBullet >= 2);
        bullet3UI.SetActive(currentBullet >= 3);
    }

    private IEnumerator ReloadOnTime(float duration)
    {
        if (isReloading) yield break;
        isReloading = true;

        reloadTimer = 0;
        while (reloadTimer < duration)
        {
            if (!player.GetComponent<PlayerController>().isGrounded)
            {
                isReloading = false;
                yield break;
            }

            reloadTimer += Time.deltaTime;
            yield return null;
        }

        currentBullet ++;
        isReloading = false;
    }

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
        if ( playerRb.linearVelocityY < 0f && offset.y > .1f)
        {
            float bonus = Mathf.Abs(playerRb.linearVelocityY) * 0.07f;
            finalVector.y += bonus;
        }
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
