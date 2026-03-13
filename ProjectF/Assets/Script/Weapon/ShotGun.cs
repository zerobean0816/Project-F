using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Rendering;

public class ShotGun : MonoBehaviour
{   
    [SerializeField] float shootStrength = 30;
    [SerializeField] int maxAmmo = 4;

    [SerializeField] GameObject bullet1;
    [SerializeField] GameObject bullet2;
    [SerializeField] GameObject bullet3;

    public bool isPressed;
    public Vector2 KnockBackForce {get; private set;}

    GameObject player;
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
            if (_currentBullet <= 0)
            {
                isShootable = false;
            }
            else
            {
                isShootable = true;
            }
        }
    }
    private bool isShootable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.playerManager.player;
        if (player == null)
        {
            Debug.Log("[ShotGun] : Player is NULL in GmaeManger.PlayerManager");
        }

        currentBullet = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isShootable)
        {
            isPressed = true;
            currentBullet --;
            Shoot();
            GameManager.Instance.uIManager.SetBulletCount(currentBullet);
        }
        else if (!isShootable)
        {
            ReloadOnTime(.5f);
        }

        ShowBulletByCount();
    }

    void Shoot()
    {
        //Debug.Log("Shotgun Shotted!");
        if (isPressed && isShootable)
        {
            Debug.Log($"[ShootGun] : BulletCount = {currentBullet}");
            SetKnockBackForce();
        }
    }
    
    void ShowBulletByCount()
    {
        if (currentBullet == 1)
        {
            bullet1.SetActive(true);

            bullet2.SetActive(false);
            bullet3.SetActive(false);
        }
        else if (currentBullet == 2)
        {
            bullet1.SetActive(true);
            bullet2.SetActive(true);

            bullet3.SetActive(false);
        }
        else if (currentBullet == 3)
        {
            bullet1.SetActive(true);
            bullet2.SetActive(true);
            bullet3.SetActive(true);
        }

        else
        {
            bullet1.SetActive(false);
            bullet2.SetActive(false);
            bullet3.SetActive(false);
        }
    }

    private IEnumerator ReloadOnTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        currentBullet = maxAmmo;
        Debug.Log("[PlayerManager] : Stun wore off.");
    }

    void SetKnockBackForce()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePOs = GameManager.Instance.mousePos;
        Vector2 oppositeDirection = (playerPos- mousePOs).normalized;

        KnockBackForce = oppositeDirection * shootStrength;

        //Debug.Log($"Opposite Direction {oppositeDirection} | KnockBackFroce: {KnockBackForce}");
    }
}
