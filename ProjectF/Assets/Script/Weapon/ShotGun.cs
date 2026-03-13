using System.Collections;

using UnityEngine;


public class ShotGun : MonoBehaviour
{   
    [SerializeField] float shootStrength = 30;
    [SerializeField] int maxAmmo = 3;

    [SerializeField] GameObject bullet1;
    [SerializeField] GameObject bullet2;
    [SerializeField] GameObject bullet3;


    GameObject player;
    public Vector2 KnockBackForce {get; private set;}
    public bool isPressed;
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
                isPressed = false;
            }
            else
            {
                isPressed = true;
            }
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
    void GunUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !isReloading && currentBullet > 0)
        {
            SetKnockBackForce();
            isPressed = true;
            currentBullet --;
            GameManager.Instance.uIManager.SetBulletCount(currentBullet);
        }

        if (currentBullet <= 0)
        {
            StartCoroutine(ReloadOnTime(0.5f));
        }

        ShowBulletByCount();
    }

    
    void ShowBulletByCount()
    {
        bullet1.SetActive(currentBullet >= 1);
        bullet2.SetActive(currentBullet >= 2);
        bullet3.SetActive(currentBullet >= 3);
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
}
